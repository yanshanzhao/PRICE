//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-10    1.0        MY        新建              
//-------------------------------------------------------------------------
using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*********************************
 * 类名：TraNotifyCheckInfoController
 * 功能描述：招标通知 控制器 
 * ******************************/
namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class TraNotifyCheckInfoController : Controller
    {
        //
        // GET: /SupplierQuery/TraNotifyCheckInfo/

        private BLL.Tra.BusinessQueryBLL bll = new BLL.Tra.BusinessQueryBLL();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            int dId = Auxiliary.DepartmentId();
            Model.Tra.TraChooseModel model = bll.GetModelByTraChooseId(tId,dId);
            
            return View(model);
        }

        

        #region 招标通知 方法
        /// <summary>
        /// 招标通知 结果集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="notifyLines"></param>
        /// <returns></returns>
        [HttpPost]
        [Operate(Name = OperateEnum.Search)]
        public ActionResult Index(int index, int size, string beginTime, string endTime, string notifyLines)
        {
            string where = string.Empty;
            where = " And TN.NotificationState in (1,10) And SD.DepartmentId = " + Auxiliary.DepartmentId();
            if (!string.IsNullOrEmpty(beginTime))
            {
                where += string.Format(" And TN.NotificationBeginTime = '{0}'", beginTime.Trim());
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                where += string.Format(" And TN.NotificationEndTime = '{0}'", endTime.Trim());
            }
            if (!string.IsNullOrEmpty(notifyLines))
            {
                where += string.Format(" And TN.NotificationLines = '{0}'", notifyLines.Trim());
            }
            List<Model.Tra.TraNotificationModel> list = bll.TraNotificationList(index, size, where);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 招标通知 结果集的数量
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="notifyLines"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexAmount(string beginTime, string endTime, string notifyLines)
        {
            string where = string.Empty;
            where = " And TN.NotificationState in (1,10) And SD.DepartmentId = " + Auxiliary.DepartmentId();
            if (!string.IsNullOrEmpty(beginTime))
            {
                where += string.Format(" And TN.NotificationBeginTime = '{0}'", beginTime.Trim());
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                where += string.Format(" And TN.NotificationEndTime = '{0}'", endTime.Trim());
            }
            if (!string.IsNullOrEmpty(notifyLines))
            {
                where += string.Format(" And TN.NotificationLines = '{0}'", notifyLines.Trim());
            }

            return Content(bll.TraNotifyCount(where).ToString());
        }


        #endregion

        #region 查看 方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="tId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckList(int index,int size, int tId)
        {
            string where = string.Empty;

            where += " And TN.ChooseId = " + tId;
            List<Model.Tra.TraNotificationModel> list = bll.GetTraNotificationList(index, size, where);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public ActionResult CheckAmount(int tId)
        {
            string where = string.Empty;

            where += " And ChooseId = " + tId;

            return Content(bll.GetTraNotificationAmount(where).ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AdjunctList(int tId)
        {
            List<Model.Tra.TraChooseModel> list = bll.TraChooseAdjunctList(tId);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        #endregion


    }
}
