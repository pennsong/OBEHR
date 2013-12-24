using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class Department : ClientBaseModel, IEditable<Department>
    {
        public void Edit(Department model)
        {
            Name = model.Name;
        }
    }
}