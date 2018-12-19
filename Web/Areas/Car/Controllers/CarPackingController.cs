
//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , PRICE
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-12-12    1.0        FJK        新建 - 包装器具维护     
//-------------------------------------------------------------------------
using BLL.Car;
using Model.Car;
using Newtonsoft.Json.Converters;
using SRM.Model.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Controllers;
/*********************************
* 类名：CarPackingController
* 功能描述：汽车物流-包装器具 控制器 
* ******************************/

namespace Web.Areas.Car.Controllers
{
    public class CarPackingController : Controller
    {
        //
        // GET: /Car/CarPacking/

        // 汽车物流-包装器具BLL
        CarPackingBLL bll = new CarPackingBLL();

        #region 页面

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取包装器具实体类
            CarPackingModel model = bll.GetModelByID(tId);

            // 获取供应商实体类
            // CarSupplierModel supplierModel = new CarSupplierBLL().GetModelByID(tId);
            ViewBag.SupplierNumber = "GYX001";// supplierModel.SupplierNumber;
            ViewBag.SupplierName = "供应商1";// supplierModel.SupplierName;

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            CarPackingModel model = bll.GetModelByID(tId);

            // 获取供应商实体类
            // CarSupplierModel supplierModel = new CarSupplierBLL().GetModelByID(tId);
            ViewBag.SupplierNumber = "GYX001";// supplierModel.SupplierNumber;
            ViewBag.SupplierName = "供应商1";// supplierModel.SupplierName;

            return View(model);
        }

        /// <summary>
        /// 供应商信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierDetail(string url)
        {
            ViewBag.Url = url;
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddCarPacking(CarPackingModel tModel)
        {
            // 状态 有效
            tModel.State = 1;

            // 机构ID
            tModel.CreateDepartmentId = Auxiliary.DepartmentId();

            // 用户ID
            tModel.CreateUserId = Auxiliary.UserID();

            // 公司ID
            tModel.CompanyId = Auxiliary.CompanyID();

            // 新增(返回主键ID)
            int packingId = bll.AddCarPacking(tModel);

            // 若主键>O(新增成功)
            if (packingId > 0)
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
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="packingName">器具名称</param>
        /// <param name="packingNumber">包装代码</param>
        /// <param name="supplierName">供应商名称</param> 
        /// <returns>Json</returns>
        public ActionResult CarPackingList(int index, int size, string packingName, string packingNumber, string supplierName)
        {
            StringBuilder carPacking = new StringBuilder();

            // 只能查询本机构内状态为有效的包装器具信息
            carPacking.Append("CP.State = 1 AND CP.CreateDepartmentId =" + Auxiliary.DepartmentId());

            // 器具名称
            if (!string.IsNullOrEmpty(packingName))
            {
                carPacking.Append(" AND CP.PackingName like '%" + packingName + "%'");
            }

            // 包装代码
            if (!string.IsNullOrEmpty(packingNumber))
            {
                carPacking.Append(" AND CP.PackingNumber like '%" + packingNumber + "%'");
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                carPacking.Append(" AND CS.SupplierName like '%" + supplierName+ "%'");
            }
             
            // 包装器具List
            List<CarPackingModel> list = bll.CarPackingList(index, size, carPacking.ToString());
             
            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="packingName">器具名称</param>
        /// <param name="packingNumber">包装代码</param>
        /// <param name="supplierName">供应商名称</param> 
        /// <returns>Json</returns>
        public int CarPackingCount(string packingName, string packingNumber, string supplierName)
        {
            StringBuilder carPacking = new StringBuilder();

            // 只能查询本机构内状态为有效的包装器具信息
            carPacking.Append("CP.State = 1 AND CP.CreateDepartmentId =" + Auxiliary.DepartmentId());

            // 器具名称
            if (!string.IsNullOrEmpty(packingName))
            {
                carPacking.Append(" AND CP.PackingName like '%" + packingName + "%'");
            }

            // 包装代码
            if (!string.IsNullOrEmpty(packingNumber))
            {
                carPacking.Append(" AND CP.PackingNumber like '%" + packingNumber + "%'");
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                carPacking.Append(" AND CS.SupplierName like '%" + supplierName + "%'");
            }

            return bll.CarPackingCount(carPacking.ToString());
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditCarPacking(CarPackingModel tModel)
        {
            // 编辑之前Model
            CarPackingModel beforeModel = bll.GetModelByID(tModel.PackingId);

            // 编辑
            int row = bll.EditCarPacking(tModel);

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
        /// 作废
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            // 作废之前Model
            CarPackingModel beforeModel = bll.GetModelByID(tId);

            // 作废人ID
            int delUserId = Auxiliary.UserID();

            // 作废(更改状态)
            int row = bll.InvalidState(tId, delUserId);

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

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="packingName">器具名称</param>
        /// <param name="packingNumber">包装代码</param>
        /// <param name="supplierName">供应商名称</param> 
        /// <returns>Json</returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string packingName, string packingNumber, string supplierName)
        { 
            StringBuilder carPacking = new StringBuilder();

            // 只能查询本机构内状态为有效的包装器具信息
            carPacking.Append("CP.State = 1 AND CP.CreateDepartmentId =" + Auxiliary.DepartmentId());

            // 器具名称
            if (!string.IsNullOrEmpty(packingName))
            {
                carPacking.Append(" AND CP.PackingName like '%" + packingName + "%'");
            }

            // 包装代码
            if (!string.IsNullOrEmpty(packingNumber))
            {
                carPacking.Append(" AND CP.PackingNumber like '%" + packingNumber + "%'");
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                carPacking.Append(" AND CS.SupplierName like '%" + supplierName + "%'");
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(carPacking.ToString());

            // Excel
            Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new
            {
                Detail = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });
            return Json(new { flag = "success", guid = url });
        }

        #region 获取供应商信息

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param> 
        /// <param name="size">页面条数</param>
        /// <param name="storageNumber">供应商名称</param> 
        /// <returns>Json</returns>
        public ActionResult CarSupplierList(int index, int size, string supplierName)
        {
            StringBuilder carPacking = new StringBuilder();

            // 查询本机构内状态为有效的数据
            carPacking.Append("CS.State = 1 AND CS.CreateDepartmentId =" + Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                carPacking.Append("and CS.SupplierName like '%" + supplierName + "%'");
            }
             
            // 供应商List
            List<CarSupplierModel> list = bll.CarSupplierList(index, size, carPacking.ToString());

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="storageNumber">供应商名称</param> 
        /// <returns>Json</returns>
        public int CarSupplierCount(string supplierName)
        {
            StringBuilder carPacking = new StringBuilder();

            // 查询本机构内状态为有效的数据
            carPacking.Append("CS.State = 1 AND CS.CreateDepartmentId =" + Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                carPacking.Append("and CS.SupplierName like '%" + supplierName + "%'");
            }

            return bll.CarSupplierCount(carPacking.ToString());
        }

        #endregion
        #endregion
    }
}
