using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PensionTypeController : BaseModelController<PensionType>
    {
        public PensionTypeController()
        {
            ViewBag.Name = "社保类型";
            ViewBag.Controller = "PensionType";
        }
    }
}