//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/12/13    1.0        HDS        
//-------------------------------------------------------------------------
#region using
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Model.Car;
using System.Data.SqlClient;
#endregion
/*********************************
 * 类名：CarOrderQueryDAL
 * 功能描述：订单查询 数据访问层类
 * ******************************/

namespace DAL.Car
{
    #region 订单查询
    public class CarOrderQueryDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        /// <summary>
        /// 分页总数 订单查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int PageCouts(string where)
        {
            //执行的SQL语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(1) FROM dbo.VCarOrderQuery ");
            strSql.Append(where);
            try
            {
                object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, strSql.ToString(), null);// 执行SQL语句
                if (obj != null)
                {
                    return Convert.ToInt32(obj.ToString());                                           //获取第一行第一列
                }
                else
                {
                    return 0;                                                                         //无结果返回0
                }
            }
            catch (Exception)
            {
                return 0;                                                                             //异常 返回 0
            }
        }


        /// <summary>
        /// 分页列表 订单查询
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">每页行数</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<CarOrderQueryModel> PageList(int index, int size, string where)
        {
            //执行的SQL语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"With TemTable AS(Select   Row_Number() Over(Order By OrderId ) AS 't',* FROM dbo.VCarOrderQuery WHERE =@Where )  Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ");

            SqlParameter[] param ={
                                   new SqlParameter("@Where", where), //查询条件
                                   new SqlParameter("@Index",index),  //页码
                                   new SqlParameter("@Size",size)     //每页行数
                                  }; 
            List<CarOrderQueryModel> list = new List<CarOrderQueryModel>(); 
            try
            {
                DataSet ds = SQLHelper.GetDataSet(conn, CommandType.Text, strSql.ToString(), param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    list = GetList(ds.Tables[0]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }


        /// <summary>
        /// 导出Excel的List<Model>对象
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<CarOrderQueryModel> ExcelLiset(string where)
        {
            //执行的SQL语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" Select  * FROM dbo.VCarOrderQuery WHERE =@Where  ");

            SqlParameter[] param ={
                                   new SqlParameter("@Where", where)         //查询条件
                                  };
            List<CarOrderQueryModel> list = new List<CarOrderQueryModel>();
            try
            {
                DataSet ds = SQLHelper.GetDataSet(conn, CommandType.Text, strSql.ToString(), param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    list = GetList(ds.Tables[0]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        /// <summary>
        /// 根据查询DataTable返回List<Model>对象
        /// </summary>
        /// <param name="dt">查询DataTable结果</param>
        /// <returns></returns>
        public List<CarOrderQueryModel> GetList(DataTable dt)
        {
            List<CarOrderQueryModel> list = new List<CarOrderQueryModel>(); 
            try
            { 
                int tryInt;// 强制转换int类型的结果
                DateTime tryTime;// 强制转换DateTime类型的结果
                decimal tryDecimal;// 强制转换decimal类型的结果
                #region for 循环
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CarOrderQueryModel model = new CarOrderQueryModel();
                    //订单ID(OrderId  int)
                    if (dt.Columns.Contains("OrderId"))
                    {
                        if (int.TryParse(dt.Rows[i]["OrderId"].ToString(), out tryInt))
                        {
                            model.OrderId = tryInt;
                        }
                    }

                    //交货单号(PartNo  string)
                    if (dt.Columns.Contains("PartNo"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["PartNo"].ToString()))
                        {
                            model.PartNo = dt.Rows[i]["PartNo"].ToString();
                        }
                    }

                    //订单状态(OrderState  int)
                    if (dt.Columns.Contains("OrderState"))
                    {
                        if (int.TryParse(dt.Rows[i]["OrderState"].ToString(), out tryInt))
                        {
                            model.OrderState = tryInt;
                        }
                    }

                    //订单状态(OrderStateName  string)
                    if (dt.Columns.Contains("OrderStateName"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["OrderStateName"].ToString()))
                        {
                            model.OrderStateName = dt.Rows[i]["OrderStateName"].ToString();
                        }
                    }

                    //订单号(OrderNumber  string)
                    if (dt.Columns.Contains("OrderNumber"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["OrderNumber"].ToString()))
                        {
                            model.OrderNumber = dt.Rows[i]["OrderNumber"].ToString();
                        }
                    }

                    //承运商(TransportName  string)
                    if (dt.Columns.Contains("TransportName"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["TransportName"].ToString()))
                        {
                            model.TransportName = dt.Rows[i]["TransportName"].ToString();
                        }
                    }

                    //供应商Id(SupplierId  int)
                    if (dt.Columns.Contains("SupplierId"))
                    {
                        if (int.TryParse(dt.Rows[i]["SupplierId"].ToString(), out tryInt))
                        {
                            model.SupplierId = tryInt;
                        }
                    }

                    //供应商名称(SupplierName  sting)
                    if (dt.Columns.Contains("SupplierName"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["SupplierName"].ToString()))
                        {
                            model.SupplierName = dt.Rows[i]["SupplierName"].ToString();
                        }
                    }

                    //供应商代码(SupplierNumber  sting)
                    if (dt.Columns.Contains("SupplierNumber"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["SupplierNumber"].ToString()))
                        {
                            model.SupplierNumber = dt.Rows[i]["SupplierNumber"].ToString();
                        }
                    }

                    //取货日期(TakeTime  DateTime)
                    if (dt.Columns.Contains("TakeTime"))
                    {
                        if (DateTime.TryParse(dt.Rows[i]["TakeTime"].ToString(), out tryTime))
                        {
                            model.TakeTime = tryTime;
                        }
                    }

                    //到货日期(ReachTime  DateTime)
                    if (dt.Columns.Contains("ReachTime"))
                    {
                        if (DateTime.TryParse(dt.Rows[i]["ReachTime"].ToString(), out tryTime))
                        {
                            model.ReachTime = tryTime;
                        }
                    }

                    //工厂代码(FactoryNumber  sting)
                    if (dt.Columns.Contains("FactoryNumber"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["FactoryNumber"].ToString()))
                        {
                            model.FactoryNumber = dt.Rows[i]["FactoryNumber"].ToString();
                        }
                    }

                    //工厂地址(FactoryAddress  sting)
                    if (dt.Columns.Contains("FactoryAddress"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["FactoryAddress"].ToString()))
                        {
                            model.FactoryAddress = dt.Rows[i]["FactoryAddress"].ToString();
                        }
                    }

                    //车牌号(CarNumber  sting)
                    if (dt.Columns.Contains("CarNumber"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["CarNumber"].ToString()))
                        {
                            model.CarNumber = dt.Rows[i]["CarNumber"].ToString();
                        }
                    }

                    //取货总收入(ReachToIncome  sting)
                    if (dt.Columns.Contains("ReachToIncome"))
                    {
                        if (decimal.TryParse(dt.Rows[i]["ReachToIncome"].ToString(), out tryDecimal))
                        {
                            model.ReachToIncome = tryDecimal;
                        }
                    }

                    //返空总收入(ReturnToIncome  sting)
                    if (dt.Columns.Contains("ReturnToIncome"))
                    {
                        if (decimal.TryParse(dt.Rows[i]["ReturnToIncome"].ToString(), out tryDecimal))
                        {
                            model.ReturnToIncome = tryDecimal;
                        }
                    }

                    //总收入(TotalIncome  sting)
                    if (dt.Columns.Contains("TotalIncome"))
                    {
                        if (decimal.TryParse(dt.Rows[i]["TotalIncome"].ToString(), out tryDecimal))
                        {
                            model.TotalIncome = tryDecimal;
                        }
                    }

                    //状态(State  int)
                    if (dt.Columns.Contains("State"))
                    {
                        if (int.TryParse(dt.Rows[i]["State"].ToString(), out tryInt))
                        {
                            model.State = tryInt;
                        }
                    }

                    //创建机构Id(CreateDepartmentId  int)
                    if (dt.Columns.Contains("CreateDepartmentId"))
                    {
                        if (int.TryParse(dt.Rows[i]["CreateDepartmentId"].ToString(), out tryInt))
                        {
                            model.CreateDepartmentId = tryInt;
                        }
                    }

                    //创建用户id(CreateUserId  int)
                    if (dt.Columns.Contains("CreateUserId"))
                    {
                        if (int.TryParse(dt.Rows[i]["CreateUserId"].ToString(), out tryInt))
                        {
                            model.CreateUserId = tryInt;
                        }
                    }

                    //创建时间(CreateTime  sting)
                    if (dt.Columns.Contains("CreateTime"))
                    {
                        if (DateTime.TryParse(dt.Rows[i]["CreateTime"].ToString(), out tryTime))
                        {
                            model.CreateTime = tryTime;
                        }
                    }

                    // 操作报价Id(HandleId  int)
                    if (dt.Columns.Contains("HandleId"))
                    {
                        if (int.TryParse(dt.Rows[i]["HandleId"].ToString(), out tryInt))
                        {
                            model.HandleId = tryInt;
                        }
                    }

                    //删除用户Id(DelUserId  int)
                    if (dt.Columns.Contains("DelUserId"))
                    {
                        if (int.TryParse(dt.Rows[i]["DelUserId"].ToString(), out tryInt))
                        {
                            model.DelUserId = tryInt;
                        }
                    }

                    //删除时间(DelTime  sting)
                    if (dt.Columns.Contains("DelTime"))
                    {
                        if (DateTime.TryParse(dt.Rows[i]["DelTime"].ToString(), out tryTime))
                        {
                            model.DelTime = tryTime;
                        }
                    }

                    //系统公司id(CompanyId  int)
                    if (dt.Columns.Contains("CompanyId"))
                    {
                        if (int.TryParse(dt.Rows[i]["CompanyId"].ToString(), out tryInt))
                        {
                            model.CompanyId = tryInt;
                        }
                    }

                    //交货数量(SumPartNumber  int)
                    if (dt.Columns.Contains("SumPartNumber"))
                    {
                        if (int.TryParse(dt.Rows[i]["SumPartNumber"].ToString(), out tryInt))
                        {
                            model.SumPartNumber = tryInt;
                        }
                    }

                    //箱数(SumBoxNumber  int)
                    if (dt.Columns.Contains("SumBoxNumber"))
                    {
                        if (int.TryParse(dt.Rows[i]["SumBoxNumber"].ToString(), out tryInt))
                        {
                            model.SumBoxNumber = tryInt;
                        }
                    }

                    //拖数(SumDragNumber  int)
                    if (dt.Columns.Contains("SumDragNumber"))
                    {
                        if (int.TryParse(dt.Rows[i]["SumDragNumber"].ToString(), out tryInt))
                        {
                            model.SumDragNumber = tryInt;
                        }
                    }

                    //报价类型(OfferTypes  int)
                    if (dt.Columns.Contains("OfferTypes"))
                    {
                        if (int.TryParse(dt.Rows[i]["OfferTypes"].ToString(), out tryInt))
                        {
                            model.OfferTypes = tryInt;
                        }
                    }

                    //报价类型(OfferTypesName  int)
                    if (dt.Columns.Contains("OfferTypesName"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["OfferTypesName"].ToString()))
                        {
                            model.OfferTypesName = dt.Rows[i]["OfferTypesName"].ToString();
                        }
                    }

                    list.Add(model);
                } 
                #endregion
            }
            catch (Exception)
            {

                throw;
            }
            return list;
        }
    }
    #endregion

   public class CarOrderDetailDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        /// <summary>
        /// 分页总数 订单明细查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int PageCouts(string where)
        {
            //执行的SQL语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(1) FROM dbo.CarOrderDetail ");
            strSql.Append(where);
            try
            {
                object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, strSql.ToString(), null);// 执行SQL语句
                if (obj != null)
                {
                    return Convert.ToInt32(obj.ToString());                                           //获取第一行第一列
                }
                else
                {
                    return 0;                                                                         //无结果返回0
                }
            }
            catch (Exception)
            {
                return 0;                                                                             //异常 返回 0
            }
        }

        /// <summary>
        /// 分页列表 订单明细查询
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">每页行数</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<CarOrderDetailQueryModel> PageList(int index, int size, string where)
        {
            //执行的SQL语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"With TemTable AS(Select   Row_Number() Over(Order By OrderId ) AS 't',* FROM dbo.VCarOrderDetailQuery WHERE =@Where )  Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ");

            SqlParameter[] param ={
                                   new SqlParameter("@Where", where), //查询条件
                                   new SqlParameter("@Index",index),  //页码
                                   new SqlParameter("@Size",size)     //每页行数
                                  };
            List<CarOrderDetailQueryModel> list = new List<CarOrderDetailQueryModel>();
            try
            {
                DataSet ds = SQLHelper.GetDataSet(conn, CommandType.Text, strSql.ToString(), param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    list = GetList(ds.Tables[0]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        /// <summary>
        /// 导出Excel的List<Model>对象
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<CarOrderDetailQueryModel> ExcelLiset(string where)
        {
            //执行的SQL语句
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" Select  * FROM dbo.VCarOrderDetailQuery WHERE =@Where  ");

            SqlParameter[] param ={
                                   new SqlParameter("@Where", where)         //查询条件
                                  };
            List<CarOrderDetailQueryModel> list = new List<CarOrderDetailQueryModel>();
            try
            {
                DataSet ds = SQLHelper.GetDataSet(conn, CommandType.Text, strSql.ToString(), param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    list = GetList(ds.Tables[0]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        /// <summary>
        /// 根据查询DataTable返回List<Model>对象
        /// </summary>
        /// <param name="dt">查询DataTable结果</param>
        /// <returns></returns>
        public List<CarOrderDetailQueryModel> GetList(DataTable dt)
        {
            List<CarOrderDetailQueryModel> list = new List<CarOrderDetailQueryModel>();
            try
            {
                int tryInt;// 强制转换int类型的结果 
                decimal tryDecimal;// 强制转换decimal类型的结果
                #region for 循环
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CarOrderDetailQueryModel model = new CarOrderDetailQueryModel();

                    //交货单号(PartNo  string)
                    if (dt.Columns.Contains("PartNo"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["PartNo"].ToString()))
                        {
                            model.PartNo = dt.Rows[i]["PartNo"].ToString();
                        }
                    }

                    //订单号(OrderNumber  string)
                    if (dt.Columns.Contains("OrderNumber"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["OrderNumber"].ToString()))
                        {
                            model.OrderNumber = dt.Rows[i]["OrderNumber"].ToString();
                        }
                    }

                    //零件名称(PartName  string)
                    if (dt.Columns.Contains("PartName"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["PartName"].ToString()))
                        {
                            model.PartName = dt.Rows[i]["PartName"].ToString();
                        }
                    }

                    //零件代码(PartNumberString  string)
                    if (dt.Columns.Contains("PartNumberString"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["PartNumberString"].ToString()))
                        {
                            model.PartNumberString = dt.Rows[i]["PartNumberString"].ToString();
                        }
                    }

                    //包装代码(PackingNumber  string)
                    if (dt.Columns.Contains("PackingNumber"))
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[i]["PackingNumber"].ToString()))
                        {
                            model.PackingNumber = dt.Rows[i]["PackingNumber"].ToString();
                        }
                    }

                    //SNP(SNP  sting)
                    if (dt.Columns.Contains("SNP"))
                    {
                        if (decimal.TryParse(dt.Rows[i]["SNP"].ToString(), out tryDecimal))
                        {
                            model.SNP = tryDecimal;
                        }
                    }

                    //交货数量(PartNumber  int)
                    if (dt.Columns.Contains("PartNumber"))
                    {
                        if (int.TryParse(dt.Rows[i]["PartNumber"].ToString(), out tryInt))
                        {
                            model.PartNumber = tryInt;
                        }
                    }

                    //箱数(BoxNumber  int)
                    if (dt.Columns.Contains("BoxNumber"))
                    {
                        if (int.TryParse(dt.Rows[i]["BoxNumber"].ToString(), out tryInt))
                        {
                            model.BoxNumber = tryInt;
                        }
                    }

                    //拖数(DragNumber  int)
                    if (dt.Columns.Contains("DragNumber"))
                    {
                        if (int.TryParse(dt.Rows[i]["DragNumber"].ToString(), out tryInt))
                        {
                            model.DragNumber = tryInt;
                        }
                    }

                    //实收箱数(ReceiptBoxNumber  int)
                    if (dt.Columns.Contains("ReceiptBoxNumber"))
                    {
                        if (int.TryParse(dt.Rows[i]["ReceiptBoxNumber"].ToString(), out tryInt))
                        {
                            model.ReceiptBoxNumber = tryInt;
                        }
                    }

                    //实收拖数(ReceiptDragNumber  int)
                    if (dt.Columns.Contains("ReceiptDragNumber"))
                    {
                        if (int.TryParse(dt.Rows[i]["ReceiptDragNumber"].ToString(), out tryInt))
                        {
                            model.ReceiptDragNumber = tryInt;
                        }
                    }

                    //取货收入(ReachIncome  sting)
                    if (dt.Columns.Contains("ReachIncome"))
                    {
                        if (decimal.TryParse(dt.Rows[i]["ReachIncome"].ToString(), out tryDecimal))
                        {
                            model.ReachIncome = tryDecimal;
                        }
                    }

                    //返空收入(ReturnIncome  sting)
                    if (dt.Columns.Contains("ReturnIncome"))
                    {
                        if (decimal.TryParse(dt.Rows[i]["ReturnIncome"].ToString(), out tryDecimal))
                        {
                            model.ReturnIncome = tryDecimal;
                        }
                    }

                    //总收入(Income  sting)
                    if (dt.Columns.Contains("Income"))
                    {
                        if (decimal.TryParse(dt.Rows[i]["Income"].ToString(), out tryDecimal))
                        {
                            model.Income = tryDecimal;
                        }
                    }

                    //取货单价(ReachPrices  sting)
                    if (dt.Columns.Contains("ReachPrices"))
                    {
                        if (decimal.TryParse(dt.Rows[i]["ReachPrices"].ToString(), out tryDecimal))
                        {
                            model.ReachPrices = tryDecimal;
                        }
                    }

                    //返空单价(ReturnPrices  sting)
                    if (dt.Columns.Contains("ReturnPrices"))
                    {
                        if (decimal.TryParse(dt.Rows[i]["ReturnPrices"].ToString(), out tryDecimal))
                        {
                            model.ReturnPrices = tryDecimal;
                        }
                    }

                    list.Add(model);
                }
                #endregion
            }
            catch (Exception)
            {

                throw;
            }
            return list;
        }
    }


}
