//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-28    1.0        FJK        新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.Model.Supplier;
using System.Linq;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using SRM.BLL.Supplier;
#endregion
/*********************************
 * 类名：TraAbnormityRecordController
 * 功能描述：异常整改记录表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraAbnormityRecordController : Controller
    {
        //
        // GET: /Tra/TraAbnormityRecord/

        // 异常整改记录BLL
        TraAbnormityRecordBLL bll = new TraAbnormityRecordBLL();

        // 异常整改记录附件BLL
        TraAbnormityRecordAdjunctBLL tirbll = new TraAbnormityRecordAdjunctBLL();

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
           ViewBag.StorageNumber = Auxiliary.CurCompanyAutoNum("RAN");
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
            TraAbnormityRecordModel model = bll.GetModelByID(tId);

            // 运输供应商Model
            SupplierTransportModel TRAmodel = new SupplierTransportBLL().GetModelById(model.TransportId);

            // 供应商名称 
            ViewBag.SupplierName = TRAmodel.SupplierName;

            // 运输供应商编号
            ViewBag.TransportNumber = TRAmodel.TransportNumber;

            // 运输供应商类型 
            ViewBag.SupplierTypeName = TRAmodel.SupplierTypeName;

            // 附件
            List<temfile> fileList = tirbll.AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList); 

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        { 
            // 获取数据
            TraAbnormityRecordModel model = bll.GetModelByID(tId);

            // 运输供应商Model
            SupplierTransportModel TRAmodel = new SupplierTransportBLL().GetModelById(model.TransportId);

            // 供应商名称 
            model.SupplierName = TRAmodel.SupplierName;

            // 运输供应商编号
            model.TransportNumber = TRAmodel.TransportNumber;

            // 运输供应商类型 
            model.SupplierTypeName = TRAmodel.SupplierTypeName;

            // 评估日期
            if (model.EvaluateTime == "")
            {
                ViewBag.EvaluateTime = "";
            }
            else
            {
                ViewBag.EvaluateTime = model.EvaluateTime;
            }

            // 跟进人 
            if (model.RectificationUser == "")
            {
                ViewBag.RectificationUser = "";
            }
            else
            {
                ViewBag.RectificationUser = model.RectificationUser;
            }

            // 整改评估状态
            if (model.EvaluateState == 0)
            {
                ViewBag.EvaluateState = "";
            }
            else
            {
                ViewBag.EvaluateState = model.EvaluateState;
            }

            // 整改效果评估 
            if (model.EffectEvaluate == "")
            {
                ViewBag.EffectEvaluate = "";
            }
            else
            {
                ViewBag.EffectEvaluate = model.EffectEvaluate;
            }

            // 附件
            List<temfile> fileList = tirbll.AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);

            return View(model);
        }
         
        /// <summary>
        /// 月绩效信息列表
        /// </summary>  
        public ActionResult MonthCheckDetail(string tUrl, string tIds, int tTransportId, int tTwoAbnormity, string tType)
        {
            // URL
            ViewBag.Url = tUrl;

            // 月绩效ID
            ViewBag.Ids = tIds;

            // 运输供应商ID
            ViewBag.TransportId = tTransportId;

            // 二次整改
            ViewBag.TwoAbnormity = tTwoAbnormity;

            // 新增/编辑
            ViewBag.Type = tType; 
            return View();
        }

        /// <summary>
        /// 巡查信息列表
        /// </summary>  
        public ActionResult PatrolRecordDetail(string tUrl, string tIds, int tTransportId, string tType)
        {
            ViewBag.Url = tUrl;
            ViewBag.Ids = tIds;
            ViewBag.TransportId = tTransportId;
            ViewBag.Type = tType;
            return View();
        }
         
        /// <summary>
        /// 评估
        /// </summary> 
        [Operate(Name = OperateEnum.Assess)]
        public ActionResult Assess(int tId)
        {
            // 获取数据
            TraAbnormityRecordModel model = bll.GetModelByID(tId);
            
            // 运输供应商Model
            SupplierTransportModel TRAmodel = new SupplierTransportBLL().GetModelById(model.TransportId);

            // 供应商名称 
            model.SupplierName = TRAmodel.SupplierName;

            // 运输供应商编号
            model.TransportNumber = TRAmodel.TransportNumber;

            // 运输供应商类型 
            model.SupplierTypeName = TRAmodel.SupplierTypeName;
             
            // 评估日期
            if (model.EvaluateTime == "")
            {
                ViewBag.EvaluateTime = "";
            }
            else
            {
                ViewBag.EvaluateTime = model.EvaluateTime;
            }

            // 跟进人 
            if (model.RectificationUser == "")
            {
                ViewBag.RectificationUser = "";
            }
            else
            {
                ViewBag.RectificationUser = model.RectificationUser;
            }

            // 整改评估状态
            if (model.EvaluateState == 0)
            {
                ViewBag.EvaluateState = "";
            }
            else
            {
                ViewBag.EvaluateState = model.EvaluateState;
            }

            // 整改效果评估 
            if (model.EffectEvaluate == "")
            {
                ViewBag.EffectEvaluate = "";
            }
            else
            {
                ViewBag.EffectEvaluate = model.EffectEvaluate;
            }

            // 附件
            List<temfile> fileList = tirbll.AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);

            return View(model);
        }

        /// <summary>
        /// 异常整改措施及自评（新增模块）
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int tId)
        {
            Model.Tra.TraAbnormityRecordModel model = new TraAbnormityRecordModel();
            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierType">供应商类型</param>
        /// <param name="supplierNumber">整改状态</param>
        /// <returns>Json</returns>
        public ActionResult AbnormityRecordList(int index, int size, string supplierName, string supplierType, string abnormityState)
        {
            // 查询本机构录入的所有的有效的异常整改信息
            string where = " AbnormityState NOT IN (10,15) AND CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 供应商类型
            if (!string.IsNullOrEmpty(supplierType))
            {
                where += string.Format(" And DictionaryId = {0}", supplierType.Trim());
            }

            // 整改状态
            if (!string.IsNullOrEmpty(abnormityState))
            {
                where += string.Format(" And AbnormityState = {0}", abnormityState.Trim());
            }   

            // 异常整改记录List
            List<TraAbnormityRecordModel> list = bll.AbnormityRecordList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierType">供应商类型</param>
        /// <param name="supplierNumber">整改状态</param>
        /// <returns></returns>
        public int AbnormityRecordCount(string supplierName, string supplierType, string abnormityState)
        {
            // 查询本机构录入的所有的有效的异常整改信息
            string where = " AbnormityState NOT IN (10,15) AND CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 供应商类型
            if (!string.IsNullOrEmpty(supplierType))
            {
                where += string.Format(" And DictionaryId = {0}", supplierType.Trim());
            }

            // 整改状态
            if (!string.IsNullOrEmpty(abnormityState))
            {
                where += string.Format(" And AbnormityState = {0}", abnormityState.Trim());
            }

            return bll.AbnormityRecordCount(where);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddAbnormityRecord(TraAbnormityRecordModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            //整改计划编号 TAB开头 8位流水号
            tModel.AbnormityNumber = Auxiliary.CurCompanyAutoNum("TAB");

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建账号ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认初始状态0
            tModel.AbnormityState = 0;

            // 使用状态默认未使用0
            tModel.UseState = 0;
               
            // 新增(返回主键ID)
            int AbnormityId = bll.AddAbnormityRecord(tModel);

            // 若主键>O(新增成功)
            if (AbnormityId > 0)
            {
                // 月度绩效信息 年度绩效结果明细表
                if (!string.IsNullOrEmpty(tModel.CheckIdList))
                {
                    List<string> checkIdList = new List<string>(tModel.CheckIdList.Split(','));
                    new TraAbnormityRecordDetailBLL().AddRecordDetailList(checkIdList, AbnormityId, 0);
                }

                // 巡查信息 年度绩效结果明细表
                if (!string.IsNullOrEmpty(tModel.RecordIdList))
                {
                    List<string> recordIdList = new List<string>(tModel.RecordIdList.Split(','));
                    new TraAbnormityRecordDetailBLL().AddRecordDetailList(recordIdList, AbnormityId, 1);
                }
                 
                // 附件
                if (!string.IsNullOrEmpty(tModel.FileList))
                {
                    List<temfile> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.FileList);
                    new TraAbnormityRecordAdjunctBLL().AddFiles(fileList, AbnormityId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }
            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditAbnormityRecord(TraAbnormityRecordModel tModel)
        {
            // 编辑之前Model
            TraAbnormityRecordModel beforeModel = bll.GetModelByID(tModel.AbnormityId);

            // 编辑
            int row = bll.EditAbnormityRecord(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
            { 
                // 附件
                if (!string.IsNullOrEmpty(tModel.FileList))
                {
                    List<temfile> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.FileList);
                    new TraAbnormityRecordAdjunctBLL().AddFiles(fileList, tModel.AbnormityId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel);

                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 评估
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AssessAbnormityRecord(TraAbnormityRecordModel tModel)
        {
            // 编辑之前Model
            TraAbnormityRecordModel beforeModel = bll.GetModelByID(tModel.AbnormityId);

            // 评估
            int row = bll.AssessAbnormityRecord(tModel);
             
            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 异常整改记录状态待评估改为已评估
                if (beforeModel.AbnormityState == 2)
                {
                    row = bll.ChangeState(tModel.AbnormityId, 5);
                }

                // 如果整改状态为二次整改 修改二次整改为是
                if (tModel.EvaluateState == 2 && tModel.TwoAbnormity == 0)
                {
                    bll.ChangeTwoAbnormity(tModel.AbnormityId, 1);
                }

                // 附件
                if (!string.IsNullOrEmpty(tModel.FileList))
                {
                    List<temfile> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.FileList);
                    new TraAbnormityRecordAdjunctBLL().AddFiles(fileList, tModel.AbnormityId);
                }

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
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitState(int tId)
        {
            int row = 0;

            // 提交之前Model
            TraAbnormityRecordModel beforeModel = bll.GetModelByID(tId);

            // 提交时状态为计划制作0 提交后变更为执行计划1
            if (beforeModel.AbnormityState == 0)
            {
                row = bll.ChangeState(tId,1);
            }

            // 提交时状态为评估(待评估,已评估)状态时
            if (beforeModel.AbnormityState == 2 || beforeModel.AbnormityState == 5)
            {
                // 若整改评估状态为整改通过1时提交后变更状态为整改通过3
                if (beforeModel.EvaluateState == 1)
                {
                    // 变更状态为整改通过3
                    row = bll.ChangeState(tId, 3);
                     
                    // 如果整改评估状态为整改通过，则将对应的运输月度考核（TraMonthCheck）的整改状态变为整改结束
                    // 将对应的巡查记录表（TraPatrolRecord）的整改完成2-整改已完成
                    new TraAbnormityRecordDetailBLL().SumbitAbnormityRecord(tId);
                }

                // 若整改评估状态为二次整改2 时提交后变更状态为二次整改4
                if (beforeModel.EvaluateState == 2)
                {
                    row = bll.ChangeState(tId, 4);
                } 
            }
             
            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "提交", Id = tId, State = "提交" });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "提交", Id = tId, State = "初始" });
            return Json(new { flag = "fail", content = "提交失败！" });
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
            TraAbnormityRecordModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 如果异常整改记录不是二次整改的将运输月度考核（TraMonthCheck）的整改状态变为1-需要整改
                if (beforeModel.AbnormityState != 4)
                {
                    new TraAbnormityRecordDetailBLL().InvalidRecordDetailById(tId);
                }

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
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierNumber">供应商编号</param>
        /// <param name="supplierType">供应商类型</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string supplierType, string abnormityState)
        {
            // 查询本机构录入的所有的有效的异常整改信息
            string where = " AbnormityState NOT IN (10,15) AND CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 供应商类型
            if (!string.IsNullOrEmpty(supplierType))
            {
                where += string.Format(" And DictionaryId = {0}", supplierType.Trim());
            }

            // 整改状态
            if (!string.IsNullOrEmpty(abnormityState))
            {
                where += string.Format(" And AbnormityState = {0}", abnormityState.Trim());
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where);

            // guid
            string guid = string.Empty;

            // 模板文件路径
            string TemplatePath = System.Web.HttpContext.Current.Server.MapPath(@"/upload/export/Template/TraAbnormityRecord.xls");

            // 模板导出
            Auxiliary.TemplateExport(dt, TemplatePath, "A3", "TraAbnormityRecord", ref guid);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new
            {
                Detail = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });
            return Json(new { flag = "success", guid = guid });
        }

        #region 月度绩效信息列表

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="transportId">运输供应商ID</param> 
        /// <param name="twoAbnormity">二次整改</param> 
        /// <returns>Json</returns>
        public ActionResult MonthCheckList(int index, int size, string tTransportId, string tTwoAbnormity, string tYear, string tIds, string tType)
        {
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询同运输供应商的提交的需要整改的考核信息 
                where = string.Format(" TransportId = {0} AND State = 5", tTransportId);

                if (tTwoAbnormity =="0")
                {
                    where += string.Format(" And IsNorm = {0}", 1);
                }
                else if (tTwoAbnormity == "1")
                {
                    where += string.Format(" And IsNorm = {0}", 2);
                }

                // 考核年
                if (!string.IsNullOrEmpty(tYear))
                {
                    where += string.Format(" And CheckYear = {0}", tYear.Trim());
                }

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And CheckId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And CheckId NOT IN (SELECT OtherId FROM TraAbnormityRecordDetail WHERE AbnormityId = {0} AND DetailType = 0 AND DetailState = 1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where += string.Format(" CheckId IN ({0})", tIds.Trim());
            }

            // 异常整改记录List
            List<TraMonthCheckModel> list = new TraMeasuresAndSelfEvaluationBLL().MonthCheckList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="transportId">运输供应商ID</param> 
        /// <param name="twoAbnormity">二次整改</param> 
        /// <returns></returns>
        public int MonthCheckCount(string tTransportId, string tTwoAbnormity, string tYear, string tIds, string tType)
        {
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询同运输供应商的提交的需要整改的考核信息 
                where = string.Format(" TransportId = {0} AND State = 5", tTransportId);

                if (tTwoAbnormity == "0")
                {
                    where += string.Format(" And IsNorm = {0}", 1);
                }
                else if (tTwoAbnormity == "1")
                {
                    where += string.Format(" And IsNorm = {0}", 2);
                }

                // 考核年
                if (!string.IsNullOrEmpty(tYear))
                {
                    where += string.Format(" And CheckYear = {0}", tYear.Trim());
                }

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And CheckId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And CheckId NOT IN (SELECT OtherId FROM TraAbnormityRecordDetail WHERE AbnormityId = {0} AND DetailType = 0 AND DetailState = 1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where += string.Format(" CheckId IN ({0})", tIds.Trim());
            }

            return new TraMeasuresAndSelfEvaluationBLL().MonthCheckCount(where);
        }

        #endregion

        #region 巡查信息

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="transportId">运输供应商ID</param> 
        /// <param name="twoAbnormity">二次整改</param> 
        /// <returns>Json</returns>
        public ActionResult PatrolRecordList(int index, int size, string tTransportId, string tIds, string tType)
        {
            string where = "";

            // 是否按照ID查询
            if (tType!= null)
            {
                // 查询同运输供应商的提交需要整改的且整改状态为整改未完成的巡查记录信息 
                where = string.Format(" TransportId = {0} AND RecordState = 1 AND NormState = 1 AND IsNorm = 1", tTransportId);

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And RecordId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And RecordId NOT IN (SELECT OtherId FROM TraAbnormityRecordDetail WHERE AbnormityId = {0} AND DetailType = 1 AND DetailState = 1)", tIds.Trim());
                    }
                } 
            }
            else
            {
                where = string.Format(" RecordId IN ({0})", tIds.Trim());
            }

            // 异常整改记录List
            List<TraPatrolRecordModel> list = new TraMeasuresAndSelfEvaluationBLL().PatrolRecordList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="transportId">运输供应商ID</param> 
        /// <param name="twoAbnormity">二次整改</param> 
        /// <returns></returns>
        public int PatrolRecordCount(string tTransportId, string tIds, string tType)
        {
            string where = "";

            // 是否按照ID查询
            if (tType != null)
            {
                // 查询同运输供应商的提交需要整改的且整改状态为整改未完成的巡查记录信息 
                where = string.Format(" TransportId = {0} AND RecordState = 1 AND NormState = 1 AND IsNorm = 1", tTransportId);

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And RecordId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And RecordId NOT IN (SELECT OtherId FROM TraAbnormityRecordDetail WHERE AbnormityId = {0} AND DetailType = 1 AND DetailState = 1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where = string.Format(" RecordId IN ({0})", tIds.Trim());
            }

            return new TraMeasuresAndSelfEvaluationBLL().PatrolRecordCount(where);
        }

        #endregion

        #region 结果明细表

        /// <summary>
        /// 批量新增 结果明细表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddRecordDetailList(string tIds, int tAbnormityId, int tDetailType)
        {
            // 反序列化
            List<string> DetailList = new List<string>(tIds.Split(','));

            // 新增(返回主键ID)
            int row = new TraAbnormityRecordDetailBLL().AddRecordDetailList(DetailList, tAbnormityId, tDetailType);

            // 若主键>O(新增成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="tAbnormityId">异常记录Id</param>
        /// <param name="tDetailType">类型</param> 
        /// <returns>Json</returns>
        public ActionResult RecordDetailList(int index, int size, string tAbnormityId, string tDetailType)
        {
            // 根据主表AbnormityId和类型DetailType查询明细表中所属的状态为有效的数据
            string where = string.Format(" AND AbnormityId = {0} AND DetailType = {1}", tAbnormityId, tDetailType);

            // 结果明细List
            List<TraAbnormityRecordDetailModel> list = new List<TraAbnormityRecordDetailModel>();

            if (tDetailType == "0")
            {
                list = new TraAbnormityRecordDetailBLL().RecordDetailOfMonthCheckList(index, size, where);
            }
            else if (tDetailType == "1")
            {
                list = new TraAbnormityRecordDetailBLL().RecordDetailOfPatrolRecordList(index, size, where);
            } 

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="tAbnormityId">异常记录Id</param>
        /// <param name="tDetailType">类型</param> 
        /// <returns></returns>
        public int RecordDetailCount(string tAbnormityId, string tDetailType)
        {
            int count = 0;

            // 根据主表AbnormityId和类型DetailType查询明细表中所属的状态为有效的数据
            string where = string.Format(" AND AbnormityId = {0} AND DetailType = {1}", tAbnormityId, tDetailType);

            if (tDetailType == "0")
            {
                count = new TraAbnormityRecordDetailBLL().RecordDetailOfMonthCheckCount(where);
            }
            else if (tDetailType == "1")
            {
                count = new TraAbnormityRecordDetailBLL().RecordDetailOfPatrolRecordCount(where);
            } 

            return count;
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns> 
        public ActionResult InvalidRecordDetail(int tAbnormityDetailId, int tOtherId, int tDetailType)
        {
            // 作废(更改状态)
            int row = new TraAbnormityRecordDetailBLL().InvalidRecordDetail(tAbnormityDetailId, tOtherId, tDetailType);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                return Json(new { flag = "success", content = "作废成功！" });
            }

            return Json(new { flag = "fail", content = "作废失败！" });
        }

        #endregion
         
        #region  2018-10-18 20:09 米远添加新方法

        #region SubmitState_2 待评估前提交

        /// <summary>
        /// 待评估前提交
        /// </summary>
        /// <param name="abnormityId"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        [HttpPost]
        public ActionResult SubmitState_2(int abnormityId)
        {

            // 提交(更改状态)
            int row = bll.UpdateEvaluateStateByAbnormityId(abnormityId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "提交", Id = abnormityId, State = "提交" });
                return Json(new { flag = "success", content = "提交成功" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "提交", Id = abnormityId, State = "初始" });
            return Json(new { flag = "fail", content = "提交失败" });
        }

        #endregion
          
        /// <summary>
        /// 评估
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Assess(Model.Tra.TraAbnormityRecordModel model)
        {
            int count = 0;
            if (model.MonthCheckList != null || model.MonthCheckList != "")
            {
                List<Model.Tra.TraAbnormityRecordModel> monthCheckList =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.Tra.TraAbnormityRecordModel>>(model.MonthCheckList);
                if (monthCheckList != null)
                {
                    //string checkIds = string.Empty;
                    //for (int i = 0; i < monthCheckList.Count; i++)
                    //{
                    //    checkIds += monthCheckList[i].CheckId + ",";
                    //}

                    count = bll.UpdateAssessByAbnormityId(model, monthCheckList[0].CheckId);
                    if (count > 0)
                    {
                        // 系统日志
                        Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                        return Json(new { flag = "sucess" });
                    }
                }
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
          
        #endregion
         
        #endregion
    }
}
