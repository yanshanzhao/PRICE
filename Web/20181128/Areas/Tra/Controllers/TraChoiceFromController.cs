// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-30    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using System.Web.Mvc;
using System;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using System.Linq;
#endregion
/*********************************
 * 类名：TraChoiceFromController
 * 功能描述：评分模板建立 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraChoiceFromController : Controller
    {
        //
        // GET: /Tra/TraChoiceFrom/

        TraChoiceFromBLL bll = new TraChoiceFromBLL();

        // 运输月度绩效自定义元件BLL
        TraChoiceFromComponentBLL TMCFCbll = new TraChoiceFromComponentBLL();

        // 运输月度考核表单自定义附件明细BLL
        TraChoiceFromAdjunctBLL TMCFAbll = new TraChoiceFromAdjunctBLL();

        //// 月度考核自定义机构BLL
        //TraMonthCheckFromDeparBLL TMCFDbll = new TraMonthCheckFromDeparBLL();

        #region 页面

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
            TraChoiceFromModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            TraChoiceFromModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// ComponentDetail 运输月度绩效自定义元件表(明细)
        /// </summary>  
        public ActionResult ComponentDetail(int tId)
        {
            ViewBag.ChoiceFromId = tId;
            return View();
        }

        /// <summary>
        /// AdjunctType 模板附件类型(新增附件类型)
        /// </summary>
        /// <returns></returns>  
        public ActionResult AdjunctType(string url, string choiceFromId, string type)
        {
            ViewBag.url = url;
            ViewBag.ChoiceFromId = choiceFromId;
            ViewBag.type = type;
            return View();
        }

        /// <summary>
        /// Component 元件表(获取元件)
        /// </summary>
        /// <returns></returns>  
        public ActionResult Component(string url, string tId, string tType)
        {
            ViewBag.url = url;
            ViewBag.ids = tId;
            ViewBag.type = tType;
            return View();
        }

        /// <summary>
        /// AdjunctTypeDetail 运输月度考核表单自定义附件表(明细)
        /// </summary>  
        public ActionResult AdjunctTypeDetail(string url, int tId)
        {
            ViewBag.url = url;
            ViewBag.ChoiceFromId = tId;

            // 获取数据
            TraChoiceFromAdjunctModel model = TMCFAbll.GetModelByID(tId);

            return View(model);
        }
        #endregion


        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddTraChoiceFrom(TraChoiceFromModel tModel)
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
            tModel.ChoiceFromNumber = Auxiliary.CurCompanyAutoNum("TCF");

            // 新增(返回主键ID)
            int ChoiceFromId = bll.AddTraChoiceFrom(tModel);

            // 若主键>O(新增成功)
            if (ChoiceFromId > 0)
            {
                if (!string.IsNullOrEmpty(tModel.ComponentIdList))
                {
                    // 模版明细信息TraTraChoiceFromComponent(运输月度绩效自定义元件表) 
                    List<string> componentIdList = new List<string>(tModel.ComponentIdList.Split(','));
                    TMCFCbll.AddComponentList(componentIdList, ChoiceFromId);
                }

                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    // 新增模版附件类型TraTraChoiceFromAdjunct(运输月度考核表单自定义附件明细)
                    List<TempChoiceFromAdjunctModel> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempChoiceFromAdjunctModel>>(tModel.AdjunctList);

                    TMCFAbll.AddAdjunctTypeList(adjunctList, ChoiceFromId);
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
        /// <param name="choiceId">元件ID</param> 
        /// <returns>Json</returns>
        public ActionResult ComponentList(int index, int size, string choiceId)
        {
            // 查询本公司内有效的(非作废)绩效元件信息
            string where = " State != 20 AND CompanyId =" + Auxiliary.CompanyID();

            // 元件编号
            where += string.Format(" And ChoiceId IN ({0})", choiceId.Trim());

            // 运输月度考核元件List
            List<TraChoiceComponentModel> list = bll.ComponentList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="choiceId">元件ID</param> 
        /// <returns></returns>
        public int ComponentAmount(string choiceId)
        {
            // 查询本公司内有效的(非作废)绩效元件信息
            string where = " State != 20 AND CompanyId =" + Auxiliary.CompanyID();

            // 元件编号 
            where += string.Format(" And ChoiceId IN ({0})", choiceId.Trim());

            return bll.ComponentAmount(where);
        }

        #endregion

        #endregion

        #region 数据集

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="choiceFromNumber">自定义编号</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns>Json</returns>
        public ActionResult TraChoiceFromList(int index, int size, string choiceFromNumber, string state, string createTime)
        {
            // 查询本公司内有效的(非作废,非删除)绩效信息
            string where = " State != 30 AND State != 40 AND CompanyId =" + Auxiliary.CompanyID();

            // 自定义编号
            if (!string.IsNullOrEmpty(choiceFromNumber))
            {
                where += string.Format(" And ChoiceFromNumber like '%{0}%'", choiceFromNumber.Trim());
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
            List<TraChoiceFromModel> list = bll.TraChoiceFromList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="choiceFromNumber">自定义编号</param> 
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int TraChoiceFromAmount(string choiceFromNumber,  string state, string createTime)
        {
            // 查询本公司内有效的(非作废,非删除)绩效信息
            string where = " State != 30 AND State != 40 AND CompanyId =" + Auxiliary.CompanyID();

            // 自定义编号
            if (!string.IsNullOrEmpty(choiceFromNumber))
            {
                where += string.Format(" And ChoiceFromNumber like '%{0}%'", choiceFromNumber.Trim());
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

            return bll.TraChoiceFromAmount(where);
        }
        #endregion

        #region 编辑

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraChoiceFrom(TraChoiceFromModel tModel)
        {
            // 编辑之前Model
            TraChoiceFromModel beforeModel = bll.GetModelByID(tModel.ChoiceFromId);

            // 编辑
            int row = bll.EditTraChoiceFrom(tModel);

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
        #endregion

        #region 提交

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitState(int tId)
        {
            // 编辑之前Model
            TraChoiceFromModel beforeModel = bll.GetModelByID(tId);

            // 查询同公司同考核类型有效(提交状态)数据
            int result = bll.TraChoiceFromAmount(" State = 10 AND CompanyId = " + beforeModel.CompanyId);
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Exist, new { Detail = "提交", Id = tId, State = "初始" });
                return Json(new { flag = "exist", content = "已存在同公司同考核类型有效的数据！" });
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
        #endregion

        #region 作废

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            // 作废之前Model
            TraChoiceFromModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId, beforeModel.State);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 将明细中的数据变为无效(根据ChoiceFromId)
                TMCFCbll.InvalidByCheckFromId(tId);

                // 将模板附件类型中的数据变为无效(同ChoiceFromId)
                TMCFAbll.InvalidByChoiceFromId(tId);

                //// 将分配的机构供应商变为无效(同ChoiceFromId)
                //TMCFDbll.InvalidByCheckFromId(tId);

                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion

        #region 明细 运输月度绩效自定义元件表

        #region 批量新增 运输月度绩效自定义元件表

        /// <summary>
        /// 批量新增 运输月度绩效自定义元件表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddComponentList(string componentList, int choiceFromId)
        {
            // 反序列化
            List<string> ComponentList = new List<string>(componentList.Split(','));

            // 新增(返回主键ID)
            int ChoiceFromId = TMCFCbll.AddComponentList(ComponentList, choiceFromId);

            // 新增之后的Model
            TraChoiceFromComponentModel afterModel = TMCFCbll.GetModelByID(ChoiceFromId);

            // 若主键>O(新增成功)
            if (ChoiceFromId > 0)
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

        #region 明细List 运输月度绩效自定义元件表

        /// <summary>
        /// 明细List 运输月度绩效自定义元件表
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <returns>Json</returns>
        public ActionResult TraChoiceFromComponentList(int index, int size, int tId)
        {
            // where条件 (有效状态)
            string where = " TMCFC.State = 1 AND TMCFC.ChoiceFromId =" + tId;

            // 明细List 
            List<TraChoiceFromComponentModel> FromComponentList = TMCFCbll.TraChoiceFromComponentList(index, size, where);

            return Json(FromComponentList);
        }
        #endregion

        #region 明细count 运输月度绩效自定义元件表

        /// <summary>
        /// 明细count 运输月度绩效自定义元件表
        /// </summary> 
        /// <param name="tId">id</param> 
        /// <returns></returns>
        public int TraChoiceFromComponentAmount(int tId)
        {
            // where条件 (有效状态)
            string where = " TMCFC.State = 1 AND TMCFC.ChoiceFromId =" + tId;

            return TMCFCbll.TraChoiceFromComponentAmount(where);
        }
        #endregion

        #region 重置排序序号 运输月度绩效自定义元件表

        /// <summary>
        /// 重置排序序号 运输月度绩效自定义元件表
        /// </summary> 
        /// <returns></returns>
        public ActionResult ChangeSort(int tId, int tSort)
        {
            int row = TMCFCbll.ChangeSort(tId, tSort);
            if (row > 0)
            {
                return Json(new { flag = "success" });
            }
            return Json(new { flag = "fail", content = "重置排序序号失败！" });
        }
        #endregion

        #region 作废 运输月度绩效自定义元件表

        /// <summary>
        /// 作废 运输月度绩效自定义元件表
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidTraChoiceFromComponent(int tId)
        {
            // 作废之前Model
            TraChoiceFromComponentModel beforeModel = TMCFCbll.GetModelByID(tId);

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

        #endregion
        
        #region 模板附件类型 运输月度考核表单自定义附件明细

        #region 批量新增 运输月度考核表单自定义附件明细

        /// <summary>
        /// 批量新增 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddTraChoiceFromAdjunct(TraChoiceFromAdjunctModel tModel)
        {
            string where = " AND AdjunctName ='" + tModel.AdjunctName + "'";

            // 是否存在同附件名称的有效的数据(同ChoiceFromId)
            int row = TMCFAbll.ChoiceFromAdjunctAmount(tModel.ChoiceFromId, where);

            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, tModel);
                return Json(new { flag = "exist", content = "已存在同附件名称的有效的数据！" });
            }

            // 状态默认有效
            tModel.State = 1;

            // 新增(返回主键ID)
            int ChoiceFromAdjunctId = TMCFAbll.AddChoiceFromAdjunct(tModel);

            // 若主键>O(新增成功)
            if (ChoiceFromAdjunctId > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 模板附件类型List 运输月度考核表单自定义附件明细

        /// <summary>
        /// 模板附件类型List 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="tId">主键ID</param> 
        /// <returns>Json</returns>
        public ActionResult TraChoiceFromAdjunctList(int tId)
        {
            // where条件 (有效状态)
            string where = " State = 1 AND ChoiceFromId =" + tId;

            // 模板附件类型List
            List<TraChoiceFromAdjunctModel> FromAdjunctList = TMCFAbll.ChoiceFromAdjunctList(where);

            return Json(FromAdjunctList);
        }
        #endregion

        #region 模板附件类型count 运输月度考核表单自定义附件明细

        /// <summary>
        /// 模板附件类型count 运输月度考核表单自定义附件明细
        /// </summary> 
        /// <param name="tId">id</param> 
        /// <returns></returns>
        public int TraChoiceFromAdjunctAmount(int tId)
        {
            return TMCFAbll.ChoiceFromAdjunctAmount(tId, "");
        }
        #endregion

        #region 编辑 运输月度考核表单自定义附件明细

        /// <summary>
        /// 编辑 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraChoiceFromAdjunct(TraChoiceFromAdjunctModel tModel)
        {
            // 编辑之前Model
            TraChoiceFromAdjunctModel beforeModel = TMCFAbll.GetModelByID(tModel.ChoiceFromAdjunctId);

            // 编辑
            int row = TMCFAbll.EditChoiceFromAdjunct(tModel);

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
        #endregion

        #region 作废 运输月度考核表单自定义附件明细

        /// <summary>
        /// 作废 运输月度考核表单自定义附件明细
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidTraChoiceFromAdjunct(int tId)
        {
            // 作废之前Model
            TraChoiceFromAdjunctModel beforeModel = TMCFAbll.GetModelByID(tId);

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
