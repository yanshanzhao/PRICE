using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRM.Web.Areas.Storage.Controllers
{
    public class StorageComponentController : Controller
    {

        //
        private BLL.Storage.StorageComponentBLL bll = new BLL.Storage.StorageComponentBLL();

        //
        private BLL.Storage.StorageComponentDetailBLL SCDbll = new BLL.Storage.StorageComponentDetailBLL();

        //
        // GET: /Storage/StorageComponent/

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
        //[Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            Model.Storage.StorageComponentModel model = bll.GetModelByID(tId);
            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            Model.Storage.StorageComponentModel model = bll.GetModelByID(tId);
            return View(model);
        }

        /// <summary>
        /// 运输评估元件
        /// </summary>
        /// <returns></returns>
        public ActionResult Component(string url, string tId, string score, string type)
        {
            ViewBag.url = url;
            ViewBag.ids = tId;
            ViewBag.type = type;
            ViewBag.score = score;

            return View();
        }

        /// <summary>
        /// 运输评估元件明细
        /// </summary>
        /// <returns></returns>
        public ActionResult ComponentDetail(string url,string id)
        {
            ViewBag.url = url;
            ViewBag.id = id;

            return View();
        }

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
        public ActionResult Index(int index, int size,string componentId, string ComponentNumber, string ComponentName, string AssessTypeName, string State, string IsBasic, string CreateTime)
        {
            string where = string.Empty;
            where = " And SC.State != 10 ";
            //元件Id
            if (!string.IsNullOrEmpty(componentId))
            {
                where += String.Format(" And SC.ComponentId in ({0})", componentId.Trim());
            }

            //元件编号
            if (!string.IsNullOrEmpty(ComponentNumber))
            {
                where += String.Format(" And SC.ComponentNumber like '%{0}%'", ComponentNumber.Trim());
            }
            //元件名称
            if (!string.IsNullOrEmpty(ComponentName))
            {
                where += String.Format(" And SC.ComponentName like '%{0}%'", ComponentName.Trim());
            }
            //评估分类
            if (!string.IsNullOrEmpty(AssessTypeName))
            {
                where += String.Format(" And SC.AssessTypeName = {0}", AssessTypeName.Trim());
            }
            //状态
            if (!string.IsNullOrEmpty(State))
            {
                where += String.Format(" And SC.State = {0}", State.Trim());
            }
            //基本类型
            //if (!string.IsNullOrEmpty(IsBasic))
            //{
            //    where += String.Format(" And TC.IsBasic = {0}", IsBasic.Trim());
            //}
            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += String.Format(" And convert(varchar,SC.CreateTime,120) like '%{0}%'", CreateTime.Trim());
            }
            List<Model.Storage.StorageComponentModel> list = bll.GetStorageComponentList(index, size, where);

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
        public ActionResult StorageComponentAmount(string componentId, string ComponentNumber, string ComponentName, string AssessTypeName, string State, string IsBasic, string CreateTime)
        {
            string where = string.Empty;
            where = " And SC.State != 10 ";
            //元件Id
            if (!string.IsNullOrEmpty(componentId))
            {
                where += String.Format(" And SC.ComponentId in ({0})", componentId.Trim());
            }
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
                where += String.Format(" And State = {0}", State.Trim());
            }
            ////基本类型
            //if (!string.IsNullOrEmpty(IsBasic))
            //{
            //    where += String.Format(" And TC.IsBasic = {0}", IsBasic.Trim());
            //}
            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += String.Format(" And convert(varchar,SC.CreateTime,120) like '%{0}%'", CreateTime.Trim());
            }
            int count = bll.StorageComponentAmount(where);
            return Content(count.ToString());
        }
        #endregion

        #region  添加 仓储评估元件表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(Model.Storage.StorageComponentModel model)
        {
            model.State = 0;// 状态 初始

            model.ComponentNumber = Auxiliary.CurCompanyAutoNum("SCN");//运输选择编号

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID



            int ComponentId = bll.AddStorageComponent(model);

            if (ComponentId > 0)
            {
                if (!string.IsNullOrEmpty(ComponentId.ToString()))
                {
                    List<Model.Storage.StorageComponentDetailModel> list =
                       Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.Storage.StorageComponentDetailModel>>(model.ComponentDetailList);

                    SCDbll.AddComponentList(list, ComponentId);
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
        public ActionResult EditStorageComponent(Model.Storage.StorageComponentModel model)
        {

            int rows = bll.EditStorageComponent(model);

            if (rows > 0)
            {
                int rows1 = SCDbll.EditStorageComponentDetail(model);
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

        #region 作废状态  运输评估元件表

        /// <summary>
        /// 变更 运输评估元件表
        /// </summary>
        /// <param name="TraChooseId">运输选择id</param>
        /// <param name="delUserId">作废负责人id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InvalidState(int tId)
        {
            Model.Storage.StorageComponentModel beforeModel = bll.GetModelByID(tId);

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
        
        #region 提交状态 运输评估元件表

        /// <summary>
        /// 变更 运输评估元件表
        /// </summary>
        /// <param name="ComponentId">运输评估元件id</param> 
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitStorageComponent(int tId)
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


        #region 明细表

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
        public ActionResult StorageComponentDetailList(int index, int size,int componentId)
        {
            string where = string.Empty;
            where = " And SCD.State = 1 ";
            //元件编号
            
            where += String.Format(" And SC.ComponentId = {0} ", componentId);

            List<Model.Storage.StorageComponentDetailModel> list = SCDbll.GetStorageComponentDetailList(index, size, where);

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
        public ActionResult StorageComponentDetailAmount(int componentId)
        {
            string where = string.Empty;
            where = " And SCD.State = 1 ";
            //元件编号

            where += String.Format(" And SC.ComponentId = {0} ", componentId);

            int count = SCDbll.StorageComponentDetailAmount(where);
            return Content(count.ToString());
        }
        #endregion

        #endregion

        #endregion

    }
}
