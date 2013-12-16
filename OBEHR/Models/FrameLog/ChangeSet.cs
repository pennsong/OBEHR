using System;
using System.Collections.Generic;
using FrameLog.Models;

namespace OBEHR.Models.FrameLog
{
    public class ChangeSet : IChangeSet<PPUser>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual PPUser Author { get; set; }
        public virtual List<ObjectChange> ObjectChanges { get; set; }

        IEnumerable<IObjectChange<PPUser>> IChangeSet<PPUser>.ObjectChanges
        {
            get { return ObjectChanges; }
        }

        void IChangeSet<PPUser>.Add(IObjectChange<PPUser> objectChange)
        {
            ObjectChanges.Add((ObjectChange)objectChange);
        }

        public override string ToString()
        {
            return string.Format("By {0} on {1}, with {2} ObjectChanges",
                Author, Timestamp, ObjectChanges.Count);
        }
    }
}
