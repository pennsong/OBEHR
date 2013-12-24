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
    public class PensionRule : CitySupplierHukouBaseModel, IEditable<PensionRule>
    {
        public PensionRule()
        {
            Name = "无";
            Clients = new List<Client> { };
        }

        [DisplayName("社保类型")]
        public int PensionTypeId { get; set; }

        [DisplayName("养老基数上限")]
        public decimal Yljssx { get; set; }

        [DisplayName("养老基数下限")]
        public decimal Yljsxx { get; set; }

        [DisplayName("养老个人比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Grylbl { get; set; }

        [DisplayName("养老企业比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Qyylbl { get; set; }

        [DisplayName("养老个人固定缴费")]
        public decimal Ylgrgdjf { get; set; }

        [DisplayName("养老企业固定缴费")]
        public decimal Ylqygdjf { get; set; }

        [DisplayName("失业基数上限")]
        public decimal Syjssx { get; set; }

        [DisplayName("失业基数下限")]
        public decimal Syjsxx { get; set; }

        [DisplayName("失业个人比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Sygrbl { get; set; }

        [DisplayName("失业企业比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Syqybl { get; set; }

        [DisplayName("失业个人固定缴费")]
        public decimal Sygrgdjf { get; set; }

        [DisplayName("失业企业固定缴费")]
        public decimal Syqygdjf { get; set; }

        [DisplayName("医疗基数上限")]
        public decimal Yiliaojssx { get; set; }

        [DisplayName("医疗基数下限")]
        public decimal Yiliaojsxx { get; set; }

        [DisplayName("医疗个人比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Yiliaogrbl { get; set; }

        [DisplayName("医疗企业比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Yiliaoqybl { get; set; }

        [DisplayName("医疗个人固定缴费")]
        public decimal Yiliaogrgdjf { get; set; }

        [DisplayName("医疗企业固定缴费")]
        public decimal Yiliaoqygdjf { get; set; }

        [DisplayName("补充基数上限")]
        public decimal Bcjssx { get; set; }

        [DisplayName("补充基数下限")]
        public decimal Bcjsxx { get; set; }

        [DisplayName("补充个人比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Bcgrbl { get; set; }

        [DisplayName("补充企业比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Bcqybl { get; set; }

        [DisplayName("补充个人固定缴费")]
        public decimal Bcgrgdjf { get; set; }

        [DisplayName("补充企业固定缴费")]
        public decimal Bcqygdjf { get; set; }

        [DisplayName("其他基数上限")]
        public decimal Qtjssx { get; set; }

        [DisplayName("其他基数下限")]
        public decimal Qtjsxx { get; set; }

        [DisplayName("其他个人比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Qtgrbl { get; set; }

        [DisplayName("其他企业比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Qtqybl { get; set; }

        [DisplayName("其他个人固定缴费")]
        public decimal Qtgrgdjf { get; set; }

        [DisplayName("其他企业固定缴费")]
        public decimal Qtqygdjf { get; set; }

        [DisplayName("工伤基数上限")]
        public decimal Gsjssx { get; set; }

        [DisplayName("工伤基数下限")]
        public decimal Gsjsxx { get; set; }

        [DisplayName("工伤企业比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Gsqybl { get; set; }

        [DisplayName("工伤企业固定缴费")]
        public decimal Gsqygdjf { get; set; }

        [DisplayName("生育基数上限")]
        public decimal Shengyujssx { get; set; }

        [DisplayName("生育基数下限")]
        public decimal Shengyujsxx { get; set; }

        [DisplayName("生育企业比例")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")]
        public decimal Shengyuqybl { get; set; }

        [DisplayName("生育企业固定缴费")]
        public decimal Shengyuqygdjf { get; set; }

        [DisplayName("取舍规则")]
        public NumberRule NumberRule { get; set; }

        public virtual PensionType PensionType { get; set; }

        [DisplayName("适用客户")]
        public virtual ICollection<Client> Clients { get; set; }

        public void Edit(PensionRule model)
        {
            Name = model.Name;
            Yljssx = model.Yljssx;
            Yljsxx = model.Yljsxx;
            Grylbl = model.Grylbl;
            Qyylbl = model.Qyylbl;
            Ylgrgdjf = model.Ylgrgdjf;
            Ylqygdjf = model.Ylqygdjf;
            Syjssx = model.Syjssx;
            Syjsxx = model.Syjsxx;
            Sygrbl = model.Sygrbl;
            Syqybl = model.Syqybl;
            Sygrgdjf = model.Sygrgdjf;
            Syqygdjf = model.Syqygdjf;
            Yiliaojssx = model.Yiliaojssx;
            Yiliaojsxx = model.Yiliaojsxx;
            Yiliaogrbl = model.Yiliaogrbl;
            Yiliaoqybl = model.Yiliaoqybl;
            Yiliaogrgdjf = model.Yiliaogrgdjf;
            Yiliaoqygdjf = model.Yiliaoqygdjf;
            Bcjssx = model.Bcjssx;
            Bcjsxx = model.Bcjsxx;
            Bcgrbl = model.Bcgrbl;
            Bcqybl = model.Bcqybl;
            Bcgrgdjf = model.Bcgrgdjf;
            Bcqygdjf = model.Bcqygdjf;
            Qtjssx = model.Qtjssx;
            Qtjsxx = model.Qtjsxx;
            Qtgrbl = model.Qtgrbl;
            Qtqybl = model.Qtqybl;
            Qtgrgdjf = model.Qtgrgdjf;
            Qtqygdjf = model.Qtqygdjf;
            Gsjssx = model.Gsjssx;
            Gsjsxx = model.Gsjsxx;
            Gsqybl = model.Gsqybl;
            Gsqygdjf = model.Gsqygdjf;
            Shengyujssx = model.Shengyujssx;
            Shengyujsxx = model.Shengyujsxx;
            Shengyuqybl = model.Shengyuqybl;
            Shengyuqygdjf = model.Shengyuqygdjf;
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
                        var PensionTypeName = db.PensionTypeRepository.GetByID(PensionTypeId).ToString();

                        tmpStr = cityName + "_" + SupplierName + "_" + HukouType + "_" + PensionTypeName;
                    }
                }
                else
                {
                    tmpStr = City.ToString() + "_" + Supplier.ToString() + "_" + HukouType + "_" + PensionType.ToString();
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