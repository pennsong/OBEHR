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

namespace OBEHR.Controllers
{
    public class BaseController<Model> : Controller where Model : SoftDelete, IEditable<Model>
    {
        public UnitOfWork uw;
        public GenericRepository<Model> gr;
        public List<string> path;

        public BaseController()
        {
            uw = new UnitOfWork();
            gr = (GenericRepository<Model>)(typeof(UnitOfWork).GetProperty(typeof(Model).Name + "Repository").GetValue(uw));
        }

        //
        // GET: /Model/
        public virtual ActionResult Index(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "Index" }, { "actionAjax", "Get" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }
        public virtual PartialViewResult Get(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = gr.Get(filter: q => q.Name.ToUpper().Contains(keyword), orderBy: q => q.OrderBy(a => a.Id));

            if (!includeSoftDeleted)
            {
                results = results.Where(a => a.IsDeleted == false);
            }

            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<Model>.Page(this, rv, results));
        }

        //
        // GET: /Certificate/Details/5
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

            return View(result);
        }

        //
        // GET: /Model/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string returnUrl = "Index")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
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
                catch (Exception e)
                {
                    if (e.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "新建记录失败!" + e.ToString());
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View("Create", model);
        }

        //
        // GET: /Certificate/Edit/5
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

            return View(result);
        }

        //
        // POST: /Certificate/Edit/5
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
                    result.Edit(model);
                    uw.PPSave();
                    Common.RMOk(this, "记录:" + model + "保存成功!");
                    return Redirect(Url.Content(returnUrl));
                }
                catch (Exception e)
                {
                    if (e.InnerException.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    {
                        ModelState.AddModelError(string.Empty, "相同名称的记录已存在,保存失败!");
                    }
                }
            }
            ViewBag.ReturnUrl = returnUrl;

            return View("Edit", model);
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

            return View(result);
        }

        //
        // POST: /Certificate/Delete/5
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
                if (e.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    Common.RMError(this, "记录" + removeName + "被其他记录引用, 不能删除!");
                }
                else
                {
                    Common.RMError(this, "记录" + removeName + "删除失败!" + e.ToString());
                }
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

            return View(result);
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
            return PartialView(result);
        }

        protected override void Dispose(bool disposing)
        {
            uw.Dispose();
            base.Dispose(disposing);
        }
    }
}