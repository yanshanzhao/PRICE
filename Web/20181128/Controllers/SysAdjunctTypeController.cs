//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-15    1.0        FJK        新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.BLL.Sys;
using SRM.Model.Sys;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：SysAdjunctTypeController
 * 功能描述：附件类型表 控制器  
 * *******************************/

namespace SRM.Web.Controllers
{
    public class SysAdjunctTypeController : Controller
    {
        //
        // GET: /SysAdjunctType/ 
        // 附件类型BLL
        SysAdjunctTypeBLL bll = new SysAdjunctTypeBLL();

        #region 页面

        /// <summary>
        /// Index
        /// </summary> 
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add
        /// </summary> 
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary> 
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
           SysAdjunctTypeModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary>
        public ActionResult View(int tId)
        {
            // 获取数据
           SysAdjunctTypeModel model = bll.GetModelByID(tId);

            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 附件类型新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddSysAdjunctType(SysAdjunctTypeModel tModel)
        {
            // 默认状态为有效1
            tModel.State = 1;

            // 默认排序为0
            tModel.Sort = 0;

            // 默认公司
            tModel.CompanyId = Auxiliary.CompanyID();

            // 查询同公司同附件类型是否存在同附件名称数据
            // bll.AddSysAdjunctType(tModel);

            // 新增
            if (bll.AddSysAdjunctType(tModel) > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="adjunctName">附件名称</param>
        /// <param name="adjunctType">附件类型</param>
        /// <returns></returns>
        public ActionResult SysAdjunctTypeList(int index, int size, string adjunctName, string adjunctType)
        {
            // 只能查询本公司内的附件信息
            string where = " SAT.CompanyId =" + Auxiliary.CompanyID();

            // 附件名称
            if (!string.IsNullOrEmpty(adjunctName))
            {
                where += string.Format(" And AdjunctName like '%{0}%'", adjunctName.Trim());
            }

            // 附件类型
            if (!string.IsNullOrEmpty(adjunctType))
            {
                where += string.Format(" And AdjunctType = '{0}'", adjunctType.Trim());
            }

            // 附件类型List
            List<SysAdjunctTypeModel> list = bll.SysAdjunctTypeList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="adjunctName">附件名称</param>
        /// <param name="adjunctType">附件类型</param>
        /// <returns></returns>
        public int SysAdjunctTypeCount(string adjunctName, string adjunctType)
        {
            // 只能查询本公司内的附件信息
            string where = " SAT.CompanyId =" + Auxiliary.CompanyID();

            // 附件名称
            if (!string.IsNullOrEmpty(adjunctName))
            {
                where += string.Format(" And AdjunctName like '%{0}%'", adjunctName.Trim());
            }

            // 附件类型
            if (!string.IsNullOrEmpty(adjunctType))
            {
                where += string.Format(" And AdjunctType = '{0}'", adjunctType.Trim());
            }

            return bll.SysAdjunctTypeCount(where);
        }

        /// <summary>
        /// 附件类型编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditSysAdjunctType(SysAdjunctTypeModel tModel)
        {
            // 编辑之前Model
            SysAdjunctTypeModel beforeModel = bll.GetModelByID(tModel.AdjunctId);

            // 编辑
            int result = bll.EditSysAdjunctType(tModel);

            // 若影响行数>O(修改成功)
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }
         
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="adjunctName">附件名称</param>
        /// <param name="adjunctType">附件类型</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string adjunctName, string adjunctType)
        {
            // 只能查询本公司内的附件信息
            string where = " CompanyId =" + Auxiliary.CompanyID();

            // 附件名称
            if (!string.IsNullOrEmpty(adjunctName))
            {
                where += string.Format(" And AdjunctName like '%{0}%'", adjunctName.Trim());
            }

            // 附件类型
            if (!string.IsNullOrEmpty(adjunctType))
            {
                where += string.Format(" And AdjunctType = '{0}'", adjunctType.Trim());
            }

            // DataTable
            System.Data.DataTable dt = null;// bll.ExportDataTable(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }
        #endregion 
    }
}
