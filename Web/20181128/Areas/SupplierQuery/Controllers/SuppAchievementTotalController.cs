//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-04    1.0        FJK        新建                   
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using SRM.Web.Controllers;
using System.Web.Mvc;
using SRM.Model.Supplier;
using SRM.BLL.Supplier;
using System;
using Aspose.Cells;
using System.IO;
using System.Data;
using SRM.BLL.Tra;
using SRM.Model.Tra;
#endregion
/*********************************
 * 类名：SuppAchievementTotalController
 * 功能描述：供应商绩效汇总 控制器 
 * ******************************/

namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class SuppAchievementTotalController : Controller
    {
        //
        // GET: /SupplierQuery/SuppAchievementTotal/

        #region 文件变量

        /// <summary>
        /// Aspose - Workbook
        /// </summary>
        private Workbook CurrentWorkbook;

        /// <summary>
        /// Worksheet
        /// </summary>
        private Worksheet DetailSheet;
        #endregion

        // 运输月度考核BLL
        TraMonthCheckBLL bll = new TraMonthCheckBLL();

        #region 页面

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="departmentName">部门名称</param> 
        /// <returns></returns>
        public ActionResult MonthCheckList(string departmentName, string year, string month)
        {
            // where条件
            string where = "";

            // 本公司ID
            int companyId = Auxiliary.CompanyID();

            // 部门名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%'", departmentName.Trim());
            }

            // 考核年
            if (!string.IsNullOrEmpty(year))
            {
                where += string.Format(" And CheckYear = {0}", year.Trim());
            }

            // 考核月
            if (!string.IsNullOrEmpty(month))
            {
                where += string.Format(" And CheckMonth = {0}", month.Trim());
            } 

            // 运输供应商数量汇总List
            List<TraMonthCheckModel> list = bll.MonthCheckList(where, companyId);
            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="departmentName">部门名称</param> 
        /// <returns></returns>
        public int MonthCheckAmount(string departmentName,string year, string month)
        {
            // where条件(本公司ID)
            string where = " CompanyId =" + Auxiliary.CompanyID();

            // 部门名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%'", departmentName.Trim());
            }

            // 考核年
            if (!string.IsNullOrEmpty(year))
            {
                where += string.Format(" And CheckYear = {0}", year.Trim());
            }

            // 考核月
            if (!string.IsNullOrEmpty(month))
            {
                where += string.Format(" And CheckMonth = {0}", month.Trim());
            }

            return bll.MonthCheckAmount(where);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string departmentName, string year, string month)
        {
            // where条件
            string where = "";

            // 本公司ID
            int companyId = Auxiliary.CompanyID();

            // 部门名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%'", departmentName.Trim());
            }

            // 考核年
            if (!string.IsNullOrEmpty(year))
            {
                where += string.Format(" And CheckYear = {0}", year.Trim());
            }

            // 考核月
            if (!string.IsNullOrEmpty(month))
            {
                where += string.Format(" And CheckMonth = {0}", month.Trim());
            }

            // 运输供应商数量汇总DataTable
            System.Data.DataTable dt = bll.ExportCheckTable(where, companyId);

            // guid
            string guid = string.Empty;

            // 模板文件路径
            string TemplatePath = System.Web.HttpContext.Current.Server.MapPath(@"/upload/export/Template/SuppAchievementTotal.xls");

            // 模板导出
            Auxiliary.TemplateExport(dt, TemplatePath, "A4", "SuppAchievementTotal", ref guid);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = guid });
        }

        #endregion
    }
}
