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
    [Authorize(Roles = "HRAdmin")]
    public class ClientCityController : Controller
    {
        public UnitOfWork UW;
        public GenericRepository<ClientCity> GR;
        public List<string> path;
        private string ViewPath1 = "~/Views/";
        public string ViewPath = "ClientCity";
        private string ViewPath2 = "/";

        public ClientCityController()
        {
            UW = new UnitOfWork();
            GR = UW.ClientCityRepository;
            ViewBag.Name = "客户城市";
            ViewBag.Controller = "ClientCity";
            ViewPath = "ClientCity";
        }

        //
        // GET: /Model/
        public virtual ActionResult Index(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "Index" }, { "actionAjax", "Get" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View(ViewPath1 + ViewPath + ViewPath2 + "Index.cshtml");
        }

        public virtual PartialViewResult Get(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = ClientCityCommon.GetQuery(UW, includeSoftDeleted, keyword);

            if (!includeSoftDeleted)
            {
                results = results.Where(a => a.IsDeleted == false);
            }

            results = results.OrderBy(a => a.Name).OrderBy(a => a.Client.Name);

            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(ViewPath1 + ViewPath + ViewPath2 + "Get.cshtml", Common<ClientCity>.Page(this, rv, results));
        }

        //
        // GET: /Certificate/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = GR.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            ViewBag.ReturnUrl = returnUrl;

            return View(ViewPath1 + ViewPath + ViewPath2 + "Details.cshtml", result);
        }

        //
        // GET: /Model/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string returnUrl = "Index")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(ViewPath1 + ViewPath + ViewPath2 + "Create.cshtml");
        }

        //
        // POST: /Model/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSave(ClientCity model, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            //end 检查记录在权限范围内
            if (ModelState.IsValid)
            {
                try
                {
                    GR.Insert(model);
                    UW.PPSave();
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
            return View(ViewPath1 + ViewPath + ViewPath2 + "Create.cshtml", model);
        }

        //
        // GET: /Certificate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id = 0, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = GR.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            ViewBag.ReturnUrl = returnUrl;

            var model = new EditClientCity
            {
                Id = result.Id,
                ClientId = result.ClientId,
                EnterDocumentsIds = result.EnterDocuments.Select(a => a.Id).ToList(),
            };

            return View(model);
        }

        //
        // POST: /Certificate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave(EditClientCity model, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = GR.GetByID(model.Id);
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
                    UW.context.Entry(result).Collection(p => p.EnterDocuments).Load();
                    var enterDocuments = UW.DocumentRepository.Get().Where(a => model.EnterDocumentsIds.Any(b => b == a.Id)).ToList();
                    result.EnterDocuments = enterDocuments;
                    UW.PPSave();
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

            return View("Edit", model);
        }

        //
        // GET: /Model/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id = 0, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = GR.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            ViewBag.ReturnUrl = returnUrl;

            return View(ViewPath1 + ViewPath + ViewPath2 + "Delete.cshtml", result);
        }

        //
        // POST: /Certificate/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSave(int id, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = GR.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内
            var removeName = result.ToString();
            try
            {
                GR.Delete(id);
                UW.PPSave();
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
            var result = GR.GetByID(id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            ViewBag.ReturnUrl = returnUrl;

            return View(ViewPath1 + ViewPath + ViewPath2 + "Restore.cshtml", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreSave(ClientCity model, string returnUrl = "Index")
        {
            //检查记录在权限范围内
            var result = GR.GetByID(model.Id);
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            try
            {
                result.IsDeleted = false;
                UW.PPSave();
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
            var result = GR.GetByID(id);
            return PartialView(ViewPath1 + ViewPath + ViewPath2 + "Abstract.cshtml", result);
        }

        [ChildActionOnly]
        public virtual PartialViewResult AbstractEdit(int id)
        {
            var result = GR.GetByID(id);
            return PartialView(result);
        }

        protected override void Dispose(bool disposing)
        {
            UW.Dispose();
            base.Dispose(disposing);
        }
    }
}