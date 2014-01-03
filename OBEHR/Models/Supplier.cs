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
    public class Supplier : BaseModel, IEditable<Supplier>
    {
        [DisplayName("社保")]
        public bool IsPension { get; set; }
        [DisplayName("公积金")]
        public bool IsAccumulation { get; set; }

        public void Edit(Supplier model)
        {
            Name = model.Name;
            IsPension = model.IsPension;
            IsAccumulation = model.IsAccumulation;
        }
    }
}