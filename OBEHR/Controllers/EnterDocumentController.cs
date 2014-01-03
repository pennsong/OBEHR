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
    public class EnterDocumentController : ClientCityBaseModelController<EnterDocument>
    {
        public EnterDocumentController()
        {
            ViewBag.Name = "入职材料";
            ViewBag.Controller = "EnterDocument";
            ViewPath = "EnterDocument";
        }

        //
        // GET: /Model/Edit/5
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

            var model = new EditEnterDocument
            {
                Id = result.Id,
                ClientId = result.ClientId,
                Name = result.Name,
                EnterDocumentsIds = result.EnterDocuments.Select(a => a.Id).ToList(),
            };

            return View(model);
        }

        //
        // POST: /Model/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSave2(EditEnterDocument model, string returnUrl = "Index")
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
                    result.Name = model.Name;
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
    }
}