using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccumulationTaxFreeController : CitySupplierBaseModelController<AccumulationTaxFree>
    {
        public AccumulationTaxFreeController()
        {
            ViewBag.Name = "公积金免税额度";
            ViewBag.Controller = "AccumulationTaxFree";
            ViewPath = "AccumulationTaxFree";
        }
    }
}