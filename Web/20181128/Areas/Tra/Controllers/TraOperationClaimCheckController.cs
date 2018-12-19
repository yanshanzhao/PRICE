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
 * 类名：TraOperationClaimCheckController
 * 功能描述：运作要求维护 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraOperationClaimCheckController : Controller
    {
        //
        // GET: /Tra/TraOperationClaimCheck/

        //运作要求维护
        TraOperationClaimCheckBLL bll = new TraOperationClaimCheckBLL();

        //运作附件
        TraOperationeAdjunctBLL tcbll = new TraOperationeAdjunctBLL();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            TraOperationClaimCheckModel model = bll.GetModelByID(tId);

            //获取附件信息
            List<temfileClaim> filelist = tcbll.SuppFileList(model.OperationClaimId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        #region 方法

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
        public ActionResult TraOperationClaimCheckList(int index, int size, string theme, string beginTimes, string endTimes, string state, string createTime)
        {
            string where = string.Format(" toc.State = 1 and TSM.UserId={0}", Auxiliary.UserID());

            // 运作主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And toc.Theme like '%{0}%'", theme.Trim());
            } 

            // 运作时间 
            if (!string.IsNullOrEmpty(beginTimes))
            {
                if (!string.IsNullOrEmpty(endTimes))
                {
                    where += string.Format(" And toc.CreateTime BETWEEN '{0}' AND '{1}' ", beginTimes.Trim(), endTimes.Trim());
                }
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

            List<TraOperationClaimCheckModel> list = bll.TraOperationClaimCheckList(index, size, where);

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
        public int TraOperationClaimCheckCount(string theme, string beginTimes, string endTimes, string state, string createTime)
        {
            string where = string.Format(" toc.State = 1 and TSM.UserId={0}", Auxiliary.UserID());

            // 运作主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And toc.Theme like '%{0}%'", theme.Trim());
            }
             
            // 运作时间 
            if (!string.IsNullOrEmpty(beginTimes))
            {
                if (!string.IsNullOrEmpty(endTimes))
                {
                    where += string.Format(" And toc.CreateTime BETWEEN '{0}' AND '{1}' ", beginTimes.Trim(), endTimes.Trim());
                }
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
            return bll.TraOperationClaimCheckCount(where);
        }
        #endregion
         
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
        public ActionResult OperationClaimCheckAllotList(int index, int size, int operationClaimId, string supplierName, string state)
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

            List<TraOperationClaimCheckModel> list = bll.OperationClaimCheckAllotList(index, size, where);

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
        public int OperationClaimCheckAllotCount(int operationClaimId, string supplierName, string state)
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

            return bll.OperationClaimCheckAllotCount(where);
        }
        #endregion

        #endregion
    }
}
