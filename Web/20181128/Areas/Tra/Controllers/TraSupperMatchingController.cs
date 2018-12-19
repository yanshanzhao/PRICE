// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-15    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.BLL.Tra;
using SRM.Model.Tra;
#endregion
/*********************************
 * 类名：TraSupperMatchingController
 * 功能描述：供应商用户匹配 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraSupperMatchingController : Controller
    {
        //
        // GET: /Tra/TraSupperMatching/

        TraSupperMatchingBLL bll = new TraSupperMatchingBLL();

        #region 页面 

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns> 
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            TraSupperMatchingModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            TraSupperMatchingModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 运输供应商信息
        /// </summary>
        public ActionResult TraSupplierDetail(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// 供应商账号维护明细信息
        /// </summary>
        public ActionResult TraSupperMatchingDetail(string url)
        {
            ViewBag.url = url;
            return View();
        }

        #endregion

        #region 方法 

        #region 新增 供应商用户匹配

        /// <summary>
        /// 新增 供应商用户匹配
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddTraSupperMatching(TraSupperMatchingModel model)
        {

            model.State = 0; //状态:0-初始;1-有效;5-作废;10-使用

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构id

            model.CretaeUserId = Auxiliary.UserID(); // 用户id

            if (bll.isExistUserId(model.UserId) != 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" }); 
            }

            int SupperMatching = bll.AddTraSupperMatching(model);

            if (SupperMatching > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 数据集 供应商用户匹配

        /// <summary>
        /// 数据集 供应商用户匹配
        /// </summary>
        /// <param name="index">当前索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult TraSupperMatchingList(int index, int size, string supplierName, string state)
        {
            string where = string.Format(" TSM.State != 5 and TSM.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TSM.State = {0}", state.Trim());
            }

            List<TraSupperMatchingModel> list = bll.TraSupperMatchingList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 供应商用户匹配

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public int TraSupperMatchingCount(string supplierName, string state)
        {
            string where = string.Format(" TSM.State != 5 and TSM.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TSM.State = {0}", state.Trim());
            }

            return bll.TraSupperMatchingCount(where);
        }
        #endregion

        #region 编辑 供应商用户匹配

        /// <summary>
        /// 编辑 供应商用户匹配
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraSupperMatching(TraSupperMatchingModel model)
        {

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构id

            model.CretaeUserId = Auxiliary.UserID(); // 用户id

            TraSupperMatchingModel beforeModel = bll.GetModelByID(model.SupperMatchingId);

            if (bll.isExistUserId(model.UserId) != 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "exist" });
            }

            //修改 供应商用户匹配
            int result = bll.EditTraSupperMatching(model);

            if (result > 0)
            {

                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 提交按钮逻辑

        /// <summary>
        /// 提交按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitTraSupperMatching(int tId)
        {
            //更新状态 将供应商用户匹配状态更新为有效
            int row = bll.SubmitTraSupperMatching(tId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail", content = "提交失败！" });
        }
        #endregion

        #region 作废按钮逻辑

        /// <summary>
        /// 作废按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            TraSupperMatchingModel beforeModel = bll.GetModelByID(tId);

            //将供应商用户匹配状态更新为作废
            int row = bll.InvalidState(tId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion

        #region 导出按钮逻辑

        /// <summary>
        /// 导出按钮逻辑
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string state)
        {
            string where = string.Format(" TSM.State != 5 and TSM.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TSM.State = {0}", state.Trim());
            }

            System.Data.DataTable dt = bll.ExportDataTable(where);
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new
            {
                Type = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        #region 供应商明细列表数据

        /// <summary>
        /// 供应商明细列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public ActionResult SupperDetailList(int index, int size, string supplierName)
        {
            string where = string.Format(" (ST.TransportState='F2' OR ST.TransportState='F4' OR ST.TransportState='F7') AND S.State=1 AND S.State!=20 AND S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            //供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            List<TraSupperMatchingModel> list = bll.SupperDetailList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 供应商明细数据记录数

        /// <summary>
        /// 供应商明细数据记录数
        /// </summary> 
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public int SupperDetailCount(string supplierName)
        {
            //string where = string.Format(" ST.TransportState='F4'AND ST.CompanyId={0}", Auxiliary.CompanyID());
            //string where = string.Format(" ST.TransportState='F2' AND S.State=1 AND S.State!=20 AND S.CreateDepartmentId={0}", Auxiliary.DepartmentId());
            string where = string.Format(" (ST.TransportState='F2' OR ST.TransportState='F4' OR ST.TransportState='F7') AND S.State=1 AND S.State!=20 AND S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            //供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName  like '%{0}%' ", supplierName.Trim());
            }

            return bll.SupperDetailCount(where);
        }
        #endregion

        #region 供应商匹配列表数据

        /// <summary>
        /// 供应商匹配列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="departmentName">机构名称</param>
        /// <returns></returns>
        public ActionResult SupperMatchingList(int index, int size, string departmentName)
        {
            string where = string.Format(" SD.State = 1 AND SD.DeparType=3 AND SD.CompanyId={0}", Auxiliary.CompanyID());

            //机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName like '%{0}%'", departmentName.Trim());
            }

            List<TraSupperMatchingModel> list = bll.SupperMatchingList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 供应商匹配记录数

        /// <summary>
        /// 供应商匹配记录数
        /// </summary> 
        /// <param name="departmentName">机构名称</param>
        /// <returns></returns>
        public int SupperMatchingCount(string departmentName)
        {
            string where = string.Format(" SD.State = 1 AND SD.DeparType=3 AND SD.CompanyId={0}", Auxiliary.CompanyID());

            //机构名称
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And SD.DepartmentName  like '%{0}%' ", departmentName.Trim());
            }

            return bll.SupperMatchingCount(where);
        }
        #endregion

        #endregion

    }
}
