using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Models.ViewModels
{
    public class EditBaseModel
    {
        [HiddenInput(DisplayValue = false)]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("名称")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}