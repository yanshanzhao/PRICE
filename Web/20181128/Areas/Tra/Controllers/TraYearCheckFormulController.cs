//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-20    1.0        FJK        新建       
//-------------------------------------------------------------------------
#region 参考
using System.Collections.Generic;
using System.Web.Mvc;

using SRM.Model.Tra;
using SRM.Web.Controllers;
using SRM.BLL.Tra;
using Newtonsoft.Json.Converters;
using SRM.BLL.Sys;
using SRM.Model.Sys;
#endregion
/*********************************
 * 类名：TraYearCheckFormulController
 * 功能描述：年度绩效公式表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraYearCheckFormulController : Controller
    {
        //
        // GET: /Tra/TraYearCheckFormul/

        // 年度绩效公式BLL
        TraYearCheckFormulBLL bll = new TraYearCheckFormulBLL();

        #region 页面

        /// <summary>
        /// Index
        /// </summary> 
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add
        /// </summary> 
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary> 
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            TraYearCheckFormulModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary>
        public ActionResult View(int tId)
        {
            // 获取数据
            TraYearCheckFormulModel model = bll.GetModelByID(tId);

            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="tFormulaType">公式类型</param>
        /// <param name="tState">状态</param>
        /// <returns></returns>
        public ActionResult YearCheckFormulList(int index, int size, string tFormulaType, string tState)
        {
            // 查询本公司内状态为初始和提交的信息
            string where = string.Format(" State IN (0,1) AND CompanyId = {0}", Auxiliary.CompanyID());

            // 公式类型
            if (!string.IsNullOrEmpty(tFormulaType))
            {
                where += string.Format(" And FormulaType = '{0}'", tFormulaType.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(tState))
            {
                where += string.Format(" And State = '{0}'", tState.Trim());
            }

            // 年度绩效公式List
            List<TraYearCheckFormulModel> list = bll.YearCheckFormulList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="checkType">考核类型</param>
        /// <param name="operateName">运营类型名称</param>
        /// <returns></returns>
        public int YearCheckFormulCount(string checkType, string operateName)
        {
            // 查询本公司内非作废状态的数据
            string where = "  State != 10 AND CompanyId =" + Auxiliary.CompanyID();

            // 考核类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And CheckType = '{0}'", checkType.Trim());
            }

            // 运营类型名称
            if (!string.IsNullOrEmpty(operateName))
            {
                where += string.Format(" And YearCheckFormulName like '%{0}%'", operateName.Trim());
            }

            return bll.YearCheckFormulCount(where);
        }

        /// <summary>
        /// 年度绩效公式新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddYearCheckFormul(TraYearCheckFormulModel tModel)
        {
            // 默认状态为创建 0
            tModel.State = 0;

            // 默认创建人为当前登录人
            tModel.CreateUserId = Auxiliary.UserID();

            // 默认创建机构ID为当前登录人所属机构
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 默认公司ID为当前登录人所属公司
            tModel.CompanyId = Auxiliary.CompanyID();

            // 新增
            if (bll.AddYearCheckFormul(tModel) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 年度绩效公式编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditYearCheckFormul(TraYearCheckFormulModel tModel)
        {
            // 编辑之前Model
            TraYearCheckFormulModel beforeModel = bll.GetModelByID(tModel.FormulaId);

            // 编辑
            int result = bll.EditYearCheckFormul(tModel);

            // 若影响行数>O(修改成功)
            if (result > 0)
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
                // 提交数据Model
                TraYearCheckFormulModel SubmitModel = bll.GetModelByID(tId);

                // 将提交的结果插入到月考核取值条件（TraMonthCheckWhere）表
                TraMonthCheckWhereModel TMCWmodel = new TraMonthCheckWhereModel();

                //TMCWmodel.WhereValue = SubmitModel.OperateName;
                //TMCWmodel.Project = SubmitModel.Project;
                //TMCWmodel.CheckType = SubmitModel.CheckType;
                //TMCWmodel.ColumnName = "OperateValue";
                //TMCWmodel.ColumnValue = SubmitModel.OperateId.ToString();

                //// 默认类型为0:运营 月考核取值:0 状态:1 公司:当前登录人所属公司
                //TMCWmodel.WhereType = 0;
                //TMCWmodel.CheckValueNumber = "0";
                //TMCWmodel.State = 1;
                //TMCWmodel.CompanyId = Auxiliary.CompanyID();

                //// 新增月考核取值条件
                //TMCWbll.AddMonthCheckWhere(TMCWmodel);

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
            TraYearCheckFormulModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 成功行数
            int row = 0;

            // 作废(更改状态)
            if (beforeModel.State == 1)
            {
                // 状态为提交状态的，作废之后状态变为作废状态
                bll.InvalidState(tId, delUserId, 10);
            }
            else
            {
                // 状态为初始状态的，作废之后状态变为删除状态
                bll.InvalidState(tId, delUserId, 20);
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

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="checkType">考核类型</param>
        /// <param name="operateName">运营类型名称</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string checkType, string operateName)
        {
            // 查询本公司内非作废状态的数据
            string where = "  State != 10 AND CompanyId =" + Auxiliary.CompanyID();

            // 考核类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And CheckType = '{0}'", checkType.Trim());
            }

            // 运营类型名称
            if (!string.IsNullOrEmpty(operateName))
            {
                where += string.Format(" And OperateName like '%{0}%'", operateName.Trim());
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }
        #endregion 
    }
}
