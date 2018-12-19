// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-24    1.0        ZBB       新建                    
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
using System.Text;
using System.IO;
using Aspose.Cells;
#endregion
/*********************************
 * 类名：TraChooseEvaluateController
 * 功能描述：运输供应商选择评价表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraChooseEvaluateController : Controller
    {
        // 
        // GET: /Tra/TraChooseEvaluate/

        //运输供应商选择评价
        TraChooseEvaluateBLL bll = new TraChooseEvaluateBLL();

        //运输供应商选择评价附件表
        TraChoiceEvaluateAdjunctBLL abll = new TraChoiceEvaluateAdjunctBLL();

        //运输供应商选择评价附件表
        TraChoiceFromAdjunctBLL TCFbll = new TraChoiceFromAdjunctBLL();


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
        [Operate(Name = OperateEnum.Score)]
        public ActionResult Add(int tId)
        {
            ViewBag.TraChooseId = tId;

            List<Model.Tra.TraChooseEvaluateModel> evaluatecontentlist = bll.EvaluateContentList(Auxiliary.CompanyID());

            List<Model.Tra.TraChooseEvaluateModel> evaluatelist = bll.EvaluateList(Auxiliary.CompanyID());

            List<Model.Tra.TraChooseEvaluateModel> evaluatesuppliername = bll.EvaluateSupplierNameList(tId);

            int evaluatesuppliernamecount = bll.EvaluateSupplierNameCount(tId);

            int evaluatecontentcount = bll.EvaluateContentCount();
            if (evaluatecontentlist.Count != 0)
            {
                ViewBag.ChoiceFromId = evaluatecontentlist[0].ChoiceFromId;

                ViewBag.choiceevaluateid = 0;
            }
            return View(new SuppModels
            {
                EvaluateContentList = evaluatecontentlist,

                EvaluateList = evaluatelist,

                EvaluateSupplierNameList = evaluatesuppliername,

                EvaluateSupplierNameCount = evaluatesuppliernamecount,

                EvaluateContentCount = evaluatecontentcount,
            });

        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Score)]
        public ActionResult Edit(int tId)
        {
            //// 获取数据
            TraChooseEvaluateModel model = bll.GetModelByID(tId);

            TraChooseEvaluateModel choiceevaluateid = bll.GetModelByIDs(tId);

            // 附件
            List<TempChoiceEvaluateAdjunctModel> fileList = abll.AdjunctListById(choiceevaluateid.ChoiceEvaluateId);

            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);

            List<string> oldlist = fileList.Select(p => p.FileName + p.ext).ToList<string>();

            ViewBag.choiceevaluateid = choiceevaluateid.ChoiceEvaluateId;//运输评价选择id

            ViewBag.ChoiceEvaluateId = tId;//运输选择评价id

            ViewBag.trachooseid = model.TraChooseId;//运输选择id

            ViewBag.transportid = model.TransportId;//运输供应商id

            ViewBag.transportnumber = model.TransportNumber;//运输供应商编号

            ViewBag.choicefromid = model.ChoiceFromId;//运输选择评价自定义表单id

            ViewBag.evaluateuser = model.EvaluateUser;//评价负责人

            ViewBag.evaluatetime = model.EvaluateTime;//评价时间

            ViewBag.suppliername = model.SupplierName;//供应商名称

            ViewBag.evaluateremark = model.EvaluateRemark;//评价备注

            //显示上方的标题
            List<Model.Tra.TraChooseEvaluateModel> evaluatecontentlist = bll.EvaluateContentList(Auxiliary.CompanyID());

            //显示运输评价选择内容信息
            List<Model.Tra.TraChooseEvaluateModel> evaluateeditlist = bll.EvaluateEditList(tId);

            List<Model.Tra.TraChooseEvaluateModel> evaluatesuppliername = bll.EvaluateSupplierNameList(tId);

            int evaluatesuppliernamecount = bll.EvaluateSupplierNameCount(tId);

            int evaluatecontentcount = bll.EvaluateContentCount();

            return View(new SuppModels
            {
                EvaluateContentList = evaluatecontentlist,//显示上方的标题

                EvaluateEditList = evaluateeditlist,//显示运输评价选择内容信息

                EvaluateSupplierNameList = evaluatesuppliername,

                EvaluateSupplierNameCount = evaluatesuppliernamecount,

                EvaluateContentCount = evaluatecontentcount,
            });
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            //// 获取数据
            TraChooseEvaluateModel model = bll.GetModelByID(tId);

            TraChooseEvaluateModel choiceevaluateid = bll.GetModelByIDs(tId);

            // 附件
            List<TempChoiceEvaluateAdjunctModel> fileList = abll.AdjunctListById(choiceevaluateid.ChoiceEvaluateId);

            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);

            List<string> oldlist = fileList.Select(p => p.FileName + p.ext).ToList<string>();

            ViewBag.choiceevaluateid = choiceevaluateid.ChoiceEvaluateId;//运输评价选择id

            ViewBag.ChoiceEvaluateId = tId;//运输选择评价id

            ViewBag.trachooseid = model.TraChooseId;//运输选择id

            ViewBag.transportid = model.TransportId;//运输供应商id

            ViewBag.transportnumber = model.TransportNumber;//运输供应商编号

            ViewBag.choicefromid = model.ChoiceFromId;//运输选择评价自定义表单id

            ViewBag.evaluateuser = model.EvaluateUser;//评价负责人

            ViewBag.evaluatetime = model.EvaluateTime;//评价时间

            ViewBag.suppliername = model.SupplierName;//供应商名称

            ViewBag.evaluateremark = model.EvaluateRemark;//评价备注

            //显示上方的标题
            List<Model.Tra.TraChooseEvaluateModel> evaluatecontentlist = bll.EvaluateContentList(Auxiliary.CompanyID());

            //显示运输评价选择内容信息
            List<Model.Tra.TraChooseEvaluateModel> evaluateeditlist = bll.EvaluateEditList(tId);

            List<Model.Tra.TraChooseEvaluateModel> evaluatesuppliername = bll.EvaluateSupplierNameList(tId);

            int evaluatesuppliernamecount = bll.EvaluateSupplierNameCount(tId);

            int evaluatecontentcount = bll.EvaluateContentCount();

            return View(new SuppModels
            {
                EvaluateContentList = evaluatecontentlist,// 显示上方的标题

                EvaluateEditList = evaluateeditlist,//显示运输评价选择内容信息

                EvaluateSupplierNameList = evaluatesuppliername,

                EvaluateSupplierNameCount = evaluatesuppliernamecount,

                EvaluateContentCount = evaluatecontentcount,
            });
        }

        /// <summary>
        /// ContentFile
        /// </summary>
        /// <returns></returns>  
        public ActionResult EvaluateFile(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// 供应商信息
        /// </summary>
        public ActionResult TraSuppInfoDetail(string url, int tId)
        {
            ViewBag.TraChooseId = tId;
            ViewBag.url = url;
            return View();
        }

        #endregion

        #region 方法 

        #region 评分新增方法

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddEvaluateScoreList(TraChooseEvaluateModel tModel)
        {
            int row = 0;

            tModel.CreateDepartmentId = Auxiliary.DepartmentId();//机构id

            tModel.CreateUserId = Auxiliary.UserID();// 用户ID 

            tModel.State = 1;// 状态：有效 

            tModel.CompanyId = Auxiliary.CompanyID(); // 公司ID

            // 新增
            int ChoiceEvaluateId = bll.AddTraChooseEvaluate(tModel); // 主键ID

            if (!string.IsNullOrEmpty(tModel.ScoreList))
            {
                List<TempChooseEvaluateModel> scoreList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempChooseEvaluateModel>>(tModel.ScoreList);

                row = bll.ChooseEvaluateScoreList(scoreList, ChoiceEvaluateId);
            }
            else
            {
                return Json(new { flag = "fail" });
            }
            if (row > 0)
            {
                // 修改TraEvaluate得分(根据ChoiceEvaluateId)
                bll.EditScore(tModel.TraChooseId, tModel.TotalScore);

                bll.EditEvaluateinfo(tModel);

                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    List<TempChoiceEvaluateAdjunctModel> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempChoiceEvaluateAdjunctModel>>(tModel.AdjunctList);

                    abll.AddContentAdjunctList(adjunctList, ChoiceEvaluateId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });

            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 数据集 获取选择条件详细 

        /// <summary>
        /// 数据集 获取选择条件详细
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult EvaluateLists()
        {
            List<TraChooseEvaluateModel> evaluateLists = bll.EvaluateLists(Auxiliary.CompanyID());

            return Json(evaluateLists);
        }
        #endregion

        #region 数据集 评价内容标题 

        /// <summary>
        /// 数据集 评价内容标题
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult EvaluateList()
        {
            List<TraChooseEvaluateModel> evaluateList = bll.EvaluateList(Auxiliary.CompanyID());

            return Json(evaluateList);
        }
        #endregion

        #region 数据集 评价内容标题 

        /// <summary>
        /// 数据集 评价内容标题
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult EvaluateSupplierNameList(int tId)
        {
            List<TraChooseEvaluateModel> evaluateList = bll.EvaluateSupplierNameList(tId);

            return Json(evaluateList);
        }

        /// <summary>
        /// 数据集 评价内容标题
        /// </summary> 
        /// <returns>Json</returns>
        public int EvaluateSupplierNameCount(int tId)
        {
            return bll.EvaluateSupplierNameCount(tId);
        }
        /// <summary>
        /// 数据集 评价内容标题
        /// </summary> 
        /// <returns>Json</returns>
        public int EvaluateContentCount()
        {
            return bll.EvaluateContentCount();
        }
        #endregion

        #region 数据集 评价内容信息 

        /// <summary>
        /// 数据集 评价内容信息
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult EvaluatedownList()
        {
            List<TraChooseEvaluateModel> evaluatedownList = bll.EvaluatedownList();

            return Json(evaluatedownList);
        }
        #endregion

        #region 数据集 运输元件内容列表信息 

        /// <summary>
        /// 数据集 运输元件内容列表信息
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult EvaluateContentList()
        {
            List<TraChooseEvaluateModel> evaluateContentList = bll.EvaluateContentList(Auxiliary.CompanyID());

            return Json(evaluateContentList);
        }
        #endregion 

        #region 评分编辑按钮

        /// <summary>
        /// 评分编辑按钮
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraChooseEvaluate(TraChooseEvaluateModel tModel)
        {
            TraChooseEvaluateModel choiceevaluateid = bll.GetModelByIDs(tModel.TraChooseId);

            // 编辑之前Model
            TraChooseEvaluateModel beforeModel = bll.GetModelByChoiceEvaluateId(choiceevaluateid.ChoiceEvaluateId);

            // 影响行数
            int row = 0;

            if (!string.IsNullOrEmpty(tModel.ScoreList))
            {
                List<TempChooseEvaluateModel> scoreList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempChooseEvaluateModel>>(tModel.ScoreList);

                row = bll.ChooseEvaluateScoreList(scoreList, choiceevaluateid.ChoiceEvaluateId);
            }

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 修改Evaluate得分(根据ChoiceEvaluateId)
                bll.EditScore(tModel.TraChooseId, tModel.TotalScore);

                //修改元件基本信息
                bll.EditEvaluateinfo(tModel);

                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    List<TempChoiceEvaluateAdjunctModel> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempChoiceEvaluateAdjunctModel>>(tModel.AdjunctList);

                    abll.AddContentAdjunctList(adjunctList, choiceevaluateid.ChoiceEvaluateId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel);

                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 显示index中的数据集

        /// <summary>
        /// 显示index中的数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="applyTime">申请时间</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="evaluateState">评价状态</param>
        /// <param name="isNotification">启用招标</param>
        /// <param name="notificationType">招标状态</param>
        /// <param name="isEvaluate">是否评价</param>
        /// <returns></returns>
        public ActionResult TraChooseEvaluateList(int index, int size, string applyTime, string supplierName, string evaluateState, string isNotification, string notificationType, string isEvaluate)
        {
            string where = string.Format(" TC.UseState = 5 and TC.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 申请时间
            if (!string.IsNullOrEmpty(applyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", applyTime.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 评价审核状态
            if (!string.IsNullOrEmpty(evaluateState))
            {
                where += string.Format(" And TC.EvaluateState = {0}", evaluateState.Trim());
            }

            // 启用招标
            if (!string.IsNullOrEmpty(isNotification))
            {
                where += string.Format(" And TC.IsNotification = {0}", isNotification.Trim());
            }

            // 招标状态
            if (!string.IsNullOrEmpty(notificationType))
            {
                where += string.Format(" And TC.NotificationType = {0}", notificationType.Trim());
            }

            // 是否评价
            if (!string.IsNullOrEmpty(isEvaluate))
            {
                where += string.Format(" And TC.IsEvaluate = {0}", isEvaluate.Trim());
            }

            List<TraChooseEvaluateModel> list = bll.TraChooseEvaluateList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 显示index中的数据记录

        /// <summary>
        /// 显示index中的数据记录
        /// </summary>
        /// <param name="applyTime">申请时间</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="evaluateState">评价状态</param>
        /// <param name="isNotification">启用招标</param>
        /// <param name="notificationType">招标状态</param>
        /// <param name="isEvaluate">是否评价</param>
        /// <returns></returns>
        public int TraChooseEvaluateCount(string applyTime, string supplierName, string evaluateState, string isNotification, string notificationType, string isEvaluate)
        {
            string where = string.Format(" TC.UseState = 5 and TC.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 申请时间
            if (!string.IsNullOrEmpty(applyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", applyTime.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 评价审核状态
            if (!string.IsNullOrEmpty(evaluateState))
            {
                where += string.Format(" And TC.EvaluateState = {0}", evaluateState.Trim());
            }

            // 启用招标
            if (!string.IsNullOrEmpty(isNotification))
            {
                where += string.Format(" And TC.IsNotification = {0}", isNotification.Trim());
            }

            // 招标状态
            if (!string.IsNullOrEmpty(notificationType))
            {
                where += string.Format(" And TC.NotificationType = {0}", notificationType.Trim());
            }

            // 是否评价
            if (!string.IsNullOrEmpty(isEvaluate))
            {
                where += string.Format(" And TC.IsEvaluate = {0}", isEvaluate.Trim());
            }

            return bll.TraChooseEvaluateCount(where);
        }
        #endregion

        #region 提交按钮逻辑

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitTraChooseEvaluate(int tId)
        {

            // 影响行数
            int row = 0;

            // 更新之前table
            TraChooseEvaluateModel beforeModel = bll.GetModelByID(tId);

            // 审核流程model
            BasisAuditRelationModel model = Auxiliary.IsMatch(5, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID());

            if (model != null)
            {
                bll.SubmitTraChooseEvaluate(tId);
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


            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail", content = "无匹配审核流程,不能提交！" });
        }
        #endregion

        #region 作废按钮逻辑

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            TraChooseEvaluateModel beforeModel = bll.GetModelByID(tId);

            int delUserId = Auxiliary.UserID();

            //作废运输选择供应商表
            int row = bll.InvalidState(tId);

            //运输供应商选择评价表 
            bll.InvalidEvaluateState(tId, delUserId);

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
        /// 导出
        /// </summary>
        /// <param name="applyTime">申请时间</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="evaluateState">评价状态</param>
        /// <param name="isNotification">启用招标</param>
        /// <param name="notificationType">招标状态</param>
        /// <param name="isEvaluate">是否评价</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string applyTime, string supplierName, string evaluateState, string isNotification, string notificationType, string isEvaluate)
        {
            string where = string.Format(" TC.UseState = 5 and TC.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 申请时间
            if (!string.IsNullOrEmpty(applyTime))
            {
                where += string.Format(" And convert(varchar,TC.ApplyTime,120) like '%{0}%'", applyTime.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 评价审核状态
            if (!string.IsNullOrEmpty(evaluateState))
            {
                where += string.Format(" And TC.EvaluateState = {0}", evaluateState.Trim());
            }

            // 启用招标
            if (!string.IsNullOrEmpty(isNotification))
            {
                where += string.Format(" And TC.IsNotification = {0}", isNotification.Trim());
            }

            // 招标状态
            if (!string.IsNullOrEmpty(notificationType))
            {
                where += string.Format(" And TC.NotificationType = {0}", notificationType.Trim());
            }

            // 是否评价
            if (!string.IsNullOrEmpty(isEvaluate))
            {
                where += string.Format(" And TC.IsEvaluate = {0}", isEvaluate.Trim());
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

        #region 模板附件类型List 运输月度考核表单自定义附件明细

        /// <summary>
        /// 模板附件类型List 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <returns>Json</returns>
        public List<TraChooseEvaluateModel> ChoicesuppliesList(string tId)
        {

            string where = string.Format(" 1=1 ");

            //运输选择id
            if (!string.IsNullOrEmpty(tId))
            {
                where += string.Format(" And TC.TraChooseId = {0}", tId.Trim());
            }

            return bll.ChoicesuppliesList(where);
        }

        #endregion

        #region 模板附件类型List 运输月度考核表单自定义附件明细

        /// <summary>
        /// 模板附件类型List 运输月度考核表单自定义附件明细
        /// </summary>
        /// <returns>Json</returns>
        public ActionResult ChoiceFromAdjunctList()
        {
            //// where条件 (有效状态)
            string where = " State = 1 ";

            // 模板附件类型List
            List<TraChoiceFromAdjunctModel> FromAdjunctList = TCFbll.ChoiceFromAdjunctList(where);

            return Json(FromAdjunctList);
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

            TraChooseEvaluateModel choiceevaluateids = bll.GetModelByIDs(tId);

            // 附件
            List<TempChoiceEvaluateAdjunctModel> fileList = abll.AdjunctListById(choiceevaluateids.ChoiceEvaluateId);

            return Json(fileList);
        }

        #endregion

        #region 模板附件类型count 运输选择评价表单自定义附件

        /// <summary>
        /// 模板附件类型count 运输选择评价表单自定义附件
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public int ChoiceFromAdjunctAmount(int tId)
        {
            return TCFbll.ChoiceFromAdjunctAmount(tId, "");
        }
        #endregion

        #region 编辑 运输选择评价表单自定义附件

        /// <summary>
        /// 编辑 运输选择评价表单自定义附件
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditChoiceFromFromAdjunct(TraChoiceFromAdjunctModel tModel)
        {
            // 编辑之前Model
            TraChoiceFromAdjunctModel beforeModel = TCFbll.GetModelByID(tModel.ChoiceFromAdjunctId);

            // 编辑
            int row = TCFbll.EditChoiceFromAdjunct(tModel);

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
        #endregion

        #region 作废 运输选择评价表单自定义附件

        /// <summary>
        /// 作废 运输选择评价表单自定义附件
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidChoiceFromAdjunct(int tId)
        {
            // 作废之前Model
            TraChoiceFromAdjunctModel beforeModel = TCFbll.GetModelByID(tId);

            // 作废(更改状态)
            int row = TCFbll.InvalidState(tId);

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

        #region 模板附件类型List 运输月度考核表单自定义附件明细

        /// <summary>
        /// 模板附件类型List 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <param name="choiceFromId">运输选择评价自定义表单id</param>
        /// <returns>Json</returns>
        public ActionResult ChoiceFromComponentList(int tId, int choiceFromId)
        {
            // where条件 (有效状态)
            string where = " State = 10 AND ChoiceFromId =" + tId;

            // 模板附件类型List
            List<TraChooseEvaluateModel> ComponentList = bll.ChoiceFromComponentList(where);

            return Json(ComponentList);
        }

        #endregion

        #region 显示供应商信息 分页列表数据

        /// <summary>
        /// 显示供应商信息
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="transportNumber">运输供应商编号</param>
        /// <returns></returns>
        public ActionResult SuppInfoList(int index, int size, string transportNumber, string traChooseId)
        {
            string where = string.Format(" TC.UseState=5 and TC.TraChooseId={0}", traChooseId.Trim());

            //运输供应商编号
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And ST.TransportNumber like '%{0}%'", transportNumber.Trim());
            }

            List<TraChooseEvaluateModel> list = bll.SuppInfoList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 显示供应商信息 数据记录数

        /// <summary>
        /// 显示供应商信息 数据记录数
        /// </summary> 
        /// <param name="transportNumber">运输供应商编号</param>
        /// <returns></returns>
        public int SuppInfoCount(string transportNumber, string traChooseId)
        {
            string where = string.Format(" TC.UseState=5 and TC.TraChooseId={0}", traChooseId.Trim());

            //运输供应商编号
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And ST.TransportNumber like '%{0}%'", transportNumber.Trim());
            }

            return bll.SuppInfoCount(where);
        }
        #endregion

        #region 获取评估分数明细

        /// <summary>
        /// 获取评估分数明细
        /// </summary>
        /// <param name="id">运输选择评价id</param>
        /// <returns></returns>
        public ActionResult AssessScore(string id)
        {
            return Json(new TraChooseEvaluateBLL().GetAssessDetailScore(id));
        }
        #endregion

        #endregion
    }

    #region 评分辅助类

    /// <summary>
    /// 评分辅助类
    /// </summary>
    public class SuppModels
    {
        //显示上方的标题
        public List<Model.Tra.TraChooseEvaluateModel> EvaluateContentList { get; set; }

        //显示运输评价列表
        public List<Model.Tra.TraChooseEvaluateModel> EvaluateList { get; set; }

        //显示运输评价选择内容信息
        public List<Model.Tra.TraChooseEvaluateModel> EvaluateEditList { get; set; }

        //显示运输供应商名称
        public List<Model.Tra.TraChooseEvaluateModel> EvaluateSupplierNameList { get; set; }

        //显示运输供应商名称
        public int EvaluateSupplierNameCount { get; set; }

        //显示运输供应商名称
        public int EvaluateContentCount { get; set; }

    }
    #endregion

}
