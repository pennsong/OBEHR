using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class PositionController : ClientBaseModelController<Position>
    {
        public PositionController()
        {
            ViewBag.Name = "职位";
            ViewBag.Controller = "Position";
        }
    }
}