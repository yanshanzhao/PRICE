//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-01    1.0        HDS        新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BLL.Sys;
using Model.Sys;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：SysLoginController
 * 功能描述：登陆日志 控制器  
 * ******************************/
namespace Web.Controllers
{
    public class SysLoginController : Controller
    {
        SysLogBLL bll = new SysLogBLL();

        /// <summary>
        /// 登陆日志列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        ///登陆日志列表数据
        /// </summary>
        public ActionResult List(int index, int size)
        {
            string where = " l.module='login' ";

            if (!Auxiliary.IsAdmin())
            {
                where = string.Format(" l.module='login' and l.CompanyId={0} ", Auxiliary.CompanyID());
            }    

            List<SysLogModel> list = bll.SysLoginPageList(index, size, where);

            list.ForEach(p => p.Message = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            return Json(list);
        }
        /// <summary>
        ///登陆日志列表数据总数
        /// </summary>
        public int Count()
        {
            string where = " module='login' ";

            if (!Auxiliary.IsAdmin())
            {
                where = string.Format(" module='login' and l.CompanyId={0} ", Auxiliary.CompanyID());
            }

            return bll.SysLoginPageCount(where);
        }
    }
}
