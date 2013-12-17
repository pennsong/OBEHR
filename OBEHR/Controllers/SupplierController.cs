using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupplierController : BaseModelController<Supplier>
    {
        public SupplierController()
        {
            ViewBag.Name = "供应商";
            ViewBag.Controller = "Supplier";
            base.ViewPath = "Supplier";
        }
    }
}