﻿using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class BudgetCenterController : ClientBaseModelController<BudgetCenter>
    {
        public BudgetCenterController()
        {
            ViewBag.Name = "成本中心";
            ViewBag.Controller = "BudgetCenter";
        }
    }
}