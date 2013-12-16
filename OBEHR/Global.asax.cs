﻿using OBEHR.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OBEHR
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //建立数据库
            Database.SetInitializer<OBEHRContext>(new OBEHRInitializer());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
