using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models.ViewModels
{
    public class FieldHistory
    {
        public string UserName { get; set; }
        public DateTime Time { get; set; }
        public string Value { get; set; }
    }
}