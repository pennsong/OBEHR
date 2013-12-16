using System;
using System.Data;
using System.Transactions;
using FrameLog.Contexts;
using FrameLog.Logging;
using FrameLog.Models;
using FrameLog.Filter;
using System.Data.Entity.Core.Objects;

namespace FrameLog
{
    public class FrameLogModule<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        public bool Enabled { get; set; }
        private IChangeSetFactory<TChangeSet, TPrincipal> factory;
        private IFrameLogContext<TChangeSet, TPrincipal> context;        
        private LoggingFilter filter;

        public FrameLogModule(IChangeSetFactory<TChangeSet, TPrincipal> factory, 
            IFrameLogContext<TChangeSet, TPrincipal> context)
        {
            this.factory = factory;
            this.context = context;
            this.filter = LoggingFilter.Get(context);
            Enabled = true;
        }

        public int SaveChanges(TPrincipal principal)
        {
            return SaveChanges(principal, new SaveOptions());
        }
        public int SaveChanges(TPrincipal principal, SaveOptions options)
        {
            if (!Enabled)
                return context.SaveChanges(options);

            int result = 0;
            // We want to split saving and logging into two steps, so that when we
            // generate the log objects the database has already assigned IDs to new
            // objects. Then we can log about them meaningfully. So we wrap it in a
            // transaction so that even though there are two saves, the change is still
            // atomic.
            using (var scope = new TransactionScope())
            {
                // First we save all the changes, but we do not accept the changes 
                // (i.e. we keep our record of them).
                result = context.SaveChanges(SaveOptions.DetectChangesBeforeSave);

                // Then, based on that record of changes we create log objects for 
                // them and then accept the first set of changes.
                logChanges(principal);

                // Then we save the changes that result from creating the log objects, 
                // and accept this second set of changes.
                context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave
                    | SaveOptions.DetectChangesBeforeSave);

                scope.Complete();
            }
            return result;
        }

        private void logChanges(TPrincipal principal)
        {
            var logger = new ChangeLogger<TChangeSet, TPrincipal>(context, factory, filter);

            // This returns the log objects, but they are not attached to the context
            // so the context change tracker hasn't noticed them
            var oven = logger.Log(context.ObjectStateManager);

            // So when we accept changes, we are only accepting the changes from the
            // original changes - the context hasn't yet detected the log changes
            context.AcceptAllChanges();

            // This code then attaches the log objects to the context
            if (oven.HasChangeSet)
            {
                // First do any deferred log value calculations. 
                // See PropertyChange.Bake for more information
                TChangeSet changeSet = oven.Bake(DateTime.Now, principal);
                context.AddChangeSet(changeSet);
            }
        }
    }
}