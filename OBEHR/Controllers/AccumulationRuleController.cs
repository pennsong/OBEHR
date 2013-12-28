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
    public class AccumulationRuleController : BaseModelController<AccumulationRule>
    {
        public AccumulationRuleController()
        {
            ViewBag.Name = "公积金规则";
            ViewBag.Controller = "AccumulationRule";
            ViewPath = "AccumulationRule";
            ViewPathBase = "AccumulationRule";
        }

        public override PartialViewResult Get(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false, FormCollection fc = null)
        {
            keyword = keyword.ToUpper();
            var results = Common.GetAccumulationRuleQuery(UW, includeSoftDeleted, keyword);

            if (!includeSoftDeleted)
            {
                results = results.Where(a => a.IsDeleted == false);
            }

            results = results.OrderBy(a => a.Name);

            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<AccumulationRule>.Page(this, rv, results));
        }
    }
}