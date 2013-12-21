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

namespace OBEHR.Lib
{
    public class Common<Model> where Model : class
    {
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
        public static List<Model> GetList(bool includeSoftDeleted = false, string filter = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetQuery(db, includeSoftDeleted, filter, true).ToList();
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
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
            }
            return result;
        }
    }

    public class ClientBaseCommon<Model> where Model : ClientBaseModel
    {
        //query and list
        public static List<Model> GetList(bool includeSoftDeleted = false, string filter = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetQuery(db, includeSoftDeleted, filter, true).ToList();
            }
        }
        public static List<Model> GetClientList(int clientId, bool includeSoftDeleted = false, string filter = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetClientQuery(db, clientId, includeSoftDeleted, filter, true).ToList();
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
        public static List<ClientCity> GetList(bool includeSoftDeleted = false, string filter = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetQuery(db, includeSoftDeleted, filter, true).ToList();
            }
        }
        public static IQueryable<ClientCity> GetQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<ClientCity> result;

            var rep = db.ClientCityRepository;

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
        public static List<Model> GetList(bool includeSoftDeleted = false, string filter = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetQuery(db, includeSoftDeleted, filter, true).ToList();
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
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord) || a.ClientCity.Client.Name.ToUpper().Contains(keyWord) || a.ClientCity.City.Name.ToUpper().Contains(keyWord));
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
        public static List<PPUser> GetHRAdminList(bool includeSoftDeleted = false, string keyWord = null)
        {
            using (var db = new UnitOfWork())
            {
                return GetHRAdminQuery(db, includeSoftDeleted, keyWord, true).ToList();
            }
        }
        public static IQueryable<PPUser> GetHRAdminQuery(UnitOfWork db, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            return GetRoleQuery(db, "HRAdmin", includeSoftDeleted, keyWord, noTrack);
        }
        public static IQueryable<PPUser> GetRoleQuery(UnitOfWork db, string roleName, bool includeSoftDeleted = false, string keyWord = null, bool noTrack = false)
        {
            IQueryable<PPUser> result;

            var rep = db.PPUserRepository;

            result = rep.Get(noTrack);

            result = result.Where(a => a.ApplicationUser.Roles.Any(ab => ab.Role.Name == roleName));

            if (!String.IsNullOrWhiteSpace(keyWord))
            {
                keyWord = keyWord.ToUpper();
                result = result.Where(a => a.Name.ToUpper().Contains(keyWord));
            }

            if (!includeSoftDeleted)
            {
                result = result.Where(a => a.IsDeleted == false);
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