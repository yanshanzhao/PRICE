//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-20    1.0        zbb        新建               
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
using SRM.BLL.Sys;
using Newtonsoft.Json.Converters;
using SRM.Model.Sys;
#endregion
/*********************************
 * 类名：TraNotificationCheckController
 * 功能描述：招标通知 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraNotificationCheckController : Controller
    {
        //
        // GET: /Tra/TraNotificationCheck/

        //运作要求维护
        TraNotificationCheckBLL bll = new TraNotificationCheckBLL();

        //运作附件
        TraNotificationDetailBLL tcbll = new TraNotificationDetailBLL();


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
            TraNotificationCheckModel model = bll.GetModelByID(tId);
            //获取附件信息
            List<temfileNotification> filelist = tcbll.NotificationFileList(model.NotificationId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);
            return View(model);
        }
        #region 方法

        #region 数据集 招标通知表

        /// <summary>
        /// 数据集 招标通知表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="beginTime">招标开始时间</param>
        /// <param name="endTime">招标结束时间</param> 
        /// <param name="notificationLines">招标路线</param>
        /// <returns></returns>
        public ActionResult TraNotificationCheckList(int index, int size, string beginTime, string endTime, string notificationLines)
        {
            string where = string.Format(" (TN.NotificationState = 1 OR TN.NotificationState = 10) And NotifyState=1 And TSM.UserId={0}", Auxiliary.UserID());
             
            // 运作时间 
            if (!string.IsNullOrEmpty(beginTime))
            {
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += string.Format(" And TN.CreateTime BE TWEEN '{0}' AND '{1}' ", beginTime.Trim(), endTime.Trim());
                }
            }

            // 招标路线
            if (!string.IsNullOrEmpty(notificationLines))
            {
                where += string.Format(" And TN.NotificationLines  LIKE '%{0}%'", notificationLines.Trim());
            }

            List<TraNotificationCheckModel> list = bll.TraNotificationCheckList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region 数据记录数 招标通知表

        /// <summary>
        /// 数据记录数 招标通知表
        /// </summary> 
        /// <param name="beginTime">招标开始时间</param>
        /// <param name="endTime">招标结束时间</param> 
        /// <param name="notificationLines">招标路线</param>
        /// <returns></returns>
        public int TraNotificationCheckCount(string beginTime, string endTime, string notificationLines)
        {
            string where = string.Format(" (TN.NotificationState = 1 OR TN.NotificationState = 10) And NotifyState=1 And TSM.UserId={0}", Auxiliary.UserID());

            // 运作时间 
            if (!string.IsNullOrEmpty(beginTime))
            {
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += string.Format(" And TN.CreateTime BETWEEN '{0}' AND '{1}' ", beginTime.Trim(), endTime.Trim());
                }
            }

            // 招标路线
            if (!string.IsNullOrEmpty(notificationLines))
            {
                where += string.Format(" And TN.NotificationLines  LIKE '%{0}%'", notificationLines.Trim());
            }

            return bll.TraNotificationCheckCount(where);
        }
        #endregion

        #endregion
    }
}
