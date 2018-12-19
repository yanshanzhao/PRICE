//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-06    1.0        FJK        新建-绩效考核模板建立                    
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
 * 类名：TraMonthCheckFromController
 * 功能描述：运输月度绩效自定义表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraMonthCheckFromController : Controller
    {
        //
        // GET: /Tra/TraMonthCheckFrom/

        // 运输月度绩效自定义BLL
        TraMonthCheckFromBLL bll = new TraMonthCheckFromBLL();

        // 运输月度绩效自定义元件BLL
        TraMonthCheckFromComponentBLL TMCFCbll = new TraMonthCheckFromComponentBLL();

        // 运输月度考核表单自定义附件明细BLL
        TraMonthCheckFromAdjunctBLL TMCFAbll = new TraMonthCheckFromAdjunctBLL();

        // 月度考核自定义机构BLL
        TraMonthCheckFromDeparBLL TMCFDbll = new TraMonthCheckFromDeparBLL();

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
            TraMonthCheckFromModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            TraMonthCheckFromModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// Component 元件表(获取元件)
        /// </summary>
        /// <returns></returns>  
        public ActionResult Component(string url, string tId, int tCheckFromType, string tType)
        {
            ViewBag.url = url;
            ViewBag.ids = tId;
            ViewBag.CheckFromType = tCheckFromType;
            ViewBag.type = tType;
            return View();
        }

        /// <summary>
        /// AdjunctType 模板附件类型(新增附件类型)
        /// </summary>
        /// <returns></returns>  
        public ActionResult AdjunctType(string url, string checkFromId, string type)
        {
            ViewBag.url = url;
            ViewBag.checkFromId = checkFromId;
            @ViewBag.type = type;
            return View();
        }

        /// <summary>
        /// ComponentDetail 运输月度绩效自定义元件表(明细)
        /// </summary>  
        public ActionResult ComponentDetail(int tId)
        {
            ViewBag.CheckFromId = tId;
            return View();
        }

        /// <summary>
        /// AdjunctTypeDetail 运输月度考核表单自定义附件表(明细)
        /// </summary>  
        public ActionResult AdjunctTypeDetail(string url, int tId)
        {
            ViewBag.url = url;
            ViewBag.CheckFromId = tId;

            // 获取数据
            TraMonthCheckFromAdjunctModel model = TMCFAbll.GetModelByID(tId);

            return View(model); 
        }

        /// <summary>
        /// Allot
        /// </summary> 
        [Operate(Name = OperateEnum.Allot)]
        public ActionResult Allot(int tId)
        {
            // 获取数据
            TraMonthCheckFromModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// AllotDetail
        /// </summary>  
        public ActionResult AllotDetail(string url, int checkFromId)
        {
            ViewBag.url = url;
            ViewBag.CheckFromId = checkFromId;

            return View();
        } 
        #endregion

        #region 方法

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddMonthCheckFrom(TraMonthCheckFromModel tModel)
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
            tModel.CheckFromNumber = Auxiliary.CurCompanyAutoNum("TCF");

            // 新增(返回主键ID)
            int CheckFromId = bll.AddMonthCheckFrom(tModel);

            // 若主键>O(新增成功)
            if (CheckFromId > 0)
            {
                if (!string.IsNullOrEmpty(tModel.ComponentIdList))
                {
                    // 模版明细信息TraMonthCheckFromComponent(运输月度绩效自定义元件表) 
                    List<string> componentIdList = new List<string>(tModel.ComponentIdList.Split(','));
                    TMCFCbll.AddComponentList(componentIdList, CheckFromId);
                }

                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    // 新增模版附件类型TraMonthCheckFromAdjunct(运输月度考核表单自定义附件明细)
                    List<TempMonthCheckFromAdjunctModel> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempMonthCheckFromAdjunctModel>>(tModel.AdjunctList);

                    TMCFAbll.AddAdjunctTypeList(adjunctList, CheckFromId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        #region 获取已选择的元件信息

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="checkComponentId">元件ID</param> 
        /// <returns>Json</returns>
        public ActionResult ComponentList(int index, int size, string checkComponentId)
        {
            // 查询本公司内有效的(非作废)绩效元件信息
            string where = " State != 20 AND CompanyId =" + Auxiliary.CompanyID();

            // 元件编号
            where += string.Format(" And CheckComponentId IN ({0})", checkComponentId.Trim());
             
            // 运输月度考核元件List
            List<TraMonthCheckComponentModel> list = bll.ComponentList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="checkComponentId">元件ID</param> 
        /// <returns></returns>
        public int ComponentAmount(string checkComponentId)
        {
            // 查询本公司内有效的(非作废)绩效元件信息
            string where = " State != 20 AND CompanyId =" + Auxiliary.CompanyID();

            // 元件编号 
            where += string.Format(" And CheckComponentId IN ({0})", checkComponentId.Trim());
             
            return bll.ComponentAmount(where);
        }

        #endregion
        #endregion

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
        public ActionResult MonthCheckFromList(int index, int size, string checkFromNumber, string checkFromType, string state, string createTime)
        {
            // 查询本公司内有效的(非作废,非删除)绩效信息
            string where = " State != 30 AND State != 40 AND CompanyId =" + Auxiliary.CompanyID();

            // 自定义编号
            if (!string.IsNullOrEmpty(checkFromNumber))
            {
                where += string.Format(" And CheckFromNumber like '%{0}%'", checkFromNumber.Trim());
            }
             
            // 绩效类型 
            if (!string.IsNullOrEmpty(checkFromType))
            {
                where += string.Format(" And CheckFromType = {0}", checkFromType.Trim());
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

            // 运输月度绩效自定义List
            List<TraMonthCheckFromModel> list = bll.MonthCheckFromList(index, size, where);

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
        public int MonthCheckFromAmount(string checkFromNumber, string checkFromType, string state, string createTime)
        {
            // 查询本公司内有效的(非作废,非删除)绩效信息
            string where = " State != 30 AND State != 40 AND CompanyId =" + Auxiliary.CompanyID();

            // 自定义编号
            if (!string.IsNullOrEmpty(checkFromNumber))
            {
                where += string.Format(" And CheckFromNumber like '%{0}%'", checkFromNumber.Trim());
            }

            // 绩效类型 
            if (!string.IsNullOrEmpty(checkFromType))
            {
                where += string.Format(" And CheckFromType = {0}", checkFromType.Trim());
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

            return bll.MonthCheckFromAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditMonthCheckFrom(TraMonthCheckFromModel tModel)
        {
            // 编辑之前Model
            TraMonthCheckFromModel beforeModel = bll.GetModelByID(tModel.CheckFromId);

            // 编辑
            int row = bll.EditMonthCheckFrom(tModel);

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
            TraMonthCheckFromModel beforeModel = bll.GetModelByID(tId);

            // 查询同公司同考核类型有效(提交状态)数据
           int result = bll.MonthCheckFromAmount(" State = 10 AND CompanyId = " + beforeModel.CompanyId + " AND CheckFromType=" + beforeModel.CheckFromType); 
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Exist, new { Detail = "提交", Id = tId, State = "初始" });
                return Json(new { flag = "exist", content = "本公司已存在同考核类型有效的月度考核模板！" });
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
            TraMonthCheckFromModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId, beforeModel.State);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 将明细中的数据变为无效(根据CheckFromId)
                TMCFCbll.InvalidByCheckFromId(tId);

                // 将模板附件类型中的数据变为无效(同CheckFromId)
                TMCFAbll.InvalidByCheckFromId(tId);

                // 将分配的机构供应商变为无效(同CheckFromId)
                TMCFDbll.InvalidByCheckFromId(tId);

                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
          
        #region 明细 运输月度绩效自定义元件表

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
            int CheckFromId = TMCFCbll.AddComponentList(ComponentList, checkFromId);

            // 新增之后的Model
            // TraMonthCheckFromComponentModel afterModel = TMCFCbll.GetModelByID(CheckFromId);

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
        /// 明细List 运输月度绩效自定义元件表
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <returns>Json</returns>
        public ActionResult MonthCheckFromComponentList(int index, int size, int tId)
        {
            // where条件 (有效状态)
            string where = " TMCFC.State = 1 AND TMCFC.CheckFromId =" + tId;

            // 明细List 
            List<TraMonthCheckFromComponentModel> FromComponentList = TMCFCbll.MonthCheckFromComponentList(index, size, where);

            return Json(FromComponentList);
        }

        /// <summary>
        /// 明细count 运输月度绩效自定义元件表
        /// </summary>
        /// <param name="checkFromNumber">自定义编号</param>
        /// <param name="checkFromType">绩效类型</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int MonthCheckFromComponentAmount(int tId)
        { 
            return TMCFCbll.MonthCheckFromComponentAmount(tId);
        }

        /// <summary>
        /// 重置排序序号 运输月度绩效自定义元件表
        /// </summary> 
        /// <returns></returns>
        public ActionResult ChangeSort(int tId,int tSort)
        {
            int row = TMCFCbll.ChangeSort(tId, tSort);
            if (row>0)
            {
                return Json(new { flag = "success" });
            }
            return Json(new { flag = "fail", content = "重置排序序号失败！" }); 
        }

        /// <summary>
        /// 作废 运输月度绩效自定义元件表
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidMonthCheckFromComponent(int tId)
        {
            // 作废之前Model
            TraMonthCheckFromComponentModel beforeModel = TMCFCbll.GetModelByID(tId);
             
            // 作废(更改状态)
            int row = TMCFCbll.InvalidState(tId);

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

        #region 模板附件类型 运输月度考核表单自定义附件明细

        /// <summary>
        /// 批量新增 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddMonthCheckFromAdjunct(TraMonthCheckFromAdjunctModel tModel)
        {
            string where = " AND AdjunctName ='"+ tModel.AdjunctName+"'";

            // 是否存在同附件名称的有效的数据(同CheckFromId)
            int row = TMCFAbll.MonthCheckFromAdjunctAmount(tModel.CheckFromId, where);

            if (row>0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, tModel); 
                return Json(new { flag = "exist", content = "已存在同附件名称的有效的数据！" });
            }

            // 状态默认有效
            tModel.State = 1;

            // 新增(返回主键ID)
            int CheckFromAdjunctId = TMCFAbll.AddMonthCheckFromAdjunct(tModel);
             
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
        /// 模板附件类型List 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <returns>Json</returns>
        public ActionResult MonthCheckFromAdjunctList(int tId)
        {
            // where条件 (有效状态)
            string where = " State = 1 AND CheckFromId =" + tId;

            // 模板附件类型List
            List<TraMonthCheckFromAdjunctModel> FromAdjunctList = TMCFAbll.MonthCheckFromAdjunctList(where);

            return Json(FromAdjunctList);
        }

        /// <summary>
        /// 模板附件类型count 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="checkFromNumber">自定义编号</param>
        /// <param name="checkFromType">绩效类型</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int MonthCheckFromAdjunctAmount(int tId)
        {
            return TMCFAbll.MonthCheckFromAdjunctAmount(tId,"");
        }

        /// <summary>
        /// 编辑 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditMonthCheckFromAdjunct(TraMonthCheckFromAdjunctModel tModel)
        {
            // 编辑之前Model
            TraMonthCheckFromAdjunctModel beforeModel = TMCFAbll.GetModelByID(tModel.CheckFromAdjunctId);

            // 编辑
            int row = TMCFAbll.EditMonthCheckFromAdjunct(tModel);

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
        /// 作废 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidMonthCheckFromAdjunct(int tId)
        {
            // 作废之前Model
            TraMonthCheckFromAdjunctModel beforeModel = TMCFAbll.GetModelByID(tId);
             
            // 作废(更改状态)
            int row = TMCFAbll.InvalidState(tId);

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
