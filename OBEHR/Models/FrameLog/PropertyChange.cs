using FrameLog.Models;

namespace OBEHR.Models.FrameLog
{
    public class PropertyChange : IPropertyChange<PPUser>
    {
        public int Id { get; set; }
        public virtual ObjectChange ObjectChange { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
        public int? ValueAsInt { get; set; }

        IObjectChange<PPUser> IPropertyChange<PPUser>.ObjectChange 
        { 
            get { return ObjectChange; }
            set { ObjectChange = (ObjectChange)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", PropertyName, Value);
        }
    }
}
