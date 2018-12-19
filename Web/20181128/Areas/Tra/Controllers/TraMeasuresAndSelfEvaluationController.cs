//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-22    1.0        FJK        新建 - 异常整改措施及自评     
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
 * 类名：TraMeasuresAndSelfEvaluation
 * 功能描述：异常整改记录表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraMeasuresAndSelfEvaluationController : Controller
    {
        //
        // GET: /Tra/TraMeasuresAndSelfEvaluation/

        // 异常整改记录BLL
        TraMeasuresAndSelfEvaluationBLL bll = new TraMeasuresAndSelfEvaluationBLL();

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
        [Operate(Name = OperateEnum.Reform)]
        public ActionResult Reform(int tId)
        {
            // 获取数据
            TraAbnormityRecordModel model = bll.GetModelByID(tId);

            // 整改跟进人 
            if (model.UserRectification == "")
            {
                ViewBag.UserRectification = "";
            }
            else
            { 
                ViewBag.UserRectification = model.UserRectification;
            }

            // 计划完成日期
            if (model.PlanEndTime == "")
            {
                ViewBag.PlanEndTime = "";
            }
            else
            {
                ViewBag.PlanEndTime = model.PlanEndTime;
            }

            // 问题原因
            if (model.RectificationReason == "")
            {
                ViewBag.RectificationReason = "";
            }
            else
            {
                ViewBag.RectificationReason = model.RectificationReason;
            }

            // 整改措施 
            if (model.RectificationMethod == "")
            {
                ViewBag.RectificationMethod = "";
            }
            else
            {
                ViewBag.RectificationMethod = model.RectificationMethod;
            }

            // 整改实际完成时间
            if (model.ActualFinishTime == null)
            {
                ViewBag.ActualFinishTime = "";
            }
            else
            {
                ViewBag.ActualFinishTime = model.ActualFinishTime;
            }

            // 整改效果自评
            if (model.EffectEvaluation == "")
            {
                ViewBag.EffectEvaluation = "";
            }
            else
            {
                ViewBag.EffectEvaluation = model.EffectEvaluation;
            }

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

            // 整改跟进人 
            if (model.UserRectification == "")
            {
                ViewBag.UserRectification = "";
            }
            else
            {
                ViewBag.UserRectification = model.UserRectification;
            }

            // 计划完成日期
            if (model.PlanEndTime == null)
            {
                ViewBag.PlanEndTime = "";
            }
            else
            {
                ViewBag.PlanEndTime = model.PlanEndTime;
            }

            // 问题原因
            if (model.RectificationReason == "")
            {
                ViewBag.RectificationReason = "";
            }
            else
            {
                ViewBag.RectificationReason = model.RectificationReason;
            }

            // 整改措施 
            if (model.RectificationMethod == "")
            {
                ViewBag.RectificationMethod = "";
            }
            else
            {
                ViewBag.RectificationMethod = model.RectificationMethod;
            }

            // 整改实际完成时间
            if (model.ActualFinishTime == null)
            {
                ViewBag.ActualFinishTime = "";
            }
            else
            {
                ViewBag.ActualFinishTime = model.ActualFinishTime;
            }

            // 整改效果自评
            if (model.EffectEvaluation == "")
            {
                ViewBag.EffectEvaluation = "";
            }
            else
            {
                ViewBag.EffectEvaluation = model.EffectEvaluation;
            }

            // 附件
            List<temfile> fileList = tirbll.AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);

            return View(model);
        }

        #endregion

        #region 方法
         
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddAbnormityRecord(TraAbnormityRecordModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建账号ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认初始状态0
            tModel.AbnormityState = 0;

            // 使用状态默认未使用0
            tModel.UseState = 0;

            // 新增(返回主键ID)
            int incentiveId = bll.AddAbnormityRecord(tModel);

            // 若主键>O(新增成功)
            if (incentiveId > 0)
            {
                // 附件
                if (!string.IsNullOrEmpty(tModel.FileList))
                {
                    List<temfile> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.FileList);
                    tirbll.AddFiles(fileList, incentiveId);
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
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierType">供应商类型</param>
        /// <param name="abnormityState">异常整改记录状态</param>
        /// <returns>Json</returns>
        public ActionResult AbnormityRecordList(int index, int size, string supplierName, string supplierType, string abnormityState)
        {
            // 查询本机构录入的所有的有效的异常整改信息 
            string where = " AbnormityState NOT IN (10,15) AND DepartmentId =" + Auxiliary.DepartmentId();

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

            // 异常整改记录状态
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
        /// <param name="supplierNumber">供应商编号</param>
        /// <param name="supplierType">供应商类型</param>
        /// <returns></returns>
        public int AbnormityRecordCount(string supplierName, string supplierType, string abnormityState)
        {
            // 查询本机构录入的所有的有效的异常整改信息 
            string where = " AbnormityState NOT IN (10,15) AND DepartmentId =" + Auxiliary.DepartmentId();

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

            // 异常整改记录状态
            if (!string.IsNullOrEmpty(abnormityState))
            {
                where += string.Format(" And AbnormityState = {0}", abnormityState.Trim());
            }

            return bll.AbnormityRecordCount(where);
        }

        /// <summary>
        /// 整改
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult ReformAbnormityRecord(TraAbnormityRecordModel tModel)
        {
            // 编辑之前Model
            TraAbnormityRecordModel beforeModel = bll.GetModelByID(tModel.AbnormityId);

            // 编辑
            int row = bll.EditAbnormityRecord(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 修改实际执行整改状态为已整改
                 bll.ChangePlanEndStat(tModel.AbnormityId,1);

                // 附件
                if (!string.IsNullOrEmpty(tModel.FileList))
                {
                    List<temfile> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.FileList);
                    tirbll.AddFiles(fileList, tModel.AbnormityId);
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
            // 提交(更改状态)
            int row = bll.SubmitState(tId);

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
            string where = " AbnormityState NOT IN (10,15) AND DepartmentId =" + Auxiliary.DepartmentId();

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

            // 异常整改记录状态
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

        #endregion
    }
}
