﻿using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccumulationTypeController : BaseModelController<AccumulationType>
    {
        public AccumulationTypeController()
        {
            ViewBag.Name = "公积金类型";
            ViewBag.Controller = "AccumulationType";
        }
    }
}