using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class DepartmentController : ClientBaseModelController<Department>
    {
        public DepartmentController()
        {
            ViewBag.Name = "部门";
            ViewBag.Controller = "Department";
        }
    }
}