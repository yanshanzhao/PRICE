//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-10    1.0        MY        新建      
//2018-11-23    1.0        HDS       新增一列（列结余）        
//-------------------------------------------------------------------------
#region using
using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.BLL.Query;
#endregion
/*********************************
 * 类名：DistributionInfoController
 * 功能描述：配送人员信息汇总 控制器 
 * ******************************/

namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class DistributionInfoController : Controller
    {
        #region 变量
        //配送人员信息汇总Bll
        private QueryTraDistributorBLL bll = new QueryTraDistributorBLL();
        #endregion

        //
        // GET: /SupplierQuery/DistributionInfo/

        #region 页面        
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 方法 
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">每页行数</param>
        /// <param name="departmentName">RDC名称</param>
        /// <param name="suppName">运输供应商名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpPost]
        [Operate(Name = OperateEnum.Search)]
        public ActionResult Index(int index, int size, string departmentName, string suppName, string startTime, string endTime)
        {
            //查询条件
            string where = "  WHERE CompanyId = " + Auxiliary.CompanyID();

            //结余查询条件
            string balanceWhere = "  WHERE CompanyId = " + Auxiliary.CompanyID();

            //RDC名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%' ", departmentName.Trim());
                balanceWhere += string.Format(" And DepartmentName like '%{0}%' ", departmentName.Trim());
            }
            //运输供应商名称
            if (!string.IsNullOrEmpty(suppName))
            {
                where += string.Format(" And SupplierName like '%{0}%' ", suppName.Trim());
                balanceWhere += string.Format(" And SupplierName like '%{0}%' ", departmentName.Trim());
            }

            //汇总开始时间
            if (!string.IsNullOrEmpty(startTime))
            {
                where += string.Format(" And submitTime >='{0}' ", startTime.Trim());
                balanceWhere += string.Format(" And submitTime <'{0}' ", startTime.Trim());
            }

            //汇总结束时间
            if (!string.IsNullOrEmpty(endTime))
            {
                DateTime dt = Convert.ToDateTime(endTime); 
                where += string.Format(" And submitTime <'{0}' ", dt.AddDays(1).Date.ToString()); 
            }

            //获取分页列表信息
            List<Model.Tra.TraDistributorTotalModel> list = bll.GetDistributorQuery(index, size, balanceWhere, where); 

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 分页总共数量
        /// </summary>
        /// <param name="departmentName">RDC名称</param>
        /// <param name="suppName">运输供应商名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public ActionResult IndexAmount(string departmentName, string suppName, string startTime, string endTime)
        {
            // 总行数
            int rowCount = 0;
            //查询条件
            string where = " WHERE CompanyId = " + Auxiliary.CompanyID();

            //结余查询条件
            string balanceWhere = "  WHERE CompanyId = " + Auxiliary.CompanyID();

            //RDC名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%' ", departmentName.Trim());
                balanceWhere += string.Format(" And DepartmentName like '%{0}%' ", departmentName.Trim());
            }
            //运输供应商名称
            if (!string.IsNullOrEmpty(suppName))
            {
                where += string.Format(" And SupplierName like '%{0}%' ", suppName.Trim());
                balanceWhere += string.Format(" And SupplierName like '%{0}%' ", departmentName.Trim());
            }

            //汇总开始时间
            if (!string.IsNullOrEmpty(startTime))
            {
                where += string.Format(" And submitTime >='{0}' ", startTime.Trim());
                balanceWhere += string.Format(" And submitTime <'{0}' ", startTime.Trim());
            }

            //汇总结束时间
            if (!string.IsNullOrEmpty(endTime))
            {
                DateTime dt = Convert.ToDateTime(endTime);
                where += string.Format(" And submitTime <'{0}' ", dt.AddDays(1).Date.ToString());
            }
            //获取分页总共数量
            rowCount = bll.GetDistributorQueryAmount(balanceWhere, where);

            return Content(rowCount.ToString());
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="departmentName">RDC名称</param>
        /// <param name="suppName">运输供应商名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string departmentName, string suppName, string startTime, string endTime)
        {
            //查询条件
            string where = " WHERE  CompanyId = " + Auxiliary.CompanyID();

            //结余查询条件
            string balanceWhere = " WHERE  CompanyId = " + Auxiliary.CompanyID();

            //RDC名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%' ", departmentName.Trim());
                balanceWhere += string.Format(" And DepartmentName like '%{0}%' ", departmentName.Trim());
            }
            //运输供应商名称
            if (!string.IsNullOrEmpty(suppName))
            {
                where += string.Format(" And SupplierName like '%{0}%' ", suppName.Trim());
                balanceWhere += string.Format(" And SupplierName like '%{0}%' ", departmentName.Trim());
            }

            //汇总开始时间
            if (!string.IsNullOrEmpty(startTime))
            {
                where += string.Format(" And submitTime >='{0}' ", startTime.Trim());
                balanceWhere += string.Format(" And submitTime <'{0}' ", startTime.Trim());
            }

            //汇总结束时间
            if (!string.IsNullOrEmpty(endTime))
            {
                DateTime endTimes = Convert.ToDateTime(endTime);
                where += string.Format(" And submitTime <'{0}' ", endTimes.AddDays(1).Date.ToString());
            }


            // 配送人员信息查询DataTable
            System.Data.DataTable table = bll.ExportDistributionInfoTable(balanceWhere, where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(table);
             
            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }
        #endregion
    }
}
