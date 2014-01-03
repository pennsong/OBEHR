using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class Position : ClientBaseModel, IEditable<Position>
    {
        public void Edit(Position model)
        {
            Name = model.Name;
        }
    }
}