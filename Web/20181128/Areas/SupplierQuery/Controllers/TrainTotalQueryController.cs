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
using Newtonsoft.Json.Converters;

/*********************************
 * 类名：TrainTotalQueryController
 * 功能描述：培训情况汇总查询 控制器 
 * ******************************/
namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class TrainTotalQueryController : Controller
    {
        private BLL.Tra.BusinessQueryBLL bll = new BLL.Tra.BusinessQueryBLL();
        //
        // GET: /SupplierQuery/TrainTotalQuery/

        //
        public ActionResult Index()
        {
            return View();
        }

        //查看
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int cId)
        {
            Model.Tra.TraOperationClaimModel model = bll.GetModelById(cId);
            return View(model);
        }

        #region 方法
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="departmentName"></param>
        /// <param name="theme"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        [HttpPost]
        [Operate(Name = OperateEnum.Search)]
        public ActionResult Index(int index,int size,string departmentName,string theme,string destination)
        {
            string where = string.Empty;
            where = " And SD.CompanyId = " + Auxiliary.CompanyID();

            //机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%' ", departmentName.Trim());
            }

            //主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And TOC.Theme like '%{0}%' ", theme.Trim());
            }

            //评论
            if (!string.IsNullOrEmpty(destination))
            {
                where += string.Format(" And TOC.Destination like '%{0}%' ", destination.Trim());
            }
            List<Model.Tra.TrainTotalModel> list = bll.GetTrainTotalList(index, size, where);
            //return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 分页总数
        /// </summary>
        /// <param name="departmentName"></param>
        /// <param name="theme"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public int IndexAmount(string departmentName, string theme, string destination)
        {
            string where = string.Empty;
            where = " And SD.CompanyId = " + Auxiliary.CompanyID();

            //机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%' ", departmentName.Trim());
            }

            //主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And TOC.Theme like '%{0}%' ", theme.Trim());
            }

            //评论
            if (!string.IsNullOrEmpty(destination))
            {
                where += string.Format(" And TOC.Destination like '%{0}%' ", destination.Trim());
            }

            return bll.GetTrainTotalAmount(where);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="departmentName"></param>
        /// <param name="supplierName"></param>
        /// <param name="theme"></param>
        /// <param name="recordTime"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(int index, int size, string departmentName, string theme, string destination)
        {

            // where条件
            string where = string.Empty;

            // 本公司ID
            where = " And SD.CompanyId = " + Auxiliary.CompanyID();

            //机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%' ", departmentName.Trim());
            }

            //主题
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And TOC.Theme like '%{0}%' ", theme.Trim());
            }

            //评论
            if (!string.IsNullOrEmpty(destination))
            {
                where += string.Format(" And TOC.Destination like '%{0}%' ", destination.Trim());
            }

            // 巡查执行查询DataTable
            System.Data.DataTable dt = bll.ExportTrainTotalTable(index,size,where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);
            
            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdjunctList(int cId)
        {
            List<Model.Tra.TraOperationeAdjunctModel> list = bll.GetAdjunctListByCId(cId);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        public ActionResult GetCheckPartToLow(int cId)
        {
            List<Model.Tra.TrainTotalModel> list = bll.GetCheckPartToLow(cId);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        #endregion 
    }
}
