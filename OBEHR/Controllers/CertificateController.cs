using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class CertificateController : BaseController<Certificate>
    {
        public CertificateController()
        {
            ViewBag.Name = "证件";
            ViewBag.Controller = "Certificate";
        }
    }
}