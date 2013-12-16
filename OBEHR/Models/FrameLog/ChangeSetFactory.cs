using FrameLog.Models;
using System.Collections.Generic;

namespace OBEHR.Models.FrameLog
{
    public class ChangeSetFactory : IChangeSetFactory<ChangeSet, PPUser>
    {
        public ChangeSet ChangeSet()
        {
            var set = new ChangeSet();
            set.ObjectChanges = new List<ObjectChange>();
            return set;
        }

        public IObjectChange<PPUser> ObjectChange()
        {
            var o = new ObjectChange();
            o.PropertyChanges = new List<PropertyChange>();
            return o;
        }

        public IPropertyChange<PPUser> PropertyChange()
        {
            return new PropertyChange();
        }
    }
}
