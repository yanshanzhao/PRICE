//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-16    1.0        FJK        新建-维护关系信息                    
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
 * 类名：TraRelationController
 * 功能描述：运输供应商培养表 控制器 
 * ******************************/
  
namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraRelationController : Controller
    {
        //
        // GET: /Tra/TraRelation/

        // 运输供应商培养BLL
        TraRelationBLL bll = new TraRelationBLL();

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
            TraRelationModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            TraRelationModel model = bll.GetModelByID(tId);

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
        public ActionResult AddRelation(TraRelationModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认创建 0
            tModel.State = 0;
             
            // 新增(返回主键ID)
            int RelationId = bll.AddRelation(tModel);

            // 若主键>O(新增成功)
            if (RelationId > 0)
            {
                // 更改年度绩效结果表使用状态
                new TraYearCheckResultBLL().ChangeState(tModel.CheckYearId, 1);

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
        public ActionResult RelationList(int index, int size, string supplierName, string year, string suppServiceName, string state, string createTime)
        {
            // 查询本系统内所有供应商有效的维护关系信息。
            // string where = " TR.State != 10";

            // 查询本公内所有供应商维护关系信息。
            string where = " TR.CompanyId =" + Auxiliary.CompanyID();

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

            // 服务等级名称  
            if (!string.IsNullOrEmpty(suppServiceName))
            {
                where += string.Format(" And SuppServiceName  like '%{0}%'", suppServiceName.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TR.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TR.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            // 运输供应商培养List
            List<TraRelationModel> list = bll.RelationList(index, size, where);

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
        public int RelationAmount(string supplierName, string year, string suppServiceName, string state, string createTime)
        {
            // 查询本系统内所有供应商有效的维护关系信息。
            // string where = " TR.State != 10";

            // 查询本公司内所有供应商维护关系信息。
            string where = " TR.CompanyId =" + Auxiliary.CompanyID();

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

            // 服务等级名称  
            if (!string.IsNullOrEmpty(suppServiceName))
            {
                where += string.Format(" And SuppServiceName = {0}", suppServiceName.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TR.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TR.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            return bll.RelationAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditRelation(TraRelationModel tModel)
        {
            // 编辑之前Model
            TraRelationModel beforeModel = bll.GetModelByID(tModel.RelationId);

            // 编辑
            int row = bll.EditRelation(tModel);

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
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            // 作废之前Model
            TraRelationModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 更改年度绩效结果表使用状态
                // new TraYearCheckResultBLL().ChangeState(,1);

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
        public ActionResult Export(string supplierName, string year, string suppServiceName, string state, string createTime)
        {
            // 查询本系统内所有供应商有效的维护关系信息。
            // string where = " TR.State != 10";

            // 查询本系统内所有供应商维护关系信息。
            string where = " 1=1";

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

            // 服务等级名称  
            if (!string.IsNullOrEmpty(suppServiceName))
            {
                where += string.Format(" And SuppServiceName = {0}", suppServiceName.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TR.CreateTime,120) like '%{0}%'", createTime.Trim());
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

        #region 以TraYearCheckResult为主表查询运输供应商

        /// <summary>
        /// 数据集 运输供应商
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">运输供应商名称</param> 
        /// <returns>Json</returns>
        public ActionResult ResultTransportList(int index, int size, string supplierName)
        {
            // 状态为提交 5  使用状态为未使用 0。
            string where = " CheckState = 0 AND TYCR.State = 5 AND TYCR.CompanyId =" + Auxiliary.CompanyID();

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 运输供应商List
            List<TraYearCheckResultModel> list = new TraYearCheckResultBLL().ResultTransportList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数 运输供应商
        /// </summary> 
        /// <param name="supplierName">供应商名称</param> 
        /// <returns></returns>
        public int ResultTransportCount(string supplierName)
        {
            // 状态为提交 5  使用状态为未使用 0。
            string where = " CheckState = 0 AND TYCR.State = 5 AND TYCR.CompanyId =" + Auxiliary.CompanyID();

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            return new TraYearCheckResultBLL().ResultTransportCount(where);
        }

        #endregion

        #endregion
    }
}
