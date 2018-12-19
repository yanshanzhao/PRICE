using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRM.Web.Areas.Storage.Controllers
{
    public class StorageFromController : Controller
    {
        private BLL.Storage.StorageFromBLL bll = new BLL.Storage.StorageFromBLL();

        private BLL.Storage.StorageFromComponentBLL SFCbll = new BLL.Storage.StorageFromComponentBLL();

        private BLL.Storage.StorageFromAdjunctBLL SFAbll = new BLL.Storage.StorageFromAdjunctBLL();
        //
        // GET: /Storage/StorageFrom/

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
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            Model.Storage.StorageFromModel model = bll.GetModelByID(tId);
            return View(model);
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            Model.Storage.StorageFromModel model = bll.GetModelByID(tId);
            return View(model);
        }
        /// <summary>
        /// Component 元件表(获取元件)
        /// </summary>
        /// <returns></returns>  
        public ActionResult Component(string url, string tId, int tFromType, string tType)
        {
            ViewBag.url = url;
            ViewBag.ids = tId;
            ViewBag.FromType = tFromType;
            ViewBag.type = tType;
            return View();
        }

        /// <summary>
        /// AdjunctType 模板附件类型(新增附件类型)
        /// </summary>
        /// <returns></returns>  
        public ActionResult AdjunctType(string url, string fromId, string type)
        {
            ViewBag.url = url;
            ViewBag.FromId = fromId;
            @ViewBag.type = type;
            return View();
        }


        #region 方法

        #region  查询列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(int index,int size,string fromNumber,string createTime,string state)
        {

            string where = string.Empty;

            where = " And State in (0,10) ";

            //元件编号
            if (!string.IsNullOrEmpty(fromNumber))
            {
                where += String.Format(" And FromNumber like '%{0}%'", fromNumber.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(state))
            {
                where += String.Format(" And State = {0}", state.Trim());
            }
            

            //创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += String.Format(" And convert(varchar,SF.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            List<Model.Storage.StorageFromModel> list = bll.StorageFromList(index, size, where);

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
        public ActionResult StorageFromAmount(string fromNumber, string createTime, string state)
        {
            string where = string.Empty;

            where = " And State in (0,10) ";

            //元件编号
            if (!string.IsNullOrEmpty(fromNumber))
            {
                where += String.Format(" And FromNumber like '%{0}%'", fromNumber.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(state))
            {
                where += String.Format(" And State = {0}", state.Trim());
            }


            //创建时间
            if (!string.IsNullOrEmpty(createTime))
            {
                where += String.Format(" And convert(varchar,SF.CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            int count = bll.StorageFromAmount(where);
            return Content(count.ToString());
        }
        #endregion

        #region 新增
        [HttpPost]
        public ActionResult Add(Model.Storage.StorageFromModel model)
        {
            model.State = 0;// 状态 初始

            model.FromNumber = Auxiliary.CurCompanyAutoNum("SCF");//运输选择编号

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            int FromId = bll.AddStorageFrom(model);

            // 若主键>O(新增成功)
            if (FromId > 0)
            {
                if (!string.IsNullOrEmpty(model.ComponentIdList))
                {
                    // 模版明细信息TraMonthCheckFromComponent(运输月度绩效自定义元件表) 
                    List<string> componentIdList = new List<string>(model.ComponentIdList.Split(','));
                    SFCbll.AddComponentList(componentIdList, FromId);
                }

                if (!string.IsNullOrEmpty(model.AdjunctList))
                {
                    // 新增模版附件类型 TraAssessFromAdjunct (运输月度考核表单自定义附件明细)
                    List<Model.Storage.StorageFromAdjunctModel> adjunctList = 
                        Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.Storage.StorageFromAdjunctModel>>(model.AdjunctList);

                    SFAbll.AddAdjunctTypeList(adjunctList, FromId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        #endregion

        #region 作废 软删除 (状态不可用)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssessFromId"></param>
        /// <returns></returns>
        public ActionResult InvalidState(int tId)
        {
            int count = bll.DelStorageFromByFromId(tId);
            
            // 若影响行数>O(修改成功)
            if (count > 0)
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

        #region 提交 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssessFromId"></param>
        /// <returns></returns>
        public ActionResult SubmitState(int tId)
        {
            int count = bll.SubmitStste(tId);
            
            // 若影响行数>O(修改成功)
            if (count > 0)
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


        #region 编辑
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Model.Storage.StorageFromModel model)
        {
            int count = bll.UpdateStorageFrom(model);
            if (count > 0)
            {

                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "提交成功" });
            }
            // 系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, model);
            return Json(new { flag = "fail", content = "提交失败" });
        }

        #endregion

        #endregion

        #region 仓储评估元件明细表

        #region 仓储表单自定义元件表

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="componentId">元件ID</param> 
        /// <returns>Json</returns>
        public ActionResult ComponentList(int index, int size, string fromId)
        {
            // 查询本公司内有效的(非作废)绩效元件信息
            string where = " And SF.State = 0 And SC.CompanyId =" + Auxiliary.CompanyID();

            // 元件编号
            where += String.Format(" And SFC.FromId IN ({0}) ", fromId);

            // 运输月度考核元件List
            List<Model.Storage.StorageFromComponentModel> list = SFCbll.StorageFromComponentList(index, size, where);

            //// DateTime类型字段转换
            //IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            //timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";, Newtonsoft.Json.Formatting.Indented, timeFormat
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="componentId">元件ID</param> 
        /// <returns></returns>
        public int ComponentAmount(string fromId)
        {
            // 查询本公司内有效的(非作废)绩效元件信息
            string where = " And SF.State = 0 AND SC.CompanyId =" + Auxiliary.CompanyID();

            // 元件编号 
            where += String.Format(" And SFC.FromId IN ({0}) ", fromId);

            return SFCbll.StorageFromComponentAmount(where);
        }

        #endregion

        #region 添加
        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentList"></param>
        /// <param name="fromId"></param>
        /// <returns></returns>
        public ActionResult AddStorageFromComponent(string componentList, int fromId)
        {

            // 反序列化
            List<string> ComponentList = new List<string>(componentList.Split(','));

            // 新增(返回主键ID)
            int rows = SFCbll.AddComponentList(ComponentList, fromId);


            // 若主键>O(新增成功)
            if (rows > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
            return Json(new { flag = "fail" });
        }

        #endregion

        #region 删除
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelStorageFromComponentById(int id)
        {
            int row = SFCbll.DelStorageFromComponentById(id);

            // 行数>O(删除成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, null);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, null);
            return Json(new { flag = "fail" });
        }

        #endregion

        #endregion

        #region 仓储附件表  功能

        #region  新增 仓储附件表

        #region 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="fromId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FromAdjunctList(int index,int size,string fromId)
        {
            string where = string.Empty;

            //状态
            if (!string.IsNullOrEmpty(fromId))
            {
                where += String.Format(" And FromId = {0} ", fromId.Trim());
            }

            List<Model.Storage.StorageFromAdjunctModel> list = SFAbll.StorageFromAdjunctPageList(index,size,where);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FromAdjunctAmount(string fromId)
        {

            string where = string.Empty;

            //状态
            if (!string.IsNullOrEmpty(fromId))
            {
                where += String.Format(" And FromId = {0} ", fromId.Trim());
            }

             int count = SFAbll.StorageFromAdjunctAmount(fromId,where);

            return Content(count.ToString());
        }
        #endregion


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddStorageFromAdjunct(Model.Storage.StorageFromAdjunctModel model)
        {
            //状态默认有效
            model.State = 1;

            int rows = SFAbll.AddStorageFromAdjunct(model);
            if (rows > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success" });
            }
            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "success" });
        }


        #endregion

        #endregion

    }
}
