//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-09-10    1.0        zbb        新建               
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.Model.Tra;
using SRM.Web.Controllers;
using SRM.BLL.Tra;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：TraSupplierLayeredController
 * 功能描述：运输供应商分层 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraSupplierLayeredController : Controller
    {
        //运作要求维护
        TraSupplierLayeredBLL bll = new TraSupplierLayeredBLL();

        //
        // GET: /Tra/TraSupplierLayered/
        #region 页面

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns> 
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            TraSupplierLayeredModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            TraSupplierLayeredModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 运输供应商
        /// </summary> 
        public ActionResult TraSupplier(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// 供应商分层信息
        /// </summary> 
        public ActionResult SupplierLayered(string url)
        {
            ViewBag.url = url;
            return View();
        }

        #endregion

        #region 方法 

        #region 新增运输供应商分层
        /// <summary>
        /// 新增运输供应商分层
        /// </summary>
        /// <param name="model"></param> 
        /// <returns></returns>
        public ActionResult AddTraSupplierLayered(TraSupplierLayeredModel model)
        {

            model.State = 0;// 状态 未提交 

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            int LayeredId = bll.AddTraSupplierLayered(model);

            if (LayeredId > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        #endregion

        #region index数据集

        /// <summary>
        /// index数据集
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="layeredName">分层名称</param>
        /// <param name="beginTime">分层开始时间</param>
        /// <param name="endTime">分层结束时间</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult TraSupplierLayeredList(int index, int size, string supplierName, string layeredName, string beginTime, string endTime, string state)
        {
            string where = string.Format(" tsl.State != 10 and tsl.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And s.SupplierName  like '%{0}%' ", supplierName.Trim());
            }

            // 分层名称
            if (!string.IsNullOrEmpty(layeredName))
            {
                where += string.Format(" And sl.LayeredName  like '%{0}%' ", layeredName.Trim());
            }

            // 分层开始时间
            if (!string.IsNullOrEmpty(beginTime))
            {
                where += string.Format(" And convert(varchar,tsl.BeginTime,120) like '%{0}%'", beginTime.Trim());
            }

            // 分层结束时间
            if (!string.IsNullOrEmpty(endTime))
            {
                where += string.Format(" And convert(varchar,tsl.EndTime,120) like '%{0}%'", endTime.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And tsl.State = {0}", state.Trim());
            }

            List<TraSupplierLayeredModel> list = bll.TraSupplierLayeredList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region 数据记录数
        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="layeredName">分层名称</param>
        /// <param name="beginTime">分层开始时间</param>
        /// <param name="endTime">分层结束时间</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public int TraSupplierLayeredCount(string supplierName, string layeredName, string beginTime, string endTime, string state)
        {
            string where = string.Format(" tsl.State != 10 and tsl.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And s.SupplierName  like '%{0}%' ", supplierName.Trim());
            }

            // 分层名称
            if (!string.IsNullOrEmpty(layeredName))
            {
                where += string.Format(" And sl.LayeredName  like '%{0}%' ", layeredName.Trim());
            }

            // 分层开始时间
            if (!string.IsNullOrEmpty(beginTime))
            {
                where += string.Format(" And convert(varchar,tsl.BeginTime,120) like '%{0}%'", beginTime.Trim());
            }

            // 分层结束时间
            if (!string.IsNullOrEmpty(endTime))
            {
                where += string.Format(" And convert(varchar,tsl.EndTime,120) like '%{0}%'", endTime.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And tsl.State = {0}", state.Trim());
            }
            return bll.TraSupplierLayeredCount(where);
        }
        #endregion

        #region 编辑运输供应商分层
        /// <summary>
        /// 编辑运输供应商分层
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraSupplierLayered(TraSupplierLayeredModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            TraSupplierLayeredModel beforeModel = bll.GetModelByID(model.TraLayeredId);

            int result = bll.EditTraSupplierLayered(model);

            if (result > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 提交按钮逻辑

        /// <summary>
        /// 提交按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitTraSupplierLayered(int tId)
        {
            int row = bll.SubmitTraSupplierLayered(tId);

            TraSupplierLayeredModel beforeModel = bll.GetModelByID(tId);

            int transportid = beforeModel.TransportId;//运输供应商id

            DateTime begintime = beforeModel.BeginTime;//开始时间

            bll.UpdateisEndInfo(transportid,begintime);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail", content = "提交失败！" });
        }
        #endregion

        #region 作废按钮逻辑

        /// <summary>
        /// 作废按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            TraSupplierLayeredModel beforeModel = bll.GetModelByID(tId);

            int row = 0;//行数

            int delUserId = Auxiliary.UserID();//作废负责人

            row = bll.InvalidState(tId);//作废之后 状态变为删除状态

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion

        #region 导出按钮逻辑

        /// <summary>
        /// 导出按钮逻辑
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="layeredName">分层名称</param>
        /// <param name="beginTime">分层开始时间</param>
        /// <param name="endTime">分层结束时间</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string layeredName, string beginTime, string endTime, string state)
        {
            string where = string.Format(" tsl.State != 10 and tsl.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And s.SupplierName  like '%{0}%' ", supplierName.Trim());
            }

            // 分层名称
            if (!string.IsNullOrEmpty(layeredName))
            {
                where += string.Format(" And sl.LayeredName  like '%{0}%' ", layeredName.Trim());
            }

            // 分层开始时间
            if (!string.IsNullOrEmpty(beginTime))
            {
                where += string.Format(" And convert(varchar,tsl.BeginTime,120) like '%{0}%'", beginTime.Trim());
            }

            // 分层结束时间
            if (!string.IsNullOrEmpty(endTime))
            {
                where += string.Format(" And convert(varchar,tsl.EndTime,120) like '%{0}%'", endTime.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And tsl.State = {0}", state.Trim());
            }

            System.Data.DataTable dt = bll.ExportDataTable(where);
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Export, ResultEnum.Sucess, new
            {
                Type = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        #region 运输供应商列表

        /// <summary>
        /// 运输供应商列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public ActionResult TraSupplierList(int index, int size, string supplierName)
        {
            //查询本机内运作状态的运输供应商也不包含状态为非作废的未结束运输供应商
            string where = string.Format("  ST.TransportState='F4' AND ST.TransportId  not in (SELECT TransportId FROM TraSupplierLayered WHERE State!=10 AND IsEnd=0) AND S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            //供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            List<TraSupplierLayeredModel> list = bll.TraSupplierList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public int TraSupplierCount(string supplierName)
        {
            //查询本机内运作状态的运输供应商也不包含状态为非作废的未结束运输供应商
            string where = string.Format("  ST.TransportState='F4' AND ST.TransportId  not in (SELECT TransportId FROM TraSupplierLayered WHERE State!=10 AND IsEnd=0) AND S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName  like '%{0}%' ", supplierName.Trim());
            }

            return bll.TraSupplierCount(where);
        }

        #endregion


        #region 运输运作供应商列表

        /// <summary>
        /// 运输运作供应商列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="layeredName">分层名称</param>
        /// <returns></returns>
        public ActionResult SupplierLayeredList(int index, int size, string layeredName)
        {
            string where = string.Format("  SL.State=1 AND SL.CompanyId={0}", Auxiliary.DepartmentId());

            //供应商名称
            if (!string.IsNullOrEmpty(layeredName))
            {
                where += string.Format(" And SL.layeredName like '%{0}%'", layeredName.Trim());
            }

            List<TraSupplierLayeredModel> list = bll.SupplierLayeredList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="layeredName">分层名称</param>
        /// <returns></returns>
        public int SupplierLayeredCount(string layeredName)
        {
            string where = string.Format("  SL.State=1 AND SL.CompanyId={0}", Auxiliary.DepartmentId());

            if (!string.IsNullOrEmpty(layeredName))
            {
                where += string.Format(" And SL.layeredName  like '%{0}%' ", layeredName.Trim());
            }

            return bll.SupplierLayeredCount(where);
        }

        #endregion

        #endregion
    }
}
