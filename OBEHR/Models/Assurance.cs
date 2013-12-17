using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class Assurance : ClientBaseModel
    {
        public override string ToString()
        {
            return Name;
        }
    }
}