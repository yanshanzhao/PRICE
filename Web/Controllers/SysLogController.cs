//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-17    1.0        HDS        新建               
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
 * 类名：SysLogController
 * 功能描述：系统日志 控制器  
 * ******************************/
namespace Web.Controllers
{
    public class SysLogController : Controller
    {
        SysLogBLL bll = new SysLogBLL();
        /// <summary>
        ///日志列表表
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 日志分页列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="modid">模块编号</param>
        public ActionResult List(int index, int size,int modid)
        {


            string where = string.Empty;

            if (modid != -1)
            {
                if (Auxiliary.IsAdmin())
                {
                    where = " 1=1 ";
                }
                else
                {
                    where = string.Format("  l.CompanyId={0} ", Auxiliary.CompanyID());
                }

                List<SysLogModel> list = bll.SysLogPageLists(index, size, where,modid.ToString());


                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
            }
            else
            {
                if (Auxiliary.IsAdmin())
                {
                    where = " l.module!='exception' and l.module!='login' ";
                }
                else
                {
                    where = string.Format(" l.module!='exception'  and l.module!='login'  and l.CompanyId={0}", Auxiliary.CompanyID());
                }

                List<SysLogModel> list = bll.SysLogPageLists(index, size, where);


                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
            }
        

           
        }

        /// <summary>
        /// 日志列表数据总数
        /// </summary>
        /// <param name="modid">模块编号</param>
        /// <returns></returns>
        public int Count(int modid)
        {
            string where = string.Empty;

            if (modid != -1)
            {
                if (Auxiliary.IsAdmin())
                {
                    where = " 1=1 ";
                }
                else
                {
                    where = string.Format("  l.CompanyId={0} ", Auxiliary.CompanyID());
                }

                return bll.SysLogPageCounts(where,modid.ToString());
            }
            else
            {
                if (Auxiliary.IsAdmin())
                {
                    where = " l.module!='exception'  and l.module!='login'  ";
                }
                else
                {
                    where = string.Format(" l.module!='exception'  and l.module!='login'  and l.CompanyId={0}", Auxiliary.CompanyID());
                }

                return bll.SysLogPageCounts(where);
            }

           
        }
    }
}
