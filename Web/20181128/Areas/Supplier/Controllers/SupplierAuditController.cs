//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-15    1.0        FJK        新建          
//2018-06-22    1.1        MH        审核记录                   
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
 * 类名：SupplierAuditController
 * 功能描述：仓储供应商审核 控制器 
 * ******************************/

namespace SRM.Web.Areas.Supplier.Controllers
{
    public class SupplierAuditController : Controller
    {
        //
        // GET: /Supplier/SupplierAudit/

        SupplierAuditBLL bll = new SupplierAuditBLL();

        // 仓储供应商登记BLL
        SupplierStorageBLL ssbll = new SupplierStorageBLL();

        // 附件BLL
        SupplierAdjunctBLL sabll = new SupplierAdjunctBLL();

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
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.Check)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            SupplierAuditModel model = bll.GetModelByID(tId);

            // 附件
            List<temfile> filelist = sabll.SuppFileList(model.PresentId, 30);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);
            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();
            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            SupplierAuditModel model = bll.GetModelByID(tId);

            // 附件
            List<temfile> filelist = sabll.SuppFileList(model.PresentId, 30);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);
            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();
            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }
        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">显示条数</param>
        /// <param name="storageNumber">仓储供应商编号</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="presentDepartment">提交机构</param>
        /// <param name="presentTime">提交时间</param>
        /// <param name="state">审核状态</param>
        /// <returns></returns>
        public ActionResult SupplierAuditList(int index, int size, string storageNumber, string supplierName, string presentDepartmentId, string presentTime, string state)
        {
            // 查询审核人员为当前登录人且审核类型为仓储开发审核的审核信息
            string where = " SupplierAuditType = 1 AND AuditUserId = " + Auxiliary.UserID();

            // 仓储供应商编号
            if (!string.IsNullOrEmpty(storageNumber))
            {
                where += string.Format(" And StorageNumber like '%{0}%'", storageNumber.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 提交机构
            if (!string.IsNullOrEmpty(presentDepartmentId))
            {
                where += string.Format(" And PresentDepartmentId = {0}", presentDepartmentId.Trim());
            }

            // 提交时间
            if (!string.IsNullOrEmpty(presentTime))
            {
                where += string.Format(" And convert(varchar,PresentTime,120) like '%{0}%'", presentTime.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And DA.State = {0}", state.Trim());
            }

            // 供应商审核List
            List<SupplierAuditModel> list = bll.SupplierAuditList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="storageNumber">仓储供应商编号</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="presentDepartment">提交机构</param>
        /// <param name="presentTime">提交时间</param>
        /// <param name="state">审核状态</param>
        /// <returns></returns>
        public int SupplierAuditAmount(string storageNumber, string supplierName, string presentDepartmentId, string presentTime, string state)
        {
            // 查询审核人员为当前登录人且审核类型为仓储开发审核的审核信息
            string where = " SupplierAuditType = 1 AND AuditUserId = " + Auxiliary.UserID();

            // 仓储供应商编号
            if (!string.IsNullOrEmpty(storageNumber))
            {
                where += string.Format(" And StorageNumber like '%{0}%'", storageNumber.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 提交机构
            if (!string.IsNullOrEmpty(presentDepartmentId))
            {
                where += string.Format(" And PresentDepartmentId = {0}", presentDepartmentId.Trim());
            }

            // 提交时间
            if (!string.IsNullOrEmpty(presentTime))
            {
                where += string.Format(" And convert(varchar,PresentTime,120) like '%{0}%'", presentTime.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And DA.State = {0}", state.Trim());
            }

            return bll.SupplierAuditAmount(where);
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <returns></returns>
        public ActionResult ThroughAudit(SupplierAuditModel tModel)
        {
            // 审核前Model
            SupplierAuditModel beforeModel = bll.GetModelByID(tModel.SupplierAuditId);

            // 审核流程Model
            BasisAuditRelationModel BARmodel = Auxiliary.GetAuditRelationById(beforeModel.AuditRelationId);

            // 判断是否有下一级审核
            BasisAuditRelationModel model = Auxiliary.IsRelationByBeforeId(beforeModel.AuditRelationId);

            // 若有 新增审核信息
            if (model != null)
            {
                // 修改审核记录状态(审核通过 1)
                bll.ChangeState(tModel.SupplierAuditId, 1, tModel.AuditRemark);

                SupplierAuditModel Auditmodel = new SupplierAuditModel();
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

                // 默认状态 未审核
                Auditmodel.State = 0;

                // 上一审核ID
                Auditmodel.BeforeId = tModel.SupplierAuditId;

                SupplierAuditBLL auditBLL = new SupplierAuditBLL();
                auditBLL.AddAuditRelation(Auditmodel);
                 
                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "审核通过", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });
                return Json(new { flag = "success", content = "审核通过！" });
            }
            else
            {
                // 当前审核流程是否为结束审核流程
                // 若否
                if (BARmodel.EndAudit == 0)
                {
                    // 供应商日志
                    Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Fail, new { Type = "审核流程不完善", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                    return Json(new { flag = "fail", content = "无下一级审核人，请完善审核流程！" });
                }
                else if (BARmodel.EndAudit == 1)
                { 
                    // 修改审核记录状态(审核通过 1)
                    bll.ChangeState(tModel.SupplierAuditId, 1, tModel.AuditRemark);

                    // 若是 修改仓储供应商登记表仓储状态(合格 F2)
                    ssbll.ChangeStorageState(tModel.PresentId, "F2");

                    // 供应商日志
                    Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "审核通过", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });
                    return Json(new { flag = "success", content = "审核通过！" });
                }
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Fail, new
            {
                Type = "审核失败",
                Id = tModel.SupplierAuditId,
                AuditRelationNumber = beforeModel.AuditRelationNumber,
                AuditRelationName = beforeModel.AuditRelationName
            });

            return Json(new { flag = "fail", content = "审核失败！" });
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <returns></returns>
        public ActionResult RejectAudit(SupplierAuditModel tModel)
        {
            // 审核前Model
            SupplierAuditModel beforeModel = bll.GetModelByID(tModel.SupplierAuditId);

            // 修改审核记录状态(驳回 4)
            int row = bll.ChangeState(tModel.SupplierAuditId, 4, tModel.AuditRemark);

            if (row > 0)
            {
                // 驳回后根据审核流程ID查询审核流程Model 获取退回类型 
                BasisAuditRelationModel model = Auxiliary.GetAuditRelationById(beforeModel.AuditRelationId);

                // 退回类型为 无 0 或者 结束审核 3
                // 更改本数据状态为驳回,登记表状态不合格 
                if (model.ReturnType == 0 || model.ReturnType == 3)
                {
                    // 修改仓储供应商登记表仓储状态(不合格 F3)
                    ssbll.ChangeStorageState(tModel.PresentId, "F3");
                }
                else if (model.ReturnType == 1)
                {
                    // 退回类型为退回申请人 1
                    // 更改本数据状态为驳回,登记表提交状态为已退回 
                    ssbll.ChangeState(tModel.PresentId, 10);
                }
                else if (model.ReturnType == 2)
                {
                    // 退回类型为退回上一个人 2 
                }

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Reject, ResultEnum.Sucess, new { Type = "审核驳回", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                return Json(new { flag = "success", content = "审核驳回！" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Reject, ResultEnum.Fail, new { Type = "驳回失败", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

            return Json(new { flag = "fail", content = "驳回失败！" });
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <returns></returns>
        [Operate(Name = OperateEnum.Revoke)]
        public ActionResult RevokeAudit(int tId)
        {
            // 撤销前Model
            SupplierAuditModel beforeModel = bll.GetModelByID(tId);

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
                SupplierAuditModel NextModel = bll.GetNextAuditModel(tId);

                // 若存在下一级审核 下一级审核状态是否为未审核
                if (NextModel != null)
                {
                    // 若否 不允许撤销(提示 下一流程已进行审核操作)
                    if (NextModel.State != 0)
                    {
                        // 供应商日志
                        Auxiliary.SupplierCustomLog(OperateEnum.Revoke, ResultEnum.Fail, new { Type = "撤销失败", Id = tId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

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
                            Auxiliary.SupplierCustomLog(OperateEnum.Revoke, ResultEnum.Sucess, new { Type = "撤销成功", Id = tId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

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
                        Auxiliary.SupplierCustomLog(OperateEnum.Revoke, ResultEnum.Sucess, new { Type = "撤销成功", Id = tId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                        return Json(new { flag = "success", content = "撤销成功！" });
                    }
                }

            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Revoke, ResultEnum.Fail, new { Type = "撤销失败", Id = tId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

            return Json(new { flag = "fail", content = "撤销失败！" });
        }
        #endregion

        #region 审核记录

        /// <summary>
        /// 审核记录
        /// </summary>
        /// <param name="suppid">运输或仓储供应商id</param>
        /// <param name="audittype">审核类型</param>
        /// <returns></returns>
        public ActionResult LogList(int suppid, int audittype)
        {
            ViewBag.suppid = suppid;
            ViewBag.audittype = audittype;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suppid"></param>
        /// <param name="audittype"></param>
        /// <returns></returns>
        public ActionResult LogLists(int suppid, int audittype)
        {
            return Json(new SupplierAuditsBLL().SupplierAuditList(suppid, audittype));
        }
        #endregion
    }
}
