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
using OBEHR.Models.Base;
using OBEHR.Lib;
using OBEHR.Models.Interfaces;
using System.Data.Entity.Core;
using OBEHR.Models.ViewModels;
using AutoMapper;

namespace OBEHR.Controllers
{
    public class BaseModelController<Model> : Controller where Model : BaseModel
    {
        public UnitOfWork uw;
        public GenericRepository<Model> gr;
        public List<string> path;

        public BaseModelController()
        {
            uw = new UnitOfWork();
            gr = (GenericRepository<Model>)(typeof(UnitOfWork).GetProperty(typeof(Model).Name + "Repository").GetValue(uw));
        }

        //
        // GET: /Model/
        public virtual ActionResult Index(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "Index" }, { "actionAjax", "Get" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View("~/Views/BaseModel/Index.cshtml");
        }

        public virtual PartialViewResult Get(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = BaseCommon<Model>.GetQuery(uw, includeSoftDeleted, keyword);

            if (!includeSoftDeleted)
            {
                results = results.Where(a => a.IsDeleted == false);
            }

            results = results.OrderBy(a => a.Name);

            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView("~/Views/BaseModel/Get.cshtml", Common<Model>.Page(this, rv, results));
        }

        //
        // GET: /Model/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = gr.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            ViewBag.ReturnUrl = returnUrl;

            return View("~/Views/BaseModel/Details.cshtml", result);
        }

        //
        // GET: /Model/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string returnUrl = "Index")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/BaseModel/Create.cshtml");
        }

        //
        // POST: /Model/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSave(Model model, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            //end 检查记录在权限范围内
            if (ModelState.IsValid)
            {
                try
                {
                    gr.Insert(model);
                    uw.PPSave();
                    Common.RMOk(this, "记录:" + model + "新建成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (UpdateException e)
                {
                    if (e.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "新建记录失败!" + e.ToString());
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "新建记录失败!" + e.ToString());
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/BaseModel/Create.cshtml", model);
        }

        //
        // GET: /Model/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id = 0, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = gr.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            ViewBag.ReturnUrl = returnUrl;

            return View("~/Views/BaseModel/Edit.cshtml", result);
        }

        //
        // POST: /Model/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(Model model, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = gr.GetByID(model.Id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            if (ModelState.IsValid)
            {
                try
                {
                    var parent = (BaseModel)result;
                    parent.Edit(model);
                    uw.PPSave();
                    Common.RMOk(this, "记录:" + result + "保存成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (UpdateException e)
                {
                    if (e.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "编辑记录失败!" + e.ToString());
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "编辑记录失败!" + e.ToString());
                }
            }
            ViewBag.ReturnUrl = returnUrl;

            return View("~/Views/BaseModel/Edit.cshtml", model);
        }

        //
        // GET: /Model/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = 0, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = gr.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            ViewBag.ReturnUrl = returnUrl;

            return View("~/Views/BaseModel/Delete.cshtml", result);
        }

        //
        // POST: /Model/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSave(int id, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = gr.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内
            var removeName = result.ToString();
            try
            {
                gr.Delete(id);
                uw.PPSave();
                Common.RMOk(this, "记录:" + removeName + "删除成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                Common.RMError(this, "记录" + removeName + "删除失败!" + e.ToString());
            }
            return Redirect(Url.Content(returnUrl));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(int id = 0, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = gr.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            ViewBag.ReturnUrl = returnUrl;

            return View("~/Views/BaseModel/Restore.cshtml", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreSave(Model model, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = gr.GetByID(model.Id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            try
            {
                result.IsDeleted = false;
                uw.PPSave();
                Common.RMOk(this, "记录:" + result + "恢复成功!");
                return Redirect(Url.Content(returnUrl));
            }
            catch (Exception e)
            {
                Common.RMOk(this, "记录" + result + "恢复失败!" + e.ToString());
            }
            return Redirect(Url.Content(returnUrl));
        }

        [ChildActionOnly]
        public virtual PartialViewResult Abstract(int id)
        {
            var result = gr.GetByID(id);
            return PartialView("~/Views/BaseModel/Abstract.cshtml", result);
        }

        protected override void Dispose(bool disposing)
        {
            uw.Dispose();
            base.Dispose(disposing);
        }
    }
}