//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-15    1.0        FJK        新建             
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
 * 类名：TraMonthCheckAdjunctController
 * 功能描述：运输月度考核附件 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraMonthCheckAdjunctController : Controller
    {
        //
        // GET: /Tra/TraMonthCheckAdjunct/

        // 运输月度考核附件bll
        TraMonthCheckAdjunctBLL bll = new TraMonthCheckAdjunctBLL();

        #region 页面

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        } 
        #endregion

        #region 方法
          
        /// <summary>
        /// 数据集 
        /// </summary>
        /// <param name="checkId">月度考核id</param> 
        /// <returns>Json</returns>
        public ActionResult MonthCheckAdjunctList(int checkId)
        {
            // 根据CheckId 
            string where = " State=1 AND CheckId = " + checkId;

            // 运输月度考核附件List
            List<TraMonthCheckAdjunctModel> list = bll.MonthCheckAdjunctList(where);

            return Json(list);
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns> 
        public ActionResult InvalidState(int tId)
        {
            // 作废之前Model
            TraMonthCheckAdjunctModel beforeModel = bll.GetModelByID(tId);
             
            // 作废(更改状态)
            int row = bll.InvalidState(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败" });
        }

        #endregion
    }
}
