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
    public class PensionTaxFree : CitySupplierBaseModel, IEditable<PensionTaxFree>
    {
        public PensionTaxFree()
        {
            Name = "无";
        }

        [DisplayName("免税金额")]
        public decimal FreeValue { get; set; }
        [DisplayName("免税基数")]
        public decimal FreeBase { get; set; }
        [DisplayName("免税比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal FreePercent { get; set; }

        public void Edit(PensionTaxFree model)
        {
            Name = model.Name;
            FreeValue = model.FreeValue;
            FreeBase = model.FreeBase;
            FreePercent = model.FreePercent;
        }
    }
}