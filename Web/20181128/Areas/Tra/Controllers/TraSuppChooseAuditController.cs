// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-30    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using System.Linq;
using SRM.BLL.Supplier;
using SRM.Model.Supplier;
#endregion
/*********************************
 * 类名：TraSuppChooseAuditController
 * 功能描述：供应商选用审核 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraSuppChooseAuditController : Controller
    {
        //
        // GET: /Tra/TraSuppChooseAudit/

        //供应商选用审核BLL
        TraSuppChooseAuditBLL bll = new TraSuppChooseAuditBLL();

        // 选用评分表BLL
        TraChooseEvaluateBLL sbll = new TraChooseEvaluateBLL();

        //选用评分附件表
        TraChoiceEvaluateAdjunctBLL abll = new TraChoiceEvaluateAdjunctBLL();

        TraChooseBLL sbl = new TraChooseBLL();


        #region 页面

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
        public ActionResult Audit(int tId)
        {

            //// 获取数据
            TraSuppChooseAuditModel model = bll.GetModelByID(tId);

            TraSuppChooseAuditModel modelsupp = bll.GetModesupplByID(tId);

            TraChooseEvaluateModel models = sbll.GetModelByIDS(model.TraChooseId);

            TraChooseEvaluateModel choiceevaluateid = sbll.GetModelByIDs(model.TraChooseId);

            // 附件
            List<TempChoiceEvaluateAdjunctModel> fileList = abll.AdjunctListById(choiceevaluateid.ChoiceEvaluateId);

            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);

            List<string> oldlist = fileList.Select(p => p.FileName + p.ext).ToList<string>();

            ViewBag.trachooseid = model.TraChooseId;//运输选择id

            ViewBag.transportid = model.TransportId;//运输供应商id

            ViewBag.transportnumber = model.TransportNumber;//运输编号

            ViewBag.choicefromid = model.ChoiceFromId;//运输元件表单id

            ViewBag.evaluateuser = model.EvaluateUser;//评价负责人

            ViewBag.evaluatetime = model.EvaluateTime;//评价时间

            ViewBag.suppliername = modelsupp.SupplierName;//供应商名称

            ViewBag.evaluateremark = model.EvaluateRemark; //评价备注

            ViewBag.evaluatestate = model.EvaluateState;//评价状态

            ViewBag.evaluatestatename = model.EvaluateStateName;//显示评价状态名称

            ViewBag.evaluatesuppmark = model.EvaluateSuppMark; //评价总得分

            ViewBag.applytime = model.ApplyTime;  //申请时间

            ViewBag.supplierauditid = model.SupplierAuditId;//供应商审核id

            ViewBag.presentid = model.PresentId; //提交id

            ViewBag.auditremark = model.AuditRemark; //审核备注

            //评价内容列表
            List<Model.Tra.TraChooseEvaluateModel> evaluatecontentlist = sbll.EvaluateContentList(Auxiliary.CompanyID());

            //编辑时评价内容列表

            List<Model.Tra.TraChooseEvaluateModel> evaluateeditlist = sbll.EvaluateEditListss(model.TraChooseId, models.ChoiceEvaluateId);
            List<Model.Tra.TraChooseEvaluateModel> evaluatesuppliername = sbll.EvaluateSupplierNameList(model.TraChooseId);
            int evaluatesuppliernamecount = sbll.EvaluateSupplierNameCount(model.TraChooseId);
            int evaluatecontentcount = sbll.EvaluateContentCount();

            return View(new SuppModelss
            {
                EvaluateContentList = evaluatecontentlist,
                EvaluateEditListss = evaluateeditlist,
                EvaluateSupplierNameList = evaluatesuppliername,
                EvaluateSupplierNameCount = evaluatesuppliernamecount,
                EvaluateContentCount = evaluatecontentcount,
            });
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            //// 获取数据
            TraSuppChooseAuditModel model = bll.GetModelByID(tId);

            TraSuppChooseAuditModel modelsupp = bll.GetModesupplByID(tId);

            TraChooseEvaluateModel models = sbll.GetModelByIDS(model.TraChooseId);

            TraChooseEvaluateModel choiceevaluateid = sbll.GetModelByIDs(model.TraChooseId);

            // 附件
            List<TempChoiceEvaluateAdjunctModel> fileList = abll.AdjunctListById(choiceevaluateid.ChoiceEvaluateId);

            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);

            List<string> oldlist = fileList.Select(p => p.FileName + p.ext).ToList<string>();

            ViewBag.trachooseid = model.TraChooseId;//运输选择id

            ViewBag.transportid = model.TransportId;//运输供应商id

            ViewBag.transportnumber = model.TransportNumber;//运输编号

            ViewBag.choicefromid = model.ChoiceFromId;//运输元件表单id

            ViewBag.evaluateuser = model.EvaluateUser;//评价负责人

            ViewBag.evaluatetime = model.EvaluateTime;//评价时间

            ViewBag.suppliername = modelsupp.SupplierName;//供应商名称

            ViewBag.evaluateremark = model.EvaluateRemark; //评价备注

            ViewBag.evaluatestate = model.EvaluateState;//评价状态

            ViewBag.evaluatestatename = model.EvaluateStateName;//显示评价状态名称

            ViewBag.evaluatesuppmark = model.EvaluateSuppMark; //评价总得分

            ViewBag.applytime = model.ApplyTime;  //申请时间

            ViewBag.supplierauditid = model.SupplierAuditId;//供应商审核id

            ViewBag.presentid = model.PresentId; //提交id

            ViewBag.auditremark = model.AuditRemark; //审核备注

            //评价内容列表
            List<Model.Tra.TraChooseEvaluateModel> evaluatecontentlist = sbll.EvaluateContentList(Auxiliary.CompanyID());

            //编辑时评价内容列表
            List<Model.Tra.TraChooseEvaluateModel> evaluateeditlist = sbll.EvaluateEditListss(model.TraChooseId, models.ChoiceEvaluateId);

            List<Model.Tra.TraChooseEvaluateModel> evaluatesuppliername = sbll.EvaluateSupplierNameList(model.TraChooseId);
            int evaluatesuppliernamecount = sbll.EvaluateSupplierNameCount(model.TraChooseId);
            int evaluatecontentcount = sbll.EvaluateContentCount();

            return View(new SuppModelss
            {
                EvaluateContentList = evaluatecontentlist,
                EvaluateEditListss = evaluateeditlist,
                EvaluateSupplierNameList = evaluatesuppliername,
                EvaluateSupplierNameCount = evaluatesuppliernamecount,
                EvaluateContentCount = evaluatecontentcount,
            });
        }
        #endregion

        #region 方法

        #region index中查询出来的数据列表

        /// <summary>
        /// index中查询出来的数据列表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">显示条数</param>
        /// <param name="storageNumber">仓储供应商编号</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="presentDepartment">提交机构</param>
        /// <param name="presentTime">提交时间</param>
        /// <param name="state">审核状态</param>
        /// <returns></returns>
        public ActionResult TraSuppChooseAuditList(int index, int size, string createDepartmentId, string applyTime, string evaluateState, string supplierName, string isNotification, string notificationType, string isEvaluate, string state, string auditTime)
        {
            string where = " SA.SupplierAuditType=5 AND SA.State!=10 and  SA.AuditUserId = " + Auxiliary.UserID();
            //string where = " SA.SupplierAuditType=5 AND SA.State!=10 ";

            //申请机构
            if (!string.IsNullOrEmpty(createDepartmentId))
            {
                where += string.Format(" And TC.CreateDepartmentId = {0}", createDepartmentId.Trim());
            }

            //申请时间
            if (!string.IsNullOrEmpty(applyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", applyTime.Trim());
            }

            //评价审核状态
            if (!string.IsNullOrEmpty(evaluateState))
            {
                where += string.Format(" And  TC.EvaluateState = {0}", evaluateState.Trim());
            }

            //评价审核状态
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And  S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            //评价审核状态
            if (!string.IsNullOrEmpty(isNotification))
            {
                where += string.Format(" And  TC.IsNotification = {0}", isNotification.Trim());
            }

            //招标状态
            if (!string.IsNullOrEmpty(notificationType))
            {
                where += string.Format(" And  TC.NotificationType = {0}", notificationType.Trim());
            }

            //是否评价
            if (!string.IsNullOrEmpty(isEvaluate))
            {
                where += string.Format(" And  TC.IsEvaluate = {0}", isEvaluate.Trim());
            }

            //审核状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And  SA.State = {0}", state.Trim());
            }

            //审核时间
            if (!string.IsNullOrEmpty(auditTime))
            {
                where += string.Format(" And convert(varchar,SA.AuditTime,120) like '%{0}%'", auditTime.Trim());
            }

            List<TraSuppChooseAuditModel> list = bll.TraSuppChooseAuditList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region index中查询出来的数据集

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="storageNumber">仓储供应商编号</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="presentDepartment">提交机构</param>
        /// <param name="presentTime">提交时间</param>
        /// <param name="state">审核状态</param>
        /// <returns></returns>
        public int TraSuppChooseAuditAmount(string createDepartmentId, string applyTime, string evaluateState, string supplierName, string isNotification, string notificationType, string isEvaluate, string state, string auditTime)
        {

            string where = " SA.SupplierAuditType=5 AND SA.State!=10 and  SA.AuditUserId = " + Auxiliary.UserID();
            //string where = " SA.SupplierAuditType=5 AND SA.State!=10 ";

            //申请机构
            if (!string.IsNullOrEmpty(createDepartmentId))
            {
                where += string.Format(" And TC.CreateDepartmentId = {0}", createDepartmentId.Trim());
            }

            //申请时间
            if (!string.IsNullOrEmpty(applyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", applyTime.Trim());
            }

            //评价审核状态
            if (!string.IsNullOrEmpty(evaluateState))
            {
                where += string.Format(" And  TC.EvaluateState = {0}", evaluateState.Trim());
            }

            //评价审核状态
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And  S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            //评价审核状态
            if (!string.IsNullOrEmpty(isNotification))
            {
                where += string.Format(" And  TC.IsNotification = {0}", isNotification.Trim());
            }

            //招标状态
            if (!string.IsNullOrEmpty(notificationType))
            {
                where += string.Format(" And  TC.NotificationType = {0}", notificationType.Trim());
            }

            //是否评价
            if (!string.IsNullOrEmpty(isEvaluate))
            {
                where += string.Format(" And  TC.IsEvaluate = {0}", isEvaluate.Trim());
            }

            //审核状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And  SA.State = {0}", state.Trim());
            }

            //审核时间
            if (!string.IsNullOrEmpty(auditTime))
            {
                where += string.Format(" And convert(varchar,SA.AuditTime,120) like '%{0}%'", auditTime.Trim());
            }


            return bll.TraSuppChooseAuditAmount(where);
        }

        #endregion

        #region index中的撤销按钮

        /// <summary>
        /// index中的撤销按钮
        /// </summary>
        /// <param name="tId">主键id</param>
        /// <param name="tPresentId">提交id</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Revoke)]
        public ActionResult RevokeAudit(int tId, int tPresentId)
        {
            // 撤销前Model
            TraSuppChooseAuditModel beforeModel = bll.GetModelByID(tId);

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
                TraSuppChooseAuditModel NextModel = bll.GetNextAuditModel(tId);

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

                        //sbll.SubmitTraChooseEvaluate(beforeModel.TraChooseId);

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

        #region 审核通过按钮逻辑

        /// <summary> 
        /// 审核通过按钮逻辑
        /// </summary>
        /// <param name="tModel">数据model</param>
        /// <returns></returns>
        public ActionResult ThroughAudit(TraSuppChooseAuditModel tModel)
        {
            // 审核前Model
            TraSuppChooseAuditModel beforeModel = bll.GetModelByID(tModel.SupplierAuditId);

            // 审核流程Model
            BasisAuditRelationModel BARmodel = Auxiliary.GetAuditRelationById(beforeModel.AuditRelationId);

            // 判断是否有下一级审核
            BasisAuditRelationModel model = Auxiliary.IsRelationByBeforeId(beforeModel.AuditRelationId);

            // 若有 新增审核信息
            if (model != null)
            {
                // 修改审核记录状态(审核通过 1)
                bll.ChangeState(tModel.SupplierAuditId, 1, tModel.AuditRemark);

                TraSuppChooseAuditModel Auditmodel = new TraSuppChooseAuditModel();
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

                TraSuppChooseAuditBLL auditBLL = new TraSuppChooseAuditBLL();
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

                    //将申请表中的状态修改为审核通过
                    sbl.TraChooseEvaluateState(tModel.PresentId, 5);

                    //修改运输供应商登记表仓储状态(代运作 F7)
                    sbl.ChangeSuppTransportState(tModel.PresentId);

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

        #region 驳回按钮逻辑

        /// <summary>
        /// 驳回按钮逻辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult RejectAudit(TraSuppChooseAuditModel model)
        {
            // 审核前Model
            TraSuppChooseAuditModel beforeModel = bll.GetModelByID(model.SupplierAuditId);

            // 修改审核记录状态(驳回 4)
            int row = bll.ChangeState(model.SupplierAuditId, 4, beforeModel.AuditRemark);

            if (row > 0)
            {
                // 更改本数据状态为驳回,登记表提交状态为已退回 
                sbl.ChangeState(beforeModel.PresentId, 10);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "审核驳回", Id = model.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                return Json(new { flag = "success", content = "审核驳回！" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "驳回失败", Id = model.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

            return Json(new { flag = "fail", content = "驳回失败！" });
        }
        #endregion

        #region 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   

        /// <summary>
        /// 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="supplierAuditId">供应商审核id</param>
        /// <returns></returns>
        public ActionResult TraSuppChooseAuditdetaileditList(int index, int size, string supplierAuditId)
        {

            string where = string.Format(" DA.SupplierAuditId in ({0})", supplierAuditId);

            List<TraSuppChooseAuditModel> list = bll.TraSuppChooseAuditdetaileditList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 编辑按钮时：显示下方的仓储选择明细数据记录count 

        /// <summary>
        /// 编辑按钮时：显示下方的仓储选择明细数据记录count
        /// </summary>
        /// <param name=""></param> 
        /// <returns></returns>
        public int TraSuppChooseAuditdetaileditAmount(string supplierAuditId)
        {
            string where = string.Format(" DA.SupplierAuditId in ({0})", supplierAuditId);

            return bll.TraSuppChooseAuditdetaileditAmount(where);
        }
        #endregion

        #region 导出按钮逻辑

        /// <summary>
        /// 导出按钮逻辑
        /// </summary>
        /// <param name="datumType"></param>
        /// <param name="claimType"></param>
        /// <param name="state"></param>
        /// <param name="createTime"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string createDepartmentId, string applyTime, string evaluateState, string supplierName, string isNotification, string notificationType, string isEvaluate, string state, string auditTime)
        {

            string where = " SA.SupplierAuditType=5 AND SA.State!=10 and  SA.AuditDepartmentId = " + Auxiliary.DepartmentId();

            //申请机构
            if (!string.IsNullOrEmpty(createDepartmentId))
            {
                where += string.Format(" And TC.CreateDepartmentId = {0}", createDepartmentId.Trim());
            }

            //申请时间
            if (!string.IsNullOrEmpty(applyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", applyTime.Trim());
            }

            //评价审核状态
            if (!string.IsNullOrEmpty(evaluateState))
            {
                where += string.Format(" And  TC.EvaluateState = {0}", evaluateState.Trim());
            }

            //评价审核状态
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And  S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            //评价审核状态
            if (!string.IsNullOrEmpty(isNotification))
            {
                where += string.Format(" And  TC.IsNotification = {0}", isNotification.Trim());
            }

            //招标状态
            if (!string.IsNullOrEmpty(notificationType))
            {
                where += string.Format(" And  TC.NotificationType = {0}", notificationType.Trim());
            }

            //是否评价
            if (!string.IsNullOrEmpty(isEvaluate))
            {
                where += string.Format(" And  TC.IsEvaluate = {0}", isEvaluate.Trim());
            }

            //审核状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And  SA.State = {0}", state.Trim());
            }

            //审核时间
            if (!string.IsNullOrEmpty(auditTime))
            {
                where += string.Format(" And convert(varchar,SA.AuditTime,120) like '%{0}%'", auditTime.Trim());
            }

            System.Data.DataTable dt = bll.ExportDataTable(where);
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new
            {
                Type = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        #region 数据集 获取选择条件详细 

        /// <summary>
        /// 数据集 获取选择条件详细
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult EvaluateLists()
        {
            List<TraChooseEvaluateModel> evaluateLists = sbll.EvaluateLists(Auxiliary.CompanyID());

            return Json(evaluateLists);
        }
        #endregion

        #region 模板附件类型List 运输月度考核表单自定义附件明细

        /// <summary>
        /// 模板附件类型List 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <returns>Json</returns>
        public ActionResult ChoiceFromAdjunctLists(int tId)
        {

            TraChooseEvaluateModel choiceevaluateids = sbll.GetModelByIDs(tId);

            // 附件
            List<TempChoiceEvaluateAdjunctModel> fileList = abll.AdjunctListById(choiceevaluateids.ChoiceEvaluateId);

            return Json(fileList);
        }

        #endregion
        /// <summary>
        /// 数据集 评价内容标题
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult EvaluateSupplierNameList(int tId)
        {
            List<TraChooseEvaluateModel> evaluateList = sbll.EvaluateSupplierNameList(tId);

            return Json(evaluateList);
        }
        #endregion
        /// <summary>
        /// 数据集 评价内容标题
        /// </summary> 
        /// <returns>Json</returns>
        public int EvaluateSupplierNameCount(int tId)
        {
            return sbll.EvaluateSupplierNameCount(tId);
        } /// <summary>
          /// 数据集 评价内容标题
          /// </summary> 
          /// <returns>Json</returns>
        public int EvaluateContentCount()
        {
            return sbll.EvaluateContentCount();
        }
    }

    #region 评分辅助类

    /// <summary>
    /// 评分辅助类
    /// </summary>
    public class SuppModelss
    {
        //评价内容列表
        public List<Model.Tra.TraChooseEvaluateModel> EvaluateContentList { get; set; }

        //评价列表
        public List<Model.Tra.TraChooseEvaluateModel> EvaluateList { get; set; }

        //编辑时评价内容列表
        public List<Model.Tra.TraChooseEvaluateModel> EvaluateEditListss { get; set; }
         

        //显示运输供应商名称
        public List<Model.Tra.TraChooseEvaluateModel> EvaluateSupplierNameList { get; set; }

        //显示运输供应商名称
        public int EvaluateSupplierNameCount { get; set; }

        //显示运输供应商名称
        public int EvaluateContentCount { get; set; }

    }
    #endregion
}
