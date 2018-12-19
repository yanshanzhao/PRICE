//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-04    1.0        FJK        新建                    
//-------------------------------------------------------------------------
#region 参考
using System.Collections.Generic;

using System.Web.Mvc;
using SRM.Model.Supplier;
using SRM.BLL.Supplier;
using SRM.Web.Controllers;
using Newtonsoft.Json.Converters;
using System.Data;
using SRM.Model.Basis;
#endregion
/*********************************
 * 类名：SupplierServiceLevelController
 * 功能描述：运输供应商服务等级表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Supplier.Controllers
{
    public class SupplierServiceLevelController : Controller
    {
        //
        // GET: /Supplier/SupplierServiceLevel/

        // 服务级别维护BLL
        SupplierServiceLevelBLL bll = new SupplierServiceLevelBLL();

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
        public ActionResult Edit(string tId)
        {
            // 获取数据
            SupplierServiceLevelModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 查看
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(string tId)
        {
            // 获取数据
            SupplierServiceLevelModel model = bll.GetModelByID(tId);

            return View(model);
        } 
        #endregion

        #region 方法

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddServiceLevel(SupplierServiceLevelModel tModel)
        {
            // 默认状态为有效状态1
            tModel.SuppServiceState = 1;

            // 默认公司ID为当前登录人所属公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 若主键>O(新增成功)
            if (bll.AddServiceLevel(tModel) > 0)
            {
                // 记录供应商日志
                Auxiliary.SupplierLog<SupplierServiceLevelModel>(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 记录供应商日志
            Auxiliary.SupplierLog<SupplierServiceLevelModel>(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="suppKindId">服务类型</param>
        /// <param name="suppTurnoverName">规模级别名称</param>
        /// <param name="suppServiceName">服务级别名称</param>
        /// <param name="suppServiceState">状态</param>
        /// <returns></returns>
        public ActionResult ServiceLevelList(int index, int size, string suppKindId, string suppTurnoverName, string suppServiceName, string suppServiceState)
        {
            // 查询本公司内所有的运输供应商服务等级信息
            string where = " ssl.CompanyId =" + Auxiliary.CompanyID();

            // 服务类型
            if (!string.IsNullOrEmpty(suppKindId))
            {
                where += string.Format(" And stl.SuppKindId = {0}", suppKindId.Trim());
            }

            // 规模级别名称
            if (!string.IsNullOrEmpty(suppTurnoverName))
            {
                where += string.Format(" And stl.SuppTurnoverName like '%{0}%'", suppTurnoverName.Trim());
            }

            // 服务级别名称
            if (!string.IsNullOrEmpty(suppServiceName))
            {
                where += string.Format(" And ssl.SuppServiceName like '%{0}%'", suppServiceName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(suppServiceState))
            {
                where += string.Format(" And ssl.SuppServiceState = {0}", suppServiceState.Trim());
            }

            // 供应商服务等级维护List
            List<SupplierServiceLevelModel> list = bll.SupplierServiceLevelList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="suppKindId">服务类型</param>
        /// <param name="suppTurnoverName">规模级别名称</param>
        /// <param name="suppServiceName">服务级别名称</param>
        /// <param name="suppServiceState">状态</param>
        /// <returns></returns>
        public int ServiceLevelCount(string suppKindId, string suppTurnoverName, string suppServiceName, string suppServiceState)
        {
            // 查询本公司内所有的运输供应商服务等级信息
            string where = " ssl.CompanyId =" + Auxiliary.CompanyID();

            // 服务类型
            if (!string.IsNullOrEmpty(suppKindId))
            {
                where += string.Format(" And stl.SuppKindId = {0}", suppKindId.Trim());
            }

            // 规模级别名称
            if (!string.IsNullOrEmpty(suppTurnoverName))
            {
                where += string.Format(" And stl.SuppTurnoverName like '%{0}%'", suppTurnoverName.Trim());
            }

            // 服务级别名称
            if (!string.IsNullOrEmpty(suppServiceName))
            {
                where += string.Format(" And ssl.SuppServiceName like '%{0}%'", suppServiceName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(suppServiceState))
            {
                where += string.Format(" And ssl.SuppServiceState = {0}", suppServiceState.Trim());
            }

            return bll.BasisServiceLevelAmount(where);
        }

        /// <summary>
        /// 最大年绩效上限
        /// </summary> 
        /// <returns></returns>
        public decimal GetMaxPerformance()
        {
            // 查询本公司内所有的运输供应商服务等级信息
            string where = " SuppServiceState =1 AND CompanyId =" + Auxiliary.CompanyID();
             
            return bll.GetMaxPerformance(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditServiceLevel(SupplierServiceLevelModel tModel)
        {
            // 编辑之前Model
            SupplierServiceLevelModel beforeModel = bll.GetModelByID(tModel.SuppServiceId.ToString());

            // 编辑
            int result = bll.EditServiceLevel(tModel);

            // 若影响行数>O(修改成功)
            if (result > 0)
            {
                // 供应商日志
                Auxiliary.SupplierLog<SupplierServiceLevelModel>(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel); 
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierLog<SupplierServiceLevelModel>(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel); 
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        //[Operate(Name = OperateEnum.Invalid)]
        public ActionResult ChangeState(string tId)
        {
            // 作废之前Model
            SupplierServiceLevelModel beforeModel = bll.GetModelByID(tId);

            // 作废(更改状态)
            int row = bll.ChangeState(tId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "作废", Id = tId }); 
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "作废", Id = tId }); 
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="suppKindId">服务类型</param>
        /// <param name="suppTurnoverName">规模级别名称</param>
        /// <param name="suppServiceName">服务级别名称</param>
        /// <param name="suppServiceState">状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string suppKindId, string suppTurnoverName, string suppServiceName, string suppServiceState)
        {
            // 查询本公司内所有的运输供应商服务等级信息
            string where = " ssl.CompanyId =" + Auxiliary.CompanyID();

            // 服务类型
            if (!string.IsNullOrEmpty(suppKindId))
            {
                where += string.Format(" And stl.SuppKindId = {0}", suppKindId.Trim());
            }

            // 规模级别名称
            if (!string.IsNullOrEmpty(suppTurnoverName))
            {
                where += string.Format(" And stl.SuppTurnoverName like '%{0}%'", suppTurnoverName.Trim());
            }

            // 服务级别名称
            if (!string.IsNullOrEmpty(suppServiceName))
            {
                where += string.Format(" And ssl.SuppServiceName like '%{0}%'", suppServiceName.Trim());
            }

            // 状态
            if (!string.IsNullOrEmpty(suppServiceState))
            {
                where += string.Format(" And ssl.SuppServiceState = {0}", suppServiceState.Trim());
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where);

            // Excel
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Export, ResultEnum.Sucess, new
            { Detail = "导出",UserId = Auxiliary.UserID(),ExportTime = System.DateTime.Now});
            return Json(new { flag = "success", guid = url });
        }

        /// <summary>
        /// 获取服务类型
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns> 
        public ActionResult GetDictionaryName()
        {
            // 服务类型
            List<BasisDictionaryModel> listDictionaryName = bll.GetDictionaryName(); 
            return Json(listDictionaryName);
        } 
        #endregion

    }
}
