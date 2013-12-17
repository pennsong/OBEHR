using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class DocumentController : ClientBaseModelController<Document>
    {
        public DocumentController()
        {
            ViewBag.Name = "客户材料";
            ViewBag.Controller = "Document";
            ViewPath = "Document";
        }
    }
}