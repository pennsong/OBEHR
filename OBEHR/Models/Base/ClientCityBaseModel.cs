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
    public class ClientCityBaseModel : ClientBaseModel
    {
        [DisplayName("城市")]
        public int? CityId { get; set; }
        public virtual EnterDocument ClientCity { get; set; }
    }
}