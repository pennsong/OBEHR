using OBEHR.Models.Interfaces;
using OBEHR.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Models.Base
{
    public class BaseModel : IEditable<EditBaseModel>
    {
        public BaseModel()
        {
            IsDeleted = false;
        }

        [HiddenInput(DisplayValue = false)]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("名称")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("已删除")]
        public bool IsDeleted { get; set; }

        //FrameLog related
        public object Reference
        {
            get { return Id; }
        }
        //end FrameLog related
        public void Edit(EditBaseModel model)
        {
            Name = model.Name;
        }
    }
}