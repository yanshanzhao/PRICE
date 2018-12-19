// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-11    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.BLL.Tra;
using System.Linq;
using SRM.Model.Tra;
using SRM.BLL.Supplier;
using SRM.Model.Supplier;
#endregion
/*********************************
 * 类名：TraChooseAuditController
 * 功能描述：运输选择审核表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraChooseAuditController : Controller
    {
        //
        // GET: /Tra/TraChooseAudit/

        //仓储供应商审核BLL
        TraChooseAuditBLL bll = new TraChooseAuditBLL();

        // 仓储供应商申请BLL
        TraChooseBLL sbl = new TraChooseBLL();
        //运作附件
        TraChooseBeginAdjunctBLL tcbll = new TraChooseBeginAdjunctBLL();

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
            // 获取数据
            TraChooseAuditModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temTraChooseBeginAdjunct> filelist = tcbll.TraChooseBeginFileList(model.TraChooseId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            TraChooseAuditModel model = bll.GetModelByID(tId);
            //获取附件信息
            List<temTraChooseBeginAdjunct> filelist = tcbll.TraChooseBeginFileList(model.TraChooseId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }
        #endregion

        #region 方法

        #region index中查询出来的数据列表

        /// <summary>
        /// index中查询出来的数据列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="CreateDepartmentId"></param>
        /// <param name="ApplyTime"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult TraChooseAuditList(int index, int size, string CreateDepartmentId, string ApplyTime, string state)
        {
            // 查询审核机构为本机构且审核类型为运输选择审核的审核信息
            string where = " SA.SupplierAuditType=4 and  SA.AuditUserId = " + Auxiliary.UserID();

            //创建机构
            if (!string.IsNullOrEmpty(CreateDepartmentId))
            {
                where += string.Format(" And TCH.CreateDepartmentId = {0}", CreateDepartmentId.Trim());
            }

            //申请时间
            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,TCH.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And  SA.State = {0}", state.Trim());
            }

            List<TraChooseAuditModel> list = bll.TraChooseAuditList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region index中查询出来的数据集

        /// <summary>
        /// index中查询出来的数据集
        /// </summary>  
        /// <param name="CreateDepartmentId">提交机构</param>
        /// <param name="ApplyTime">申请时间</param>
        /// <param name="state">审核状态</param>
        /// <returns></returns>
        public int TraChooseAuditAmount(string CreateDepartmentId, string ApplyTime, string state)
        {
            string where = " SA.SupplierAuditType=4 and  SA.AuditUserId = " + Auxiliary.UserID();

            //创建机构id
            if (!string.IsNullOrEmpty(CreateDepartmentId))
            {
                where += string.Format(" And TCH.CreateDepartmentId = {0}", CreateDepartmentId.Trim());
            }

            //申请时间
            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,TCH.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And  SA.State = {0}", state.Trim());
            }

            return bll.TraChooseAuditAmount(where);
        }

        #endregion

        #region index中的撤销按钮

        /// <summary>
        /// 撤销
        /// <param name="tId"></param>
        /// <param name="tPresentId"></param>
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Revoke)]
        public ActionResult RevokeAudit(int tId, int tPresentId)
        {
            // 撤销前Model
            TraChooseAuditModel beforeModel = bll.GetModelByID(tId);

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
                TraChooseAuditModel NextModel = bll.GetNextAuditModel(tId);

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

        #region 审核通过按钮逻辑

        /// <summary> 
        /// 审核通过
        /// <param name="tModel"></param>
        /// </summary>
        /// <returns></returns>
        public ActionResult ThroughAudit(TraChooseAuditModel tModel)
        {

            // 审核前Model
            TraChooseAuditModel beforeModel = bll.GetModelByID(tModel.SupplierAuditId);

            // 审核流程Model
            BasisAuditRelationModel BARmodel = Auxiliary.GetAuditRelationById(beforeModel.AuditRelationId);

            // 判断是否有下一级审核
            BasisAuditRelationModel model = Auxiliary.IsRelationByBeforeId(beforeModel.AuditRelationId);

            // 若有 新增审核信息
            if (model != null)
            {
                // 修改审核记录状态(审核通过 1)
                bll.ChangeState(tModel.SupplierAuditId, 1, tModel.AuditRemark);

                TraChooseAuditModel Auditmodel = new TraChooseAuditModel();

                //供应商审核关系编号(流程编号):以ARN开头
                Auditmodel.AuditRelationNumber = model.AuditRelationNumber;

                //供应商审核关系id
                Auditmodel.AuditRelationId = model.AuditRelationId;

                //供应商id
                Auditmodel.OtherId = beforeModel.OtherId; 

                //提交表id
                Auditmodel.PresentId = beforeModel.PresentId; 

                //审核类型
                Auditmodel.SupplierAuditType = beforeModel.SupplierAuditType;

                //提起机构id
                Auditmodel.PresentDepartmentId = model.DepartmentId; 

                //提起人员id
                Auditmodel.PresentUserId = model.UserId; 

                //审核机构id
                Auditmodel.AuditDepartmentId = model.ToDepartmentId;

                //审核人员id
                Auditmodel.AuditUserId = model.ToUserId; 

                //供应商审核关系名称
                Auditmodel.AuditRelationName = model.AuditRelationName; 

                //系统公司id
                Auditmodel.CompanyId = model.CompanyId;

                // 默认状态 未审核
                Auditmodel.State = 0;

                // 上一审核ID
                Auditmodel.BeforeId = tModel.SupplierAuditId;

                TraChooseAuditBLL auditBLL = new TraChooseAuditBLL();
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
                    sbl.TraChooseState(tModel.PresentId, 5);

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

        /// <summary>
        /// 审核不通过
        /// </summary>
        /// <returns></returns>
        public ActionResult FailAudit(TraChooseAuditModel tModel)
        {
            // 审核前Model
            TraChooseAuditModel beforeModel = bll.GetModelByID(tModel.SupplierAuditId);

            // 修改审核记录状态(驳回 4)
            int row = bll.ChangeState(tModel.SupplierAuditId, 3, tModel.AuditRemark);

            if (row > 0)
            {
                sbl.ChangeStatechoose(tModel.PresentId, 8);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "审核不通过", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                return Json(new { flag = "success", content = "审核不通过！" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "审核不通过", Id = tModel.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

            return Json(new { flag = "fail", content = "审核失败！" });
        }

        #region 驳回按钮逻辑

        /// <summary>
        /// 驳回
        /// <param name="model"></param>
        /// </summary>
        /// <returns></returns>
        public ActionResult RejectAudit(TraChooseAuditModel model)
        {
            // 审核前Model
            TraChooseAuditModel beforeModel = bll.GetModelByID(model.SupplierAuditId);

            // 修改审核记录状态(驳回 4)
            int row = bll.ChangeState(model.SupplierAuditId, 4, model.AuditRemark);

            if (row > 0)
            {
                // 更改本数据状态为驳回,登记表提交状态为驳回
                sbl.ChangeStatechoose(model.PresentId, 10);

                // 更改本数据状态为驳回,登记表提交状态为初始
                sbl.ChangeStates(model.PresentId, 0);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "审核驳回", Id = model.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

                return Json(new { flag = "success", content = "审核驳回！" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "驳回失败", Id = model.SupplierAuditId, AuditRelationNumber = beforeModel.AuditRelationNumber, AuditRelationName = beforeModel.AuditRelationName });

            return Json(new { flag = "fail" });
        }
        #endregion

        #region 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   

        /// <summary>
        /// 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="supplierAuditId"></param>
        /// <returns></returns>
        public ActionResult TraChooseAuditdetaileditList(int index, int size, string supplierAuditId)
        {

            string where = string.Format(" DA.SupplierAuditId in ({0})  and TCHD.DetailState!=10", supplierAuditId);

            List<TraChooseAuditModel> list = bll.TraChooseAuditdetaileditList(index, size, where);

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
        public int TraChooseAuditdetaileditAmount(string supplierAuditId)
        {
            string where = string.Format(" DA.SupplierAuditId in ({0})  and TCHD.DetailState!=10", supplierAuditId);

            return bll.TraChooseAuditdetaileditAmount(where);
        }
        #endregion


        #region 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   

        /// <summary>
        /// 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   
        /// </summary> 
        /// <param name="OperationRecordId"></param>
        /// <returns></returns>
        public ActionResult TraChooseeditList(int index, int size,string supplierAuditId)
        {

            //string where = string.Format(" TC.TraChooseId in ({0})", traChooseId);
            string where = string.Format(" DA.SupplierAuditId in ({0}) and TCL.State!=0", supplierAuditId);
              
            List<TraChooseAuditModel> list = bll.TraChooseeditList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 编辑按钮时：显示下方的仓储选择明细数据记录count 

        /// <summary>
        /// 编辑按钮时：显示下方的仓储选择明细数据记录count
        /// </summary>
        /// <param name="OperationRecordId"></param> 
        /// <returns></returns>
        public int TraChooseeditAmount(string supplierAuditId)
        {
            //string where = string.Format(" TC.TraChooseId in ({0})", traChooseId);
            string where = string.Format(" DA.SupplierAuditId in ({0}) and TCL.State!=0", supplierAuditId);

            return bll.TraChooseeditAmount(where);
        }
        #endregion

        #endregion

    }
}
