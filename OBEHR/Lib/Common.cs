using OBEHR.Models.ViewModels;
using OBEHR.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OBEHR.Models.DAL;
using OBEHR.Models.Base;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Reflection;

namespace OBEHR.Lib
{
    public class Common<Model> where Model : class
    {
        public static IQueryable<Model> DynamicContains<TProperty>(
            IQueryable<Model> query,
            string property,
            IEnumerable<TProperty> items)
        {
            var pe = Expression.Parameter(typeof(Model));
            var me = Expression.Property(pe, property);
            var ce = Expression.Constant(items);
            var call = Expression.Call(typeof(Enumerable), "Contains", new[] { me.Type }, ce, me);
            var lambda = Expression.Lambda<Func<Model, bool>>(call, pe);
            return query.Where(lambda);
        }

        //property: xxxx@=, xxxx@>, xxxx@>= ... 
        public static IQueryable<Model> DynamicFilter(
            IQueryable<Model> query,
            string property,
            string target)
        {
            var pe = Expression.Parameter(typeof(Model), "pe");

            var tmp = property.Split('@');

            if (tmp[1] == "%")//模糊匹配
            {
                var words = tmp[0].Split('|');

                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                var wordsExps = new List<Expression>();
                foreach (var item in words)
                {
                    var propertyNames = item.Split('.');
                    Expression left = pe;

                    foreach (var prop in propertyNames)
                    {
                        left = Expression.PropertyOrField(left, prop);
                    }

                    var right = Expression.Constant(target);

                    wordsExps.Add(Expression.Call(left, method, right));
                }

                Expression finalExp = wordsExps[0];
                for (int i = 1; i < wordsExps.Count; i++)
                {
                    finalExp = Expression.OrElse(finalExp, wordsExps[i]);
                }
                var lambda = Expression.Lambda<Func<Model, bool>>(finalExp, pe);
                return query.Where(lambda);
            }
            else
            {

                var propertyNames = tmp[0].Split('.');

                Expression left = pe;
                foreach (var prop in propertyNames)
                {
                    left = Expression.PropertyOrField(left, prop);
                }

                var right = Expression.Constant(target);

                BinaryExpression call = null;
                if (tmp[1] == "=")
                {
                    call = Expression.Equal(left, right);
                }
                else if (tmp[1] == ">")
                {
                    call = Expression.GreaterThan(left, right);
                }
                else if (tmp[1] == ">=")
                {
                    call = Expression.GreaterThanOrEqual(left, right);
                }
                else if (tmp[1] == "<")
                {
                    call = Expression.LessThan(left, right);
                }
                else if (tmp[1] == "<=")
                {
                    call = Expression.LessThanOrEqual(left, right);
                }

                var lambda = Expression.Lambda<Func<Model, bool>>(call, pe);
                return query.Where(lambda);
            }

        }

        public static IQueryable<Model> Page(Controller c, RouteValueDictionary rv, IQueryable<Model> q, int size = 2)
        {
            var tmpPage = rv.Where(a => a.Key == "page").Select(a => a.Value).SingleOrDefault();
            int page = int.Parse(tmpPage.ToString());
            var tmpTotalPage = (int)Math.Ceiling(((decimal)(q.Count()) / size));
            page = page > tmpTotalPage ? tmpTotalPage : page;
            page = page == 0 ? 1 : page;
            rv.Add("totalPage", tmpTotalPage);
            rv["page"] = page;

            c.ViewBag.RV = rv;
            return q.Skip(((tmpTotalPage > 0 ? page : 1) - 1) * size).Take(size);
        }
    }

    public class BaseCommon<Model> where Model : BaseModel
    {
        //query and list
        public static List<Model> GetList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<Model> GetQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false, Dictionary<string, string> filter = null)
        {
            IQueryable<Model> result;

            var rep = (GenericRepository<Model>)(typeof(UnitOfWork).GetProperty(typeof(Model).Name + "Repository").GetValue(db));

            var baseType = typeof(Model).BaseType.Name;

            result = rep.Get(noTrack);

            if (baseType == "ClientBaseModel")
            {
                //Admin
                //end Admin

                //HRAdmin, HR
                if (HttpContext.Current.User.IsInRole("HRAdmin") || HttpContext.Current.User.IsInRole("HR"))
                {

                    var ppUser = db.context.UserManager.FindById(HttpContext.Current.User.Identity.GetUserId());

                    var clientsIds = ppUser.HRAdminClients.Select(a => a.Id);

                    result = Common<Model>.DynamicContains(result, "ClientId", clientsIds);
                }
                //end HRAdmin, HR
            }

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }

            //filter
            if (filter != null)
            {
                foreach (var item in filter)
                {
                    result = Common<Model>.DynamicFilter(result, item.Key, item.Value);
                }
            }
            //end filter
            return result;
        }
    }

    public class ClientBaseCommon<Model> where Model : ClientBaseModel
    {
        //query and list
        public static List<Model> GetList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static List<Model> GetClientList(int clientId, bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetClientQuery(db, clientId, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<Model> GetQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<Model> result;

            var rep = (GenericRepository<Model>)(typeof(UnitOfWork).GetProperty(typeof(Model).Name + "Repository").GetValue(db));

            result = rep.Get(noTrack);

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord) || a.Client.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        public static IQueryable<Model> GetClientQuery(UnitOfWork db, int clientId, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<Model> result;

            var rep = (GenericRepository<Model>)(typeof(UnitOfWork).GetProperty(typeof(Model).Name + "Repository").GetValue(db));

            result = rep.Get(noTrack).Where(a => a.ClientId == clientId);

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord) || a.Client.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
    }

    public class ClientCityCommon
    {
        //query and list
        public static List<EnterDocument> GetList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<EnterDocument> GetQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<EnterDocument> result;

            var rep = db.EnterDocumentRepository;

            result = rep.Get(noTrack);

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.Client.Name.ToUpper().Contains(keyWord) || a.City.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
    }

    public class ClientCityBaseCommon<Model> where Model : ClientCityBaseModel
    {
        //query and list
        public static List<Model> GetList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<Model> GetQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<Model> result;

            var rep = (GenericRepository<Model>)(typeof(UnitOfWork).GetProperty(typeof(Model).Name + "Repository").GetValue(db));

            result = rep.Get(noTrack);

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord) || a.Client.Name.ToUpper().Contains(keyWord) || a.City.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
    }

    public class CitySupplierBaseCommon<Model> where Model : CitySupplierBaseModel
    {
        //query and list
        public static List<Model> GetList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<Model> GetQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<Model> result;

            var rep = (GenericRepository<Model>)(typeof(UnitOfWork).GetProperty(typeof(Model).Name + "Repository").GetValue(db));

            result = rep.Get(noTrack);

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord) || a.City.Name.ToUpper().Contains(keyWord) || a.Supplier.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
    }

    public class Common
    {
        //Supplier
        public static List<Supplier> GetPensionSupplierList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetPensionSupplierQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<Supplier> GetPensionSupplierQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<Supplier> result;

            var rep = db.SupplierRepository;

            result = rep.Get(noTrack);

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }

            result = result.Where(a => a.IsPension == true);
            return result;
        }
        public static List<Supplier> GetAccumulationSupplierList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetAccumulationSupplierQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<Supplier> GetAccumulationSupplierQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<Supplier> result;

            var rep = db.SupplierRepository;

            result = rep.Get(noTrack);

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }

            result = result.Where(a => a.IsAccumulation == true);
            return result;
        }
        //end Supplier

        //PensionRule
        public static List<PensionRule> GetPensionRuleList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetPensionRuleQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<PensionRule> GetPensionRuleQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<PensionRule> result;

            var rep = db.PensionRuleRepository;

            result = rep.Get(noTrack);

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();

                //Enum
                var hukouNameList = Enum.GetNames(typeof(HukouType)).Where(a => a.ToUpper().Contains(keyWord));

                var hukouList = new List<HukouType> { };

                foreach (var item in hukouNameList)
                {
                    hukouList.Add((HukouType)(Enum.Parse(typeof(HukouType), item, true)));
                }
                //end Enum

                result = result.Where(a => a.Name.ToUpper().Contains(keyWord) || a.City.Name.ToUpper().Contains(keyWord) || a.Supplier.Name.ToUpper().Contains(keyWord) || a.PensionType.Name.ToUpper().Contains(keyWord) || hukouList.Contains(a.HukouType));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end PensionRule

        //AccumulationRule
        public static List<AccumulationRule> GetAccumulationRuleList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetAccumulationRuleQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<AccumulationRule> GetAccumulationRuleQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<AccumulationRule> result;

            var rep = db.AccumulationRuleRepository;

            result = rep.Get(noTrack);

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();

                //Enum
                var hukouNameList = Enum.GetNames(typeof(HukouType)).Where(a => a.ToUpper().Contains(keyWord));

                var hukouList = new List<HukouType> { };

                foreach (var item in hukouNameList)
                {
                    hukouList.Add((HukouType)(Enum.Parse(typeof(HukouType), item, true)));
                }
                //end Enum

                result = result.Where(a => a.Name.ToUpper().Contains(keyWord) || a.City.Name.ToUpper().Contains(keyWord) || a.Supplier.Name.ToUpper().Contains(keyWord) || a.AccumulationType.Name.ToUpper().Contains(keyWord) || hukouList.Contains(a.HukouType));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
        //end AccumulationRule

        public static List<PPUser> GetHRAdminList(string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetHRAdminQuery(db, keyWord, true).ToList();
            }
        }
        public static List<PPUser> GetHRList(string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetHRQuery(db, keyWord, true).ToList();
            }
        }
        public static IQueryable<PPUser> GetHRAdminQuery(UnitOfWork db, string keyWord = null, bool noTrack = false)
        {
            return GetRoleQuery(db, "HRAdmin", keyWord, noTrack);
        }
        public static IQueryable<PPUser> GetHRQuery(UnitOfWork db, string keyWord = null, bool noTrack = false)
        {
            return GetRoleQuery(db, "HR", keyWord, noTrack);
        }
        public static IQueryable<PPUser> GetRoleQuery(UnitOfWork db, string roleName, string keyWord = null, bool noTrack = false)
        {
            IQueryable<PPUser> result;

            var rep = db.PPUserRepository;

            result = rep.Get(noTrack);

            result = result.Where(a => a.Roles.Any(ab => ab.Role.Name == roleName));

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.UserName.ToUpper().Contains(keyWord));
            }

            return result;
        }
        public static string UploadImg(Controller controller, HttpPostedFileBase filebase, string path = "")
        {
            DateTime importNow = DateTime.Now;
            TimeSpan _TimeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            long importTime = (long)_TimeSpan.TotalMilliseconds;

            HttpPostedFileBase file = controller.Request.Files["files"];
            string FileName;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                return "文件不能为空";
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                int Maxsize = 5 * 1024 * 1024;//定义上传文件的最大空间大小为5M
                string FileType = ".xls,.xlsx,.png,.jpg,.jpeg,.pdf";//定义上传文件的类型字符串

                FileName = NoFileName + importNow.ToString("yyyyMMddhhmmss") + "_" + importTime + fileEx;
                if (!FileType.Contains(fileEx.ToLower()))
                {
                    return "文件类型不对，只能上传xls,xlsx,png,jpg,jpeg,pdf格式的文件";
                }
                if (filesize >= Maxsize)
                {
                    return "上传文件超过2M，不能上传";
                }
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Content/UploadedFolder/" + path + "/" + importNow.ToString("yyyyMMdd") + "/");
                string uploadPath = AppDomain.CurrentDomain.BaseDirectory + "Content/UploadedFolder/" + path + "/" + importNow.ToString("yyyyMMdd") + "/";
                savePath = Path.Combine(uploadPath, FileName);
                file.SaveAs(savePath);
                return "OK" + importNow.ToString("yyyyMMdd") + "/" + FileName;
            }
        }

        //带消息提示的返回索引页面
        public static void RMError(Controller controller, string msg = "权限范围内没有找到对应记录")
        {
            Msg message = new Msg { MsgType = MsgType.ERROR, Content = msg };
            controller.TempData["msg"] = message;
        }

        public static void RMOk(Controller controller, string msg = "操作成功!")
        {
            Msg message = new Msg { MsgType = MsgType.OK, Content = msg };
            controller.TempData["msg"] = message;
        }

        public static void RMWarn(Controller controller, string msg)
        {
            Msg message = new Msg { MsgType = MsgType.WARN, Content = msg };
            controller.TempData["msg"] = message;
        }
    }
}