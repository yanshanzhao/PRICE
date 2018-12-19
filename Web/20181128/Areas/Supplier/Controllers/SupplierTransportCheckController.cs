//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-20    1.0        MH         新建               
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
#endregion
/*********************************
 * 类名：SupplierTransportCheckController
 * 功能描述：运输供应商审核 控制器 
 * ******************************/
namespace SRM.Web.Areas.Supplier.Controllers
{
    public class SupplierTransportCheckController : Controller
    {
        SupplierTransportCheckBLL bll = new SupplierTransportCheckBLL();
        SupplierBLL basebll = new SupplierBLL();
        SupplierTransportBLL tranbll = new SupplierTransportBLL();
        SupplierAdjunctBLL sabll = new SupplierAdjunctBLL();
        SupplierAuditsBLL checkbll = new SupplierAuditsBLL();

        public ActionResult Index()
        {
            ViewBag.UserId = Auxiliary.UserID();
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
        public ActionResult List(int index, int size, string num, string name, string line, int suppclass, int suppscope, int supplevel, string suppstate, int state, string starttime, string endtime,int depid)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append(string.Format("  ts.CompanyId={0} and ts.DepartmentId={1} ", Auxiliary.CompanyID(), Auxiliary.DepartmentId()));
            sb.Append(string.Format("  sa.CompanyId={0} and sa.AuditDepartmentId={1} ", Auxiliary.CompanyID(), Auxiliary.DepartmentId()));
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
                sb.Append(string.Format(" And sa.State={0}  ", state));
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sb.Append("  And sa.PresentTime>'" + starttime + "' ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sb.Append(" And sa.PresentTime<'" + Convert.ToDateTime(endtime).ToString("yyyy-MM-dd") + "' ");
            }

            if (depid != -1)
            {
                sb.Append(string.Format(" And sd.DepartmentId={0}  ", depid));
            }

            List<SupplierTransportCheckModel> list = bll.SupplierTransportPageList(index, size, sb.ToString());

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
        public int Count(string num, string name, string line, int suppclass, int suppscope, int supplevel, string suppstate, int state, string starttime, string endtime,int depid)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append(string.Format("  ts.CompanyId={0} and ts.DepartmentId={1} ", Auxiliary.CompanyID(), Auxiliary.DepartmentId()));
            sb.Append(string.Format("  sa.CompanyId={0} and sa.AuditDepartmentId={1} ", Auxiliary.CompanyID(), Auxiliary.DepartmentId()));
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
                sb.Append(string.Format(" And sa.State={0}  ", state));
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sb.Append("  And sa.PresentTime>'" + starttime + "' ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sb.Append(" And sa.PresentTime<'" + Convert.ToDateTime(endtime).ToString("yyyy-MM-dd") + "' ");
            }
            if (depid != -1)
            {
                sb.Append(string.Format(" And sd.DepartmentId={0}  ", depid));
            }
            //   sb.Append(" And ts.State!=20 ");

            return bll.SupplierTransportPageCount(sb.ToString());
        }

        /// <summary>
        /// 查看页面
        /// </summary>
        /// <param name="ids">审核主键</param>
        [Operate(Name =OperateEnum.View)]
        public ActionResult Views(string ids)
        {
            SupplierAuditsModel checkmodel = checkbll.GetModelByID(ids);

            ViewBag.remark = checkmodel.AuditRemark ?? string.Empty;
            ViewBag.checkid = ids;
            SupplierTransportModel model = tranbll.GetModelByID(checkmodel.PresentId.ToString());

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

            List<temfile> filelist = sabll.SuppFileList(checkmodel.PresentId, 20);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 审核页面
        /// </summary>
        /// <param name="ids">审核主键</param>
        [Operate(Name =OperateEnum.Check)]
        public ActionResult Check(string ids)
        {
            SupplierAuditsModel checkmodel = checkbll.GetModelByID(ids);

            ViewBag.remark = checkmodel.AuditRemark ?? string.Empty;
            ViewBag.checkid = ids;
            SupplierTransportModel model = tranbll.GetModelByID(checkmodel.PresentId.ToString());

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

            List<temfile> filelist = sabll.SuppFileList(checkmodel.PresentId, 20);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="id">审核主键</param>
        /// <param name="remark">审核意见</param>
        public ActionResult Pass(string id,string remark)
        {
            // 审核前Model
            SupplierAuditsModel checkmodel = checkbll.GetModelByID(id);
                                                 
            if (checkmodel != null)
            {
                // 审核流程Model
                BasisAuditRelationModel BARmodel = Auxiliary.GetAuditRelationById(checkmodel.AuditRelationId);

                // 判断是否有下一级审核
                BasisAuditRelationModel model = Auxiliary.IsRelationByBeforeId(checkmodel.AuditRelationId);

                // 若有 新增审核信息
                if (model != null)
                {
                    // 审核表中的审核状态
                    int res = checkbll.ChangeState(Convert.ToInt32(id), 1, remark);

                    SupplierAuditModel Auditmodel = new SupplierAuditModel();
                    Auditmodel.AuditRelationNumber = model.AuditRelationNumber;
                    Auditmodel.AuditRelationId = model.AuditRelationId;

                    Auditmodel.OtherId = checkmodel.OtherId;
                    Auditmodel.PresentId = checkmodel.PresentId;
                    Auditmodel.SupplierAuditType = checkmodel.SupplierAuditType;

                    Auditmodel.PresentDepartmentId = model.DepartmentId;
                    Auditmodel.PresentUserId = model.UserId;
                    Auditmodel.AuditDepartmentId = model.ToDepartmentId;
                    Auditmodel.AuditUserId = model.ToUserId;
                    Auditmodel.AuditRelationName = model.AuditRelationName;
                    Auditmodel.CompanyId = model.CompanyId;

                    // 默认状态 未审核
                    Auditmodel.State = 0;

                    // 上一审核ID
                    Auditmodel.BeforeId = checkmodel.SupplierAuditId;

                    SupplierAuditBLL auditBLL = new SupplierAuditBLL();
                    auditBLL.AddAuditRelation(Auditmodel);

                    // 供应商日志 
                    Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "运输供应商审核审核通过", Id = id, Memo = remark ?? string.Empty });
                    return Json(new { flag = "ok" });
                }
                else
                {
                    // 当前审核流程是否为结束审核流程
                    // 若否
                    if (BARmodel.EndAudit == 0)
                    {
                        // 供应商日志

                        Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Fail, new { Type = "审核流程不完善", Id = id, Memo = remark ?? string.Empty });
                        return Json(new { flag = "fail" , content = "无下一级审核人，请完善审核流程！"}); 
                    }
                    else if (BARmodel.EndAudit == 1)
                    {
                        // 审核表中的审核状态
                        int res = checkbll.ChangeState(Convert.ToInt32(id), 1, remark);
                         
                        // 供应商状态改为合格
                        tranbll.UpdateSuppTranStates(checkmodel.PresentId.ToString(), "F2");
                         
                        // 供应商日志
                        Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "运输供应商审核审核通过", Id = id, Memo = remark ?? string.Empty });
                        return Json(new { flag = "ok" });
                    }
                }
                 
            }

            Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Fail, new { Type = "运输供应商审核审核失败", Id = id, Memo = remark ?? string.Empty });
            return Json(new { flag = "fail", content = "审核失败！" });
        }

        /// <summary>
        /// 驳回操作
        /// </summary>
        /// <param name="id">审核主键</param>
        /// <param name="remark">审核意见</param>
        public ActionResult Reject(string id,string remark)
        {
            SupplierAuditsModel checkmodel = checkbll.GetModelByID(id);

            if (checkmodel != null)
            {
                tranbll.UpdateSuppTranStates(checkmodel.PresentId.ToString(), "F3");

                tranbll.UpdateSuppState(checkmodel.PresentId.ToString(), 10);

                int res = checkbll.ChangeState(Convert.ToInt32(id), 4, remark);
                 
                if (res > 0)
                {
                    Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Sucess, new { Type = "运输供应商驳回", Id = id, Memo = remark ?? string.Empty });
                    return Json(new { flag = "ok" });
                }
            }

            Auxiliary.SupplierCustomLog(OperateEnum.Check, ResultEnum.Fail, new { Type = "运输供应商驳回", Id = id, Memo = remark ?? string.Empty });
            return Json(new { flag = "fail" });
        }
        /// <summary>
        /// 撤销操作
        /// </summary>
        /// <param name="id">审核主键</param>
        /// <param name="remark">审核意见</param>
        [Operate(Name =OperateEnum.Revoke)]
        public ActionResult Revoke(string id)
        {
            SupplierAuditsModel checkmodel = checkbll.GetModelByID(id);

            if (checkmodel != null)
            {
                tranbll.UpdateSuppTranStates(checkmodel.PresentId.ToString(), "F1");

                int res = checkbll.ClearSuppState(Convert.ToInt32(id));

                if (res > 0)
                {
                    Auxiliary.SupplierCustomLog(OperateEnum.Revoke, ResultEnum.Sucess, new { Type = "运输供应商撤销", Id = id, Memo = "" });
                    return Json(new { flag = "ok" });
                }
            }

            Auxiliary.SupplierCustomLog(OperateEnum.Revoke, ResultEnum.Fail, new { Type = "运输供应商撤销", Id = id, Memo =""});
            return Json(new { flag = "fail" });
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
        [Operate(Name =OperateEnum.Export)]
        public ActionResult Export(string num, string name, string line, int suppclass, int suppscope, int supplevel, string suppstate, int state, string starttime, string endtime, int depid)
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
                sb.Append(string.Format(" And sa.State={0}  ", state));
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sb.Append("  And sa.PresentTime>'" + starttime + "' ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sb.Append(" And sa.PresentTime<'" + Convert.ToDateTime(endtime).ToString("yyyy-MM-dd") + "' ");
            }

            if (depid != -1)
            {
                sb.Append(string.Format(" And sd.DepartmentId={0}  ", depid));
            }

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
    }
}
