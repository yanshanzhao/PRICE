//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-20    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.BLL.Storage;
using SRM.Model.Storage;
using SRM.Model.Supplier;
using SRM.BLL.Supplier;
#endregion
/*********************************
 * 类名：StorageChooseAuditController
 * 功能描述：仓储选择审核表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Storage.Controllers
{
    public class StorageChooseAuditController : Controller
    {
        //
        // GET: /Storage/StorageChooseAudit/

        //仓储供应商审核BLL
        StorageChooseAuditBLL bll = new StorageChooseAuditBLL();

        // 仓储供应商申请BLL
        StorageChooseBLL sbl = new StorageChooseBLL();


        #region 页面

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.UserId = Auxiliary.UserID();
            return View();
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Check)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            StorageChooseAuditModel model = bll.GetModelByID(tId);
            return View(model);
        }

        /// <summary>
        /// View
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            StorageChooseAuditModel model = bll.GetModelByID(tId);
            return View(model);
        }
        #endregion

        #region 方法

        #region 仓储选择审核列表

        /// <summary>
        /// 仓储选择审核列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="CreateDepartmentId"></param>
        /// <param name="ApplyTime"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult SupplierChooseAuditList(int index, int size, string CreateDepartmentId, string ApplyTime, string state)
        {
            // 查询审核人员为本人且审核类型为仓储选择审核的审核信息
            string where = " SA.SupplierAuditType=2  and  SA.AuditUserId = " + Auxiliary.UserID();

            if (!string.IsNullOrEmpty(CreateDepartmentId))
            {
                where += string.Format(" And SCH.CreateDepartmentId = {0}", CreateDepartmentId.Trim());
            }

            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,sch.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And  SA.State = {0}", state.Trim());
            }

            List<StorageChooseAuditModel> list = bll.SupplierChooseAuditList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region 仓储选择审核 数据记录数

        /// <summary>
        /// 仓储选择审核 数据记录数
        /// </summary>
        /// <param name="CreateDepartmentId"></param>
        /// <param name="ApplyTime"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int SupplierChooseAuditAmount(string CreateDepartmentId, string ApplyTime, string state)
        {
            // 查询审核人员为本人且审核类型为仓储选择审核的审核信息
            string where = " SA.SupplierAuditType=2  and  SA.AuditUserId = " + Auxiliary.UserID();

            if (!string.IsNullOrEmpty(CreateDepartmentId))
            {
                where += string.Format(" And SCH.CreateDepartmentId = {0}", CreateDepartmentId.Trim());
            }

            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,sch.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And  SA.State = {0}", state.Trim());
            }

            return bll.SupplierChooseAuditAmount(where);
        }
        #endregion

        #region 审核通过

        /// <summary>
        /// 审核通过
        /// <param name="tModel"></param>
        /// </summary>
        /// <returns></returns>
        public ActionResult ThroughAudit(StorageChooseAuditModel tModel)
        {
            // 审核前Model
            StorageChooseAuditModel beforeModel = bll.GetModelByID(tModel.SupplierAuditId);

            // 审核流程Model
            BasisAuditRelationModel BARmodel = Auxiliary.GetAuditRelationById(beforeModel.AuditRelationId);

            // 判断是否有下一级审核
            BasisAuditRelationModel model = Auxiliary.IsRelationByBeforeId(beforeModel.AuditRelationId);

            // 若有 新增审核信息
            if (model != null)
            {

                // 修改审核记录状态(审核通过 1)
                bll.ChangeState(tModel.SupplierAuditId, 1, tModel.AuditRemark);

                StorageChooseAuditModel Auditmodel = new StorageChooseAuditModel();
                Auditmodel.AuditRelationNumber = model.AuditRelationNumber;
                Auditmodel.AuditRelationId = model.AuditRelationId;
                Auditmodel.OtherId = beforeModel.OtherId;
                Auditmodel.PresentId = beforeModel.PresentId;
                Auditmodel.SupplierAuditType = beforeModel.SupplierAuditType;
                Auditmodel.PresentDepartmentId = model.DepartmentId;
                Auditmodel.PresentUserId = model.UserId;
                Auditmodel.AuditDepartmentId = model.ToDepartmentId;
                Auditmodel.AuditUserId = model.ToUserId;
                Auditmodel.AuditRelationName = model.AuditRelationName;
                Auditmodel.CompanyId = model.CompanyId;
                Auditmodel.State = 0;// 默认状态 未审核
                Auditmodel.BeforeId = tModel.SupplierAuditId;// 上一审核ID

                StorageChooseAuditBLL auditBLL = new StorageChooseAuditBLL();
                auditBLL.AddAuditRelation(Auditmodel);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "审核通过", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });
                return Json(new { flag = "success", content = "审核通过！" });
            }
            else
            {
                // 当前审核流程是否为结束审核流程
                // 若否
                if (BARmodel.EndAudit == 0)
                {
                    // 供应商日志
                    Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "审核流程不完善", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                    return Json(new { flag = "fail", content = "无下一级审核人，请完善审核流程！" });
                }
                else if (BARmodel.EndAudit == 1)
                {
                    // 修改审核记录状态(审核通过 1)
                    bll.ChangeState(tModel.SupplierAuditId, 1, tModel.AuditRemark);

                    //将仓储供应商选择申请表中的状态修改为审核通过
                    sbl.StorageChooseState(tModel.PresentId, 5);

                    //修改仓储供应商登记表仓储状态(代运作 F2)
                    sbl.ChangeSuppStorageState(tModel.PresentId);

                    // 供应商日志
                    Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "审核通过", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });
                    return Json(new { flag = "success", content = "审核通过！" });
                }
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new
            {
                Type = "审核失败",
                Id = tModel.SupplierAuditId,
                AuditRelationNumber = beforeModel.AuditRelationNumber,
                AuditRelationName = beforeModel.AuditRelationName
            });

            return Json(new { flag = "fail", content = "审核失败！" });
        }
        #endregion

        #region 驳回按钮

        /// <summary>
        /// 驳回按钮
        /// <param name="model"></param>
        /// </summary>
        /// <returns></returns>
        public ActionResult RejectAudit(StorageChooseAuditModel model)
        {
            // 审核前Model
            StorageChooseAuditModel beforeModel = bll.GetModelByID(model.SupplierAuditId);

            // 修改审核记录状态(驳回 4)
            int row = bll.ChangeState(model.SupplierAuditId, 4, model.AuditRemark);

            if (row > 0)
            {
                // 更改本数据状态为驳回,登记表提交状态为已退回 
                sbl.ChangeState(model.PresentId, 10);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "审核驳回", Id = model.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                return Json(new { flag = "success", content = "审核驳回！" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "驳回失败", Id = model.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

            return Json(new { flag = "fail", content = "驳回失败！" });
        }
        #endregion

        #region 撤销按钮

        /// <summary>
        /// 撤销按钮
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="tPresentId"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Revoke)]
        public ActionResult RevokeAudit(int tId, int tPresentId)
        {
            // 撤销前Model
            StorageChooseAuditModel beforeModel = bll.GetModelByID(tId);

            // 获取审核流程
            BasisAuditRelationModel model = Auxiliary.GetAuditRelationById(beforeModel.AuditRelationId);

            // 撤销前判断此审核记录是否是结束审核流程 
            // 若是结束审核流程 不允许撤销
            if (model.EndAudit == 1)
            {
                return Json(new { flag = "fail", content = "此审核流程为结束审核流程,不允许撤销！" });
            }
            else if (model.EndAudit == 0)
            {
                // 若否 查询是否有下一级审核 
                StorageChooseAuditModel NextModel = bll.GetNextAuditModel(tId);

                // 若存在下一级审核 下一级审核状态是否为未审核
                if (NextModel != null)
                {
                    // 若否 不允许撤销(提示 下一流程已进行审核操作)
                    if (NextModel.State != 0)
                    {
                        // 供应商日志
                        Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "撤销失败", Id = tId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                        return Json(new { flag = "fail", content = "此审核流程的下一级审核流程已进行审核操作,不允许撤销！" });
                    }
                    else
                    {
                        // 若是未审核 将下一流程审核状态改为撤销状态
                        bll.ChangeState(NextModel.SupplierAuditId, 10, "");

                        // 更改本数据审核状态为未审核状态,清除本数据审核信息
                        int row = bll.ChangeState(tId, 0, "");
                        if (row > 0)
                        {
                            // 供应商日志
                            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "撤销成功", Id = tId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                            return Json(new { flag = "success", content = "撤销成功！" });
                        }
                    }
                }
                else
                {
                    // 若不存在下一级审核 更改本数据审核状态为未审核状态,清除本数据审核信息
                    int row = bll.ChangeState(tId, 0, "");
                    if (row > 0)
                    {
                        // 供应商日志
                        Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "撤销成功", Id = tId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                        return Json(new { flag = "success", content = "撤销成功！" });
                    }
                }

            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });
            return Json(new { flag = "fail", content = "撤销失败！" });
        }
        #endregion

        #region 仓储选择审核最下方列表

        /// <summary>
        /// 仓储选择审核最下方列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="supplierAuditId"></param>
        /// <returns></returns>
        public ActionResult StorageChooseAuditInfoList(int index, int size, string supplierAuditId)
        {

            string where = string.Format(" suppAud.SupplierAuditId in ({0})", supplierAuditId);

            List<StorageChooseAuditModel> list = bll.StorageChooseAuditInfoList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 仓储选择审核最下方列表 最新

        /// <summary>
        /// 仓储选择审核最下方列表 最新
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="supplierAuditId"></param>
        /// <returns></returns>
        public ActionResult StorageChooseAuditInfoLists(int index, int size, string supplierAuditId)
        {

            string where = string.Format(" suppAud.SupplierAuditId in ({0})", supplierAuditId);

            List<StorageChooseAuditModel> list = bll.StorageChooseAuditInfoLists(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 仓储选择审核最下方的数据行数

        /// <summary>
        /// 数据列表
        /// </summary>
        /// <param name="supplierAuditId"></param>
        /// <returns></returns>
        public int StorageChooseAuditInfoAmount(string supplierAuditId)
        {

            string where = string.Format(" suppAud.SupplierAuditId in ({0})", supplierAuditId);

            return bll.StorageChooseAuditInfoAmount(where);
        }
        #endregion

        #region 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult StorageChooseAuditdetaileditList(int index, int size, string supplierAuditId)
        {

            string where = string.Format(" DA.SupplierAuditId in ({0})", supplierAuditId);

            List<StorageChooseAuditModel> list = bll.StorageChooseAuditdetaileditList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 编辑按钮时：显示下方的仓储选择明细数据记录count 

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="supplierAuditId"></param> 
        /// <returns></returns>
        public int StorageChooseAuditdetaileditAmount(string supplierAuditId)
        {
            string where = string.Format(" DA.SupplierAuditId in ({0})", supplierAuditId);

            return bll.StorageChooseAuditdetaileditAmount(where);
        }
        #endregion

        #region 撤销按钮
        /// <summary>
        /// 审核不通过
        /// </summary>
        /// <returns></returns>
        public ActionResult FailAudit(StorageChooseAuditModel tModel)
        {
            // 审核前Model
            StorageChooseAuditModel beforeModel = bll.GetModelByID(tModel.SupplierAuditId);

            // 修改审核记录状态(驳回 4)
            int row = bll.ChangeState(tModel.SupplierAuditId, 3, tModel.AuditRemark);

            if (row > 0)
            {
                sbl.ChangeState(tModel.PresentId, 8);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "审核不通过", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                return Json(new { flag = "success", content = "审核不通过！" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "审核不通过", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

            return Json(new { flag = "fail", content = "审核失败！" });
        }
        #endregion

        #endregion

    }
}
