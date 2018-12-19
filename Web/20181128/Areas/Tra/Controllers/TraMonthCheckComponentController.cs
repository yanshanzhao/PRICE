//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-05    1.0        FJK        新建-绩效考核元件                  
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
 * 类名：TraMonthCheckComponentController
 * 功能描述：运输月度考核元件表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraMonthCheckComponentController : Controller
    {
        //
        // GET: /Tra/TraMonthCheckComponent/

        // 运输月度考核元件BLL
        TraMonthCheckComponentBLL bll = new TraMonthCheckComponentBLL();

        // 月考核取值BLL
        TraMonthCheckValueBLL TMCVdal = new TraMonthCheckValueBLL();

        // 月考核取值条件BLL
        TraMonthCheckWhereBLL TMCWdal = new TraMonthCheckWhereBLL();

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
            TraMonthCheckComponentModel model = bll.GetModelByID(tId);
             
            // 分子取值表列
            TraMonthCheckValueModel MoleculeModel = TMCVdal.GetModelByID(model.MoleculeSurface); 
            ViewBag.Molecule = MoleculeModel.CheckValue;

            // 分母取值表列
            TraMonthCheckValueModel DenominatorModel = TMCVdal.GetModelByID(model.DenominatorColumn); 
            ViewBag.Denominator = DenominatorModel.CheckValue;

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            TraMonthCheckComponentModel model = bll.GetModelByID(tId);

            // 分子取值表列
            TraMonthCheckValueModel MoleculeModel = TMCVdal.GetModelByID(model.MoleculeSurface);
            ViewBag.Molecule = MoleculeModel.CheckValue;

            // 分母取值表列
            TraMonthCheckValueModel DenominatorModel = TMCVdal.GetModelByID(model.DenominatorColumn);
            ViewBag.Denominator = DenominatorModel.CheckValue;

            return View(model);
        }
        #endregion

        #region 方法

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddMonthCheckComponent(TraMonthCheckComponentModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认创建 1
            tModel.State = 1;

            // 月考核元件编号
            tModel.CheckComponentNumber = Auxiliary.CurCompanyAutoNum("TMC");

            // 是否存在重元件名称数据
            int result = bll.ExistForName(tModel.CheckComponentId, tModel.CheckComponentName, tModel.CheckComponentType, tModel.Project, tModel.CompanyId);
            if (result > 0)
            {
                // 系统日志 
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Exist, tModel);
                return Json(new { flag = "exist" });
            }

            // 新增(返回主键ID)
            int CheckComponentId = bll.AddMonthCheckComponent(tModel);

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
        /// <param name="checkComponentNumber">元件编号</param>
        /// <param name="checkComponentName">元件名称</param>
        /// <param name="checkComponentType">考核类型</param>
        /// <param name="project">项目</param>
        /// <param name="isFormula">公式计算</param>
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns>Json</returns>
        public ActionResult MonthCheckComponentList(int index, int size, string checkComponentNumber, string checkComponentName, string checkComponentType, string project, string isFormula, string state, string createTime, string ids, string tCheckFromType, string type)
        {
            string where = "";
            if (type != null)
            {
                // 查询本公司内提交状态绩效元件信息
                where = " State = 5 AND CompanyId =" + Auxiliary.CompanyID();
            }
            else
            {
                // 查询本公司内有效的(非作废)绩效元件信息
                where = " State != 20 AND CompanyId =" + Auxiliary.CompanyID();
            }

            // 元件编号
            if (!string.IsNullOrEmpty(checkComponentNumber))
            {
                where += string.Format(" And CheckComponentNumber like '%{0}%'", checkComponentNumber.Trim());
            }

            // 元件名称 
            if (!string.IsNullOrEmpty(checkComponentName))
            {
                where += string.Format(" And CheckComponentName like '%{0}%'", checkComponentName.Trim());
            }

            // 考核类型 
            if (!string.IsNullOrEmpty(checkComponentType))
            {
                where += string.Format(" And CheckComponentType = {0}", checkComponentType.Trim());
            }

            // 项目 
            if (!string.IsNullOrEmpty(project))
            {
                where += string.Format(" And Project = {0}", project.Trim());
            }

            // 公式计算 
            if (!string.IsNullOrEmpty(isFormula))
            {
                where += string.Format(" And IsFormula = {0}", isFormula.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And CheckComponentId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And CheckComponentId NOT IN (SELECT CheckComponentId FROM TraMonthCheckFromComponent WHERE CheckFromId = {0} AND State = 1)", ids.Trim());
                }
            }

            // 考核类型 
            if (!string.IsNullOrEmpty(tCheckFromType))
            {
                where += string.Format(" And CheckComponentType = {0}", tCheckFromType.Trim());
            }

            // 运输月度考核元件List
            List<TraMonthCheckComponentModel> list = bll.MonthCheckComponentList(index, size, where);

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
        public int MonthCheckComponentAmount(string checkComponentNumber, string checkComponentName, string checkComponentType, string project, string isFormula, string state, string createTime, string ids, string tCheckFromType, string type)
        { 
            string where = "";
            if (type != null)
            {
                // 查询本公司内提交状态绩效元件信息
                where = " State = 5 AND CompanyId =" + Auxiliary.CompanyID();
            }
            else
            {
                // 查询本公司内有效的(非作废)绩效元件信息
                where = " State != 20 AND CompanyId =" + Auxiliary.CompanyID();
            }

            // 元件编号
            if (!string.IsNullOrEmpty(checkComponentNumber))
            {
                where += string.Format(" And CheckComponentNumber like '%{0}%'", checkComponentNumber.Trim());
            }

            // 元件名称 
            if (!string.IsNullOrEmpty(checkComponentName))
            {
                where += string.Format(" And CheckComponentName like '%{0}%'", checkComponentName.Trim());
            }

            // 考核类型 
            if (!string.IsNullOrEmpty(checkComponentType))
            {
                where += string.Format(" And CheckComponentType = {0}", checkComponentType.Trim());
            }

            // 项目 
            if (!string.IsNullOrEmpty(project))
            {
                where += string.Format(" And Project = {0}", project.Trim());
            }

            // 公式计算 
            if (!string.IsNullOrEmpty(isFormula))
            {
                where += string.Format(" And IsFormula = {0}", isFormula.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", createTime.Trim());
            }
             
            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And CheckComponentId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And CheckComponentId NOT IN (SELECT CheckComponentId FROM TraMonthCheckFromComponent WHERE CheckFromId = {0} AND State = 1)", ids.Trim());
                }
            }

            // 考核类型 
            if (!string.IsNullOrEmpty(tCheckFromType))
            {
                where += string.Format(" And CheckComponentType = {0}", tCheckFromType.Trim());
            }

            return bll.MonthCheckComponentAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditMonthCheckComponent(TraMonthCheckComponentModel tModel)
        {
            // 编辑之前Model
            TraMonthCheckComponentModel beforeModel = bll.GetModelByID(tModel.CheckComponentId);

            // 是否存在重元件名称数据
            int result = bll.ExistForName(tModel.CheckComponentId, tModel.CheckComponentName,tModel.CheckComponentType,tModel.Project, beforeModel.CompanyId);
            if (result > 0)
            {
                // 系统日志 
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Exist, beforeModel);
                return Json(new { flag = "exist" });
            }

            // 编辑
            int row = bll.EditMonthCheckComponent(tModel);

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
            TraMonthCheckComponentModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 根据运输月度考核元件表主键ID查询运输月度绩效自定义元件表中状态为有效的数量 
            int result = bll.EffectiveStateAmount(tId);
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
        /// <param name="checkComponentNumber">元件编号</param>
        /// <param name="checkComponentName">元件名称</param>
        /// <param name="checkComponentType">考核类型</param>
        /// <param name="project">项目</param>
        /// <param name="isFormula">公式计算</param>
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string checkComponentNumber, string checkComponentName, string checkComponentType, string project, string isFormula, string state, string createTime)
        {
            // 查询本公司内有效的(非作废)绩效元件信息
            string where = " State != 20 AND CompanyId =" + Auxiliary.CompanyID();

            // 元件编号
            if (!string.IsNullOrEmpty(checkComponentNumber))
            {
                where += string.Format(" And CheckComponentNumber like '%{0}%'", checkComponentNumber.Trim());
            }

            // 元件名称 
            if (!string.IsNullOrEmpty(checkComponentName))
            {
                where += string.Format(" And CheckComponentName like '%{0}%'", checkComponentName.Trim());
            }

            // 考核类型 
            if (!string.IsNullOrEmpty(checkComponentType))
            {
                where += string.Format(" And CheckComponentType = {0}", checkComponentType.Trim());
            }

            // 项目 
            if (!string.IsNullOrEmpty(project))
            {
                where += string.Format(" And Project = {0}", project.Trim());
            }

            // 公式计算 
            if (!string.IsNullOrEmpty(isFormula))
            {
                where += string.Format(" And IsFormula = {0}", isFormula.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And State = {0}", state.Trim());
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

        #region 月考核取值条件

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="checkComponentNumber">元件编号</param>
        /// <param name="checkComponentName">元件名称</param>
        /// <param name="checkComponentType">考核类型</param>
        /// <param name="project">项目</param>
        /// <param name="isFormula">公式计算</param>
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns>Json</returns>
        public ActionResult MonthCheckWhereList(string tCheckComponentType,string tProject, string checkValueNumber)
        {
            // 条件
            string where = " 1=1";

            // 考核类型   
            if (!string.IsNullOrEmpty(tCheckComponentType))
            { 
                where += string.Format(" And CheckType = {0}", tCheckComponentType.Trim());
            }

            // 项目   
            if (!string.IsNullOrEmpty(tProject))
            { 
                where += string.Format(" And Project = {0}", tProject.Trim());
            }

            // 月考核取值 编号   
            if (!string.IsNullOrEmpty(checkValueNumber))
            {
                if (checkValueNumber == "0")
                {
                    where += " And State = 1 AND CompanyId =" + Auxiliary.CompanyID();
                }
                else
                {
                    where += " And State = 1";
                }

                where += string.Format(" And CheckValueNumber = {0}", checkValueNumber.Trim());
            }

            // List
            List<TraMonthCheckWhereModel> list = TMCWdal.MonthCheckWhereList(where);
             
            return Json(list);
        } 
        #endregion

        #endregion
    }
}
