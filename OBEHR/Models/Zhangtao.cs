using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class Zhangtao : ClientBaseModel, IEditable<Zhangtao>
    {
        public void Edit(Zhangtao model)
        {
            Name = model.Name;
        }
    }
}