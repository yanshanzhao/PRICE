//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-16    1.0        FJK        新建 - 年度绩效                    
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
using System.Text;
#endregion
/*********************************
 * 类名：TraYearCheckResultController
 * 功能描述：年度绩效结果表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraYearCheckResultController : Controller
    {
        //
        // GET: /Tra/TraYearCheckResult/
        // 运输年度考核BLL
        TraYearCheckResultBLL bll = new TraYearCheckResultBLL();
        TraYearCheckResultDetailBLL Detailbll = new TraYearCheckResultDetailBLL();

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
            TraYearCheckResultModel model = bll.GetModelByID(tId);

            SupplierTransportModel stModel = new SupplierTransportBLL().GetModelByID(model.TransportId.ToString());
            SupplierModel suppModel = new SupplierBLL().GetModelByID(stModel.SupplierId.ToString());

            // 运输供应商名称
            ViewBag.SupplierName = suppModel.SupplierName;

            // 运输供应商编号
            ViewBag.TransportNumber = stModel.TransportNumber;

            // 附件
            List<temfile> fileList = new TraYearCheckResultAdjunctBLL().AdjunctListById(tId);
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
            TraYearCheckResultModel model = bll.GetModelByID(tId);

            SupplierTransportModel stModel = new SupplierTransportBLL().GetModelByID(model.TransportId.ToString());
            SupplierModel suppModel = new SupplierBLL().GetModelByID(stModel.SupplierId.ToString());

            // 运输供应商名称
            ViewBag.SupplierName = suppModel.SupplierName;

            // 运输供应商编号
            ViewBag.TransportNumber = stModel.TransportNumber;

            // 附件
            List<temfile> fileList = new TraYearCheckResultAdjunctBLL().AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);
            List<string> oldlist = fileList.Select(p => p.filename + p.ext).ToList<string>();

            return View(model);
        }

        /// <summary>
        /// TransportDetail
        /// </summary>  
        public ActionResult TransportDetail(string url, string type)
        {
            ViewBag.url = url;
            ViewBag.type = type;
            return View();
        }

        /// <summary>
        /// 月绩效信息列表
        /// </summary>  
        public ActionResult MonthCheckDetail(string tUrl, string tIds, int tTransportId, string tYear, string tType)
        {
            ViewBag.Url = tUrl;
            ViewBag.Ids = tIds;
            ViewBag.Type = tType;
            ViewBag.Year = tYear;
            ViewBag.TransportId = tTransportId;
            return View();
        }

        /// <summary>
        /// 巡查信息列表
        /// </summary>  
        public ActionResult PatrolRecordDetail(string tUrl, string tIds, int tTransportId, string tType)
        {
            ViewBag.Url = tUrl;
            ViewBag.Ids = tIds;
            ViewBag.Type = tType;
            ViewBag.TransportId = tTransportId;
            return View();
        }

        /// <summary>
        /// 年评估信息列表
        /// </summary>  
        public ActionResult YearCheckDetail(string tUrl, string tIds, int tTransportId, string tYear, string tType)
        {
            ViewBag.Url = tUrl;
            ViewBag.Ids = tIds;
            ViewBag.Type = tType;
            ViewBag.Year = tYear;
            ViewBag.TransportId = tTransportId;
            return View();
        }

        #endregion

        #region 方法 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddTraYearCheckResult(TraYearCheckResultModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认创建 0
            tModel.State = 0;

            // 使用状态默认未使用 0
            tModel.CheckState = 0;

            // 年度考核编号
            tModel.ResultNumber = Auxiliary.CurCompanyAutoNum("TYR");

            // 新增(返回主键ID)
            int ResultId = bll.AddTraYearCheckResult(tModel);

            // 若主键>O(新增成功)
            if (ResultId > 0)
            {
                // 月度绩效信息 年度绩效结果明细表
                if (!string.IsNullOrEmpty(tModel.CheckIdList))
                {
                    List<string> checkIdList = new List<string>(tModel.CheckIdList.Split(','));
                    new TraYearCheckResultDetailBLL().AddResultDetailList(checkIdList, ResultId,0);
                }

                // 巡查信息 年度绩效结果明细表
                if (!string.IsNullOrEmpty(tModel.RecordIdList))
                {
                    List<string> recordIdList = new List<string>(tModel.RecordIdList.Split(','));
                    new TraYearCheckResultDetailBLL().AddResultDetailList(recordIdList,ResultId,1);
                }

                // 年评估信息 年度绩效结果明细表
                if (!string.IsNullOrEmpty(tModel.CheckYearIdList))
                {
                    List<string> checkYearIdList = new List<string>(tModel.CheckYearIdList.Split(','));
                    new TraYearCheckResultDetailBLL().AddResultDetailList(checkYearIdList, ResultId,2);
                }

                // 附件
                if (!string.IsNullOrEmpty(tModel.FileList))
                {
                    List<temfile> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.FileList);
                    new TraYearCheckResultAdjunctBLL().AddFiles(fileList, ResultId);
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
        /// <param name="supplierName">运输供应商名称</param>
        /// <param name="year">考核年</param>
        /// <param name="month">考核月</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns>Json</returns>
        public ActionResult TraYearCheckResultList(int index, int size, string supplierName, string year, string state, string createTime)
        {
            // 非作废,本机构 
            string where = " TYC.CreateDepartmentId=" + Auxiliary.DepartmentId() + " AND TYC.State != 10";

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }
             
            // 考核年  
            if (!string.IsNullOrEmpty(year))
            {
                where += string.Format(" And CheckYear = {0}", year.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TYC.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TYC.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            // 运输年度考核List
            List<TraYearCheckResultModel> list = bll.TraYearCheckResultList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="checkComponentNumber">元件编号</param>
        /// <param name="checkComponentName">元件名称</param>
        /// <param name="checkComponentType">考核类型</param>
        /// <param name="project">项目</param>
        /// <param name="isFormula">公式计算</param>
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int TraYearCheckResultCount(string supplierName, string year, string state, string createTime)
        {
            // 非作废,本机构 
            string where = " TYC.CreateDepartmentId=" + Auxiliary.DepartmentId() + " AND TYC.State != 10";

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }
             
            // 考核年  
            if (!string.IsNullOrEmpty(year))
            {
                where += string.Format(" And CheckYear = {0}", year.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TYC.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TYC.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            return bll.TraYearCheckResultCount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraYearCheckResult(TraYearCheckResultModel tModel)
        {
            // 编辑之前Model
            TraYearCheckResultModel beforeModel = bll.GetModelByID(tModel.ResultId);

            // 编辑
            int row = bll.EditTraYearCheckResult(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 附件
                if (!string.IsNullOrEmpty(tModel.FileList))
                {
                    List<temfile> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.FileList);
                    new TraYearCheckResultAdjunctBLL().AddFiles(fileList, beforeModel.ResultId);
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
        public ActionResult SubmitState(int tId, int transportId, int checkYear)
        { 
            // 提交(更改状态)
            int row = bll.SubmitState(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "提交", Id = tId, State = "提交" });
                return Json(new { flag = "success", content = "提交成功" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "提交", Id = tId, State = "初始" });
            return Json(new { flag = "fail", content = "提交失败" });
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
            TraYearCheckResultModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 根据主表ID修改明细表状态为无效 并将对应表的使用状态修改为未使用
                new TraYearCheckResultDetailBLL().InvalidResultDetailById(tId);

                // 根据主表ID修改附件表表状态为无效
                new TraYearCheckResultAdjunctBLL().InvalidAdjunct(tId);

                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败" });
        }

        #region 计算公式 年度绩效公式表

        /// <summary>
        /// 数据集
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult FormulaList()
        {
            // 查询状态为提交且本公司的有效期内的公式
            string where = " CompanyId=" + Auxiliary.CompanyID();

            // List
            List<TraYearCheckFormulModel> list = bll.FormulaList(where);

            return Json(list);
        }

        /// <summary>
        /// 获取公式百分比
        /// </summary> 
        /// <returns></returns>
        public int GetPercent(int tFormulaId, int tDetailType)
        { 
            return bll.GetPercent(tFormulaId, tDetailType);
        }

        #endregion

        #region 运输供应商List 运输供应商

        /// <summary>
        /// 数据集 运输供应商
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">运输供应商名称</param> 
        /// <returns>Json</returns>
        public ActionResult TransportList(int index, int size, string supplierName)
        {
            // 同机构 运作状态。
            string where = " TransportState = 'F4' AND DepartmentId =" + Auxiliary.DepartmentId();

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运输供应商List
            List<TraYearCheckModel> list = new TraYearCheckBLL().TransportList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数 运输供应商
        /// </summary> 
        /// <param name="supplierName">供应商名称</param> 
        /// <returns></returns>
        public int TransportAmount(string supplierName)
        {
            // 同机构 运作状态。
            string where = " TransportState = 'F4' AND DepartmentId =" + Auxiliary.DepartmentId();

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            return new TraYearCheckBLL().TransportAmount(where);
        }

        #endregion

        #region 月度绩效List 运输月度考核
         
        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="tTransportId">运输供应商ID</param>
        /// <param name="tIds">需排除的月绩效数据ID</param>
        /// <param name="tType">新增/编辑</param> 
        /// <returns>Json</returns>
        public ActionResult MonthCheckList(int index, int size, string tTransportId, string tYear, string tIds, string tType)
        {
            // 条件
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询同运输供应商的所有使用状态为未使用且提交的的同年的月绩效信息 
                where = string.Format(" CheckState = 0 AND State =5 AND TransportId = {0} AND CheckYear = {1}", tTransportId, tYear);

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And CheckId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And CheckId NOT IN (SELECT OtherId FROM TraYearCheckResultDetail WHERE ResultId = {0} AND DetailType = 0 AND DetailState = 1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where = string.Format(" CheckId IN ({0})", tIds.Trim());
            }

            // 月度绩效List
            List<TraMonthCheckModel> list = bll.MonthCheckList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="tTransportId">运输供应商ID</param>
        /// <param name="tIds">需排除的月绩效数据ID</param>
        /// <param name="tType">新增/编辑</param> 
        /// <returns></returns>
        public int MonthCheckCount(string tTransportId, string tYear, string tIds, string tType)
        {
            // 条件
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询同运输供应商的所有使用状态为未使用且提交的的同年的月绩效信息 
                where = string.Format(" CheckState = 0 AND State =5 AND TransportId = {0} AND CheckYear = {1}", tTransportId, tYear);

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And CheckId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And CheckId NOT IN (SELECT OtherId FROM TraYearCheckResultDetail WHERE ResultId = {0} AND DetailType = 0 AND DetailState = 1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where = string.Format(" CheckId IN ({0})", tIds.Trim());
            }

            return bll.MonthCheckCount(where);
        }
         
        #endregion

        #region 巡查记录List 巡查记录表
          
        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="tTransportId">运输供应商ID</param>
        /// <param name="tIds">需排除的月绩效数据ID</param>
        /// <param name="tType">新增/编辑</param> 
        /// <returns>Json</returns>
        public ActionResult PatrolRecordList(int index, int size, string tTransportId, string tIds, string tType)
        {
            // 条件
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询同运输供应商的所有巡查状态为提交的的巡查信息
                where = string.Format(" RecordState =1 AND TransportId = {0}", tTransportId);

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And RecordId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And RecordId NOT IN (SELECT OtherId FROM TraYearCheckResultDetail WHERE ResultId = {0} AND DetailType = 1 AND DetailState = 1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where = string.Format(" RecordId IN ({0})", tIds.Trim());
            }

            // 月度绩效List
            List<TraPatrolRecordModel> list = bll.PatrolRecordList(index, size, where);
              
            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat)); 
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="tTransportId">运输供应商ID</param>
        /// <param name="tIds">需排除的月绩效数据ID</param>
        /// <param name="tType">新增/编辑</param> 
        /// <returns></returns>
        public int PatrolRecordCount(string tTransportId, string tIds, string tType)
        {
            // 条件
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询同运输供应商的所有巡查状态为提交的的巡查信息
                where = string.Format(" RecordState =1 AND TransportId = {0}", tTransportId);

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And RecordId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And RecordId NOT IN (SELECT OtherId FROM TraYearCheckResultDetail WHERE ResultId = {0} AND DetailType = 1 AND DetailState = 1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where = string.Format(" RecordId IN ({0})", tIds.Trim());
            }

            return bll.PatrolRecordCount(where);
        }
        #endregion 

        #region 年度评估List 年度评估表
  
        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="tTransportId">运输供应商ID</param>
        /// <param name="tIds">需排除的月绩效数据ID</param>
        /// <param name="tType">新增/编辑</param> 
        /// <returns>Json</returns>
        public ActionResult YearCheckList(int index, int size, string tTransportId, string tYear, string tIds, string tType)
        {
            // 条件
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询同运输供应商的所有使用状态为未使用且提交的的同年的年评估信息
                where = string.Format(" CheckState = 0 AND State =5 AND TransportId = {0} AND CheckYear = {1}", tTransportId, tYear);

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And CheckYearId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And CheckYearId NOT IN (SELECT OtherId FROM TraYearCheckResultDetail WHERE ResultId = {0} AND DetailType = 2 AND DetailState = 1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where = string.Format(" CheckYearId IN ({0})", tIds.Trim());
            }

            // 年度评估List
            List<TraYearCheckModel> list = bll.YearCheckList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="tTransportId">运输供应商ID</param>
        /// <param name="tIds">需排除的月绩效数据ID</param>
        /// <param name="tType">新增/编辑</param> 
        /// <returns></returns>
        public int YearCheckCount(string tTransportId, string tYear, string tIds, string tType)
        {
            // 条件
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询同运输供应商的所有使用状态为未使用且提交的的同年的年评估信息
                where = string.Format(" CheckState = 0 AND State =5 AND TransportId = {0} AND CheckYear = {1}", tTransportId, tYear);

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And CheckYearId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And CheckYearId NOT IN (SELECT OtherId FROM TraYearCheckResultDetail WHERE ResultId = {0} AND DetailType = 2 AND DetailState = 1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where = string.Format(" CheckYearId IN ({0})", tIds.Trim());
            }

            return bll.YearCheckCount(where);
        }

        #endregion

        #region 结果明细表

        /// <summary>
        /// 批量新增 结果明细表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddResultDetailList(string tIds, int tResultId,int tDetailType)
        {
            // 反序列化
            List<string> DetailList = new List<string>(tIds.Split(','));

            // 新增(返回主键ID)
            int row = new TraYearCheckResultDetailBLL().AddResultDetailList(DetailList, tResultId, tDetailType);
             
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
        /// <param name="tTransportId">运输供应商ID</param>
        /// <param name="tIds">需排除的月绩效数据ID</param>
        /// <param name="tType">新增/编辑</param> 
        /// <returns>Json</returns>
        public ActionResult ResultDetailList(int index, int size, string tResultId, string tDetailType)
         {
            // 根据主表ResultId和类型DetailType查询明细表中所属的状态为有效的数据
            string where = string.Format(" AND ResultId = {0} AND DetailType = {1}", tResultId, tDetailType);

            // 结果明细List
            List<TraYearCheckResultDetailModel> list = new List<TraYearCheckResultDetailModel>();

            if (tDetailType == "0")
            {
                list = new TraYearCheckResultDetailBLL().ResultDetailOfMonthCheckList(index, size, where);
            }
            else if (tDetailType == "1")
            {
                list = new TraYearCheckResultDetailBLL().ResultDetailOfPatrolRecordList(index, size, where);
            }
            else if (tDetailType == "2")
            {
                list = new TraYearCheckResultDetailBLL().ResultDetailOfYearCheckList(index, size, where);
            }

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="tTransportId">运输供应商ID</param>
        /// <param name="tIds">需排除的月绩效数据ID</param>
        /// <param name="tType">新增/编辑</param> 
        /// <returns></returns>
        public int ResultDetailCount(string tResultId, string tDetailType)
        {
            int count = 0;

            // 根据主表ResultId和类型DetailType查询明细表中所属的状态为有效的数据
            string where = string.Format(" AND ResultId = {0} AND DetailType = {1}", tResultId, tDetailType);
             
            if (tDetailType == "0")
            {
                count = new TraYearCheckResultDetailBLL().ResultDetailOfMonthCheckCount(where);
            }
            else if (tDetailType == "1")
            {
                count = new TraYearCheckResultDetailBLL().ResultDetailOfPatrolRecordCount(where);
            }
            else if (tDetailType == "2")
            {
                count = new TraYearCheckResultDetailBLL().ResultDetailOfYearCheckCount(where);
            }

            return count;
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns> 
        public ActionResult InvalidResultDetail(int tResultDetailId, int tOtherId, int tDetailType)
        {
            // 作废(更改状态)
            int row = new TraYearCheckResultDetailBLL().InvalidResultDetail(tResultDetailId, tOtherId, tDetailType);
             
            // 若影响行数>O(修改成功)
            if (row > 0)
            { 
                return Json(new { flag = "success", content = "作废成功！" });
            }
             
            return Json(new { flag = "fail", content = "作废失败！" });
        }

        #endregion 

        #endregion
    }
}
