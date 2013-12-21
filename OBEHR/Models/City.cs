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
    public class City : BaseModel, IEditable<City>
    {
        [DisplayName("工作客户")]
        public virtual ICollection<Client> WorkClients { get; set; }
        [DisplayName("纳税客户")]
        public virtual ICollection<Client> TaxClients { get; set; }
        [DisplayName("社保客户")]
        public virtual ICollection<Client> PensionClients { get; set; }
        [DisplayName("公积金客户")]
        public virtual ICollection<Client> AccumulationClients { get; set; }
        public override string ToString()
        {
            return Name;
        }

        public void Edit(City model)
        {
            Name = model.Name;
        }
    }
}