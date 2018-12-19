//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-08    1.0        FJK        新建       
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
 * 类名：TraOperateController
 * 功能描述：运营类型维护表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraOperateController : Controller
    {
        //
        // GET: /Tra/TraOperate/

        // 运营类型维护BLL
        TraOperateBLL bll = new TraOperateBLL();

        // 月考核取值条件BLL
        TraMonthCheckWhereBLL TMCWbll = new TraMonthCheckWhereBLL();

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
            TraOperateModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary>
        public ActionResult View(int tId)
        {
            // 获取数据
            TraOperateModel model = bll.GetModelByID(tId);

            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="checkType">考核类型</param>
        /// <param name="operateName">运营类型名称</param>
        /// <returns></returns>
        public ActionResult OperateList(int index, int size, string checkType, string operateName)
        {
            // 查询本公司内非作废状态的数据
            string where = " State != 10 AND CompanyId =" + Auxiliary.CompanyID();

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

            // 运营类型维护List
            List<TraOperateModel> list = bll.OperateList(index, size, where);

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
        public int OperateCount(string checkType, string operateName)
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

            return bll.OperateCount(where);
        }

        /// <summary>
        /// 运营类型维护新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddOperate(TraOperateModel tModel)
        { 
            // 默认状态为创建 0
            tModel.State = 0;

            // 默认明细状态为无 0
            tModel.DetailState = 0;

            // 默认创建人为当前登录人
            tModel.CreateUserId = Auxiliary.UserID();

            // 默认创建机构ID为当前登录人所属机构
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 默认公司ID为当前登录人所属公司
            tModel.CompanyId = Auxiliary.CompanyID();

            // 新增
            if (bll.AddOperate(tModel) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 运营类型维护编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditOperate(TraOperateModel tModel)
        {
            // 编辑之前Model
            TraOperateModel beforeModel = bll.GetModelByID(tModel.OperateId);

            // 编辑
            int result = bll.EditOperate(tModel);

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
                TraOperateModel SubmitModel = bll.GetModelByID(tId);

                // 将提交的结果插入到月考核取值条件（TraMonthCheckWhere）表
                TraMonthCheckWhereModel TMCWmodel = new TraMonthCheckWhereModel();
                 
                TMCWmodel.WhereValue = SubmitModel.OperateName;
                TMCWmodel.Project = SubmitModel.Project;
                TMCWmodel.CheckType = SubmitModel.CheckType;
                TMCWmodel.ColumnName = "OperateValue";
                TMCWmodel.ColumnValue = SubmitModel.OperateId.ToString();

                // 默认类型为0:运营 月考核取值:0 状态:1 公司:当前登录人所属公司
                TMCWmodel.WhereType = 0;
                TMCWmodel.CheckValueNumber = "0";
                TMCWmodel.State = 1;
                TMCWmodel.CompanyId = Auxiliary.CompanyID();

                // 新增月考核取值条件
                TMCWbll.AddMonthCheckWhere(TMCWmodel);

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
            TraOperateModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 根据主键ID作废明细ID 修改主表明细状态
                bll.InvalidStateById(tId);
                new TraOperateBLL().UpdateDetailState(tId, 0);

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
