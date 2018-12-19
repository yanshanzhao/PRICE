//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-09-06    1.0       MY         新建
//-------------------------------------------------------------------------

#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

/*********************************
 * 类名：TraComponentDetailController
 * 功能描述：运输评估元件明细表 控制器
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraComponentDetailController : Controller
    {
        //
        // GET: /Tra/TraComponentDetail/
        private BLL.Tra.TraComponentDetailBLL bll = new BLL.Tra.TraComponentDetailBLL();

        #region  方法

        #region 运输评估元件明细表 分页查询列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="checktraComponentId">元件表ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TraComponentDetailList(int index,int size, string checktraComponentId)
        {
            string where = string.Empty;

            where = " And TCD.State = 1 ";
            

            //元件编号
            
            if (!string.IsNullOrEmpty(checktraComponentId))
            {
                where += string.Format(" And TCD.ComponentId = {0}", checktraComponentId.Trim());
            }
            List<Model.Tra.TraComponentDetailModel> list = bll.GetTraComponentDetailList(index, size, where);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        #endregion

        #region  分页条件查询总数
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TraComponentDetailAmount(string checktraComponentId)
        {
            string where = string.Empty;
            
            where = " And TCD.State = 1 ";


            if (!string.IsNullOrEmpty(checktraComponentId))
            {
                where += string.Format(" And TCD.ComponentId = {0}", checktraComponentId.Trim());
            }
            int count = bll.TraComponentDetailAmount(where);
            return Content(count.ToString());
        }
        #endregion

        #endregion 


    }
}
