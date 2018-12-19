//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-09    1.0        FJK        新建                 
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
 * 类名：TraMonthCheckFromDeparController
 * 功能描述：月度考核自定义机构表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraMonthCheckFromDeparController : Controller
    {
        //
        // GET: /Tra/TraMonthCheckFromDepar/

        // 月度考核自定义机构BLL
        TraMonthCheckFromDeparBLL bll = new TraMonthCheckFromDeparBLL();

        #region 页面

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns> 
        // [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }
        #endregion

        #region 方法

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddMonthCheckFromDepar(TraMonthCheckFromDeparModel tModel)
        {
            // 新增(返回主键ID)
            int CheckFromId = bll.AddMonthCheckFromDepar(tModel);

            // 若主键>O(新增成功)
            if (CheckFromId > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddTransportSupplierList(string transportSupplierList, int checkFromId)
        {

            List<TraMonthCheckFromDeparModel> TransportSupplierList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TraMonthCheckFromDeparModel>>(transportSupplierList);
             
            // 新增(返回主键ID)
            int CheckFromId = bll.AddTransportSupplierList(TransportSupplierList, checkFromId);
             
            TraMonthCheckFromDeparModel afterModel = bll.GetModelByID(CheckFromId);
             
            // 若主键>O(新增成功)
            if (CheckFromId > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, afterModel);
                return Json(new { flag = "success", content = "分配成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, afterModel);
            return Json(new { flag = "fail", content = "分配失败！" });
        }

        #endregion

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="checkFromId">月度考核自定义id</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierType">供应商类型</param> 
        /// <param name="supplierScope">供应商范围</param> 
        /// <returns>Json</returns>
        public ActionResult MonthCheckFromDeparList(int index, int size, string checkFromId, string supplierName, string supplierType, string supplierScope)
        {
            // 有效的
            string where = " TMCFD.State = 1 AND TMCFD.DepartmentId=" + Auxiliary.DepartmentId();

            // 月度考核自定义id 
            if (!string.IsNullOrEmpty(checkFromId))
            {
                where += string.Format(" And CheckFromId = {0}", checkFromId.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 供应商类型  
            if (!string.IsNullOrEmpty(supplierType))
            {
                where += string.Format(" And TransportKind = {0}", supplierType.Trim());
            }

            // 供应商范围 
            if (!string.IsNullOrEmpty(supplierScope))
            {
                where += string.Format(" And DistributionScope = {0}", supplierScope.Trim());
            }
             
            // 运输月度绩效自定义List
            List<TraMonthCheckFromDeparModel> list = bll.MonthCheckFromDeparList(index, size, where); 
            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierType">供应商类型</param> 
        /// <param name="supplierScope">供应商范围</param> 
        /// <returns></returns>
        public int MonthCheckFromDeparAmount(string checkFromId, string supplierName, string supplierType, string supplierScope)
        {
            // 有效的
            string where = " TMCFD.State = 1 AND TMCFD.DepartmentId=" + Auxiliary.DepartmentId();

            // 月度考核自定义id 
            if (!string.IsNullOrEmpty(checkFromId))
            {
                where += string.Format(" And CheckFromId = {0}", checkFromId.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 供应商类型  
            if (!string.IsNullOrEmpty(supplierType))
            {
                where += string.Format(" And TransportKind = {0}", supplierType.Trim());
            }

            // 供应商范围 
            if (!string.IsNullOrEmpty(supplierScope))
            {
                where += string.Format(" And DistributionScope = {0}", supplierScope.Trim());
            }

            return bll.MonthCheckFromDeparAmount(where);
        }
         
        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            // 作废之前Model
            TraMonthCheckFromDeparModel beforeModel = bll.GetModelByID(tId);
             
            // 作废(更改状态)
            int row = bll.InvalidState(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败" });
        }

        #region 运输供应商

        /// <summary>
        /// 分页列表 供应商信息
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public ActionResult SupplierList(int index, int size,string checkFromId, string supplierName,string supplierType, string supplierScope)
        {
            string where = " TransportState = 'F4' AND ST.DepartmentId=" + Auxiliary.DepartmentId();

            // 供应商名
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 自定义表ID
            if (!string.IsNullOrEmpty(checkFromId))
            {
                where += string.Format(" And TransportId NOT IN (SELECT TransportId FROM TraMonthCheckFromDepar WHERE CheckFromId = {0} AND State =1)", checkFromId.Trim());
            }
             
            // 供应商类型  
            if (!string.IsNullOrEmpty(supplierType))
            {
                where += string.Format(" And TransportKind = {0}", supplierType.Trim());
            }

            // 供应商范围 
            if (!string.IsNullOrEmpty(supplierScope))
            {
                where += string.Format(" And DistributionScope = {0}", supplierScope.Trim());
            }

            List<SupplierTransportModel> list = bll.SupplierList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 分页总数 供应商信息
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns> 
        public int SupplierAmount(string checkFromId, string supplierName, string supplierType, string supplierScope)
        {
            string where = " TransportState = 'F4' AND ST.DepartmentId=" + Auxiliary.DepartmentId();

            // 供应商名
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 自定义表ID
            if (!string.IsNullOrEmpty(checkFromId))
            {
                where += string.Format(" And TransportId NOT IN (SELECT TransportId FROM TraMonthCheckFromDepar WHERE CheckFromId = {0} AND State =1)", checkFromId.Trim());
            }

            // 供应商类型  
            if (!string.IsNullOrEmpty(supplierType))
            {
                where += string.Format(" And TransportKind = {0}", supplierType.Trim());
            }

            // 供应商范围 
            if (!string.IsNullOrEmpty(supplierScope))
            {
                where += string.Format(" And DistributionScope = {0}", supplierScope.Trim());
            }

            return bll.SupplierAmount(where);
        }
        #endregion
        #endregion
    }
}
