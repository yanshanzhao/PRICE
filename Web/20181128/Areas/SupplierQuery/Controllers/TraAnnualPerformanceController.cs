//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-22    1.0        FJK        新建 - 年度绩效     
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.Model.Supplier;
using System.Linq;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using SRM.BLL.Supplier;
#endregion
/*********************************
 * 类名：TraMeasuresAndSelfEvaluation
 * 功能描述：运输年度绩效表 控制器 
 * ******************************/

namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class TraAnnualPerformanceController : Controller
    {
        //
        // GET: /SupplierQuery/TraAnnualPerformance/

        // 年度绩效BLL
        TraYearCheckResultBLL bll = new TraYearCheckResultBLL();

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
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierType">供应商类型</param>
        /// <param name="abnormityState">异常整改记录状态</param>
        /// <returns>Json</returns>
        public ActionResult AnnualPerformanceList(int index, int size, string tYear)
        {
            // 查询本账户的运输供应商的考核结果
            string where = " UserId =" + Auxiliary.UserID();

            // 异常整改记录状态
            if (!string.IsNullOrEmpty(tYear))
            {
                where += string.Format(" AND CheckYear = {0}", tYear.Trim());
            }

            // 年度绩效List
            List<TraYearCheckResultModel> list =  bll.AnnualPerformanceList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierNumber">供应商编号</param>
        /// <param name="supplierType">供应商类型</param>
        /// <returns></returns>
        public int AnnualPerformanceCount(string tYear)
        {
            // 查询本账户的运输供应商的考核结果
            string where = " UserId =" + Auxiliary.UserID();

            // 异常整改记录状态
            if (!string.IsNullOrEmpty(tYear))
            {
                where += string.Format(" AND CheckYear = {0}", tYear.Trim());
            }

            return bll.AnnualPerformanceCount(where);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult AnnualPerformanceExport(string tYear)
        {
            // 查询本账户的运输供应商的考核结果
            string where = " UserId =" + Auxiliary.UserID();

            // 异常整改记录状态
            if (!string.IsNullOrEmpty(tYear))
            {
                where += string.Format(" AND CheckYear = {0}", tYear.Trim());
            }

            // 运输供应商数量汇总DataTable
            System.Data.DataTable dt = bll.AnnualPerformanceExport(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new
            {
                Detail = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });
            return Json(new { flag = "success", guid = url });
        }

        #endregion
    }
}
