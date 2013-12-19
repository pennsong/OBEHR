using OBEHR.Models.Base;
using OBEHR.Models.DAL;
using OBEHR.Models.Interfaces;
using OBEHR.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OBEHR.Models
{
    public class ClientCity : ClientBaseModel
    {
        public ClientCity()
        {
            Name = "无";
        }

        [DisplayName("城市")]
        public int? CityId { get; set; }

        [DisplayName("入职材料")]
        public virtual ICollection<Document> EnterDocuments { get; set; }

        public virtual City City { get; set; }
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