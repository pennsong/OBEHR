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
    public class Document : ClientBaseModel, IEditable<Document>
    {
        [DisplayName("分数")]
        [Range(0, 100)]
        public int Point { get; set; }
        public override string ToString()
        {
            return Name;
        }

        public void Edit(Document model)
        {
            Name = model.Name;
            Point = model.Point;
        }
    }
}