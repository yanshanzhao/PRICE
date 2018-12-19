//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-11    1.0        FJK        新建                     
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
 * 类名：TraYearCheckComponentController
 * 功能描述：运输年度考核元件 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraYearCheckComponentController : Controller
    {
        //
        // GET: /Tra/TraYearCheckComponent/

        // 运输年度考核元件bll
        TraYearCheckComponentBLL bll = new TraYearCheckComponentBLL();

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
            TraYearCheckComponentModel model = bll.GetModelByID(tId);
             
            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            TraYearCheckComponentModel model = bll.GetModelByID(tId);
             
            return View(model);
        }
        #endregion

        #region 方法 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddYearCheckComponent(TraYearCheckComponentModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认初始 0
            tModel.State = 0;

            // 年考核元件编号 
            tModel.CheckComponentNumber = Auxiliary.CurCompanyAutoNum("TYC");
             
            // 新增(返回主键ID)
            int CheckComponentId = bll.AddYearCheckComponent(tModel);

            // 若主键>O(新增成功)
            if (CheckComponentId > 0)
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
        /// <param name="tClassify">分类</param>
        /// <param name="tCheckProject">项目</param> 
        /// <param name="tState">状态</param>
        /// <param name="tCreateTime">创建时间</param>
        /// <returns>Json</returns>
        public ActionResult YearCheckComponentList(int index, int size, string tClassify, string tCheckProject, string tState,string tCreateTime)
        {
            // 查询本公司内有效的年度绩效元件信息。
            string where = " State != 10 AND CompanyId =" + Auxiliary.CompanyID();

            // 分类
            if (!string.IsNullOrEmpty(tClassify))
            {
                where += string.Format(" And Classify like '%{0}%'", tClassify.Trim());
            }
             
            // 项目 
            if (!string.IsNullOrEmpty(tCheckProject))
            {
                where += string.Format(" And CheckComponentType = {0}", tCheckProject.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(tState))
            {
                where += string.Format(" And State = {0}", tState.Trim());
            }
             
            // 创建时间 
            if (!string.IsNullOrEmpty(tCreateTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", tCreateTime.Trim());
            }
 
            // 运输月度考核元件List
            List<TraYearCheckComponentModel> list = bll.YearCheckComponentList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="tClassify">分类</param>
        /// <param name="tCheckProject">项目</param> 
        /// <param name="tState">状态</param>
        /// <param name="tCreateTime">创建时间</param>
        /// <returns></returns>
        public int YearCheckComponentCount(string tClassify, string tCheckProject, string tState, string tCreateTime)
        {
            // 查询本公司内有效的年度绩效元件信息。
            string where = " State != 10 AND CompanyId =" + Auxiliary.CompanyID();

            // 分类
            if (!string.IsNullOrEmpty(tClassify))
            {
                where += string.Format(" And Classify like '%{0}%'", tClassify.Trim());
            }

            // 项目 
            if (!string.IsNullOrEmpty(tCheckProject))
            {
                where += string.Format(" And CheckComponentType = {0}", tCheckProject.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(tState))
            {
                where += string.Format(" And State = {0}", tState.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(tCreateTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", tCreateTime.Trim());
            }

            return bll.YearCheckComponentCount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditYearCheckComponent(TraYearCheckComponentModel tModel)
        {
            // 编辑之前Model
            TraYearCheckComponentModel beforeModel = bll.GetModelByID(tModel.CheckComponentId);
 
            // 编辑
            int row = bll.EditYearCheckComponent(tModel);

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
            TraYearCheckComponentModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 根据运输年度考核元件表主键ID查询运输月度绩效自定义元件表中状态为有效的数量 
            int result = bll.EffectiveStateCount(tId);
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
                return Json(new { flag = "fail", content = "作废失败" });
            }

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
        /// <param name="tClassify">分类</param>
        /// <param name="tCheckProject">项目</param> 
        /// <param name="tState">状态</param>
        /// <param name="tCreateTime">创建时间</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string tClassify, string tCheckProject, string tState, string tCreateTime)
        {
            // 查询本公司内有效的年度绩效元件信息。
            string where = "State != 10 AND CompanyId =" + Auxiliary.CompanyID();

            // 分类
            if (!string.IsNullOrEmpty(tClassify))
            {
                where += string.Format(" And Classify like '%{0}%'", tClassify.Trim());
            }

            // 项目 
            if (!string.IsNullOrEmpty(tCheckProject))
            {
                where += string.Format(" And CheckComponentType = {0}", tCheckProject.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(tState))
            {
                where += string.Format(" And State = {0}", tState.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(tCreateTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", tCreateTime.Trim());
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

        #region 查询运输年度考核元件（新增）

        /// <summary>
        /// 数据集
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult AddCompnentList()
        { 
            // 运输年度考核List
            List<TraYearCheckComponentModel> list = bll.AddCompnentList(Auxiliary.CompanyID());
             
            return Json(list);
        }
        #endregion

        #region 查询运输年度考核元件（编辑）

        /// <summary>
        /// 数据集
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult EditCompnentList(int tId)
        {
            // 运输年度考核List
            List<TraYearCheckContentModel> list = bll.EditCompnentList(tId);

            return Json(list);
        }
        #endregion

        #endregion
    }
}
