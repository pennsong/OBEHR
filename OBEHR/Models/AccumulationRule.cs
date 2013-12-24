using OBEHR.Models.Base;
using OBEHR.Models.DAL;
using OBEHR.Models.Interfaces;
using OBEHR.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class AccumulationRule : CitySupplierHukouBaseModel, IEditable<AccumulationRule>
    {
        public AccumulationRule()
        {
            Name = "无";
            Clients = new List<Client> { };
        }

        [DisplayName("公积金类型")]
        public int AccumulationTypeId { get; set; }

        [DisplayName("公积金基数上限")]
        public decimal Gjjjssx { get; set; }

        [DisplayName("公积金基数下限")]
        public decimal Gjjjsxx { get; set; }

        [DisplayName("公积金个人比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Gjjgrbl { get; set; }

        [DisplayName("公积金企业比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Gjjqybl { get; set; }

        [DisplayName("公积金个人固定缴费")]
        public decimal Gjjgrgdjf { get; set; }

        [DisplayName("公积金企业固定缴费")]
        public decimal Gjjqygdjf { get; set; }

        [DisplayName("补充公积金基数上限")]
        public decimal Bcgjjjssx { get; set; }

        [DisplayName("补充公积金基数下限")]
        public decimal Bcgjjjsxx { get; set; }

        [DisplayName("补充公积金个人比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Bcgjjgrbl { get; set; }

        [DisplayName("补充公积金企业比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Bcgjjqybl { get; set; }

        [DisplayName("补充公积金个人固定缴费")]
        public decimal Bcgjjgrgdjf { get; set; }

        [DisplayName("补充公积金企业固定缴费")]
        public decimal Bcgjjqygdjf { get; set; }

        [DisplayName("取舍规则")]
        public NumberRule NumberRule { get; set; }

        public virtual AccumulationType AccumulationType { get; set; }

        [DisplayName("适用客户")]
        public virtual ICollection<Client> Clients { get; set; }

        public void Edit(AccumulationRule model)
        {
            Name = model.Name;
            Gjjjssx = model.Gjjjssx;
            Gjjjsxx = model.Gjjjsxx;
            Gjjgrbl = model.Gjjgrbl;
            Gjjqybl = model.Gjjqybl;
            Gjjgrgdjf = model.Gjjgrgdjf;
            Gjjqygdjf = model.Gjjqygdjf;
            Bcgjjjssx = model.Bcgjjjssx;
            Bcgjjjsxx = model.Bcgjjjsxx;
            Bcgjjgrbl = model.Bcgjjgrbl;
            Bcgjjqybl = model.Bcgjjqybl;
            Bcgjjgrgdjf = model.Bcgjjgrgdjf;
            Bcgjjqygdjf = model.Bcgjjqygdjf;
            NumberRule = model.NumberRule;
        }

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
                        var AccumulationTypeName = db.AccumulationTypeRepository.GetByID(AccumulationTypeId).ToString();

                        tmpStr = cityName + "_" + SupplierName + "_" + HukouType + "_" + AccumulationTypeName;
                    }
                }
                else
                {
                    tmpStr = City.ToString() + "_" + Supplier.ToString() + "_" + HukouType + "_" + AccumulationType.ToString();
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