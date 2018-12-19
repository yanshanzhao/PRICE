//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-11-02    1.0        FJK        新建-年度评估模板建立                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.Model.Supplier;
using System.Linq;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using SRM.BLL.Supplier;
#endregion
/*********************************
 * 类名：TraYearCheckFromController
 * 功能描述：运输年度绩效自定义表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraYearCheckFromController : Controller
    {
        //
        // GET: /Tra/TraYearCheckFrom/

        // 运输年度绩效自定义BLL
        TraYearCheckFromBLL bll = new TraYearCheckFromBLL();

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
            TraYearCheckFromModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            TraYearCheckFromModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 自定义元件
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="tId">元件ID</param>
        /// <param name="tCheckFromType">绩效类型</param>
        /// <param name="tType">新增/编辑</param>
        /// <returns></returns>
        public ActionResult Component(string url, string tId, string tType)
        {
            ViewBag.url = url;
            ViewBag.ids = tId; 
            ViewBag.type = tType;
            return View();
        }

        /// <summary>
        /// AdjunctType 模板附件类型(新增附件类型)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="checkFromId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult AdjunctType(string url, string checkFromId, string type)
        {
            ViewBag.url = url;
            ViewBag.checkFromId = checkFromId;
            @ViewBag.type = type;
            return View();
        }

        /// <summary>
        /// ComponentDetail 运输年度绩效自定义元件表(明细)
        /// </summary>  
        public ActionResult ComponentDetail(int tId)
        {
            ViewBag.CheckFromId = tId;
            return View();
        }

        /// <summary>
        /// AdjunctTypeDetail 运输年度考核表单自定义附件表(明细)
        /// </summary>  
        public ActionResult AdjunctTypeDetail(string url, int tId)
        {
            ViewBag.url = url;
            ViewBag.CheckFromId = tId;

            // 获取数据
            TraYearCheckFromAdjunctModel model = new TraYearCheckFromAdjunctBLL().GetModelByID(tId); 

            return View(model);
        }
        #endregion

        #region 方法
         
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddYearCheckFrom(TraYearCheckFromModel tModel)
        {
            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 创建机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 创建人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 状态默认创建 0
            tModel.State = 0;

            // 年度考核自定义编号 
            tModel.CheckFromNumber = Auxiliary.CurCompanyAutoNum("TYF");

            // 新增(返回主键ID)
            int CheckFromId = bll.AddYearCheckFrom(tModel);

            // 若主键>O(新增成功)
            if (CheckFromId > 0)
            {
                if (!string.IsNullOrEmpty(tModel.ComponentIdList))
                {
                    // 模版明细信息TraYearCheckFromComponent(运输年度绩效自定义元件表) 
                    List<string> componentIdList = new List<string>(tModel.ComponentIdList.Split(','));
                    new TraYearCheckFromComponentBLL().AddComponentList(componentIdList, CheckFromId);
                }

                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    // 新增模版附件类型TraYearCheckFromAdjunct(运输年度考核表单自定义附件明细)
                    List<TempYearCheckFromAdjunctModel> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempYearCheckFromAdjunctModel>>(tModel.AdjunctList);

                    new TraYearCheckFromAdjunctBLL().AddAdjunctTypeList(adjunctList, CheckFromId);
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
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="checkFromNumber">自定义编号</param>
        /// <param name="checkFromType">绩效类型</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns>Json</returns>
        public ActionResult YearCheckFromList(int index, int size, string checkFromNumber, string checkFromType, string state, string createTime)
        {
            // 查询本公司内有效的(非作废,非删除)绩效信息
            string where = " State NOT IN (30,40) AND CompanyId =" + Auxiliary.CompanyID();

            // 自定义编号
            if (!string.IsNullOrEmpty(checkFromNumber))
            {
                where += string.Format(" And CheckFromNumber like '%{0}%'", checkFromNumber.Trim());
            }
             
            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            // 运输年度绩效自定义List
            List<TraYearCheckFromModel> list = bll.YearCheckFromList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="checkFromNumber">自定义编号</param>
        /// <param name="checkFromType">绩效类型</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int YearCheckFromCount(string checkFromNumber, string checkFromType, string state, string createTime)
        {
            // 查询本公司内有效的(非作废,非删除)绩效信息
            string where = " State NOT IN (30,40) AND CompanyId =" + Auxiliary.CompanyID();

            // 自定义编号
            if (!string.IsNullOrEmpty(checkFromNumber))
            {
                where += string.Format(" And CheckFromNumber like '%{0}%'", checkFromNumber.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            return bll.YearCheckFromCount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditYearCheckFrom(TraYearCheckFromModel tModel)
        {
            // 编辑之前Model
            TraYearCheckFromModel beforeModel = bll.GetModelByID(tModel.CheckFromId);

            // 编辑
            int row = bll.EditYearCheckFrom(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
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
        /// 提交
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitState(int tId)
        {
            // 编辑之前Model
            TraYearCheckFromModel beforeModel = bll.GetModelByID(tId);

            // 查询同公司是否存在有效的(提交状态)年度考核模版信息
            int result = bll.YearCheckFromCount(" State = 10 AND CompanyId = " + beforeModel.CompanyId);
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Exist, new { Detail = "提交", Id = tId, State = "初始" });
                return Json(new { flag = "exist", content = "本公司已存在有效的年度考核模版！" });
            }

            // 提交(更改状态)
            int row = bll.SubmitState(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "提交", Id = tId, State = "提交" });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "提交", Id = tId, State = "初始" });
            return Json(new { flag = "fail", content = "提交失败！" });
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            // 作废之前Model
            TraYearCheckFromModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId, beforeModel.State);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 将明细中的数据变为无效(根据CheckFromId)
                new TraYearCheckFromComponentBLL().InvalidByCheckFromId(tId);

                // 将模板附件类型中的数据变为无效(同CheckFromId)
                new TraYearCheckFromAdjunctBLL().InvalidByCheckFromId(tId);
                 
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }

        #region 年评估元件

        /// <summary>
        /// 数据集 获取已选择的元件信息
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="checkComponentId">元件ID</param> 
        /// <returns>Json</returns>
        public ActionResult ComponentList(int index, int size, string tIds,string tType)
        { 
            // 条件
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询本公司内有效的绩效元件信息
                where = " State = 1 AND CompanyId =" + Auxiliary.CompanyID();

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And CheckComponentId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And CheckComponentId NOT IN (SELECT CheckComponentId FROM TraYearCheckFromComponent WHERE CheckFromId ={0} AND State =1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where = string.Format(" CheckComponentId IN ({0})", tIds.Trim());
            }
             
            // 运输年度考核元件List
            List<TraYearCheckComponentModel> list = bll.ComponentList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数 获取已选择的元件信息
        /// </summary>
        /// <param name="checkComponentId">元件ID</param> 
        /// <returns></returns>
        public int ComponentCount(string tIds, string tType)
        {
            // 条件
            string where = "";

            // 是否根据ID查询
            if (tType != null)
            {
                // 查询本公司内有效的绩效元件信息
                where = " State = 1 AND CompanyId =" + Auxiliary.CompanyID();

                // 排除已有的ID 
                if (!string.IsNullOrEmpty(tIds))
                {
                    if (tType == "add")
                    {
                        where += string.Format(" And CheckComponentId NOT IN ({0})", tIds.Trim());
                    }
                    else if (tType == "edit")
                    {
                        where += string.Format(" And CheckComponentId NOT IN (SELECT CheckComponentId FROM TraYearCheckFromComponent WHERE CheckFromId ={0} AND State =1)", tIds.Trim());
                    }
                }
            }
            else
            {
                where = string.Format(" CheckComponentId IN ({0})", tIds.Trim());
            }

            return bll.ComponentCount(where);
        }

        #endregion

        #region 明细 运输年度绩效自定义元件表

        /// <summary>
        /// 批量新增 运输年度绩效自定义元件表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddComponentList(string componentList, int checkFromId)
        {
            // 反序列化
            List<string> ComponentList = new List<string>(componentList.Split(','));

            // 新增(返回主键ID)
            int CheckFromId =  new TraYearCheckFromComponentBLL().AddComponentList(ComponentList, checkFromId);
             
            // 若主键>O(新增成功)
            if (CheckFromId > 0)
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
        /// 明细List 运输年度绩效自定义元件表
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <returns>Json</returns>
        public ActionResult YearCheckFromComponentList(int index, int size, int tId)
        {
            // where条件 (有效状态)
            string where = " TYCFC.State = 1 AND TYCFC.CheckFromId =" + tId;

            // 明细List 
            List<TraYearCheckFromComponentModel> FromComponentList = new TraYearCheckFromComponentBLL().YearCheckFromComponentList(index, size, where);

            return Json(FromComponentList);
        }

        /// <summary>
        /// 明细count 运输年度绩效自定义元件表
        /// </summary>
        /// <param name="checkFromNumber">自定义编号</param>
        /// <param name="checkFromType">绩效类型</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int YearCheckFromComponentCount(int tId)
        {
            return new TraYearCheckFromComponentBLL().YearCheckFromComponentCount(tId);
        }

        /// <summary>
        /// 重置排序序号 运输年度绩效自定义元件表
        /// </summary> 
        /// <returns></returns>
        public ActionResult ChangeSort(int tId, int tSort)
        {
            int row = new TraYearCheckFromComponentBLL().ChangeSort(tId, tSort);
            if (row > 0)
            {
                return Json(new { flag = "success" });
            }
            return Json(new { flag = "fail", content = "重置排序序号失败！" });
        }

        /// <summary>
        /// 作废 运输年度绩效自定义元件表
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidYearCheckFromComponent(int tId)
        {
            // 作废之前Model
            TraYearCheckFromComponentModel beforeModel = new TraYearCheckFromComponentBLL().GetModelByID(tId); 

            // 作废(更改状态)
            int row = new TraYearCheckFromComponentBLL().InvalidState(tId); 

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }

        #endregion

        #region 模板附件类型 运输年度考核表单自定义附件明细

        /// <summary>
        /// 批量新增 运输年度考核表单自定义附件明细
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddYearCheckFromAdjunct(TraYearCheckFromAdjunctModel tModel)
        {
            string where = " AND AdjunctName ='" + tModel.AdjunctName + "'";

            // 是否存在同附件名称的有效的数据(同CheckFromId)
            int row = new TraYearCheckFromAdjunctBLL().YearCheckFromAdjunctCount(tModel.CheckFromId, where);

            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, tModel);
                return Json(new { flag = "exist", content = "已存在同附件名称的有效的数据！" });
            }

            // 状态默认有效
            tModel.State = 1;

            //// 新增(返回主键ID)
            int CheckFromAdjunctId = new TraYearCheckFromAdjunctBLL().AddYearCheckFromAdjunct(tModel); 

            // 若主键>O(新增成功)
            if (CheckFromAdjunctId > 0)
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
        /// 模板附件类型List 运输年度考核表单自定义附件明细
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <returns>Json</returns>
        public ActionResult YearCheckFromAdjunctList(int tId)
        {
            // where条件 (有效状态)
            string where = " State = 1 AND CheckFromId =" + tId;

            // 模板附件类型List
            List<TraYearCheckFromAdjunctModel> FromAdjunctList = new TraYearCheckFromAdjunctBLL().YearCheckFromAdjunctList(where); 

            return Json(FromAdjunctList);
        }

        /// <summary>
        /// 模板附件类型count 运输年度考核表单自定义附件明细
        /// </summary>
        /// <param name="checkFromNumber">自定义编号</param>
        /// <param name="checkFromType">绩效类型</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int YearCheckFromAdjunctCount(int tId)
        {
            return 0;/*new TraYearCheckFromAdjunctBLL().YearCheckFromAdjunctCount(tId, "");*/
        }

        /// <summary>
        /// 编辑 运输年度考核表单自定义附件明细
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditYearCheckFromAdjunct(TraYearCheckFromAdjunctModel tModel)
        {
            // 编辑之前Model
            TraYearCheckFromAdjunctModel beforeModel = new TraYearCheckFromAdjunctBLL().GetModelByID(tModel.CheckFromAdjunctId);

            // 编辑
            int row = new TraYearCheckFromAdjunctBLL().EditYearCheckFromAdjunct(tModel);

            // 若影响行数>O(修改成功)
            if (row > 0)
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
        /// 作废 运输年度考核表单自定义附件明细
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidYearCheckFromAdjunct(int tId)
        {
            // 作废之前Model
            TraYearCheckFromAdjunctModel beforeModel = new TraYearCheckFromAdjunctBLL().GetModelByID(tId);

            // 作废(更改状态)
            int row = new TraYearCheckFromAdjunctBLL().InvalidState(tId); 

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion
        #endregion
    }
}
