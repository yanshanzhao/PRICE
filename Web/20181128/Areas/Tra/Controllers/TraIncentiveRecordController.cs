//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-25    1.0        FJK        新建                    
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
 * 类名：TraIncentiveRecordController
 * 功能描述：激励记录表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraIncentiveRecordController : Controller
    {
        //
        // GET: /Tra/TraIncentiveRecord/
         
        // 激励记录BLL
        TraIncentiveRecordBLL bll = new TraIncentiveRecordBLL();

        // 附件BLL
        TraIncentiveRecordAdjunctBLL tirbll = new TraIncentiveRecordAdjunctBLL();

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
            // ViewBag.StorageNumber = Auxiliary.CurCompanyAutoNum("RAN");
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
           TraIncentiveRecordModel model = bll.GetModelByID(tId);

            // 附件
            List<temfile> fileList = tirbll.AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList); 
            List<string> oldlist = fileList.Select(p => p.filename + p.ext).ToList<string>();
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
            TraIncentiveRecordModel model = bll.GetModelByID(tId);

            // 附件
            List<temfile> fileList = tirbll.AdjunctListById(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);

            List<string> oldlist = fileList.Select(p => p.filename + p.ext).ToList<string>(); 
            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 运输供应商
        /// </summary>
        /// <param name="url">iframe地址</param>
        /// <returns></returns>
        public ActionResult TransportSupplier(string url)
        {
            ViewBag.url = url;
            return View();
        }

        #endregion

        #region 方法

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddIncentiveRecord(TraIncentiveRecordModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认初始状态0
            tModel.IncentiveState =0;

            // 新增(返回主键ID)
            int incentiveId = bll.AddIncentiveRecord(tModel);

            // 若主键>O(新增成功)
            if (incentiveId > 0)
            { 
                // 附件
                if (!string.IsNullOrEmpty(tModel.SuppFileList))
                {
                    List<temfile> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.SuppFileList);
                    tirbll.AddFiles(fileList, incentiveId);
                }  

                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" }); 
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel); 
            return Json(new { flag = "fail" });
        }

        #region 运输供应商信息

        /// <summary>
        /// 分页列表 运输供应商信息
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public ActionResult TransportSupplierList(int index, int size, string supplierName)
        {
            // 本机构
            string where = " AND DepartmentId="+Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" AND SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 运输供应商信息List
            List<TraIncentiveRecordModel> list = bll.TransportSupplierList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 分页总数 运输供应商信息
        /// </summary> 
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public int TransportSupplierAmount(string supplierName)
        {
            // 本机构
            string where = " AND DepartmentId=" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" AND SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            return bll.TransportSupplierAmount(where);
        }
        #endregion
        #endregion

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierNumber">供应商编号</param>
        /// <param name="supplierType">供应商类型</param>
        /// <returns>Json</returns>
        public ActionResult IncentiveRecordList(int index, int size,string supplierName, string supplierNumber, string supplierType)
        {
            // 查询本机构录入的有效的激励措施信息
            string where = " IncentiveState != 10 AND DepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 供应商编号
            if (!string.IsNullOrEmpty(supplierNumber))
            {
                where += string.Format(" And supplierNumber like '%{0}%'", supplierNumber.Trim());
            }

            // 供应商类型
            if (!string.IsNullOrEmpty(supplierType))
            {
                where += string.Format(" And DictionaryId = {0}", supplierType.Trim());
            }

            // 激励记录List
            List<TraIncentiveRecordModel> list = bll.IncentiveRecordList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss"; 
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierNumber">供应商编号</param>
        /// <param name="supplierType">供应商类型</param>
        /// <returns></returns>
        public int IncentiveRecordCount(string supplierName, string supplierNumber, string supplierType)
        {
            // 查询本机构录入的有效的激励措施信息
            string where = " IncentiveState != 10 AND DepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 供应商编号
            if (!string.IsNullOrEmpty(supplierNumber))
            {
                where += string.Format(" And supplierNumber like '%{0}%'", supplierNumber.Trim());
            }

            // 供应商类型
            if (!string.IsNullOrEmpty(supplierType))
            {
                where += string.Format(" And DictionaryId = {0}", supplierType.Trim());
            }

            return bll.IncentiveRecordAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditIncentiveRecord(TraIncentiveRecordModel tModel)
        {
            // 编辑之前Model
            TraIncentiveRecordModel beforeModel = bll.GetModelByID(tModel.IncentiveId);

            // 编辑
            int row = bll.EditIncentiveRecord(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
            { 
                // 附件
                if (!string.IsNullOrEmpty(tModel.SuppFileList))
                {
                    List<temfile> fileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(tModel.SuppFileList); 
                    tirbll.AddFiles(fileList, tModel.IncentiveId);
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
                return Json(new { flag = "success", content= "提交成功" });
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
            TraIncentiveRecordModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

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
        /// <param name="supplierName">供应商名称</param>
        /// <param name="supplierNumber">供应商编号</param>
        /// <param name="supplierType">供应商类型</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string supplierNumber, string supplierType)
        {
            // 查询本机构录入的有效的激励措施信息
            string where = " IncentiveState != 10 AND DepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 供应商编号
            if (!string.IsNullOrEmpty(supplierNumber))
            {
                where += string.Format(" And supplierNumber like '%{0}%'", supplierNumber.Trim());
            }

            // 供应商类型
            if (!string.IsNullOrEmpty(supplierType))
            { 
                where += string.Format(" And DictionaryId = {0}", supplierType.Trim());
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出",UserId = Auxiliary.UserID(),ExportTime = System.DateTime.Now
            }); 
            return Json(new { flag = "success", guid = url });
        }

        #endregion 
    }
}
