//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-06    1.0        MH         新建            
//2018-08-20    1.0        FJK        修改提交功能           
//-------------------------------------------------------------------------
#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using SRM.BLL.Supplier;
using SRM.Model.Supplier;
using SRM.Web.Controllers;
using Newtonsoft.Json.Converters;
using SRM.Model.Basis;
using SRM.BLL.Sys;
using SRM.Model.Sys;
using System.IO;
using Aspose.Cells;
using SRM.BLL.Tra;
using SRM.BLL.Basis;
#endregion
/*********************************
 * 类名：SupplierTransportController
 * 功能描述：运输供应商 控制器 
 * ******************************/
namespace SRM.Web.Areas.Supplier.Controllers
{
    public class SupplierTransportController : Controller
    {
        SupplierBLL basebll = new SupplierBLL();
        SupplierTransportBLL bll = new SupplierTransportBLL();
        SupplierAdjunctBLL sabll = new SupplierAdjunctBLL();

        // 线路维护BLL
        BasisLineBLL BLbll = new BasisLineBLL();

        SupplierCyclBLL scbll = new SupplierCyclBLL();

        #region 主页

        /// <summary>
        /// 主页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 供应商列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="name">供应商名称</param>
        /// <param name="num">供应商编号</param>
        /// <param name="line">配送线路</param>
        /// <param name="suppclass">供应商种类</param>
        /// <param name="suppscope">配送供应商范围</param>
        /// <param name="supplevel">供应商级别</param>
        /// <param name="suppstate">供应商状态</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="state">状态</param>
        public ActionResult List(int index, int size, string num, string name, string line, int suppclass, int suppscope, int supplevel, string suppstate, int state, string starttime, string endtime)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("  ts.CompanyId={0} and ts.DepartmentId={1} ", Auxiliary.CompanyID(), Auxiliary.DepartmentId()));

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And s.SupplierName like '%" + name.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(num))
            {
                sb.Append(string.Format(" And ts.TransportNumber='{0}' ", num));
            }
            if (!string.IsNullOrEmpty(line))
            {
                sb.Append(string.Format(" And ts.TransitLines like '%{0}%' ", line.Trim()));
            }
            if (suppclass != -1)
            {
                sb.Append(string.Format(" And ts.TransportKind={0}  ", suppclass));
            }

            if (suppscope != -1)
            {
                sb.Append(string.Format(" And ts.DistributionScope={0}  ", suppscope));
            }

            if (supplevel != -1)
            {
                sb.Append(string.Format(" And ts.SupplierLevel={0}  ", supplevel));
            }

            if (suppstate.Trim() != "-1")
            {
                sb.Append(string.Format(" And ts.TransportState='{0}'  ", suppstate.Trim()));
            }

            if (state != -1)
            {
                sb.Append(string.Format(" And ts.State={0}  ", state));
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sb.Append("  And ts.CretaeTime>'" + starttime + "' ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sb.Append(" And ts.CretaeTime<'" + Convert.ToDateTime(endtime).ToString("yyyy-MM-dd") + "' ");
            }

            sb.Append(" And ts.State!=20 ");

            List<SupplierTransportModel> list = bll.SupplierTransportPageList(index, size, sb.ToString());

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <param name="name">供应商名称</param>
        /// <param name="num">供应商编号</param>
        /// <param name="line">配送线路</param>
        /// <param name="suppclass">供应商种类</param>
        /// <param name="suppscope">配送供应商范围</param>
        /// <param name="supplevel">供应商级别</param>
        /// <param name="suppstate">供应商状态</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="state">状态</param>
        public int Count(string num, string name, string line, int suppclass, int suppscope, int supplevel, string suppstate, int state, string starttime, string endtime)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("  ts.CompanyId={0} and ts.DepartmentId={1} ", Auxiliary.CompanyID(), Auxiliary.DepartmentId()));

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And s.SupplierName like '%" + name.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(num))
            {
                sb.Append(string.Format(" And ts.TransportNumber='{0}' ", num));
            }
            if (!string.IsNullOrEmpty(line))
            {
                sb.Append(string.Format(" And ts.TransitLines like '%{0}%' ", line.Trim()));
            }
            if (suppclass != -1)
            {
                sb.Append(string.Format(" And ts.TransportKind={0}  ", suppclass));
            }

            if (suppscope != -1)
            {
                sb.Append(string.Format(" And ts.DistributionScope={0}  ", suppscope));
            }

            if (supplevel != -1)
            {
                sb.Append(string.Format(" And ts.SupplierLevel={0}  ", supplevel));
            }

            if (suppstate.Trim() != "-1")
            {
                sb.Append(string.Format(" And ts.TransportState='{0}'  ", suppstate.Trim()));
            }

            if (state != -1)
            {
                sb.Append(string.Format(" And ts.State={0}  ", state));
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sb.Append("  And ts.CretaeTime>'" + starttime + "' ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sb.Append(" And ts.CretaeTime<'" + Convert.ToDateTime(endtime).ToString("yyyy-MM-dd") + "' ");
            }

            sb.Append(" And ts.State!=20 ");

            return bll.SupplierTransportPageCount(sb.ToString());
        }

        /// <summary>
        /// 获取系统字典表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DictList()
        {
            return Json(basebll.GetDictLists(Auxiliary.CompanyID()));
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
            SupplierTransportModel beforeModel = bll.GetModelByID(tId.ToString());

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
                scbll.AddBeginTypes(beforeModel.SupplierId, beforeModel.DepartmentId, beforeModel.TransportId, 0,1);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "启用", Id = tId, Number = beforeModel.TransportNumber });
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "启用", Id = tId, Number = beforeModel.TransportNumber });
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
            SupplierTransportModel beforeModel = bll.GetModelByID(tId.ToString());

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
                scbll.UdateEndTypes(beforeModel.SupplierId, 0, 0, beforeModel.TransportId);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "停用", Id = tId, Number = beforeModel.TransportNumber });
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "停用", Id = tId, Number = beforeModel.TransportNumber });
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
            SupplierTransportModel beforeModel = bll.GetModelByID(tId.ToString());

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
                scbll.UdateEndTypes(beforeModel.SupplierId, 0, 1, beforeModel.TransportId);

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "拉黑", Id = tId, Number = beforeModel.TransportNumber });
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "拉黑", Id = tId, Number = beforeModel.TransportNumber });
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 添加
        /// <summary>
        /// 运输供应商添加页面
        /// </summary>
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult AddInfo(SupplierTransportModel model)
        {
            //默认不处理
            model.OperationExperience = string.Empty;
            model.Potential = string.Empty;

            model.TransitLines = model.TransitLines ?? string.Empty;
            model.TransitBeginId = 0;
            model.TransitEndId = 0;
            model.DepartureTime = model.DepartureTime ?? string.Empty;
            model.ArrivalTime = model.ArrivalTime ?? string.Empty;
            model.BusinessHours = model.BusinessHours ?? string.Empty;
            model.DisqualificationCause = model.DisqualificationCause ?? string.Empty;
            model.Remark = model.Remark ?? string.Empty;

            model.State = 0;
            model.TransportState = "F1";
            model.DepartmentId = Auxiliary.DepartmentId();
            model.CretaeUserId = Auxiliary.UserID();
            model.CretaeTime = DateTime.Now;
            model.UseState = 0;
            model.CompanyId = Auxiliary.CompanyID();

            model.TransportNumber = Auxiliary.CurCompanyAutoNum("TRA");
            model.OperationTime = model.OperationTime ?? string.Empty;

            //评估部分暂时不处理
            model.AssessScore = "0";
            model.AssessResult = 0;
            model.DisqualificationCause = string.Empty;


            model.UserName = Auxiliary.GetUserName();

            int suppid = bll.AddSupplierTransport(model);

            if (suppid > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppFileList))
                {
                    List<temfile> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(model.SuppFileList);
                    sabll.AddFiles(fflist, suppid, 20, ref filenames);
                }
                model.SuppFileList = filenames; //记录文件列表日志

                // 新增评估模板 
                // TraAssessFrom 查询本公司状态为提交的模板信息
                Model.Tra.TraAssessFromModel TAFmodel = new TraAssessFromBLL().GetModelByCompanyId(Auxiliary.CompanyID());

                if (TAFmodel != null)
                {
                    Model.Tra.TraAssessFromDeparModel TAFDmodel = new Model.Tra.TraAssessFromDeparModel();
                    TAFDmodel.AssessFromId = TAFmodel.AssessFromId;
                    TAFDmodel.DepartmentId = Auxiliary.DepartmentId();
                    TAFDmodel.TransportId = suppid;
                    TAFDmodel.State = 1;

                    // 新增到TraAssessFromDepar中
                    new TraAssessFromDeparBLL().AddTraAssessFromDepar(TAFDmodel);
                    Auxiliary.SupplierLog<SupplierTransportModel>(OperateEnum.Add, ResultEnum.Sucess, model);
                    return Json(new { flag = "ok", content = "新增成功！" });
                }
                else
                {
                    Auxiliary.SupplierLog<SupplierTransportModel>(OperateEnum.Add, ResultEnum.Sucess, model);
                    return Json(new { flag = "ok", content = "新增成功，但无评估模板信息！" });
                }
            }
            else
            {
                Auxiliary.SupplierLog<SupplierTransportModel>(OperateEnum.Add, ResultEnum.Fail, model);
                return Json(new { flag = "fail" });
            }
        }
        #endregion

        #region 编辑
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(string ids)
        {
            SupplierTransportModel model = bll.GetModelByID(ids);

            if (model.FatherId == 0)
            {
                ViewBag.pname = "无";
            }
            else
            {
                SupplierModel pmodel = basebll.GetModelByID(model.FatherId.ToString());

                if (pmodel != null)
                {
                    ViewBag.pname = pmodel.SupplierName;
                }
                else
                {
                    ViewBag.pname = "无";
                }
            }

            List<temfile> filelist = sabll.SuppFileList(Convert.ToInt32(ids), 20);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        public ActionResult EditInfo(SupplierTransportModel model)
        {
            //默认不处理
            model.OperationExperience = string.Empty;
            model.Potential = string.Empty;

            model.TransitLines = model.TransitLines ?? string.Empty;
            model.TransitBeginId = 0;
            model.TransitEndId = 0;
            model.DepartureTime = model.DepartureTime ?? string.Empty;
            model.ArrivalTime = model.ArrivalTime ?? string.Empty;
            model.BusinessHours = model.BusinessHours ?? string.Empty;
            model.DisqualificationCause = model.DisqualificationCause ?? string.Empty;
            model.Remark = model.Remark ?? string.Empty;

            model.State = 0;
            model.TransportState = "F1";
            model.DepartmentId = Auxiliary.DepartmentId();
            model.CretaeUserId = Auxiliary.UserID();
            model.CretaeTime = DateTime.Now;
            model.UseState = 0;
            model.CompanyId = Auxiliary.CompanyID();

            //    model.TransportNumber = Auxiliary.CurCompanyAutoNum("TRA");
            model.OperationTime = model.OperationTime ?? string.Empty;

            //评估部分暂时不处理
            model.AssessScore = "0";
            model.AssessResult = 0;
            model.DisqualificationCause = string.Empty;

            model.UserName = Auxiliary.GetUserName();

            SupplierTransportModel oldModel = bll.GetModelByID(model.TransportId.ToString());

            int suppid = bll.UpdateSupplierTransport(model);

            if (suppid > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppFileList))
                {
                    List<temfile> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(model.SuppFileList);
                    sabll.AddFiles(fflist, model.TransportId, 20, ref filenames);
                }
                model.SuppFileList = filenames; //记录文件列表日志

                //补齐原实体信息
                string[] tems = model.TemSelectData.Split(',');

                if (tems.Length == 3)
                {
                    oldModel.TransportKindName = tems[0];
                    oldModel.DistributionScopeName = tems[1];
                    oldModel.SupplierLevelName = tems[2];
                }
                oldModel.SuppFileList = model.SuppOldFileList ?? string.Empty;
                oldModel.CooperationTypeName = oldModel.CooperationType == 1 ? "是" : "否";
                oldModel.IsAssessName = oldModel.IsAssess == 1 ? "是" : "否";
                oldModel.LocaleInspectName = oldModel.LocaleInspect == 1 ? "是" : "否";

                Auxiliary.SupplierLog<SupplierTransportModel>(OperateEnum.Edit, ResultEnum.Sucess, oldModel, model);
                return Json(new { flag = "ok" });
            }
            else
            {
                Auxiliary.SupplierLog<SupplierTransportModel>(OperateEnum.Edit, ResultEnum.Fail, oldModel, model);
                return Json(new { flag = "fail" });
            }

        }
        #endregion

        #region 查看

        [Operate(Name = OperateEnum.View)]
        public ActionResult Views(string ids)
        {
            SupplierTransportModel model = bll.GetModelByID(ids);

            if (model.FatherId == 0)
            {
                ViewBag.pname = "无";
            }
            else
            {
                SupplierModel pmodel = basebll.GetModelByID(model.FatherId.ToString());

                if (pmodel != null)
                {
                    ViewBag.pname = pmodel.SupplierName;
                }
                else
                {
                    ViewBag.pname = "无";
                }
            }

            List<temfile> filelist = sabll.SuppFileList(Convert.ToInt32(ids), 20);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }
        #endregion

        #region 作废
        /// <summary>
        /// 作废
        /// </summary>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult Delete(string ids, string names)
        {
            SupplierTransportModel model = bll.GetModelByID(ids);

            if (model.UseState == 1)
            {
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Depend, new { Type = "作废", Id = ids, Name = names, Number = model.TransportNumber });
                return Json(new { flag = "use" });
            }
            if (model.State != 0)
            {
                return Json(new { flag = "no" });
            }
            if (model.State == 0)
            {
                int res = bll.DeleteSupplierTransportByID(ids);

                if (res > 0)
                {
                    Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "作废", Id = ids, Name = names, Number = model.TransportNumber });
                    return Json(new { flag = "ok" });
                }
            }

            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "作废", Id = ids, Name = names, Number = model.TransportNumber });
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 导出

        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string num, string name, string line, int suppclass, int suppscope, int supplevel, string suppstate, int state, string starttime, string endtime)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("  ST.CompanyId={0} and ST.DepartmentId={1} ", Auxiliary.CompanyID(), Auxiliary.DepartmentId()));

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And S.SupplierName like '%" + name.Trim() + "%' ");
            }

            if (!string.IsNullOrEmpty(num))
            {
                sb.Append(string.Format(" And ST.TransportNumber='{0}' ", num));
            }
            if (!string.IsNullOrEmpty(line))
            {
                sb.Append(string.Format(" And ST.TransitLines like '%{0}%' ", line.Trim()));
            }
            if (suppclass != -1)
            {
                sb.Append(string.Format(" And ST.TransportKind={0}  ", suppclass));
            }

            if (suppscope != -1)
            {
                sb.Append(string.Format(" And ST.DistributionScope={0}  ", suppscope));
            }

            if (supplevel != -1)
            {
                sb.Append(string.Format(" And ST.SupplierLevel={0}  ", supplevel));
            }

            if (suppstate.Trim() != "-1")
            {
                sb.Append(string.Format(" And ST.TransportState='{0}'  ", suppstate.Trim()));
            }

            if (state != -1)
            {
                sb.Append(string.Format(" And ST.State={0}  ", state));
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sb.Append("  And ST.CretaeTime>'" + starttime + "' ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sb.Append(" And ST.CretaeTime<'" + Convert.ToDateTime(endtime).ToString("yyyy-MM-dd") + "' ");
            }

            sb.Append(" And ST.State!=20 ");

            System.Data.DataTable dt = bll.ExportDataTable(sb.ToString(), Auxiliary.CompanyID());
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new
            {
                Type = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });

            return Json(new { flag = "ok", guid = url });

        }
        #endregion

        #region 提交
        /// <summary>
        /// 提交信息
        /// </summary>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitInfo(string transportId)
        {
            int res = 0;

            SupplierTransportModel beforeModel = bll.GetModelByID(transportId);

            // 根据供应商ID查询是否存在同供应商状态为()的数据 
           int count = bll.SummbitCount(beforeModel.SupplierId);

            if (count >0)
            {
                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Fail, new { Type = "提交", Id = transportId, Number = beforeModel.TransportNumber });

                return Json(new { flag = "exist", content = "同供应商已存在已提交数据！" });
            }

            //无所属的供应商 提交状态变为  已提交
            bll.UpdateSuppState(transportId, 1);
            if (beforeModel.FatherId == 0)
            {
                res = bll.UpdateSuppTranStates(transportId, "F2");
            }
            else
            { 
                res = bll.UpdateSuppTranStates(transportId, "F4");
            }

            if (res > 0)
            {
                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Sucess, new { Type = "提交", Id = transportId, Number = beforeModel.TransportNumber });

                return Json(new { flag = "ok", content = "提交成功！" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Submit, ResultEnum.Fail, new { Type = "提交", Id = transportId, Number = beforeModel.TransportNumber });

            return Json(new { flag = "fail", content = "提交失败！" });
        }
        #endregion

        #region 辅助(添加/编辑)

        #region 供应商选择功能
        /// <summary>
        /// 供应商查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Select(string url)
        {
            ViewBag.url = url;
            return View();
        }
        /// <summary>
        /// 供应商列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="name">供应商名称</param>
        public ActionResult SelList(int index, int size, string name)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("  ts.CompanyId={0} and ts.DepartmentId={1} ", Auxiliary.CompanyID(), Auxiliary.DepartmentId()));

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And s.SupplierName like '%" + name.Trim() + "%' ");
            }

            sb.Append(string.Format(" And ts.TransportState='{0}'  ", "F4"));

            List<SupplierTransportModel> list = bll.SupplierTransportPageList(index, size, sb.ToString());

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <param name="name">供应商名称</param>
        public int SelCount(string name)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("  ts.CompanyId={0} and ts.DepartmentId={1} ", Auxiliary.CompanyID(), Auxiliary.DepartmentId()));

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And s.SupplierName like '%" + name.Trim() + "%' ");
            }

            sb.Append(string.Format(" And ts.TransportState='{0}'  ", "F4"));

            return bll.SupplierTransportPageCount(sb.ToString());
        }

        #endregion
        /// <summary>
        /// 文件上传页面
        /// </summary>       
        public ActionResult Files(string type, string url)
        {
            ViewBag.url = url;//前台寻找iframe
            ViewBag.type = type;
            return View();
        }
        /// <summary>
        /// 文件类型列表
        /// </summary>
        /// <param name="type">供应商类型（0 运输  ……）</param>
        public ActionResult FileTypes(string type)
        {
            int typecode = 0;

            if (type == "tra")// 运输供应商登记
            {
                typecode = 0;
            }
            else if (type == "storage")// 仓储供应商登记
            {
                typecode = 1;
            }
            else if (type == "start")// 仓储启用登记
            {
                typecode = 2;
            }
            else if (type == "notification")// 招标通知
            {
                typecode = 3;
            }
            else if (type == "bid")// 供应商投标
            {
                typecode = 4;
            }
            else if (type == "claim")// 运作要求维护
            {
                typecode = 5;
            }
            else if (type == "patrol")// 过程巡查维护
            {
                typecode = 6;
            }
            else if (type == "record")// 过程巡查记录
            {
                typecode = 7;
            }
            //else if (type == "abnormity")// 异常整改记录
            //{
            //    typecode = 8;
            //}
            else if (type == "incentive")// 激励措施记录
            {
                typecode = 9;
            }
            else if (type == "patrolrecord")// 培训执行记录
            {
                typecode = 10;
            }
            else if (type == "assess")// 仓储评估
            {
                typecode = 11;
            }
            else if (type == "trachoose")// 运输选择申请
            {
                typecode = 12;
            }
            else if (type == "result")// 年度绩效结果
            {
                typecode = 13;
            }
            else if (type == "plan")// 异常整改计划及评估 
            {
                typecode = 14;
            }
            else if (type == "measures")// 异常整改计划及评估附件
            {
                typecode = 15;
            }

            //待定 
            return Json(new SysAdjunctTypeBLL().SysAdjunctTypePageList(string.Format(" CompanyId={0} And AdjunctType={1}", Auxiliary.CompanyID(), typecode)));
        }
        #endregion

        #region 线路
        /// <summary>
        /// 线路维护
        /// </summary>
        /// <param name="suppid">运输供应商主键</param>
        /// <returns></returns>
        public ActionResult SubList(string suppid)
        {
            ViewBag.suppid = suppid;

            return View();
        }

        /// <summary>
        /// 线路明细列表
        /// </summary>
        /// <param name="index">页号</param>
        /// <param name="size">页数</param>
        /// <param name="suppid">运输供应商主键</param>
        /// <param name="name">线路名称</param>
        /// <param name="beginid">开始位置</param>
        /// <param name="endid">结束位置</param>
        /// <returns></returns>
        public ActionResult SubLists(int index, int size, string suppid, string name, int beginid, int endid)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("  l.TransportId={0} ", suppid));

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And l.TransitLineNumber like '%" + name.Trim() + "%' ");
            }

            if (beginid != -1)
            {
                sb.Append(string.Format(" and l.TransitBeginId={0} ", beginid));
            }

            if (endid != -1)
            {
                sb.Append(string.Format(" and l.TransitEndId={0} ", endid));
            }

            SupplierTransportLineBLL linebll = new SupplierTransportLineBLL();

            return Json(linebll.SupplierTransportLinePageList(index, size, sb.ToString()));
        }
        /// <param name="suppid">运输供应商主键</param>
        /// <param name="name">线路名称</param>
        /// <param name="beginid">开始位置</param>
        /// <param name="endid">结束位置</param>
        public int SubCounts(string suppid, string name, int beginid, int endid)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("  l.TransportId={0} ", suppid));

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And l.TransitLineNumber like '%" + name.Trim() + "%' ");
            }

            if (beginid != -1)
            {
                sb.Append(string.Format(" and l.TransitBeginId={0} ", beginid));
            }

            if (endid != -1)
            {
                sb.Append(string.Format(" and  l.TransitEndId={0} ", endid));
            }

            SupplierTransportLineBLL linebll = new SupplierTransportLineBLL();

            return linebll.SupplierTransportLinePageCount(sb.ToString());
        }
        /// <summary>
        /// 线路添加页面
        /// </summary>
        /// <param name="suppid">运输供应商主键</param>
        public ActionResult SubAdd(string suppid)
        {
            ViewBag.suppid = suppid;
            return View();
        }
        /// <summary>
        /// 线路添加功能
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public ActionResult SubAddInfo(SupplierTransportLineModel model)
        {
            model.State = 1;
            model.ArrivalTime = model.ArrivalTime ?? string.Empty;
            model.BusinessHours = model.BusinessHours ?? string.Empty;
            model.DepartureTime = model.DepartureTime ?? string.Empty;
            model.OperationTime = model.OperationTime ?? string.Empty;

            // 判断同运输供应商是否存在同线路的有效数据 
            int result = new SupplierTransportLineBLL().SupplierTransportLinePageCount(" l.State =1 AND l.TransportId=" + model.TransportId + " AND l.TransitLineNumber='" + model.TransitLineNumber + "'");
            if (result > 0)
            {
                // 系统日志
                Auxiliary.SupplierLog<SupplierTransportLineModel>(OperateEnum.Add, ResultEnum.Fail, model);
                return Json(new { flag = "exist" , content = "本运输供应商已存在此线路！"}); 
            }
            
            int suppid = new SupplierTransportLineBLL().AddSupplierTransportLine(model);

            if (suppid > 0)
            {
                // 修改使用状态为 已使用
                BLbll.ChangeUseState(model.LineId);

                Auxiliary.SupplierLog<SupplierTransportLineModel>(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }
            else
            {
                Auxiliary.SupplierLog<SupplierTransportLineModel>(OperateEnum.Add, ResultEnum.Fail, model);
                return Json(new { flag = "fail" });
            }
        }
        /// <summary>
        /// 线路编辑页面
        /// </summary>
        /// <param name="ids">线路主键</param>
        /// <param name="suppid">运输供应商主键</param>
        /// <returns></returns>
        public ActionResult SubEdit(string ids, string suppid)
        {
            ViewBag.suppid = suppid;

            SupplierTransportLineModel model = new SupplierTransportLineBLL().GetModelByID(ids);

            SysAreasBLL areabll = new SysAreasBLL();

            SysAreasModel area1Model = areabll.GetModelByID(model.TransitBeginId.ToString());

            if (area1Model != null)
            {
                model.TransitBeginName = area1Model.AreaName;
            }

            SysAreasModel area2Model = areabll.GetModelByID(model.TransitEndId.ToString());

            if (area2Model != null)
            {
                model.TransitEndName = area2Model.AreaName;
            }

            return View(model);

        }
        /// <summary>
        /// 编辑线路信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SubEditInfo(SupplierTransportLineModel model)
        {
            model.ArrivalTime = model.ArrivalTime ?? string.Empty;
            model.BusinessHours = model.BusinessHours ?? string.Empty;
            model.DepartureTime = model.DepartureTime ?? string.Empty;
            model.OperationTime = model.OperationTime ?? string.Empty;


            int suppid = new SupplierTransportLineBLL().UpdateSupplierTransportLine(model);

            if (suppid > 0)
            {

                Auxiliary.SupplierLog<SupplierTransportLineModel>(OperateEnum.Edit, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }
            else
            {
                Auxiliary.SupplierLog<SupplierTransportLineModel>(OperateEnum.Edit, ResultEnum.Fail, model);
                return Json(new { flag = "fail" });
            }
        }
        /// <summary>
        /// 作废线路
        /// </summary>
        /// <param name="ids">线路主键</param>
        public ActionResult DelSubinfo(string ids)
        {
            SupplierTransportLineModel model = new SupplierTransportLineBLL().GetModelByID(ids);

            if (model != null)
            {
                if (model.State == 10)
                {
                    return Json(new { flag = "use" });
                }
                int res = new SupplierTransportLineBLL().DeleteSupplierTransportLineByID(ids);

                if (res > 0)
                {
                    return Json(new { flag = "ok" });
                }
            }
            return Json(new { flag = "fail" });
        }
        /// <summary>
        /// 线路导入
        /// </summary>
        /// <param name="path">文件</param>
        /// <param name="suppid">运输供应商编号</param>
        /// <returns></returns>
        public ActionResult ImportInfo(string path, string suppid)
        {
            List<SysAreasModel> areaList = new SysAreasBLL().ALLAreasList();
            List<SupplierTransportLineModel> list = new List<SupplierTransportLineModel>();

            // 线路维护表
            BasisLineModel BLmodel = new BasisLineModel();

            int failcount = 0;

            // 失败内容
            string content = "第";

            using (FileStream fs = new FileStream(Server.MapPath(path), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Workbook wb = new Workbook(fs);
                Worksheet ws = wb.Worksheets[0];
                Cells cells = ws.Cells;
                int rowCounts = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, false).Rows.Count;

                int emptyRow = 0;

                for (int i = 1; i < rowCounts; i++)
                {
                    string num = ws.Cells[i, 0].StringValue.ToString();
                    string linename = ws.Cells[i, 1].StringValue.ToString();
                    string beginid = ws.Cells[i, 2].StringValue.ToString();
                    string endid = ws.Cells[i, 3].StringValue.ToString();
                    string departureTime = ws.Cells[i, 4].StringValue.ToString();
                    string arrivalTime = ws.Cells[i, 5].StringValue.ToString();
                    string businessHours = ws.Cells[i, 6].StringValue.ToString();
                    string operationTime = ws.Cells[i, 7].StringValue.ToString();

                    if (string.IsNullOrEmpty(linename) && string.IsNullOrEmpty(beginid) && string.IsNullOrEmpty(endid))
                    {
                        emptyRow++;
                        if (emptyRow == 3)
                        {
                            break;
                        }
                        continue;
                    }

                    if (string.IsNullOrEmpty(linename) || string.IsNullOrEmpty(beginid) || string.IsNullOrEmpty(endid))
                    {
                        content += i + " ";
                        failcount++;
                        continue;
                    }

                    SupplierTransportLineModel model = new SupplierTransportLineModel();
                    model.TransitLineNumber = linename;

                    // 判断线路名称在线路维护表中是否存在
                    BLmodel = new SupplierTransportLineBLL().GetModelByName(linename);

                    if (BLmodel != null)
                    {
                        // 线路ID
                        model.LineId = BLmodel.LineId;
                    }
                    else
                    {
                        content += i + " ";
                        failcount++;
                        // 跳过此数据
                        continue;
                    }

                    SysAreasModel beginarea = areaList.Where(p => p.AreaName.Contains(beginid)).FirstOrDefault();

                    if (beginarea == null)
                    {
                        content += i + " ";
                        failcount++;
                        continue;
                    }

                    SysAreasModel endmodel = areaList.Where(p => p.AreaName.Contains(endid)).FirstOrDefault();

                    if (endmodel == null)
                    {
                        content += i + " ";
                        failcount++;
                        continue;
                    }
                    model.TransportId = Convert.ToInt32(suppid);
                    model.TransitBeginId = beginarea.AreaId;
                    model.TransitEndId = endmodel.AreaId;
                    model.DepartureTime = departureTime;
                    model.ArrivalTime = arrivalTime;
                    model.BusinessHours = businessHours;
                    model.OperationTime = operationTime;
                    model.State = 1;

                    list.Add(model);

                    if (string.IsNullOrEmpty(linename) && string.IsNullOrEmpty(beginid) && string.IsNullOrEmpty(endid))
                    {
                        break;
                    }

                }
            }

            bool res = new SupplierTransportLineBLL().AddBulk(list);
            if (res)
            {
                content += "行";
                return Json(new { flag = "ok", failcount = failcount, successcount = list.Count, content });
            }
            else
            {
                return Json(new { flag = "fail", failcount = failcount, content });
            }

        }
        #endregion

        #region 评估

        /// <summary>
        /// 评估页面
        /// </summary>
        [Operate(Name = OperateEnum.Assess)]
        public ActionResult Assess(string ids, string res, string coo, int assessid, int tranid)
        {
            ViewBag.res = res;
            ViewBag.coo = coo == "0" ? "否" : "是";
            ViewBag.assessid = assessid;
            ViewBag.tranid = tranid;

            SupplierTurnoverAssessModel turassmodel = new SupplierTurnoverAssessBLL().GetModelByID(tranid.ToString());

            if (turassmodel == null)
            {
                ViewBag.turassessid = 0;
                ViewBag.potential = "";
                ViewBag.remark = "";
            }
            else
            {
                ViewBag.turassessid = turassmodel.TurnoverAssessId;
                ViewBag.potential = turassmodel.Potential;
                ViewBag.remark = turassmodel.Remark;

                List<temfile> filelist = new SupplierTurnoverAssessAdjunctBLL().SuppFileList(turassmodel.TurnoverAssessId.ToString());
                ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

                List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

                ViewBag.oldfiles = String.Join(",", oldlist);
            }

            SupplierBLL suppbll = new SupplierBLL();
            TraAssessBLL tabll = new TraAssessBLL();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(" ta.AssessFromId={0} ", assessid));
            sb.Append(string.Format(" and  ta.CompanyId={0}", Auxiliary.CompanyID()));

            List<Model.Tra.TraAssessModel> comlist = tabll.CompnentList(sb.ToString());
            List<Model.Tra.TraAssessModel> comlists = tabll.CompnentsList(sb.ToString());

            return View(new SuppModels
            {
                SuppModel = suppbll.GetModelByID(ids),
                CompnentList = comlist,
                CompnentLists = comlists
            });
        }

        /// <summary>
        /// 评估查看页面
        /// </summary>
        [Operate(Name = OperateEnum.AssessView)]
        public ActionResult AssessView(string ids, string res, string coo, int assessid, int tranid)
        {
            ViewBag.res = res;
            ViewBag.coo = coo == "0" ? "否" : "是";
            ViewBag.assessid = assessid;
            ViewBag.tranid = tranid;

            SupplierTurnoverAssessModel turassmodel = new SupplierTurnoverAssessBLL().GetModelByID(tranid.ToString());

            if (turassmodel == null)
            {
                ViewBag.turassessid = 0;
                ViewBag.potential = "";
                ViewBag.remark = "";
            }
            else
            {
                ViewBag.turassessid = turassmodel.TurnoverAssessId;
                ViewBag.potential = turassmodel.Potential;
                ViewBag.remark = turassmodel.Remark;

                List<temfile> filelist = new SupplierTurnoverAssessAdjunctBLL().SuppFileList(turassmodel.TurnoverAssessId.ToString());
                ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

                List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

                ViewBag.oldfiles = String.Join(",", oldlist);
            }

            SupplierBLL suppbll = new SupplierBLL();
            TraAssessBLL tabll = new TraAssessBLL();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(" ta.AssessFromId={0} ", assessid));
            sb.Append(string.Format(" and  ta.CompanyId={0}", Auxiliary.CompanyID()));

            List<Model.Tra.TraAssessModel> comlist = tabll.CompnentList(sb.ToString());
            List<Model.Tra.TraAssessModel> comlists = tabll.CompnentsList(sb.ToString());

            return View(new SuppModels
            {
                SuppModel = suppbll.GetModelByID(ids),
                CompnentList = comlist,
                CompnentLists = comlists
            });
        }

        public ActionResult IsHasAssess(string suppid)
        {
            string depid = Auxiliary.DepartmentId().ToString();

            int assessid = new BLL.Tra.TraAssessBLL().IsHasAssess(depid, suppid);

            if (assessid == 0)
            {
                return Json(new
                {
                    flag = "no",
                    content = "此供应商没有评估模板，不可评估！"
                });
            }

            // 判断是否有线路
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(" l.TransportId={0} ", suppid));

            int lineRow = new SupplierTransportLineBLL().SupplierTransportLinePageCount(sb.ToString());

            if (lineRow == 0)
            {
                return Json(new
                {
                    flag = "no",
                    content = "此供应商没有添加线路，不可评估！"
                });
            }

            return Json(new
            {
                flag = "ok",
                id = assessid
            });
        }

        /// <summary>
        /// 评估文件上传页面
        /// </summary>       
        public ActionResult AssessFiles(string assformid, string url)
        {
            ViewBag.url = url;//前台寻找iframe
            ViewBag.assformid = assformid;
            return View();
        }

        public ActionResult AssessScore(string id)
        {
            return Json(new SupplierTurnoverAssessBLL().GetAssessDetailScore(id));
        }

        /// <summary>
        /// 文件上传类型列表
        /// </summary>
        /// <param name="assessformid">评估编号</param>
        /// <returns></returns>
        public ActionResult AssessFileTypes(int assessformid)
        {
            return Json(new BLL.Tra.TraAssessFromAdjunctBLL().TraAssessFromAdjunctPageList(string.Format(" AssessFromId={0}", assessformid)));
        }

        /// <summary>
        /// 评估提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AssessInfo(TemAssessSave model)
        {
            SupplierTurnoverAssessModel tmodel = new SupplierTurnoverAssessModel();

            tmodel.TurnoverAssessId = model.TurnoverAssessId;
            tmodel.TransportId = model.TransportId;
            tmodel.AssessFromId = model.AssessFromId;
            tmodel.CompanyId = Auxiliary.CompanyID();
            tmodel.CreateDepartmentId = Auxiliary.DepartmentId();
            tmodel.CreateTime = DateTime.Now;
            tmodel.CreateUserId = Auxiliary.UserID();

            tmodel.Potential = model.Potential ?? string.Empty;
            tmodel.Remark = model.Remark ?? string.Empty;
            tmodel.StandardScore = model.StandardScore;

            tmodel.ValueScore = 0;

            tmodel.TurnoverAssessNumber = Auxiliary.CurCompanyAutoNum("TAN");
            tmodel.State = 1;
            tmodel.AssessResult = 0;

            int minscore = new BLL.Tra.TraAssessBLL().AssessMinScore(model.AssessFromId);

            List<SupplierTurnoverAssessAdjunctModel> list2 = new List<SupplierTurnoverAssessAdjunctModel>();

            if (!string.IsNullOrEmpty(model.SuppFileList))
            {
                List<temfile> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfile>>(model.SuppFileList);

                foreach (var item in fflist)
                {
                    SupplierTurnoverAssessAdjunctModel filemodel = new SupplierTurnoverAssessAdjunctModel();

                    filemodel.FileName = item.filename + item.ext;
                    filemodel.FileUrl = item.path;
                    filemodel.State = 1;
                    filemodel.TurnoverAssessId = model.TurnoverAssessId;
                    list2.Add(filemodel);
                }
            }


            List<SupplierTurnoverAssessContentModel> list1 = new List<SupplierTurnoverAssessContentModel>();
            if (!string.IsNullOrEmpty(model.ScoreList))
            {
                List<AssessDetails> sslist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AssessDetails>>(model.ScoreList);
                foreach (var item in sslist)
                {
                    SupplierTurnoverAssessContentModel scoremodel = new SupplierTurnoverAssessContentModel();
                    scoremodel.ComponentDetailId = item.id;
                    scoremodel.value = item.score;
                    scoremodel.TurnoverAssessId = model.TurnoverAssessId;
                    list1.Add(scoremodel);
                }
            }

            int asscore = list1.Sum(p => p.value);
            tmodel.AssessScore = asscore.ToString();
            if (asscore >= minscore)
            {
                tmodel.AssessResult = 1;
                bll.QualifiedResult(model.TransportId, asscore, tmodel.AssessResult);
            }
            else
            {
                tmodel.AssessResult = 0;
                bll.UnqualifiedResult(model.TransportId, asscore, tmodel.AssessResult);
            }

            int res = 0;
            if (model.TurnoverAssessId == 0) //添加
            {
                res = new SupplierTurnoverAssessBLL().AddAllInfo(tmodel, list1, list2);
            }
            else
            {
                res = new SupplierTurnoverAssessBLL().UpdateAllInfo(tmodel, list1, list2);
            }

            if (res == 0)
            {
                model.Type = "添加";
                Auxiliary.SupplierLog<TemAssessSave>(OperateEnum.Assess, ResultEnum.Fail, model);
                return Json(new { flag = "fail" });

            }
            else
            {
                model.Type = "修改";
                Auxiliary.SupplierLog<TemAssessSave>(OperateEnum.Assess, ResultEnum.Fail, model);
                return Json(new { flag = "ok" });
            }
        }
        #endregion
    }

    #region 评估辅助类
    public class SuppModels
    {
        public SupplierModel SuppModel { get; set; }

        public List<Model.Tra.TraAssessModel> CompnentList { get; set; }
        public List<Model.Tra.TraAssessModel> CompnentLists { get; set; }
    }
    #endregion
}
