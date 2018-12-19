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
using SRM.BLL.Basis;
#endregion
/*********************************
 * 类名：TraSupplierTransitController
 * 功能描述：供应商运作信息 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraSupplierTransitController : Controller
    {
        //
        // GET: /Tra/TraSupplierTransit/

        // 供应商运作信息BLL
        TraSupplierTransitBLL bll = new TraSupplierTransitBLL();

        // 供应商审核BLL
        SupplierAuditBLL auditBLL = new SupplierAuditBLL();

        // 运输选择BLL
        TraChooseBLL chooseBLL = new TraChooseBLL();

        // 关键节点BLL
        BasisKeyNodeBLL KeyNodeBLL = new BasisKeyNodeBLL();

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
            TraSupplierTransitModel model = bll.GetModelByID(tId);

            // 获取运作编号
            TraOperationClaimModel tocModel = new TraOperationClaimBLL().GetModelByID(model.OperationClaimId);
            if (tocModel != null)
            {
                ViewBag.OperationNumber = tocModel.OperationNumber;
            }

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            TraSupplierTransitModel model = bll.GetModelByID(tId);

            // 获取运作编号
            TraOperationClaimModel tocModel = new TraOperationClaimBLL().GetModelByID(model.OperationClaimId);
            if (tocModel != null)
            {
                ViewBag.OperationNumber = tocModel.OperationNumber;
            }

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.Trail)]
        public ActionResult Trail(int tId, int supplierAuditType)
        {
            ViewBag.PresentId = tId;
            ViewBag.SupplierAuditType = supplierAuditType;

            return View();
        }

        /// <summary>
        /// ChooseSupplier
        /// </summary>  
        public ActionResult ChooseSupplier(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// ChooseOperation
        /// </summary>  
        public ActionResult ChooseOperation(int tTransportId, string url)
        {
            ViewBag.TransportId = tTransportId;
            ViewBag.url = url;
            return View();
        }

        #endregion

        #region 方法
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddSupplierTransit(TraSupplierTransitModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认初始 0
            tModel.State = 0;

            // 新增(返回主键ID)
            int SupplierTransitId = bll.AddSupplierTransit(tModel);

            // 若主键>O(新增成功)
            if (SupplierTransitId > 0)
            {
                // 更改评价审核状态变为使用状态。
                chooseBLL.ChangeEvaluateState(tModel.ChooseId, 20);

                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 数据集 运输供应商选择
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">运输供应商名称</param> 
        /// <returns>Json</returns>
        public ActionResult ChooseSupplierList(int index, int size, string supplierName)
        {
            // 评价审核状态为审核通过的信息。
            string where = " EvaluateState = 5 AND TC.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运输选择List
            List<TraChooseModel> list = bll.ChooseSupplierList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数 运输供应商选择
        /// </summary>
        /// <param name="supplierName">运输供应商名称</param> 
        /// <returns></returns>
        public int ChooseSupplierAmount(string supplierName)
        {
            // 评价审核状态为审核通过的信息。
            string where = " EvaluateState = 5 AND TC.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            return bll.ChooseSupplierAmount(where);
        }

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">运输供应商名称</param>
        /// <param name="officialTime">运作时间</param>
        /// <param name="signContract">签订合同</param>
        /// <param name="iscultivate">参与培训</param>
        /// <param name="state">状态</param> 
        /// <returns>Json</returns>
        public ActionResult SupplierTransitList(int index, int size, string supplierName, string officialTime, string signContract, string iscultivate, string state)
        {
            // 只能查询本机构录入且状态为非作废的运作申请信息。
            string where = " TST.State != 40 AND TST.CreateDepartmentId =" + Auxiliary.DepartmentId();

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
            if (!string.IsNullOrEmpty(iscultivate))
            {
                where += string.Format(" And IsCultivate = {0}", iscultivate.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TST.State = {0}", state.Trim());
            }

            // 供应商运作信息List
            List<TraSupplierTransitModel> list = bll.SupplierTransitList(index, size, where);

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
        /// <param name="iscultivate">参与培训</param>
        /// <param name="state">状态</param> 
        /// <returns></returns>
        public int SupplierTransitAmount(string supplierName, string officialTime, string signContract, string iscultivate, string state)
        {
            // 只能查询本机构录入且状态为非作废的运作申请信息。
            string where = " TST.State != 40 AND TST.CreateDepartmentId =" + Auxiliary.DepartmentId();

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
            if (!string.IsNullOrEmpty(iscultivate))
            {
                where += string.Format(" And IsCultivate = {0}", iscultivate.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TST.State = {0}", state.Trim());
            }

            return bll.SupplierTransitAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditSupplierTransit(TraSupplierTransitModel tModel)
        {
            // 编辑之前Model
            TraSupplierTransitModel beforeModel = bll.GetModelByID(tModel.SupplierTransitId);

            // 编辑
            int row = bll.EditSupplierTransit(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel);

                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <param name="tSupplierid">供应商ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitState(int tId, int tSupplierid)
        {
            // 更新之前table
            TraSupplierTransitModel beforeModel = bll.GetModelByID(tId);

            // 影响行数
            int row = 0;

            // 查询本公司的签订合同是否配置(关键节点表（BasisKeyNode）
            // 关键节点编号 关键节点字段 当前时间(是否在开始至结束至简) (查询非作废状态) 
            BasisKeyNodeModel KeyNodeModel = KeyNodeBLL.GetModelByName("TraSupplierTransit", "SignContract", Auxiliary.CompanyID());

            // 在有效期内在查看最小值为0或1(为0 否签订 为1 是签订) 
            if (KeyNodeModel != null && beforeModel.SignContract == KeyNodeModel.MinValue)
            {
                // 如果大于配置信息则使用供应商审核关系维护中关键节点为是的信息
                BasisAuditRelationModel Controlmodel = new BasisAuditRelationBLL().IsMatchForControl(6, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID(), "Control");

                // 若审核流程不为NULL
                if (Controlmodel != null)
                {
                    // 提交(更改状态为审核中 5)
                    bll.ChangeState(tId, 5);

                    // 审核Model
                    SupplierAuditModel Auditmodel = new SupplierAuditModel();
                    Auditmodel.AuditRelationNumber = Controlmodel.AuditRelationNumber;
                    Auditmodel.AuditRelationId = Controlmodel.AuditRelationId;
                    Auditmodel.OtherId = tSupplierid;
                    Auditmodel.PresentId = tId;
                    Auditmodel.SupplierAuditType = Controlmodel.AuditRelationType;
                    Auditmodel.PresentDepartmentId = Controlmodel.DepartmentId;
                    Auditmodel.PresentUserId = Controlmodel.UserId;
                    Auditmodel.AuditDepartmentId = Controlmodel.ToDepartmentId;
                    Auditmodel.AuditUserId = Controlmodel.ToUserId;
                    Auditmodel.AuditRelationName = Controlmodel.AuditRelationName;
                    Auditmodel.CompanyId = Controlmodel.CompanyId;

                    // 默认状态 未审核
                    Auditmodel.State = 0;

                    // 新增审核数据
                    row = auditBLL.AddAuditRelation(Auditmodel);

                    // 若影响行数>O(新增成功)
                    if (row > 0)
                    {
                        // 供应商日志
                        Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "提交", Id = tId, State = "审核中" });
                        return Json(new { flag = "success", content = "提交成功！" });
                    }

                    // 关键节点表数据改成使用状态
                    //KeyNodeBLL.ChangeState(KeyNodeModel.Id, 1);
                }
                else
                {
                    // 系统日志
                    Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "提交", Id = tId, State = "初始" });
                    return Json(new { flag = "fail", content = "无匹配审核流程！" });
                }
            }
            else
            {
                // 审核流程model
                // 查询BasisAuditRelation的类型为运输运作准备审核(6)的，提交账号为本账号的流程开始节点为是(1)的有效状态(1)信息
                BasisAuditRelationModel model = new BasisAuditRelationBLL().IsMatchForControl(6, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID(), "");

                // 若审核流程不为NULL
                if (model != null)
                {
                    // 提交(更改状态为审核中 5)
                    bll.ChangeState(tId, 5);

                    // 审核Model
                    SupplierAuditModel Auditmodel = new SupplierAuditModel();
                    Auditmodel.AuditRelationNumber = model.AuditRelationNumber;
                    Auditmodel.AuditRelationId = model.AuditRelationId;
                    Auditmodel.OtherId = tSupplierid;
                    Auditmodel.PresentId = tId;
                    Auditmodel.SupplierAuditType = model.AuditRelationType;
                    Auditmodel.PresentDepartmentId = model.DepartmentId;
                    Auditmodel.PresentUserId = model.UserId;
                    Auditmodel.AuditDepartmentId = model.ToDepartmentId;
                    Auditmodel.AuditUserId = model.ToUserId;
                    Auditmodel.AuditRelationName = model.AuditRelationName;
                    Auditmodel.CompanyId = model.CompanyId;

                    // 默认状态 未审核
                    Auditmodel.State = 0;

                    // 新增审核数据
                    row = auditBLL.AddAuditRelation(Auditmodel);

                    // 若影响行数>O(新增成功)
                    if (row > 0)
                    {
                        // 供应商日志
                        Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "提交", Id = tId, State = "审核中" });
                        return Json(new { flag = "success", content = "提交成功！" });
                    }
                }
                else
                {
                    // 系统日志
                    Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "提交", Id = tId, State = "初始" });
                    return Json(new { flag = "fail", content = "无匹配审核流程！" });
                }
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "提交", Id = tId, State = "初始" });
            return Json(new { flag = "fail", content = "提交失败！" });
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Revoke)]
        public ActionResult RevokeData(int tId)
        {
            // 更新之前table
            TraSupplierTransitModel beforeModel = bll.GetModelByID(tId);

            // 流程开始已提（非撤销状态）的数量 
            int count = 0;

            // 查询本公司的签订合同是否配置(关键节点表（BasisKeyNode）
            // 关键节点编号 关键节点字段 当前时间(是否在开始至结束至简) (查询非作废状态) 
            BasisKeyNodeModel KeyNodeModel = KeyNodeBLL.GetModelByName("TraSupplierTransit", "SignContract", Auxiliary.CompanyID());

            // 在有效期内在查看最小值为0或1(为0 否签订 为1 是签订) 
            if (KeyNodeModel != null && beforeModel.SignContract == KeyNodeModel.MinValue)
            {
                // 如果大于配置信息则使用供应商审核关系维护中关键节点为是的信息
                BasisAuditRelationModel Controlmodel = new BasisAuditRelationBLL().IsMatchForControl(6, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID(), "Control");

                // 若审核流程不为NULL
                if (Controlmodel != null)
                {
                    // 查询提交的数据是否已审核(流程开始) 根据本数据ID(提交表ID) 流程ID(审核流程ID) 查询审核表(SupplierAuditModel)
                    SupplierAuditModel model = new SupplierAuditBLL().GetRevokeModel(tId, Controlmodel.AuditRelationId);

                    // 如果未审核
                    if (model.State == 0)
                    {
                        // 查询流程开始已提（非撤销状态）的数量
                        count = new SupplierAuditBLL().GetRevokeCouunt(tId, Controlmodel.AuditRelationId);

                        if (count > 1)
                        {
                            // 修改本数据状态为驳回状态
                            bll.ChangeState(tId, 1); 

                            // 修改审核表数据状态为撤销状态
                            new SupplierAuditBLL().ChangeState(model.SupplierAuditId, 10, "");

                            Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "撤销", Id = tId, State = "驳回状态" });
                            return Json(new { flag = "success", content = "撤销成功！" });
                        }
                        else
                        {
                            // 修改本数据状态为初始状态
                            bll.ChangeState(tId, 0); 

                            // 修改审核表数据状态为撤销状态
                            new SupplierAuditBLL().ChangeState(model.SupplierAuditId, 10, "");

                            Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "撤销", Id = tId, State = "初始状态" });
                            return Json(new { flag = "success", content = "撤销成功！" });
                        }
                    }
                    else
                    {
                        Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "撤销", Id = tId, State = "审核中" });
                        return Json(new { flag = "fail", content = "已经审核的数据无法撤销！" });
                    }
                }
                else
                {
                    // 系统日志
                    Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "撤销", Id = tId });
                    return Json(new { flag = "fail", content = "无匹配审核流程！" });
                }
            }
            else
            {
                // 审核流程model
                // 查询BasisAuditRelation的类型为运输运作准备审核(6)的，提交账号为本账号的流程开始节点为是(1)的有效状态(1)信息
                BasisAuditRelationModel NoControlmodel = new BasisAuditRelationBLL().IsMatchForControl(6, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID(), "");

                // 若审核流程不为NULL
                if (NoControlmodel != null)
                {
                    // 查询提交的数据是否已审核(流程开始) 根据本数据ID(提交表ID) 流程ID(审核流程ID) 查询审核表(SupplierAuditModel)
                    SupplierAuditModel model = new SupplierAuditBLL().GetRevokeModel(tId, NoControlmodel.AuditRelationId);

                    // 如果未审核
                    if (model.State == 0)
                    {
                        // 查询流程开始已提（非撤销状态）的数量
                        count = new SupplierAuditBLL().GetRevokeCouunt(tId, NoControlmodel.AuditRelationId);

                        if (count > 1)
                        {
                            // 修改本数据状态为驳回状态
                            bll.ChangeState(tId, 1); 

                            // 修改审核表数据状态为撤销状态
                            new SupplierAuditBLL().ChangeState(model.SupplierAuditId, 10, "");

                            Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "撤销", Id = tId, State = "驳回状态" });
                            return Json(new { flag = "success", content = "撤销成功！" });
                        }
                        else
                        {
                            // 修改本数据状态为初始状态
                            bll.ChangeState(tId, 0); 

                            // 修改审核表数据状态为撤销状态
                            new SupplierAuditBLL().ChangeState(model.SupplierAuditId, 10, "");

                            Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "撤销", Id = tId, State = "初始状态" });
                            return Json(new { flag = "success", content = "撤销成功！" });
                        }
                    }
                    else
                    {
                        Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "撤销", Id = tId, State = "审核中" });
                        return Json(new { flag = "fail", content = "已经审核的数据无法撤销！" });
                    }
                }
                else
                {
                    // 系统日志
                    Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "撤销", Id = tId, State = "审核中" });
                    return Json(new { flag = "fail", content = "无匹配审核流程！" });
                }
            }
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
            TraSupplierTransitModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 将对应的运输供应商选择的评价审核状态变为审核通过状态 
                chooseBLL.ChangeEvaluateState(beforeModel.ChooseId, 5);

                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败" });
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="supplierName">运输供应商名称</param>
        /// <param name="officialTime">运作时间</param>
        /// <param name="signContract">签订合同</param>
        /// <param name="iscultivate">参与培训</param>
        /// <param name="state">状态</param> 
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string officialTime, string signContract, string iscultivate, string state)
        {
            // 只能查询本机构录入且状态为非作废的运作申请信息。
            string where = " TST.State != 40 AND TST.CreateDepartmentId =" + Auxiliary.DepartmentId();

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
            if (!string.IsNullOrEmpty(iscultivate))
            {
                where += string.Format(" And IsCultivate = {0}", iscultivate.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TST.State = {0}", state.Trim());
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where);

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

        #region 审核轨迹列表

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="tPresentId">提交表ID</param>
        /// <param name="tSupplierAuditType">审核类型</param> 
        /// <returns>Json</returns>
        public ActionResult GetRelationList(int tPresentId, int tSupplierAuditType)
        {
            // 审核轨迹List 
            List<SupplierAuditModel> list = auditBLL.GetRelationList(tPresentId, tSupplierAuditType);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion
        #endregion
    }
}
