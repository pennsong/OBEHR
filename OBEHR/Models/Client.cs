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
        public Client()
        {
            WorkCities = new List<City> { };
            TaxCities = new List<City> { };
            PensionCities = new List<City> { };
            AccumulationCities = new List<City> { };
            AccumulationRules = new List<AccumulationRule> { };
            PensionRules = new List<PensionRule> { };

            HRPPUsers = new List<PPUser> { };
        }

        [DisplayName("HR系统管理员")]
        public string HRAPPUserId { get; set; }

        [DisplayName("工作城市")]
        public virtual ICollection<City> WorkCities { get; set; }
        [DisplayName("纳税城市")]
        public virtual ICollection<City> TaxCities { get; set; }
        [DisplayName("社保城市")]
        public virtual ICollection<City> PensionCities { get; set; }
        [DisplayName("公积金城市")]
        public virtual ICollection<City> AccumulationCities { get; set; }
        [DisplayName("HR")]
        public virtual ICollection<PPUser> HRPPUsers { get; set; }
        [DisplayName("公积金规则")]
        public virtual ICollection<AccumulationRule> AccumulationRules { get; set; }
        [DisplayName("社保规则")]
        public virtual ICollection<PensionRule> PensionRules { get; set; }

        public virtual PPUser HRAPPUser { get; set; }

        public List<City> GetWorkCities()
        {
            return WorkCities.Where(a => a.IsDeleted == false).ToList();
        }
        public List<City> GetTaxCities()
        {
            return TaxCities.Where(a => a.IsDeleted == false).ToList();
        }
        public List<City> GetPensionCities()
        {
            return PensionCities.Where(a => a.IsDeleted == false).ToList();
        }
        public List<City> GetAccumulationCities()
        {
            return AccumulationCities.Where(a => a.IsDeleted == false).ToList();
        }
        public List<AccumulationRule> GetAccumulationRules()
        {
            return AccumulationRules.Where(a => a.IsDeleted == false).ToList();
        }

        public void Edit(Client model)
        {
            Name = model.Name;
            HRAPPUserId = model.HRAPPUserId;
        }
    }
}