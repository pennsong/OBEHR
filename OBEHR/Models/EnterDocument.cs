using OBEHR.Models.Base;
using OBEHR.Models.DAL;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class EnterDocument : ClientCityBaseModel
    {
        public EnterDocument()
        {
            Name = "无";
            Documents = new List<Document> { };
        }

        [DisplayName("入职材料")]
        public virtual ICollection<Document> Documents { get; set; }
        public override string ToString()
        {
            var tmpStr = "";
            if (Client == null)
            {
                using (var db = new UnitOfWork())
                {
                    var clientName = db.ClientRepository.GetByID(ClientId).Name;
                    var cityName = "无";
                    if (CityId != null)
                    {
                        cityName = db.CityRepository.GetByID(CityId).Name;
                    }
                    tmpStr = clientName + "_" + cityName;
                }
            }
            else
            {
                tmpStr = Client.ToString() + "_";
                if (City == null)
                {
                    tmpStr += "无";
                }
                else
                {
                    tmpStr += City.ToString();
                }
            }
            return tmpStr;
        }
    }
}