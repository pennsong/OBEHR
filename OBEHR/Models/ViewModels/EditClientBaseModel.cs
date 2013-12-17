using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Models.ViewModels
{
    public class EditClientBaseModel : EditBaseModel
    {
        [DisplayName("客户")]
        public Client Client { get; set; }
    }
}