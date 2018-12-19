// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-29    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using System.Web.Mvc;
using System;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using System.Linq;
#endregion
/*********************************
 * 类名：TraChooseController
 * 功能描述：运输选择元件表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraChoiceComponentController : Controller
    {
        //
        // GET: /Tra/TraChoiceComponent/

        // 运输月度考核元件BLL
        TraChoiceComponentBLL bll = new TraChoiceComponentBLL();

        #region 页面

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
            TraChoiceComponentModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            TraChoiceComponentModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// Detail
        /// </summary> 
        [Operate(Name = OperateEnum.Detail)]
        public ActionResult Detail(int tId)
        {
            // 获取数据
            TraChoiceComponentModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 添加明细信息
        /// </summary>
        public ActionResult AddDetail(string url, int tId)
        {
            ViewBag.url = url;
            ViewBag.ChoiceId = tId;
            return View();
        }

        /// <summary>
        /// 编辑明细信息
        /// </summary>
        public ActionResult EditDetail(string url, int tId)
        {
            ViewBag.url = url;
            ViewBag.ChoiceDetailId = tId;
            TraChoiceComponentModel model = bll.GetModelByIDs(tId);
            return View(model);
        }
        #endregion

        #region 方法

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddTraChoiceComponent(TraChoiceComponentModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认创建 1
            tModel.State = 0;

            // 月考核元件编号
            tModel.ChoiceNumber = Auxiliary.CurCompanyAutoNum("TCC");

            // 是否存在重元件名称数据
            int result = bll.ExistForName(tModel.ChoiceId, tModel.ChoiceName, tModel.ChoiceType, tModel.CompanyId);
            if (result > 0)
            {
                // 系统日志 
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Exist, tModel);
                return Json(new { flag = "exist" });
            }

            // 新增(返回主键ID)
            int ChoiceId = bll.AddTraChoiceComponent(tModel);

            // 若主键>O(新增成功)
            if (ChoiceId > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        #endregion

        #region 明细新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddDetails(TraChoiceComponentModel tModel)
        {
            // 编辑之前Model
            TraChoiceComponentModel beforeModel = bll.GetModelByID(tModel.ChoiceId);

            // 公司ID
            tModel.ChoiceId = beforeModel.ChoiceId; 

            // 状态默认创建 1
            tModel.State = 1;
              
            // 新增(返回主键ID)
            int ChoiceId = bll.AddTraChoiceComponentDetail(tModel);

            // 若主键>O(新增成功)
            if (ChoiceId > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 数据集

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="checkComponentNumber">元件编号</param>
        /// <param name="checkComponentName">元件名称</param>
        /// <param name="checkComponentType">评分分类</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns>Json</returns>
        public ActionResult TraChoiceComponentList(int index, int size, string choiceNumber, string choiceName, string choiceType, string state, string createTime, string ids, string type)
        { 
            string where = "";
            if (type != null)
            {
                // 查询本公司内提交状态绩效元件信息
                where = " State = 1 AND TCC.CompanyId =" + Auxiliary.CompanyID();
            }
            else
            {
                // 查询本公司内有效的(非作废)绩效元件信息
                where = " (State != 2 OR State != 10) AND TCC.CompanyId =" + Auxiliary.CompanyID();
            }

            // 元件编号
            if (!string.IsNullOrEmpty(choiceNumber))
            {
                where += string.Format(" And TCC.ChoiceNumber like '%{0}%'", choiceNumber.Trim());
            }

            // 元件名称 
            if (!string.IsNullOrEmpty(choiceName))
            {
                where += string.Format(" And TCC.ChoiceName like '%{0}%'", choiceName.Trim());
            }

            // 评分分类 
            if (!string.IsNullOrEmpty(choiceType))
            {
                where += string.Format(" And TCC.ChoiceType = {0}", choiceType.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TCC.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TCC.CreateTime,120) like '%{0}%'", createTime.Trim());
            }
            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And ChoiceId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And ChoiceId NOT IN (SELECT ChoiceId FROM TraChoiceFromComponent WHERE ChoiceFromId = {0} AND State = 1)", ids.Trim());
                }
            }

            // 运输月度考核元件List
            List<TraChoiceComponentModel> list = bll.TraChoiceComponentList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="checkComponentNumber">元件编号</param>
        /// <param name="checkComponentName">元件名称</param>
        /// <param name="checkComponentType">评分分类</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int TraChoiceComponentAmount(string choiceNumber, string choiceName, string choiceType, string state, string createTime, string ids, string type)
        {
            string where = "";
            if (type != null)
            {
                // 查询本公司内提交状态绩效元件信息
                where = " State = 1 AND TCC.CompanyId =" + Auxiliary.CompanyID();
            }
            else
            {
                // 查询本公司内有效的(非作废)绩效元件信息
                where = " (State != 2 OR State != 10) AND TCC.CompanyId =" + Auxiliary.CompanyID();
            }

            // 元件编号
            if (!string.IsNullOrEmpty(choiceNumber))
            {
                where += string.Format(" And TCC.ChoiceNumber like '%{0}%'", choiceNumber.Trim());
            }

            // 元件名称 
            if (!string.IsNullOrEmpty(choiceName))
            {
                where += string.Format(" And TCC.ChoiceName like '%{0}%'", choiceName.Trim());
            }

            // 评分分类 
            if (!string.IsNullOrEmpty(choiceType))
            {
                where += string.Format(" And TCC.ChoiceType = {0}", choiceType.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TCC.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TCC.CreateTime,120) like '%{0}%'", createTime.Trim());
            }
            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And ChoiceId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And ChoiceId NOT IN (SELECT ChoiceId FROM TraChoiceFromComponent WHERE ChoiceFromId = {0} AND State = 1)", ids.Trim());
                }
            }
            return bll.TraChoiceComponentAmount(where);
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraChoiceComponent(TraChoiceComponentModel tModel)
        {
            // 编辑之前Model
            TraChoiceComponentModel beforeModel = bll.GetModelByID(tModel.ChoiceId);

            // 是否存在重元件名称数据
            int result = bll.ExistForName(tModel.ChoiceId, tModel.ChoiceName, tModel.ChoiceType, beforeModel.CompanyId);
            if (result > 0)
            {
                // 系统日志 
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Exist, beforeModel);
                return Json(new { flag = "exist" });
            }

            // 编辑
            int row = bll.EditTraChoiceComponent(tModel);

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

        #region 编辑 招标通知表

        /// <summary>
        /// 编辑 招标通知表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditDetails(TraChoiceComponentModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            TraChoiceComponentModel beforeModel = bll.GetModelByIDs(model.ChoiceDetailId);

            int result = bll.EditDetails(model);

            if (result > 0) 
            { 
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 提交

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
        #endregion

        #region 作废

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            // 作废之前Model
            TraChoiceComponentModel beforeModel = bll.GetModelByID(tId);

            int row = 0;

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 根据运输月度考核元件表主键ID查询运输月度绩效自定义元件表中状态为有效的数量 
            int result = bll.EffectiveStateAmount(tId);

            if (result == 0)
            {
                // 作废(更改状态)
                row = bll.InvalidStates(tId, delUserId);

            }
            if (beforeModel.State == 0)
            {
                // 删除(更改状态)
                row = bll.InvalidState(tId, delUserId);
            }

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
        #endregion

        #region 导出

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="checkComponentNumber">元件编号</param>
        /// <param name="checkComponentName">元件名称</param>
        /// <param name="checkComponentType">评分分类</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string choiceNumber, string choiceName, string choiceType, string state, string createTime)
        {
            string where = string.Format(" TCC.CompanyId={0}", Auxiliary.CompanyID());

            // 元件编号
            if (!string.IsNullOrEmpty(choiceNumber))
            {
                where += string.Format(" And TCC.ChoiceNumber like '%{0}%'", choiceNumber.Trim());
            }

            // 元件名称 
            if (!string.IsNullOrEmpty(choiceName))
            {
                where += string.Format(" And TCC.ChoiceName like '%{0}%'", choiceName.Trim());
            }

            // 评分分类 
            if (!string.IsNullOrEmpty(choiceType))
            {
                where += string.Format(" And TCC.ChoiceType = {0}", choiceType.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TCC.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TCC.CreateTime,120) like '%{0}%'", createTime.Trim());
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

        #endregion

        #region 明细方法

        #region 分配明细 数据集

        /// <summary>
        /// 分配明细 数据集
        /// </summary>
        /// <param name="index">当前页面</param>
        /// <param name="size">页面索引</param>
        /// <param name="choiceId">运作id</param>
        /// <param name="choiceContent">选择内容</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult TraChoiceComponentDetailList(int index, int size, int choiceId, string choiceContent)
        {

            string where = string.Format("  TCCD.State =1  And TCCD.ChoiceId = {0}", choiceId);

            // 选择内容
            if (!string.IsNullOrEmpty(choiceContent))
            {
                where += string.Format(" And TCCD.ChoiceContent LIKE '%{0}%'", choiceContent.Trim());
            }

            List<TraChoiceComponentModel> list = bll.TraChoiceComponentDetailList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 分配明细 数据记录数

        /// <summary>
        /// 分配明细 数据记录数
        /// </summary>
        /// <param name="choiceId">运作id</param>
        /// <param name="choiceContent">选择内容</param>
        /// <returns></returns>
        public int TraChoiceComponentDetailCount(int choiceId, string choiceContent)
        {
            string where = string.Format("  TCCD.State =1  And TCCD.ChoiceId = {0}", choiceId);

            // 选择内容
            if (!string.IsNullOrEmpty(choiceContent))
            {
                where += string.Format(" And TCCD.ChoiceContent LIKE '%{0}%'", choiceContent.Trim());
            }

            return bll.TraChoiceComponentDetailCount(where);
        }
        #endregion

        #region 作废元件明细数据

        /// <summary>
        ///  作废元件明细数据
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidDetailState(int tId)
        {
            TraChoiceComponentModel beforeModel = bll.GetModelByID(tId);

            //作废元件明细数据
            int row = bll.InvalidDetailState(tId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion

        #endregion
    }
}
