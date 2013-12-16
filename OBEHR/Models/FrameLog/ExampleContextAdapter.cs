using System;
using System.Linq;
using FrameLog.Contexts;
using FrameLog.Models;
using OBEHR.Models.DAL;

namespace OBEHR.Models.FrameLog
{
    public class ExampleContextAdapter : DbContextAdapter<ChangeSet, PPUser>
    {
        private OBEHRContext context;

        public ExampleContextAdapter(OBEHRContext context)
            : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<PPUser>> ChangeSets
        {
            get { return context.ChangeSets; }
        }
        public override IQueryable<IObjectChange<PPUser>> ObjectChanges
        {
            get { return context.ObjectChanges; }
        }
        public override IQueryable<IPropertyChange<PPUser>> PropertyChanges
        {
            get { return context.PropertyChanges; }
        }
        public override void AddChangeSet(ChangeSet changeSet)
        {
            context.ChangeSets.Add(changeSet);
        }

        public override Type UnderlyingContextType
        {
            get { return typeof(OBEHRContext); }
        }
    }
}
