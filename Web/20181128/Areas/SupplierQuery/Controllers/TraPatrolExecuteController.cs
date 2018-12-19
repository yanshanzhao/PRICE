//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-10    1.0        MY        新建   
//2018-11-28    1.0        HDS       修改bug           
//-------------------------------------------------------------------------
#region using
using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;

using SRM.BLL.Query;
using SRM.Model.Tra;
#endregion
/*********************************
 * 类名：TraYearCheckAdjunctController
 * 功能描述：运输年度考核附件 控制器 
 * ******************************/
namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    /// <summary>
    /// 巡查执行查询
    /// </summary>
    public class TraPatrolExecuteController : Controller
    {
        //
        // GET: /SupplierQuery/TraPatrolExecute/
        private QueryOperationClaimBLL  bll = new QueryOperationClaimBLL();

        #region 页面
        public ActionResult Index()
        {
            return View();
        }
        #endregion


        #region 方法

        /// <summary>
        /// 每页信息
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="departmentName">RDC名称</param>
        /// <param name="patrolNumber">巡查编号</param>
        /// <param name="patrolType">巡查类型</param>
        /// <param name="beginTime">巡查创建时间</param>
        /// <returns></returns>
        [HttpPost]
        [Operate(Name = OperateEnum.Search)]
        public ActionResult Index(int index,int size,string departmentName,string patrolNumber,string patrolType,string beginTime)
        {
            string where = string.Empty;
            where = "   SD.CompanyId = " + Auxiliary.CompanyID();

            //机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%'", departmentName.Trim());
            }

            //过程编号
            if (!string.IsNullOrEmpty(patrolNumber))
            {
                where += string.Format(" And TP.PatrolNumber like '%{0}%'", patrolNumber.Trim());
            }

            //过程类型
            if (!string.IsNullOrEmpty(patrolType))
            {
                where += string.Format(" And TP.PatrolType = '{0}'", patrolType.Trim());
            }

            //开始时间
            if (!string.IsNullOrEmpty(beginTime))
            {
                DateTime dt = Convert.ToDateTime(beginTime);
                where += string.Format(" And TP.CreateTime>='{0}' and TP.CreateTime<'{1}'", beginTime, dt.AddDays(1).Date);
            }

            List<TraPatrolExecuteModel> list = bll.TraPatrolExecuteList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }


        /// <summary>
        /// 数量汇总
        /// </summary>
        /// <param name="departmentName">RDC名称</param>
        /// <param name="patrolNumber">巡查编号</param>
        /// <param name="patrolType">巡查类型</param>
        /// <param name="beginTime">巡查创建时间</param>
        /// <returns></returns>
        [HttpPost]
        public int IndexAmount(string departmentName, string patrolNumber, string patrolType, string beginTime)
        {
            string where = string.Empty;
            where = "  SD.CompanyId = " + Auxiliary.CompanyID();

            //机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%'", departmentName.Trim());
            }

            //过程编号
            if (!string.IsNullOrEmpty(patrolNumber))
            {
                where += string.Format(" And TP.PatrolNumber like '%{0}%'", patrolNumber.Trim());
            }

            //过程类型
            if (!string.IsNullOrEmpty(patrolType))
            {
                where += string.Format(" And TP.PatrolType = '{0}'", patrolType.Trim());
            }

            //开始时间
            if (!string.IsNullOrEmpty(beginTime))
            {
                DateTime dt = Convert.ToDateTime(beginTime);
                where += string.Format(" And TP.CreateTime>='{0}' and TP.CreateTime<'{1}'", beginTime, dt.AddDays(1).Date);
            } 
            return bll.TraPatrolExecuteAmount(where);
        }
        
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="departmentName"></param>
        /// <param name="patrolNumber"></param>
        /// <param name="patrolType"></param>
        /// <param name="beginTime"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string departmentName, string patrolNumber, string patrolType, string beginTime)
        {

            // where条件
            string where = string.Empty;

            // 本公司ID
            where = "   SD.CompanyId = " + Auxiliary.CompanyID();

            //机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%'", departmentName.Trim());
            }

            //过程编号
            if (!string.IsNullOrEmpty(patrolNumber))
            {
                where += string.Format(" And TP.PatrolNumber like '%{0}%'", patrolNumber.Trim());
            }

            //过程类型
            if (!string.IsNullOrEmpty(patrolType))
            {
                where += string.Format(" And TP.PatrolType = '{0}'", patrolType.Trim());
            }

            //开始时间
            if (!string.IsNullOrEmpty(beginTime))
            {
                where += string.Format(" And convert(varchar,TPA.BeginTime,120) like '%{0}%'", beginTime.Trim());
            }


            // 巡查执行查询DataTable
            System.Data.DataTable dt = bll.ExportTotalTable(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt); 
            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }
        #endregion
    }
}
