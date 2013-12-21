using OBEHR.Lib;
using OBEHR.Models;
using OBEHR.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace OBEHR.Controllers
{
    public class ClientController : BaseModelController<Client>
    {
        public ClientController()
        {
            ViewBag.Name = "客户";
            ViewBag.Controller = "Client";
        }

        [Authorize(Roles = "Admin")]
        public override ActionResult Index(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "Index" }, { "actionAjax", "Get" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "Admin")]
        public override PartialViewResult Get(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();
            var results = BaseCommon<Client>.GetQuery(UW, includeSoftDeleted, keyword);

            if (!includeSoftDeleted)
            {
                results = results.Where(a => a.IsDeleted == false);
            }

            results = results.OrderBy(a => a.Name);

            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<Client>.Page(this, rv, results));
        }

        [Authorize(Roles = "Admin, HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Details(int id = 0, string returnUrl = "Index")
        {
            Client result = null;
            //检查记录在权限范围内
            if (User.IsInRole("Admin"))
            {
                result = BaseCommon<Client>.GetQuery(UW).Where(a => a.Id == id).SingleOrDefault();
            }
            else if (User.IsInRole("HRAdmin"))
            {
                var ppUserId = User.Identity.GetUserId();
                result = BaseCommon<Client>.GetQuery(UW).Where(a => a.HRAPPUserId == ppUserId).Where(a => a.Id == id).SingleOrDefault();
            }
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create(string returnUrl = "Index")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Model/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult CreateSave(Client model, string returnUrl = "Index")
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
            return View("Create", model);
        }

        //
        // GET: /Model/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Edit(int id = 0, string returnUrl = "Index")
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

            return View(result);
        }

        //
        // POST: /Model/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult EditSave(Client model, string returnUrl = "Index")
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
                    result.Edit(model);
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Delete(int id = 0, string returnUrl = "Index")
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

            return View(result);
        }

        //
        // POST: /Model/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult DeleteSave(int id, string returnUrl = "Index")
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Restore(int id = 0, string returnUrl = "Index")
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

            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult RestoreSave(Client model, string returnUrl = "Index")
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
        public override PartialViewResult Abstract(int id)
        {
            var result = GR.GetByID(id);
            return PartialView(result);
        }

        //
        // GET: /Model/
        [Authorize(Roles = "HRAdmin")]
        public virtual ActionResult HRAIndex(int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            ViewBag.RV = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", "HRAIndex" }, { "actionAjax", "HRAGet" }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return View();
        }

        [Authorize(Roles = "HRAdmin")]
        public virtual PartialViewResult HRAGet(string returnRoot, string actionAjax = "", int page = 1, string keyword = "", bool includeSoftDeleted = false)
        {
            keyword = keyword.ToUpper();

            var ppUserId = User.Identity.GetUserId();
            var results = BaseCommon<Client>.GetQuery(UW, includeSoftDeleted, keyword).Where(a => a.HRAPPUserId == ppUserId);

            if (!includeSoftDeleted)
            {
                results = results.Where(a => a.IsDeleted == false);
            }

            results = results.OrderBy(a => a.Name);

            var rv = new RouteValueDictionary { { "tickTime", DateTime.Now.ToLongTimeString() }, { "returnRoot", returnRoot }, { "actionAjax", actionAjax }, { "page", page }, { "keyword", keyword }, { "includeSoftDeleted", includeSoftDeleted } };
            return PartialView(Common<Client>.Page(this, rv, results));
        }

        //
        // GET: /Model/Edit/5
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRAEdit(int id = 0, string returnUrl = "HRAIndex")
        {
            //检查记录在权限范围内
            var ppUserId = User.Identity.GetUserId();
            var result = BaseCommon<Client>.GetQuery(UW).Where(a => a.HRAPPUserId == ppUserId).Where(a => a.Id == id).SingleOrDefault();
            if (result == null)
            {
                Common.RMError(this);
                return Redirect(Url.Content(returnUrl));
            }
            //end 检查记录在权限范围内

            ViewBag.ReturnUrl = returnUrl;

            var model = new HRAEditClient
            {
                Id = result.Id,
                WorkCitiesIds = result.WorkCities.Select(a => a.Id).ToList(),
                TaxCitiesIds = result.TaxCities.Select(a => a.Id).ToList(),
                PensionCitiesIds = result.PensionCities.Select(a => a.Id).ToList(),
                AccumulationCitiesIds = result.AccumulationCities.Select(a => a.Id).ToList(),
                HRPPUsersIds = result.HRPPUsers.Select(a => a.Id).ToList(),
            };

            return View(model);
        }

        //
        // POST: /Model/Edit/5
        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HRAEditSave(HRAEditClient model, string returnUrl = "HRAIndex")
        {
            //检查记录在权限范围内
            var ppUserId = User.Identity.GetUserId();
            var result = BaseCommon<Client>.GetQuery(UW).Where(a => a.HRAPPUserId == ppUserId).Where(a => a.Id == model.Id).SingleOrDefault();
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
                    UW.context.Entry(result).Collection(p => p.WorkCities).Load();
                    UW.context.Entry(result).Collection(p => p.TaxCities).Load();
                    UW.context.Entry(result).Collection(p => p.PensionCities).Load();
                    UW.context.Entry(result).Collection(p => p.AccumulationCities).Load();
                    UW.context.Entry(result).Collection(p => p.HRPPUsers).Load();
                    var workCities = UW.CityRepository.Get().Where(a => model.WorkCitiesIds.Any(b => b == a.Id)).ToList();
                    var taxCities = UW.CityRepository.Get().Where(a => model.TaxCitiesIds.Any(b => b == a.Id)).ToList();
                    var pensionCities = UW.CityRepository.Get().Where(a => model.PensionCitiesIds.Any(b => b == a.Id)).ToList();
                    var accumulationCities = UW.CityRepository.Get().Where(a => model.AccumulationCitiesIds.Any(b => b == a.Id)).ToList();
                    var ppUsers = UW.context.Users.Where(a => model.HRPPUsersIds.Any(b => b == a.Id)).ToList();
                    result.WorkCities = workCities;
                    result.TaxCities = taxCities;
                    result.PensionCities = pensionCities;
                    result.AccumulationCities = accumulationCities;
                    result.HRPPUsers = ppUsers;
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

            return View("HRAEdit", model);
        }
    }
}