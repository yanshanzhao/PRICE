//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-06    1.0        ZBB        新建                   
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
 * 类名：TraSuppMonthCheckController
 * 功能描述：运输供应商月度绩效查询 控制器 
 * ******************************/

namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class TraSuppMonthCheckController : Controller
    {
        //
        // GET: /SupplierQuery/TraSuppMonthCheck/
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

        public ActionResult Index()
        {
            return View();
        }

        #region 方法 运输供应商月度绩效查询

        #region 数据集 运输供应商月度绩效查询

        /// <summary>
        /// 数据集 运输供应商月度绩效查询
        /// </summary>
        /// <param name="supplierName"></param>
        /// <param name="checkFromType"></param>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public ActionResult TraSuppMonthCheckList(string supplierName, string checkFromType, string years, string months)
        {

            // where条件
            string where = "";

            // 本公司ID
            int companyId = Auxiliary.CompanyID();

            // 公司名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 月度考核自定义类型
            if (!string.IsNullOrEmpty(checkFromType))
            {
                where += string.Format(" And TMCF.CheckFromType like '%{0}%'", checkFromType.Trim());
            }

            // 考核年
            if (!string.IsNullOrEmpty(years))
            {
                where += string.Format(" And TMC.CheckYear = {0}", years.Trim());
            }

            // 考核月
            if (!string.IsNullOrEmpty(months))
            {
                where += string.Format(" And TMC.CheckMonth = {0}", months.Trim());
            }

            // 运输供应商数量汇总List 
            //List<TraMonthCheckModel> list = bll.TraSuppMonthCheckList(where, companyId);
            List<TraMonthCheckModel> list = bll.TraSuppMonthCheckList_2(where, companyId,Convert.ToInt32(years), Convert.ToInt32(months));
            return Json(list);
        }

        #endregion

        #region 数据记录数 运输供应商月度绩效查询

        /// <summary>
        /// 数据记录数 运输供应商月度绩效查询
        /// </summary>
        /// <param name="supplierName"></param>
        /// <param name="checkFromType"></param>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public int TraSuppMonthCheckAmount(string supplierName, string checkFromType, string years, string months)
        {
              
            // where条件
            string where = "";

            // 本公司ID
            int companyId = Auxiliary.CompanyID();

            // 公司名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 月度考核自定义类型
            if (!string.IsNullOrEmpty(checkFromType))
            {
                where += string.Format(" And TMCF.CheckFromType like '%{0}%'", checkFromType.Trim());
            }

            // 考核年
            if (!string.IsNullOrEmpty(years))
            {
                where += string.Format(" And TMC.CheckYear = {0}", years.Trim());
            }

            // 考核月
            if (!string.IsNullOrEmpty(months))
            {
                where += string.Format(" And TMC.CheckMonth = {0}", months.Trim());
            }

            //return bll.TraSuppMonthCheckAmount(where);
            return bll.TraSuppMonthCheckAmount_2(where, companyId, Convert.ToInt32(years), Convert.ToInt32(months));
        }
        #endregion

        #region 导出数据 运输供应商月度绩效查询

        /// <summary>
        /// 导出数据 运输供应商月度绩效查询
        /// </summary>
        /// <param name="supplierName"></param>
        /// <param name="checkFromType"></param>
        /// <param name="years"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult TraSuppMonthCheckExport(string supplierName, string checkFromType, string years, string months)
        {

            // where条件
            string where = "";

            // 本公司ID
            int companyId = Auxiliary.CompanyID();

            // 公司名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 月度考核自定义类型
            if (!string.IsNullOrEmpty(checkFromType))
            {
                where += string.Format(" And TMCF.CheckFromType like '%{0}%'", checkFromType.Trim());
            }

            // 考核年
            if (!string.IsNullOrEmpty(years))
            {
                where += string.Format(" And TMC.CheckYear = {0}", years.Trim());
            }

            // 考核月
            if (!string.IsNullOrEmpty(months))
            {
                where += string.Format(" And TMC.CheckMonth = {0}", months.Trim());
            }

            // 运输供应商数量汇总DataTable
            System.Data.DataTable dt = bll.TraSuppMonthCheckExportTable(where, companyId);

            // guid
            string guid = string.Empty;

            // 模板文件路径
            string TemplatePath = System.Web.HttpContext.Current.Server.MapPath(@"/upload/export/Template/TraSuppMonthCheck.xls");

            // 模板导出
            Auxiliary.TemplateExport(dt, TemplatePath, "A4", "TraSuppMonthCheck", ref guid);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = guid });
        }
        #endregion

        #endregion
    }
}
