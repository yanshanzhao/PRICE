//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-10    1.0        zbb        新建               
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
 * 类名：TraPatrolRecordController
 * 功能描述：过程巡查记录 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraPatrolRecordController : Controller
    {
        //
        // GET: /Tra/TraPatrolRecord/

        //过程巡查维护
        TraPatrolRecordBLL bll = new TraPatrolRecordBLL();

        //过程巡查记录附件
        TraPatrolRecordAdjunctBLL abll = new TraPatrolRecordAdjunctBLL();

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
            TraPatrolRecordModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfilePatrolRecord> filelist = abll.SuppFileList(model.RecordId);
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
            TraPatrolRecordModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfilePatrolRecord> filelist = abll.SuppFileList(model.RecordId);
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
            TraPatrolRecordModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfilePatrolRecord> filelist = abll.SuppFileList(model.RecordId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);
            return View(model);
        }

        /// <summary>
        /// 供应商明细
        /// </summary>
        public ActionResult TraPatrolRecordDetail(string url)
        {
            ViewBag.url = url;
            return View();
        }
        #endregion

        #region 方法 

        #region 新增 过程巡查记录

        /// <summary>
        /// 新增过程巡查记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns
        public ActionResult AddTraPatrolRecord(TraPatrolRecordModel model)
        {

            model.RecordState = 0;// 巡查记录状态:0-初始;1-提交;10-作废

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID();//公司id

            if (model.NormState == 1)
            { 
                model.IsNorm = 1;// 整改完成：0-不用整改;1-整改未完成;2-整改已完成
            }
            else
            {
                model.IsNorm = 0;// 整改完成：0-不用整改;1-整改未完成;2-整改已完成
            }

            //变更 将过程巡查记录状态更新为已使用
            bll.ChangeState(model.PatrolAllotId);

            //添加 过程巡查记录表
            int TraPatrolRecord = bll.AddTraPatrolRecord(model);

            if (TraPatrolRecord > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.TraPatrolRecordFileList))
                {
                    List<temfilePatrolRecord> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfilePatrolRecord>>(model.TraPatrolRecordFileList);
                    abll.AddFilesForSupplier(fflist, TraPatrolRecord, ref filenames);
                }
                model.TraPatrolRecordFileList = filenames;

                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 数据集 过程巡查记录

        /// <summary>
        /// 数据集 过程巡查记录
        /// </summary>
        /// <param name="index">当前索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="PatrolNumber">巡查编码</param>
        /// <param name="RecordTime">巡查时间</param>
        /// <param name="PatrolType">提交类型</param>
        /// <param name="RecordState">巡查状态</param>
        /// <returns></returns>
        public ActionResult TraPatrolRecordList(int index, int size, string PatrolNumber, string RecordTime, string PatrolType, string RecordState)
        {

            string where = string.Format(" TPR.RecordState != 10 and TPR.CreateDepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + ")) And TPR.CreateUserId={0}", Auxiliary.UserID());

            // 巡查编码
            if (!string.IsNullOrEmpty(PatrolNumber))
            {
                where += string.Format(" And TP.PatrolNumber LIKE '%{0}%'", PatrolNumber.Trim());
            }

            // 巡查时间
            if (!string.IsNullOrEmpty(RecordTime))
            {
                where += string.Format(" And convert(varchar,TPR.RecordTime,120) like '%{0}%'", RecordTime.Trim());
            }

            // 提交类型
            if (!string.IsNullOrEmpty(PatrolType))
            {
                where += string.Format(" And TP.PatrolType = {0}", PatrolType.Trim());
            }

            // 巡查状态
            if (!string.IsNullOrEmpty(RecordState))
            {
                where += string.Format(" And TPR.RecordState = {0}", RecordState.Trim());
            }

            List<TraPatrolRecordModel> list = bll.TraPatrolRecordList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 过程巡查记录

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="PatrolNumber">巡查编码</param>
        /// <param name="RecordTime">巡查时间</param>
        /// <param name="PatrolType">提交类型</param>
        /// <param name="RecordState">巡查状态</param>
        /// <returns></returns>
        public int TraPatrolRecordCount(string PatrolNumber, string RecordTime, string PatrolType, string RecordState)
        {
            string where = string.Format(" TPR.RecordState != 10 and TPR.CreateDepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + ")) And TPR.CreateUserId={0}", Auxiliary.UserID());

            // 巡查编码
            if (!string.IsNullOrEmpty(PatrolNumber))
            {
                where += string.Format(" And TP.PatrolNumber LIKE '%{0}%'", PatrolNumber.Trim());
            }

            // 巡查时间
            if (!string.IsNullOrEmpty(RecordTime))
            {
                where += string.Format(" And convert(varchar,TPR.RecordTime,120) like '%{0}%'", RecordTime.Trim());
            }

            // 提交类型
            if (!string.IsNullOrEmpty(PatrolType))
            {
                where += string.Format(" And TP.PatrolType = {0}", PatrolType.Trim());
            }

            // 巡查状态
            if (!string.IsNullOrEmpty(RecordState))
            {
                where += string.Format(" And TPR.RecordState = {0}", RecordState.Trim());
            }
            return bll.TraPatrolRecordCount(where);
        }
        #endregion

        #region 编辑 过程巡查记录

        /// <summary>
        /// 编辑 过程巡查记录
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraPatrolRecord(TraPatrolRecordModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            if (model.NormState == 1)
            {
                model.IsNorm = 1;// 整改完成：0-不用整改;1-整改未完成;2-整改已完成
            }

            else
            {
                model.IsNorm = 0;// 整改完成：0-不用整改;1-整改未完成;2-整改已完成
            }

            TraPatrolRecordModel beforeModel = bll.GetModelByID(model.RecordId);

            int result = bll.EditTraPatrolRecord(model);

            if (result > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.TraPatrolRecordFileList))
                {
                    List<temfilePatrolRecord> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfilePatrolRecord>>(model.TraPatrolRecordFileList);
                    abll.AddFilesForSupplier(fflist, model.RecordId, ref filenames);
                }
                model.TraPatrolRecordFileList = filenames;

                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 提交按钮逻辑

        /// <summary>
        /// 提交按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitTraPatrolRecord(int tId)
        {
            int row = bll.SubmitTraPatrolRecord(tId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail", content = "提交失败！" });
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
            TraPatrolRecordModel beforeModel = bll.GetModelByID(tId);

            int delUserId = Auxiliary.UserID();

            bll.ChangeStates(beforeModel.PatrolAllotId);

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
        /// <param name="PatrolNumber">巡查编码</param>
        /// <param name="RecordTime">巡查时间</param>
        /// <param name="PatrolType">提交类型</param>
        /// <param name="RecordState">巡查状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string PatrolNumber, string RecordTime, string PatrolType, string RecordState)
        {
            string where = string.Format(" TPR.RecordState != 10 and TPR.CreateDepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + ")) And TPR.CreateUserId={0}", Auxiliary.UserID());

            // 巡查编码
            if (!string.IsNullOrEmpty(PatrolNumber))
            {
                where += string.Format(" And TP.PatrolNumber LIKE '%{0}%'", PatrolNumber.Trim());
            }

            // 巡查时间
            if (!string.IsNullOrEmpty(RecordTime))
            {
                where += string.Format(" And convert(varchar,TPR.RecordTime,120) like '%{0}%'", RecordTime.Trim());
            }

            // 提交类型
            if (!string.IsNullOrEmpty(PatrolType))
            {
                where += string.Format(" And TP.PatrolType = {0}", PatrolType.Trim());
            }

            // 巡查状态
            if (!string.IsNullOrEmpty(RecordState))
            {
                where += string.Format(" And TPR.RecordState = {0}", RecordState.Trim());
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

        #region 数据集 过程巡查记录明细

        /// <summary>
        /// 数据集 过程巡查记录明细
        /// </summary>
        /// <param name="index">当前索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="patrolType">巡查状态</param>
        /// <returns></returns>
        public ActionResult TraPatrolRecordDetailList(int index, int size, string patrolType)
        {

            string where = string.Format(" TP.PatrolState = 10  And TP.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 巡查状态
            if (!string.IsNullOrEmpty(patrolType))
            {
                where += string.Format(" And TP.PatrolType = {0}", patrolType.Trim());
            }

            List<TraPatrolRecordModel> list = bll.TraPatrolRecordDetailList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 过程巡查记录明细

        /// <summary>
        /// 数据记录数 过程巡查记录明细
        /// </summary>
        /// <param name="patrolType">巡查类型</param> 
        /// <returns></returns>
        public int TraPatrolRecordDetailCount(string patrolType)
        {
            string where = string.Format(" TP.PatrolState = 10  And TP.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 巡查类型
            if (!string.IsNullOrEmpty(patrolType))
            {
                where += string.Format(" And TP.PatrolType = {0}", patrolType.Trim());
            }
            return bll.TraPatrolRecordDetailCount(where);
        }
        #endregion

        #endregion

    }
}
