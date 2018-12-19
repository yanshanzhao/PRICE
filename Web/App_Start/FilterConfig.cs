using System.Web;
using System.Web.Mvc;

namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filters.Add(new HandleErrorAttribute());
        }
    }
}