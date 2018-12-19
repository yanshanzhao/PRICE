//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-14    1.0        HDS         新建               
//-------------------------------------------------------------------------
#region using 
using Newtonsoft.Json.Converters;
using SRM.BLL.Basis;
using SRM.BLL.Supplier;
using SRM.BLL.Tra;
using SRM.Model.Basis;
using SRM.Model.Supplier;
using SRM.Model.Tra;
using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
#endregion
/*********************************
 * 类名：TraDistributorAdjustExamineController
 * 功能描述：配送变更审核 控制器 
 * ******************************/
namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraDistributorAdjustExamineController : Controller
    {
        //
        // GET: /Tra/TraDistributorAdjustExamine/

        #region 成员变量
        TraDistributorAdjustExamineBLL bll = new TraDistributorAdjustExamineBLL();
        TraDistributorBLL adjusetbll    = new TraDistributorBLL();// 配送人员
        TraDistributorAdjustDetailBLL detailBll = new TraDistributorAdjustDetailBLL();//调整明细
        TraDistributorAdjustBLL butorBll = new TraDistributorAdjustBLL();//调整 
        SupplierAuditsBLL checkbll = new SupplierAuditsBLL();//供应商审核
        BasisAuditRelationBLL basisBll = new BasisAuditRelationBLL();//流程

        #endregion

        #region 首页
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        #region 查询       
        /// <summary>
        /// 查询配送人员变更行数
        /// </summary>
        /// <param name="starttime">申请开始时间</param>
        /// <param name="endtime">申请结束时间</param> 
        /// <param name="lineName">线路名称</param>
        /// <param name="auditstarttime">审核开始时间</param>
        /// <param name="auditendtime">审核结束时间</param> 
        /// <param name="state">状态</param>
        /// <returns></returns>
        public int RowCount(string starttime, string endtime, string lineName, string auditstarttime, string auditendtime, int state)
        {
            StringBuilder sbStr = new StringBuilder();
            sbStr.Append(" where  b.AuditDepartmentId=" + Auxiliary.DepartmentId());//只允许查询本机构
            sbStr.Append(" and b.STATE<=4  ");//有效的信息
            if (!string.IsNullOrEmpty(starttime))//申请开始时间
            {
                sbStr.Append("and a.CreateTime>='" + starttime + "'");
            }
            if (!string.IsNullOrEmpty(endtime))//申请结束时间
            {
                sbStr.Append("and a.CreateTime<='" + endtime + "'");
            } 
            if (!string.IsNullOrEmpty(lineName))//线路名称
            {
                sbStr.Append("and f.TransitLineNumber like '%" + lineName + "%'");
            }
            if (!string.IsNullOrEmpty(auditstarttime))//审核开始时间
            {
                sbStr.Append("and b.AuditTime>='" + auditstarttime + "'");
            }
            if (!string.IsNullOrEmpty(auditendtime))//审核结束时间
            {
                sbStr.Append("and b.AuditTime<='" + auditendtime + "'");
            }
            if (state != -1)//状态
            {
                sbStr.Append("and b.STATE='" + state + "'");
            }
            return bll.TraDistributorAdjustExaminePageCount(sbStr.ToString());
        }

        /// <summary>
        /// 查询配送人员变更信息
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="starttime">申请开始时间</param>
        /// <param name="endtime">申请结束时间</param> 
        /// <param name="lineName">线路名称</param>
        /// <param name="auditstarttime">审核开始时间</param>
        /// <param name="auditendtime">审核结束时间</param> 
        /// <param name="state">状态</param> 
        /// <returns></returns>
        public ActionResult list(int index,int size, string starttime, string endtime, string lineName, string auditstarttime, string auditendtime, int state)
        {
            StringBuilder sbStr = new StringBuilder();
            sbStr.Append(" where  b.AuditDepartmentId=" + Auxiliary.DepartmentId());//只允许查询本机构
            sbStr.Append(" and b.STATE<=4  ");//有效的信息
            if (!string.IsNullOrEmpty(starttime))//申请开始时间
            {
                sbStr.Append("and a.CreateTime>='" + starttime + "'");
            }
            if (!string.IsNullOrEmpty(endtime))//申请结束时间
            {
                sbStr.Append("and a.CreateTime<='" + endtime + "'");
            }
            if (!string.IsNullOrEmpty(lineName))//线路名称
            {
                sbStr.Append("and f.TransitLineNumber like '%" + lineName + "%'");
            }
            if (!string.IsNullOrEmpty(auditstarttime))//审核开始时间
            {
                sbStr.Append("and b.AuditTime>='" + auditstarttime + "'");
            }
            if (!string.IsNullOrEmpty(auditendtime))//审核结束时间
            {
                sbStr.Append("and b.AuditTime<='" + auditendtime + "'");
            }
            if (state != -1)//状态
            {
                sbStr.Append("and b.STATE='" + state + "'");
            }
            List<TraDistributorAdjustExamineModel> listModel = bll.TraDistributorAdjustExaminePageList(index, size, sbStr.ToString());
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();//定义显示时间样式
            timeFormat.DateTimeFormat = "yyyy-MM-dd";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(listModel, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 查看
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="presentId"> 配送人员变更id</param>
        /// <param name="auditId">供应商审核id</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int presentId, int auditId)
        {
            // 获取配送人员的进步信息；
            string where = " DistributorUserId in (select DistributorUserId from TraDistributorAdjust where UserAdjustId=" + presentId + " ) ";
            TraDistributorModel model = adjusetbll.GetModelByWhere(where);
            SupplierAuditsModel checkModel = checkbll.GetModelByID(auditId.ToString()); 
            ViewBag.SupplierName = model.TransportSupplierName;//供应商名称
            ViewBag.DistributionScopeName = model.DistributionScopeName;//配送范围
            ViewBag.LineName = model.LineName;//线路名称
            ViewBag.AuditRemark = checkModel.AuditRemark;//审核意见
            ViewBag.presentId = presentId;
            return View();
        }
        #endregion

        #region 审核
        /// <summary> 
        /// 审核操作
        /// </summary>
        /// <param name="presentId"> 配送人员变更id</param>
        /// <param name="auditId">供应商审核id</param>
        /// <returns></returns>
        [Operate(Name =OperateEnum.Check)]
       
        public ActionResult Check(int presentId,int auditId)
        {
            // 获取配送人员的基本信息；
            string where = " DistributorUserId in (select DistributorUserId from TraDistributorAdjust where UserAdjustId=" + presentId + " ) ";
            TraDistributorModel model = adjusetbll.GetModelByWhere(where);
            //获取调整信息
            //List<TraDistributorAdjustDetailModel> lisetModel = detailBll.ListModel(presentId);
            ViewBag.auditId = auditId;//调整id
            ViewBag.SupplierName = model.TransportSupplierName;//供应商名称
            ViewBag.DistributionScopeName = model.DistributionScopeName;//配送范围
            ViewBag.LineName = model.LineName;//线路名称
            ViewBag.presentId = presentId;//配送人员变更id
            return View();

        }
        public ActionResult TraDistributorAdjustDetailModelList(int presentId)
        {
            //获取调整信息
            List<TraDistributorAdjustDetailModel> lisetModel = detailBll.ListModel(presentId); 
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(lisetModel)); 
            
        }




        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="adjustId">审核id</param>
        /// <param name="auditRemark">审核意见</param>
        /// <param name="presentId"> 配送人员变更id</param>
        /// <returns></returns>
        public ActionResult ThroughAudit(int adjustId,string auditRemark)
        {
            string flagStr = "";//处理结果
            string contentStr = "";//处理解释
            ResultEnum enumStr = ResultEnum.Fail;//处理失败
            //SupplierAuditModel checkModel = checkbll.GetModelByID(adjustId);// 获取审核的实体 
            SupplierAuditsModel checkModel = checkbll.GetModelByID(adjustId.ToString());
            if (checkModel != null)
            {              
                int res = checkbll.ChangeState(adjustId, 1, auditRemark);//更改审核表
                if (res > 0)
                {
                    //获取本次审核是否为流程结束点
                    BasisAuditRelationModel basisModel = basisBll.GetModelByID(checkModel.AuditRelationId); 
                    if (basisModel.EndAudit == 1)//流程结束 更改配送表信息
                    {
                        #region 审核流程结束
                        //获取调整配送人员表
                        TraDistributorAdjustModel model = butorBll.GetModelById(checkModel.OtherId);
                        //更改配送人员表
                        if (adjusetbll.CheckTraDistribuor(model.DistributorUserId, model.UserAdjustId) > 0)
                        { 
                            butorBll.UpdateDistributorAdjustState(5, checkModel.OtherId);
                            flagStr = "success";
                            contentStr = "配送人员信息审核通过";
                            enumStr = ResultEnum.Sucess;
                        }
                        else
                        {
                            flagStr = "fail";
                            contentStr = "审核异常";
                        }
                        #endregion 
                    }
                    else //流程未结束  继续操作供应商审核表
                    {
                        #region 审核流程未结束 
                        //获取下一流程信息
                        BasisAuditRelationModel toBasisModel = basisBll.IsRelationByBeforeId(basisModel.AuditRelationId);
                        if (toBasisModel != null)//有下一流程
                        {
                            #region 填充下一个流程信息 
                            //添加供应商审核信息
                            SupplierAuditsModel addModel = new SupplierAuditsModel();
                            addModel.AuditRelationNumber = toBasisModel.AuditRelationNumber;
                            addModel.AuditRelationId = toBasisModel.AuditRelationId;
                            addModel.OtherId = checkModel.OtherId;
                            addModel.PresentId = checkModel.PresentId;
                            addModel.SupplierAuditType = checkModel.SupplierAuditType;
                            addModel.PresentDepartmentId = toBasisModel.DepartmentId;
                            addModel.PresentUserId = toBasisModel.UserId;
                            addModel.PresentTime = DateTime.Now;//当前时间
                            addModel.AuditDepartmentId = toBasisModel.ToDepartmentId;
                            addModel.AuditUserId = toBasisModel.ToUserId;
                            addModel.AuditRelationName = toBasisModel.AuditRelationName;
                            addModel.State =0;//未审核
                            addModel.CompanyId = toBasisModel.CompanyId;
                            addModel.BeforeId= checkModel.SupplierAuditId; 
                            if(checkbll.AddSupplierAuditsNew(addModel)>0)//添加下一审核流程成功
                            {
                                flagStr = "success";
                                contentStr = "配送人员信息审核通过";
                                enumStr = ResultEnum.Sucess;
                            }
                            else
                            {
                                flagStr = "fail";
                                contentStr = "审核异常";
                            }
                            #endregion
                        }
                        else //和审核通过流程一致
                        {
                            #region 无下一流程  默认为审核结束
                            //获取调整配送人员表
                            TraDistributorAdjustModel model = butorBll.GetModelById(checkModel.OtherId);
                            //更改配送人员表
                            if (adjusetbll.CheckTraDistribuor(model.DistributorUserId, model.UserAdjustId) > 0)
                            {
                                butorBll.UpdateDistributorAdjustState(5, checkModel.OtherId);
                                flagStr = "success";
                                contentStr = "配送人员信息审核通过";
                                enumStr = ResultEnum.Sucess;
                            }
                            else
                            {
                                flagStr = "fail";
                                contentStr = "审核异常";
                            }
                            #endregion

                        } 
                        #endregion
                    }
                }
                else
                {
                    flagStr = "fail";
                    contentStr = "审核异常";
                }
            }else
            {
                flagStr = "fail";
                contentStr = "审核异常";
            }
            //记录日志
            Auxiliary.SupplierCustomLog(OperateEnum.Check, enumStr, new { Type = "配送变更审核", Id = adjustId, Memo = auditRemark ?? string.Empty });
            //返回结果
            return Json(new { flag = flagStr, content= contentStr });
        }


        /// <summary>
        /// 驳回
        /// </summary>
        /// <param name="adjustId">审核id</param>
        /// <param name="auditRemark">审核意见</param>
        /// <returns></returns>
        public ActionResult RejectAudit(int adjustId, string auditRemark)
        {
            string flagStr = "";//处理结果
            string contentStr = "";//处理解释
            ResultEnum enumStr = ResultEnum.Fail;//处理失败
            SupplierAuditsModel checkModel = checkbll.GetModelByID(adjustId.ToString());
            if (checkModel != null)
            {
                int res = checkbll.ChangeState(adjustId, 4, auditRemark);//更改审核表
                if (res > 0)
                { 
                    //获取本次审核撤销类型 都默认为审核结束
                    BasisAuditRelationModel basisModel = basisBll.GetModelByID(checkModel.AuditRelationId);
                    #region 审核不通过流程结束   
                    butorBll.UpdateDistributorAdjustState(30, checkModel.OtherId);  
                    #endregion
                    flagStr = "success";
                    enumStr = ResultEnum.Sucess;
                    contentStr = "驳回成功！";
                }
                else
                {
                    flagStr = "fail";
                    contentStr = "驳回异常！";
                }
            }
            else
            {
                flagStr = "fail";
                contentStr = "驳回异常！";
            }
            //记录日志
            Auxiliary.SupplierCustomLog(OperateEnum.Reject, enumStr, new { Type = "配送变更驳回", Id = adjustId, Memo = auditRemark ?? string.Empty });
            //返回结果
            return Json(new { flag = flagStr, content = contentStr });
        }


        [Operate(Name = OperateEnum.Revoke)]
        /// <summary>
        /// 撤销审核
        /// </summary>
        /// <param name="ids">供应商审核id</param>
        /// <returns></returns>
        public ActionResult RemoveCheck(int ids)
        {
            string flagStr = "fail";//处理结果 默认失败
            string contentStr = "";//处理解释
            ResultEnum enumStr = ResultEnum.Fail;//处理失败   
            int typeInt = butorBll.RemoveCheck(ids);
            if(typeInt==-1)
            {
                contentStr = "撤销异常";

            } else if(typeInt==0 || typeInt==10)
            {
                contentStr = "本次审核为审核结束节点，无法撤销！";
            }
            else if (typeInt == 20)
            {
                contentStr = "下一审核流程已进行审核操作，无法撤销！";
            }
            else 
            { 
				flagStr = "success";
				enumStr = ResultEnum.Sucess; 
                contentStr = "成功撤销！";
            }
            //记录日志
            Auxiliary.SupplierCustomLog(OperateEnum.Revoke, enumStr, new { Type = "配送变更撤销", Id = ids });
            //返回结果
            return Json(new { flag = flagStr, content = contentStr });
        } 
        #endregion

        #region 导出
        public ActionResult Export(string starttime, string endtime, string lineName, string auditstarttime, string auditendtime, int state)
        {
            StringBuilder sbStr = new StringBuilder();
            sbStr.Append(" where  b.AuditDepartmentId=" + Auxiliary.DepartmentId());//只允许查询本机构
            sbStr.Append(" and b.STATE<=4  ");//有效的信息
            if (!string.IsNullOrEmpty(starttime))//申请开始时间
            {
                sbStr.Append("and a.CreateTime>='" + starttime + "'");
            }
            if (!string.IsNullOrEmpty(endtime))//申请结束时间
            {
                sbStr.Append("and a.CreateTime<='" + endtime + "'");
            }
            if (!string.IsNullOrEmpty(lineName))//线路名称
            {
                sbStr.Append("and f.TransitLineNumber like '%" + lineName + "%'");
            }
            if (!string.IsNullOrEmpty(auditstarttime))//审核开始时间
            {
                sbStr.Append("and b.AuditTime>='" + auditstarttime + "'");
            }
            if (!string.IsNullOrEmpty(auditendtime))//审核结束时间
            {
                sbStr.Append("and b.AuditTime<='" + auditendtime + "'");
            }
            if (state != -1)//状态
            {
                sbStr.Append("and b.STATE='" + state + "'");
            }
            DataTable dt = bll.ExportDataTable(sbStr.ToString());
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);
            return Json(new { flag = "ok", guid = url });

        }
        #endregion




    }
}
