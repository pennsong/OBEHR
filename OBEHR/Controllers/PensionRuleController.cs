using OBEHR.Lib;
using OBEHR.Models;
using OBEHR.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OBEHR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PensionRuleController : BaseModelController<PensionRule>
    {
        public PensionRuleController()
        {
            ViewBag.Name = "社保规则";
            ViewBag.Controller = "PensionRule";
            ViewPath = "PensionRule";
            ViewPathBase = "PensionRule";
        }

        public override PartialViewResult Get(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false, FormCollection fc = null)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetPensionRuleQuery(UW, includeSoftDeleted, keyword);

            if (!includeSoftDeleted)
            {
                results = results.Where(a => a.IsDeleted == false);
            }

            results = results.OrderBy(a => a.Name);

            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<PensionRule>.Page(this, rv, results));
        }
    }
}