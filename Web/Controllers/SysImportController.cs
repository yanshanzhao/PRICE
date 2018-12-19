//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-23    1.0        HDS         新建               
//-------------------------------------------------------------------------
#region using 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;


using BLL.Sys;
using Model.Sys;
using System.IO;
using Aspose.Cells;
using System.Data;

#endregion

namespace Web.Controllers
{
    public class SysImportController : Controller
    {
        //
        // GET: /SysImport/

        #region 成员变量
        SysImportExcelBLL SysBll = new SysImportExcelBLL();//导入批次
        SysImportDetailBLL ImportDetailBLL = new SysImportDetailBLL();//导入配置明细
        SysImportDetaiViewlBLL DetailViewBLL = new SysImportDetaiViewlBLL();//导入配置页面显示
        SysImportBLL ImportBLL = new SysImportBLL();//导入配置
        #endregion

        #region 页面
        /// <summary>
        /// 导入页面
        /// </summary>
        /// <param name="ImportNumber">导入配置编号</param>
        /// <param name="ModuleName">模块名称</param>
        /// <returns></returns>
        public ActionResult Import(string ImportNumber, string ModuleName)
        {
            ViewBag.ImportNumber = ImportNumber;
            ViewBag.ModuleName = ModuleName;
            return View();
        }


        /// <summary>
        /// 导入明细
        /// </summary> 
        /// <param name="ExcelId">导入PC的id</param>
        /// <param name="ModuleName">模块名称</param>
        /// <param name="ImportNumber">导入配置编号</param>
        /// <returns></returns>
        public ActionResult ExcelView(int ExcelId, string ModuleName, string ImportNumber)
        {
            ViewBag.ExcelId = ExcelId;
            ViewBag.ImportNumber = ImportNumber;
            ViewBag.ModuleName = ModuleName;
            //获取导入编号为 *** 的导入配置
            List<SysImportDetaiViewlModel> lisetDetailView = new List<SysImportDetaiViewlModel>();
            lisetDetailView = DetailViewBLL.GetViewListl(ImportNumber);
            DataTable DetailView = SysBll.ExcelView(ExcelId, ImportNumber);//返回明细信息
            return View(new SysImportViewModel
            {
                LisetModel = lisetDetailView,// 显示表头
                ColumnCount = lisetDetailView.Count,//表头数量
                DetailView = DetailView //导入明细表
            });
        }

        #endregion

        #region 导入页面方法 

        /// <summary>
        /// 分页-总行数
        /// </summary>
        /// <param name="starttime">上传开始时间</param>
        /// <param name="endtime">上传结束时间</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public int RowCount(string starttime, string endtime, int state, string importNumber)
        {
            StringBuilder sbStr = new StringBuilder();
            //查询本机构导入的配送员信息 非作废的信息
            sbStr.Append(string.Format("  CreateDepartmentId={0} and ImportNumber='{1}' and State<20 ", Auxiliary.DepartmentId(), importNumber));
            if (!string.IsNullOrEmpty(starttime))
            {
                sbStr.Append(string.Format(" And CreateTime >='{0}' ", Convert.ToDateTime(starttime).Date));
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sbStr.Append(string.Format(" And CreateTime<'{0}' ", Convert.ToDateTime(endtime).AddDays(1).Date));
            }
            if (state != -1)
            {
                sbStr.Append(string.Format(" And State={0}  ", state));
            }
            return SysBll.SysImportExcelAmount(sbStr.ToString());
        }


        /// <summary>
        /// 分页-列表
        /// </summary>
        /// <param name="index">第几页</param>
        /// <param name="size">每页行数</param>
        /// <param name="starttime">上传开始时间</param>
        /// <param name="endtime">上传结束时间</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public ActionResult ImportList(int index, int size, string starttime, string endtime, int state, string importNumber)
        {
            StringBuilder sbStr = new StringBuilder();
            //查询本机构导入的配送员信息 非作废的信息
            sbStr.Append(string.Format("  CreateDepartmentId={0} and ImportNumber='{1}' and State<20 ", Auxiliary.DepartmentId(), importNumber));
            if (!string.IsNullOrEmpty(starttime))
            {
                sbStr.Append(string.Format(" And CreateTime >='{0}' ", Convert.ToDateTime(starttime).Date));
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sbStr.Append(string.Format(" And CreateTime<'{0}' ", Convert.ToDateTime(endtime).AddDays(1).Date));
            }
            if (state != -1)
            {
                sbStr.Append(string.Format(" And State={0}  ", state));
            }
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();//定义显示时间样式
            timeFormat.DateTimeFormat = "yyyy-MM-dd";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(SysBll.SysImportExcelList(sbStr.ToString(), size, index), Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 上传-并导入数据库
        /// </summary>
        /// <param name="path">文件存放地址</param>
        /// <param name="filename">文件名称</param>
        /// <param name="importNumber">导入编号</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Import)]
        public ActionResult ImportInfo(string path, string filename, string importNumber)
        {
            string contentStr = "";//提示
            try
            {
                SysImportModel model = ImportBLL.GetModel(importNumber);//获取导入的基本信息
                string notImportType = "";//无法导入的类型 无导入信息; 表头有空值; 表头导入数量大于配置数量;导入异常; 无导入配置信息; 无导入配置信息
                bool excelImportBool = true;
                int successCount = 0;//导入成功行数
                                     //int exclId = -2;//新增的ID  -1 异常;0-插入异常;
                List<string> importDetailSql = new List<string>();
                List<int> exceptionList = new List<int>();//导入内容异常的行数
                List<int> exceNullList = new List<int>();//导入异常行（空行或首列空值的行）
                if (model != null)
                {
                    //获取导入批次的ID
                    SysImportExcelModel excelModel = new SysImportExcelModel();
                    excelModel.ExcelName = filename;//文件名称
                    excelModel.ExcelUrl = path;//文件存放地址
                    excelModel.State = 0;//状态：0-初始；1-验证异常；5-验证成功;10-导入成功;20-作废
                    excelModel.ImportNumber = importNumber;//导入编号
                    excelModel.CreateDepartmentId = Auxiliary.DepartmentId();//创建机构
                    excelModel.CreateUserId = Auxiliary.UserID();//创建账号ID
                    excelModel.CreateTime = DateTime.Now;//创建时间
                    excelModel.CompanyId = Auxiliary.CompanyID();//创建公司
                                                                 //exclId = SysBll.AddModel(excelModel);//新增的ID  -1 异常;0-插入异常;
                    #region 插入成功 获取导入批次的ID  
                    //获取导入编号为 *** 的导入配置
                    List<SysImportDetailModel> lisetImportDetail = new List<SysImportDetailModel>();
                    lisetImportDetail = ImportDetailBLL.GetListModel(importNumber);
                    int ImportDetailCounts = lisetImportDetail.Count;//导入配置的机构
                    #region  有导入配置信息  
                    if (ImportDetailCounts > 0)
                    {
                        if (ImportDetailCounts < 30)
                        {
                            using (FileStream fs = new FileStream(Server.MapPath(path), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                Workbook wb = new Workbook(fs);
                                Worksheet ws = wb.Worksheets[0];
                                Cells cells = ws.Cells;
                                int rowCount = cells.MaxDataRow + 1;//当Excel没有一行数据时，读取到的cells.MaxDataRow=-1，当有一行数据时cells.MaxDataRow=0     MaxDataRow：获取最大包含数据列;列下标从0开始
                                int cellCount = cells.MaxDataColumn + 1;//当Excel没有一行数据时，读取到的cells.MaxDataColumn=-1，当有一行数据时cells.MaxDataColumn=0     MaxDataColumn：获取最大包含数据列;列下标从0开始
                                if (rowCount > 1) //第一行为表头，
                                {
                                    if (ImportDetailCounts >= cellCount)
                                    {
                                        //获取第一行，根据第一行的标头找到数据库中对应的列 
                                        List<string> dbcolumn = new List<string>();//数据库对应的列
                                        List<string> excelcolumn = new List<string>();//excel 中的列名
                                        List<int> columntype = new List<int>();//列类型：0-无;1-int;2-decimal;3-varchar;4-nvarchar;5-datetime
                                        List<int> columnlength = new List<int>();//列限长:0-无      
                                        SysImportDetailModel ImportDetail;
                                        for (int i = 0; i < cellCount; i++)
                                        {
                                            Cell cell = ws.Cells[0, i];
                                            if (!string.IsNullOrEmpty(cell.StringValue.ToString().Trim()))
                                            {
                                                ImportDetail = lisetImportDetail.Find(
                                                     delegate (SysImportDetailModel detailModel)
                                                     {
                                                         return detailModel.Excelcolumn == cell.StringValue.ToString().Trim();
                                                     }
                                                    );
                                                if (ImportDetail != null)//  通过导入Excel的表头  找到对应的导入列
                                                {
                                                    dbcolumn.Add(ImportDetail.Dbcolumn);//数据库对应的列
                                                    excelcolumn.Add(ImportDetail.Excelcolumn);//excel 中的列名
                                                    columntype.Add(ImportDetail.Columntype);//列类型：0-无;1-int;2-decimal;3-varchar;4-nvarchar;5-datetime
                                                    columnlength.Add(ImportDetail.Columnlength);//列限长:0-无
                                                }
                                                else
                                                {
                                                    excelImportBool = false;
                                                    notImportType = "表头信息与配置的信息不一致";
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                excelImportBool = false;
                                                notImportType = "表头有空值";
                                                break;
                                            }
                                        }
                                        #region 将Excel导入到表中
                                        if (excelImportBool)
                                        {
                                            #region 导入参数
                                            string insertColumn = ",ExcelId,ImportDepartmentId,ImportUserId,ImportTime,CompanyId,ProvingSate";//补充导入的列
                                            string insertColumnValue = "',{0}," + Auxiliary.DepartmentId() + "," + Auxiliary.UserID() + ",getdate()," + Auxiliary.CompanyID() + "," + 0 + "";//补充导入的列值
                                            string Value; //单元格值                                       
                                            int insertRows = 0;//待导入的行数 
                                            string insertStr = " insert into " + model.TableName + "(" + string.Join(",", dbcolumn) + insertColumn + ") values ";
                                            List<string> RowValues = new List<string>();//每列填充值 
                                            StringBuilder insertValueStr = new StringBuilder();//填充信息
                                            int RowIndex = 0;//行号
                                            bool ICell = true;//检查每行是否有异常
                                            string sql = "";//执行的SQL语句
                                            #endregion

                                            #region for  控制行数 
                                            for (int i = 1; i < rowCount; i++)// 控制行数
                                            {
                                                RowValues.Clear();//清空每列填充值
                                                RowIndex = i + 1;//获取行号
                                                if (exceNullList.Count >= 3)//空行超过三行
                                                {
                                                    break;
                                                }
                                                #region 控制行  并进行验证每个单元格的数据格式是否符合要求 
                                                try
                                                {
                                                    ICell = true;
                                                    for (int j = 0; j < cellCount; j++)// 控制列数
                                                    {
                                                        Cell cell = ws.Cells[i, j];//获取i行j列单元格内的数据
                                                        if (j == 0 && string.IsNullOrEmpty(cell.StringValue.ToString().Trim()))//每列
                                                        {
                                                            exceNullList.Add(RowIndex);
                                                            break; //结束本行的循环
                                                        }
                                                        else
                                                        {
                                                            #region 获取单元格数据 给Value赋值
                                                            if (cell == null)
                                                            {
                                                                Value = "";
                                                            }
                                                            else
                                                            {
                                                                if (string.IsNullOrEmpty(cell.StringValue.ToString().Trim()))
                                                                {
                                                                    Value = "";
                                                                }
                                                                else
                                                                {
                                                                    Value = cell.StringValue.ToString().Trim(); //去掉空格
                                                                    Value = Value.Replace("\r\n", "").Replace("\n", "").Replace("\t", "").Replace(@"""", ""); //去掉特殊字符 
                                                                }
                                                            }
                                                            #endregion 获取单元格数据 给Value赋值

                                                            #region 对于值的类型和值的长度进行验证
                                                            int types = columntype[j];
                                                            int lenths = columnlength[j];
                                                            //列类型：0-无;1-int;2-decimal;3-varchar;4-nvarchar;5-datetime
                                                            if (types > 0)
                                                            {
                                                                if (types == 1) //1-int
                                                                {
                                                                    int tryInt;
                                                                    if (!int.TryParse(Value, out tryInt))
                                                                    {
                                                                        ICell = false;
                                                                        continue;
                                                                    }
                                                                }
                                                                else if (types == 2) //2-decimal
                                                                {
                                                                    decimal tryDecimal;
                                                                    if (!decimal.TryParse(Value, out tryDecimal))
                                                                    {
                                                                        ICell = false;
                                                                        continue;
                                                                    }
                                                                }
                                                                else if (types == 3) //3-varchar
                                                                {
                                                                    char tryDhar;
                                                                    if (!char.TryParse(Value, out tryDhar))
                                                                    {
                                                                        ICell = false;
                                                                        continue;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (tryDhar.ToString().Length > lenths)// 长度验证
                                                                        {
                                                                            ICell = false;
                                                                            continue;
                                                                        }
                                                                    }
                                                                }
                                                                else if (types == 4) //4-nvarchar
                                                                {
                                                                    if (Value.Length > columnlength[j])
                                                                    {
                                                                        ICell = false;
                                                                        continue;
                                                                    }
                                                                }
                                                                else if (types == 5) //5-datetime
                                                                {
                                                                    DateTime tryDateTime;
                                                                    if (!DateTime.TryParse(Value, out tryDateTime))
                                                                    {
                                                                        ICell = false;
                                                                        continue;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    ICell = false;
                                                                    continue;
                                                                }
                                                            }
                                                            #endregion
                                                            RowValues.Add(Value);
                                                        }
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    ICell = false;
                                                }
                                                #endregion 控制行  并进行验证每个单元格的数据格式是否符合要求

                                                #region 将行的结果填充到   insertValueStr中 
                                                if (ICell)
                                                {
                                                    string values = InSertValues("'" + string.Join("','", RowValues) + insertColumnValue, '(') + ",";
                                                    insertValueStr.Append(values);
                                                    insertRows++;
                                                }
                                                else
                                                {
                                                    exceptionList.Add(RowIndex);
                                                }
                                                #endregion

                                                #region 每20行提交到数据库中
                                                if (insertRows > 0 && insertRows % 20 == 0)//每20行提交到数据库中
                                                {
                                                    if (insertValueStr.Length > 0)
                                                    {
                                                        sql = insertStr + insertValueStr.ToString();
                                                        sql = sql.Substring(0, sql.Length - 1); 
                                                        importDetailSql.Add(sql); 
                                                        insertValueStr.Clear();
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion for  控制行数

                                            #region 将剩余sql执行 并清空字符 
                                            if (insertValueStr.Length > 0)
                                            {
                                                sql = insertStr + insertValueStr.ToString();
                                                sql = sql.Substring(0, sql.Length - 1);
                                                importDetailSql.Add(sql);
                                                //int rowsCout = SysBll.ExecuteSql(sql);
                                                //if (rowsCout > 0)
                                                //{
                                                //    successCount = successCount + rowsCout;
                                                //} 
                                            }
                                            insertValueStr.Clear();
                                            #endregion

                                            #region 执行所有个SQL语句
                                            successCount = SysBll.importDetailSql(excelModel, importDetailSql);
                                            importDetailSql.Clear();
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        excelImportBool = false;
                                        notImportType = "表头导入数量大于配置数量";
                                    }
                                }
                                else
                                {
                                    excelImportBool = false;
                                    notImportType = "无导入信息";
                                }
                            }
                        }
                        else
                        {
                            excelImportBool = false;
                            notImportType = "系统不支持超过30列的导入！";
                        }
                    }
                    #endregion 有导入配置信息
                    else
                    {
                        excelImportBool = false;
                        notImportType = "无导入配置信息";
                    }
                    #endregion
                }
                else
                {
                    excelImportBool = false;
                    notImportType = "无导入配置信息";
                }

                if (successCount == 0)
                {
                    excelImportBool = false;
                    notImportType = "导入异常"; 
                }
                #region 记录日志并 提示处理结果
              
                                       // 系统日志
                if (excelImportBool)
                {
                    contentStr = "成功导入" + successCount + "行!";
                    if (exceptionList.Count > 0)
                    {
                        contentStr = "其中无法导入" + exceptionList.Count + ",行号分别是" + string.Join(",", exceptionList) + "。";
                    }
                    Auxiliary.Log(OperateEnum.Import, ResultEnum.Sucess, new { Type = "导入", Success = successCount, content = contentStr });
                    return Json(new { flag = "ok", content = contentStr });
                }
                else
                {
                    contentStr = notImportType;
                    Auxiliary.Log(OperateEnum.Import, ResultEnum.Fail, new { Type = "导入", content = contentStr });
                    return Json(new { flag = "fail", content = contentStr });
                }
                #endregion
            }
            catch (Exception ex)
            { 
                contentStr = "导入异常";
                Auxiliary.Log(OperateEnum.Import, ResultEnum.Fail, new { Type = "导入", content = contentStr });
                return Json(new { flag = "fail", content = contentStr });
            } 
        }


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="excelid">导入Excel PC id</param>
        /// <param name="importnumber">导入编号</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Import)]
        public ActionResult ImportExcel(int excelid, string importnumber, string state, string tiems)
        {
            string contentStr = "导入成功!";
            bool ImportBool = true;
            if (Convert.ToDateTime(tiems).ToString("yyyMMdd") != DateTime.Now.ToString("yyyMMdd"))
            {
                ImportBool = false;
                contentStr = "距离上传时间过长，不允许导入！";
            }
            else
            {
                string procName = "P" + importnumber.Trim() + "Import";//验证存储过程名称
                int ImportState = SysBll.ExcelImport(excelid, procName);//导入结果 0-导入成功;1-导入异常;-1 导入异常
                if (ImportState != 0)
                {
                    ImportBool = false;
                    contentStr = "导入异常！";
                }
            }
            if (ImportBool)//导入成功
            {
                //记录日志
                Auxiliary.Log(OperateEnum.Import, ResultEnum.Sucess, new { Type = "验证", excelid = excelid, content = contentStr });
                return Json(new { flag = "ok", content = contentStr });
            }
            else
            {
                //记录日志
                Auxiliary.Log(OperateEnum.Import, ResultEnum.Fail, new { Type = "验证", excelid = excelid, content = contentStr });
                return Json(new { flag = "fail", content = contentStr });
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="starttime">上传开始时间</param>
        /// <param name="endtime">上传结束时间</param>
        /// <param name="state">状态</param>
        /// 
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string starttime, string endtime, int state, string importNumber)
        {
            StringBuilder sbStr = new StringBuilder();
            //查询本机构导入的配送员信息 非作废的信息
            sbStr.Append(string.Format("  CreateDepartmentId={0} and ImportNumber='{1}' and State<20 ", Auxiliary.DepartmentId(), importNumber));
            if (!string.IsNullOrEmpty(starttime))
            {
                sbStr.Append(string.Format(" And CreateTime >='{0}' ", Convert.ToDateTime(starttime).Date));
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sbStr.Append(string.Format(" And CreateTime<'{0}' ", Convert.ToDateTime(endtime).AddDays(1).Date));
            }
            if (state != -1)
            {
                sbStr.Append(string.Format(" And State={0}  ", state));
            }
            System.Data.DataTable dt = SysBll.ExportDataTable(sbStr.ToString());
            Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);
            // 供应商日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Sucess, new
            {
                Type = "导出",
                UserId = Auxiliary.UserID(),
                ExportTime = System.DateTime.Now
            });
            return Json(new { flag = "ok", guid = url });
        }


        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="excelid">导入Excel PC id</param>
        /// <param name="importnumber">导入编号</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Validated)]
        public ActionResult Validated(int excelid, string importnumber, string state)
        {
            string contentStr = "";
            string procName = "P" + importnumber.Trim() + "Validated";//验证存储过程名称
            contentStr = SysBll.ExcelValidated(excelid, Auxiliary.CompanyID(), procName);
            //记录日志
            Auxiliary.Log(OperateEnum.Validated, ResultEnum.Sucess, new { Type = "验证", excelid = excelid, content = contentStr });
            return Json(new { flag = "ok", content = contentStr });
        }


        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="excelid">导入Excel PC id</param> 
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult Invalid(int excelid)
        {
            string contentStr = "作废成功！";
            int rowCounts = SysBll.UpdateModel(20, Auxiliary.UserID(), DateTime.Now, excelid);
            if (rowCounts == 0)
            {
                contentStr = "作废失败！";
                //记录日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "作废", excelid = excelid, content = contentStr });
                return Json(new { flag = "fail", content = contentStr });
            }
            else
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "作废", excelid = excelid, content = contentStr });
                return Json(new { flag = "ok", content = contentStr });
            }

        }
        #endregion

        #region 导入明细方法



        /// <summary>
        /// 导出明细
        /// </summary>
        /// <param name="importNumber">导入编号</param>
        /// <param name="excelid">导入的ID</param>
        /// <returns></returns>
        public ActionResult ExcelDetailView(string importNumber, int excelid)
        {
            DataTable dt = SysBll.ExcelDetailView(excelid, importNumber);//获取导出样式和导出数据
            Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);
            return Json(new { flag = "ok", guid = url });
        }
        #endregion

        #region 在字符串两端添加指定的字符型引号和括号类符号
        /// <summary>
        /// 在字符串两端添加指定的字符型引号和括号类符号
        /// </summary>
        /// <param name="s"></param>
        /// <param name="quoter"></param>
        /// <returns></returns>
        private string InSertValues(string SertValues, char quoter)
        {
            char[] quoters = { '"', '\'', '(', '[', '@', '>' };
            if (!quoters.Contains(quoter))
                return SertValues;
            else
                switch (quoter)
                {
                    case '"':
                        return '"' + SertValues + '"';
                    case '\'':
                        return '\'' + SertValues + '\'';
                    case '(':
                        return '(' + SertValues + ')';
                    case '[':
                        return '[' + SertValues + ']';
                    case '@':
                        return '@' + SertValues;
                    case '>':
                        return '>' + SertValues;
                    default:
                        return SertValues;
                }
        }
        #endregion


    }

    #region 辅助类
    public class SysImportViewModel
    {
        public List<SysImportDetaiViewlModel> LisetModel { get; set; }
        public int ColumnCount { get; set; }
        public DataTable DetailView { get; set; }
    }
    #endregion
}
