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
    public class ClientBaseModel : BaseModel
    {
        [DisplayName("客户")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}