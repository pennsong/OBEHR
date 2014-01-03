using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class BudgetCenter : ClientBaseModel, IEditable<BudgetCenter>
    {
        public void Edit(BudgetCenter model)
        {
            Name = model.Name;
        }
    }
}