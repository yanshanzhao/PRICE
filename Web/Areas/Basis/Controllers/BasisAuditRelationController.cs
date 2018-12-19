//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-07    1.0        FJK        新建     
//2018-07-27    1.1        FJK        新增 1.判断是否有下一级审核流程及审核流程状态 2.编辑修改审核人员审核机构时同时修改下一级审核流程的提起人员提起机构            
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Web.Controllers;
using Model.Basis;
using BLL.Basis;
using System.Web.Mvc;
using System;
#endregion
/*********************************
 * 类名：SupplierServiceLevelController
 * 功能描述：供应商审核关系维护表 控制器 
 * ******************************/

namespace Web.Areas.Basis.Controllers
{
    public class BasisAuditRelationController : Controller
    {
        //
        // GET: /Basis/BasisAuditRelation/

        // 供应商审核关系维护BLL
        BasisAuditRelationBLL bll = new BasisAuditRelationBLL();

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
            BasisAuditRelationModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// Append
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Append)]
        public ActionResult Append(int tId)
        {
            // 获取数据
            BasisAuditRelationModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            BasisAuditRelationModel model = bll.GetModelByID(tId);

            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddAuditRelation(BasisAuditRelationModel tModel, string tOperate)
        {
            // Add/Append 新增/追加
            if (tOperate == "Add")
            {
                // 默认退回类型为无
                tModel.ReturnType = 0;

                // 默认结束审核为否
                tModel.EndAudit = 0;

                // 默认流程节点为是
                tModel.BeginNode = 1;

                // 上一个流程ID 0
                tModel.BeforeId = 0;

                // 流程编号
                tModel.AuditRelationNumber = Auxiliary.CurCompanyAutoNum("RAN");
            }
            else if (tOperate == "Append")
            {
                // 默认流程节点为否
                tModel.BeginNode = 0;

                // 上一个流程ID
                tModel.BeforeId = tModel.AuditRelationId;
            }

            // 默认状态为有效
            tModel.State = 1;

            // 公司ID(当前登录人所属公司ID)
            tModel.CompanyId = Auxiliary.CompanyID();

            // 若>O(新增成功)
            if (bll.AddAuditRelation(tModel) > 0)
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
        /// <param name="auditRelationTypeName">类型名称</param>
        /// <param name="auditRelationNumber">流程编号</param>
        /// <param name="departmentId">提起机构</param>
        /// <param name="todepartmentId">审核机构</param>
        /// <returns></returns>
        public ActionResult AuditRelationList(int index, int size, int auditRelationTypeName, string auditRelationNumber, string departmentId, string todepartmentId)
        {
            // 查询本公司内所有审核关系的信息。
            string where = " bar.CompanyId=" + Auxiliary.CompanyID();

            // 类型名称
            if (auditRelationTypeName != 0)
            {
                where += string.Format(" And AuditRelationType = '{0}'", auditRelationTypeName);
            }

            // 流程编号
            if (!string.IsNullOrEmpty(auditRelationNumber))
            {
                where += string.Format(" And AuditRelationNumber like '%{0}%'", auditRelationNumber.Trim());
            }

            // 提起机构
            if (!string.IsNullOrEmpty(departmentId))
            {
                where += string.Format(" And bar.DepartmentId = '{0}'", departmentId.Trim());
            }

            // 审核机构
            if (!string.IsNullOrEmpty(todepartmentId))
            {
                where += string.Format(" And ToDepartmentId = '{0}'", todepartmentId.Trim());
            }

            // 供应商审核关系List
            List<BasisAuditRelationModel> list = bll.AuditRelationList(index, size, where);
            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="auditRelationTypeName">类型名称</param>
        /// <param name="auditRelationNumber">流程编号</param>
        /// <param name="departmentId">提起机构</param>
        /// <param name="todepartmentId">审核机构</param>
        /// <returns></returns>
        public int AuditRelationCount(int auditRelationTypeName, string auditRelationNumber, string departmentId, string todepartmentId)
        {
            // 查询本公司内所有审核关系的信息。
            string where = " bar.CompanyId=" + Auxiliary.CompanyID();

            // 类型名称
            if (auditRelationTypeName != 0)
            {
                where += string.Format(" And AuditRelationType = '{0}'", auditRelationTypeName);
            }

            // 流程编号
            if (!string.IsNullOrEmpty(auditRelationNumber))
            {
                where += string.Format(" And AuditRelationNumber like '%{0}%'", auditRelationNumber.Trim());
            }

            // 提起机构
            if (!string.IsNullOrEmpty(departmentId))
            {
                where += string.Format(" And bar.DepartmentId = '{0}'", departmentId.Trim());
            }

            // 审核机构
            if (!string.IsNullOrEmpty(todepartmentId))
            {
                where += string.Format(" And ToDepartmentId = '{0}'", todepartmentId.Trim());
            }

            return bll.AuditRelationAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditAuditRelation(BasisAuditRelationModel tModel)
        {
            // 编辑之前Model
            BasisAuditRelationModel beforeModel = bll.GetModelByID(tModel.AuditRelationId);

            // 编辑
            int result = bll.EditAuditRelation(tModel);

            // 若影响行数>O(修改成功)
            if (result > 0)
            {
                // 查询是否有下一级审核流程 
                BasisAuditRelationModel model = bll.IsRelationByBeforeId(tModel.AuditRelationId);
                if (model != null && model.State == 1)
                {
                    // 修改下一级审核流程的提起人及提起机构
                    model.UserId = tModel.ToUserId;
                    model.DepartmentId = tModel.ToDepartmentId;
                    bll.EditAuditRelation(model);
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
        /// 作废
        /// </summary>
        /// <param name="ids">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult ChangeState(int tId, string tNumber)
        {
            // 作废之前Model
            BasisAuditRelationModel beforeModel = bll.GetModelByID(tId);

            // 审核流程表中同流程编号的ID集合
            List<int> list = bll.IsExistForNumber(tNumber.Trim());

            // 审核表中是否有同流程编号并未审核(State = 0)数据
            int AuditRow = bll.IsExistForAudit(tNumber.Trim());

            // 存在
            if (AuditRow != 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Exist, beforeModel);
                return Json(new { flag = "exist" });
            }

            // 未存在
            // ID集
            string ids = string.Join(",", list);

            // 修改/批量修改
            int row = bll.ChangeState(ids);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="auditRelationTypeName">类型名称</param>
        /// <param name="auditRelationNumber">流程编号</param>
        /// <param name="departmentId">提起机构</param>
        /// <param name="todepartmentId">审核机构</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(int auditRelationTypeName, string auditRelationNumber, string departmentId, string todepartmentId)
        {
            // 查询本公司内所有审核关系的信息。
            string where = " bar.CompanyId=" + Auxiliary.CompanyID();

            // 类型名称
            if (auditRelationTypeName != 0)
            {
                where += string.Format(" And AuditRelationType = '{0}'", auditRelationTypeName);
            }

            // 流程编号
            if (!string.IsNullOrEmpty(auditRelationNumber))
            {
                where += string.Format(" And AuditRelationNumber like '%{0}%'", auditRelationNumber.Trim());
            }

            // 提起机构
            if (!string.IsNullOrEmpty(departmentId))
            {
                where += string.Format(" And bar.DepartmentId = '{0}'", departmentId.Trim());
            }

            // 审核机构
            if (!string.IsNullOrEmpty(todepartmentId))
            {
                where += string.Format(" And ToDepartmentId = '{0}'", todepartmentId.Trim());
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where);

            // Excel
            Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }

        /// <summary>
        /// 判断是否有下一级审核流程及审核流程状态
        /// </summary> 
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        public ActionResult IsRelationByBeforeId(string tId)
        {
            // 供应商审核关系Model
            BasisAuditRelationModel model = bll.IsRelationByBeforeId(int.Parse(tId));

            if (model != null && model.State == 1)
            {
                return Json(new { flag = "exist" });
            }

            return Json(new { flag = "success" });
        }
        #endregion
    }
}
