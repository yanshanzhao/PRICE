//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-08    1.0        FJK        新建       
//-------------------------------------------------------------------------
#region 参考
using System.Collections.Generic;
using System.Web.Mvc;

using SRM.Model.Tra;
using SRM.Web.Controllers;
using SRM.BLL.Tra;
using Newtonsoft.Json.Converters;
using SRM.BLL.Sys;
using SRM.Model.Sys;
#endregion
/*********************************
 * 类名：TraOperateDetailController
 * 功能描述：运营类型维护值域表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraOperateDetailController : Controller
    {
        //
        // GET: /Tra/TraOperateDetail/

        // 运营类型维护值域BLL
        TraOperateDetailBLL bll = new TraOperateDetailBLL();

        // 运营类型维护BLL
        TraOperateBLL TObll = new TraOperateBLL();

        #region 页面

        /// <summary>
        /// Index
        /// </summary> 
        [Operate(Name = OperateEnum.Detail)]
        public ActionResult Index(int tOperateId, int tIsInterval, int tIsNumber)
        {
            ViewBag.OperateId = tOperateId;
            ViewBag.IsInterval = tIsInterval;
            ViewBag.IsNumber = tIsNumber;
            return View();
        }

        /// <summary>
        /// Add
        /// </summary> 
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add(int tOperateId, int tIsInterval, int tIsNumber)
        {
            ViewBag.OperateId = tOperateId;
            ViewBag.IsInterval = tIsInterval;
            ViewBag.IsNumber = tIsNumber;

            return View();
        }

        /// <summary>
        /// Edit
        /// </summary> 
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId, int tOperateId, int tIsInterval, int tIsNumber)
        {
            ViewBag.OperateId = tOperateId;
            ViewBag.IsInterval = tIsInterval;
            ViewBag.IsNumber = tIsNumber;

            // 获取数据
            TraOperateDetailModel model = bll.GetModelByID(tId);

            return View(model);
        }
        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="tOperateId">主表主键ID</param> 
        /// <returns></returns>
        public ActionResult OperateDetailList(int index, int size, int tOperateId)
        {
            // 根据主表主键ID查询有效状态的数据
            string where = " DetailState = 1 AND OperateId =" + tOperateId;

            // 运营类型维护值域List
            List<TraOperateDetailModel> list = bll.OperateDetailList(index, size, where);
            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="tOperateId">主表主键ID</param> 
        /// <returns></returns>
        public int OperateDetailCount(int tOperateId)
        {
            // 根据主表主键ID查询有效状态的数据
            string where = " DetailState = 1 AND OperateId =" + tOperateId;

            // 运营类型维护值域Count
            return bll.OperateDetailCount(where);
        }

        /// <summary>
        /// 运营类型维护值域新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddOperateDetail(TraOperateDetailModel tModel)
        {
            // 默认状态为有效 1
            tModel.DetailState = 1;

            // 新增
            if (bll.AddOperateDetail(tModel) > 0)
            {
                // 根据主表主键ID查询有效状态的数据
                string where = " DetailState = 1 AND OperateId =" + tModel.OperateId; 
                int DetailCount = bll.OperateDetailCount(where);
                if (DetailCount >0&& DetailCount < 2)
                {
                    // 修改主表是否有明细 有
                    TObll.UpdateDetailState(tModel.OperateId, 1);
                }

                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 运营类型维护值域编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditOperateDetail(TraOperateDetailModel tModel)
        {
            // 编辑之前Model
            TraOperateDetailModel beforeModel = bll.GetModelByID(tModel.OperateId);

            // 编辑
            int result = bll.EditOperateDetail(tModel);

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
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            // 作废之前Model
            TraOperateDetailModel beforeModel = bll.GetModelByID(tId);

            // 作废(更改状态)
            int row = bll.ChangeState(tId, 0);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 根据主表主键ID查询有效状态的数据
                string where = " DetailState = 1 AND OperateId =" + beforeModel.OperateId;
                int DetailCount = bll.OperateDetailCount(where);
                if (DetailCount <= 0)
                {
                    // 修改主表是否有明细 无
                    TObll.UpdateDetailState(beforeModel.OperateId, 0);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败" });
        }

        #endregion 
    }
}
