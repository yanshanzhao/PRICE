//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-04    1.0        zbb        新建               
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
 * 类名：TraPatrolController
 * 功能描述：过程巡查维护 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraPatrolController : Controller
    {
        //
        // GET: /Tra/TraPatrol/

        //过程巡查维护
        TraPatrolBLL bll = new TraPatrolBLL();

        //过程巡查记录附件
        TraPatrolAdjunctBLL abll = new TraPatrolAdjunctBLL();

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
            TraPatrolModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfilePatrol> filelist = abll.SuppFileList(model.PatrolId);
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
            TraPatrolModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfilePatrol> filelist = abll.SuppFileList(model.PatrolId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 分配
        /// </summary> 
        [Operate(Name = OperateEnum.Allot)]
        public ActionResult Allot(int tId)
        {
            // 获取数据
            TraPatrolModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfilePatrol> filelist = abll.SuppFileList(model.PatrolId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);
            return View(model);
        }

        /// <summary>
        /// 选择供应商规模信息
        /// </summary>
        public ActionResult AddDetail(string url, int tId)
        {
            ViewBag.url = url;
            ViewBag.PatrolId = tId;
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult EditTraPatrolAllot(string url, int tId)
        {
            ViewBag.url = url;

            // 获取数据
            TraPatrolModel model = bll.GetEditTraPatrolAllotByID(tId);

            return View(model);
        }

        #endregion

        #region 方法 

        #region 新增过程巡查维护

        /// <summary>
        /// 新增过程巡查维护
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddTraPatrol(TraPatrolModel model)
        {

            model.PatrolState = 1;// 状态：0-作废;1-有效;10-分配

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.PatrolNumber = Auxiliary.CurCompanyAutoNum("TPN");//巡查编号

            int TraPatrol = bll.AddTraPatrol(model);

            if (TraPatrol > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.TraPatrolFileList))
                {
                    List<temfilePatrol> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfilePatrol>>(model.TraPatrolFileList);
                    abll.AddFilesForSupplier(fflist, TraPatrol, ref filenames);
                }
                model.TraPatrolFileList = filenames;

                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 数据集 过程巡查维护

        /// <summary>
        /// 数据集 过程巡查维护
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="patrolType">巡查类型</param>
        /// <param name="patrolState">巡查状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public ActionResult TraPatrolList(int index, int size, string patrolType, string patrolState, string createTime)
        { 
            string where = " PatrolState != 0 and CreateDepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + "))";

            // 巡查类型
            if (!string.IsNullOrEmpty(patrolType))
            {
                where += string.Format(" And PatrolType = '{0}'", patrolType.Trim());
            }

            // 巡查状态
            if (!string.IsNullOrEmpty(patrolState))
            {
                where += string.Format(" And PatrolState = {0}", patrolState.Trim());
            }

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar, CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            List<TraPatrolModel> list = bll.TraPatrolList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 过程巡查维护

        /// <summary>
        /// 数据记录数 过程巡查维护
        /// </summary>
        /// <param name="patrolType">巡查类型</param>
        /// <param name="patrolState">巡查状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int TraPatrolCount(string patrolType, string patrolState, string createTime)
        {
            string where = " PatrolState != 0 and CreateDepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + "))";

            // 巡查类型
            if (!string.IsNullOrEmpty(patrolType))
            {
                where += string.Format(" And PatrolType = '{0}'", patrolType.Trim());
            }

            // 巡查状态
            if (!string.IsNullOrEmpty(patrolState))
            {
                where += string.Format(" And PatrolState = {0}", patrolState.Trim());
            }

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar, CreateTime,120) like '%{0}%'", createTime.Trim());
            }
            return bll.TraPatrolCount(where);
        }
        #endregion

        #region 编辑过程巡查维护

        /// <summary>
        /// 编辑过程巡查维护
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraPatrol(TraPatrolModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            TraPatrolModel beforeModel = bll.GetModelByID(model.PatrolId);

            //修改过程巡查维护
            int result = bll.EditTraPatrol(model);

            if (result > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.TraPatrolFileList))
                {
                    List<temfilePatrol> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfilePatrol>>(model.TraPatrolFileList);
                    abll.AddFilesForSupplier(fflist, model.PatrolId, ref filenames);
                }
                model.TraPatrolFileList = filenames;

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
            TraPatrolModel beforeModel = bll.GetModelByID(tId);

            int delUserId = Auxiliary.UserID();//作废负责人id

            //将过程巡查维护状态更新为作废
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
        /// <param name="patrolType">巡查类型</param>
        /// <param name="patrolState">巡查状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string patrolType, string patrolState, string createTime)
        {
            string where = " PatrolState != 0 and CreateDepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + "))";

            // 巡查类型
            if (!string.IsNullOrEmpty(patrolType))
            {
                where += string.Format(" And PatrolType = '{0}'", patrolType.Trim());
            }

            // 巡查状态
            if (!string.IsNullOrEmpty(patrolState))
            {
                where += string.Format(" And PatrolState = {0}", patrolState.Trim());
            }

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar, CreateTime,120) like '%{0}%'", createTime.Trim());
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

        #endregion

        #region 方法 分配

        #region 数据集

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">当前页面</param>
        /// <param name="size">页面索引</param>
        /// <param name="patrolId">巡查维护id</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult TraPatrolAllotList(int index, int size, int patrolId, string supplierName, string state)
        {

            string where = string.Format("  TPA.State != 0  And TPA.PatrolId = {0}", patrolId);

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TPA.State = {0}", state.Trim());
            }

            List<TraPatrolModel> list = bll.TraPatrolAllotList(index, size, where);

            if (list.Count == 0)
            {
                bll.ChangePatrolStates(patrolId);
            }

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region  数据记录数

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="patrolId">巡查维护id</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public int TraPatrolAllotCount(int patrolId, string supplierName, string state)
        {
            string where = string.Format("  TPA.State != 0  And TPA.PatrolId = {0}", patrolId);

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TPA.State = {0}", state.Trim());
            }

            return bll.TraPatrolAllotCount(where);
        }
        #endregion

        #region 巡查过程分配表 数据集

        /// <summary>
        /// 巡查过程分配表 数据集
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="patrolId">过程巡查维护id</param>
        /// <returns></returns>
        public ActionResult TraPatrolDetailList(string supplierName, string patrolId)
        { 
            //string where = string.Format("  (TSM.State = 1 OR TSM.State = 10) And TSM.CompanyId = {0}", Auxiliary.CompanyID());
            string where = string.Format("  ST.State = 1 AND ST.TransportState!='F6' And ST.DepartmentId = {0}", Auxiliary.DepartmentId());

            //过程巡查维护id
            if (!string.IsNullOrEmpty(patrolId))
            {
                //where += string.Format(" And TSM.SupperMatchingId NOT IN (SELECT SupperMatchingId FROM dbo.TraPatrolAllot WHERE PatrolId ={0} AND State =1)", patrolId.Trim());
                where += string.Format("  And ST.TransportId NOT IN (SELECT TransportId FROM dbo.TraOperationSupp WHERE OperationClaimId ={0} AND State =1)", patrolId.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            return Json(bll.TraPatrolDetailList(where));
        }
        #endregion

        #region 巡查过程分配表 数据记录数

        /// <summary>
        /// 巡查过程分配表 数据记录数
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="patrolId">过程巡查维护id</param>
        /// <returns></returns>
        public int TraPatrolDetailAmount(string supplierName, string patrolId)
        {
            //string where = string.Format("  (TSM.State = 1 OR TSM.State = 10) And TSM.CompanyId = {0}", Auxiliary.CompanyID());
            //string where = string.Format("  ST.State = 1 AND ST.TransportState!='F6' And ST.CompanyId = {0}", Auxiliary.CompanyID());
            string where = string.Format("  ST.State = 1 AND ST.TransportState!='F6' And ST.DepartmentId = {0}", Auxiliary.DepartmentId());

            //过程巡查维护id
            if (!string.IsNullOrEmpty(patrolId))
            {
                //where += string.Format(" And TSM.SupperMatchingId NOT IN (SELECT SupperMatchingId FROM dbo.TraPatrolAllot WHERE PatrolId ={0} AND State =1)", patrolId.Trim());

                where += string.Format("  And ST.TransportId NOT IN (SELECT TransportId FROM dbo.TraOperationSupp WHERE OperationClaimId ={0} AND State =1)", patrolId.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            return bll.TraPatrolDetailAmount(where);
        }

        #endregion

        #region 明细新增 巡查过程分配供应商表

        /// <summary>
        /// 明细新增 巡查过程分配供应商表
        /// </summary>
        /// <param name="supperMatchingIds">供应商匹配id</param>
        /// <param name="patrolId">过程巡查维护id</param> 
        /// <returns></returns>
        public ActionResult AddTraPatrolAllot(string supperMatchingIds, string transportIds, int patrolId)
        {
            // 影响行数
            int row = 0;

            if (!string.IsNullOrEmpty(supperMatchingIds)&& !string.IsNullOrEmpty(transportIds))
            {
                List<string> supperMatchingIdList = new List<string>(supperMatchingIds.Split(','));

                List<string> transportIdList = new List<string>(transportIds.Split(','));

                row = bll.AddTraPatrolAllot(supperMatchingIdList, transportIdList, patrolId);
            }

            ////变更 将供应商匹配状态更新为使用
            //bll.ChangeState(supperMatchingIds);

            //变更 将巡查计划维护状态更新为分配
            bll.ChangePatrolState(patrolId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 编辑 巡查过程分配表 分配

        /// <summary>
        /// 编辑 巡查过程分配表 分配
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraPatrolAllotInfo(TraPatrolModel model)
        {

            TraPatrolModel beforeModel = bll.GetEditTraPatrolAllotByID(model.PatrolAllotId);

            //编辑 巡查过程分配表 分配
            int result = bll.EditTraPatrolAllotInfo(model);

            if (result > 0)
            {
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
        public ActionResult InvalidAllotState(int tId)
        {
            TraPatrolModel beforeModel = bll.GetModelByID(tId);

            //分配明细 作废
            int row = bll.InvalidAllotState(tId);
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion

        #endregion 
    }
}
