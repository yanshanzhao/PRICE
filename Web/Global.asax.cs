using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using BLL.Sys;
using Model.Sys;
using Web.Controllers;
namespace Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            BundleTable.EnableOptimizations = true;//是否启用优化
            BundleTable.Bundles.IgnoreList.Clear();
            BundleTable.Bundles.IgnoreList.Ignore(".min.js", OptimizationMode.Always);
        }

        void Application_Error(object sender, EventArgs e)
        {
            SysLogBLL bll = new SysLogBLL();

            Model.Sys.SysLogModel model = new Model.Sys.SysLogModel();
            model.Message = string.Empty;
            try
            {
                model.CompanyId = Auxiliary.CompanyID();
                model.Operator = Auxiliary.GetUserName();
            }
            catch(Exception exs)
            {
                model.Message = exs.Message.ToString();
              //  model.CompanyId = 3;
                model.Operator = "System";
            }
            model.CreateTime = DateTime.Now;

            Exception ex = Server.GetLastError();

            model.Message = string.Format("{0}\r\n{1}\r\n{2}",ex.Source,ex.Message.ToString(),ex.StackTrace.ToString());
            model.Module = "exception";
            model.Type = "";
            model.Result = string.Empty;


            bll.AddSysLog(model);         
        }
    }
}