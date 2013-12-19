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
    public class ClientCityBaseModel : BaseModel
    {
        [DisplayName("客户城市")]
        public int ClientCityId { get; set; }
        public virtual ClientCity ClientCity { get; set; }
    }
}