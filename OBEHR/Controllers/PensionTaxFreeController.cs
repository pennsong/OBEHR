using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PensionTaxFreeController : CitySupplierBaseModelController<PensionTaxFree>
    {
        public PensionTaxFreeController()
        {
            ViewBag.Name = "社保免税额度";
            ViewBag.Controller = "PensionTaxFree";
            ViewPath = "PensionTaxFree";
        }
    }
}