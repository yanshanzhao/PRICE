//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-13    1.0        FJK        新建              
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.Model.Supplier;
using System.Linq;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using SRM.BLL.Supplier;
using System.Text;
#endregion
/*********************************
 * 类名：TraMonthCheckValueController
 * 功能描述：月考核取值 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraMonthCheckValueController : Controller
    {
        //
        // GET: /Tra/TraMonthCheckValue/

        // 月考核取值BLL
        TraMonthCheckValueBLL bll = new TraMonthCheckValueBLL();

        #region 页面

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string url)
        {
            ViewBag.url = url;
            return View();
        } 
        #endregion

        #region 方法
         
        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param> 
        /// <returns>Json</returns>
        public ActionResult MonthCheckValueList(int index, int size)
        {
            // 有效信息。
            string where = " State = 1";
              
            // 月考核取值List
            List<TraMonthCheckValueModel> list = bll.MonthCheckValueList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>  
        /// <returns>int</returns>
        public int MonthCheckValueCount()
        {
            // 有效信息。
            string where = " State = 1";

            return bll.MonthCheckValueCount(where);
        } 
        #endregion
    }
}
