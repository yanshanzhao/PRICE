//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/12/13    1.0        ZBB        新建   
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using Model.Car;
#endregion
/*********************************
 * 类名：CarOrderDAL
 * 功能描述：订单记录表 数据访问层类  
 * ******************************/

namespace DAL.Car
{
    public class CarOrderDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 订单记录表

        /// <summary>
        /// 添加 订单记录表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int AddCarOrder(CarOrderModel model)
        {
            string sql = @"Insert into
                                  CarOrder
                                  (
                                       PartNo,
                                       OrderState,
                                       OrderNumber,
                                       TransportName,
                                       SupplierId,
                                       TakeTime,
                                       ReachTime,
                                       FactoryNumber,
                                       FactoryAddress,
                                       CarNumber,
                                       ReachToIncome,
                                       ReturnToIncome,
                                       TotalIncome,
                                       State,
                                       CreateDepartmentId,
                                       CreateUserId, 
                                       CreateTime,
                                       HandleId,
                                       CompanyId
                                  )
                          Values
                                  (
                                       @PartNo,
                                       @OrderState,
                                       @OrderNumber,
                                       @TransportName,
                                       @SupplierId,
                                       @TakeTime,
                                       @ReachTime,
                                       @FactoryNumber,
                                       @FactoryAddress,
                                       @CarNumber,
                                       @ReachToIncome,
                                       @ReturnToIncome,
                                       @TotalIncome,
                                       @State,
                                       @CreateDepartmentId,
                                       @CreateUserId, 
                                       GETDATE(),
                                       @HandleId,
                                       @CompanyId
                                  )";
            SqlParameter[] param ={  
                                        //交货单号  
                                        new SqlParameter("@PartNo",model.PartNo),

                                        //订单状态(1-加急；0-非加急)
                                        new SqlParameter("@OrderState",model.OrderState),

                                        //订单号 
                                        new SqlParameter("@OrderNumber",model.OrderNumber),    

                                        //承运商   
                                        new SqlParameter("@TransportName",model.TransportName), 

                                        //供应商Id
                                        new SqlParameter("@SupplierId",model.SupplierId),

                                        //取货日期
                                        new SqlParameter("@TakeTime",model.TakeTime),  

                                        //到货日期 
                                        new SqlParameter("@ReachTime",model.ReachTime), 

                                        //工厂代码
                                        new SqlParameter("@ Fact oryNu mber", model.FactoryNumber),
                                           
                                        //工厂地址
                                        new SqlParameter("@FactoryAddress",model.FactoryAddress),   
                                           
                                        //车牌号
                                        new SqlParameter("@CarNumber",model.CarNumber),   

                                        //取货总收入
                                        new SqlParameter("@ Reac hToIn come", model.ReachToIncome), 
                                             
                                        //返空总收入
                                        new SqlParameter("@ReturnToIncome",model.ReturnToIncome), 
                                             
                                        //总收入
                                        new SqlParameter("@TotalIncome",model.TotalIncome),   
                                              
                                        //状态：0-待计算；1-已计算;10-删除;20-作废
                                        new SqlParameter("@State",model.State),    

                                        //创建机构Id
                                        new SqlParameter("@CreateDepartmentId",model.CreateDepartmentId),     
                                         
                                        //创建用户id
                                        new SqlParameter("@CreateUserId",model.CreateUserId),   

                                        //操作报价Id
                                        new SqlParameter("@HandleId",model.HandleId),   

                                        //系统公司id
                                        new SqlParameter("@CompanyId",model.CompanyId)
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 修改 订单记录表

        /// <summary>
        /// 修改 订单记录表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int UpdateCarOrder(CarOrderModel model)
        {
            string sql = @"Update 
                                  CarOrder
                          Set
                                  PartNo = @PartNo ,
                                  OrderState = @OrderState ,
                                  OrderNumber = @OrderNumber ,
                                  TransportName = @TransportName ,
                                  SupplierId = @SupplierId ,
                                  TakeTime = @TakeTime,
                                  ReachTime = @ReachTime,
                                  FactoryNumber = @FactoryNumber,
                                  FactoryAddress = @FactoryAddress,
                                  CarNumber = @CarNumber,
                                  ReachToIncome = @ReachToIncome,
                                  ReturnToIncome = @ReturnToIncome ,
                                  TotalIncome = @TotalIncome ,
                                  HandleId = @HandleId  
                          Where         
                                  OrderId = @OrderId";
            SqlParameter[] param ={
                                        //订单ID
                                        new SqlParameter("@OrderId",model.OrderId),  

                                        //交货单号
                                        new SqlParameter("@PartNo",model.PartNo),    

                                        //订单状态(1-加急；0-非加急)
                                        new SqlParameter("@OrderState",model.OrderState),    
                                         
                                        //订单号
                                        new SqlParameter("@OrderNumber",model.OrderNumber), 

                                        //承运商  
                                        new SqlParameter("@TransportName",model.TransportName), 
                                         
                                        //供应商Id 
                                        new SqlParameter("@SupplierId",model.SupplierId), 
                                         
                                        //取货日期
                                        new SqlParameter("@TakeTime",model.TakeTime),  

                                        //到货日期   
                                        new SqlParameter("@ReachTime",model.ReachTime),    

                                        //工厂代码
                                        new SqlParameter("@FactoryNumber",model.FactoryNumber),   
                                            
                                        //工厂地址
                                        new SqlParameter("@FactoryAddress",model.FactoryAddress),  
                                            
                                        //车牌号 
                                        new SqlParameter("@CarNumber",model.CarNumber),      

                                        //取货总收入    
                                        new SqlParameter("@ReachToIncome",model.ReachToIncome),  

                                        //返空总收入    
                                        new SqlParameter("@ReturnToIncome",model.ReturnToIncome), 
                                          
                                        //总收入
                                        new SqlParameter("@TotalIncome",model.TotalIncome), 
                                                
                                        //操作报价Id   
                                        new SqlParameter("@HandleId",model.HandleId),
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);

            return i;
        }
        #endregion

        #region 分页列表 订单记录表

        /// <summary>
        /// 分页列表 订单记录表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public List<CarOrderModel> CarOrderList(int index, int size, string where)
        {
            string sql = @" With TemTable As 
                            ( 
                              Select
                                Row_Number() Over(Order By CO.OrderId Desc) AS 't'
                                ,CO.OrderId
                                ,CO.PartNo
                                ,CS.SupplierName
                                ,CS.SupplierNumber
                                ,CO.TakeTime
                                ,CO.ReachTime
                                ,COD.BoxNumber
                                ,COD.DragNumber 
                                ,COD.PartNumber 
                                ,CO.CarNumber  
                                 ,CH.OfferTypes
                                , CASE CH.OfferTypes
                                   WHEN '1' THEN '件数收入'
                                   WHEN '2' THEN '返空体积收入'
                                   WHEN '3' THEN '返空体积收入'
                                 END AS OfferTypesName
                                ,CO.ReachToIncome 
                                ,CO.ReturnToIncome 
                                ,CO.TotalIncome 
                                ,CO.State 
                                , CASE CO.State
                                   WHEN '0' THEN '待计算'
                                   WHEN '1' THEN '已计算'
                                   WHEN '10' THEN '删除'
                                   WHEN '20' THEN '作废'
                                 END AS StateName
                                From CarOrder CO 
								Left Join CarOrderDetail COD on CO.OrderId=COD.OrderId 
								Left Join CarHandle CH on CO.HandleId=CH.HandleId 
								Left Join CarSupplier CS on CO.SupplierId=CS.SupplierId Where " + where + @"
                            ) 
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";

            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };

            List<CarOrderModel> list = new List<CarOrderModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);

            while (dr.Read())
            {
                CarOrderModel model = new CarOrderModel();

                //订单id
                model.OrderId = Convert.ToInt32(dr["OrderId"].ToString());

                //交货单号 
                model.PartNo = dr["PartNo"].ToString();

                //供应商名称 
                model.SupplierName = dr["SupplierName"].ToString();

                //供应商代码 
                model.SupplierNumber = dr["SupplierNumber"].ToString();

                //取货日期 
                model.TakeTime = Convert.ToDateTime(dr["TakeTime"].ToString());

                //到货日期 
                model.ReachTime = Convert.ToDateTime(dr["ReachTime"].ToString());

                //箱数
                model.BoxNumber = Convert.ToInt32(dr["BoxNumber"].ToString());

                //拖数
                model.DragNumber = Convert.ToInt32(dr["DragNumber"].ToString());

                //交货数量 
                model.PartNumber = Convert.ToInt32(dr["PartNumber"].ToString());

                // 车牌号
                model.CarNumber = dr["CarNumber"].ToString();

                //生成报价状态
                model.OfferTypes = Convert.ToInt32(dr["OfferTypes"].ToString());

                //显示生成报价状态 
                model.OfferTypesName = dr["OfferTypesName"].ToString();

                //取货总收入
                model.ReachToIncome = Convert.ToInt32(dr["ReachToIncome"].ToString());

                //返空总收入
                model.ReturnToIncome = Convert.ToInt32(dr["ReturnToIncome"].ToString());

                //总收入
                model.TotalIncome = Convert.ToInt32(dr["TotalIncome"].ToString());

                //收入计算状态
                model.State = Convert.ToInt32(dr["State"].ToString());

                //显示收入计算状态
                model.StateName = dr["StateName"].ToString();

                list.Add(model);// 加入到数据集
            }
            dr.Close();//关闭 
            return list;// 返回数据集
        }
        #endregion  

        #region 分页总数 订单记录表

        /// <summary>
        /// 分页总数 订单记录表
        /// </summary>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public int CarOrderCount(string where)
        {
            string sql = @" Select 
                                    Count(CO.OrderId) 
                            From
                                    CarOrder CO
                            Where
                                    " + where + "";
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, null);
            if (obj != null)
            {
                return Convert.ToInt32(obj.ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 获取实体 订单记录表

        /// <summary>
        /// 获取实体 订单记录表
        /// </summary>
        /// <param name="Id">订单记录表id</param>
        /// <returns></returns>
        public CarOrderModel GetModelByID(int Id)
        {
            string sql = @"  Select
                                Row_Number() Over(Order By CO.OrderId Desc) AS 't'
                                ,CO.OrderId
                                ,CO.PartNo
                                ,CS.SupplierName
                                ,CS.SupplierNumber
                                ,CO.TakeTime
                                ,CO.ReachTime
                                ,COD.BoxNumber
                                ,COD.DragNumber 
                                ,COD.PartNumber 
                                ,CO.CarNumber  
                                ,CH.OfferTypes
                                , CASE CH.OfferTypes
                                   WHEN '1' THEN '件数收入'
                                   WHEN '2' THEN '返空体积收入'
                                   WHEN '3' THEN '返空体积收入'
                                 END AS OfferTypesName
                                ,CO.ReachToIncome 
                                ,CO.ReturnToIncome 
                                ,CO.TotalIncome 
                                ,CO.State 
                                , CASE CO.State
                                   WHEN '0' THEN '待计算'
                                   WHEN '1' THEN '已计算'
                                   WHEN '10' THEN '删除'
                                   WHEN '20' THEN '作废'
                                 END AS StateName                                From CarOrder CO 
								Left Join CarOrderDetail COD on CO.OrderId=COD.OrderId 
								Left Join CarHandle CH on CO.HandleId=CH.HandleId 
								Left Join CarSupplier CS on CO.SupplierId=CS.SupplierId 
                            WHERE
                                CO.OrderId=@OrderId";

            SqlParameter[] param ={
                                   new SqlParameter("@OrderId",Id)
                                  };

            CarOrderModel model = null;

            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            }
            catch (Exception)
            {
                return null;
            }
            if (dr.Read())
            {
                model = new CarOrderModel();

                //订单id
                model.OrderId = Convert.ToInt32(dr["OrderId"].ToString());

                //交货单号 
                model.PartNo = dr["PartNo"].ToString();

                //供应商名称 
                model.SupplierName = dr["SupplierName"].ToString();

                //供应商代码 
                model.SupplierNumber = dr["SupplierNumber"].ToString();

                //取货日期 
                model.TakeTime = Convert.ToDateTime(dr["TakeTime"].ToString());

                //到货日期 
                model.ReachTime = Convert.ToDateTime(dr["ReachTime"].ToString());

                //箱数
                model.BoxNumber = Convert.ToInt32(dr["BoxNumber"].ToString());

                //拖数
                model.DragNumber = Convert.ToInt32(dr["DragNumber"].ToString());

                //交货数量 
                model.PartNumber = Convert.ToInt32(dr["PartNumber"].ToString());

                // 车牌号
                model.CarNumber = dr["CarNumber"].ToString();

                //生成报价状态
                model.OfferTypes = Convert.ToInt32(dr["OfferTypes"].ToString());

                //显示生成报价状态 
                model.OfferTypesName = dr["OfferTypesName"].ToString();

                //取货总收入
                model.ReachToIncome = Convert.ToInt32(dr["ReachToIncome"].ToString());

                //返空总收入
                model.ReturnToIncome = Convert.ToInt32(dr["ReturnToIncome"].ToString());

                //总收入
                model.TotalIncome = Convert.ToInt32(dr["TotalIncome"].ToString());

                //收入计算状态
                model.State = Convert.ToInt32(dr["State"].ToString());

                //显示收入计算状态
                model.StateName = dr["StateName"].ToString();
            }

            dr.Close(); // 关闭

            return model;// 返回model
        }

        #endregion 

        #region 变更 订单记录表状态

        /// <summary>
        /// 变更 订单记录表状态
        /// </summary>
        /// <param name="Id">订单记录表id</param>
        /// <param name="State">订单记录表状态</param>
        /// <returns></returns>
        public int ChangeState(int Id, int State)
        {
            string sql = @"Update 
                                  CarOrder
                          Set                     
                                  State = @State 
                          Where         
                                  OrderId = @OrderId
                         ";
            SqlParameter[] param ={

                new SqlParameter("@OrderId",Id) ,  // 字典ID 

                new SqlParameter("@State",State) // 订单记录表状态   
            };
            // 影响行数
            int row = 0;
            try
            {
                row = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            }
            catch (Exception)
            {
                return 0;
            }
            return row;
        }

        #endregion 

        #region 导出Excel数据 订单记录表

        /// <summary>
        /// 导出Excel数据 订单记录表
        /// </summary>
        /// <param name="tWhere">检索条件</param>
        /// <returns></returns>
        public List<CarOrderModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                        (
                            Select
                                Row_Number() Over(Order By CO.OrderId Desc) AS 't'
                                ,CO.OrderId
                                ,CO.PartNo
                                ,CS.SupplierName
                                ,CS.SupplierNumber
                                ,CO.TakeTime
                                ,CO.ReachTime
                                ,COD.BoxNumber
                                ,COD.DragNumber 
                                ,COD.PartNumber 
                                ,CO.CarNumber  
                                ,CH.OfferTypes
                                , CASE CH.OfferTypes
                                   WHEN '1' THEN '件数收入'
                                   WHEN '2' THEN '返空体积收入'
                                   WHEN '3' THEN '返空体积收入'
                                 END AS OfferTypesName
                                ,CO.ReachToIncome 
                                ,CO.ReturnToIncome 
                                ,CO.TotalIncome 
                                ,CO.State 
                                , CASE CO.State
                                   WHEN '0' THEN '待计算'
                                   WHEN '1' THEN '已计算'
                                   WHEN '10' THEN '删除'
                                   WHEN '20' THEN '作废'
                                 END AS StateName
                                From CarOrder CO 
								Left Join CarOrderDetail COD on CO.OrderId=COD.OrderId 
								Left Join CarHandle CH on CO.HandleId=CH.HandleId 
								Left Join CarSupplier CS on CO.SupplierId=CS.SupplierId  Where " + tWhere + @"
                        ) 
                        Select * From TemTable ";
            List<CarOrderModel> list = new List<CarOrderModel>();   // 实例化数据集
            // 从数据库读取数据
            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            }
            catch (Exception)
            {
                return null;
            }
            // 循环数据加入到数据集中
            while (dr.Read())
            {
                CarOrderModel model = new CarOrderModel();

                //订单id
                model.OrderId = Convert.ToInt32(dr["OrderId"].ToString());

                //交货单号 
                model.PartNo = dr["PartNo"].ToString();

                //供应商名称 
                model.SupplierName = dr["SupplierName"].ToString();

                //供应商代码 
                model.SupplierNumber = dr["SupplierNumber"].ToString();

                //取货日期 
                model.TakeTime = Convert.ToDateTime(dr["TakeTime"].ToString());

                //到货日期 
                model.ReachTime = Convert.ToDateTime(dr["ReachTime"].ToString());

                //箱数
                model.BoxNumber = Convert.ToInt32(dr["BoxNumber"].ToString());

                //拖数
                model.DragNumber = Convert.ToInt32(dr["DragNumber"].ToString());

                //交货数量 
                model.PartNumber = Convert.ToInt32(dr["PartNumber"].ToString());

                // 车牌号
                model.CarNumber = dr["CarNumber"].ToString();

                //生成报价状态
                model.OfferTypes = Convert.ToInt32(dr["OfferTypes"].ToString());

                //显示生成报价状态 
                model.OfferTypesName = dr["OfferTypesName"].ToString();

                //取货总收入
                model.ReachToIncome = Convert.ToInt32(dr["ReachToIncome"].ToString());

                //返空总收入
                model.ReturnToIncome = Convert.ToInt32(dr["ReturnToIncome"].ToString());

                //总收入
                model.TotalIncome = Convert.ToInt32(dr["TotalIncome"].ToString());

                //收入计算状态
                model.State = Convert.ToInt32(dr["State"].ToString());

                //显示收入计算状态
                model.StateName = dr["StateName"].ToString();

                list.Add(model);// 加入到数据集
            }

            dr.Close();// 关闭 

            return list;// 返回数据集
        }
        #endregion
    }
}
