using System;
using System.Data;
using FrameLog.Models;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;

namespace FrameLog.Contexts
{
    public interface IFrameLogContext<TChangeSet, TPrincipal> : IHistoryContext<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        int SaveChanges(SaveOptions options);
        ObjectStateManager ObjectStateManager { get; }
        void AcceptAllChanges();

        object GetObjectByKey(EntityKey key);
        void AddChangeSet(TChangeSet changeSet);

        Type UnderlyingContextType { get; }
        MetadataWorkspace Workspace { get; }
    }
}
