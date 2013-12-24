using OBEHR.Models.Interfaces;
using OBEHR.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OBEHR.Models.Base
{
    public class CitySupplierHukouBaseModel : BaseModel
    {
        [DisplayName("城市")]
        public int CityId { get; set; }

        [DisplayName("供应商")]
        public int SupplierId { get; set; }

        [DisplayName("户口")]
        public HukouType HukouType { get; set; }

        public virtual City City { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}