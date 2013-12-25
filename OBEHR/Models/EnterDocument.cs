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
    public class EnterDocument : ClientCityBaseModel, IEditable<EnterDocument>
    {
        public EnterDocument()
        {
            Name = "无";
            EnterDocuments = new List<Document> { };
        }

        [DisplayName("入职材料")]
        public virtual ICollection<Document> EnterDocuments { get; set; }

        public List<Document> GetEnterDocuments()
        {
            return EnterDocuments.Where(a => a.IsDeleted == false).ToList();
        }
        public void Edit(EnterDocument model)
        {
            Name = model.Name;
        }

        public override string ToString()
        {
            var tmpStr = "";
            if (Client == null)
            {
                using (var db = new UnitOfWork())
                {
                    var clientName = db.ClientRepository.GetByID(ClientId).ToString();
                    var cityName = "无";
                    if (CityId != null)
                    {
                        cityName = db.CityRepository.GetByID(CityId).ToString();
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