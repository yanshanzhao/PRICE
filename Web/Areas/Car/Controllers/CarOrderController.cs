//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-12-13    1.0        ZBB        新建               
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Model.Car;
using Web.Controllers;
using BLL.Car;
using System.Text;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：CarOrderController
 * 功能描述：订单录入控制器 
 * ******************************/

namespace Web.Areas.Car.Controllers
{
    public class CarOrderController : Controller
    {
        //
        // GET: /Car/CarOrder/

        //信息预登记BLL
        CarOrderBLL bll = new CarOrderBLL();

        //
        // GET: /Car/CarOrder/

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
            CarOrderModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 查看
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int tId)
        {
            // 获取数据
            CarOrderModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 供应商信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCarOrderDetail(string url)
        {
            ViewBag.Url = url;
            return View();
        }

        /// <summary>
        /// 新增订单明细
        /// </summary>
        /// <returns></returns>
        public ActionResult CarPartDetail(string url)
        {
            ViewBag.Url = url;
            return View();
        }

        /// <summary>
        /// 选择包装代码
        /// </summary>
        /// <returns></returns>
        public ActionResult CarPackingDetail(string url)
        {
            ViewBag.Url = url;
            return View();
        }

        #endregion

        #region 方法

        #region 数据集 订单录入 

        /// <summary>
        /// 数据集 订单录入
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">显示条数</param>
        /// <param name="partno">交货单号</param>
        /// <param name="starttime">取货时间</param>
        /// <param name="endtime">取货时间</param>
        /// <param name="carnumber">车牌号</param>
        /// <param name="offertypes">生成报价状态</param>
        /// <param name="suppliername">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult CarOrderList(int index, int size, string partno, string starttime, string endtime, string carnumber, string offertypes, string suppliername, string state)
        {

            StringBuilder where = new StringBuilder();

            // 只能查询本机构收入计算状态为已计算和待计算的订单信息。
            where.Append(" (CO.State=0 OR CO.State=1) AND CO.CreateDepartmentId =" + Auxiliary.DepartmentId());

            //交货单号
            if (!string.IsNullOrEmpty(partno))
            {
                where.Append(" And CO.PartNo like '%" + partno.Trim() + "%' ");
            }

            // 取货时间 
            if (!string.IsNullOrEmpty(starttime))
            {
                if (!string.IsNullOrEmpty(endtime))
                {
                    where.Append(" And CO.TakeTime BETWEEN '" + starttime.Trim() + "' AND '" + endtime.Trim() + "' ");
                }
            }

            //车牌号
            if (!string.IsNullOrEmpty(carnumber))
            {
                where.Append(" And CO.CarNumber like '%" + carnumber.Trim() + "%' ");
            }

            //生成报价状态
            if (!string.IsNullOrEmpty(offertypes))
            {
                where.Append(" CH.OfferTypes ='" + offertypes.Trim() + "' ");
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(suppliername))
            {
                where.Append(" And CS.SupplierName like '%" + suppliername.Trim() + "%' ");
            }

            //状态
            if (!string.IsNullOrEmpty(state))
            {
                where.Append(" CO.State ='" + state.Trim() + "' ");
            }

            List<CarOrderModel> list = bll.CarOrderList(index, size, where.ToString());

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 订单录入

        /// <summary>
        /// 数据记录数 订单录入
        /// </summary>
        /// <param name="partno">交货单号</param>
        /// <param name="starttime">取货时间</param>
        /// <param name="endtime">取货时间</param>
        /// <param name="carnumber">车牌号</param>
        /// <param name="offertypes">生成报价状态</param>
        /// <param name="suppliername">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public int CarOrderCount(string partno, string starttime, string endtime, string carnumber, string offertypes, string suppliername, string state)
        {

            StringBuilder where = new StringBuilder();

            // 只能查询本机构收入计算状态为已计算和待计算的订单信息。
            where.Append(" (CO.State=0 OR CO.State=1) AND CO.CreateDepartmentId =" + Auxiliary.DepartmentId());

            //交货单号
            if (!string.IsNullOrEmpty(partno))
            {
                where.Append(" And CO.PartNo like '%" + partno.Trim() + "%' ");
            }

            // 取货时间 
            if (!string.IsNullOrEmpty(starttime))
            {
                // 到货时间 
                if (!string.IsNullOrEmpty(endtime))
                {
                    where.Append(" And CO.TakeTime BETWEEN '" + starttime.Trim() + "' AND '" + endtime.Trim() + "' ");
                }
            }

            //车牌号
            if (!string.IsNullOrEmpty(carnumber))
            {
                where.Append(" And CO.CarNumber like '%" + carnumber.Trim() + "%' ");
            }

            //生成报价状态
            if (!string.IsNullOrEmpty(offertypes))
            {
                where.Append(" CH.OfferTypes ='" + offertypes.Trim() + "' ");
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(suppliername))
            {
                where.Append(" And CS.SupplierName like '%" + suppliername.Trim() + "%' ");
            }

            //状态
            if (!string.IsNullOrEmpty(state))
            {
                where.Append(" CO.State ='" + state.Trim() + "' ");
            }

            return bll.CarOrderCount(where.ToString());
        }
        #endregion

        #region 订单录入新增

        /// <summary>
        /// 订单录入新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Add)]
        public ActionResult AddCarOrder(CarOrderModel model)
        {

            model.State = 0;// 默认状态为初始状态0待计算

            model.CompanyId = Auxiliary.CompanyID();//公司id

            model.CreateDepartmentId = Auxiliary.DepartmentId();//新增机构id

            model.CreateUserId = Auxiliary.UserID();//新增负责人id

            model.CreateTime = DateTime.Now;//新增时间

            int OrderId = bll.AddCarOrder(model);

            if (OrderId > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 订单录入编辑

        /// <summary>
        /// 订单录入编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns> 
        public ActionResult EditCarOrder(CarOrderModel model)
        {
            CarOrderModel beforeModel = bll.GetModelByID(model.OrderId);

            int result = bll.UpdateCarOrder(model);

            if (result > 0)
            {

                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success", content = "编辑成功！" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion 

        #region 订单录入作废

        /// <summary>
        /// 订单录入作废
        /// </summary>
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult DelCarOrder(int Id)
        {
            CarOrderModel beforeModel = bll.GetModelByID(Id);

            int row = bll.ChangeState(Id, 4);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 导出数据

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="partno">交货单号</param>
        /// <param name="starttime">取货时间</param>
        /// <param name="endtime">取货时间</param>
        /// <param name="carnumber">车牌号</param>
        /// <param name="offertypes">生成报价状态</param>
        /// <param name="suppliername">供应商名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string partno, string starttime, string endtime, string carnumber, string offertypes, string suppliername, string state)
        {

            StringBuilder where = new StringBuilder();

            // 只能查询本机构收入计算状态为已计算和待计算的订单信息。
            where.Append(" (CO.State=0 OR CO.State=1) AND CO.CreateDepartmentId =" + Auxiliary.DepartmentId());

            //交货单号
            if (!string.IsNullOrEmpty(partno))
            {
                where.Append(" And CO.PartNo like '%" + partno.Trim() + "%' ");
            }

            // 取货时间 
            if (!string.IsNullOrEmpty(starttime))
            {
                //到货时间
                if (!string.IsNullOrEmpty(endtime))
                {
                    where.Append(" And CO.TakeTime BETWEEN '" + starttime.Trim() + "' AND '" + endtime.Trim() + "' ");
                }
            }

            //车牌号
            if (!string.IsNullOrEmpty(carnumber))
            {
                where.Append(" And CO.CarNumber like '%" + carnumber.Trim() + "%' ");
            }

            //生成报价状态
            if (!string.IsNullOrEmpty(offertypes))
            {
                where.Append(" CH.OfferTypes ='" + offertypes.Trim() + "' ");
            }

            // 供应商名称
            if (!string.IsNullOrEmpty(suppliername))
            {
                where.Append(" And CS.SupplierName like '%" + suppliername.Trim() + "%' ");
            }

            //状态
            if (!string.IsNullOrEmpty(state))
            {
                where.Append(" CO.State ='" + state.Trim() + "' ");
            }

            System.Data.DataTable dt = bll.ExportDataTable(where.ToString());
            Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        #endregion
    }
}
