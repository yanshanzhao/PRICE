//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-08    1.0        HDS        新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BLL.Sys;
using Model.Sys;
#endregion
/*********************************
 * 类名：SysExcepController
 * 功能描述：系统异常 控制器  
 * ******************************/
namespace Web.Controllers
{
    public class SysExcepController : Controller
    {

        SysLogBLL bll = new SysLogBLL();
        /// <summary>
        ///异常列表页
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///异常列表数据
        /// </summary>
        public ActionResult List(int index, int size)
        {
           string where = " module='exception' ";

           List<SysLogModel> list= bll.SysLogPageList(index,size,where);

           list.ForEach(p => p.Result = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));

           return Json(list);
        }
        /// <summary>
        ///异常列表数据总数
        /// </summary>
        public int Count()
        {
          string where = " module='exception' ";
          return bll.SysLogPageCount(where);
        }
        /// <summary>
        ///异常明细页面
        /// </summary>
        [Operate(Name=OperateEnum.View)]
        [ValidateInput(false)]
        public ActionResult View(string type,string ids)
        {
            SysLogModel model = bll.GetModelByID(ids);

            ViewBag.msg = model.Message;
            ViewBag.type = type;
            return View();
        }
    }
}
