// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-07    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using SRM.BLL.Basis;
using System.Web.Mvc;
using System;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using System.Linq;
using SRM.BLL.Supplier;
using SRM.Model.Supplier;
#endregion
/*********************************
 * 类名：TraChooseController
 * 功能描述：运输选择申请表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraChooseController : Controller
    {
        //
        // GET: /Tra/TraChoose/
        TraChooseBLL bll = new TraChooseBLL();
        BasisAuditRelationBLL relationbll = new BasisAuditRelationBLL();
        BasisKeyNodeBLL keynodebll = new BasisKeyNodeBLL();

        // 关键节点BLL
        BasisKeyNodeBLL KeyNodeBLL = new BasisKeyNodeBLL();

        //运作附件
        TraChooseBeginAdjunctBLL tcbll = new TraChooseBeginAdjunctBLL();

        #region 页面 

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            BasisKeyNodeModel keynodemodel = keynodebll.GetModelByName("TraChoose", "MonthCost", Auxiliary.CompanyID());
            if (keynodemodel != null)
            {
                ViewBag.MonthCost = keynodemodel.MinValue;
            }
            else
            {
                ViewBag.MonthCost = "";
            }
            return View();
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns> 
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            ViewBag.StorageNumber = Auxiliary.CurCompanyAutoNum("RAN");

            BasisKeyNodeModel keynodemodel = keynodebll.GetModelByName("TraChoose", "MonthCost", Auxiliary.CompanyID());
            if (keynodemodel != null)
            {
                ViewBag.MonthCost = keynodemodel.MinValue;
            }
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
            TraChooseModel model = bll.GetModelByID(tId);
            //获取附件信息
            List<temTraChooseBeginAdjunct> filelist = tcbll.TraChooseBeginFileList(model.TraChooseId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);
            BasisKeyNodeModel keynodemodel = keynodebll.GetModelByName("TraChoose", "MonthCost", Auxiliary.CompanyID());
            if (keynodemodel != null)
            {
                ViewBag.MonthCost = keynodemodel.MinValue;
            }
            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            TraChooseModel model = bll.GetModelByID(tId);
            //获取附件信息
            List<temTraChooseBeginAdjunct> filelist = tcbll.TraChooseBeginFileList(model.TraChooseId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);
            return View(model);
        }

        /// <summary>
        /// 供应商明细
        /// </summary>
        public ActionResult SupplierDetail(string url)
        {
            ViewBag.url = url;
            return View();
        }

        #region 弹出仓储供应商信息窗口

        /// <summary>
        /// 选择供应商规模信息
        /// </summary>
        public ActionResult TraChooseDetail(string url, string ids, string idss, string type)
        {
            ViewBag.url = url;
            ViewBag.type = type;
            ViewBag.ids = ids;
            ViewBag.idss = idss;
            return View();
        }
        #endregion

        #region 选择线路窗口

        /// <summary>
        /// 选择线路窗口
        /// </summary>
        public ActionResult TraChooseLine(string url, string ids, string type)
        {
            ViewBag.url = url;
            ViewBag.type = type;
            ViewBag.ids = ids;
            return View();
        }
        #endregion

        #endregion

        #region 方法 

        #region 显示index中的数据集

        /// <summary>
        /// 显示index中的数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size">页面条数</param>
        /// <param name="ApplyTime">申请时间</param>
        /// <param name="State">使用状态</param>
        /// <param name="UseState">审核状态</param>
        /// <returns></returns>
        public ActionResult TraChooseList(int index, int size, string ApplyTime, string State, string UseState, string SupplierName)
        {
            string where = " TC.State != 10 AND TC.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(SupplierName))
            {
                where += string.Format(" And TC.TraChooseId IN ( SELECT TraChooseId FROM TraChooseDetail WHERE TransportId IN( SELECT TransportId FROM SupplierTransport WHERE SupplierId IN (SELECT SupplierId FROM Supplier where SupplierName like '%{0}%')))", SupplierName.Trim());
            }

            // 申请时间
            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            // 使用状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And TC.State = {0}", State.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(UseState))
            {
                where += string.Format(" And TC.UseState = {0}", UseState.Trim());
            }

            List<TraChooseModel> list = bll.TraChooseList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 显示index中的数据记录

        /// <summary>
        /// 显示index中的数据记录
        /// </summary>
        /// <param name="ApplyTime">申请时间</param>
        /// <param name="State">使用状态</param>
        /// <param name="UseState">审核状态</param>
        /// <returns></returns>
        public int TraChooseAmount(string ApplyTime, string State, string UseState, string SupplierName)
        {
            string where = " TC.State != 10 AND TC.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(SupplierName))
            {
                where += string.Format(" And TC.TraChooseId IN ( SELECT TraChooseId FROM TraChooseDetail WHERE TransportId IN( SELECT TransportId FROM SupplierTransport WHERE SupplierId IN (SELECT SupplierId FROM Supplier where SupplierName like '%{0}%')))", SupplierName.Trim());
            }

            // 申请时间
            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            // 使用状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And TC.State = {0}", State.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(UseState))
            {
                where += string.Format(" And TC.UseState = {0}", UseState.Trim());
            }

            return bll.TraChooseAmount(where);
        }
        #endregion

        #region 新增时：主表明细表同时新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddTraChoose(TraChooseModel model)
        {

            model.State = 0;// 状态 初始

            model.UseState = 0;// 审核状态 初始
             
            model.NotificationType = 0;// 招标状态:0-初始;1-使用；2-结束

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            model.TraChooseNumber = Auxiliary.CurCompanyAutoNum("TRC");//运输选择编号

            int TraChooseId = bll.AddTraChoose(model);

            if (TraChooseId > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.TraChooseBeginAdjunct))
                {
                    List<temTraChooseBeginAdjunct> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temTraChooseBeginAdjunct>>(model.TraChooseBeginAdjunct);
                    tcbll.AddFilesForSupplier(fflist, TraChooseId, ref filenames);
                }
                model.TraChooseBeginAdjunct = filenames;

                if (!string.IsNullOrEmpty(model.TraChooseIdList))
                {
                    List<string> traChooseIdList = new List<string>(model.TraChooseIdList.Split(','));
                    if (traChooseIdList.Count > 10)
                    {
                        Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
                        return Json(new { flag = "error" });
                    }
                    else
                    {
                        bll.AddTraChooseDetail(traChooseIdList, TraChooseId);
                    }
                }
                if (!string.IsNullOrEmpty(model.ChooseIdList))
                {
                    List<string> traChooseIdList = new List<string>(model.ChooseIdList.Split(','));

                    bll.AddTraChooseLine(traChooseIdList, TraChooseId);
                }
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 选择线路
        /// <summary>
        /// 明细新增 运作分配供应商表
        /// </summary>
        /// <param name="supperMatchingIds">供应商匹配id</param>
        /// <param name="operationClaimId">运作id</param>
        /// <returns></returns>
        public ActionResult AddTraChooseLine(string LineId, int trachooseid)
        {
            // 影响行数
            int row = 0;

            if (!string.IsNullOrEmpty(LineId))
            {
                List<string> LineIdList = new List<string>(LineId.Split(','));
                row = bll.AddTraChooseLine(LineIdList, trachooseid);
            }

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 编辑时：保存主表的数据

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraChoose(TraChooseModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            TraChooseModel beforeModel = bll.GetModelByID(model.TraChooseId);

            int result = bll.EditTraChoose(model);
            if (result > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.TraChooseBeginAdjunct))
                {
                    List<temTraChooseBeginAdjunct> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temTraChooseBeginAdjunct>>(model.TraChooseBeginAdjunct);
                    tcbll.AddFilesForSupplier(fflist, model.TraChooseId, ref filenames);
                }
                model.TraChooseBeginAdjunct = filenames;

                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }

        #endregion
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Revoke)]
        public ActionResult RevokeData(int tId)
        {
            // 更新之前table
            TraChooseModel beforeModel = bll.GetModelByID(tId);

            // 流程开始已提（非撤销状态）的数量 
            int count = 0;

            // 查询本公司的签订合同是否配置(关键节点表（BasisKeyNode）
            // 关键节点编号 关键节点字段 当前时间(是否在开始至结束至简) (查询非作废状态) 
            BasisKeyNodeModel keynodemodel = keynodebll.GetModelByName("TraChoose", "MonthCost", Auxiliary.CompanyID());

            // 在有效期内在查看最小值为0或1(为0 否签订 为1 是签订)  
            if (keynodemodel != null && beforeModel.MonthCost > keynodemodel.MinValue)
            {
                // 如果大于配置信息则使用供应商审核关系维护中关键节点为是的信息 
                BasisAuditRelationModel relationmodel = relationbll.IsMatchForControl(4, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID(), "Control");

                // 若审核流程不为NULL
                if (relationmodel != null)
                {
                    // 查询提交的数据是否已审核(流程开始) 根据本数据ID(提交表ID) 流程ID(审核流程ID) 查询审核表(TraChooseAuditModel)
                    TraChooseAuditModel model = new TraChooseAuditBLL().GetRevokeModel(tId, relationmodel.AuditRelationId);

                    // 如果未审核
                    if (model.State == 0)
                    {
                        // 查询流程开始已提（非撤销状态）的数量
                        count = new TraChooseAuditBLL().GetRevokeCouunt(tId, relationmodel.AuditRelationId);

                        if (count > 1)
                        {
                            // 修改本数据状态为驳回状态
                            bll.ChangeStatechoose(tId, 10);

                            // 修改本数据状态为初始状态
                            bll.ChangeStates(tId, 0);

                            // 修改审核表数据状态为撤销状态
                            new TraChooseAuditBLL().ChangeState(model.SupplierAuditId, 10, "");

                            Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "撤销", Id = tId, State = "驳回状态" });
                            return Json(new { flag = "success", content = "撤销成功！" });
                        }
                        else
                        {
                            // 修改本数据审核状态为初始状态
                            bll.ChangeStatechoose(tId, 0);

                            // 修改本数据状态为初始状态
                            bll.ChangeStates(tId, 0);

                            // 修改审核表数据状态为撤销状态
                            new TraChooseAuditBLL().ChangeState(model.SupplierAuditId, 10, "");

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
                BasisAuditRelationModel NoControlmodel = new BasisAuditRelationBLL().IsMatchForControl(4, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID(), "");

                // 若审核流程不为NULL
                if (NoControlmodel != null)
                {
                    // 查询提交的数据是否已审核(流程开始) 根据本数据ID(提交表ID) 流程ID(审核流程ID) 查询审核表(TraChooseAuditModel)
                    TraChooseAuditModel model = new TraChooseAuditBLL().GetRevokeModel(tId, NoControlmodel.AuditRelationId);

                    // 如果未审核
                    if (model.State == 0)
                    {
                        // 查询流程开始已提（非撤销状态）的数量
                        count = new TraChooseAuditBLL().GetRevokeCouunt(tId, NoControlmodel.AuditRelationId);

                        if (count > 1)
                        {
                            // 修改本数据状态为驳回状态
                            bll.ChangeStatechoose(tId, 10);

                            // 修改本数据状态为初始状态
                            bll.ChangeStates(tId, 0);

                            // 修改审核表数据状态为撤销状态
                            new TraChooseAuditBLL().ChangeState(model.SupplierAuditId, 10, "");

                            Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "撤销", Id = tId, State = "驳回状态" });
                            return Json(new { flag = "success", content = "撤销成功！" });
                        }
                        else
                        {
                            // 修改本数据状态为初始状态
                            bll.ChangeStatechoose(tId, 0);

                            // 修改本数据状态为初始状态
                            bll.ChangeStates(tId, 0);

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

        #region 编辑时；新增明细表 

        /// <summary>
        /// 编辑时；新增明细表
        /// </summary>
        /// <param name="chooseDetail">选择明细</param>
        /// <param name="checkSupplierChooseId">供应商选择id</param>
        /// <returns></returns>
        public ActionResult AddTraChooseDetails(string chooseDetail, string checkSupplierChooseId)
        {
            List<string> traChooseDetailList = new List<string>(chooseDetail.Split(','));

            if (traChooseDetailList.Count > 10)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
                return Json(new { flag = "error" });
            }
            else
            {
                int row = bll.AddTraChooseDetail(traChooseDetailList, Convert.ToInt32(checkSupplierChooseId));

                if (row > 0)
                {
                    Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                    return Json(new { flag = "success", content = "添加成功！" });
                }
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
                return Json(new { flag = "fail" });
            }
        }
        #endregion

        #region 新增按钮时：显示下方的仓储选择明细数据集list

        /// <summary>
        /// 新增按钮时：显示下方的仓储选择明细数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="checkTraChooseId"></param>
        /// <returns></returns>
        public ActionResult TraChoosedetailaddList(int index, int size, string checkTraChooseId)
        {

            string where = string.Format(" ST.TransportId in ({0})", checkTraChooseId);

            List<TraChooseModel> list = bll.TraChoosedetailaddList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 新增按钮时：显示下方的仓储选择明细数据记录count

        /// <summary>
        /// 新增按钮时：显示下方的仓储选择明细数据记录count
        /// </summary>
        /// <param name="checkTraChooseId"></param> 
        /// <returns></returns>
        public int TraChoosedetailaddAmount(string checkTraChooseId)
        {

            string where = string.Format(" ST.TransportId in ({0})", checkTraChooseId);

            return bll.TraChoosedetailaddAmount(where);
        }
        #endregion

        #region 选择供应商 作废
         
        /// <summary>
        /// 作废 选择供应商
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidDetail(int tId)
        {
            // 作废之前Model
            TraChooseModel beforeModel = bll.GetdetailModelByID(tId);

            // 作废(更改状态)
            int row = bll.InvalidDetail(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }

        #endregion

        #region 选择线路表 作废
         
        /// <summary>
        /// 作废 选择线路表
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidLine(int tId)
        {
            // 作废之前Model
            TraChooseModel beforeModel = bll.GetLineModelByID(tId);

            // 作废(更改状态)
            int row = bll.InvalidLine(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }

        #endregion

        #region 新增按钮时：显示下方的仓储选择明细数据集list

        /// <summary>
        /// 新增按钮时：显示下方的仓储选择明细数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="traChooseId"></param>
        /// <returns></returns>
        public ActionResult TraChooseLineaddList(int index, int size, string traChooseId)
        {

            string where = string.Format(" BL.LineId in ({0})", traChooseId);

            List<TraChooseModel> list = bll.TraChooseLineaddList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 新增按钮时：显示下方的仓储选择明细数据记录count

        /// <summary>
        /// 新增按钮时：显示下方的仓储选择明细数据记录count
        /// </summary>
        /// <param name="traChooseId"></param> 
        /// <returns></returns>
        public int TraChooselineaddAmount(string traChooseId)
        {

            string where = string.Format(" BL.LineId in ({0})", traChooseId);

            return bll.TraChooselineaddAmount(where);
        }
        #endregion

        #region 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   

        /// <summary>
        /// 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   
        /// </summary> 
        /// <param name="checktraChooseId"></param>
        /// <returns></returns>
        public ActionResult TraChoosedetaileditList(string checktraChooseId)
        {

            string where = string.Format(" TC.TraChooseId in ({0})  and TCD.DetailState!=10", checktraChooseId);

            List<TraChooseModel> list = bll.TraChoosedetaileditList(where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 编辑按钮时：显示下方的仓储选择明细数据记录count 

        /// <summary>
        /// 编辑按钮时：显示下方的仓储选择明细数据记录count
        /// </summary>
        /// <param name="checktraChooseId"></param> 
        /// <returns></returns>
        public int TraChoosedetaileditAmount(string checktraChooseId)
        {
            string where = string.Format(" TC.TraChooseId in ({0})  and TCD.DetailState!=10", checktraChooseId);

            return bll.TraChoosedetaileditAmount(where);
        }
        #endregion

        #region 新增时和编辑时： 弹出的仓储供应商数据集list 

        /// <summary>
        /// 新增时和编辑时： 弹出的仓储供应商数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="supplierName"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult SupplierStoragelist(int index, int size, string supplierName, string distributionScope, string type, string ids, string idss)
        {
            string str = idss;
            int count = str.Count(ch => ch == ',') + 1;
            string where = string.Format("  ST.DepartmentId={0} and (ST.TransportState='F4' OR ST.TransportState='F2') AND ST.TransportId IN (select TransportId from SupplierTransportLine where LineId IN (" + idss + ") AND SupplierTransportLine.State>0 group by TransportId having count(TransitLineId) = (" + count + "))  ", Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And ST.TransportId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And ST.TransportId NOT IN (SELECT TransportId FROM TraChooseDetail WHERE TraChooseId ={0} and DetailState != 10)", ids.Trim());
                }
            }
            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 配送供应商范围
            if (!string.IsNullOrEmpty(distributionScope))
            {
                if (distributionScope == "干线" || distributionScope == "终端" || distributionScope == "综合")
                {
                    where += string.Format(" And bd2.DictionaryName = '{0}'", distributionScope.Trim());
                }
                else if (distributionScope == "干线/综合")
                {
                    where += string.Format(" And bd2.DictionaryName IN ('干线','综合')");
                }
                else if (distributionScope == "终端/综合")
                {  
                    where += string.Format(" And bd2.DictionaryName IN ('终端','综合')");
                }
            }

            List<TraChooseModel> list = bll.SupplierStoragelist(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 新增时和编辑时： 弹出的仓储供应商数据集count 

        /// <summary>
        /// 新增时和编辑时： 弹出的仓储供应商数据集count
        /// </summary>
        /// <param name="supplierName"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int SupplierStorageAmount(string supplierName, string distributionScope, string type, string ids, string idss)
        {
            string str = idss;
            int count = str.Count(ch => ch == ',') + 1;
            string where = string.Format("  ST.DepartmentId={0} and (ST.TransportState='F4' OR ST.TransportState='F2') AND ST.TransportId IN (select TransportId from SupplierTransportLine where LineId IN (" + idss + ") AND SupplierTransportLine.State>0 group by TransportId having count(TransitLineId) = (" + count + "))  ", Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And ST.TransportId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And ST.TransportId NOT IN (SELECT TransportId FROM TraChooseDetail WHERE TraChooseId ={0} and DetailState != 10)", ids.Trim());
                }
            }
            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 配送供应商范围
            if (!string.IsNullOrEmpty(distributionScope))
            {
                if (distributionScope == "干线" || distributionScope == "终端" || distributionScope == "综合")
                {
                    where += string.Format(" And bd2.DictionaryName = '{0}'", distributionScope.Trim());
                }
                else if (distributionScope == "干线/综合")
                {
                    where += string.Format(" And bd2.DictionaryName IN ('干线','综合')");
                }
                else if (distributionScope == "终端/综合")
                {
                    where += string.Format(" And bd2.DictionaryName IN ('终端','综合')");
                }
            }
            return bll.SupplierStorageAmount(where);
        }
        #endregion

        #region 提交按钮逻辑

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitTraChoose(int tId)
        {

            // 影响行数
            int row = 0;

            // 更新之前table
            TraChooseModel beforeModel = bll.GetModelByID(tId);

            BasisKeyNodeModel keynodemodel = keynodebll.GetModelByName("TraChoose", "MonthCost", Auxiliary.CompanyID());

            if (keynodemodel != null && beforeModel.MonthCost > keynodemodel.MinValue)
            {
                //是否有匹配审核流程Model 供应商审核关系维护表 
                BasisAuditRelationModel relationmodel = relationbll.IsMatchForControl(4, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID(), "Control");

                if (relationmodel != null)
                {
                    bll.SubmitTraChoose(tId);
                    TraChooseAuditModel Auditmodel = new TraChooseAuditModel();
                    Auditmodel.AuditRelationNumber = relationmodel.AuditRelationNumber;
                    Auditmodel.AuditRelationId = relationmodel.AuditRelationId;
                    Auditmodel.OtherId = 0;
                    Auditmodel.PresentId = tId;
                    Auditmodel.SupplierAuditType = relationmodel.AuditRelationType;
                    Auditmodel.PresentDepartmentId = relationmodel.DepartmentId;
                    Auditmodel.PresentUserId = relationmodel.UserId;
                    Auditmodel.AuditDepartmentId = relationmodel.ToDepartmentId;
                    Auditmodel.AuditUserId = relationmodel.ToUserId;
                    Auditmodel.AuditRelationName = relationmodel.AuditRelationName;
                    Auditmodel.CompanyId = relationmodel.CompanyId;

                    // 默认状态 未审核
                    Auditmodel.State = 0;
                    TraChooseAuditBLL auditBLL = new TraChooseAuditBLL();

                    row = auditBLL.AddAuditRelation(Auditmodel);
                    if (row > 0)
                    {
                        Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                        return Json(new { flag = "success", content = "提交成功！" });
                    }

                    // 关键节点表数据改成使用状态
                    //keynodebll.ChangeState(keynodemodel.Id, 1);
                }
                else
                {
                    // 系统日志
                    Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
                    return Json(new { flag = "fail", content = "无匹配审核流程！" });
                }
            }
            else
            {
                //是否有匹配审核流程Model 供应商审核关系维护表 
                BasisAuditRelationModel model = relationbll.IsMatchForControl(4, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID(), "");

                if (model != null)
                {
                    bll.SubmitTraChoose(tId);
                    TraChooseAuditModel Auditmodel = new TraChooseAuditModel();
                    Auditmodel.AuditRelationNumber = model.AuditRelationNumber;
                    Auditmodel.AuditRelationId = model.AuditRelationId;
                    Auditmodel.OtherId = 0;
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
                    TraChooseAuditBLL auditBLL = new TraChooseAuditBLL();

                    row = auditBLL.AddAuditRelation(Auditmodel);
                    if (row > 0)
                    {
                        Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                        return Json(new { flag = "success", content = "提交成功！" });
                    }
                }
                else
                {
                    // 系统日志
                    Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
                    return Json(new { flag = "fail", content = "无匹配审核流程！" });
                }
            }
            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail", content = "无匹配审核流程,不能提交！" });
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
            TraChooseModel beforeModel = bll.GetModelByID(tId);

            int delUserId = Auxiliary.UserID();

            int row = bll.InvalidState(tId, delUserId);
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 导出按钮逻辑

        /// <summary>
        /// 导出按钮逻辑
        /// </summary>
        /// <param name="ApplyTime"></param>
        /// <param name="State"></param>
        /// <param name="UseState"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string ApplyTime, string State, string UseState, string SupplierName)
        {
            string where = " TC.State != 10 AND TC.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(SupplierName))
            {
                where += string.Format(" And TC.TraChooseId IN ( SELECT TraChooseId FROM TraChooseDetail WHERE TransportId IN( SELECT TransportId FROM SupplierTransport WHERE SupplierId IN (SELECT SupplierId FROM Supplier where SupplierName like '%{0}%')))", SupplierName.Trim());
            }

            // 申请时间
            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            // 使用状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And TC.State = {0}", State.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(UseState))
            {
                where += string.Format(" And TC.UseState = {0}", UseState.Trim());
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

        #region 新增时和编辑时： 弹出的仓储供应商数据集list 

        /// <summary>
        /// 新增时和编辑时： 弹出的仓储供应商数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="lineName"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult TraChooseLineballlist(int index, int size, string lineName, string type, string ids)
        {
            string where = string.Format("  BL.CreateDepartmentId={0} AND  BL.State=1 ", Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And BL.LineId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And BL.LineId NOT IN (SELECT LineId FROM TraChooseLine WHERE TraChooseId ={0} and State != 0)", ids.Trim());
                }
            }
            // 线路名称
            if (!string.IsNullOrEmpty(lineName))
            {
                where += string.Format(" And BL.LineName LIKE '%{0}%'", lineName.Trim());
            }
            List<TraChooseModel> list = bll.TraChooseLineballlist(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 新增时和编辑时： 弹出的仓储供应商数据集list 

        /// <summary>
        /// 新增时和编辑时： 弹出的仓储供应商数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="lineName"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns> 
        public int TraChooseLineballAmount(string lineName, string type, string ids)
        {
            string where = string.Format("  BL.CreateDepartmentId={0} AND  BL.State=1 ", Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And BL.LineId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And BL.LineId NOT IN (SELECT LineId FROM TraChooseLine WHERE TraChooseId ={0})", ids.Trim());
                }
            }
            // 线路名称
            if (!string.IsNullOrEmpty(lineName))
            {
                where += string.Format(" And BL.LineName LIKE '%{0}%'", lineName.Trim());
            }

            return bll.TraChooseLineballAmount(where);
        }
        #endregion

        #region 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   

        /// <summary>
        /// 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   
        /// </summary> 
        /// <param name="OperationRecordId"></param>
        /// <returns></returns>
        public ActionResult TraChooseeditList(string traChooseId)
        {

            string where = string.Format(" TC.TraChooseId in ({0}) and TCL.State!=0", traChooseId);

            List<TraChooseModel> list = bll.TraChooseeditList(where);

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
        public int TraChooseeditAmount(string traChooseId)
        {
            string where = string.Format(" TC.TraChooseId in ({0}) and TCL.State!=0", traChooseId);

            return bll.TraChooseeditAmount(where);
        }
        #endregion

        #endregion

    }
}
