using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class ZhangtaoController : ClientBaseModelController<Zhangtao>
    {
        public ZhangtaoController()
        {
            ViewBag.Name = "账套";
            ViewBag.Controller = "Zhangtao";
        }
    }
}