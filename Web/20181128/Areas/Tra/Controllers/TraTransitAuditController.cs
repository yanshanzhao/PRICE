//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-24    1.0        FJK        新建           
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
using Newtonsoft.Json.Converters;
using SRM.Model.Basis;
#endregion
/*********************************
 * 类名：TraTransitAuditController
 * 功能描述：供应商运作审核 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraTransitAuditController : Controller
    {
        //
        // GET: /Tra/TraTransitAudit/

        // 供应商运作审核BLL
        TraTransitAuditBLL TTAbll = new TraTransitAuditBLL();

        // 供应商运作信息BLL
        TraSupplierTransitBLL TSTbll = new TraSupplierTransitBLL();

        // 供应商审核BLL
        SupplierAuditBLL bll = new SupplierAuditBLL();

        // 运输供应商BLL
        SupplierTransportBLL STbll = new SupplierTransportBLL();

        SupplierCyclBLL scbll = new SupplierCyclBLL();

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
            SupplierAuditModel model = TTAbll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            SupplierAuditModel model = TTAbll.GetModelByID(tId);

            return View(model);
        }
        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">显示条数</param> 
        /// <param name="supplierName">运输供应商名称</param>
        /// <param name="officialTime">运作时间</param>
        /// <param name="signContract">签订合同</param>
        /// <param name="isCultivate">参与培训</param>
        /// <param name="state">审核状态</param>
        /// <returns></returns>
        public ActionResult TransitAuditList(int index, int size, string supplierName, string officialTime, string signContract, string isCultivate, string state)
        {
            // 查询审核人员为当前登录人且审核类型为运输运作准备审核的审核信息
            string where = " SA.SupplierAuditType = 6 AND SA.AuditUserId = " + Auxiliary.UserID();

            // 运输供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运作时间
            if (!string.IsNullOrEmpty(officialTime))
            {
                where += string.Format(" And convert(varchar,OfficialTime,120) like '%{0}%'", officialTime.Trim());
            }

            // 签订合同
            if (!string.IsNullOrEmpty(signContract))
            {
                where += string.Format(" And SignContract = {0}", signContract.Trim());
            }

            // 参与培训
            if (!string.IsNullOrEmpty(isCultivate))
            {
                where += string.Format(" And IsCultivate = {0}", isCultivate.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And SA.State = {0}", state.Trim());
            }

            // 供应商审核List
            List<SupplierAuditModel> list = TTAbll.TransitAuditList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="supplierName">运输供应商名称</param>
        /// <param name="officialTime">运作时间</param>
        /// <param name="signContract">签订合同</param>
        /// <param name="isCultivate">参与培训</param>
        /// <param name="state">审核状态</param>
        /// <returns></returns>
        public int TransitAuditAmount(string supplierName, string officialTime, string signContract, string isCultivate, string state)
        {
            // 查询审核人员为当前登录人且审核类型为运输运作准备审核的审核信息
            string where = " SA.SupplierAuditType = 6 AND SA.AuditUserId = " + Auxiliary.UserID();

            // 运输供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运作时间
            if (!string.IsNullOrEmpty(officialTime))
            {
                where += string.Format(" And convert(varchar,OfficialTime,120) like '%{0}%'", officialTime.Trim());
            }

            // 签订合同
            if (!string.IsNullOrEmpty(signContract))
            {
                where += string.Format(" And SignContract = {0}", signContract.Trim());
            }

            // 参与培训
            if (!string.IsNullOrEmpty(isCultivate))
            {
                where += string.Format(" And IsCultivate = {0}", isCultivate.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And SA.State = {0}", state.Trim());
            }

            return TTAbll.TransitAuditAmount(where);
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <returns></returns>
        public ActionResult ThroughAudit(SupplierAuditModel tModel)
        {
            // 审核前Model
            SupplierAuditModel beforeModel = TTAbll.GetModelByID(tModel.SupplierAuditId);

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

                    //  若无 
                    // 供应商运作信息表状态(审核通过 10)
                    TSTbll.ChangeState(beforeModel.PresentId, 10);

                    // 根据提交表ID获取实体Model得到所对应的运输供应商ID(TransportId)
                    TraSupplierTransitModel TSTmodel = TSTbll.GetModelByID(beforeModel.PresentId);

                    // 运输供应商表运输供应商状态(运作 F4)
                    STbll.UpdateSuppTranStates(TSTmodel.TransportId.ToString(), "F4");

                    /// <param name="supplierId">供应商id</param>
                    /// <param name="deparTmentId">所属机构</param>
                    /// <param name="otherId">运输(仓储)供应商id 通过Types区别</param>
                    /// <param name="types">类型：0-运输供应商;1-仓储供应商</param>
                    /// <param name="beginTypes">周期开始类型：0：待运作->运作;1-停用->运作</param>
                    scbll.AddBeginTypes(beforeModel.OtherId, beforeModel.AuditDepartmentId, beforeModel.PresentId, 0,0);

                    // 将运作信息新增到财务表中
                    SupplierFinanceModel SFModel = new SupplierFinanceModel();
                    SFModel.SupplierId = TSTmodel.SupplierId;
                    SFModel.OtherId = TSTmodel.TransportId;
                    SFModel.SCMNumber = TSTmodel.SCMNumber;
                    SFModel.ProcurementNumber = TSTmodel.ProcurementNumber;
                    SFModel.OfficialTime = TSTmodel.OfficialTime;
                    SFModel.SignContract = TSTmodel.SignContract;
                    SFModel.ContractNumber = TSTmodel.ContractNumber;

                    // 合同开始时间
                    // SFModel.ContractBeginTime = TSTmodel.ContractBeginTime;

                    // 合同结束时间
                    // SFModel.ContractEndTime = TSTmodel.ContractEndTime;

                    SFModel.AccountsPeriod = TSTmodel.AccountsPeriod;
                    SFModel.Deposit = TSTmodel.Deposit;
                    SFModel.IsCultivate = TSTmodel.IsCultivate;
                    SFModel.CultivateIntro = TSTmodel.CultivateIntro;
                    SFModel.CooperationRemark = TSTmodel.CooperationRemark;
                    SFModel.CompanyId = TSTmodel.CompanyId;
                    new SupplierFinanceBLL().AddSupplierFinance(SFModel);

                    // 供应商日志
                    Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "审核通过", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });
                    return Json(new { flag = "success", content = "审核通过！" });
                }
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Fail, new { Type = "审核失败", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

            return Json(new { flag = "fail" });
        }
         
        /// <summary>
        /// 驳回
        /// </summary>
        /// <returns></returns>
        public ActionResult RejectAudit(SupplierAuditModel tModel)
        {
            // 审核前Model
            SupplierAuditModel beforeModel = TTAbll.GetModelByID(tModel.SupplierAuditId);

            // 更改审核记录状态(驳回 4)
            int row = bll.ChangeState(tModel.SupplierAuditId, 4, beforeModel.AuditRemark);

            if (row > 0)
            {
                // 更改登记表提交状态为驳回
                TSTbll.ChangeState(beforeModel.PresentId, 1);

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
        /// <param name="tPresentId">提交表ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Revoke)]
        public ActionResult RevokeAudit(int tId)
        {
            // 撤销前Model
            SupplierAuditModel beforeModel = TTAbll.GetModelByID(tId);

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
    }
}
