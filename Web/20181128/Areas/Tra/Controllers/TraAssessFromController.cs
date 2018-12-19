//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-09-12    1.0       MY         新建
//-------------------------------------------------------------------------
#region 参考
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SRM.Model.Tra;
using SRM.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
#endregion

/*********************************
 * 类名：TraComponentController
 * 功能描述：运输评估元件自定义表 控制器
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraAssessFromController : Controller
    {
        //
        // GET: /Tra/TraAssessFrom/ 
        //运输评估元件自定义表
        private BLL.Tra.TraAssessFromBLL bll = new BLL.Tra.TraAssessFromBLL();

        //运输评估自定义元件表BLL
        private BLL.Tra.TraAssessFromComponentBLL TAFCBbll = new BLL.Tra.TraAssessFromComponentBLL();

        // 运输评估表单自定义附件明细BLL
        private BLL.Tra.TraAssessFromAdjunctBLL TAFAbll = new BLL.Tra.TraAssessFromAdjunctBLL();

        #region 页面

        /// <summary>
        /// index
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
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            Model.Tra.TraAssessFromModel model = bll.GetModelByID(tId);
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
            Model.Tra.TraAssessFromModel model = bll.GetModelByID(tId);
            return View(model);
        }

        /// <summary>
        /// Component 元件表(获取元件)
        /// </summary>
        /// <returns></returns>  
        public ActionResult Component(string url, string tId, int tAssessFromType, string tType)
        {
            ViewBag.url = url;
            ViewBag.ids = tId;
            ViewBag.AssessFromType = tAssessFromType;
            ViewBag.type = tType;
            return View();
        }

        /// <summary>
        /// AdjunctType 模板附件类型(新增附件类型)
        /// </summary>
        /// <returns></returns>  
        public ActionResult AdjunctType(string url, string assessFromId, string type)
        {
            ViewBag.url = url;
            ViewBag.assessFromId = assessFromId;
            @ViewBag.type = type;
            return View();
        }

        #endregion

        #region 方法
         
        /// <summary>
        /// 运输评估表单自定义表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(int index,int size,string AssessFromNumber, string tAssessFromName, string tAssessFromType, string State,string CreateTime)
        {
            string where = string.Empty;

            where = " And TAF.State NOT IN (30,40) And TAF.CompanyId = " + Auxiliary.CompanyID();

            //元件编号
            if (!string.IsNullOrEmpty(AssessFromNumber))
            {
                where += String.Format(" And AssessFromNumber like '%{0}%'", AssessFromNumber.Trim());
            }
             
            //状态
            if (!string.IsNullOrEmpty(State))
            {
                where += String.Format(" And TAF.State = {0}", State.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(tAssessFromType))
            {
                where += String.Format(" And TAF.AssessFromType = {0}", tAssessFromType.Trim());
            }

            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += String.Format(" And convert(varchar,TC.CreateTime,120) like '%{0}%'", CreateTime.Trim());
            }

            List<Model.Tra.TraAssessFromModel> list = bll.TraAssessFromList(index, size, where);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
    
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
        public ActionResult TraAssessFromAmount(string AssessFromNumber,string AssessFromName, string tAssessFromType, string State, string CreateTime)
        {
            string where = string.Empty;

            where = " And TAF.State NOT IN (30,40) And TAF.CompanyId = " + Auxiliary.CompanyID();

            //元件编号
            if (!string.IsNullOrEmpty(AssessFromNumber))
            {
                where += String.Format(" And AssessFromNumber like '%{0}%'", AssessFromNumber.Trim());
            }
 
            //状态
            if (!string.IsNullOrEmpty(State))
            {
                where += String.Format(" And TAF.State = {0}", State.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(tAssessFromType))
            {
                where += String.Format(" And TAF.AssessFromType = {0}", tAssessFromType.Trim());
            }

            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += String.Format(" And convert(varchar,TC.CreateTime,120) like '%{0}%'", CreateTime.Trim());
            }
            int count = bll.TraAssessFromAmount(where);
            return Content(count.ToString());
        }
     
        [HttpPost]
        public ActionResult Add(Model.Tra.TraAssessFromModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认创建 0
            tModel.State = 0;

            // 月度考核自定义编号 
            tModel.AssessFromNumber = Auxiliary.CurCompanyAutoNum("TFN");
            
            if (tModel.AssessFromRemark == "" || tModel.AssessFromRemark == null)
            {
                tModel.AssessFromRemark = "";
            }


            // 新增(返回主键ID)
            int AssessFromId = bll.AddTraAssessFrom(tModel);

            // 若主键>O(新增成功)
            if (AssessFromId > 0)
            {
                if (!string.IsNullOrEmpty(tModel.ComponentIdList))
                {
                    // 模版明细信息TraMonthCheckFromComponent(运输月度绩效自定义元件表) 
                    List<string> componentIdList = new List<string>(tModel.ComponentIdList.Split(','));
                    TAFCBbll.AddComponentList(componentIdList, AssessFromId);
                }

                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    // 新增模版附件类型 TraAssessFromAdjunct (运输月度考核表单自定义附件明细)
                    List<TraAssessFromAdjunctModel> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TraAssessFromAdjunctModel>>(tModel.AdjunctList);

                    TAFAbll.AddAdjunctTypeList(adjunctList, AssessFromId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }
   
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult EditTraAssessFrom(Model.Tra.TraAssessFromModel model)
        {
            int count = bll.EditTraAssessFrom(model);
            if (count > 0)
            {

                if (!string.IsNullOrEmpty(model.AdjunctList))
                {
                    // 新增模版附件类型 TraAssessFromAdjunct (运输月度考核表单自定义附件明细)
                    List<TraAssessFromAdjunctModel> adjunctList = 
                        Newtonsoft.Json.JsonConvert.DeserializeObject<List<TraAssessFromAdjunctModel>>(model.AdjunctList);

                    TAFAbll.AddAdjunctTypeList(adjunctList, model.AssessFromId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success" });
            }
            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
          
        /// <summary>
        /// 变更 状态 40:删除 
        /// </summary>
        /// <param name="AssessFromId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InvalidState(int tId)
        {
            Model.Tra.TraAssessFromModel beforeModel = bll.GetModelByID(tId);

            int row = bll.DelTraAssessFromByAssessFromId(tId);
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }
        
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitTraAssessFrom(int tId)
        {

            // 提交(更改状态)
            int row = bll.SubmitStste(tId);

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
       
        /// <summary>
        /// 批量新增 运输月度绩效自定义元件表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddComponentList(string componentList, int checkFromId)
        {
            // 反序列化
            List<string> ComponentList = new List<string>(componentList.Split(','));

            // 新增(返回主键ID)
            int rows = TAFCBbll.AddComponentList(ComponentList, checkFromId);
             
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Del(int id)
        {
            int row =  TAFCBbll.UpdateStateToDel(id);

            // 若主键>O(新增成功)
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
          
        /// <summary>
        /// 批量新增 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddTraAssessFromAdjunct(Model.Tra.TraAssessFromAdjunctModel tModel)
        {
            string where = " AND AdjunctName ='" + tModel.AdjunctName + "'";

            // 是否存在同附件名称的有效的数据(同CheckFromId)
            int row = TAFAbll.TraAssessFromAdjunctAmount(tModel.AssessFromId.ToString(), where);

            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, tModel);
                return Json(new { flag = "exist", content = "已存在同附件名称的有效的数据！" });
            }

            // 状态默认有效
            tModel.State = 1;

            // 新增(返回主键ID)
            int FromAdjunctId = TAFAbll.AddTraAssessFromAdjunct(tModel);

            // 若主键>O(新增成功)
            if (FromAdjunctId > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }
          
        #region 附件表
        
        #region 获取实体 运输评估表单自定义附件明细
        /// <summary>
        ///  获取实体 运输评估表单自定义附件明细
        /// </summary>
        public ActionResult GetAdjunctModelByID(string id)
        {
            Model.Tra.TraAssessFromAdjunctModel model =  TAFAbll.GetModelByID(id);
            
            return Content(JsonConvert.SerializeObject(model));
        }
        #endregion

        #region 修改附件表

        /// <summary>
        /// 修改附件表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditAssessFromAdjunct(Model.Tra.TraAssessFromAdjunctModel model)
        {
            int count = TAFAbll.UpdateTraAssessFromAdjunct(model);
            if (count > 0 )
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success" });
            }
            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 编辑时用到 附件列表   分页查询And 总数查询
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdjunctList(int index,int size,string id)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                where += string.Format(" And AssessFromId = {0}", id.Trim());
            }
           List<Model.Tra.TraAssessFromAdjunctModel> list =  TAFAbll.TraAssessFromAdjunctList(index,size,where);

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }
        [HttpPost]
        public ActionResult AdjunctAmount(string id)
        {
            string where = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                where += string.Format(" And AssessFromId = {0}", id.Trim());
            }
            int count = TAFAbll.TraAssessFromAdjunctAmount(id,where);

            return Content(count.ToString());
        }

        #endregion

        #endregion

        #region 获取已选择的元件信息

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="checkComponentId">元件ID</param> 
        /// <returns>Json</returns>
        public ActionResult ComponentList(int index, int size, string componentId)
        {
            // 查询本公司内有效的(非作废)绩效元件信息
            string where = " And TAFC.State != 0 AND TC.CompanyId =" + Auxiliary.CompanyID();

            // 元件编号
            if (!string.IsNullOrEmpty(componentId))
            {
                where += string.Format(" And TAFC.AssessFromId IN ({0})", componentId.Trim());
            }

            // 运输月度考核元件List
            List<TraAssessFromComponentModel> list = TAFCBbll.TraAssessFromComponentList(index, size, where);

            // DateTime类型字段转换
            //IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            //timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="componentId">元件ID</param> 
        /// <returns></returns>
        public int ComponentAmount(string componentId)
        {
            // 查询本公司内有效的(非作废)绩效元件信息
            string where = " And TAFC.State != 0 AND TC.CompanyId =" + Auxiliary.CompanyID();

            // 元件编号
            if (!string.IsNullOrEmpty(componentId))
            {
                where += string.Format(" And TAFC.AssessFromId IN ({0})", componentId.Trim());
            }

            return TAFCBbll.AssessFromComponentIdAmount(where);
        }

        #endregion
         
        #endregion
    }
}
