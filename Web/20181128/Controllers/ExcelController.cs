using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRM.Web.Controllers
{
    public class ExcelController : Controller
    {
        //
        // GET: /Excel/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// excel文件下载
        /// </summary>
        /// <param name="guid">文件guid</param>
        /// <param name="name">文件名称</param>
        public ActionResult Down(string guid,string name)
        {
            string names = string.Format("{0}.xls", name);

            string url = "/upload/export/" + DateTime.Now.ToString("yyyyMM");

            if (!string.IsNullOrEmpty(url))
            {
                string path = HttpContext.Server.MapPath(string.Format("{0}/{1}.xls",url,guid));

                if (Request.Browser.Browser.ToString().Contains("IE"))
                {
                    names = Server.UrlEncode(names);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + names + "");
                }
                return File(path, "application/octet-stream", names);
            }
            return View();
        }

        /// <summary>
        /// excel文件下载
        /// </summary>
        /// <param name="name">模板文件真实文件名</param>
        /// <param name="displayname">模板文件显示的名称</param>
        public ActionResult DownTemplate(string name,string displayname)
        {
            string names = string.Format("{0}.xlsx", displayname);

            string url = "/upload/import/";

            if (!string.IsNullOrEmpty(url))
            {
                string path = HttpContext.Server.MapPath(string.Format("{0}/{1}.xlsx", url, name));

                if (Request.Browser.Browser.ToString().Contains("IE"))
                {
                    names = Server.UrlEncode(names);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + names + "");
                }
                return File(path, "application/octet-stream", names);
            }
            return View();
        }
    }
}
