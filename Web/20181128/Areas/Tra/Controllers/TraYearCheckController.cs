//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-11    1.0        FJK        新建 - 年度评估                    
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
 * 类名：TraYearCheckController
 * 功能描述：运输年度考核 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraYearCheckController : Controller
    {
        //
        // GET: /Tra/TraYearCheck/

        // 运输年度考核BLL
        TraYearCheckBLL bll = new TraYearCheckBLL();

        // 运输年度考核内容BLL
        TraYearCheckContentBLL TYCCbll = new TraYearCheckContentBLL();

        // 运输年度考核附件BLL
        TraYearCheckAdjunctBLL TYCAbll = new TraYearCheckAdjunctBLL();

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
            // 附件
            List<TempYearCheckAdjunctModel> fileList = TYCAbll.AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);
            List<string> oldlist = fileList.Select(p => p.FileName + p.ext).ToList<string>(); 
             
            // 获取数据
            TraYearCheckModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
         [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 附件
            List<TempYearCheckAdjunctModel> fileList = TYCAbll.AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);
            List<string> oldlist = fileList.Select(p => p.FileName + p.ext).ToList<string>();

            // 获取数据
            TraYearCheckModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// TransportDetail
        /// </summary>  
        public ActionResult TransportDetail(string url)
        {
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
        public ActionResult AddYearCheck(TraYearCheckModel tModel)
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
            tModel.CheckNumber = Auxiliary.CurCompanyAutoNum("TYN");

            // 新增(返回主键ID)
            int CheckYearId = bll.AddYearCheck(tModel);

            // 若主键>O(新增成功)
            if (CheckYearId > 0)
            {
                // 考核内容
                if (!string.IsNullOrEmpty(tModel.ScoreList))
                {
                    List<TempYearCheckContentModel> scoreList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempYearCheckContentModel>>(tModel.ScoreList);

                      TYCCbll.AddYearCheckContentList(scoreList, CheckYearId);
                }

                // 附件
                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    List<TempYearCheckAdjunctModel> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempYearCheckAdjunctModel>>(tModel.AdjunctList);
                    TYCAbll.AddYearCheckAdjunctList(fileList, CheckYearId);
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
        public ActionResult YearCheckList(int index, int size, string supplierName, string transportNumber, string year, string state, string createTime)
        { 
            // 非作废,本机构 
            string where = " TYC.CreateDepartmentId=" + Auxiliary.DepartmentId() + " AND TYC.State != 10";

            // 本机构 
            // string where = " TYC.CreateDepartmentId=" + Auxiliary.DepartmentId();

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运输供应商编号 
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And TransportNumber like '%{0}%'", transportNumber.Trim());
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
            List<TraYearCheckModel> list = bll.YearCheckList(index, size, where);

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
        public int YearCheckAmount(string supplierName, string transportNumber, string year, string state, string createTime)
        {
            // 非作废,本机构 
            string where = " TYC.CreateDepartmentId=" + Auxiliary.DepartmentId() + " AND TYC.State != 10";

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运输供应商编号 
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And TransportNumber like '%{0}%'", transportNumber.Trim());
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

            return bll.YearCheckAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditYearCheck(TraYearCheckModel tModel)
        {
            // 编辑之前Model
            TraYearCheckModel beforeModel = bll.GetModelByID(tModel.CheckYearId);

            // 编辑
            int row = bll.EditYearCheck(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 考核内容
                if (!string.IsNullOrEmpty(tModel.ScoreList))
                {
                    List<TempYearCheckContentModel> scoreList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempYearCheckContentModel>>(tModel.ScoreList);

                    TYCCbll.AddYearCheckContentList(scoreList, tModel.CheckYearId);
                }

                // 附件
                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    List<TempYearCheckAdjunctModel> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempYearCheckAdjunctModel>>(tModel.AdjunctList);
                    TYCAbll.AddYearCheckAdjunctList(fileList, tModel.CheckYearId);
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
        public ActionResult SubmitState(int tId,int transportId,int checkYear)
        {
            // 判断本年是否已存在同运输供应商ID,状态为提交状态的数据. 
            int result = bll.YearCheckAmount(" TYC.State = 5 AND TYC.TransportId = " + transportId + " AND CheckYear=" + checkYear);

            // 若影响行数>O(存在同运输供应商数据)
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Exist, new { Detail = "提交", Id = tId, State = "初始" });
                return Json(new { flag = "exist", content = "同运输供应商同年不能重复提交！" });
            }

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
            TraYearCheckModel beforeModel = bll.GetModelByID(tId);

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
        /// <param name="checkComponentNumber">元件编号</param>
        /// <param name="checkComponentName">元件名称</param>
        /// <param name="checkComponentType">考核类型</param>
        /// <param name="project">项目</param>
        /// <param name="isFormula">公式计算</param>
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
         [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string transportNumber, string year, string state, string createTime)
        {
            // 非作废,本机构 
            string where = " TYC.CreateDepartmentId=" + Auxiliary.DepartmentId() + " AND TYC.State != 10";

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运输供应商编号 
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And TransportNumber like '%{0}%'", transportNumber.Trim());
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

        #region 以SupplierTransport为主表查询运输供应商

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
            string where = " TransportState = 'F4' AND DepartmentId ="+Auxiliary.DepartmentId();

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运输供应商List
            List<TraYearCheckModel> list = bll.TransportList(index, size, where);

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

            return bll.TransportAmount(where);
        }

        #endregion

        #endregion
    }
}
