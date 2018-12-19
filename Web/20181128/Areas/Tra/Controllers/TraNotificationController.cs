//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-20    1.0        zbb        新建               
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
using SRM.BLL.Sys;
using Newtonsoft.Json.Converters;
using SRM.Model.Sys;
#endregion
/*********************************
 * 类名：TraNotificationController
 * 功能描述：招标通知 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraNotificationController : Controller
    {

        //运作要求维护
        TraNotificationBLL bll = new TraNotificationBLL();

        //运作要求维护
        SysCompanyBLL SBbll = new SysCompanyBLL();

        //运作附件
        TraNotificationDetailBLL tcbll = new TraNotificationDetailBLL();

        //
        // GET: /Tra/TraNotification/

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
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        { 
            ViewBag.NotificationNumber = Auxiliary.CurCompanyAutoNum("ZB");

            // 根据公司ID获取MODEL
            SysCompanyModel model = SBbll.GetModelByID(Auxiliary.CompanyID().ToString()); 

            ViewBag.Expertise = model.Expertise;

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
            TraNotificationModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfileNotification> filelist = tcbll.NotificationFileList(model.NotificationId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);
            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            TraNotificationModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfileNotification> filelist = tcbll.NotificationFileList(model.NotificationId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 运作要求信息
        /// </summary>
        public ActionResult TraSuppChooseInfo(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult EditNotificationInfo(string url,int tId)
        {

            ViewBag.url = url;

            // 获取数据
            TraNotificationModel model = bll.GetNotificationByID(tId);

            return View(model);
        }

        #endregion

        #region 方法 

        #region 新增 招标通知表
        /// <summary>
        /// 新增 招标通知表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddTraNotification(TraNotificationModel model)
        {

            model.NotifyState = 0;// 通知状态:0-未通知;1-已通知; 

            model.NotificationState = 0;// 0-申请招标；1-招标中；10-招标结束;20-作废 

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            int OperationClaimId = bll.AddTraNotification(model);

            if (OperationClaimId > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.NotificationFileList))
                {
                    List<temfileNotification> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfileNotification>>(model.NotificationFileList);
                    tcbll.AddFilesForSupplier(fflist, OperationClaimId, ref filenames);
                }
                model.NotificationFileList = filenames;

                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        #endregion

        #region 数据集 招标通知表

        /// <summary>
        /// 数据集 招标通知表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="traChooseNumber">运输选择编号</param>
        /// <param name="createDepartmentId">申请机构id</param>
        /// <param name="applyTime">申请时间</param>
        /// <param name="notificationLines">招标路线</param>
        /// <returns></returns>
        public ActionResult TraNotificationList(int index, int size, string traChooseNumber, string createDepartmentId, string applyTime, string notificationLines)
        {
            string where = string.Format(" TN.NotificationState != 20 and TN.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 运输选择编号
            if (!string.IsNullOrEmpty(traChooseNumber))
            {
                where += string.Format(" And TC.TraChooseNumber  LIKE '%{0}%'", traChooseNumber.Trim());
            }

            // 申请机构id
            if (!string.IsNullOrEmpty(createDepartmentId))
            {
                where += string.Format(" And TC.CreateDepartmentId = {0}", createDepartmentId.Trim());
            }

            // 申请时间
            if (!string.IsNullOrEmpty(applyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", applyTime.Trim());
            }

            // 招标路线
            if (!string.IsNullOrEmpty(notificationLines))
            {
                where += string.Format(" And TN.NotificationLines  LIKE '%{0}%'", notificationLines.Trim());
            } 

            List<TraNotificationModel> list = bll.TraNotificationList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region 数据记录数 招标通知表

        /// <summary>
        /// 数据记录数 招标通知表
        /// </summary>
        /// <param name="traChooseNumber">运输选择编号</param>
        /// <param name="createDepartmentId">申请机构id</param>
        /// <param name="applyTime">申请时间</param>
        /// <param name="notificationLines">招标路线</param> 
        /// <returns></returns>
        public int TraNotificationCount(string traChooseNumber, string createDepartmentId, string applyTime, string notificationLines)
        {
            string where = string.Format(" TN.NotificationState != 20 and TN.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 运输选择编号
            if (!string.IsNullOrEmpty(traChooseNumber))
            {
                where += string.Format(" And TC.TraChooseNumber  LIKE '%{0}%'", traChooseNumber.Trim());
            }

            // 申请机构id
            if (!string.IsNullOrEmpty(createDepartmentId))
            {
                where += string.Format(" And TC.CreateDepartmentId = {0}", createDepartmentId.Trim());
            }

            // 申请时间
            if (!string.IsNullOrEmpty(applyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", applyTime.Trim());
            }

            // 招标路线
            if (!string.IsNullOrEmpty(notificationLines))
            {
                where += string.Format(" And TN.NotificationLines  LIKE '%{0}%'", notificationLines.Trim());
            }

            return bll.TraNotificationCount(where);
        }
        #endregion

        #region 提交按钮逻辑操作

        /// <summary>
        /// 提交按钮逻辑操作
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitTraNotification(int tId)
        {
            int row = bll.SubmitTraNotification(tId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail", content = "提交失败！" });
        }
        #endregion

        #region 编辑 招标通知表

        /// <summary>
        /// 编辑 招标通知表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraNotification(TraNotificationModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            TraNotificationModel beforeModel = bll.GetModelByID(model.NotificationId);

            int result = bll.EditTraNotification(model);

            if (result > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.NotificationFileList))
                {
                    List<temfileNotification> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfileNotification>>(model.NotificationFileList);
                    tcbll.AddFilesForSupplier(fflist, model.NotificationId, ref filenames);
                }
                model.NotificationFileList = filenames;


                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 作废 招标通知表

        /// <summary>
        /// 作废 招标通知表
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            TraNotificationModel beforeModel = bll.GetModelByID(tId);

            int row = 0;

            //将申请表中的招标状态修改为初始状态
            bll.TraChooseState(beforeModel.TraChooseId);

            int delUserId = Auxiliary.UserID();//作废负责人id

            row = bll.InvalidState(tId, delUserId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion

        #region 结束按钮 招标通知表

        /// <summary>
        /// 结束按钮 招标通知表
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.End)]
        public ActionResult EndTraNotification(int tId)
        {
            TraNotificationModel beforeModel = bll.GetModelByID(tId);

            int row = 0;

            //将申请表中的招标状态修改为结束状态
            bll.TraChooseEndState(beforeModel.TraChooseId);

            int delUserId = Auxiliary.UserID();

            row = bll.EndTraNotification(tId, delUserId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.End, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "结束成功！" });
            }
            Auxiliary.Log(OperateEnum.End, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "结束失败！" });
        }
        #endregion

        #region 导出 招标通知表

        /// <summary>
        /// 导出 招标通知表
        /// </summary>
        /// <param name="traChooseNumber">运输选择编号</param>
        /// <param name="createDepartmentId">申请机构id</param>
        /// <param name="applyTime">申请时间</param>
        /// <param name="notificationLines">招标路线</param> 
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string traChooseNumber, string createDepartmentId, string applyTime, string notificationLines)
        {
            string where = string.Format(" TN.NotificationState != 20 and TN.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 运输选择编号
            if (!string.IsNullOrEmpty(traChooseNumber))
            {
                where += string.Format(" And TC.TraChooseNumber  LIKE '%{0}%'", traChooseNumber.Trim());
            }

            // 申请机构id
            if (!string.IsNullOrEmpty(createDepartmentId))
            {
                where += string.Format(" And TC.CreateDepartmentId = {0}", createDepartmentId.Trim());
            }

            // 申请时间
            if (!string.IsNullOrEmpty(applyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", applyTime.Trim());
            }

            // 招标路线
            if (!string.IsNullOrEmpty(notificationLines))
            {
                where += string.Format(" And TN.NotificationLines  LIKE '%{0}%'", notificationLines.Trim());
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

        #region 点击申请按钮弹出的运输供应商申请信息 数据列表

        /// <summary>
        /// 点击申请按钮弹出的运输供应商申请信息 数据列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="traChooseNumber">运输选择编号</param>
        /// <returns></returns>
        public ActionResult TraSuppChooseInfoList(int index, int size, string traChooseNumber)
        {
            string where = string.Format(" TC.UseState=5 and TC.NotificationType=0 and TC.IsNotification=1  And TC.CreateDepartmentId={0} And TC.CompanyId={1}", Auxiliary.DepartmentId(),Auxiliary.CompanyID());

            //运输选择编号
            if (!string.IsNullOrEmpty(traChooseNumber))
            {
                where += string.Format(" And TC.TraChooseNumber like '%{0}%'", traChooseNumber.Trim());
            }

            List<TraNotificationModel> list = bll.TraSuppChooseInfoList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 点击申请按钮弹出的运输供应商申请信息 数据记录数

        /// <summary>
        /// 点击申请按钮弹出的运输供应商申请信息 数据记录数
        /// </summary> 
        /// <param name="traChooseNumber">运输选择编号</param>
        /// <returns></returns>
        public int TraSuppChooseInfoCount(string traChooseNumber)
        {
            string where = string.Format(" TC.UseState=5 and TC.NotificationType=0 and TC.IsNotification=1  And TC.CreateDepartmentId={0} And TC.CompanyId={1}", Auxiliary.DepartmentId(), Auxiliary.CompanyID());

            //运输选择编号
            if (!string.IsNullOrEmpty(traChooseNumber))
            {
                where += string.Format(" And TC.TraChooseNumber like '%{0}%'", traChooseNumber.Trim());
            }

            return bll.TraSuppChooseInfoCount(where);
        }
        #endregion

        #region 编辑 招标通知表
        /// <summary>
        /// 编辑 招标通知表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraNotificationInfo(TraNotificationModel model)
        { 

            TraNotificationModel beforeModel = bll.GetNotificationByID(model.NotificationId); 

            int result = bll.EditTraNotificationInfo(model);

            if (result > 0)
            {
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #endregion

    }
}
