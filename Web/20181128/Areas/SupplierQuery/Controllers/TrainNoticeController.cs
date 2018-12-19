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
 * 类名：TrainNoticeController
 * 功能描述：培训通知 控制器 
 * ******************************/
namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class TrainNoticeController : Controller
    {
        //
        // GET: /SupplierQuery/TrainNotice/

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
            Model.Tra.TrainNoticeModel model = bll.GetModelByClaimId(tId); 
            return View(model);
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpPost]
        [Operate(Name = OperateEnum.Search)]
        public ActionResult Index(int index,int size)
        {
            string where = string.Empty;

            List<Model.Tra.TrainNoticeModel> list = bll.TrainNoticeList(index,size,where);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));

        }

        /// <summary>
        /// 分页总数
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexAmount()
        {
            string where = string.Empty;
            int count = bll.TrainNoticeAmount(where);
            return Content(count.ToString());
        }

        /// <summary>
        /// 附件
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public ActionResult AdjunctList(int tId)
        {
            List<Model.Tra.TrainNoticeModel > list = bll.TrainNoticeAdjunctList(tId);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
    }
}
