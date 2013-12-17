using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class Assurance : ClientBaseModel, IEditable<Assurance>
    {
        public override string ToString()
        {
            return Name;
        }

        public void Edit(Assurance model)
        {
            Name = model.Name;
        }
    }
}