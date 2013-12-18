﻿using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class EnterDocumentController : ClientCityBaseModelController<EnterDocument>
    {
        public EnterDocumentController()
        {
            ViewBag.Name = "入职材料";
            ViewBag.Controller = "EnterDocument";
        }
    }
}