using OBEHR.Models.Base;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class City : SoftDelete, IEditable<City>
    {
        public void Edit(City model)
        {
            Name = model.Name;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}