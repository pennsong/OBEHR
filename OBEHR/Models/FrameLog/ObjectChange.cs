using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameLog.Models;

namespace OBEHR.Models.FrameLog
{
    public class ObjectChange : IObjectChange<PPUser>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string ObjectReference { get; set; }
        public virtual ChangeSet ChangeSet { get; set; }
        public virtual List<PropertyChange> PropertyChanges { get; set; }

        IEnumerable<IPropertyChange<PPUser>> IObjectChange<PPUser>.PropertyChanges
        {
            get { return PropertyChanges; }
        }
        void IObjectChange<PPUser>.Add(IPropertyChange<PPUser> propertyChange)
        {
            PropertyChanges.Add((PropertyChange)propertyChange);
        }
        IChangeSet<PPUser> IObjectChange<PPUser>.ChangeSet
        {
            get { return ChangeSet; }
            set { ChangeSet = (ChangeSet)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", TypeName, ObjectReference);
        }
    }
}
