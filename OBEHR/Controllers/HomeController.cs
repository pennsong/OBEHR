using OBEHR.Models.DAL;
using OBEHR.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBEHR.Controllers
{
    public class HomeController : Controller
    {
        private OBEHRContext db = new OBEHRContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult History(string ClassName, string Id, string FieldName)
        {
            ViewBag.ClassName = ClassName;
            ViewBag.FieldName = FieldName;

            var results = from a in db.ChangeSets
                          join b in db.ObjectChanges on a.Id equals b.ChangeSet.Id
                          join c in db.PropertyChanges on b.Id equals c.ObjectChange.Id
                          join d in db.Users on a.Author.Id equals d.Id
                          where b.TypeName == ClassName
                          where b.ObjectReference == Id
                          where c.PropertyName == FieldName
                          orderby a.Timestamp descending
                          select new FieldHistory
                          {
                              UserName = d.UserName,
                              Time = a.Timestamp,
                              Value = c.Value,
                          };

            return View(results);

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}