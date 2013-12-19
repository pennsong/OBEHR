using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class LevelController : ClientBaseModelController<Level>
    {
        public LevelController()
        {
            ViewBag.Name = "职级";
            ViewBag.Controller = "Level";
        }
    }
}