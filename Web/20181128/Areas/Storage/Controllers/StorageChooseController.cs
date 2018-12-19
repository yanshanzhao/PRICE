//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-14    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.BLL.Storage;
using SRM.Model.Storage;
using SRM.Model.Supplier;
using SRM.BLL.Supplier;
#endregion
/*********************************
 * 类名：StorageChooseController
 * 功能描述：仓储选择申请表 控制器 
 * ******************************/
namespace SRM.Web.Areas.Storage.Controllers
{
    public class StorageChooseController : Controller
    {
        //
        // GET: /Storage/StorageChoose/ 

        StorageChooseBLL bll = new StorageChooseBLL();

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
            ViewBag.StorageNumber = Auxiliary.CurCompanyAutoNum("RAN");
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
            StorageChooseModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            StorageChooseModel model = bll.GetModelByID(tId);
            return View(model);
        }

        #region 弹出仓储供应商信息窗口

        /// <summary>
        /// 选择供应商规模信息
        /// </summary>
        public ActionResult StorageChooseDetail(string url, string ids, string type)
        {
            ViewBag.url = url;
            ViewBag.type = type;
            ViewBag.ids = ids;
            return View();
        }
        #endregion

        #endregion

        #region 方法

        #region 显示index中的数据集

        /// <summary>
        /// 显示index中的数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="ApplyTime"></param>
        /// <param name="State"></param>
        /// <param name="UseState"></param>
        /// <returns></returns>
        public ActionResult StorageChooseList(int index, int size, string ApplyTime, string State, string UseState)
        {
            string where = " sch.State != 10 AND sch.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 申请时间
            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,sch.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And sch.State = {0}", State.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(UseState))
            {
                where += string.Format(" And sch.UseState = {0}", UseState.Trim());
            }

            List<StorageChooseModel> list = bll.StorageChooseList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 显示index中的数据记录

        /// <summary>
        /// 显示index中的数据记录
        /// </summary>
        /// <param name="ApplyTime"></param>
        /// <param name="State"></param>
        /// <param name="UseState"></param>
        /// <returns></returns>
        public int StorageChooseCount(string ApplyTime, string State, string UseState)
        {
            string where = " sch.State != 10 AND sch.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 申请时间 
            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,sch.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And sch.State = {0}", State.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(UseState))
            {
                where += string.Format(" And sch.UseState = {0}", UseState.Trim());
            }
            return bll.StorageChooseCount(where);
        }
        #endregion

        #region  新增时：主表明细表同时新增 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddStorageChoose(StorageChooseModel model)
        {

            model.State = 0;// 状态 未提交

            model.UseState = 0;// 使用状态 未使用

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            model.StorageChooseNumber = Auxiliary.CurCompanyAutoNum("STC");//仓储选择编号

            int StorageChooseId = bll.AddStorageChoose(model);

            if (StorageChooseId > 0)
            {
                if (!string.IsNullOrEmpty(model.StorageChooseIdList))
                {
                    List<string> storageChooseIdList = new List<string>(model.StorageChooseIdList.Split(','));
                    bll.AddStorageChooseDetail(storageChooseIdList, StorageChooseId);
                }
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 编辑时：保存主表的数据 

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditStorageChoose(StorageChooseModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            StorageChooseModel beforeModel = bll.GetModelByID(model.StorageChooseId);

            int result = bll.EditStorageChoose(model);

            if (result > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);

            return Json(new { flag = "fail" });
        }
        #endregion

        #region 编辑时；新增明细表 

        /// <summary>
        /// 编辑时；新增明细表
        /// </summary>
        /// <param name="chooseDetail"></param>
        /// <param name="checkSupplierChooseId"></param>
        /// <returns></returns>
        public ActionResult AddStorageChooses(string chooseDetail, string checkSupplierChooseId)
        {
            List<string> chooseDetailList = new List<string>(chooseDetail.Split(','));
            int row = bll.AddStorageChooseDetail(chooseDetailList, Convert.ToInt32(checkSupplierChooseId));

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 新增时和编辑时： 弹出的仓储供应商数据集list 

        /// <summary>
        /// 新增时和编辑时： 弹出的仓储供应商数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="supplierName"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult SupplierStoragelist(int index, int size, string supplierName, string type, string ids)
        {

            string where = string.Format(" supp.StorageState='F2' and supp.DepartmentId={0}", Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And supp.StorageId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And supp.StorageId NOT IN (SELECT SupplierStorageId FROM StorageChooseDetail WHERE StorageChooseId ={0})", ids.Trim());
                }
            }
            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And sup.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }
            List<StorageChooseModel> list = bll.SupplierStoragelist(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 新增时和编辑时： 弹出的仓储供应商数据集count 

        /// <summary>
        /// 新增时和编辑时： 弹出的仓储供应商数据集count  
        /// </summary>
        /// <param name="supplierName"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int SupplierStorageAmount(string supplierName, string type, string ids)
        {
            string where = string.Format(" supp.StorageState='F2' and supp.DepartmentId={0}", Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And supp.StorageId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And supp.StorageId NOT IN (SELECT SupplierStorageId FROM StorageChooseDetail WHERE StorageChooseId ={0})", ids.Trim());
                }
            }
            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And sup.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            return bll.SupplierStorageAmount(where);
        }
        #endregion

        #region 新增按钮时：显示下方的仓储选择明细数据集list

        /// <summary>
        /// 新增按钮时：显示下方的仓储选择明细数据集list
        /// </summary>
        /// <param name="checkSupplierChooseId"></param>
        /// <returns></returns>
        public ActionResult StorageChoosedetailaddList(string checkSupplierChooseId)
        {
            string where = string.Format(" supp.StorageId in ({0})", checkSupplierChooseId);

            List<StorageChooseModel> list = bll.StorageChoosedetailaddList(where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 新增按钮时：显示下方的仓储选择明细数据记录count 

        /// <summary>
        /// 新增按钮时：显示下方的仓储选择明细数据记录count   
        /// </summary>
        /// <param name="checkSupplierChooseId"></param>
        /// <returns></returns>
        public int StorageChoosedetailaddAmount(string checkSupplierChooseId)
        {
            string where = string.Format(" supp.StorageId in ({0})", checkSupplierChooseId);

            return bll.StorageChoosedetailaddAmount(where);
        }
        #endregion

        #region 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   

        /// <summary>
        /// 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list
        /// </summary>
        /// <param name="checkSupplierChooseId"></param>
        /// <returns></returns>
        public ActionResult StorageChoosedetaileditList(string checkSupplierChooseId)
        {

            string where = string.Format(" sc.StorageChooseId in ({0})", checkSupplierChooseId);

            List<StorageChooseModel> list = bll.StorageChoosedetaileditList(where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 编辑按钮时：显示下方的仓储选择明细数据记录count 

        /// <summary>
        /// 编辑按钮时：显示下方的仓储选择明细数据记录count
        /// </summary>
        /// <param name="checkSupplierChooseId"></param>
        /// <returns></returns>
        public int StorageChoosedetaileditAmount(string checkSupplierChooseId)
        {
            string where = string.Format(" sc.StorageChooseId in ({0})", checkSupplierChooseId);

            return bll.StorageChoosedetaileditAmount(where);
        }
        #endregion

        #region 提交按钮逻辑

        /// <summary>
        /// 提交按钮逻辑
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="tSupplierid"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitStorageChoose(int tId, int tSupplierid)
        {
            // 影响行数
            int row = 0;

            // 更新之前table
            StorageChooseModel beforeModel = bll.GetModelByID(tId);

            // 审核流程model
            BasisAuditRelationModel model = Auxiliary.IsMatch(2, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID());

            if (model != null)
            {
                bll.SubmitStorageChoose(tId);
                StorageChooseAuditModel Auditmodel = new StorageChooseAuditModel();
                Auditmodel.AuditRelationNumber = model.AuditRelationNumber;
                Auditmodel.AuditRelationId = model.AuditRelationId;
                Auditmodel.OtherId = tSupplierid;
                Auditmodel.PresentId = tId;
                Auditmodel.SupplierAuditType = model.AuditRelationType;
                Auditmodel.PresentDepartmentId = model.DepartmentId;
                Auditmodel.PresentUserId = model.UserId;
                Auditmodel.AuditDepartmentId = model.ToDepartmentId;
                Auditmodel.AuditUserId = model.ToUserId;
                Auditmodel.AuditRelationName = model.AuditRelationName;
                Auditmodel.CompanyId = model.CompanyId;

                // 默认状态 未审核
                Auditmodel.State = 0;
                StorageChooseAuditBLL auditBLL = new StorageChooseAuditBLL();

                row = auditBLL.AddAuditRelation(Auditmodel);
                if (row > 0)
                {
                    Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                    return Json(new { flag = "success", content = "提交成功！" });
                }
            }


            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail", content = "无匹配审核流程,不能提交！" });
        }
        #endregion

        #region 作废按钮逻辑

        /// <summary>
        /// 作废按钮逻辑
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            StorageChooseModel beforeModel = bll.GetModelByID(tId);

            int delUserId = Auxiliary.UserID();

            int row = bll.InvalidState(tId, delUserId);
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region  导出按钮逻辑

        /// <summary>
        /// 导出按钮逻辑
        /// </summary>
        /// <param name="ApplyTime"></param>
        /// <param name="State"></param>
        /// <param name="UseState"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string ApplyTime, string State, string UseState)
        {
            string where = " sch.State != 10 AND sch.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 申请时间
            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,sch.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And sch.State = {0}", State.Trim());
            }

            // 审核状态
            if (!string.IsNullOrEmpty(UseState))
            {
                where += string.Format(" And sch.UseState = {0}", UseState.Trim());
            }
            System.Data.DataTable dt = bll.ExportDataTable(where);
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new
            {
                Type = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        #region 撤销按钮
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Revoke)]
        public ActionResult RevokeData(int tId)
        {
            // 更新之前table
            StorageChooseModel beforeModel = bll.GetModelByID(tId);

            // 流程开始已提（非撤销状态）的数量 
            int count = 0;
              
            BasisAuditRelationModel models = Auxiliary.IsMatch(2, 1, 1, Auxiliary.UserID(), Auxiliary.DepartmentId(), Auxiliary.CompanyID());

            // 若审核流程不为NULL
            if (models != null)
            {
                // 查询提交的数据是否已审核(流程开始) 根据本数据ID(提交表ID) 流程ID(审核流程ID) 查询审核表(StorageChooseAuditModel)
                StorageChooseAuditModel model = new StorageChooseAuditBLL().GetRevokeModel(tId, models.AuditRelationId);

                // 如果未审核
                if (model.State == 0)
                {
                    // 查询流程开始已提（非撤销状态）的数量
                    count = new StorageChooseAuditBLL().GetRevokeCouunt(tId, model.AuditRelationId);

                    if (count > 1)
                    {
                        // 修改本数据状态为驳回状态
                        bll.ChangeState(tId, 1);

                        // 修改审核表数据状态为撤销状态
                        new StorageChooseAuditBLL().ChangeState(model.SupplierAuditId, 10, "");

                        Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "撤销", Id = tId, State = "驳回状态" });
                        return Json(new { flag = "success", content = "撤销成功！" });
                    }
                    else
                    {
                        // 修改本数据状态为初始状态
                        bll.ChangeState(tId, 0);

                        // 修改审核表数据状态为撤销状态
                        new StorageChooseAuditBLL().ChangeState(model.SupplierAuditId, 10, "");

                        Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "撤销", Id = tId, State = "初始状态" });
                        return Json(new { flag = "success", content = "撤销成功！" });
                    }
                }
                else
                {
                    Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "撤销", Id = tId, State = "审核中" });
                    return Json(new { flag = "fail", content = "已经审核的数据无法撤销！" });
                }
            }
            else
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "撤销", Id = tId });
                return Json(new { flag = "fail", content = "无匹配审核流程！" });
            }
        }

        #endregion

        #endregion 
    }
}