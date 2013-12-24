using OBEHR.Models.DAL;
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
    public class CitySupplierBaseModel : BaseModel
    {
        [DisplayName("城市")]
        public int CityId { get; set; }

        [DisplayName("供应商")]
        public int SupplierId { get; set; }

        public virtual City City { get; set; }
        public virtual Supplier Supplier { get; set; }

        public override string ToString()
        {
            if (Name == "无")
            {
                var tmpStr = "";
                if (City == null)
                {
                    using (var db = new UnitOfWork())
                    {
                        var cityName = db.CityRepository.GetByID(CityId).ToString();
                        var SupplierName = db.SupplierRepository.GetByID(SupplierId).ToString();

                        tmpStr = cityName + "_" + SupplierName;
                    }
                }
                else
                {
                    tmpStr = City.ToString() + "_" + Supplier.ToString();
                }
                return tmpStr;
            }
            else
            {
                return Name;
            }
        }
    }
}