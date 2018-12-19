//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-07    1.0        FJK        新建       
//-------------------------------------------------------------------------
#region 参考
using System.Collections.Generic;
using System.Web.Mvc;

using SRM.Model.Basis;
using SRM.Web.Controllers;
using SRM.BLL.Basis;
using Newtonsoft.Json.Converters;
using SRM.BLL.Sys;
using SRM.Model.Sys;
#endregion
/*********************************
 * 类名：BasisIntercalateController
 * 功能描述：部门考核设置表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Basis.Controllers
{
    public class BasisIntercalateController : Controller
    {
        //
        // GET: /Basis/BasisIntercalate/

        // 部门考核设置BLL
        BasisIntercalateBLL bll = new BasisIntercalateBLL();

        // 系统组织机构BLL
        SysDepartmentBLL SDbll = new SysDepartmentBLL();

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
            // 机构名称
            SysDepartmentModel sdModel = SDbll.GetModelByID(Auxiliary.DepartmentId().ToString());
            ViewBag.DepartmentName = sdModel.DepartmentName;

            return View();
        }

        /// <summary>
        /// Edit
        /// </summary> 
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            BasisIntercalateModel model = bll.GetModelByID(tId);

            // 机构名称
            SysDepartmentModel sdModel = SDbll.GetModelByID(model.DepartmentId.ToString());
            ViewBag.DepartmentName = sdModel.DepartmentName;

            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="departmentName">机构名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult IntercalateList(int index, int size, string departmentName, string state)
        {
            string where = " 1=1";

            // 查询本部门下（含本部门）的考核设置信息 
            if (Auxiliary.DepartmentId() != 0)
            {
                where += " AND BI.DepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + "))";
            }
            else
            {
                List<BasisIntercalateModel> NotList = null;
                return Json(NotList);
            }
             
            // 机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%'", departmentName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And BI.State = '{0}'", state.Trim());
            }

            // 部门考核设置List
            List<BasisIntercalateModel> list = bll.IntercalateList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="departmentName">机构名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public int IntercalateCount(string departmentName, string state)
        {
            // 查询本部门下（含本部门）的考核设置信息
            string where = " BI.DepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + "))";

            // 机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%'", departmentName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And BI.State = '{0}'", state.Trim());
            }

            return bll.IntercalateCount(where);
        }

        /// <summary>
        /// 部门考核设置新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddIntercalate(BasisIntercalateModel tModel)
        {
            // 判断同机构是否存在已设置的考核信息
            BasisIntercalateModel Model = bll.GetModelByDepartmentId(Auxiliary.DepartmentId());
            if (Model != null)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, Model);
                return Json(new { flag = "exist" });
            }

            // 默认状态为有效 1
            tModel.State = 1;

            // 默认创建人为当前登录人
            tModel.CreateUserId = Auxiliary.UserID();

            // 默认机构ID为当前登录人所属机构
            tModel.DepartmentId = Auxiliary.DepartmentId();

            // 默认公司ID为当前登录人所属公司
            tModel.CompanyId = Auxiliary.CompanyID();

            // 新增
            if (bll.AddIntercalate(tModel) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 部门考核设置编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditIntercalate(BasisIntercalateModel tModel)
        { 
            // 编辑之前Model
            BasisIntercalateModel beforeModel = bll.GetModelByID(tModel.IntercalateId);

            // 编辑
            int result = bll.EditIntercalate(tModel);

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
        /// 导出
        /// </summary>
        /// <param name="departmentName">机构名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string departmentName, string state)
        {
            // 查询本部门下（含本部门）的考核设置信息
            string where = " BI.DepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + "))";

            // 机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And DepartmentName like '%{0}%'", departmentName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And BI.State = '{0}'", state.Trim());
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
