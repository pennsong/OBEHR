using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class ClientController : BaseModelController<Client>
    {
        public ClientController()
        {
            ViewBag.Name = "客户";
            ViewBag.Controller = "Client";
        }
    }
}