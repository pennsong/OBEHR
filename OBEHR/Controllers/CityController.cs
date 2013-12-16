using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class CityController : BaseController<City>
    {
        public CityController()
        {
            ViewBag.Name = "城市";
            ViewBag.Controller = "City";
        }
    }
}