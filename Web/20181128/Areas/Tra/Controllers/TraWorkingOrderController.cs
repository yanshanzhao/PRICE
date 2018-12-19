//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-09    1.0        ZBB        新建               
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Aspose.Cells;

using SRM.Model.Tra;
using SRM.Web.Controllers;
using SRM.BLL.Tra;
using Newtonsoft.Json.Converters;
using SRM.BLL.Basis;
using System.Data;
using SRM.Model.Basis;
#endregion
/*********************************
 * 类名：TraWorkingOrderController
 * 功能描述：运作异常订单记录 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraWorkingOrderController : Controller
    {

        //运作异常订单记录
        TraWorkingOrderBLL bll = new TraWorkingOrderBLL();

        //系统字典BLL
        BasisDictionaryBLL BDBLL = new BasisDictionaryBLL();

        BasisIntercalateBLL BIBLL = new BasisIntercalateBLL();

        //
        // GET: /Tra/TraWorkingOrder/

        #region 方法

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
            TraWorkingOrderModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            TraWorkingOrderModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 运作供应商信息
        /// </summary> 
        public ActionResult TraWorkingOrderSupplier(string url)
        {
            ViewBag.url = url;
            return View();
        }

        #endregion

        #region 方法 

        #region 新增 运作异常订单记录

        /// <summary>
        /// 新增 运作异常订单记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns
        public ActionResult AddTraWorkingOrder(TraWorkingOrderModel model)
        {

            model.State = 5;// 状态：5-有效(提交操作);10-作废

            model.UseState = 0;// 状态：0-未使用;1-已使用

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID();//公司id

            int TraWorkingOrderId = 0;

            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");//当前时间

            int nowdate1 = Convert.ToInt32(nowdate.Split('-')[1]);//取当前时间的月份

            int nowdate2 = Convert.ToInt32(nowdate.Split('-')[2]);//取当前时间的天

            string date1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); //获取系统前一个月的时间

            int dt3 = Convert.ToInt32(date1.Split('-')[1]);//取上一个月的月份

            string date = model.WorkingTime.ToString("yyyy-MM-dd");//运作时间

            int dt1 = Convert.ToInt32(date.Split('-')[1]);//取当前时间的月份

            //如果当前时间和运作时间在同一个月内
            if (nowdate1 == dt1)
            {
                TraWorkingOrderId = bll.AddTraWorkingOrder(model);
            }
            else
            {
                //运作时间是否为当前时间的上一个月
                if (dt1 == dt3)
                {
                    //查询部门考核设置表中的数据
                    BasisIntercalateModel IntercalateModel = BIBLL.GetModelByDepartmentId(model.CreateDepartmentId);

                    if (IntercalateModel != null)
                    {
                        int days = IntercalateModel.Days;
                        //判断运作时间（日）是否小于最后日期
                        if (days > nowdate2)
                        {
                            TraWorkingOrderId = bll.AddTraWorkingOrder(model);
                        }
                        else
                        {
                            Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                            return Json(new { flag = "fail", content = "考核已结束无法进行保存操作！" });
                        }
                    }
                    else
                    {
                        Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                        return Json(new { flag = "fail", content = "未维护有效的部门考核日期限制！" });
                    }
                }
                else
                {
                    Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                    return Json(new { flag = "fail", content = "选择的日期不在考核期范围之内！" });
                }
            }
            if (TraWorkingOrderId > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 数据集 运作异常订单记录

        /// <summary>
        /// 数据集 运作异常订单记录
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="transportNumber">运输供应商编号</param> 
        /// <param name="workingTime">运作时间</param> 
        /// <param name="checkType">考核类型</param>
        /// <returns></returns>
        public ActionResult TraWorkingOrderList(int index, int size, string supplierName, string transportNumber, string workingTime, string checkType)
        {

            //查询本机构内所有有效的
            string where = string.Format(" TWO.State = 5 and TWO.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 运输供应商编号
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And ST.TransportNumber LIKE '%{0}%'", transportNumber.Trim());
            }

            // 运作时间
            if (!string.IsNullOrEmpty(workingTime))
            {
                where += string.Format(" And convert(varchar,TWO.WorkingTime,120) like '%{0}%'", workingTime.Trim());
            }

            // 考核类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And TWO.CheckType = {0}", checkType.Trim());
            }

            List<TraWorkingOrderModel> list = bll.TraWorkingOrderList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 运作异常订单记录

        /// <summary>
        /// 数据记录数 运作异常订单记录
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="transportNumber">运输供应商编号</param> 
        /// <param name="workingTime">运作时间</param> 
        /// <param name="checkType">考核类型</param>
        /// <returns></returns>
        public int TraWorkingOrderCount(string supplierName, string transportNumber, string workingTime, string checkType)
        {
            //查询本机构内所有有效的
            string where = string.Format(" TWO.State = 5 and TWO.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 运输供应商编号
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And ST.TransportNumber LIKE '%{0}%'", transportNumber.Trim());
            }

            // 运作时间
            if (!string.IsNullOrEmpty(workingTime))
            {
                where += string.Format(" And convert(varchar,TWO.WorkingTime,120) like '%{0}%'", workingTime.Trim());
            }

            // 考核类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And TWO.CheckType = {0}", checkType.Trim());
            }

            return bll.TraWorkingOrderCount(where);
        }
        #endregion

        #region 编辑 运作异常订单记录

        /// <summary>
        /// 编辑 运作异常订单记录
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraWorkingOrder(TraWorkingOrderModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            TraWorkingOrderModel beforeModel = bll.GetModelByID(model.OrderId);

            int TraWorkingOrderId = 0;

            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");//当前时间

            int nowdate1 = Convert.ToInt32(nowdate.Split('-')[1]);//取当前时间的月份

            int nowdate2 = Convert.ToInt32(nowdate.Split('-')[2]);//取当前时间的天

            string date1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); //获取系统前一个月的时间

            int dt3 = Convert.ToInt32(date1.Split('-')[1]);//取上一个月的月份

            string date = model.WorkingTime.ToString("yyyy-MM-dd");//运作时间

            int dt1 = Convert.ToInt32(date.Split('-')[1]);//取当前时间的月份

            //如果当前时间和运作时间在同一个月内
            if (nowdate1 == dt1)
            {
                TraWorkingOrderId = bll.EditTraWorkingOrder(model);
            }
            else
            {
                //运作时间是否为当前时间的上一个月
                if (dt1 == dt3)
                {
                    //查询部门考核设置表中的数据
                    BasisIntercalateModel IntercalateModel = BIBLL.GetModelByDepartmentId(model.CreateDepartmentId);

                    if (IntercalateModel != null)
                    {
                        int days = IntercalateModel.Days;
                        //判断运作时间（日）是否小于最后日期
                        if (days > nowdate2)
                        {
                            TraWorkingOrderId = bll.EditTraWorkingOrder(model);
                        }
                        else
                        {
                            Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                            return Json(new { flag = "fail", content = "考核已结束无法进行保存操作！" });
                        }
                    }
                    else
                    {
                        Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                        return Json(new { flag = "fail", content = "未维护有效的部门考核日期限制！" });
                    }
                }
                else
                {
                    Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                    return Json(new { flag = "fail", content = "选择的日期不在考核期范围之内！" });
                }
            }
            if (TraWorkingOrderId > 0)
            {
                return Json(new { flag = "success", content = "编辑成功！" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 作废按钮逻辑

        /// <summary>
        /// 作废按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            TraWorkingOrderModel beforeModel = bll.GetModelByID(tId);

            int delUserId = Auxiliary.UserID();//作废负责人

            int row = bll.InvalidState(tId, delUserId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion

        #region 导出按钮逻辑

        /// <summary>
        /// 导出按钮逻辑
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="transportNumber">运输供应商编号</param> 
        /// <param name="workingTime">运作时间</param> 
        /// <param name="checkType">考核类型</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string transportNumber, string workingTime, string checkType)
        {
            //查询本机构内所有有效的
            string where = string.Format(" TWO.State = 5 and TWO.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }

            // 运输供应商编号
            if (!string.IsNullOrEmpty(transportNumber))
            {
                where += string.Format(" And ST.TransportNumber LIKE '%{0}%'", transportNumber.Trim());
            }

            // 运作时间
            if (!string.IsNullOrEmpty(workingTime))
            {
                where += string.Format(" And convert(varchar,TWO.WorkingTime,120) like '%{0}%'", workingTime.Trim());
            }

            // 考核类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And TWO.CheckType = {0}", checkType.Trim());
            }


            System.Data.DataTable dt = bll.ExportDataTable(where);
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new
            {
                Type = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });

            return Json(new { flag = "success", guid = url });
        }

        #endregion

        #region 运输运作供应商列表

        /// <summary>
        /// 运输运作供应商列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public ActionResult TraWorkingOrderSupplierList(int index, int size, string supplierName)
        {
            string where = string.Format(" ST.TransportState='F4' and S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            //供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            List<TraWorkingOrderModel> list = bll.TraWorkingOrderSupplierList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public int TraWorkingOrderSupplierCount(string supplierName)
        {
            string where = string.Format(" ST.TransportState='F4' and S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName  like '%{0}%' ", supplierName.Trim());
            }

            return bll.TraWorkingOrderSupplierCount(where);
        }
        #endregion

        #region 获取系统字典表数据

        /// <summary>
        /// 获取系统字典表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkingOrderlist()
        {
            return Json(BDBLL.GetWorkingOrderlist(Auxiliary.CompanyID()));
        }
        #endregion

        #region  批量导入每日异常订单信息

        /// <summary>
        /// 批量导入每日异常订单信息
        /// </summary>
        /// <param name="path">excel文件</param> 
        [Operate(Name = OperateEnum.Import)]
        public ActionResult ImportInfo(string path)
        {
            List<TraWorkingOrderModel> list = new List<TraWorkingOrderModel>();

            int failcount = 0;

            // 失败内容
            string content = "第";

            List<Model.Basis.Dicts> Dict = BDBLL.GetWorkingOrderlist(Auxiliary.CompanyID());
            using (FileStream fs = new FileStream(Server.MapPath(path), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Workbook wb = new Workbook(fs);
                Worksheet ws = wb.Worksheets[0];

                Cells cells = ws.Cells;

                int rowCounts = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, false).Rows.Count;

                for (int i = 1; i < rowCounts; i++)
                {
                    //发货日期 物流单号    客户ID 异常客户姓名  到达站 异常类型    产品名称 数量 单位  处理措施 金额   责任环节 供应商编号 供应商名称 记录人   处理日期 原因分析
                    string num = ws.Cells[i, 0].StringValue.ToString();//序号

                    string workingtime = ws.Cells[i, 1].StringValue.ToString();//发货日期

                    string logisticsnumber = ws.Cells[i, 2].StringValue.ToString();//物流单号

                    string customerid = ws.Cells[i, 3].StringValue.ToString();//客户ID

                    string abnormalcustomer = ws.Cells[i, 4].StringValue.ToString();//异常客户姓名

                    string arrivalsite = ws.Cells[i, 5].StringValue.ToString();//到达站

                    string normname = ws.Cells[i, 6].StringValue.ToString();//异常类型

                    string productname = ws.Cells[i, 7].StringValue.ToString(); //产品名称

                    string number = ws.Cells[i, 8].StringValue.ToString();//数量

                    string unit = ws.Cells[i, 9].StringValue.ToString();//单位

                    string solve = ws.Cells[i, 10].StringValue.ToString();//处理措施

                    string cargodamage = ws.Cells[i, 11].StringValue.ToString();//金额

                    string checktype = ws.Cells[i, 12].StringValue.ToString();//责任环节

                    string transportnumber = ws.Cells[i, 13].StringValue.ToString();//供应商编号

                    string transportname = ws.Cells[i, 14].StringValue.ToString();//供应商名称

                    string recorduser = ws.Cells[i, 15].StringValue.ToString();//记录人

                    string solvetime = ws.Cells[i, 16].StringValue.ToString();//处理日期 

                    string analysis = ws.Cells[i, 17].StringValue.ToString();//原因分析 

                    //供应商名称、运输供应商编号 、责任环节（调拨、干线和终端）、运作时间 发货日期、物流单号

                    TraWorkingOrderModel model = new TraWorkingOrderModel();

                    if (string.IsNullOrEmpty(transportname) && string.IsNullOrEmpty(transportnumber) && string.IsNullOrEmpty(checktype) && string.IsNullOrEmpty(workingtime) && string.IsNullOrEmpty(logisticsnumber))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(transportname) || string.IsNullOrEmpty(transportnumber) || string.IsNullOrEmpty(checktype) || string.IsNullOrEmpty(workingtime) || string.IsNullOrEmpty(logisticsnumber))
                    {
                        content += i + " ";
                        failcount++;
                        continue;
                    }

                    #region 判断供应商是否存在

                    string where = string.Format(" ST.TransportState='F4' and S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

                    if (!string.IsNullOrEmpty(transportname))
                    {
                        where += string.Format(" And s.SupplierName  = '{0}'", transportname.Trim());
                    }

                    TraWorkingOrderModel OrderModel = bll.TraWorkingOrderSupplierLists(where);

                    if (OrderModel == null)
                    {
                        content += i + " ";
                        failcount++;
                        continue;//跳出此条数据
                    }

                    #endregion 

                    #region 判断供应商编号是否存在

                    string wheres = string.Format(" ST.TransportState='F4' and S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

                    if (!string.IsNullOrEmpty(transportnumber))
                    {
                        wheres += string.Format(" And ST.TransportNumber  = '{0}'", transportnumber.Trim());
                    }

                    TraWorkingOrderModel OrderModels = bll.TraWorkingOrderSupplierLists(wheres);

                    if (OrderModels == null)
                    {
                        content += i + " ";
                        failcount++;
                        continue;//跳出此条数据
                    }

                    #endregion

                    #region 验证时间

                    string nowdate = DateTime.Now.ToString("yyyy-MM-dd");//当前时间

                    int nowdate1 = Convert.ToInt32(nowdate.Split('-')[1]);//取当前时间的月份

                    int nowdate2 = Convert.ToInt32(nowdate.Split('-')[2]);//取当前时间的天

                    string date1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); //获取系统前一个月的时间

                    int dt3 = Convert.ToInt32(date1.Split('-')[1]);//取上一个月的月份

                    string date = workingtime.ToString();//运作时间

                    int dt1 = Convert.ToInt32(date.Split('-')[1]);//取当前时间的月份

                    //如果当前时间和运作时间在同一个月内
                    if (nowdate1 != dt1)
                    {
                        //运作时间是否为当前时间的上一个月
                        if (dt1 == dt3)
                        {
                            //查询部门考核设置表中的数据
                            BasisIntercalateModel IntercalateModel = BIBLL.GetModelByDepartmentId(Auxiliary.DepartmentId());

                            if (IntercalateModel != null)
                            {
                                int days = IntercalateModel.Days;
                                //判断运作时间（日）是否小于最后日期
                                if (days <= nowdate2)
                                {
                                    content += i + " ";
                                    failcount++;
                                    continue;//跳出此条数据
                                }
                            }
                            else
                            {
                                content += i + " ";
                                failcount++;
                                continue;//跳出此条数据
                            }
                        }
                        else
                        {
                            content += i + " ";
                            failcount++;
                            continue;//跳出此条数据
                        }
                    }

                    #endregion

                    model.TransportId = OrderModel.TransportId;

                    model.WorkingTime = Convert.ToDateTime(workingtime);

                    model.CheckTypeName = checktype;

                    if (checktype == "调拨")
                    {
                        model.CheckType = 0;
                    }
                    else if (checktype == "配送(干线)"|| checktype == "配送（干线）" || checktype == "配送(干线）" || checktype == "配送（干线)" || checktype == "配送干线")
                    {
                        model.CheckType = 1;
                    }
                    else if (checktype == "配送(终端)"|| checktype == "配送（终端）" || checktype == "配送(终端）" || checktype == "配送（终端)" || checktype == "配送终端")
                    {
                        model.CheckType = 2;
                    }
                    else
                    {
                        continue;
                    }

                    model.LogisticsNumber = logisticsnumber;

                    model.OrderTypeName = "0";

                    //// 通过不合格品类型查询 
                    //Model.Basis.Dicts modelDict = Dict.Find(delegate (Model.Basis.Dicts modelDictMX)
                    //{
                    //    return modelDictMX.Name.Equals(ordertypename);
                    //});
                    //if (modelDict == null)
                    //{
                    //    continue;
                    //}
                    //else
                    //{
                    //    model.OrderType = Convert.ToInt32(modelDict.Id);
                    //}
                    model.CargoDamage = Convert.ToInt32(cargodamage);//金额

                    model.State = 5;// 状态：5-有效(提交操作);10-作废

                    model.UseState = 0;// 状态：0-未使用;1-已使用

                    model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

                    model.CreateUserId = Auxiliary.UserID();// 用户ID

                    model.CreateTime = DateTime.Now;//创建时间

                    model.CompanyId = Auxiliary.CompanyID();//公司id

                    model.CustomerId = customerid;//客户ID

                    model.AbnormalCustomer = abnormalcustomer;//异常客户姓名

                    model.ArrivalSite = arrivalsite;//到达站

                    model.NormName = normname;//异常类型

                    model.ProductName = productname;//产品名称

                    model.Number = number;//数量

                    model.Unit = unit;//单位

                    model.Solve = solve;//处理措施

                    model.RecordUser = recorduser;//记录人

                    model.SolveTime = Convert.ToDateTime(solvetime); //处理日期

                    model.Analysis = analysis;//原因分析

                    list.Add(model);

                    //如果为空，添加验证
                    if (string.IsNullOrEmpty(logisticsnumber))
                    {
                        break;
                    }
                }
            }

            bool res = false;

            if (list.Count > 0)
                res = new TraWorkingOrderBLL().AddBulk(list);

            if (res)
            {
                content += "行";
                Auxiliary.SupplierCustomLog(OperateEnum.Import, ResultEnum.Sucess, new { Type = "导入", Success = list.Count, Fail = failcount });
                return Json(new { flag = "ok", failcount = failcount, successcount = list.Count, content });
            }
            else
            {
                Auxiliary.SupplierCustomLog(OperateEnum.Import, ResultEnum.Fail, new { Type = "导入", Fail = failcount });
                return Json(new { flag = "fail", failcount = failcount, content });
            }

        }
        #endregion

        #endregion
    }
}
