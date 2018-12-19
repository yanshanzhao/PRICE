using DBUtility;
using Model.Car;
using SRM.Model.Car;
using System;
//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//018-12-12     1.0        FJK        新增
//-------------------------------------------------------------------------
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
/*********************************
 * 类名：CarPackingDAL
 * 功能描述：汽车物流-包装器具 数据访问层类
 * ******************************/

namespace DAL.Car
{
   public class CarPackingDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 新增 汽车物流-包装器具表

        /// <summary>
        /// 新增 汽车物流-包装器具表
        /// </summary>
        /// <param name="model">实体Model</param>
        /// <returns>int</returns>
        public int AddCarPacking(CarPackingModel tModel)
        {
            string sql = @"INSERT INTO CarPacking
                                 ( 
                                 PackingName
                                 ,PackingNumber
                                 ,Lengths
                                 ,Widths
                                 ,Heights
                                 ,Volume
                                 ,Prices
                                 ,DragDatio
                                 ,DragVolume
                                 ,ReturnDatio
                                 ,ReturnVolume
                                 ,ReturnPrices
                                 ,SupplierId
                                 ,State
                                 ,CreateDepartmentId
                                 ,CreateUserId
                                 ,CreateTime 
                                 ,CompanyId
                                 )
                           VALUES
                                 (
                                 @PackingName 
                                 ,@PackingNumber
                                 ,@Lengths
                                 ,@Widths
                                 ,@Heights
                                 ,@Volume
                                 ,@Prices
                                 ,@DragDatio
                                 ,@DragVolume
                                 ,@ReturnDatio
                                 ,@ReturnVolume
                                 ,@ReturnPrices
                                 ,@SupplierId
                                 ,@State
                                 ,@CreateDepartmentId
                                 ,@CreateUserId
                                 ,GETDATE()
                                 ,@CompanyId   
                                 );select @@identity";
            SqlParameter[] param ={ 

                // 器具名称 
                new SqlParameter("@PackingName",tModel.PackingName),
                 
                // 包装代码
                new SqlParameter("@PackingNumber",tModel.PackingNumber),

                // 长度(单位米)
                new SqlParameter("@Lengths",tModel.Lengths),
                 
                // 宽度(单位米)
                new SqlParameter("@Widths",tModel.Widths),

                // 高度(单位米)
                new SqlParameter("@Heights",tModel.Heights),
                  
                // 体积(单位立方米)
                new SqlParameter("@Volume",tModel.Volume),
                 
                // 单价(单位 元/立方米)
                new SqlParameter("@Prices",tModel.Prices),
                 
                // 托体积系数
                new SqlParameter("@DragDatio",tModel.DragDatio),

                // 托体积
                new SqlParameter("@DragVolume",tModel.DragVolume),

                // 返空体积系数
                new SqlParameter("@ReturnDatio",tModel.ReturnDatio),

                // 返空体积
                new SqlParameter("@ReturnVolume",tModel.ReturnVolume),

                // 返空单价(单位 元/立方米)
                new SqlParameter("@ReturnPrices",tModel.ReturnPrices),

                // 供应商ID
                new SqlParameter("@SupplierId",tModel.SupplierId),

                // 状态
                new SqlParameter("@State",tModel.State),

                // 创建机构Id
                new SqlParameter("@CreateDepartmentId",tModel.CreateDepartmentId),

                // 创建用户id
                new SqlParameter("@CreateUserId",tModel.CreateUserId),

                // 系统公司id
                new SqlParameter("@CompanyId",tModel.CompanyId)
            };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);

            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }
        #endregion

        #region 分页列表 汽车物流-包装器具表

        /// <summary>
        /// 分页列表 汽车物流-包装器具表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns>
        public List<CarPackingModel> CarPackingList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                            SELECT
                               Row_Number() Over(Order By PackingId DESC) AS 't' 
                                ,PackingId
                            	,PackingName
                            	,PackingNumber
                            	,Lengths
                            	,Widths
                            	,Heights
                            	,Volume
                            	,Prices
                            	,DragDatio
                            	,DragVolume
                            	,ReturnDatio
                            	,ReturnVolume
                            	,ReturnPrices
                            	,SupplierNumber
                            	,SupplierName 
                            FROM CarPacking CP
                            LEFT JOIN CarSupplier CS ON CP.SupplierId = CS.SupplierId
                            WHERE " + tWhere + @" 
                           )
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index";

            SqlParameter[] param ={
                new SqlParameter("@Size",tSize),
                new SqlParameter("@Index",tIndex)
                                  };

            // 实例化数据集
            List<CarPackingModel> list = new List<CarPackingModel>();

            // 从数据库读取数据
            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                return null;
            }

            // 循环数据加入到数据集中
            while (dr.Read())
            {
                CarPackingModel model = new CarPackingModel();

                // 器具ID
                model.PackingId = Convert.ToInt32(dr["PackingId"].ToString());

                // 器具名称
                model.PackingName = dr["PackingName"].ToString();

                // 包装代码
                model.PackingNumber = dr["PackingNumber"].ToString();

                // 长度
                model.Lengths = Convert.ToInt32(dr["Lengths"].ToString());

                // 宽度
                model.Widths = Convert.ToInt32(dr["Widths"].ToString());

                // 高度
                model.Heights = Convert.ToDecimal(dr["Heights"].ToString());

                // 体积 
                model.Volume = Convert.ToInt32(dr["Volume"].ToString());

                // 单价 
                model.Prices = Convert.ToDecimal(dr["Prices"].ToString());

                // 拖体积系数 
                model.DragDatio = Convert.ToDecimal(dr["DragDatio"].ToString());

                // 拖体积
                model.DragVolume = Convert.ToDecimal(dr["DragVolume"].ToString());

                // 返空体积系数 
                model.ReturnDatio = Convert.ToDecimal(dr["ReturnDatio"].ToString());

                // 返空体积 
                model.ReturnVolume = Convert.ToDecimal(dr["ReturnVolume"].ToString());

                // 返空单价
                model.ReturnPrices = Convert.ToDecimal(dr["ReturnPrices"].ToString());

                // 供应商代码
                model.SupplierNumber = dr["SupplierNumber"].ToString();

                // 供应商名称 
                model.SupplierName = dr["SupplierName"].ToString();
                 
                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }
        #endregion

        #region 分页总数 汽车物流-包装器具表

        /// <summary>
        /// 分页总数 汽车物流-包装器具表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int CarPackingCount(string tWhere)
        {
            string sql = @"SELECT
                               Count(PackingId) 
                           FROM CarPacking CP
                           LEFT JOIN CarSupplier CS ON CP.SupplierId = CS.SupplierId
                           WHERE " + tWhere;

            object obj = new object();
            try
            {
                obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, null);

                if (obj != null)
                {
                    return Convert.ToInt32(obj.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        #region 变更状态 汽车物流-包装器具表
         
        /// <summary>
        /// 作废状态 汽车物流-包装器具表
        /// </summary>
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int InvalidState(int tId, int delUserId)
        {
            string sql = @"Update 
                              CarPacking
                           Set                     
                              State = 0
                              ,DelUserId = @DelUserId
                              ,DelTime = GETDATE()
                          WHERE
                              PackingId = @PackingId";

            SqlParameter[] param ={  
                // ID主键
                new SqlParameter("@PackingId",tId),
                
                // 作废人
                new SqlParameter("@DelUserId",delUserId)
            };

            // 影响行数
            int row = 0;
            try
            {
                row = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return row;
        }
         
        #endregion

        #region 编辑 汽车物流-包装器具表

        /// <summary>
        /// 编辑(修改得分) 汽车物流-包装器具表
        /// </summary>
        public int EditCarPacking(CarPackingModel tModel)
        {
            string sql = @" UPDATE 
                                CarPacking 
                            SET   
                                PackingName = @PackingName  
                                ,PackingNumber = @PackingNumber  
                                ,Lengths = @Lengths  
                                ,Widths = @Widths  
                                ,Heights = @Heights  
                                ,Volume = @Volume  
                                ,DragDatio = @DragDatio  
                                ,DragVolume = @DragVolume  
                                ,ReturnDatio = @ReturnDatio  
                                ,ReturnVolume = @ReturnVolume  
                                ,ReturnPrices = @ReturnPrices  
                          WHERE
                                PackingId = @PackingId 
                          ";

            SqlParameter[] param ={ 
                 
                // ID
                new SqlParameter("@PackingId",tModel.PackingId),
                 
                // 器具名称
                new SqlParameter("@PackingName",tModel.PackingName),
                 
                // 包装代码
                new SqlParameter("@PackingNumber",tModel.PackingNumber),
                 
                // 长度
                new SqlParameter("@Lengths",tModel.Lengths),
                 
                // 宽度
                new SqlParameter("@Widths",tModel.Widths),
                 
                // 高度
                new SqlParameter("@Heights",tModel.Heights),
                 
                // 体积
                new SqlParameter("@Volume",tModel.Volume),
                 
                // 托体积系数
                new SqlParameter("@DragDatio",tModel.DragDatio),
                 
                // 托体积
                new SqlParameter("@DragVolume",tModel.DragVolume),
                 
                // 返空体积系数
                new SqlParameter("@ReturnDatio",tModel.ReturnDatio),
                 
                // 返空体积
                new SqlParameter("@ReturnVolume",tModel.ReturnVolume),
                 
                // 返空单价
                new SqlParameter("@ReturnPrices",tModel.ReturnPrices)
            };

            // 影响行数
            int row = 0;
            try
            {
                row = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);

                return row;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region 实体Model 汽车物流-包装器具表

        /// <summary>
        /// 获取实体 汽车物流-包装器具表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public CarPackingModel GetModelByID(int tId)
        {
            string sql = @" SELECT 
                                PackingId
                                ,PackingName
                                ,PackingNumber
                                ,Lengths
                                ,Widths
                                ,Heights
                                ,Volume
                                ,Prices
                                ,DragDatio
                                ,DragVolume
                                ,ReturnDatio
                                ,ReturnVolume
                                ,ReturnPrices
                                ,SupplierId
                                ,State
                                ,CreateDepartmentId
                                ,CreateUserId
                                ,CreateTime
                                ,DelUserId
                                ,ISNULL(DelTime,'') DelTime
                                ,CompanyId
                            FROM CarPacking 
                            WHERE 
                            	PackingId = @PackingId";

            SqlParameter[] param ={
                                   new SqlParameter("@PackingId",tId)
                                  };

            CarPackingModel model = null;
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
                model = new CarPackingModel();
                 
                // 器具ID
                model.PackingId = Convert.ToInt32(dr["PackingId"].ToString());

                // 器具名称
                model.PackingName = dr["PackingName"].ToString();

                // 包装代码
                model.PackingNumber = dr["PackingNumber"].ToString();

                // 长度
                model.Lengths = Convert.ToInt32(dr["Lengths"].ToString());

                // 宽度
                model.Widths = Convert.ToInt32(dr["Widths"].ToString());

                // 高度
                model.Heights = Convert.ToDecimal(dr["Heights"].ToString());

                // 体积 
                model.Volume = Convert.ToInt32(dr["Volume"].ToString());

                // 单价 
                model.Prices = Convert.ToDecimal(dr["Prices"].ToString());

                // 拖体积系数 
                model.DragDatio = Convert.ToDecimal(dr["DragDatio"].ToString());

                // 拖体积
                model.DragVolume = Convert.ToDecimal(dr["DragVolume"].ToString());

                // 返空体积系数 
                model.ReturnDatio = Convert.ToDecimal(dr["ReturnDatio"].ToString());

                // 返空体积 
                model.ReturnVolume = Convert.ToDecimal(dr["ReturnVolume"].ToString());

                // 返空单价
                model.ReturnPrices = Convert.ToDecimal(dr["ReturnPrices"].ToString());

                // 供应商ID
                model.SupplierId = Convert.ToInt32(dr["SupplierId"].ToString());

                // 状态
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 创建机构id
                model.CreateDepartmentId = Convert.ToInt32(dr["CreateDepartmentId"].ToString());

                // 创建用户id
                model.CreateUserId = Convert.ToInt32(dr["CreateUserId"].ToString());

                // 创建时间
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());
                 
                // 删除用户Id
                model.DelUserId = Convert.ToInt32(dr["DelUserId"].ToString());

                // 删除时间
                model.DelTime = dr["DelTime"].ToString();

                // 系统公司id
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); 
            }

            // 关闭
            dr.Close();

            // 返回model
            return model;
        }

        #endregion

        #region 导出 汽车物流-包装器具表

        /// <summary>
        /// 导出 汽车物流-包装器具表
        /// </summary>
        /// <param name="tWhere">条件</param>
        /// <returns></returns>
        public List<CarPackingModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                            SELECT
                                Row_Number() Over(Order By PackingId DESC) AS 't' 
                                ,PackingId
                            	,PackingName
                            	,PackingNumber
                            	,Lengths
                            	,Widths
                            	,Heights
                            	,Volume
                            	,Prices
                            	,DragDatio
                            	,DragVolume
                            	,ReturnDatio
                            	,ReturnVolume
                            	,ReturnPrices
                            	,SupplierNumber
                            	,SupplierName 
                            FROM CarPacking CP
                            LEFT JOIN CarSupplier CS ON CP.SupplierId = CS.SupplierId
                            WHERE " + tWhere + @" 
                           )
                            Select * From TemTable";

            // 实例化数据集
            List<CarPackingModel> list = new List<CarPackingModel>();

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
                CarPackingModel model = new CarPackingModel();

                // 器具ID
                model.PackingId = Convert.ToInt32(dr["PackingId"].ToString());

                // 器具名称
                model.PackingName = dr["PackingName"].ToString();

                // 包装代码
                model.PackingNumber = dr["PackingNumber"].ToString();

                // 长度
                model.Lengths = Convert.ToInt32(dr["Lengths"].ToString());

                // 宽度
                model.Widths = Convert.ToInt32(dr["Widths"].ToString());

                // 高度
                model.Heights = Convert.ToDecimal(dr["Heights"].ToString());

                // 体积 
                model.Volume = Convert.ToInt32(dr["Volume"].ToString());

                // 单价 
                model.Prices = Convert.ToDecimal(dr["Prices"].ToString());

                // 拖体积系数 
                model.DragDatio = Convert.ToDecimal(dr["DragDatio"].ToString());

                // 拖体积
                model.DragVolume = Convert.ToDecimal(dr["DragVolume"].ToString());

                // 返空体积系数 
                model.ReturnDatio = Convert.ToDecimal(dr["ReturnDatio"].ToString());

                // 返空体积 
                model.ReturnVolume = Convert.ToDecimal(dr["ReturnVolume"].ToString());

                // 返空单价
                model.ReturnPrices = Convert.ToDecimal(dr["ReturnPrices"].ToString());

                // 供应商代码
                model.SupplierNumber = dr["SupplierNumber"].ToString();

                // 供应商名称 
                model.SupplierName = dr["SupplierName"].ToString();

                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }
        #endregion

        #region 获取运输供应商信息

        #region 分页列表 汽车物流-包装器具表

        /// <summary>
        /// 分页列表 汽车物流-包装器具表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns>
        public List<CarSupplierModel> CarSupplierList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                            SELECT
                               Row_Number() Over(Order By SupplierId DESC) AS 't' 
                                ,SupplierId
                            	,SupplierName
                            	,SupplierNumber 
                            FROM CarSupplier CS
                            WHERE " + tWhere + @" 
                           )
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index";

            SqlParameter[] param ={
                new SqlParameter("@Size",tSize),
                new SqlParameter("@Index",tIndex)
                                  };

            // 实例化数据集
            List<CarSupplierModel> list = new List<CarSupplierModel>();

            // 从数据库读取数据
            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                return null;
            }

            // 循环数据加入到数据集中
            while (dr.Read())
            {
                CarSupplierModel model = new CarSupplierModel();

                // 供应商ID
                model.SupplierId = Convert.ToInt32(dr["SupplierId"].ToString());

                // 供应商名称
                model.SupplierName = dr["SupplierName"].ToString();

                // 供应商代码
                model.SupplierNumber = dr["SupplierNumber"].ToString();
                 
                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }
        #endregion

        #region 分页总数 汽车物流-包装器具表

        /// <summary>
        /// 分页总数 汽车物流-包装器具表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int CarSupplierCount(string tWhere)
        {
            string sql = @"SELECT
                               Count(SupplierId) 
                           FROM CarSupplier CS 
                           WHERE " + tWhere;

            object obj = new object();
            try
            {
                obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, null);

                if (obj != null)
                {
                    return Convert.ToInt32(obj.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        #endregion
    }
}
