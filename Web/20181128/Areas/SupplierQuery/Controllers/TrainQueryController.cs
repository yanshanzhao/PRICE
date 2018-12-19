//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-10    1.0        MY        新建         
//2018-10-26    1.0        zbb        新建             
//-------------------------------------------------------------------------
using Aspose.Cells;
using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
using Newtonsoft.Json.Converters; 

/*********************************
 * 类名：TrainQueryController
 * 功能描述：培训情况查询 控制器 
 * ******************************/
namespace SRM.Web.Areas.SupplierQuery.Controllers
{
    public class TrainQueryController : Controller
    {
        //
        // GET: /SupplierQuery/TrainQuery/
        private BLL.Tra.BusinessQueryBLL bll = new BLL.Tra.BusinessQueryBLL();

        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Operate(Name = OperateEnum.Search)]
        public ActionResult IndexList(int index,int size,string departmentName,string supplierName,string theme,string recordTime)
        {
            string where = string.Empty;
            // 本公司ID
            where = " And SD.CompanyId = " + Auxiliary.CompanyID();

            if (bll.GetTrainQueryList(index, size, where) == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%' ", departmentName.Trim());
            }

            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%' ", supplierName.Trim());
            }
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And TOC.Theme like '%{0}%' ", theme.Trim());
            }
            if (!string.IsNullOrEmpty(recordTime))
            { 
                where += string.Format(" And convert(varchar,TOR.RecordTime,120) like '%{0}%'", recordTime.Trim());
            }  

            List<Model.Tra.TrainQueryModel> list = bll.GetTrainQueryList(index,size,where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();

            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }


        public int IndexAmount(string departmentName, string supplierName, string theme, string recordTime)
        {
            string where = string.Empty;

            // 本公司ID
            where = " And SD.CompanyId = " + Auxiliary.CompanyID();

            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%' ", departmentName.Trim());
            }

            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%' ", supplierName.Trim());
            }
            if (!string.IsNullOrEmpty(theme))
            {
                where += string.Format(" And TOC.Theme like '%{0}%' ", theme.Trim());
            }
            if (!string.IsNullOrEmpty(recordTime))
            {
                where += string.Format(" And convert(varchar,TOR.RecordTime,120) like '%{0}%'", recordTime.Trim());
            } 
            return bll.GetTrainQueryAmount(where);
        }
        

        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string departmentName, string supplierName, string theme, string recordTime)
        {

            // where条件
            string where = string.Empty;

            // 本公司ID
            where = " And SD.CompanyId = " + Auxiliary.CompanyID();

            // 部门名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%'", departmentName.Trim());
            }

            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%' ", supplierName.Trim());
            }
            if (!string.IsNullOrEmpty(theme))
            { 
                where += string.Format(" And TOC.Theme like '%{0}%' ", theme.Trim());
            }
            if (!string.IsNullOrEmpty(recordTime))
            {
                where += string.Format(" And convert(varchar,TOR.RecordTime,120) like '%{0}%'", recordTime.Trim());
            }


            // 巡查执行查询DataTable
            System.Data.DataTable dt = bll.ExportTrainTable(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            //// guid
            //string guid = string.Empty;

            //// 模板文件路径
            //string TemplatePath = System.Web.HttpContext.Current.Server.MapPath(@"/upload/export/Template/TraPatrolExecute.xls");

            //// 模板导出
            //Auxiliary.TemplateExport(dt, TemplatePath, "A4", "TraPatrolExecute", ref guid);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }

    }
}
