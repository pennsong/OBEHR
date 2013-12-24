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
    public class Calendar : ClientBaseModel, IEditable<Calendar>
    {
        public Calendar()
        {
            Name = "无";
        }

        [DisplayName("指定日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "日期格式不正确")]
        public DateTime DateDay { get; set; }

        [DisplayName("日期类型")]
        public DateType DateType { get; set; }
        public void Edit(Calendar model)
        {
            Name = model.Name;
            DateType = model.DateType;
        }
    }
}