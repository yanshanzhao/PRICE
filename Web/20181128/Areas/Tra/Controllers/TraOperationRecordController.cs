//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-01    1.0        zbb        新建               
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.Model.Tra;
using SRM.Web.Controllers;
using SRM.BLL.Tra;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：TraOperationRecordController
 * 功能描述：运作执行记录 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraOperationRecordController : Controller
    {
        //
        // GET: /Tra/TraOperationRecord/ 

        TraOperationRecordBLL bll = new TraOperationRecordBLL();
        TraOperationClaimBLL Cbll = new TraOperationClaimBLL();

        TraOperationClaimBLL Abll = new TraOperationClaimBLL();
        TraOperationeRecordAdjunctBLL abll = new TraOperationeRecordAdjunctBLL();

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
            TraOperationRecordModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfileOperationeRecord> filelist = abll.SuppFileList(model.OperationRecordId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            TraOperationRecordModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfileOperationeRecord> filelist = abll.SuppFileList(model.OperationRecordId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 运作要求信息
        /// </summary>
        public ActionResult TraOperationRecordDetail(string url)
        {
            ViewBag.url = url;
            return View();
        }

        public ActionResult TraOperationClaimDetail(string url, string ids, string idss, string type)
        {
            ViewBag.url = url;
            ViewBag.type = type;
            ViewBag.ids = ids;
            ViewBag.idss = idss;
            return View();
        }

        #endregion

        #region 方法 

        #region 新增方法逻辑

        /// <summary>
        /// 新增方法逻辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddTraOperationRecord(TraOperationRecordModel model)
        {

            model.RecordState = 1; //状态: 1 - 有效; 0 - 无效

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            //bll.ChangeState(model.OperationSuppId);

           int rows= bll.ChangeState(model.OperationRecordIdList);

            int OperationRecord = bll.AddTraOperationRecord(model);

            model.OperationRecordId = OperationRecord;

            //bll.AddTraOperationRecordSupp(model);

            if (OperationRecord > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.TraOperationRecordFileList))
                {
                    List<temfileOperationeRecord> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfileOperationeRecord>>(model.TraOperationRecordFileList);
                    abll.AddFilesForSupplier(fflist, OperationRecord, ref filenames);
                }
                model.TraOperationRecordFileList = filenames;

                if (!string.IsNullOrEmpty(model.OperationRecordIdList))
                {
                    List<string> operationrecordidlist = new List<string>(model.OperationRecordIdList.Split(','));
                    bll.AddTraOperationRecordSupp(operationrecordidlist, OperationRecord);

                }
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        /// <summary>
        /// 明细新增 运作分配供应商表
        /// </summary>
        /// <param name="supperMatchingIds">供应商匹配id</param>
        /// <param name="operationClaimId">运作id</param>
        /// <returns></returns>
        public ActionResult AddTraOperationRecordSupp(string supperMatchingIds, int operationClaimId)
        {
            // 影响行数
            int row = 0;

            if (!string.IsNullOrEmpty(supperMatchingIds))
            {
                List<string> supperMatchingIdList = new List<string>(supperMatchingIds.Split(','));
                row = bll.AddTraOperationRecordSupp(supperMatchingIdList, operationClaimId);
            }

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
            return Json(new { flag = "fail" });
        }

        #region 数据集

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">当前页面</param>
        /// <param name="size">页面索引</param>
        /// <param name="datumType">运作类型</param>
        /// <param name="claimType">要求类型</param>
        /// <param name="recordState">记录状态</param>
        /// <param name="operationNumber">运作编号</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public ActionResult TraOperationRecordList(int index, int size, string theme, string destination, string recordState, string operationNumber, string createTime)
        {
            string where = string.Format(" TOR.RecordState = 1 and TOR.CreateUserId={0}", Auxiliary.UserID());

            // 运作类型
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And TOC.Theme like '%{0}%'", theme.Trim());
            }

            // 要求类型
            if (!string.IsNullOrEmpty(destination))
            {
                where += string.Format(" And TOC.Destination like '%{0}%'", destination.Trim());
            }

            // 记录状态
            if (!string.IsNullOrEmpty(recordState))
            {
                where += string.Format(" And TOR.RecordState = {0}", recordState.Trim());
            }

            // 运作编号
            if (!string.IsNullOrEmpty(operationNumber))
            {
                where += string.Format(" And TOC.OperationNumber = {0}", operationNumber.Trim());
            }

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TOR.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            List<TraOperationRecordModel> list = bll.TraOperationRecordList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="datumType">运作类型</param>
        /// <param name="claimType">要求类型</param>
        /// <param name="recordState">记录状态</param>
        /// <param name="operationNumber">运作编号</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int TraOperationRecordCount(string theme, string destination, string recordState, string operationNumber, string createTime)
        {
            string where = string.Format(" TOR.RecordState = 1 and TOR.CreateUserId={0}", Auxiliary.UserID());

            // 运作类型
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And TOC.Theme like '%{0}%'", theme.Trim());
            }

            // 要求类型
            if (!string.IsNullOrEmpty(destination))
            {
                where += string.Format(" And TOC.Destination like '%{0}%'", destination.Trim());
            }

            // 记录状态
            if (!string.IsNullOrEmpty(recordState))
            {
                where += string.Format(" And TOR.RecordState = {0}", recordState.Trim());
            }

            // 运作编号
            if (!string.IsNullOrEmpty(operationNumber))
            {
                where += string.Format(" And TOC.OperationNumber = {0}", operationNumber.Trim());
            }

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TOR.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            return bll.TraOperationRecordCount(where);
        }
        #endregion

        #region 编辑按钮逻辑

        /// <summary>
        /// 编辑按钮逻辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraOperationRecord(TraOperationRecordModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            TraOperationRecordModel beforeModel = bll.GetModelByID(model.OperationRecordId);

            int OperationRecord = bll.EditTraOperationRecord(model);

            if (OperationRecord > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.TraOperationRecordFileList))
                {
                    List<temfileOperationeRecord> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfileOperationeRecord>>(model.TraOperationRecordFileList);
                    abll.AddFilesForSupplier(fflist, OperationRecord, ref filenames);
                }
                model.TraOperationRecordFileList = filenames;

                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 作废按钮逻辑

        /// <summary>
        /// 作废按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            TraOperationRecordModel beforeModel = bll.GetModelByID(tId);

            int delUserId = Auxiliary.UserID();//作废负责人

            bll.ChangeStates(beforeModel.OperationSuppId);

            int row = bll.InvalidState(tId, delUserId);
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion

        #region 导出按钮逻辑

        /// <summary>
        /// 导出按钮逻辑
        /// </summary>
        /// <param name="datumType">运作类型</param>
        /// <param name="claimType">要求类型</param>
        /// <param name="recordState">记录状态</param>
        /// <param name="operationNumber">运作编号</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string theme, string destination, string recordState, string operationNumber, string createTime)
        {
            string where = string.Format(" TOR.RecordState = 1 and TOR.CreateUserId={0}", Auxiliary.UserID());

            // 运作类型
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And TOC.Theme like '%{0}%'", theme.Trim());
            }

            // 要求类型
            if (!string.IsNullOrEmpty(destination))
            {
                where += string.Format(" And TOC.Destination like '%{0}%'", destination.Trim());
            }

            // 记录状态
            if (!string.IsNullOrEmpty(recordState))
            {
                where += string.Format(" And TOR.RecordState = {0}", recordState.Trim());
            }

            // 运作编号
            if (!string.IsNullOrEmpty(operationNumber))
            {
                where += string.Format(" And TOC.OperationNumber = {0}", operationNumber.Trim());
            }

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,TOR.CreateTime,120) like '%{0}%'", createTime.Trim());
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

        #region 运作记录列表数据

        /// <summary>
        /// 运作记录列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="operationNumber">运作编号</param>
        /// <returns></returns>
        public ActionResult OperationRecordList(int index, int size, string operationNumber)
        {
            string where = string.Format("  TOC.CreateDepartmentId = {0}", Auxiliary.DepartmentId());

            //运作编号
            if (!string.IsNullOrEmpty(operationNumber))
            {
                where += string.Format(" And TOC.OperationNumber like '%{0}%'", operationNumber.Trim());
            }

            List<TraOperationRecordModel> list = bll.OperationRecordList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 显示行：运作记录数据

        /// <summary>
        /// 显示行：运作记录数据
        /// </summary> 
        /// <param name="operationNumber">运作编号</param>
        /// <returns></returns>
        public int OperationRecordCount(string operationNumber)
        {
            //string where = string.Format("   State in (select State from  dbo.TraOperationSupp where State=1)   And TOC.CreateDepartmentId = {0}",
            string where = string.Format("  TOC.CreateDepartmentId = {0}", Auxiliary.DepartmentId());

            //运作编号
            if (!string.IsNullOrEmpty(operationNumber))
            {
                where += string.Format(" And TOC.OperationNumber like '%{0}%'", operationNumber.Trim());
            }

            return bll.OperationRecordCount(where);
        }
        #endregion

        #region 运作记录列表数据

        /// <summary>
        /// 运作记录列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="operationNumber">运作编号</param>
        /// <returns></returns>
        public ActionResult OperationClaimList(int index, int size, string checkTraChooseId)
        {
            string where = string.Format(" TOS.OperationSuppId in ({0})", checkTraChooseId);

            List<TraOperationRecordModel> list = bll.OperationClaimList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
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
        public ActionResult OperationClaimballList(int index, int size, string supplierName, string type, string ids, string idss)
        {
            string where = string.Format("TOS.State!=0 and  ST.TransportState!='F6'   and TOS.OperationClaimId={0} and ST.DepartmentId={1}", idss, Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And TOS.OperationSuppId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And TOS.OperationSuppId NOT IN (SELECT OperationSuppId FROM TraOperationRecordSupp WHERE OperationRecordId ={0})", ids.Trim());
                }
            }
            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }
            List<TraOperationRecordModel> list = bll.OperationClaimballList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
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
        public int OperationClaimballCount(string supplierName, string type, string ids, string idss)
        {

            string where = string.Format(" TOS.State!=0 and  ST.TransportState!='F6'   and TOS.OperationClaimId={0} and ST.DepartmentId={1}", idss, Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And TOS.OperationSuppId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And TOS.OperationSuppId NOT IN (SELECT OperationSuppId FROM TraOperationRecordSupp WHERE OperationRecordId ={0})", ids.Trim());
                }
            }
            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            return bll.OperationClaimballCount(where);
        }
        #endregion

        #region 显示行：运作记录数据

        /// <summary>
        /// 显示行：运作记录数据
        /// </summary> 
        /// <param name="operationNumber">运作编号</param>
        /// <returns></returns>
        public int OperationClaimCount(string checkTraChooseId)
        {

            string where = string.Format(" TOS.OperationSuppId in ({0})", checkTraChooseId);

            return bll.OperationClaimCount(where);
        }
        #endregion


        /// <summary>
        /// 分配明细 数据集
        /// </summary>
        /// <param name="index">当前页面</param>
        /// <param name="size">页面索引</param>
        /// <param name="operationClaimId">运作id</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult OperationClaimAllotList(int index, int size, int operationClaimId)
        {

            string where = string.Format("  TOS.State != 0  And TOS.OperationClaimId = {0}", operationClaimId);

            List<TraOperationClaimModel> list = Abll.OperationClaimAllotList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #region 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   

        /// <summary>
        /// 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   
        /// </summary> 
        /// <param name="OperationRecordId"></param>
        /// <returns></returns>
        public ActionResult TraOperationRecordeditList(string OperationRecordId)
        {

            string where = string.Format(" ORS.OperationRecordId in ({0})", OperationRecordId);

            List<TraOperationRecordModel> list = bll.TraOperationRecordeditList(where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 编辑按钮时：显示下方的仓储选择明细数据记录count 

        /// <summary>
        /// 编辑按钮时：显示下方的仓储选择明细数据记录count
        /// </summary>
        /// <param name="OperationRecordId"></param> 
        /// <returns></returns>
        public int TraOperationRecordeditAmount(string OperationRecordId)
        {
            string where = string.Format(" ORS.OperationRecordId in ({0})", OperationRecordId);

            return bll.TraOperationRecordeditAmount(where);
        }
        #endregion
        #endregion
    }
}
