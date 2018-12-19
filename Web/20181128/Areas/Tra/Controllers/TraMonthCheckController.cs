//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-11    1.0        FJK        新建-绩效考核评价                    
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
 * 类名：TraMonthCheckController
 * 功能描述：运输月度考核表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraMonthCheckController : Controller
    {
        //
        // GET: /Tra/TraMonthCheck/

        // 运输月度考核BLL
        TraMonthCheckBLL bll = new TraMonthCheckBLL();

        // 运输月度考核附件BLL
        TraMonthCheckAdjunctBLL TMCAbll = new TraMonthCheckAdjunctBLL();

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
            TraMonthCheckModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId,int checkId, int checkFromId)
        {
            // 获取数据
            TraMonthCheckModel model = bll.GetModelByID(tId);

            List<TempMonthCheckAdjunctModel> fileList = TMCAbll.AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);
            List<string> oldlist = fileList.Select(p => p.FileName + p.ext).ToList<string>();

            ViewBag.CheckId = checkId;
            ViewBag.CheckFromId = checkFromId;
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
        #endregion

        #region 方法

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddMonthCheck(TraMonthCheckModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认创建 0
            tModel.State = 0;

            // 月度考核编号
            tModel.CheckNumber = Auxiliary.CurCompanyAutoNum("TMN");

            // 新增(返回主键ID)
            int CheckId = bll.AddMonthCheck(tModel);

            // 若主键>O(新增成功)
            if (CheckId > 0)
            { 
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
        public ActionResult TransportList(int index, int size, string supplierName, string checkType)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(" TMCFD.State={0} And TMCFD.DepartmentId={1} ", "1", Auxiliary.DepartmentId()));

            if (checkType != "")
            {
                if (checkType == "0")
                {
                    // 考核类型为0(调拨)
                    sb.Append(string.Format(" And CheckFromType={0}", Convert.ToInt32(checkType)));
                }
                else if (checkType == "1")
                {
                    // 考核类型为3(配送干线)
                    sb.Append(string.Format(" And CheckFromType={0}", Convert.ToInt32(checkType)));
                }
                else if (checkType == "2")
                {
                    // 考核类型为3(配送终端)
                    sb.Append(string.Format(" And CheckFromType={0}", Convert.ToInt32(checkType)));
                }
                else if (checkType == "3")
                {
                    // 考核类型为3(配送综合-干线和终端)
                    sb.Append(string.Format(" And CheckFromType !=0"));
                }
            }

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                sb.Append(" And s.SupplierName like '%" + supplierName.Trim() + "%' ");
            }

            // 月度考核自定义机构List
            List<TraMonthCheckFromDeparModel> list = bll.TransportList(index, size, sb.ToString());

            return Json(list);
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
        public int TransportAmount(string supplierName, string checkType)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(" TMCFD.State={0} and TMCFD.DepartmentId={1} ", "1", Auxiliary.DepartmentId()));

            if (checkType != "")
            {
                if (checkType == "0")
                {
                    // 考核类型为0(调拨)
                    sb.Append(string.Format(" And CheckFromType={0}", checkType));
                }
                else if (checkType == "1")
                {
                    // 考核类型为3(配送干线)
                    sb.Append(string.Format(" And CheckFromType={0}", checkType));
                }
                else if (checkType == "2")
                {
                    // 考核类型为3(配送终端)
                    sb.Append(string.Format(" And CheckFromType={0}", checkType));
                }
                else if (checkType == "3")
                {
                    // 考核类型为3(配送综合-干线和终端)
                    sb.Append(string.Format(" And CheckFromType !=0"));
                }
            }

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                sb.Append(" And SupplierName like '%" + supplierName.Trim() + "%' ");
            }

            return bll.TransportAmount(sb.ToString());
        }

        #endregion

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
        public ActionResult MonthCheckList(int index, int size, string supplierName, string year, string month, string state, string createTime)
        {
            // 只能查询本机构创建的考核的有效信息。
            string where = " TMC.CreateDepartmentId=" + Auxiliary.DepartmentId() + " AND TMC.State != 10";

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 考核年  
            if (!string.IsNullOrEmpty(year))
            {
                where += string.Format(" And CheckYear like '%{0}%'", year.Trim());
            }

            // 考核月  
            if (!string.IsNullOrEmpty(month))
            {
                where += string.Format(" And CheckMonth = {0}", month.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TMC.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            // 运输月度考核List
            List<TraMonthCheckModel> list = bll.TraMonthCheckList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="supplierName">运输供应商名称</param>
        /// <param name="year">考核年</param>
        /// <param name="month">考核月</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int MonthCheckAmount(string supplierName, string year, string month, string state, string createTime)
        {
            // 只能查询本机构创建的考核的有效信息。
            string where = " TMC.CreateDepartmentId=" + Auxiliary.DepartmentId() + " AND TMC.State != 10";

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 考核年  
            if (!string.IsNullOrEmpty(year))
            {
                where += string.Format(" And CheckYear like '%{0}%'", year.Trim());
            }

            // 考核月  
            if (!string.IsNullOrEmpty(month))
            {
                where += string.Format(" And CheckMonth = {0}", month.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TMC.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            return bll.TraMonthCheckAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditMonthCheck(TraMonthCheckModel tModel)
        {
            // 编辑之前Model
            TraMonthCheckModel beforeModel = bll.GetModelByID(tModel.CheckId);

            // 编辑
            int row = bll.EditMonthCheck(tModel);

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
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitState(int tId)
        {
            // 提交之前Model
            TraMonthCheckModel beforeModel = bll.GetModelByID(tId);

            // 对于同运输供应商同年同月同模板ID不能重复提交 
            int result = bll.TraMonthCheckAmount(" TMC.State = 5 AND TMC.TransportId = " + beforeModel.TransportId + " AND TMC.CheckYear=" + beforeModel.CheckYear + " AND TMC.CheckMonth=" + beforeModel.CheckMonth + " AND TMC.CheckFromId=" + beforeModel.CheckFromId);

            // 若影响行数>O(存在同运输供应商同年同月数据)
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Exist, new { Detail = "提交", Id = tId, State = "初始" });
                return Json(new { flag = "exist", content = "同运输供应商同年同月不能重复提交！" });
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
            TraMonthCheckModel beforeModel = bll.GetModelByID(tId);

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
        /// <param name="supplierName">运输供应商名称</param>
        /// <param name="year">考核年</param>
        /// <param name="month">考核月</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string year, string month, string state, string createTime)
        {
            // 只能查询本机构创建的考核的有效信息。
            string where = " TMC.CreateDepartmentId=" + Auxiliary.DepartmentId() + " AND TMC.State != 10";

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 考核年  
            if (!string.IsNullOrEmpty(year))
            {
                where += string.Format(" And CheckYear like '%{0}%'", year.Trim());
            }

            // 考核月  
            if (!string.IsNullOrEmpty(month))
            {
                where += string.Format(" And CheckMonth = {0}", month.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TMC.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", createTime.Trim());
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
        #endregion
    }
}
