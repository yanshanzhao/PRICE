//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-11    1.0        ZBB        新建               
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
using SRM.BLL.Basis;
using Newtonsoft.Json.Converters;
using SRM.Model.Basis;
#endregion
/*********************************
 * 类名：TraWorkingOperateController
 * 功能描述：运作运营过程记录 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraWorkingOperateController : Controller
    {
        //
        // GET: /Tra/TraWorkingOperate/

        //运作运营过程记录
        TraWorkingOperateBLL bll = new TraWorkingOperateBLL();

        BasisIntercalateBLL BIBLL = new BasisIntercalateBLL();

        TraOperateBLL TOBLL = new TraOperateBLL();

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
            TraWorkingOperateModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            TraWorkingOperateModel model = bll.GetModelByID(tId);

            return View(model);
        }

        /// <summary>
        /// 运作供应商信息
        /// </summary> 
        public ActionResult TraWorkingOperateSupplier(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// 运营类型信息
        /// </summary> 
        public ActionResult TraWorkingOperateType(string url, string type)
        {
            ViewBag.url = url;
            ViewBag.CheckType = type;
            return View();
        }

        /// <summary>
        /// 运作供应商信息
        /// </summary> 
        public ActionResult TraOperateValue(string url, string id)
        {
            ViewBag.url = url;
            ViewBag.OperateId = id;
            return View();
        }
        #endregion

        #region 方法 

        #region 新增 运作运营过程记录

        /// <summary>
        /// 新增 运作运营过程记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns
        public ActionResult AddTraWorkingOperate(TraWorkingOperateModel model)
        {

            model.State = 5;// 状态：5-有效(提交操作);10-作废

            model.UseState = 0;// 状态：0-未使用;1-已使用

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID();//公司id

            int TraWorkingOperateId = 0;

            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");//当前时间

            int nowdate1 = Convert.ToInt32(nowdate.Split('-')[1]);//取当前时间的月份

            int nowdate2 = Convert.ToInt32(nowdate.Split('-')[2]);//取当前时间的天

            string date1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); //获取系统前一个月的时间

            int dt3 = Convert.ToInt32(date1.Split('-')[1]);//取上一个月的月份

            int failcount = 0;
            int workingmonth = model.WorkingTimeMonth;//考核月
            //运作时间是否为当前时间的上一个月
            if (workingmonth == dt3)
            {
                //查询部门考核设置表中的数据
                BasisIntercalateModel IntercalateModel = BIBLL.GetModelByDepartmentId(model.CreateDepartmentId);

                if (IntercalateModel != null)
                {
                    int days = IntercalateModel.Days;
                    //判断运作时间（日）是否小于最后日期
                    if (days > nowdate2)
                    {
                        #region 验证运营值问题

                        int operateid = model.OperateId;

                        List<TraOperateModel> traoperatemodel = TOBLL.GetModelByOperateId(operateid);

                        for (int i = 0; i < traoperatemodel.Count; i++)
                        {

                            //区间上值
                            int intervalbegin = Convert.ToInt32(traoperatemodel[i].IntervalBegin);

                            //运营值
                            int intervalvalue = Convert.ToInt32(model.OperateValue);

                            //区间下值
                            int intervalend = Convert.ToInt32(traoperatemodel[i].IntervalEnd);

                            if (intervalbegin < intervalvalue && intervalvalue < intervalend)
                            {
                                TraWorkingOperateId = bll.AddTraWorkingOperate(model);
                                break;
                            }
                            else
                            {
                                if (traoperatemodel[i].IntervalTpye == 1)
                                {
                                    if (intervalbegin == intervalvalue)
                                    {
                                        TraWorkingOperateId = bll.AddTraWorkingOperate(model);
                                        break;
                                    }
                                    else
                                    {
                                        failcount++;
                                    }
                                }
                                else if (traoperatemodel[i].IntervalTpye == 2)
                                {
                                    if (intervalvalue == intervalend)
                                    {
                                        TraWorkingOperateId = bll.AddTraWorkingOperate(model);
                                        break;
                                    }
                                    else
                                    {
                                        failcount++;
                                    }
                                }
                                else if (traoperatemodel[i].IntervalTpye == 3)
                                {
                                    if (intervalbegin == intervalvalue || intervalvalue == intervalend)
                                    {
                                        TraWorkingOperateId = bll.AddTraWorkingOperate(model);
                                        break;
                                    }
                                    else
                                    {
                                        failcount++;
                                    }
                                }
                            }
                        }
                        if (traoperatemodel.Count == failcount)
                        {
                            return Json(new { flag = "fail", content = "运营值必须在区间范围之内！" });
                        }
                        #endregion
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
                return Json(new { flag = "fail", content = "只能维护上一个月的数据！" });
            }

            if (TraWorkingOperateId > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);

            return Json(new { flag = "fail" });
        }
        #endregion

        #region 数据集 运作运营过程记录

        /// <summary>
        /// 数据集 运作运营过程记录
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="transportNumber">运输供应商编号</param> 
        /// <param name="checkType">考核类型</param>
        /// <param name="years">考核年</param>
        /// <param name="months">考核月</param>
        /// <param name="operateName">运营类型</param>
        /// <returns></returns>
        public ActionResult TraWorkingOperateList(int index, int size, string supplierName, string transportNumber, string checkType, string years, string months, string operateName)
        {

            //查询本机构内所有有效的
            string where = string.Format(" TWOT.State = 5 and TWOT.CreateDepartmentId={0}", Auxiliary.DepartmentId());

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
                where += string.Format(" And TWOT.CheckType = {0}", checkType.Trim());
            }

            // 考核年
            if (!string.IsNullOrEmpty(years))
            {
                where += string.Format(" And TWOT.WorkingTimeYear = {0}", years.Trim());
            }

            // 考核月
            if (!string.IsNullOrEmpty(months))
            {
                where += string.Format(" And TWOT.WorkingTimeMonth = {0}", months.Trim());
            }

            // 运营类型
            if (!string.IsNullOrEmpty(operateName))
            {
                where += string.Format(" And TOT.OperateName  LIKE '%{0}%'", operateName.Trim());
            }

            List<TraWorkingOperateModel> list = bll.TraWorkingOperateList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 运作运营过程记录

        /// <summary>
        /// 数据记录数 运作运营过程记录
        /// </summary>
        /// <param name="supplierName">供应商名称</param>
        /// <param name="transportNumber">运输供应商编号</param> 
        /// <param name="checkType">考核类型</param>
        /// <param name="years">考核年</param>
        /// <param name="months">考核月</param>
        /// <param name="operateName">运营类型</param>
        /// <returns></returns>
        public int TraWorkingOperateCount(string supplierName, string transportNumber, string checkType, string years, string months, string operateName)
        {
            //查询本机构内所有有效的
            string where = string.Format(" TWOT.State = 5 and TWOT.CreateDepartmentId={0}", Auxiliary.DepartmentId());

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
                where += string.Format(" And TWOT.CheckType = {0}", checkType.Trim());
            }

            // 考核年
            if (!string.IsNullOrEmpty(years))
            {
                where += string.Format(" And TWOT.WorkingTimeYear = {0}", years.Trim());
            }

            // 考核月
            if (!string.IsNullOrEmpty(months))
            {
                where += string.Format(" And TWOT.WorkingTimeMonth = {0}", months.Trim());
            }

            // 运营类型
            if (!string.IsNullOrEmpty(operateName))
            {
                where += string.Format(" And TOT.OperateName  LIKE '%{0}%'", operateName.Trim());
            }

            return bll.TraWorkingOperateCount(where);
        }
        #endregion

        #region 编辑 运作运营过程记录

        /// <summary>
        /// 编辑 运作运营过程记录
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditTraWorkingOperate(TraWorkingOperateModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            TraWorkingOperateModel beforeModel = bll.GetModelByID(model.WorkingOperateId);

            int TraWorkingOperateId = 0;

            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");//当前时间

            int nowdate1 = Convert.ToInt32(nowdate.Split('-')[1]);//取当前时间的月份

            int nowdate2 = Convert.ToInt32(nowdate.Split('-')[2]);//取当前时间的天

            string date1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); //获取系统前一个月的时间

            int dt3 = Convert.ToInt32(date1.Split('-')[1]);//取上一个月的月份

            int workingmonth = model.WorkingTimeMonth;//考核月

            //运作时间是否为当前时间的上一个月
            if (workingmonth == dt3)
            {
                //查询部门考核设置表中的数据
                BasisIntercalateModel IntercalateModel = BIBLL.GetModelByDepartmentId(model.CreateDepartmentId);

                if (IntercalateModel != null)
                {
                    int days = IntercalateModel.Days;
                    //判断运作时间（日）是否小于最后日期
                    if (days > nowdate2)
                    {
                        TraWorkingOperateId = bll.EditTraWorkingOperate(model);
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
                return Json(new { flag = "fail", content = "只能维护上一个月的数据！" });
            }

            if (TraWorkingOperateId > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "修改成功！" });
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
            TraWorkingOperateModel beforeModel = bll.GetModelByID(tId);

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
        /// <param name="checkType">考核类型</param>
        /// <param name="years">考核年</param>
        /// <param name="months">考核月</param>
        /// <param name="operateName">运营类型</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string supplierName, string transportNumber, string checkType, string years, string months, string operateName)
        {
            //查询本机构内所有有效的
            string where = string.Format(" TWOT.State = 5 and TWOT.CreateDepartmentId={0}", Auxiliary.DepartmentId());

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
                where += string.Format(" And TWOT.CheckType = {0}", checkType.Trim());
            }

            // 考核年
            if (!string.IsNullOrEmpty(years))
            {
                where += string.Format(" And TWOT.WorkingTimeYear = {0}", years.Trim());
            }

            // 考核月
            if (!string.IsNullOrEmpty(months))
            {
                where += string.Format(" And TWOT.WorkingTimeMonth = {0}", months.Trim());
            }

            // 运营类型
            if (!string.IsNullOrEmpty(operateName))
            {
                where += string.Format(" And TOT.OperateName  LIKE '%{0}%'", operateName.Trim());
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
        public ActionResult TraWorkingOperateSupplierList(int index, int size, string supplierName)
        {
            string where = string.Format(" ST.TransportState='F4' and S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            //供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName like '%{0}%'", supplierName.Trim());
            }

            List<TraWorkingOperateModel> list = bll.TraWorkingOperateSupplierList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="supplierName">供应商名称</param>
        /// <returns></returns>
        public int TraWorkingOperateSupplierCount(string supplierName)
        {
            string where = string.Format(" ST.TransportState='F4' and S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And S.SupplierName  like '%{0}%' ", supplierName.Trim());
            }

            return bll.TraWorkingOperateSupplierCount(where);
        }

        #endregion

        #region 运输运营类型列表

        /// <summary>
        /// 运输运营类型列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="operateName">供应商名称</param>
        /// <param name="checkType">考核类型</param>
        /// <returns></returns>
        public ActionResult TraWorkingOperateTypeList(int index, int size, string operateName, string checkType)
        {
            string where = string.Format(" TOT.State!=10  AND TOT.CompanyId={0}", Auxiliary.CompanyID());

            //运营类型名称
            if (!string.IsNullOrEmpty(operateName))
            {
                where += string.Format(" And TOT.OperateName  like '%{0}%' ", operateName.Trim());
            }

            //运营类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And TOT.CheckType  like '%{0}%' ", checkType.Trim());
            }

            List<TraWorkingOperateModel> list = bll.TraWorkingOperateTypeList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 数据记录数 运输运营类型列表
        /// </summary> 
        /// <param name="operateName">供应商名称</param>
        /// <param name="checkType">考核类型</param>
        /// <returns></returns>
        public int TraWorkingOperateTypeCount(string operateName, string checkType)
        {
            string where = string.Format(" TOT.State!=10  AND TOT.CompanyId={0}", Auxiliary.CompanyID());

            //运营类型名称
            if (!string.IsNullOrEmpty(operateName))
            {
                where += string.Format(" And TOT.OperateName  like '%{0}%' ", operateName.Trim());
            }

            //运营类型
            if (!string.IsNullOrEmpty(checkType))
            {
                where += string.Format(" And TOT.CheckType  like '%{0}%' ", checkType.Trim());
            }

            return bll.TraWorkingOperateTypeCount(where);
        }
        #endregion 

        #region 运营值列表

        /// <summary>
        /// 运营值列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="intervalvalue">运营值</param>
        /// <returns></returns>
        public ActionResult TraIntervalValueList(int index, int size, string intervalvalue, string operateId)
        {
            string where = string.Format(" Tope.IsInterval=0 and TOD.OperateId={0} and Tope.CreateDepartmentId={1}", operateId, Auxiliary.DepartmentId());

            //运营值
            if (!string.IsNullOrEmpty(intervalvalue))
            {
                where += string.Format(" And TOD.IntervalValue like '%{0}%'", intervalvalue.Trim());
            }

            List<TraWorkingOperateModel> list = bll.TraIntervalValueList(index, size, where);

            return Json(list);
        }

        /// <summary>
        /// 运营值数据记录数
        /// </summary> 
        /// <param name="intervalvalue">运营值</param>
        /// <returns></returns>
        public int TraIntervalValueCount(string intervalvalue, string operateId)
        {
            string where = string.Format(" Tope.IsInterval=0 and TOD.OperateId={0} and Tope.CreateDepartmentId={1}", operateId, Auxiliary.DepartmentId());

            //运营值
            if (!string.IsNullOrEmpty(intervalvalue))
            {
                where += string.Format(" And TOD.IntervalValue  like '%{0}%' ", intervalvalue.Trim());
            }

            return bll.TraIntervalValueCount(where);
        }

        #endregion

        #region  批量导入每月运营过程记录

        /// <summary>
        /// 批量导入每月运营过程记录
        /// </summary>
        /// <param name="path">excel文件</param> 
        [Operate(Name = OperateEnum.Import)]
        public ActionResult ImportInfo(string path)
        {
            List<TraWorkingOperateModel> list = new List<TraWorkingOperateModel>();

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
                    //运作名称和运作类型无法导入

                    string num = ws.Cells[i, 0].StringValue.ToString();//序号

                    string transportName = ws.Cells[i, 1].StringValue.ToString();//供应商名称

                    string checktypename = ws.Cells[i, 2].StringValue.ToString();//考核类型

                    string workingtimeyear = ws.Cells[i, 3].StringValue.ToString();//考核年

                    string workingtimemonth = ws.Cells[i, 4].StringValue.ToString();//考核月

                    string operatename = ws.Cells[i, 5].StringValue.ToString();//运营类型名称

                    string operatevalue = ws.Cells[i, 6].StringValue.ToString(); //运营值

                    TraWorkingOperateModel model = new TraWorkingOperateModel();

                    if (string.IsNullOrEmpty(transportName) && string.IsNullOrEmpty(checktypename) && string.IsNullOrEmpty(workingtimeyear) && string.IsNullOrEmpty(workingtimemonth) && string.IsNullOrEmpty(operatename) && string.IsNullOrEmpty(operatevalue))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(transportName) || string.IsNullOrEmpty(checktypename) || string.IsNullOrEmpty(workingtimeyear) || string.IsNullOrEmpty(workingtimemonth) || string.IsNullOrEmpty(operatename) || string.IsNullOrEmpty(operatevalue))
                    {
                        content += i + " ";
                        failcount++;
                        continue;
                    }

                    #region 判断供应商是否存在

                    string where = string.Format(" ST.TransportState='F4' and S.CreateDepartmentId={0}", Auxiliary.DepartmentId());

                    if (!string.IsNullOrEmpty(transportName))
                    {
                        where += string.Format(" And s.SupplierName  = '{0}'", transportName.Trim());
                    }

                    TraWorkingOperateModel OrderModel = bll.TraWorkingOperateSupplierLists(where);

                    if (OrderModel == null)
                    {
                        content += i + " ";
                        failcount++;
                        continue;//跳出此条数据
                    }
                    #endregion

                    #region 考核类型对应该类型运营类型名称

                    int checktype = 0;

                    if (checktypename == "调拨")
                    {
                        checktype = 0;
                    }
                    else if (checktypename == "配送(干线)" || checktypename == "配送（干线）" || checktypename == "配送(干线）" || checktypename == "配送（干线)" || checktypename == "配送干线")
                    {
                        checktype = 1;
                    }
                    else if (checktypename == "配送(终端)" || checktypename == "配送（终端）" || checktypename == "配送(终端）" || checktypename == "配送（终端)" || checktypename == "配送终端")
                    {
                        checktype = 2;
                    }
                    else
                    {
                        continue;
                    }

                    TraOperateModel operatenameModel = TOBLL.GetModelByID(operatename, checktype);

                    if (operatenameModel == null)
                    {
                        content += i + " ";
                        failcount++;
                        continue;//跳出此条数据
                    }

                    #endregion

                    #region 验证运营值问题

                    int operateid = operatenameModel.OperateId;

                    int failcounts = 0;

                    List<TraOperateModel> traoperatemodel = TOBLL.GetModelByOperateId(operateid);

                    for (int k = 0; k < traoperatemodel.Count; k++)
                    {

                        //区间上值
                        int intervalbegin = Convert.ToInt32(traoperatemodel[k].IntervalBegin);

                        //运营值
                        int intervalvalue = Convert.ToInt32(operatevalue);

                        //区间下值
                        int intervalend = Convert.ToInt32(traoperatemodel[k].IntervalEnd);

                        if (intervalbegin < intervalvalue && intervalvalue < intervalend)
                        {
                            model.OperateValue = operatevalue;
                            continue;
                        }
                        else
                        {
                            if (traoperatemodel[k].IntervalTpye == 1)
                            {
                                if (intervalbegin == intervalvalue)
                                {
                                    model.OperateValue = operatevalue;
                                    continue;
                                }
                                else
                                {
                                    failcounts++; 
                                }
                            }
                            else if (traoperatemodel[k].IntervalTpye == 2)
                            {
                                if (intervalvalue == intervalend)
                                {
                                    model.OperateValue = operatevalue;
                                    continue;
                                }
                                else
                                {
                                    failcounts++; 
                                }
                            }
                            else if (traoperatemodel[k].IntervalTpye == 3)
                            {
                                if (intervalbegin == intervalvalue || intervalvalue == intervalend)
                                {
                                    model.OperateValue = operatevalue;
                                    continue;
                                }
                                else
                                {
                                    failcounts++; 
                                }
                            }

                            
                        }
                    }

                    //if (traoperatemodel.Count == failcounts)
                    //{
                    //    return Json(new { flag = "fail", content = "运营值必须在区间范围之内！" });
                    //}
                    #endregion

                    #region 时间验证

                    string nowdate = DateTime.Now.ToString("yyyy-MM-dd");//当前时间

                    int nowdate1 = Convert.ToInt32(nowdate.Split('-')[1]);//取当前时间的月份

                    int nowdate2 = Convert.ToInt32(nowdate.Split('-')[2]);//取当前时间的天

                    string date1 = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"); //获取系统前一个月的时间

                    int dt3 = Convert.ToInt32(date1.Split('-')[1]);//取上一个月的月份

                    int workingmonth = Convert.ToInt32(workingtimemonth);//考核月

                    //运作时间是否为当前时间的上一个月
                    if (workingmonth == dt3)
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

                    #endregion

                    model.TransportId = OrderModel.TransportId;

                    model.WorkingTimeYear = Convert.ToInt32(workingtimeyear);

                    model.WorkingTimeMonth = Convert.ToInt32(workingtimemonth);

                    model.CheckTypeName = checktypename;

                    if (model.OperateValue == null)
                    { 
                        content += i + " ";
                        failcount++;
                        continue;
                    }
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

                    model.OperateId = operatenameModel.OperateId;

                    model.State = 5;// 状态：5-有效(提交操作);10-作废

                    model.UseState = 0;// 状态：0-未使用;1-已使用

                    model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

                    model.CreateUserId = Auxiliary.UserID();// 用户ID

                    model.CreateTime = DateTime.Now;//创建时间

                    model.CompanyId = Auxiliary.CompanyID();//公司id

                    list.Add(model);

                    //如果为空，添加验证
                    if (string.IsNullOrEmpty(workingtimeyear))
                    {
                        break;
                    }
                }
            }
            bool res = false;

            if (list.Count > 0)

                res = new TraWorkingOperateBLL().AddBulk(list);

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
