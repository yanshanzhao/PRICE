//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-28    1.0        zbb        新建               
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
 * 类名：TraOperationClaimController
 * 功能描述：运作要求维护 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraOperationClaimController : Controller
    {
        //
        // GET: /Tra/TraOperationClaim/

        //运作要求维护
        TraOperationClaimBLL bll = new TraOperationClaimBLL();

        //运作附件
        TraOperationeAdjunctBLL tcbll = new TraOperationeAdjunctBLL();

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
            TraOperationClaimModel model = bll.GetModelByID(tId);


            //获取附件信息
            List<temfileClaim> filelist = tcbll.SuppFileList(model.OperationClaimId);
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
            TraOperationClaimModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfileClaim> filelist = tcbll.SuppFileList(model.OperationClaimId);
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
            TraOperationClaimModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfileClaim> filelist = tcbll.SuppFileList(model.OperationClaimId);
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
            ViewBag.OperationClaimId = tId;
            return View();
        }
        /// <summary>
        /// 选择供应商规模信息
        /// </summary>
        public ActionResult TraResource(string url, string ids, string type)
        {
            ViewBag.url = url;
            ViewBag.ids = ids;
            ViewBag.type = type;
            return View();
        }
        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult EditOperationSupp(string url, int tId)
        {
            ViewBag.url = url;

            // 获取数据
            TraOperationClaimModel model = bll.GetOperationSuppModelByID(tId);

            return View(model);
        }

        #endregion

        #region 方法 

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddTraOperationClaim(TraOperationClaimModel model)
        {

            model.State = 0;// 状态 未提交 

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            model.OperationNumber = Auxiliary.CurCompanyAutoNum("TOC");//运作编号

            int OperationClaimId = bll.AddTraOperationClaim(model);

            if (OperationClaimId > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppFileList))
                {
                    List<temfileClaim> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfileClaim>>(model.SuppFileList);
                    tcbll.AddFilesForSupplier(fflist, OperationClaimId, ref filenames);
                }
                model.SuppFileList = filenames;

                if (!string.IsNullOrEmpty(model.ResourceIdList))
                {
                    List<string> traChooseIdList = new List<string>(model.ResourceIdList.Split(','));

                    bll.AddTraResource(traChooseIdList, OperationClaimId, model.FileName, model.FileUrl);
                }
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        #endregion

        #region 选择线路
        /// <summary>
        /// 明细新增 运作分配供应商表
        /// </summary>
        /// <param name="supperMatchingIds">供应商匹配id</param>
        /// <param name="operationClaimId">运作id</param>
        /// <returns></returns>
        public ActionResult AddTraResource(string LineId, int trachooseid, string FileName, string FileUrl)
        {
            // 影响行数
            int row = 0;

            if (!string.IsNullOrEmpty(LineId))
            {
                List<string> LineIdList = new List<string>(LineId.Split(','));
                row = bll.AddTraResource(LineIdList, trachooseid, FileName, FileUrl);
            }

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region index数据集

        /// <summary>
        /// index数据集
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="theme">运作主题</param>
        /// <param name="beginTimes">开始时间</param>
        /// <param name="endTimes">结束时间</param>
        /// <param name="state">提交状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public ActionResult TraOperationClaimList(int index, int size, string theme, string beginTimes, string endTimes, string state, string createTime)
        {
            string where = string.Format(" toc.State != 20 and toc.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 运作主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And toc.Theme like '%{0}%'", theme.Trim());
            }

            // 开始时间
            if (!string.IsNullOrEmpty(beginTimes))
            {
                where += string.Format(" And convert(varchar,toc.BeginTimes,120) like '%{0}%'", beginTimes.Trim());
            }

            // 结束时间
            if (!string.IsNullOrEmpty(endTimes))
            {
                where += string.Format(" And convert(varchar,toc.EndTimes,120) like '%{0}%'", endTimes.Trim());
            }

            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And toc.State = {0}", state.Trim());
            } 

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,toc.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            List<TraOperationClaimModel> list = bll.TraOperationClaimList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region 数据记录数
        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="theme">运作主题</param>
        /// <param name="beginTimes">开始时间</param>
        /// <param name="endTimes">结束时间</param>
        /// <param name="state">提交状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int TraOperationClaimCount(string theme, string beginTimes, string endTimes, string state, string createTime)
        {
            string where = string.Format(" toc.State != 20 and toc.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 运作主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And toc.Theme like '%{0}%'", theme.Trim());
            }

            // 开始时间
            if (!string.IsNullOrEmpty(beginTimes))
            {
                where += string.Format(" And convert(varchar,toc.BeginTimes,120) like '%{0}%'", beginTimes.Trim());
            }

            // 结束时间
            if (!string.IsNullOrEmpty(endTimes))
            {
                where += string.Format(" And convert(varchar,toc.EndTimes,120) like '%{0}%'", endTimes.Trim());
            }

            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And toc.State = {0}", state.Trim());
            } 

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,toc.CreateTime,120) like '%{0}%'", createTime.Trim());
            }
            return bll.TraOperationClaimCount(where);
        }
        #endregion

        #region index数据集

        /// <summary>
        /// index数据集
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="theme">运作主题</param>
        /// <param name="beginTimes">开始时间</param>
        /// <param name="endTimes">结束时间</param>
        /// <param name="state">提交状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public ActionResult TraOperationClaimLists(int index, int size, string theme, string beginTimes, string endTimes, string state, string createTime,string tTransportId)
        {
            string where = string.Format(" toc.State != 20 and toc.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 运作主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And toc.Theme like '%{0}%'", theme.Trim());
            }

            // 开始时间
            if (!string.IsNullOrEmpty(beginTimes))
            {
                where += string.Format(" And convert(varchar,toc.BeginTimes,120) like '%{0}%'", beginTimes.Trim());
            }

            // 结束时间
            if (!string.IsNullOrEmpty(endTimes))
            {
                where += string.Format(" And convert(varchar,toc.EndTimes,120) like '%{0}%'", endTimes.Trim());
            }

            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And toc.State = {0}", state.Trim());
            }

            // 运输供应商
            if (!string.IsNullOrEmpty(tTransportId))
            {
                where += string.Format(" And tos.TransportId = {0}", tTransportId.Trim());
            }

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,toc.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            List<TraOperationClaimModel> list = bll.TraOperationClaimLists(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region 数据记录数
        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="theme">运作主题</param>
        /// <param name="beginTimes">开始时间</param>
        /// <param name="endTimes">结束时间</param>
        /// <param name="state">提交状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int TraOperationClaimCounts(string theme, string beginTimes, string endTimes, string state, string createTime,string tTransportId)
        {
            string where = string.Format(" toc.State != 20 and toc.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 运作主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And toc.Theme like '%{0}%'", theme.Trim());
            }

            // 开始时间
            if (!string.IsNullOrEmpty(beginTimes))
            {
                where += string.Format(" And convert(varchar,toc.BeginTimes,120) like '%{0}%'", beginTimes.Trim());
            }

            // 结束时间
            if (!string.IsNullOrEmpty(endTimes))
            {
                where += string.Format(" And convert(varchar,toc.EndTimes,120) like '%{0}%'", endTimes.Trim());
            }

            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And toc.State = {0}", state.Trim());
            }

            // 运输供应商
            if (!string.IsNullOrEmpty(tTransportId))
            {
                where += string.Format(" And tos.TransportId = {0}", tTransportId.Trim());
            }

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,toc.CreateTime,120) like '%{0}%'", createTime.Trim());
            }
            return bll.TraOperationClaimCounts(where);
        }
        #endregion

        #region 编辑运作维护数据
        /// <summary>
        /// 编辑运作维护数据
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraOperationClaim(TraOperationClaimModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            TraOperationClaimModel beforeModel = bll.GetModelByID(model.OperationClaimId);

            int result = bll.EditTraOperationClaim(model);

            if (result > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppFileList))
                {
                    List<temfileClaim> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfileClaim>>(model.SuppFileList);
                    tcbll.AddFilesForSuppliers(fflist, model.OperationClaimId, ref filenames);
                }
                model.SuppFileList = filenames;

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
        public ActionResult SubmitTraOperationClaim(int tId)
        {
            int row = bll.SubmitTraOperationClaim(tId);

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
            TraOperationClaimModel beforeModel = bll.GetModelByID(tId);

            int row = 0;//行数

            int delUserId = Auxiliary.UserID();//作废负责人

            int state = beforeModel.State;//提交状态：0-未提交；1-已提交;10-作废;20-删除

            if (state == 0)
            {
                row = bll.DeleteState(tId, delUserId);//作废之后 状态变为删除状态
            }
            if (state == 1)
            {
                row = bll.InvalidState(tId, delUserId);//作废之后 状态变为作废状态
            }
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
        /// <param name="theme">运作主题</param>
        /// <param name="beginTimes">开始时间</param>
        /// <param name="endTimes">结束时间</param>
        /// <param name="state">提交状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string theme, string beginTimes, string endTimes, string state, string createTime)
        {
            string where = string.Format(" toc.State != 20 and toc.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 运作主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And toc.Theme like '%{0}%'", theme.Trim());
            }

            // 开始时间
            if (!string.IsNullOrEmpty(beginTimes))
            {
                where += string.Format(" And convert(varchar,toc.BeginTimes,120) like '%{0}%'", beginTimes.Trim());
            }

            // 结束时间
            if (!string.IsNullOrEmpty(endTimes))
            {
                where += string.Format(" And convert(varchar,toc.EndTimes,120) like '%{0}%'", endTimes.Trim());
            }

            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And toc.State = {0}", state.Trim());
            }

            // 创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,toc.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            System.Data.DataTable dt = bll.ExportDataTable(where);
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Export, ResultEnum.Sucess, new
            {
                Type = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        #region 新增时和编辑时： 弹出的仓储供应商数据集list 

        /// <summary>
        /// 新增时和编辑时： 弹出的仓储供应商数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="ResourceName"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult TraOperationClaimballlist(int index, int size, string ResourceName, string type, string ids)
        {
            string where = string.Format("  TCR.CreateDepartmentId={0} AND  TCR.State=1 ", Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And TCR.ResourceId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And TCR.ResourceId NOT IN (SELECT ResourceId FROM TraOperationeAdjunct WHERE OperationClaimId ={0} and DetailState != 0)", ids.Trim());
                }
            }
            // 线路名称
            if (!string.IsNullOrEmpty(ResourceName))
            {
                where += string.Format(" And TCR.ResourceName LIKE '%{0}%'", ResourceName.Trim());
            }
            List<TraOperationClaimModel> list = bll.TraOperationClaimballlist(index, size, where);

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
        /// <param name="ResourceName"></param>
        /// <param name="type"></param>
        /// <param name="ids"></param>
        /// <returns></returns> 
        public int TraOperationClaimballAmount(string ResourceName, string type, string ids)
        {

            string where = string.Format("  TCR.CreateDepartmentId={0} AND  TCR.State=1 ", Auxiliary.DepartmentId());

            // 排除已有的ID 
            if (!string.IsNullOrEmpty(ids))
            {
                if (type == "add")
                {
                    where += string.Format(" And TCR.ResourceId NOT IN ({0})", ids.Trim());
                }
                else if (type == "edit")
                {
                    where += string.Format(" And TCR.ResourceId NOT IN (SELECT ResourceId FROM TraOperationeAdjunct WHERE OperationClaimId ={0} and DetailState != 0)", ids.Trim());
                }
            }

            // 线路名称
            if (!string.IsNullOrEmpty(ResourceName))
            {
                where += string.Format(" And TCR.ResourceName LIKE '%{0}%'", ResourceName.Trim());
            }

            return bll.TraOperationClaimballAmount(where);
        }
        #endregion

        #region 选择线路表 作废

        /// <summary>
        /// 作废 选择线路表
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidResource(int tId)
        {
            // 作废之前Model
            TraOperationClaimModel beforeModel = bll.GetResourceModelByID(tId);

            // 作废(更改状态)
            int row = bll.InvalidResource(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }

        #endregion

        #endregion

        #region 方法 分配

        #region 分配明细 数据集

        /// <summary>
        /// 分配明细 数据集
        /// </summary>
        /// <param name="index">当前页面</param>
        /// <param name="size">页面索引</param>
        /// <param name="operationClaimId">运作id</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult OperationClaimAllotList(int index, int size, int operationClaimId, string supplierName, string state)
        {

            string where = string.Format("  TOS.State != 0  And TOS.OperationClaimId = {0}", operationClaimId);

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TOS.State = {0}", state.Trim());
            }

            List<TraOperationClaimModel> list = bll.OperationClaimAllotList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 分配明细 数据记录数

        /// <summary>
        /// 分配明细 数据记录数
        /// </summary>
        /// <param name="operationClaimId">运作id</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public int OperationClaimAllotCount(int operationClaimId, string supplierName, string state)
        {
            string where = string.Format("  TOS.State != 0  And TOS.OperationClaimId = {0}", operationClaimId);

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TOS.State = {0}", state.Trim());
            }

            return bll.OperationClaimAllotCount(where);
        }
        #endregion

        #region 运作要求明细列表 数据集

        /// <summary>
        /// 运作要求明细列表 数据集
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="ids">主键id</param>
        /// <returns></returns>
        public ActionResult OperationClaimDetailList(string supplierName, string ids)
        {
            string where = string.Format("  ST.State = 1 AND ST.TransportState!='F6' And ST.DepartmentId = {0}", Auxiliary.DepartmentId());

            //主键id
            if (!string.IsNullOrEmpty(ids))
            {
                where += string.Format("  And ST.TransportId NOT IN (SELECT TransportId FROM dbo.TraOperationSupp WHERE OperationClaimId ={0} AND State =1)", ids.Trim());
            }
            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName  LIKE '%{0}%'", supplierName.Trim());
            }

            return Json(bll.OperationClaimDetailList(where));
        }
        #endregion

        #region 运作要求明细列表 数据记录数

        /// <summary>
        /// 运作要求明细列表 数据记录数
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="ids">主键id</param>
        /// <returns></returns>
        public int OperationClaimDetailAmount(string supplierName, string ids)
        {
            string where = string.Format("  ST.State = 1 AND ST.TransportState!='F6' And ST.DepartmentId = {0}", Auxiliary.DepartmentId());

            //主键id
            if (!string.IsNullOrEmpty(ids))
            {
                where += string.Format("  And ST.TransportId NOT IN (SELECT TransportId FROM dbo.TraOperationSupp WHERE OperationClaimId ={0} AND State =1)", ids.Trim());
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            return bll.OperationClaimDetailAmount(where);
        }

        #endregion

        #region 明细新增 运作分配供应商表

        /// <summary>
        /// 明细新增 运作分配供应商表
        /// </summary>
        /// <param name="supperMatchingIds">供应商匹配id</param>
        /// <param name="operationClaimId">运作id</param>
        /// <returns></returns>
        public ActionResult AddTraOperationSupp(string supperMatchingIds, string transportIds, int operationClaimId)
        {
            // 影响行数
            int row = 0;

            if (!string.IsNullOrEmpty(supperMatchingIds) && !string.IsNullOrEmpty(transportIds))
            {
                List<string> supperMatchingIdList = new List<string>(supperMatchingIds.Split(','));

                List<string> transportIdList = new List<string>(transportIds.Split(','));

                row = bll.AddTraOperationSupp(supperMatchingIdList, transportIdList, operationClaimId);
            }

            //供应商匹配id
            //bll.ChangeState(supperMatchingIds);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 编辑 运作要求维护 分配

        /// <summary>
        /// 编辑 运作要求维护 分配
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraOperationSupp(TraOperationClaimModel model)
        {

            TraOperationClaimModel beforeModel = bll.GetOperationSuppModelByID(model.OperationSuppId);

            int result = bll.EditTraOperationSupp(model);

            if (result > 0)
            {
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 分配明细 作废

        /// <summary>
        ///  分配明细 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidAllotState(int tId)
        {
            TraOperationClaimModel beforeModel = bll.GetModelByID(tId);

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

        #region 新增按钮时：显示下方的仓储选择明细数据集list

        /// <summary>
        /// 新增按钮时：显示下方的仓储选择明细数据集list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="traChooseId"></param>
        /// <returns></returns>
        public ActionResult TraResourceaddList(int index, int size, string traChooseId)
        {

            string where = string.Format(" TCR.ResourceId in ({0})", traChooseId);

            List<TraOperationClaimModel> list = bll.TraResourceaddList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 新增按钮时：显示下方的仓储选择明细数据记录count

        /// <summary>
        /// 新增按钮时：显示下方的仓储选择明细数据记录count
        /// </summary>
        /// <param name="traChooseId"></param> 
        /// <returns></returns>
        public int TraResourceaddAmount(string traChooseId)
        {

            string where = string.Format(" TCR.ResourceId in ({0})", traChooseId);

            return bll.TraResourceaddAmount(where);
        }
        #endregion

        #region 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   

        /// <summary>
        /// 编辑按钮时：根据主键id 显示下方的仓储选择明细数据集list   
        /// </summary> 
        /// <param name="operationClaimId"></param>
        /// <returns></returns>
        public ActionResult TraResourceList(string operationClaimId, string tDetailType)
        {

            string where = string.Format(" TOC.OperationClaimId in ({0}) and DetailState != 0", operationClaimId);

            List<TraOperationClaimModel> list = new List<TraOperationClaimModel>();

            if (tDetailType == "0")
            {
                list = new TraOperationClaimBLL().TraResourceList(where);
            }

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 编辑按钮时：显示下方的仓储选择明细数据记录count 

        /// <summary>
        /// 编辑按钮时：显示下方的仓储选择明细数据记录count
        /// </summary>
        /// <param name="operationClaimId"></param> 
        /// <returns></returns>
        public int TraResourceAmount(string operationClaimId, string tDetailType)
        {

            int count = 0;

            string where = string.Format(" TOC.OperationClaimId in ({0}) and DetailState != 0", operationClaimId);

            if (tDetailType == "0")
            {
                count = bll.TraResourceAmount(where);
            }
            return count;
        }
        #endregion

        #endregion 
    }
}
