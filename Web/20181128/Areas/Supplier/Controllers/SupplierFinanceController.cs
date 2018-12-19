//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-20    1.0        FJK        新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.Model.Storage;
using SRM.Model.Supplier;
using SRM.BLL.Supplier;
using System.Linq;
#endregion
/*********************************
 * 类名：SupplierFinanceController
 * 功能描述：供应商财务信息表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Supplier.Controllers
{
    public class SupplierFinanceController : Controller
    {
        //
        // GET: /Supplier/SupplierFinance/

        // 供应商财务信息BLL
        SupplierFinanceBLL bll = new SupplierFinanceBLL();

        // 运输供应商BLL
        SupplierTransportBLL STbll = new SupplierTransportBLL();

        // 供应商附件BLL
        SupplierAdjunctBLL SAbll = new SupplierAdjunctBLL();

        // 供应商BLL
        SupplierBLL basebll = new SupplierBLL();

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
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId, int tSupplierId, int tIsFinance)
        {
            // 判断此运输供应商是否有财务信息
            SupplierFinanceModel model = bll.GetModelByID(tId, tSupplierId);

            if (model.FatherId == 0)
            {
                ViewBag.pname = "无";
            }
            else
            {
                SupplierModel pmodel = basebll.GetModelByID(model.FatherId.ToString());

                if (pmodel != null)
                {
                    ViewBag.pname = pmodel.SupplierName;
                }
                else
                {
                    ViewBag.pname = "无";
                }
            }

            List<temfile> filelist = SAbll.SuppFileList(tId, 20);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);
             
            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId, int tSupplierId, int tIsFinance)
        {
            ViewBag.IsFinance = tIsFinance;

            SupplierFinanceModel model = new SupplierFinanceModel();
             
            if (tIsFinance == 0)
            {
                model = bll.GetModelByTransportID(tId); 
            }
            else
            {
                model = bll.GetModelByID(tId, tSupplierId);
            }
             
            if (model.FatherId == 0)
            {
                ViewBag.pname = "无";
            }
            else
            {
                SupplierModel pmodel = basebll.GetModelByID(model.FatherId.ToString());

                if (pmodel != null)
                {
                    ViewBag.pname = pmodel.SupplierName;
                }
                else
                {
                    ViewBag.pname = "无";
                }
            }

            List<temfile> filelist = SAbll.SuppFileList(tId, 20);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="transportNumber">运输供应商编号</param>
        /// <param name="supplierName">供应商名称</param> 
        /// <param name="transportKind">供应商种类</param>
        /// <param name="distributionScope">配送供应商范围</param>
        /// <param name="supplierLevel">供应商级别</param>
        /// <param name="transportState">供应商状态</param>
        /// <param name="state">提交状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public ActionResult SupplierTransportList(int index, int size, string transportNumber, string supplierName, string transportKind, string distributionScope, string supplierLevel, string state, string transportState, string startTime, string endTime)
        {
            // 本机构 运输供应商状态为合格状态和运作状态
            string where = " ts.DepartmentId =" + Auxiliary.DepartmentId() + " AND (TransportState ='F2' OR TransportState ='F4')";

            // 运输供应商编号
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And ts.TransportNumber like '%{0}%'", transportNumber.Trim());
            }

            // 供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 供应商种类 
            if (!string.IsNullOrEmpty(transportKind))
            {
                where += string.Format(" And ts.TransportKind like '%{0}%'", transportKind.Trim());
            }

            // 配送供应商范围
            if (!string.IsNullOrEmpty(distributionScope))
            {
                where += string.Format(" And ts.DistributionScope like '%{0}%'", distributionScope.Trim());
            }

            // 供应商级别
            if (!string.IsNullOrEmpty(supplierLevel))
            {
                where += string.Format(" And ts.SupplierLevel like '%{0}%'", supplierLevel.Trim());
            }

            // 供应商状态
            if (!string.IsNullOrEmpty(transportState))
            {
                where += string.Format(" And ts.TransportState = '{0}'", transportState.Trim());
            }
            else
            {
                where += string.Format(" AND (TransportState ='F2' OR TransportState ='F4')");
            }

            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And ts.State like '%{0}%'", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(startTime))
            {
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += string.Format(" And ts.CretaeTime BETWEEN '{0}' AND '{1}' ", startTime.Trim(), endTime.Trim());
                }
            }

            // 运输供应商List
            List<SupplierTransportModel> list = STbll.SupplierTransportPageList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="departmentName">部门名称</param> 
        /// <returns></returns>
        public int SupplierTransportAmount(string transportNumber, string supplierName, string transportKind, string distributionScope, string supplierLevel, string state, string transportState, string startTime, string endTime)
        {
            // 本机构 运输供应商状态为合格状态和运作状态
            string where = " ts.DepartmentId =" + Auxiliary.DepartmentId() + " AND (TransportState ='F2' OR TransportState ='F4')";

            // 运输供应商编号
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And ts.TransportNumber like '%{0}%'", transportNumber.Trim());
            }

            // 供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 供应商种类 
            if (!string.IsNullOrEmpty(transportKind))
            {
                where += string.Format(" And ts.TransportKind like '%{0}%'", transportKind.Trim());
            }

            // 配送供应商范围
            if (!string.IsNullOrEmpty(distributionScope))
            {
                where += string.Format(" And ts.DistributionScope like '%{0}%'", distributionScope.Trim());
            }

            // 供应商级别
            if (!string.IsNullOrEmpty(supplierLevel))
            {
                where += string.Format(" And ts.SupplierLevel like '%{0}%'", supplierLevel.Trim());
            }

            // 供应商状态
            if (!string.IsNullOrEmpty(transportState))
            {
                where += string.Format(" And ts.TransportState = '{0}'", transportState.Trim());
            }
            else
            {
                where += string.Format(" AND (TransportState ='F2' OR TransportState ='F4')");
            }

            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And ts.State like '%{0}%'", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(startTime))
            {
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += string.Format(" And ts.CretaeTime BETWEEN '{0}' AND '{1}' ", startTime.Trim(), endTime.Trim());
                }
            }

            return STbll.SupplierTransportPageCount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditSupplierFinance(SupplierFinanceModel tModel)
        {
            // 编辑之前Model
            SupplierFinanceModel beforeModel = bll.GetModelByID(tModel.OtherId, tModel.SupplierId);

            // 编辑
            int row = bll.EditSupplierFinance(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 修改运输供应商表IsFinance
                bll.UpdateIsFinance(tModel.OtherId);

                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(tModel.SuppFileList))
                {
                    List<temfile> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.SuppFileList);
                    SAbll.AddFiles(fflist, tModel.OtherId, 20, ref filenames);
                }

                //记录文件列表日志
                tModel.SuppFileList = filenames;

                // 供应商日志
                Auxiliary.SupplierLog<SupplierFinanceModel>(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel);
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierLog<SupplierFinanceModel>(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string transportNumber, string supplierName, string transportKind, string distributionScope, string supplierLevel, string state, string transportState, string startTime, string endTime)
        {
            // 本公司 运输供应商状态为合格状态和运作状态
            string where = " ts.CompanyId =" + Auxiliary.CompanyID() + " AND (TransportState ='F2' OR TransportState ='F4')";

            // 运输供应商编号
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And ts.TransportNumber like '%{0}%'", transportNumber.Trim());
            }

            // 供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 供应商种类 
            if (!string.IsNullOrEmpty(transportKind))
            {
                where += string.Format(" And ts.TransportKind like '%{0}%'", transportKind.Trim());
            }

            // 配送供应商范围
            if (!string.IsNullOrEmpty(distributionScope))
            {
                where += string.Format(" And ts.DistributionScope like '%{0}%'", distributionScope.Trim());
            }

            // 供应商级别
            if (!string.IsNullOrEmpty(supplierLevel))
            {
                where += string.Format(" And ts.SupplierLevel like '%{0}%'", supplierLevel.Trim());
            }

            // 供应商状态
            if (!string.IsNullOrEmpty(transportState))
            {
                where += string.Format(" And ts.TransportState = '{0}'", transportState.Trim());
            }
            else
            {
                where += string.Format(" AND (TransportState ='F2' OR TransportState ='F4')");
            }

            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And ts.State like '%{0}%'", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(startTime))
            {
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += string.Format(" And ts.CretaeTime BETWEEN '{0}' AND '{1}' ", startTime.Trim(), endTime.Trim());
                }
            }

            // DataTable
            System.Data.DataTable dt = STbll.ExportDataTable(where, Auxiliary.CompanyID());

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
