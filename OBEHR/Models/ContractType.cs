using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class ContractType : ClientBaseModel, IEditable<ContractType>
    {
        public override string ToString()
        {
            return Name;
        }

        public void Edit(ContractType model)
        {
            Name = model.Name;
        }
    }
}