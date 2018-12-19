//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-22    1.0        zbb        新建               
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
 * 类名：TraSupplierNotificationController
 * 功能描述：供应商招标 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraSupplierNotificationController : Controller
    {
        //供应商招标
        TraSupplierNotificationBLL bll = new TraSupplierNotificationBLL();

        //供应商招标附件
        TraSupplierNotificationAdjunctBLL tcbll = new TraSupplierNotificationAdjunctBLL();
        //

        // GET: /Tra/TraSupplierNotification/

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
            TraSupplierNotificationModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfileSuppNotification> filelist = tcbll.SuppNotificationFileList(model.SuppNotificationId);
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
            TraSupplierNotificationModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfileSuppNotification> filelist = tcbll.SuppNotificationFileList(model.SuppNotificationId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 运作要求信息
        /// </summary>
        public ActionResult TraSuppLierInfo(string url)
        {
            ViewBag.url = url;
            return View();
        }

        #endregion

        #region 方法 

        #region 新增 供应商招标

        /// <summary>
        /// 新增 供应商招标
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddTraSupplierNotification(TraSupplierNotificationModel model)
        {

            model.State = 0;// 状态：0-初始；1-提交；10-作废;  

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            int SuppNotificationId = bll.AddTraSupplierNotification(model);

            if (SuppNotificationId > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppNotificationFileList))
                {
                    List<temfileSuppNotification> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfileSuppNotification>>(model.SuppNotificationFileList);
                    tcbll.AddFilesForSupplier(fflist, SuppNotificationId, ref filenames);
                }
                model.SuppNotificationFileList = filenames;

                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        #endregion

        #region 数据集 供应商招标

        /// <summary>
        /// 数据集 供应商招标
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="invitationName">招标名称</param>
        /// <param name="notificationLines">招标路线</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult TraSupplierNotificationList(int index, int size, string supplierName, string invitationName, string notificationLines, string state)
        {
            string where = string.Format(" TSN.State != 10 and TSN.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And s.SupplierName  LIKE '%{0}%'", supplierName.Trim());
            }

            // 招标名称
            if (!string.IsNullOrEmpty(invitationName))
            {
                where += string.Format(" And TN.InvitationName  LIKE '%{0}%'", invitationName.Trim());
            }

            // 招标路线
            if (!string.IsNullOrEmpty(notificationLines))
            {
                where += string.Format(" And TN.NotificationLines  LIKE '%{0}%'", notificationLines.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TSN.State = {0}", state.Trim());
            }

            List<TraSupplierNotificationModel> list = bll.TraSupplierNotificationList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region 数据记录数 供应商招标

        /// <summary>
        /// 数据记录数 供应商招标
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="invitationName">招标名称</param>
        /// <param name="notificationLines">招标路线</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public int TraSupplierNotificationCount(string supplierName, string invitationName, string notificationLines, string state)
        {
            string where = string.Format(" TSN.State != 10 and TSN.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And s.SupplierName  LIKE '%{0}%'", supplierName.Trim());
            }

            // 招标名称
            if (!string.IsNullOrEmpty(invitationName))
            {
                where += string.Format(" And TN.InvitationName  LIKE '%{0}%'", invitationName.Trim());
            }

            // 招标路线
            if (!string.IsNullOrEmpty(notificationLines))
            {
                where += string.Format(" And TN.NotificationLines  LIKE '%{0}%'", notificationLines.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TSN.State = {0}", state.Trim());
            }

            return bll.TraSupplierNotificationCount(where);
        }
        #endregion

        #region 编辑 供应商招标

        /// <summary>
        /// 编辑 供应商招标
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraSupplierNotification(TraSupplierNotificationModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            TraSupplierNotificationModel beforeModel = bll.GetModelByID(model.SuppNotificationId);

            //修改 招标通知表
            int result = bll.EditTraSupplierNotification(model);

            if (result > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppNotificationFileList))
                {
                    List<temfileSuppNotification> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfileSuppNotification>>(model.SuppNotificationFileList);
                    tcbll.AddFilesForSupplier(fflist, model.SuppNotificationId, ref filenames);
                }
                model.SuppNotificationFileList = filenames;


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
        public ActionResult SubmitTraSupplierNotification(int tId)
        {
            int row = bll.SubmitTraSupplierNotification(tId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail", content = "提交失败！" });
        }
        #endregion

        #region 作废 供应商招标

        /// <summary>
        /// 作废 供应商招标
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            TraSupplierNotificationModel beforeModel = bll.GetModelByID(tId);

            int row = 0;

            int delUserId = Auxiliary.UserID();

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

        #region 导出 供应商招标

        /// <summary>
        /// 导出 招标通知表
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="invitationName">招标名称</param>
        /// <param name="notificationLines">招标路线</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string invitationName, string notificationLines, string state)
        {
            string where = string.Format(" TSN.State != 10 and TSN.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And s.SupplierName  LIKE '%{0}%'", supplierName.Trim());
            }

            // 招标名称
            if (!string.IsNullOrEmpty(invitationName))
            {
                where += string.Format(" And TN.InvitationName  LIKE '%{0}%'", invitationName.Trim());
            }

            // 招标路线
            if (!string.IsNullOrEmpty(notificationLines))
            {
                where += string.Format(" And TN.NotificationLines  LIKE '%{0}%'", notificationLines.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TSN.State = {0}", state.Trim());
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
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public ActionResult TraSupplierInfoList(int index, int size, string supplierName)
        {
            string where = string.Format(" TN.NotificationState=1  And TN.CreateDepartmentId={0} And TC.CompanyId={1}", Auxiliary.DepartmentId(), Auxiliary.CompanyID());

            //供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            List<TraSupplierNotificationModel> list = bll.TraSupplierInfoList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 点击申请按钮弹出的运输供应商申请信息 数据记录数

        /// <summary>
        /// 点击申请按钮弹出的运输供应商申请信息 数据记录数
        /// </summary> 
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public int TraSupplierInfoCount(string supplierName)
        {
            string where = string.Format(" TN.NotificationState=1  And TN.CreateDepartmentId={0} And TC.CompanyId={1}", Auxiliary.DepartmentId(), Auxiliary.CompanyID());

            //供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            return bll.TraSupplierInfoCount(where);
        }
        #endregion

        #endregion

    }
}
