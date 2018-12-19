//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-10    1.0        MY        新建              
//-------------------------------------------------------------------------
using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*********************************
 * 类名：SupplierCyclTotalController
 * 功能描述：供应商信息汇总 控制器 
 * ******************************/
namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class SupplierCyclTotalController : Controller
    {
        private BLL.Tra.BusinessQueryBLL bll = new BLL.Tra.BusinessQueryBLL();
        //
        // GET: /SupplierQuery/SupplierCyclTotal/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Operate(Name = OperateEnum.Search)]
        public ActionResult Index(int index, int size, string departmentName, string beginTime, string endTime)
        {
            string where = string.Empty;
            int companyId = Auxiliary.CompanyID();

            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%' ", departmentName.Trim());
            }

            if (!string.IsNullOrEmpty(beginTime))
            {
                where += string.Format(" And EndTime >= '{0}'", beginTime.Trim());
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                where += string.Format(" And EndTime <= '{0}' ", endTime.Trim());
            }

            List<Model.Supplier.SupplierCyclTotalModel> list = bll.GetSuppCyclTotalList(index, size, companyId, beginTime, endTime, where);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));

        }

        public ActionResult IndexAmount(string departmentName, string beginTime, string endTime)
        {
            string where = string.Empty;
            where = " And CompanyId = " + Auxiliary.CompanyID();
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%' ", departmentName.Trim());
            }

            if (!string.IsNullOrEmpty(beginTime))
            {
                where += string.Format(" And BeginTiem >= '{0}'", beginTime.Trim());
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                where += string.Format(" And EndTime <= '{0}' ", endTime.Trim());
            }

            int count = bll.GetSuppCyclTotalAmount(where);
            return Content(count.ToString());
        }


        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(int index, int size, string departmentName, string beginTime, string endTime)
        {

            // where条件
            string where = string.Empty;

            // 本公司ID
            int companyId = Auxiliary.CompanyID();

            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%' ", departmentName.Trim());
            }

            // 巡查执行查询DataTable
            System.Data.DataTable dt = bll.ExportSupplierCyclTable(index, size, companyId, beginTime, endTime, where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }

    }
}
