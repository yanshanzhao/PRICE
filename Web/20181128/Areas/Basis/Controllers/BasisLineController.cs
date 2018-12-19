//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-16    1.0        FJK        新建       
//-------------------------------------------------------------------------
#region 参考
using System.Collections.Generic;
using System.Web.Mvc;

using SRM.Model.Basis;
using SRM.Web.Controllers;
using SRM.BLL.Basis;
using Newtonsoft.Json.Converters;
using SRM.BLL.Sys;
using SRM.Model.Sys;
#endregion
/*********************************
 * 类名：BasisLineController
 * 功能描述：线路维护表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Basis.Controllers
{
    public class BasisLineController : Controller
    {
        //
        // GET: /Basis/BasisLine/

        // 线路维护BLL
        BasisLineBLL bll = new BasisLineBLL();

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
            BasisLineModel model = bll.GetModelByID(tId);

            // 起始位置/结束位置名称
            SysAreasModel BeginModel = bll.GetModelByAreaId(model.BeginId);
            SysAreasModel EndModel = bll.GetModelByAreaId(model.EndId);
            ViewBag.BeginName = BeginModel.AreaName;
            ViewBag.EndName = EndModel.AreaName;

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            BasisLineModel model = bll.GetModelByID(tId);

            // 起始位置/结束位置名称
            SysAreasModel BeginModel = bll.GetModelByAreaId(model.BeginId);
            SysAreasModel EndModel = bll.GetModelByAreaId(model.EndId);
            ViewBag.BeginName = BeginModel.AreaName;
            ViewBag.EndName = EndModel.AreaName;

            return View(model);
        }

        /// <summary>
        /// Areas
        /// </summary>
        public ActionResult Areas(string url)
        {
            ViewBag.Url = url;
            return View();
        }

        /// <summary>
        /// LineName
        /// </summary>
        public ActionResult LineName(string url)
        {
            ViewBag.Url = url;
            return View();
        }
        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="LineName">线路名称</param>
        /// <param name="BeginId">起始位置</param>
        /// <param name="EndId">结束位置</param>
        /// <returns></returns>
        public ActionResult LineList(int index, int size, string lineName, string beginId, string endId)
        {
            // 查询本机构内所有的线路信息。
            string where = " CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 机构名称
            if (!string.IsNullOrEmpty(lineName))
            {
                where += string.Format(" And LineName like '%{0}%'", lineName.Trim());
            }

            // 起始位置
            if (!string.IsNullOrEmpty(beginId))
            {
                where += string.Format(" And BeginId = '{0}'", beginId.Trim());
            }

            // 结束位置
            if (!string.IsNullOrEmpty(endId))
            {
                where += string.Format(" And EndId = '{0}'", endId.Trim());
            }

            // 线路维护List
            List<BasisLineModel> list = bll.LineList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="LineName">线路名称</param>
        /// <param name="BeginId">起始位置</param>
        /// <param name="EndId">结束位置</param>
        /// <returns></returns>
        public int LineCount(string lineName, string beginId, string endId)
        {
            // 查询本机构内所有的线路信息。
            string where = " CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 机构名称
            if (!string.IsNullOrEmpty(lineName))
            {
                where += string.Format(" And LineName like '%{0}%'", lineName.Trim());
            }

            // 起始位置
            if (!string.IsNullOrEmpty(beginId))
            {
                where += string.Format(" And BeginId = '{0}'", beginId.Trim());
            }

            // 结束位置
            if (!string.IsNullOrEmpty(endId))
            {
                where += string.Format(" And EndId = '{0}'", endId.Trim());
            }

            return bll.LineCount(where);
        }

        /// <summary>
        /// 线路维护新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddLine(BasisLineModel tModel)
        {
            // 状态 1-有效(默认)
            tModel.State = 1;

            // 使用状态 0-未使用（默认） 
            tModel.UseState = 0;

            // 默认创建机构ID为当前登录人所属机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 默认创建账号ID为当前登录人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 默认公司ID为当前登录人所属公司
            tModel.CompanyId = Auxiliary.CompanyID();

            // 判断本机构是否有相同的线路
            int row = bll.LineCount(" BeginId = "+ tModel.BeginId+ " AND EndId =" + tModel.EndId + " AND CreateDepartmentId =" + Auxiliary.DepartmentId());

            if (row >0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, tModel);
                return Json(new { flag = "exist" });
            }
            else
            {
                // 新增
                if (bll.AddLine(tModel) > 0)
                {
                    Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                    return Json(new { flag = "success" });
                } 
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 线路维护编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditLine(BasisLineModel tModel)
        {
            // 编辑之前Model
            BasisLineModel beforeModel = bll.GetModelByID(tModel.LineId);

            // 编辑
            int result = bll.EditLine(tModel);

            // 若影响行数>O(修改成功)
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel);
                return Json(new { flag = "success" });
            }
            else if (result == -1)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Exist, beforeModel, tModel);
                return Json(new { flag = "exist" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult ChangeState(int tId, int tState)
        {
            int row = 0;

            if (tState == 1)
            {
                row = bll.EnableState(tId);
            }
            else if (tState == 0)
            {
                int delUserId = Auxiliary.UserID();
                row = bll.DiscontinuationState(tId, delUserId);
            }

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="LineName">线路名称</param>
        /// <param name="BeginId">起始位置</param>
        /// <param name="EndId">结束位置</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string lineName, string beginId, string endId)
        {
            // 查询本公司内所有的线路信息。
            string where = " CompanyId =" + Auxiliary.CompanyID();

            // 机构名称
            if (!string.IsNullOrEmpty(lineName))
            {
                where += string.Format(" And LineName like '%{0}%'", lineName.Trim());
            }

            // 起始位置
            if (!string.IsNullOrEmpty(beginId))
            {
                where += string.Format(" And BeginId = '{0}'", beginId.Trim());
            }

            // 结束位置
            if (!string.IsNullOrEmpty(endId))
            {
                where += string.Format(" And EndId = '{0}'", endId.Trim());
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }

        #region 区域基础表

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="LineName">位置名称</param> 
        /// <returns></returns>
        public ActionResult AreasList(int index, int size, string areaName)
        {
            // 有效的信息。
            string where = " State = 1";

            // 机构名称
            if (!string.IsNullOrEmpty(areaName))
            {
                where += string.Format(" And AreaName like '%{0}%'", areaName.Trim());
            }

            // 区域基础List
            List<SysAreasModel> list = bll.AreasList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="LineName">线路名称</param>
        /// <param name="BeginId">起始位置</param>
        /// <param name="EndId">结束位置</param>
        /// <returns></returns>
        public int AreasCount(string areaName)
        {
            // 有效的信息。
            string where = " State = 1";

            // 机构名称
            if (!string.IsNullOrEmpty(areaName))
            {
                where += string.Format(" And AreaName like '%{0}%'", areaName.Trim());
            }

            return bll.AreasCount(where);
        }

        #endregion

        #region 线路维护表_运输供应商线路

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="LineName">线路名称</param>
        /// <param name="BeginId">起始位置</param>
        /// <param name="EndId">结束位置</param>
        /// <returns></returns>
        public ActionResult GetLineList(int index, int size, string lineName)
        {
            // 查询本公司内所有的线路信息。
            string where = " BL.State = 1 AND BL.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 机构名称
            if (!string.IsNullOrEmpty(lineName))
            {
                where += string.Format(" AND LineName like '%{0}%'", lineName.Trim());
            } 

            // 线路维护List
            List<BasisLineModel> list = bll.LineList(index, size, where);
             
            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="LineName">线路名称</param>
        /// <returns></returns>
        public int GetLineCount(string lineName)
        {
            // 查询本公司内所有的线路信息。
            string where = " BL.State = 1 AND BL.CreateDepartmentId =" + Auxiliary.DepartmentId();

            // 机构名称
            if (!string.IsNullOrEmpty(lineName))
            {
                where += string.Format(" And LineName like '%{0}%'", lineName.Trim());
            }

            return bll.LineCount(where);
        } 
        #endregion
        #endregion
    }
}
