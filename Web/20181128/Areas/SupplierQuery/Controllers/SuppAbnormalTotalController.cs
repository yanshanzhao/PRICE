//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-20    1.0        FJK        新建 -  供应商异常信息汇总                
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
using SRM.Model.Tra;
using SRM.BLL.Tra;
#endregion
/*********************************
 * 类名：SuppAbnormalTotalController
 * 功能描述：供应商异常信息汇总 控制器 
 * ******************************/

namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class SuppAbnormalTotalController : Controller
    {
        //
        // GET: /SupplierQuery/SuppAbnormalTotal/

        // 运输供应商BLL
        TraWorkingBLL bll = new TraWorkingBLL();

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
        public ActionResult SuppAbnormalTotalList(string supplierName,string startTime,string endTime)
        {
            // where条件
            string where = " CompanyId =" + Auxiliary.CompanyID();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运作时间 
            if (!string.IsNullOrEmpty(startTime))
            {
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += string.Format(" And WorkingTime BETWEEN '{0}' AND '{1}' ", startTime.Trim(), endTime.Trim());
                }
            }

            // 运输供应商数量汇总List
            List<TraWorkingModel> list = bll.SuppAbnormalTotalList(where);
            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="departmentName">部门名称</param> 
        /// <returns></returns>
        public int SuppAbnormalTotalAmount(string supplierName, string startTime, string endTime)
        {
            // where条件
            string where = " TW.State = 5 AND TW.CompanyId =" + Auxiliary.CompanyID();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运作时间 
            if (!string.IsNullOrEmpty(startTime))
            {
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += string.Format(" And WorkingTime BETWEEN '{0}' AND '{1}' ", startTime.Trim(), endTime.Trim());
                }
            }

            return bll.SuppAbnormalTotalAmount(where);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string startTime, string endTime)
        {
            // where条件
            string where = " CompanyId =" + Auxiliary.CompanyID();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运作时间 
            if (!string.IsNullOrEmpty(startTime))
            {
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += string.Format(" And WorkingTime BETWEEN '{0}' AND '{1}' ", startTime.Trim(), endTime.Trim());
                }
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportTotalTable(where);

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
