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
    public class Client : BaseModel, IEditable<Client>
    {
        [DisplayName("HR系统管理员")]
        public string PPUserId { get; set; }

        [DisplayName("工作城市")]
        public virtual ICollection<City> WorkCities { get; set; }
        [DisplayName("纳税城市")]
        public virtual ICollection<City> TaxCities { get; set; }
        [DisplayName("社保城市")]
        public virtual ICollection<City> PensionCities { get; set; }
        [DisplayName("公积金城市")]
        public virtual ICollection<City> AccumulationCities { get; set; }

        public virtual PPUser PPUser { get; set; }
        public override string ToString()
        {
            return Name;
        }

        public void Edit(Client model)
        {
            Name = model.Name;
            PPUserId = model.PPUserId;
        }
    }
}