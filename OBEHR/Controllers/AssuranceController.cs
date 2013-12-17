using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class AssuranceController : ClientBaseController<Assurance>
    {
        public AssuranceController()
        {
            ViewBag.Name = "商业保险";
            ViewBag.Controller = "Assurance";
        }
    }
}