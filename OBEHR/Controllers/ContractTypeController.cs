using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class ContractTypeController : ClientBaseModelController<ContractType>
    {
        public ContractTypeController()
        {
            ViewBag.Name = "合同类型";
            ViewBag.Controller = "ContractType";
        }
    }
}