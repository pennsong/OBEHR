using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class CalendarController : ClientBaseModelController<Calendar>
    {
        public CalendarController()
        {
            ViewBag.Name = "客户日历";
            ViewBag.Controller = "Calendar";
            ViewPath = "Calendar";
        }
    }
}