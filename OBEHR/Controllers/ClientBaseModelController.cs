using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OBEHR.Models;
using OBEHR.Models.DAL;
using System.Web.Routing;
using OBEHR.Lib;
using OBEHR.Models.Interfaces;
using System.Data.Entity.Core;
using OBEHR.Models.Base;
using OBEHR.Models.ViewModels;
using AutoMapper;

namespace OBEHR.Controllers
{
    public class ClientBaseModelController<Model> : BaseModelController<Model> where Model : ClientBaseModel, IEditable<Model>
    {
        public ClientBaseModelController()
        {
            ViewPath = "ClientBaseModel";
            ViewPathBase = "ClientBaseModel";
        }
        public override PartialViewResult Get(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false, FormCollection fc = null)
        {
            keyword = keyword.ToUpper();
           
            var results = BaseCommon<Model>.GetQuery(UW, includeSoftDeleted, keyword);

            if (!includeSoftDeleted)
            {
                results = results.Where(a => a.IsDeleted == false);
            }

            results = results.OrderBy(a => a.Name).OrderBy(a => a.Client.Name);

            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(ViewPath1 + ViewPath + ViewPath2 + "Get.cshtml", Common<Model>.Page(this, rv, results));
        }
    }
}