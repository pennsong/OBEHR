using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FrameLog.Contexts;
using FrameLog.Helpers;
using FrameLog.Models;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;

namespace FrameLog.Filter
{
    /// <summary>
    /// This class determines whether or not something should be logged, as defined
    /// by the ShouldLog overloads.
    /// As some of this work is fairly expensive, the class is designed around not
    /// duplicating the work, by two mechanisms:
    /// 1. Caching
    /// 2. Making sure that only a single instance of the class is responsible for
    ///    providing this information per database
    /// </summary>
    internal class LoggingFilter
    {
        private MetadataWorkspace workspace;
        private ConcurrentDictionary<string, Type> typeLookup;
        private ConcurrentDictionary<Signature, bool> cache;
        
        /// <summary>
        /// Constructs a new LoggingFilter.
        /// We need a reference to the MetadataWorkspace for translating between
        /// the information we get from the ChangeTracker into actual concrete types
        /// that we can then pull annotations from.
        /// </summary>
        private LoggingFilter(MetadataWorkspace workspace)
        {
            this.workspace = workspace;
            cache = new ConcurrentDictionary<Signature, bool>();
            buildTypeLookup();
        }

        #region build type lookup
        /// <summary>
        /// We need to be able to translate from the namespace-qualified but *not*
        /// asssembly-qualified names that the MetadataWorkspace gives us. To this
        /// end, we build an internal lookup of all the types in all assemblies at
        /// the point when the logging filter is constructed.
        /// </summary>
        private void buildTypeLookup()
        {
            // We store errors and don't throw them. If and when a type is requested
            // that we don't have for some reason we will dust these errors off and
            // attach them to the exception that gets thrown.
            this.typeLoadErrors = new List<ReflectionTypeLoadException>();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(getTypes)
                .Distinct();
            typeLookup = new ConcurrentDictionary<string, Type>();
            foreach (var type in types)
            {
                // If there are two types with the same namespace-qualified name,
                // only one of them will end up in the lookup. This is potentially
                // a problem.
                typeLookup[type.FullName] = type;
            }
        }
        /// <summary>
        /// Gets the types and stores the errors for later. See the comment in 
        /// buildTypeLookup()
        /// </summary>
        private IEnumerable<Type> getTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                typeLoadErrors.Add(e);
                return new List<Type>();
            }
        }
        private List<ReflectionTypeLoadException> typeLoadErrors;
        #endregion

        /// <summary>
        /// True if the given class should be logged
        /// </summary>
        internal bool ShouldLog(Type type)
        {
            return withCache(new Signature(type), () =>
            {
                return shouldLog(type);
            });
        }
        /// <summary>
        /// True if the given navigation property should be logged
        /// </summary>
        internal bool ShouldLog(NavigationProperty property)
        {
            return withCache(new Signature(property), () =>
            {
                var info = getPropertyInfo(property);
                return shouldLog(info);
            });
        }
        /// <summary>
        /// True if the given scalar property should be logged (as identified
        /// by the object it belongs to, and its name as a string)
        /// </summary>
        internal bool ShouldLog(ObjectStateEntry entry, string propertyName)
        {
            return withCache(new Signature(entry.Entity.GetType(), propertyName), () =>
            {
                var info = getPropertyInfo(entry.Entity.GetType(), propertyName);
                return shouldLog(info);
            });
        }

        private bool shouldLog(PropertyInfo property)
        {
            return shouldLog(property.GetAttributes<IFilterAttribute>());            
        }
        private bool shouldLog(Type type)
        {
            return shouldLog(type.GetAttributes<IFilterAttribute>());
        }
        private bool shouldLog(IEnumerable<IFilterAttribute> filters)
        {
            return filters.All(f => f.ShouldLog());
        }      

        /// <summary>
        /// For a given task identified by a Signature either returns 
        /// the cached result of previously running it, or runs it 
        /// and caches it for later.
        /// </summary>
        private bool withCache(Signature signature, Func<bool> task)
        {
            bool result;
            if (!cache.TryGetValue(signature, out result))
            {
                result = task();
                cache[signature] = result;                     
            }
            return result;
        }

        /// <summary>
        /// Get from an EntityFramework conceptual model NavigationProperty
        /// to an actual reflection PropertyInfo.
        /// </summary>
        private PropertyInfo getPropertyInfo(NavigationProperty property)
        {
            var objectSpaceType = workspace.GetObjectSpaceType(property.DeclaringType);
            Type type;
            if (!typeLookup.TryGetValue(objectSpaceType.FullName, out type))
            {
                // For some reason the type wasn't in the typeLookup we built when we
                // constructed this class. If we encountered any errors building that lookup
                // they will be in the typeLoadErrors collection we pass to the exception.
                throw new UnknownTypeException(objectSpaceType.FullName, typeLoadErrors);
            }
            return getPropertyInfo(type, property.Name);
        }
        private static PropertyInfo getPropertyInfo(Type type, string name)
        {
            return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        /// <summary>
        /// This provides a unique key for identifying tasks and their results
        /// in a cache. It is just an ordered collection of arbitrary objects.
        /// </summary>
        private class Signature
        {
            private object[] data;

            public Signature(params object[] data)
            {
                this.data = data;
            }

            public override bool Equals(object obj)
            {
                var other = obj as Signature;
                return other != null && equals(this, other);
            }
            private static bool equals(Signature a, Signature b)
            {
                int i = 0;
                while (i < a.data.Length)
                {
                    if (i >= b.data.Length)
                        return false;
                    if (!object.Equals(a.data[i], b.data[i]))
                        return false;
                    i++;
                }
                if (i < b.data.Length)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return string.Join(",", data.Select(d => d.GetHashCode())).GetHashCode();
            }
            public override string ToString()
            {
                return string.Join(", ", data);
            }
        }

        private static Dictionary<Type, LoggingFilter> filters = new Dictionary<Type, LoggingFilter>();

        /// <summary>
        /// Gets (or constructs) the LoggingFilter for the database in question.
        /// A typical application will only have one LoggingFilter for its lifetime,
        /// but it is possible to be running FrameLog on multiple databases simultaneously.
        /// We use the IFrameLogContext.UnderlyingType to create one LoggingFilter per
        /// database-type.
        /// </summary>
        internal static LoggingFilter Get<TChangeSet, TPrincipal>(IFrameLogContext<TChangeSet, TPrincipal> context)
            where TChangeSet : IChangeSet<TPrincipal>
        {
            var id = context.UnderlyingContextType;
            lock (filters)
            {
                LoggingFilter filter;
                if (!filters.TryGetValue(id, out filter))
                {
                    filter = filters[id] = new LoggingFilter(context.Workspace);
                }
                return filter;
            }
        }
    }


}
