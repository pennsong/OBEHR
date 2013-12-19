using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class EnterDocument : ClientCityBaseModel, IEditable<EnterDocument>
    {
        public override string ToString()
        {
            return Name;
        }

        public void Edit(EnterDocument model)
        {
            Name = model.Name;
        }
    }
}