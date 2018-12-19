//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-12    1.0        FJK        新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.Model.Storage;
using SRM.Model.Supplier;
using SRM.BLL.Supplier;
using System.Linq;
using SRM.Model.Sys;
using SRM.BLL.Sys;
#endregion
/*********************************
 * 类名：SupplierStorageController
 * 功能描述：仓储供应商表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Supplier.Controllers
{
    public class SupplierStorageController : Controller
    {
        //
        // GET: /Supplier/SupplierStorage/

        // 仓储供应商BLL
        SupplierStorageBLL bll = new SupplierStorageBLL();

        // 供应商审核
        SupplierAuditBLL auditBLL = new SupplierAuditBLL();

        // 附件BLL
        SupplierAdjunctBLL sabll = new SupplierAdjunctBLL();
        SupplierCyclBLL scbll = new SupplierCyclBLL();

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
            ViewBag.StorageNumber = Auxiliary.CurCompanyAutoNum("STO");
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
            SupplierStorageModel model = bll.GetModelByID(tId);

            List<temfile> filelist = sabll.SuppFileList(tId, 30);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();
            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            SupplierStorageModel model = bll.GetModelByID(tId);

            List<temfile> filelist = sabll.SuppFileList(tId, 30);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 供应商明细
        /// </summary>
        public ActionResult SupplierDetail(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// Assess
        /// </summary>
        public ActionResult Assess(int tId)
        {
            // 获取数据
            SupplierStorageModel model = bll.GetModelByID(tId);

            // 仓储供应商ID
            ViewBag.StorageId = model.StorageId;

            // 供应商名称
            ViewBag.SupplierName = model.SupplierName;

            // 仓库名称
            ViewBag.StorageName = model.StorageName;

            // 仓库业主名称
            ViewBag.StorageOwner = model.StorageOwner;

            // 企业性质
            ViewBag.EnterpriseTypeName = model.EnterpriseTypeName;

            // 联系人
            ViewBag.ContactsName = model.ContactsName;

            // 仓库详细地址
            ViewBag.Address = model.Address;

            // 联系电话
            ViewBag.ContactsPhone = model.ContactsPhone;

            // 库区总仓储面积
            ViewBag.TotalArea = model.TotalArea;

            // 拟租总面积
            ViewBag.RentArea = model.RentArea;

            // 如是立体库
            if (model.StorageNature == 0)
            {
                // 是否立体库
                ViewBag.StorageNature = "是";

                // 空闲面积/托盘位(库区)
                ViewBag.LeisureArea = model.LeisureArea + " / " + model.TotalTray;

                // 托盘位(拟租)
                ViewBag.RentTray = model.RentTray;
            }
            else
            {
                ViewBag.StorageNature = "否";
                ViewBag.LeisureArea = "/";
                ViewBag.RentTray = "/";
            }

            // 机构名称
            SysDepartmentModel departmenModel = new SysDepartmentBLL().GetModelByID(model.DepartmentId.ToString());
            ViewBag.DepartmentName = departmenModel.DepartmentName;

            // 元件
            List<StorageComponentModel> componentList = bll.ComponentList();

            // 自定义表单ID
            ViewBag.FromId = componentList[0].FromId;

            // 合同最小值
            ViewBag.AdoptMin = componentList[0].AdoptMin;

            // 标准得分总分
            int standardScore = 0;
            for (int i = 0; i < componentList.Count; i++)
            {
                standardScore += componentList[i].StandardScore;
            }
            ViewBag.StandardScore = standardScore;

            // 元件明细
            List<StorageComponentDetailModel> componentDetailList = bll.ComponentDetailList();

            // 仓储评估Model
            SupplierStorageAssessModel assessModel = new SupplierStorageAssessBLL().GetModelByID(model.StorageId);
             
            if (assessModel != null)
            {
                // 评估ID 
                ViewBag.StorageAssessId = assessModel.StorageAssessId;

                // 评估得分
                ViewBag.AssessScore = assessModel.AssessScore;

                // 评估结果
                ViewBag.AssessResult = assessModel.AssessResult;
                if (assessModel.AssessResult == 0)
                {
                    // 评估结果显示
                    ViewBag.AssessResultName = "不及格";
                }
                else
                {
                    // 评估结果显示
                    ViewBag.AssessResultName = "合格";
                }

                // 考察人 
                ViewBag.InspectUsers = assessModel.InspectUsers;

                // 考察日期
                ViewBag.InspectTime = assessModel.InspectTime;

                // 评估人
                ViewBag.AssessUsers = assessModel.AssessUsers;

                // 评估日期
                ViewBag.AssessTime = assessModel.AssessTime;

                // 审核人
                ViewBag.AuditingUsers = assessModel.AuditingUsers;

                // 审核日期
                ViewBag.AuditingTime = assessModel.AuditingTime;

                // 评估意见
                ViewBag.AssessRemark = assessModel.AssessRemark;

                // 备注
                ViewBag.Remark = assessModel.Remark;

                List<temfile> filelist = new SupplierStorageAssessAdjunctBLL().AdjunctListById(assessModel.StorageAssessId);
                ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);
            }
            else
            {
                // 评估ID 
                ViewBag.StorageAssessId = "";

                // 评估得分
                ViewBag.AssessScore = "";

                // 评估结果
                ViewBag.AssessResult = "";

                // 评估结果显示
                ViewBag.AssessResultName = "";

                // 考察人 
                ViewBag.InspectUsers = "";

                // 考察日期
                ViewBag.InspectTime = "";

                // 评估人
                ViewBag.AssessUsers = "";

                // 评估日期
                ViewBag.AssessTime = "";

                // 审核人
                ViewBag.AuditingUsers = "";

                // 审核日期
                ViewBag.AuditingTime = "";

                // 评估意见
                ViewBag.AssessRemark = "";

                // 备注
                ViewBag.Remark = "";
            }

            return View(new StorageAssess
            {
                ComponentList = componentList,
                ComponentDetailList = componentDetailList 
            });
        }

        #endregion

        #region 方法

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddSupplierStorage(SupplierStorageModel tModel)
        {
            // 状态 未提交
            tModel.State = 0;

            // 使用状态 未使用
            tModel.UseState = 0;

            // 供应商状态(系统字典表DictionaryNumber)
            tModel.StorageState = "F1";

            // 机构ID
            tModel.DepartmentId = Auxiliary.DepartmentId();

            // 用户ID
            tModel.CretaeUserId = Auxiliary.UserID();

            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 得分结果
            tModel.AssessResult = 0;

            // 新增(返回主键ID)
            int storageId = bll.AddSupplierStorage(tModel);

            // 若主键>O(新增成功)
            if (storageId > 0)
            {
                // 附件
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(tModel.SuppFileList))
                {
                    List<temfile> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.SuppFileList);
                    sabll.AddFilesForSupplier(fflist, tModel.SupplierId, storageId, 30, ref filenames);
                }
                tModel.SuppFileList = filenames;

                // 记录供应商日志
                Auxiliary.SupplierLog<SupplierStorageModel>(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 记录供应商日志
            Auxiliary.SupplierLog<SupplierStorageModel>(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        #region 供应商信息

        /// <summary>
        /// 分页列表 供应商信息
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public ActionResult SupplierList(int index, int size, string supplierName)
        {
            string where = "AND 1=1";

            int tCompanyID = Auxiliary.CompanyID();

            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            List<SupplierModel> list = bll.SupplierList(index, size, where, tCompanyID);

            return Json(list);
        }

        /// <summary>
        /// 分页总数 供应商信息
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns> 
        public int SupplierAmount(string supplierName)
        {
            string where = "AND 1=1";

            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName  like '%{0}%' ", supplierName.Trim());
            }

            int tCompanyID = Auxiliary.CompanyID();

            return bll.SupplierAmount(where, tCompanyID);
        }
        #endregion
        #endregion

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="storageNumber">仓库供应商编号</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="enterpriseType">企业性质</param>
        /// <param name="storageNature">库房性质</param>
        /// <param name="useState">状态</param>
        /// <returns>Json</returns>
        public ActionResult SupplierStorageList(int index, int size, string storageNumber, string supplierName, string enterpriseType, string storageNature, string storageState)
        {
            // 查询录入本机构且状态为非作废的仓储供应商的供应商信息
            string where = " ss.State != 20 AND ss.DepartmentId =" + Auxiliary.DepartmentId();

            // 仓库供应商编号
            if (!string.IsNullOrEmpty(storageNumber))
            {
                where += string.Format(" And StorageNumber like '%{0}%'", storageNumber.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 企业性质
            if (!string.IsNullOrEmpty(enterpriseType))
            {
                where += string.Format(" And EnterpriseType = {0}", enterpriseType.Trim());
            }

            // 库房性质
            if (!string.IsNullOrEmpty(storageNature))
            {
                where += string.Format(" And StorageNature = {0}", storageNature.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(storageState))
            {
                where += string.Format(" And StorageState = '{0}'", storageState.Trim());
            }

            // 仓储供应商List
            List<SupplierStorageModel> list = bll.SupplierStorageList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="storageNumber">仓库供应商编号</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="enterpriseType">企业性质</param>
        /// <param name="storageNature">库房性质</param>
        /// <param name="useState">状态</param>
        /// <returns>Json</returns>
        public int SupplierStorageCount(string storageNumber, string supplierName, string enterpriseType, string storageNature, string storageState)
        {
            // 查询录入本机构且状态为非作废的仓储供应商的供应商信息
            string where = " ss.State != 20 AND ss.DepartmentId =" + Auxiliary.DepartmentId();

            // 仓库供应商编号
            if (!string.IsNullOrEmpty(storageNumber))
            {
                where += string.Format(" And StorageNumber like '%{0}%'", storageNumber.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 企业性质
            if (!string.IsNullOrEmpty(enterpriseType))
            {
                where += string.Format(" And EnterpriseType = {0}", enterpriseType.Trim());
            }

            // 库房性质
            if (!string.IsNullOrEmpty(storageNature))
            {
                where += string.Format(" And StorageNature = {0}", storageNature.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(storageState))
            {
                where += string.Format(" And StorageState = '{0}'", storageState.Trim());
            }

            return bll.SupplierStorageAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditSupplierStorage(SupplierStorageModel tModel)
        {
            // 编辑之前Model
            SupplierStorageModel beforeModel = bll.GetModelByID(tModel.StorageId);

            // 编辑
            int row = bll.EditSupplierStorage(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(tModel.SuppFileList))
                {
                    // 附件
                    List<temfile> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.SuppFileList);
                    sabll.AddFilesForSupplier(fflist, tModel.SupplierId, tModel.StorageId, 30, ref filenames);
                }
                tModel.SuppFileList = filenames;

                // 供应商日志
                Auxiliary.SupplierLog<SupplierStorageModel>(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel);
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierLog<SupplierStorageModel>(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <param name="tSupplierid">供应商ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitSupplierStorage(int tId, int tSupplierid)
        {
            // 影响行数
            int row = 0;

            // 更新之前table
            SupplierStorageModel beforeModel = bll.GetModelByID(tId);

            // 更改提交状态 
            bll.ChangeState(tId, 1);

            // 更改仓储状态
            row = bll.ChangeStorageState(tId, "F2");

            // 若影响行数>O(更改状态)
            if (row > 0)
            {
                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "提交", Id = tId, State = 1, StorageNumber = beforeModel.StorageNumber });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Fail, new { Type = "提交", Id = tId, Code = beforeModel.StorageNumber });
            return Json(new { flag = "fail", content = "提交失败！" });
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
            SupplierStorageModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "作废", Id = tId, Name = beforeModel.SupplierName, Code = beforeModel.StorageNumber });
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "作废", Id = tId, Name = beforeModel.SupplierName, Code = beforeModel.StorageNumber });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Start)]
        public ActionResult StartState(int tId)
        {
            // 启用之前Model
            SupplierStorageModel beforeModel = bll.GetModelByID(tId);

            // 启用(更改状态)
            int row = bll.StartState(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                /// <param name="supplierId">供应商id</param>
                /// <param name="deparTmentId">所属机构</param>
                /// <param name="otherId">运输(仓储)供应商id 通过Types区别</param>
                /// <param name="types">类型：0-运输供应商;1-仓储供应商</param>
                /// <param name="beginTypes">周期开始类型：0：待运作->运作;1-停用->运作</param>
                //结束对应运输供应商的生命周期（SupplierCycl表【供应商周期】）
                scbll.AddBeginTypes(beforeModel.SupplierId, beforeModel.DepartmentId, beforeModel.StorageId, 1,1);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "启用", Id = tId, Name = beforeModel.SupplierName, Code = beforeModel.StorageNumber });
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "启用", Id = tId, Name = beforeModel.SupplierName, Code = beforeModel.StorageNumber });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Deactivate)]
        public ActionResult DeactivateState(int tId)
        {
            // 停用之前Model
            SupplierStorageModel beforeModel = bll.GetModelByID(tId);

            // 停用(更改状态)
            int row = bll.DeactivateState(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            { 
                /// <param name="supplierId">供应商id</param> 
                /// <param name="types">类型：0-运输供应商;1-仓储供应商</param>
                /// <param name="endTypes">周期结束类型：0：运作->停用;1：运作->拉黑;2:统一拉黑</param>
                /// <param name="otherId">运输(仓储)供应商id 通过Types区别</param>
                //结束对应运输供应商的生命周期（SupplierCycl表【供应商周期】）
                scbll.UdateEndTypes(beforeModel.SupplierId, 1, 0, beforeModel.StorageId);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "停用", Id = tId, Name = beforeModel.SupplierName, Code = beforeModel.StorageNumber });
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "停用", Id = tId, Name = beforeModel.SupplierName, Code = beforeModel.StorageNumber });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 拉黑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Blacklisted)]
        public ActionResult BlacklistedState(int tId)
        {
            // 拉黑之前Model
            SupplierStorageModel beforeModel = bll.GetModelByID(tId);

            // 拉黑(更改状态)
            int row = bll.BlacklistedState(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                /// <param name="supplierId">供应商id</param> 
                /// <param name="types">类型：0-运输供应商;1-仓储供应商</param>
                /// <param name="endTypes">周期结束类型：0：运作->停用;1：运作->拉黑;2:统一拉黑</param>
                /// <param name="otherId">运输(仓储)供应商id 通过Types区别</param>
                //结束对应运输供应商的生命周期（SupplierCycl表【供应商周期】）
                scbll.UdateEndTypes(beforeModel.SupplierId, 1, 1, beforeModel.StorageId);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "拉黑", Id = tId, Name = beforeModel.SupplierName, Code = beforeModel.StorageNumber });
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "拉黑", Id = tId, Name = beforeModel.SupplierName, Code = beforeModel.StorageNumber });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="storageNumber">仓库供应商编号</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="enterpriseType">企业性质</param>
        /// <param name="storageNature">库房性质</param>
        /// <param name="useState">状态</param>
        /// <returns>Json</returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string storageNumber, string supplierName, string enterpriseType, string storageNature, string storageState)
        {
            // 查询录入本机构且状态为非作废的仓储供应商的供应商信息
            string where = " ss.State != 20 AND ss.DepartmentId =" + Auxiliary.DepartmentId();

            // 仓库供应商编号
            if (!string.IsNullOrEmpty(storageNumber))
            {
                where += string.Format(" And StorageNumber like '%{0}%'", storageNumber.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 企业性质
            if (!string.IsNullOrEmpty(enterpriseType))
            {
                where += string.Format(" And EnterpriseType = {0}", enterpriseType.Trim());
            }

            // 库房性质
            if (!string.IsNullOrEmpty(storageNature))
            {
                where += string.Format(" And StorageNature = {0}", storageNature.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(storageState))
            {
                where += string.Format(" And StorageState = {0}", storageState.Trim());
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Export, ResultEnum.Sucess, new { Type = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }

        #region 仓储供应商评估

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddStorageAssess(SupplierStorageAssessModel tModel)
        {
            // 仓储评估编号
            tModel.StorageAssessNumber = Auxiliary.CurCompanyAutoNum("SAN");

            // 状态 有效状态
            tModel.State = 1;

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建账号ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 新增(返回主键ID)
            int storageAssessId = new SupplierStorageAssessBLL().AddStorageAssess(tModel);

            // 若主键>O(新增成功)
            if (storageAssessId > 0)
            {
                // 修改仓储供应商表得分结果及得分
                if (tModel.AssessResult == 0)
                {
                    bll.UnqualifiedResult(tModel.StorageId, tModel.AssessResult, tModel.AssessScore);
                }
                else
                {
                    bll.QualifiedResult(tModel.StorageId, tModel.AssessResult, tModel.AssessScore);
                }

                // 仓储供应商评估内容
                if (!string.IsNullOrEmpty(tModel.AssessContentList))
                {
                    // 新增仓储评估内容(元件明细ID:相符值:不适用值)
                    List<TempStorageAssessContentModel> assessContentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempStorageAssessContentModel>>(tModel.AssessContentList);

                    new SupplierStorageAssessContentBLL().AddAssessContentList(assessContentList, storageAssessId);
                }

                // 仓储供应商评估附件
                if (!string.IsNullOrEmpty(tModel.SuppFileList))
                {
                    // 新增
                    List<temfile> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.SuppFileList);

                    new SupplierStorageAssessAdjunctBLL().AddAdjunctList(adjunctList, storageAssessId);
                }

                // 记录供应商日志
                Auxiliary.SupplierLog<SupplierStorageAssessModel>(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 记录供应商日志
            Auxiliary.SupplierLog<SupplierStorageAssessModel>(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditStorageAssess(SupplierStorageAssessModel tModel)
        {
            // 编辑之前Model
            SupplierStorageAssessModel beforeModel = new SupplierStorageAssessBLL().GetModelByID(tModel.StorageId);

            // 编辑
            int row = new SupplierStorageAssessBLL().EditStorageAssess(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 修改仓储供应商表得分结果及得分
                if (tModel.AssessResult == 0)
                {
                    bll.UnqualifiedResult(tModel.StorageId, tModel.AssessResult, tModel.AssessScore);
                }
                else
                {
                    bll.QualifiedResult(tModel.StorageId, tModel.AssessResult, tModel.AssessScore);
                }

                // 仓储供应商评估内容
                if (!string.IsNullOrEmpty(tModel.AssessContentList))
                {
                    // 新增仓储评估内容(元件明细ID:相符值:不适用值)
                    List<TempStorageAssessContentModel> assessContentList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempStorageAssessContentModel>>(tModel.AssessContentList);

                    new SupplierStorageAssessContentBLL().AddAssessContentList(assessContentList, tModel.StorageAssessId);
                }

                // 仓储供应商评估附件
                if (!string.IsNullOrEmpty(tModel.SuppFileList))
                {
                    // 新增
                    List<temfile> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.SuppFileList);

                    new SupplierStorageAssessAdjunctBLL().AddAdjunctList(adjunctList, tModel.StorageAssessId);
                }

                // 供应商日志
                Auxiliary.SupplierLog<SupplierStorageAssessModel>(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel);
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierLog<SupplierStorageAssessModel>(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 获取评估内容List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SupplierStorageAssessContentList(int storageAssessId)
        {
            return Json(new SupplierStorageAssessContentBLL().SupplierStorageAssessContentList(storageAssessId));
        }

        #endregion

        #endregion

    }
    #region 评估辅助类

    /// <summary>
    /// 评分辅助类
    /// </summary>
    public class StorageAssess
    {
        // 元件
        public List<StorageComponentModel> ComponentList { get; set; }

        // 元件明细
        public List<StorageComponentDetailModel> ComponentDetailList { get; set; }
         
        // 元件名词 编辑
        public List<StorageComponentDetailModel> ComponentDetailEditList { get; set; }

    }
    #endregion
}
