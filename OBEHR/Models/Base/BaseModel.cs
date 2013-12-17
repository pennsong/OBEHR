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
    public class BaseModel : IEditable<BaseModel>
    {
        public BaseModel()
        {
            IsDeleted = false;
        }

        [DisplayName("系统ID")]
        public int Id { get; set; }

        [DisplayName("名称")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [DisplayName("已删除")]
        public bool IsDeleted { get; set; }

        //FrameLog related
        public object Reference
        {
            get { return Id; }
        }
        //end FrameLog related
        public void Edit(BaseModel model)
        {
            Name = model.Name;
        }
    }
}