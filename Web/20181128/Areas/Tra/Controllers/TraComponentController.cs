//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-09-06    1.0       MY         新建
//-------------------------------------------------------------------------
#region 参考
using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

/*********************************
 * 类名：TraComponentController
 * 功能描述：运输评估元件表 控制器
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraComponentController : Controller
    {
        private BLL.Tra.TraComponentBLL bll = new BLL.Tra.TraComponentBLL();

        private BLL.Tra.TraComponentDetailBLL TCDBLL = new BLL.Tra.TraComponentDetailBLL();

        private BLL.Tra.TraAssessFromComponentBLL TAFCbll = new BLL.Tra.TraAssessFromComponentBLL(); 
        //
        // GET: /Tra/TraComponent/
        /// <summary>
        /// 数据列表
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
            ViewBag.StorageNumber = Auxiliary.CurCompanyAutoNum("RAN");
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
            Model.Tra.TraComponentModel model = bll.GetModelByID(tId);
            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            Model.Tra.TraComponentModel model = bll.GetModelByID(tId);
            return View(model);
        }

        #region 新增运输评估元件弹窗窗口
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public ActionResult AddComponentDetail(string url)
        {
            ViewBag.url = url;

            return View();
        }
        #endregion
         
        #region 选择运输评估元件弹窗窗口
        /// <summary>
        /// 选择运输评估元件信息
        /// </summary>
        public ActionResult TraComponentDetail(string id,string url)
        {
            ViewBag.url = url;
            ViewBag.id = id;

            return View();
        }
        #endregion

        #region 方法

        #region 分页条件查询列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="ComponentNumber"></param>
        /// <param name="ComponentName"></param>
        /// <param name="AssessTypeName"></param>
        /// <param name="State"></param>
        /// <param name="IsBaisc"></param>
        /// <param name="CreateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(int index, int size,string ComponentNumber, string ComponentName, string AssessTypeName, string State, string IsBasic, string CreateTime,string ids,string type)
        {
            string where = string.Empty; 

            // 判断是否是模板选择元件
            if (type != null)
            {
                // 使用状态 且 本公司
                where = " AND TC.State =1 And TC.CompanyId = " + Auxiliary.CompanyID();

                if (type == "index")
                {
                    // 查询已选id
                    if (!string.IsNullOrEmpty(ids))
                    {
                        where += String.Format(" And TC.ComponentId IN ({0})", ids.Trim());
                    }
                }
               else if (type == "add")
                {
                    // 排除已选id
                    if (!string.IsNullOrEmpty(ids))
                    {
                        where += String.Format(" And TC.ComponentId NOT IN ({0})", ids.Trim());
                    }
                }
                else if (type == "edit")
                {
                    // 排除已选id
                    if (!string.IsNullOrEmpty(ids))
                    {
                        where += String.Format(" And TC.ComponentId NOT IN ( SELECT ComponentId FROM TraAssessFromComponent WHERE State =1 AND AssessFromId = {0})", ids.Trim());
                    }
                }
            }
            else
            {
                // 非作废状态 且 按照公司查询
                where = " And TC.State != 10  And TC.CompanyId = " + Auxiliary.CompanyID();
            }

            //元件编号
            if (!string.IsNullOrEmpty(ComponentNumber))
            {
                where += String.Format(" And TC.ComponentNumber like '%{0}%'", ComponentNumber.Trim());
            }
            //元件名称
            if (!string.IsNullOrEmpty(ComponentName))
            {
                where += String.Format(" And TC.ComponentName like '%{0}%'", ComponentName.Trim());
            }
            //评估分类
            if (!string.IsNullOrEmpty(AssessTypeName))
            {
                where += String.Format(" And TC.AssessTypeName = {0}", AssessTypeName.Trim());
            }
            //状态
            if (!string.IsNullOrEmpty(State))
            {
                where += String.Format(" And TC.State = {0}", State.Trim());
            }
            //基本类型
            if (!string.IsNullOrEmpty(IsBasic))
            {
                where += String.Format(" And TC.IsBasic = {0}", IsBasic.Trim());
            }
            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += String.Format(" And convert(varchar,TC.CreateTime,120) like '%{0}%'", CreateTime.Trim());
            }
            List<Model.Tra.TraComponentModel> list = bll.GetTraComponentList(index, size, where);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        #endregion

        #region  分页条件查询总数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComponentNumber"></param>
        /// <param name="ComponentName"></param>
        /// <param name="AssessTypeName"></param>
        /// <param name="State"></param>
        /// <param name="IsBaisc"></param>
        /// <param name="CreateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TraComponentAmount( string ComponentNumber, string ComponentName, string AssessTypeName, string State, string IsBasic, string CreateTime, string ids, string type)
        { 
            string where = string.Empty;

            // 判断是否是模板选择元件
            if (type != null)
            {
                // 使用状态 且 本公司
                where = " AND TC.State =1 And TC.CompanyId = " + Auxiliary.CompanyID();

                if (type == "index")
                {
                    // 查询已选id
                    if (!string.IsNullOrEmpty(ids))
                    {
                        where += String.Format(" And TC.ComponentId IN ({0})", ids.Trim());
                    }
                }
                else if (type == "add")
                {
                    // 排除已选id
                    if (!string.IsNullOrEmpty(ids))
                    {
                        where += String.Format(" And TC.ComponentId NOT IN ({0})", ids.Trim());
                    }
                }
                else if (type == "edit")
                {
                    // 排除已选id
                    if (!string.IsNullOrEmpty(ids))
                    {
                        where += String.Format(" And TC.ComponentId NOT IN ( SELECT ComponentId FROM TraAssessFromComponent WHERE State =1 AND AssessFromId = {0})", ids.Trim());
                    }
                }
            }
            else
            {
                // 非作废状态 且 按照公司查询
                where = " And TC.State != 10  And TC.CompanyId = " + Auxiliary.CompanyID();
            }

            //元件编号
            if (!string.IsNullOrEmpty(ComponentNumber))
            {
                where += String.Format(" And TC.ComponentNumber like '%{0}%'", ComponentNumber.Trim());
            }
            //元件名称
            if (!string.IsNullOrEmpty(ComponentName))
            {
                where += String.Format(" And TC.ComponentName like '%{0}%'", ComponentName.Trim());
            }
            //评估分类
            if (!string.IsNullOrEmpty(AssessTypeName))
            {
                where += String.Format(" And TC.AssessTypeName = {0}", AssessTypeName.Trim());
            }
            //状态
            if (!string.IsNullOrEmpty(State))
            {
                where += String.Format(" And TC.State = {0}", State.Trim());
            }
            //基本类型
            if (!string.IsNullOrEmpty(IsBasic))
            {
                where += String.Format(" And TC.IsBasic = {0}", IsBasic.Trim());
            }
            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += String.Format(" And convert(varchar,TC.CreateTime,120) like '%{0}%'", CreateTime.Trim());
            }

            int count = bll.TraComponentAmount(where);
            return Content(count.ToString());
        }
        #endregion
        
        #region 新增时：主表明细表同时新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddTraComponent(Model.Tra.TraComponentModel model)
        {

            string where = string.Empty;

            if (!string.IsNullOrEmpty(model.ComponentName))
            {
                where += string.Format(" And CreateDepartmentId = {0} ", Auxiliary.DepartmentId());
                where += "  And AssessTypeName =" + model.AssessTypeName ;
                where += "  And ComponentName like '%" + model.ComponentName + "%'";
            }

            int count = bll.GetCountByComponentName(where);
            if (count > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            model.State = 0;// 状态 初始

            model.ComponentNumber = Auxiliary.CurCompanyAutoNum("TCN");//运输选择编号

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID
            
            if (model.Remark == "" || model.Remark == null)
            {
                model.Remark = "";
            }

            int ComponentId = bll.AddTraComponent(model);

            if (ComponentId > 0)
            {
                if (!string.IsNullOrEmpty(ComponentId.ToString()))
                {
                    List<Model.Tra.TraComponentDetailModel> list =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.Tra.TraComponentDetailModel>>(model.ComponentDetailList);
                    bll.AddComponentList(list, ComponentId);
                }
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 编辑时：主表明细表同时新增

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraComponent(Model.Tra.TraComponentModel_DTO model)
        {
            int rows = bll.EditTraComponent(model);

            if (rows > 0)
            {
                int rows1 = TCDBLL.EditTraComponentDetail(model);
                if (rows1 > 0)
                {
                    Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                    return Json(new { flag = "success", content = "修改成功！" });
                }
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion
        
        #region 导出按钮逻辑

        /// <summary>
        /// 导出按钮逻辑
        /// </summary>
        /// <param name="ApplyTime"></param>
        /// <param name="State"></param>
        /// <param name="UseState"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string ComponentNumber, string ComponentName, string AssessTypeName, string State, string IsBaisc, string CreateTime)
        {
            string where = " And TC.State != 10";
            //元件编号
            if (!string.IsNullOrEmpty(ComponentNumber))
            {
                where += String.Format(" And ComponentNumber like '%{0}%'", ComponentNumber.Trim());
            }
            //元件名称
            if (!string.IsNullOrEmpty(ComponentName))
            {
                where += String.Format(" And ComponentName like '%{0}%'", ComponentName.Trim());
            }
            //评估分类
            if (!string.IsNullOrEmpty(AssessTypeName))
            {
                where += String.Format(" And AssessTypeName = {0}", AssessTypeName.Trim());
            }
            //状态
            if (!string.IsNullOrEmpty(State))
            {
                where += String.Format(" And TC.State = {0}", State.Trim());
            }
            //基本类型
            if (!string.IsNullOrEmpty(IsBaisc))
            {
                where += String.Format(" And TC.IsBasic = {0}", IsBaisc.Trim());
            }
            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += String.Format(" And convert(varchar,TC.CreateTime,120) like '%{0}%'", CreateTime.Trim());
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

        #region 作废按钮逻辑 
        /// <summary>
        /// 作废按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        public ActionResult InvalidState(int tId)
        {

            Model.Tra.TraComponentModel beforeModel = bll.GetModelByID(tId);

            int delUserId = Auxiliary.UserID();

            int row = bll.InvalidState(tId, delUserId);
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }

        #endregion

        #region 提交按钮逻辑

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitTraComponent(int tId)
        {

            // 提交(更改状态)
            int row = bll.SubmitState(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "提交", Id = tId, State = "提交" });
                return Json(new { flag = "success", content = "提交成功" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "提交", Id = tId, State = "初始" });
            return Json(new { flag = "fail", content = "提交失败" });
        }
        #endregion

        #region 编辑时查询明细表

        private BLL.Tra.TraComponentDetailBLL detailBLL = new BLL.Tra.TraComponentDetailBLL();

        #region 运输评估元件明细表 分页查询列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="checktraComponentId">元件表ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TraComponentDetailList(int index, int size, string id)
        {
            string where = string.Empty;

            where = " And TCD.State = 1 ";
            
            //元件编号

            // 供应商名称
            if (!string.IsNullOrEmpty(id))
            {
                where += string.Format(" And TCD.ComponentId = {0}", id.Trim());
            }
            List<Model.Tra.TraComponentDetailModel> list = detailBLL.GetTraComponentDetailList(index, size, where);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        #endregion

        #region  分页条件查询总数
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TraComponentDetailAmount(string id)
        {
            string where = string.Empty;

            where = " And TCD.State = 1 ";

            // 供应商名称
            if (!string.IsNullOrEmpty(id))
            {
                where += string.Format(" And TCD.ComponentId = {0}", id.Trim());
            }
            int count = detailBLL.TraComponentDetailAmount(where);
            return Content(count.ToString());
        }
        #endregion

        #endregion
        
        #endregion
    }
}
