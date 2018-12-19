using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class TraSuppYearCheckController : Controller
    {
        private BLL.Tra.BusinessQueryBLL bll = new BLL.Tra.BusinessQueryBLL();
        //
        // GET: /SupplierQuery/TraSuppYearCheck/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Operate(Name = OperateEnum.Search)]
        public ActionResult Index(int index,int size,string suppName,string years)
        {
            string where = string.Empty;
            where = " And SD.CompanyId = " + Auxiliary.CompanyID();
            if (!string.IsNullOrEmpty(suppName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%' ", suppName.Trim());
            }

            if (!string.IsNullOrEmpty(years))
            {
                where += string.Format(" And TYCR.CheckYear = {0} ", years.Trim());
            }

            List<Model.Tra.TraSuppYearCheckModel> list = bll.GetTraSuppYearCheckList(index, size, where);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        public ActionResult IndexAmount(string suppName, string years)
        {
            int count = 0;
            string where = string.Empty;
            where = " And SD.CompanyId = " + Auxiliary.CompanyID();

            if (!string.IsNullOrEmpty(suppName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%' ", suppName.Trim());
            }

            if (!string.IsNullOrEmpty(years))
            {
                where += string.Format(" And TYCR.CheckYear = {0} ", years.Trim());
            }

            count = bll.TraSuppYearCheckAmount(where);

            return Content(count.ToString());
        }


        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string suppName, string years)
        {

            // where条件
            string where = string.Empty;

            // 本公司ID
            where = " And SD.CompanyId = " + Auxiliary.CompanyID();

            if (!string.IsNullOrEmpty(suppName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%' ", suppName.Trim());
            }

            if (!string.IsNullOrEmpty(years))
            {
                where += string.Format(" And TYCR.CheckYear = {0} ", years.Trim());
            }


            // 巡查执行查询DataTable
            System.Data.DataTable dt = bll.ExportTraSuppYearCheckTable(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            //// guid
            //string guid = string.Empty;

            //// 模板文件路径
            //string TemplatePath = System.Web.HttpContext.Current.Server.MapPath(@"/upload/export/Template/TraPatrolExecute.xls");

            //// 模板导出
            //Auxiliary.TemplateExport(dt, TemplatePath, "A4", "TraPatrolExecute", ref guid);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }

    }
}
