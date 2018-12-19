//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-26    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using System.Linq;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.BLL.Storage;
using SRM.Model.Storage;
using SRM.Model.Supplier;
using SRM.BLL.Supplier;
#endregion
/*********************************
 * 类名：StorageSupplierStartController
 * 功能描述：仓储选择申请表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Storage.Controllers
{
    public class StorageSupplierStartController : Controller
    {
        //
        // GET: /Storage/StorageSupplierStart/ 

        //仓库启用登记
        StorageSupplierStartBLL bll = new StorageSupplierStartBLL();

        //仓库选择
        StorageChooseBLL sbll = new StorageChooseBLL();

        //仓储供应商
        SupplierStorageBLL ssbll = new SupplierStorageBLL();

        //仓库启用登记附件
        StorageSupplierStartAdjunctBLL ssll = new StorageSupplierStartAdjunctBLL();
        
        //仓库启用登记附件
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
            StorageSupplierStartModel model = bll.GetModelByID(tId);
            //获取附件信息
            List<temfilees> filelist = ssll.SuppFileList(model.StorageStartId);
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
            StorageSupplierStartModel model = bll.GetModelByID(tId);
            //获取附件信息
            List<temfilees> filelist = ssll.SuppFileList(model.StorageStartId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 供应商明细
        /// </summary>
        public ActionResult SupplierStartDetail(string url)
        {
            ViewBag.url = url;
            return View();
        }
        #endregion

        #region 方法 

        #region 新增方法

        /// <summary>
        /// 新增 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddStorageSupplierStart(StorageSupplierStartModel model)
        {
            model.State = 0;// 状态 未提交 

            model.CompanyId = Auxiliary.CompanyID();//公司id

            model.CreateDepartmentId = Auxiliary.DepartmentId();//创建机构id

            model.CreateUserId = Auxiliary.UserID();//创建负责人id

            int intStorageSupplierId = bll.AddStorageSupplierStart(model);

            if (intStorageSupplierId > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppFileListes))
                {
                    List<temfilees> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfilees>>(model.SuppFileListes);
                    ssll.AddFilesForSupplier(fflist, intStorageSupplierId, ref filenames);
                }
                model.SuppFileListes = filenames;
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "保存成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 仓储供应商启用登记 数据集

        /// <summary>
        /// 仓储供应商启用登记 数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="SupplierName"></param>
        /// <param name="SignContract"></param>
        /// <param name="IsCultivate"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public ActionResult StorageSupplierStartList(int index, int size, string SupplierName, string SignContract, string IsCultivate, string State)
        {
            string where = " SSS.State != 20 AND SSS.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(SupplierName))
            {
                where += string.Format(" And s.SupplierName  like '%{0}%' ", SupplierName.Trim());
            }

            // 签订合同
            if (!string.IsNullOrEmpty(SignContract))
            {
                where += string.Format(" And SSS.SignContract = {0}", SignContract.Trim());
            }

            // 参与培训
            if (!string.IsNullOrEmpty(IsCultivate))
            {
                where += string.Format(" And SSS.IsCultivate = {0}", IsCultivate.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And SSS.State = {0}", State.Trim());
            }

            List<StorageSupplierStartModel> list = bll.StorageSupplierStartList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region 仓储供应商启用登记 数据记录数

        /// <summary>
        /// 仓储供应商启用登记 数据记录数
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="SignContract"></param>
        /// <param name="IsCultivate"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public int StorageSupplierStartCount(string SupplierName, string SignContract, string IsCultivate, string State)
        {
            string where = " SSS.State != 20 AND SSS.CreateDepartmentId =" + Auxiliary.DepartmentId();
            // 供应商名称
            if (!string.IsNullOrEmpty(SupplierName))
            {
                where += string.Format(" And s.SupplierName  like '%{0}%' ", SupplierName.Trim());
            }

            // 签订合同
            if (!string.IsNullOrEmpty(SignContract))
            {
                where += string.Format(" And SSS.SignContract = {0}", SignContract.Trim());
            }

            // 参与培训
            if (!string.IsNullOrEmpty(IsCultivate))
            {
                where += string.Format(" And SSS.IsCultivate = {0}", IsCultivate.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And SSS.State = {0}", State.Trim());
            }
            return bll.StorageSupplierStartCount(where);
        }

        #endregion

        #region 编辑按钮

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditStorageSupplierStart(StorageSupplierStartModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            StorageSupplierStartModel beforeModel = bll.GetModelByID(model.StorageStartId);

            int result = bll.EditStorageSupplierStart(model);
            if (result > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppFileListes))
                {
                    List<temfilees> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfilees>>(model.SuppFileListes);
                    ssll.AddFilesForSupplier(fflist, model.StorageStartId, ref filenames);
                }
                model.SuppFileListes = filenames;
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success", content = "编辑成功！" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 启用按钮

        /// <summary>
        /// 启用仓库启用登记
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Start)]
        public ActionResult StartStorageSupplierStart(int tId)
        {

            // 更新之前table
            StorageSupplierStartModel beforeModel = bll.GetModelByID(tId);

            int StartUserId = Auxiliary.UserID();
             
            int row = bll.StartStorageSupplierStart(tId, StartUserId);
             
            if (row > 0)
            {
                //更改仓储供应商的状态。从待运作状态变为运作状态
                ssbll.ChangeStorageState(beforeModel.AffirmStorageId, "F4");

                //将启用中的信息复制SupplierFinance（供应商财务信息）表中 
                bll.AddStorageSupplierStart(beforeModel);

                /// <param name="supplierId">供应商id</param>
                /// <param name="deparTmentId">所属机构</param>
                /// <param name="otherId">运输(仓储)供应商id 通过Types区别</param>
                /// <param name="types">类型：0-运输供应商;1-仓储供应商</param>
                /// <param name="beginTypes">周期开始类型：0：待运作->运作;1-停用->运作</param>
                scbll.AddBeginTypes(beforeModel.SupplierId,beforeModel.CreateDepartmentId,beforeModel.StorageId, 1,1);

                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "启用成功！" });
            }

            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 作废按钮

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            StorageSupplierStartModel beforeModel = bll.GetModelByID(tId);

            int delUserId = Auxiliary.UserID();

            //更改StorageChoose（仓储供应商选择）表的状态,更新为审核成功
            sbll.ChangeState(beforeModel.StorageChooseId, 5);

            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);

            int row = bll.InvalidState(tId, delUserId);
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 导出按钮

        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="SupplierName"></param>
        /// <param name="SignContract"></param>
        /// <param name="IsCultivate"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string SupplierName, string SignContract, string IsCultivate, string State)
        {
            string where = " SSS.State != 20 AND SSS.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 供应商名称
            if (!string.IsNullOrEmpty(SupplierName))
            {
                where += string.Format(" And s.SupplierName  like '%{0}%' ", SupplierName.Trim());
            }

            // 签订合同
            if (!string.IsNullOrEmpty(SignContract))
            {
                where += string.Format(" And SSS.SignContract = {0}", SignContract.Trim());
            }

            // 参与培训
            if (!string.IsNullOrEmpty(IsCultivate))
            {
                where += string.Format(" And SSS.IsCultivate = {0}", IsCultivate.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And SSS.State = {0}", State.Trim());
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

        #region 供应商启用明细 数据集

        /// <summary>
        /// 供应商启用明细 数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="ApplyTime"></param>
        /// <returns></returns>
        public ActionResult SupplierStartList(int index, int size, string ApplyTime)
        {
            string where = " sc.UseState=5 AND sc.CreateDepartmentId =" + Auxiliary.DepartmentId();  

            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,sc.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            List<StorageSupplierStartModel> list = bll.SupplierStartList(index, size, where);

            return Json(list);
        }
        #endregion

        #region 供应商启用明细 数据记录数

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="ApplyTime">申请时间</param>
        /// <returns></returns>
        public int SupplierStartCount(string ApplyTime)
        {
            string where = " sc.UseState=5 AND sc.CreateDepartmentId =" + Auxiliary.DepartmentId();

            if (!string.IsNullOrEmpty(ApplyTime))
            {
                where += string.Format(" And convert(varchar,sc.ApplyTime,120) like '%{0}%'", ApplyTime.Trim());
            }

            return bll.SupplierStartCount(where);
        }
        #endregion

        #endregion
    }
}
