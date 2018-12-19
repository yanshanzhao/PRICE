//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-12    1.0        ZBB        新建               
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
using SRM.Model.Basis;
using SRM.Web.Controllers;
using SRM.BLL.Tra;
using SRM.BLL.Basis;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：TraWorkingDealyController
 * 功能描述：每日延迟订单记录 控制器 
 * ******************************/


namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraWorkingDealyController : Controller
    {
        //
        // GET: /Tra/TraWorkingDealy/

        //每日延迟订单记录
        TraWorkingDealyBLL bll = new TraWorkingDealyBLL();

        //部门考核设置表
        BasisIntercalateBLL BIbll = new BasisIntercalateBLL();

        #region 方法

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
            TraWorkingDealyModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            TraWorkingDealyModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 运作供应商信息
        /// </summary> 
        [Operate(Name = OperateEnum.Allot)]
        public ActionResult TraWorkingSupplier(string url)
        {
            ViewBag.url = url;
            return View();
        }
        #endregion

        #region 方法 

        #region 新增 每日延迟订单记录

        /// <summary>
        /// 新增 每日延迟订单记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns
        public ActionResult AddTraWorkingDealy(TraWorkingDealyModel model)
        {

            model.State = 5;// 状态：5-有效(提交操作);10-作废

            model.UseState = 0;// 状态：0-未使用;1-已使用

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID();//公司id

            int DelayId = 0;

            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");//当前时间

            int nowdate1 = Convert.ToInt32(nowdate.Split('-')[1]);//取当前时间的月份

            int nowdate2 = Convert.ToInt32(nowdate.Split('-')[2]);//取当前时间的天

            string date1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); //获取系统前一个月的时间

            int dt3 = Convert.ToInt32(date1.Split('-')[1]);//取上一个月的月份

            string date = model.WorkingTime.ToString("yyyy-MM-dd");//运作时间

            int dt1 = Convert.ToInt32(date.Split('-')[1]);//取运作时间的月份

            int dt2 = Convert.ToInt32(date.Split('-')[2]);//取运作时间的天

            //如果当前时间和运作时间在同一个月内
            if (nowdate1 == dt1)
            {
                DelayId = bll.AddTraWorkingDealy(model);
            }
            else
            {
                //运作时间是否为当前时间的上一个月
                if (dt1 == dt3)
                {
                    //查询部门考核设置表中的数据
                    BasisIntercalateModel IntercalateModel = BIbll.GetModelByDepartmentId(model.CreateDepartmentId);

                    if (IntercalateModel != null)
                    {
                        int days = IntercalateModel.Days;
                        //判断运作时间（日）是否小于最后日期
                        if (days > nowdate2)
                        {
                            DelayId = bll.AddTraWorkingDealy(model);
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

            if (DelayId > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 数据集 每日延迟订单记录

        /// <summary>
        /// 数据集 每日延迟订单记录
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="transportNumber">运输供应商编号</param>
        /// <param name="plateNumber">车牌号</param>
        /// <param name="workingTime">运作时间</param>
        /// <param name="workingType">运作类型</param>
        /// <param name="checkType">考核类型</param>
        /// <returns></returns>
        public ActionResult TraWorkingDealyList(int index, int size, string supplierName, string transportNumber, string checkType, string workingType, string workingTime)
        {

            //查询本机构内所有有效的
            string where = string.Format(" TWD.State = 5 and TWD.CreateDepartmentId={0}", Auxiliary.DepartmentId());

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

            // 考核类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And TWD.CheckType = {0}", checkType.Trim());
            }

            // 运作类型
            if (!string.IsNullOrEmpty(workingType))
            {
                where += string.Format(" And TWD.WorkingType = {0}", workingType.Trim());
            }

            // 运作时间
            if (!string.IsNullOrEmpty(workingTime))
            {
                where += string.Format(" And convert(varchar,TWD.WorkingTime,120) like '%{0}%'", workingTime.Trim());
            }

            List<TraWorkingDealyModel> list = bll.TraWorkingDealyList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 每日延迟订单记录

        /// <summary>
        /// 数据记录数 每日延迟订单记录
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="transportNumber">运输供应商编号</param>
        /// <param name="plateNumber">车牌号</param>
        /// <param name="workingTime">运作时间</param>
        /// <param name="workingType">运作类型</param>
        /// <param name="checkType">考核类型</param>
        /// <returns></returns>
        public int TraWorkingCount(string supplierName, string transportNumber, string checkType, string workingType, string workingTime)
        {
            //查询本机构内所有有效的
            string where = string.Format(" TWD.State = 5 and TWD.CreateDepartmentId={0}", Auxiliary.DepartmentId());

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

            // 考核类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And TWD.CheckType = {0}", checkType.Trim());
            }

            // 运作类型
            if (!string.IsNullOrEmpty(workingType))
            {
                where += string.Format(" And TWD.WorkingType = {0}", workingType.Trim());
            }

            // 运作时间
            if (!string.IsNullOrEmpty(workingTime))
            {
                where += string.Format(" And convert(varchar,TWD.WorkingTime,120) like '%{0}%'", workingTime.Trim());
            }

            return bll.TraWorkingDealyCount(where);
        }
        #endregion

        #region 编辑 每日延迟订单记录

        /// <summary>
        /// 编辑 每日延迟订单记录
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraWorkingDealy(TraWorkingDealyModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            TraWorkingDealyModel beforeModel = bll.GetModelByID(model.DelayId);

            int DelayId = 0;

            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");//当前时间

            int nowdate1 = Convert.ToInt32(nowdate.Split('-')[1]);//取当前时间的月份

            int nowdate2 = Convert.ToInt32(nowdate.Split('-')[2]);//取当前时间的天

            string date1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); //获取系统前一个月的时间

            int dt3 = Convert.ToInt32(date1.Split('-')[1]);//取上一个月的月份

            string date = model.WorkingTime.ToString("yyyy-MM-dd");//运作时间

            int dt1 = Convert.ToInt32(date.Split('-')[1]);//取运作时间的月份

            int dt2 = Convert.ToInt32(date.Split('-')[2]);//取运作时间的天

            //如果当前时间和运作时间在同一个月内
            if (nowdate1 == dt1)
            {
                DelayId = bll.EditTraWorkingDealy(model);
            }
            else
            {
                //运作时间是否为当前时间的上一个月
                if (dt1 == dt3)
                {
                    //查询部门考核设置表中的数据
                    BasisIntercalateModel IntercalateModel = BIbll.GetModelByDepartmentId(model.CreateDepartmentId);

                    if (IntercalateModel != null)
                    {
                        int days = IntercalateModel.Days;
                        //判断运作时间（日）是否小于最后日期
                        if (days > nowdate2)
                        {
                            DelayId = bll.EditTraWorkingDealy(model);
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

            if (DelayId > 0)
            {
                return Json(new { flag = "success" });
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
            TraWorkingDealyModel beforeModel = bll.GetModelByID(tId);

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
        /// <param name="plateNumber">车牌号</param>
        /// <param name="workingTime">运作时间</param>
        /// <param name="workingType">运作类型</param>
        /// <param name="checkType">考核类型</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string transportNumber, string checkType, string workingType, string workingTime)
        {
            //查询本机构内所有有效的
            string where = string.Format(" TWD.State = 5 and TWD.CreateDepartmentId={0}", Auxiliary.DepartmentId());

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

            // 考核类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And TWD.CheckType = {0}", checkType.Trim());
            }

            // 运作类型
            if (!string.IsNullOrEmpty(workingType))
            {
                where += string.Format(" And TWD.WorkingType = {0}", workingType.Trim());
            }

            // 运作时间
            if (!string.IsNullOrEmpty(workingTime))
            {
                where += string.Format(" And convert(varchar,TWD.WorkingTime,120) like '%{0}%'", workingTime.Trim());
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
        public ActionResult TraWorkingDealySupplierList(int index, int size, string supplierName)
        {
            string where = string.Format("  ST.TransportState='F4' AND S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            //供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            List<TraWorkingDealyModel> list = bll.TraWorkingDealySupplierList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="companyName">公司名称</param>
        /// <returns></returns>
        public int TraWorkingDealySupplierCount(string supplierName)
        {
            string where = string.Format("  ST.TransportState='F4' AND S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName  like '%{0}%' ", supplierName.Trim());
            }

            return bll.TraWorkingDealySupplierCount(where);
        }
        #endregion

        #endregion

        #region 方法  明细

        #region  批量导入每月运营过程记录

        /// <summary>
        /// 批量导入每月运营过程记录
        /// </summary>
        /// <param name="path">excel文件</param> 
        [Operate(Name = OperateEnum.Import)]
        public ActionResult ImportInfo(string path)
        {
            List<TraWorkingDealyModel> list = new List<TraWorkingDealyModel>();

            int failcount = 0;

            // 失败内容
            string content = "第";

            using (FileStream fs = new FileStream(Server.MapPath(path), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Workbook wb = new Workbook(fs);
                Worksheet ws = wb.Worksheets[0];

                Cells cells = ws.Cells;

                int rowCounts = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, false).Rows.Count;

                for (int i = 1; i < rowCounts; i++)
                {
                    //发货日期 物流单号   客户ID 异常客户姓名  到达站 异常类型  预计到货时间 实际到货时间  迟到类型 迟到原因 是否考核 责任环节 供应商名称 供应商编号
                    string num = ws.Cells[i, 0].StringValue.ToString();//序号

                    string workingtime = ws.Cells[i, 1].StringValue.ToString();//发货日期

                    string logisticsnumber = ws.Cells[i, 2].StringValue.ToString();//物流单号

                    string customerid = ws.Cells[i, 3].StringValue.ToString();//客户ID

                    string abnormalcustomer = ws.Cells[i, 4].StringValue.ToString(); //异常客户姓名

                    string arrivalsite = ws.Cells[i, 5].StringValue.ToString(); //到达站

                    string normname = ws.Cells[i, 6].StringValue.ToString(); //异常类型

                    string plantime = ws.Cells[i, 7].StringValue.ToString(); //预计到货时间

                    string actualtime = ws.Cells[i, 8].StringValue.ToString(); //实际到货时间

                    string workingtype = ws.Cells[i, 9].StringValue.ToString(); //迟到类型

                    string delayremark = ws.Cells[i, 10].StringValue.ToString(); //迟到原因

                    string isassess = ws.Cells[i, 11].StringValue.ToString(); //是否考核

                    string checktypename = ws.Cells[i, 12].StringValue.ToString(); //责任环节

                    string suppliername = ws.Cells[i, 13].StringValue.ToString(); //供应商名称

                    string transportnumber = ws.Cells[i, 14].StringValue.ToString(); //供应商编号

                    TraWorkingDealyModel model = new TraWorkingDealyModel();

                    if (string.IsNullOrEmpty(suppliername) && string.IsNullOrEmpty(transportnumber) && string.IsNullOrEmpty(checktypename) && string.IsNullOrEmpty(workingtype) && string.IsNullOrEmpty(workingtime))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(suppliername) || string.IsNullOrEmpty(transportnumber) || string.IsNullOrEmpty(checktypename) || string.IsNullOrEmpty(workingtype) || string.IsNullOrEmpty(workingtime))
                    {
                        content += i + " ";
                        failcount++;
                        continue;
                    }

                    #region 验证该供应商名称是否存在

                    string where = string.Format(" ST.TransportState='F4' and S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

                    if (!string.IsNullOrEmpty(suppliername))
                    {
                        where += string.Format(" And s.SupplierName  = '{0}'", suppliername.Trim());
                    }

                    TraWorkingDealyModel WorkingModel = bll.TraWorkingDealySupplierLists(where);

                    if (WorkingModel == null)
                    {
                        content += i + " ";
                        failcount++;
                        continue;//跳出此条数据
                    }

                    #endregion

                    #region 验证该供应商编号是否存在

                    string wheres = string.Format(" ST.TransportState='F4' and S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

                    if (!string.IsNullOrEmpty(transportnumber))
                    {
                        where += string.Format(" And ST.TransportNumber  = '{0}'", transportnumber.Trim());
                    }

                    TraWorkingDealyModel WorkingModels = bll.TraWorkingDealySupplierLists(where);

                    if (WorkingModels == null)
                    {
                        content += i + " ";
                        failcount++;
                        continue;//跳出此条数据
                    }

                    #endregion

                    #region 时间的验证 
                    string nowdate = DateTime.Now.ToString("yyyy-MM-dd");//当前时间

                    int nowdate1 = Convert.ToInt32(nowdate.Split('-')[1]);//取当前时间的月份

                    int nowdate2 = Convert.ToInt32(nowdate.Split('-')[2]);//取当前时间的天

                    string date1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); //获取系统前一个月的时间

                    int dt3 = Convert.ToInt32(date1.Split('-')[1]);//取上一个月的月份

                    string date = workingtime.ToString();//运作时间 

                    int dt1 = Convert.ToInt32(date.Split('-')[1]);//取运作时间的月份

                    int dt2 = Convert.ToInt32(date.Split('-')[2]);//取运作时间的天

                    //如果当前时间和运作时间不在同一个月内
                    if (nowdate1 != dt1)
                    {
                        //运作时间是否为当前时间的上一个月
                        if (dt1 == dt3)
                        {
                            //查询部门考核设置表中的数据 
                            BasisIntercalateModel IntercalateModel = BIbll.GetModelByDepartmentId(Auxiliary.DepartmentId());

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

                    if (isassess == "是")
                    { 
                        model.TransportId = WorkingModel.TransportId;

                        model.CheckTypeName = checktypename;

                        if (checktypename == "调拨")
                        {
                            model.CheckType = 0;
                        }
                        else if (checktypename == "配送(干线)" || checktypename == "配送（干线）" || checktypename == "配送(干线）" || checktypename == "配送（干线)" || checktypename == "配送干线")
                        {
                            model.CheckType = 1;
                        }
                        else if (checktypename == "配送(终端)" || checktypename == "配送（终端）" || checktypename == "配送(终端）" || checktypename == "配送（终端)" || checktypename == "配送终端")
                        {
                            model.CheckType = 2;
                        }
                        else
                        {
                            continue;
                        }

                        // 运作类型:0-到货;1-到车;2-发货;3-其他
                        model.WorkingTypeName = workingtype;

                        if (workingtype == "到货")
                        {
                            model.WorkingType = 0;
                        }
                        else if (workingtype == "到车")
                        {
                            model.WorkingType = 1;
                        }
                        else if (workingtype == "发货")
                        {
                            model.WorkingType = 2;
                        }
                        else if (workingtype == "其他")
                        {
                            model.WorkingType = 3;
                        }
                        else
                        {
                            continue;
                        }

                        model.WorkingTime = Convert.ToDateTime(workingtime);

                        model.LogisticsNumber = logisticsnumber;//车牌号

                        model.CustomerId = customerid;//客户ID

                        model.AbnormalCustomer = abnormalcustomer;//异常客户姓名

                        model.ArrivalSite = arrivalsite;//到达站

                        model.NormName = normname;//异常类型

                        model.PlanTime = plantime;//预计到货时间

                        model.ActualTime = actualtime;//实际到货时间

                        model.DelayRemark = delayremark;//迟到原因

                        model.State = 5;// 状态：5-有效(提交操作);10-作废

                        model.UseState = 0;// 状态：0-未使用;1-已使用

                        model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

                        model.CreateUserId = Auxiliary.UserID();// 用户ID

                        model.CreateTime = DateTime.Now;//创建时间

                        model.CompanyId = Auxiliary.CompanyID();//公司id

                        list.Add(model);

                        //如果为空，添加验证
                        if (string.IsNullOrEmpty(suppliername))
                        {
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            bool res = false;

            if (list.Count > 0)

                res = new TraWorkingDealyBLL().AddBulk(list);

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
