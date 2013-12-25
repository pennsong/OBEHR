using OBEHR.Models.Base;
using OBEHR.Models.DAL;
using OBEHR.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OBEHR.Models.ViewModels
{
    public class EditEnterDocument
    {
        public EditEnterDocument()
        {
            EnterDocumentsIds = new List<int> { };
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<int> EnterDocumentsIds { get; set; }
    }
}