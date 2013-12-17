using OBEHR.Filters;
using System.Web;
using System.Web.Mvc;

namespace OBEHR
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new BasicAuthAttribute());
        }
    }
}
