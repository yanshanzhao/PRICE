//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , PRICE
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-12-13    1.0         YSZ       新建 
//-------------------------------------------------------------------------
using BLL.Car;
using Model.Car;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Controllers;
/*********************************
 * 类名：CarSupplierController
 * 功能描述：汽车物流-供应商 控制器
 * ******************************/
namespace Web.Areas.Car.Controllers
{
    public class CarSupplierController : Controller
    {
        //
        CarSupplierBLL bll = new CarSupplierBLL();
        #region 界面
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增供应商
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// 编辑供应商
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            CarSupplierModel model = bll.GetModelByID(tId);
            return View(model);
        }
        public ActionResult View(int tId)
        {
            // 获取数据
            CarSupplierModel model = bll.GetModelByID(tId);
            return View(model);
        }
        #endregion 界面

        #region 方法
        #region 分页供应商列表
        /// <summary>
        /// 分页供应商列表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="SupplierName">供应商名称</param>
        /// <param name="SupplierNumber">供应商代码</param>
        /// <returns></returns>
        public ActionResult SupplierList(int index, int size, string SupplierName, string SupplierNumber)
        {
            // 查询本机构内所有的线路信息。
            StringBuilder where = new StringBuilder();
            where.Append(" CreateDepartmentId =" + Auxiliary.DepartmentId()+ " AND State=1");
            if (!string.IsNullOrEmpty(SupplierName))
            {
                where.Append(" AND SupplierName like '%" + SupplierName + "%'");
            }
            if (!string.IsNullOrEmpty(SupplierNumber))
            {
                where.Append(" AND SupplierNumber like '%" + SupplierNumber + "%'");
            }

            // 供应商List
            List<CarSupplierModel> list = bll.SupplierList(index, size, where.ToString());

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion 分页供应商列表

        #region 分页供应商总数
        /// <summary>
        /// 分页供应商总数
        /// </summary>
        /// <param name="SupplierName">供应商名称</param>
        /// <param name="SupplierNumber">供应商代码</param>
        /// <returns></returns>
        public int SupplieCount(string SupplierName, string SupplierNumber)
        {
            StringBuilder where = new StringBuilder();
            where.Append(" CreateDepartmentId =" + Auxiliary.DepartmentId()+ " AND State=1");
            if (!string.IsNullOrEmpty(SupplierName))
            {
                where.Append(" AND SupplierName ='" + SupplierName + "' ");
            }
            if (!string.IsNullOrEmpty(SupplierNumber))
            {
                where.Append(" AND SupplierNumber ='" + SupplierNumber + "' ");
            }
            return bll.SupplierCount(where.ToString());
        }
        #endregion 分页供应商总数

        #region 新增供应商
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tModel">供应商实体对象</param>
        /// <returns></returns>
        public ActionResult AddSupplier(CarSupplierModel tModel)
        {
            // 状态 1-有效(默认)
            tModel.State = 1;

            // 默认创建机构ID为当前登录人所属机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 默认创建账号ID为当前登录人ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 默认公司ID为当前登录人所属公司
            tModel.CompanyId = Auxiliary.CompanyID();

            // 判断本机构是否有相同的供应商
            int row = bll.SupplierCount(" CreateDepartmentId =" + Auxiliary.DepartmentId() + " AND (SupplierName = '" + tModel.SupplierName + "' OR SupplierNumber ='" + tModel.SupplierNumber + "')");

            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, tModel);
                return Json(new { flag = "exist" });
            }
            else
            {
                // 新增
                if (bll.AddSupplier(tModel) > 0)
                {
                    Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                    return Json(new { flag = "success" });
                }
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 编辑供应商
        /// <summary>
        /// 编辑供应商
        /// </summary>
        /// <param name="tMdoel">供应商实体对象</param>
        /// <returns></returns>
        public ActionResult EditSupplier(CarSupplierModel tMdoel)
        {
            // 编辑之前Model
            CarSupplierModel beforeModel = bll.GetModelByID(tMdoel.SupplierId);

            // 编辑
            int result = bll.EditSupplier(tMdoel);

            // 若影响行数>O(修改成功)
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tMdoel);
                return Json(new { flag = "success" });
            }
            else if (result == -1)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Exist, beforeModel, tMdoel);
                return Json(new { flag = "exist" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tMdoel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 作废供应商
        /// <summary>
        /// 作废供应商
        /// </summary>
        /// <param name="ids">供应商ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            // 作废之前Model
            CarSupplierModel beforeModel = bll.GetModelByID(tId);

            //得到当年登陆用户的id
            int userId = Auxiliary.UserID();
            // 作废(更改状态)
            int row = bll.InvalidState(tId, userId);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 导出供应商
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="SupplierName">供应商名称</param>
        /// <param name="SupplierNumber">供应商代码</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string SupplierName, string SupplierNumber)
        {
            // 查询本公司内所有的供应商信息。
            StringBuilder where = new StringBuilder();
            where.Append(" CreateDepartmentId =" + Auxiliary.DepartmentId()+ " AND State=1");

            // 供应商名称
            if (!string.IsNullOrEmpty(SupplierName))
            {
                where.Append(string.Format(" And SupplierName like '%{0}%'", SupplierName.Trim()));
            }

            // 供应商代码
            if (!string.IsNullOrEmpty(SupplierNumber))
            {
                where.Append(string.Format(" AND SupplierNumber like '%{0}%'", SupplierNumber));
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where.ToString());

            // Excel
            Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new { Detail = "导出", UserId = Auxiliary.UserID(), ExportTime = System.DateTime.Now });
            return Json(new { flag = "success", guid = url });
        }
        #endregion
        #endregion
    }
}
