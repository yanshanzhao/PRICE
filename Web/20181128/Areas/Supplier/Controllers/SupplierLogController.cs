//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-09    1.0        MH         新建               
//-------------------------------------------------------------------------
#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.BLL.Supplier;
using SRM.Model.Supplier;
using SRM.Web.Controllers;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：SupplierLogController
 * 功能描述：供应商日志 控制器 
 * ******************************/
namespace SRM.Web.Areas.Supplier.Controllers
{
    public class SupplierLogController : Controller
    {

        SupplierLogBLL bll = new SupplierLogBLL();

        /// <summary>
        /// 主页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 菜单列表数据
        /// </summary>
        public ActionResult MenuList()
        {
            return Json(bll.SuppMemuList(Auxiliary.CompanyID().ToString()));
        }

        /// <summary>
        /// 日志分页列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="modid">模块编号</param>
        public ActionResult List(int index, int size, int modid)
        {
            string where = " 1=1 ";

            if (modid == -1)
            {
                where = string.Format("  l.CompanyId={0} ", Auxiliary.CompanyID());
            }
            else
            {
                where = string.Format("  l.CompanyId={0} And l.Module='{1}'  ", Auxiliary.CompanyID(),modid);
            }

            List<SupplierLogModel> list = bll.SupplierLogPageList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));       
        }

        /// <summary>
        /// 日志列表数据总数
        /// </summary>
        /// <param name="modid">模块编号</param>
        /// <returns></returns>
        public int Count(int modid)
        {
            string where = " 1=1 ";

            if (modid == -1)
            {
                where = string.Format("  l.CompanyId={0} ", Auxiliary.CompanyID());
            }
            else
            {
                where = string.Format("  l.CompanyId={0} And l.Module='{1}'  ", Auxiliary.CompanyID(), modid);
            }

            return bll.SupplierLogPageCount(where);
        }

        /// <summary>
        ///异常明细页面
        /// </summary>
        [Operate(Name = OperateEnum.View)]
        [ValidateInput(false)]
        public ActionResult Views(string type, string ids)
        {
            SupplierLogModel model = bll.GetModelByID(ids);

            ViewBag.msg = model.Message;
            ViewBag.type = type;
            return View();
        }
    }
}
