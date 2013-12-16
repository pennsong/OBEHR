using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OBEHR.Models.Base
{
    public class SoftDelete
    {
        public SoftDelete()
        {
            IsDeleted = false;
        }

        [ScaffoldColumn(false)]
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
    }
}