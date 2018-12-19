//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , PRICE
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-12-13    1.0         YSZ       新建 
//-------------------------------------------------------------------------
using DBUtility;
using Model.Car;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
/*********************************
 * 类名：CarSupplierDAL
 * 功能描述：汽车物流-供应商 数据访问层
 * ******************************/
namespace DAL.Car
{
    public class CarSupplierDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();
        #region 新增供应商表
        /// <summary>
        /// 新增供应商 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>int</returns>
        public int AddSupplier(CarSupplierModel tModel)
        {
            string sql = @"INSERT INTO dbo.CarSupplier
                           (
                                SupplierName
                                ,SupplierNumber
                                ,SenderName
                                ,SenderPhone
                                ,SenderProvince
                                ,SenderCity
                                ,SenderCounty
                                ,SenderTwon 
                                ,SenderAddress
                                ,State
                                ,CreateDepartmentId
                                ,CreateUserId
                                ,CreateTime
                                ,CompanyId
		                   )
                           VALUES
                           (
                                @SupplierName
                                ,@SupplierNumber
                                ,@SenderName
                                ,@SenderPhone
                                ,@SenderProvince
                                ,@SenderCity
                                ,@SenderCounty 
                                ,@SenderTwon
                                ,@SenderAddress
                                , 1
                                ,@CreateDepartmentId
                                ,@CreateUserId
                                ,GETDATE()
                                ,@CompanyId
                           );select @@identity";
            SqlParameter[] param ={
                 // 供应商名称
                new SqlParameter("@SupplierName",tModel.SupplierName),
                 // 供应商代码
                new SqlParameter("@SupplierNumber",tModel.SupplierNumber),
                 // 发货联系人
                new SqlParameter("@SenderName",tModel.SenderName??string.Empty),
                 // 发货联系电话
                new SqlParameter("@SenderPhone",tModel.SenderPhone??string.Empty),
                 // 发货人所属省份 
                new SqlParameter("@SenderProvince",tModel.SenderProvince??string.Empty),
                 // 发货人所属市
                new SqlParameter("@SenderCity",tModel.SenderCity??string.Empty),
                 // 发货人所属县
                new SqlParameter("@SenderCounty",tModel.SenderCounty??string.Empty), 
                 // 发货人所属镇
                new SqlParameter("@SenderTwon",tModel.SenderTwon??string.Empty),
                 // 发货人详细地址
                new SqlParameter("@SenderAddress",tModel.SenderAddress??string.Empty),
                 // 创建机构Id
                new SqlParameter("@CreateDepartmentId",tModel.CreateDepartmentId),
                 // 创建用户id
                new SqlParameter("@CreateUserId",tModel.CreateUserId),
                 // 系统公司id
                new SqlParameter("@CompanyId",tModel.CompanyId)
            };

            object obj = null;

            try
            {
                obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("唯一索引"))
                {
                    return -1;
                }
                throw;
            }

            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }

        #endregion

        #region 分页列表 供应商表 

        /// <summary>
        /// 分页列表 供应商表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns>
        public List<CarSupplierModel> SupplierList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                           SELECT
                               Row_Number() Over(Order By SupplierId ASC) AS 't'
                               ,SupplierId
                               ,SupplierName
                               ,SupplierNumber 
                               ,SenderName
                               ,SenderPhone
                               ,SenderProvince
                               ,SenderCity
                               ,SenderCounty
                               ,SenderTwon
                               ,SenderAddress
                               ,State
                               ,CASE State
                                   WHEN '0' THEN '删除'
                                   WHEN '1' THEN '有效'  
                               END AS StateName  
                               ,CreateDepartmentId
                               ,CreateUserId
                               ,CreateTime
                               ,DelUserId
                               ,DelTime
                               ,CompanyId  
                           FROM dbo.CarSupplier
                           WHERE" + tWhere + @" 
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

                // id自增主键
                model.SupplierId = Convert.ToInt32(dr["SupplierId"]);

                // 供应商名称
                model.SupplierName = dr["SupplierName"].ToString();

                // 供应商代码
                model.SupplierNumber = dr["SupplierNumber"].ToString();

                // 发货联系人
                model.SenderName = dr["SenderName"].ToString();

                // 发货联系电话
                model.SenderPhone = dr["SenderPhone"].ToString();

                // 发货人所属省份
                model.SenderProvince = dr["SenderProvince"].ToString();

                // 发货人所属市
                model.SenderCity = dr["SenderCity"].ToString();

                // 发货人所属县
                model.SenderCounty = dr["SenderCounty"].ToString();

                // 发货人所属镇
                model.SenderTwon = dr["SenderTwon"].ToString();

                // 发货人详细地址
                model.SenderAddress = dr["SenderAddress"].ToString();

                // 创建时间 
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 创建机构Id
                model.CreateDepartmentId = Convert.ToInt32(dr["CreateDepartmentId"]);

                // 创建用户id
                model.CreateUserId = Convert.ToInt32(dr["CreateUserId"]);

                model.xh = Convert.ToInt32(dr["t"]);
                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }

        #endregion  

        #region 分页总数 供应商表

        /// <summary>
        /// 分页总数 供应商表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int SupplierCount(string tWhere)
        {
            string sql = @"SELECT
                               Count(SupplierId) 
                           FROM dbo.CarSupplier
                           WHERE" + tWhere;

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

        #region 编辑 供应商

        /// <summary>
        /// 编辑 供应商
        /// </summary>
        public int EditSupplier(CarSupplierModel tModel)
        {
            string sql = @" UPDATE 
                                dbo.CarSupplier 
                            SET
                                SupplierName = @SupplierName
                                ,SupplierNumber = @SupplierNumber 
                                ,SenderName = @SenderName 
                                ,SenderPhone = @SenderPhone 
                                ,SenderProvince = @SenderProvince 
                                ,SenderCity = @SenderCity 
                                ,SenderCounty = @SenderCounty 
                                ,SenderTwon = @SenderTwon 
                                ,SenderAddress = @SenderAddress 
                          WHERE
                                SupplierId = @SupplierId 
                          ";

            SqlParameter[] param ={  
                // ID主键
                new SqlParameter("@SupplierId",tModel.SupplierId),
                 
                // 供应商名称
                new SqlParameter("@SupplierName",tModel.SupplierName),
                 
                // 供应商代码
                new SqlParameter("@SupplierNumber",tModel.SupplierNumber),
                 
                // 发货联系人
                new SqlParameter("@SenderName",tModel.SenderName??string.Empty),

                // 发货联系电话
                new SqlParameter("@SenderPhone",tModel.SenderPhone??string.Empty),
                 
                // 发货人所属省份
                new SqlParameter("@SenderProvince",tModel.SenderProvince??string.Empty),
                 
                // 发货人所属市
                new SqlParameter("@SenderCity",tModel.SenderCity??string.Empty),
                 
                // 发货人所属县
                new SqlParameter("@SenderCounty",tModel.SenderCounty??string.Empty),

                // 发货人所属镇
                new SqlParameter("@SenderTwon",tModel.SenderTwon??string.Empty),
                 
                // 发货人详细地址
                new SqlParameter("@SenderAddress",tModel.SenderAddress??string.Empty)
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
                if (ex.ToString().Contains("唯一索引"))
                {
                    return -1;
                }
                throw;
            }
        }

        #endregion

        #region 获取实体 供应商

        /// <summary>
        /// 获取实体 供应商
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public CarSupplierModel GetModelByID(int tId)
        {
            string sql = @" SELECT 
                                SupplierId
                               ,SupplierName
                               ,SupplierNumber 
                               ,SenderName
                               ,SenderPhone
                               ,SenderProvince
                               ,SenderCity
                               ,SenderCounty
                               ,SenderTwon
                               ,SenderAddress
                               ,State
                               ,CreateDepartmentId
                               ,CreateUserId
                               ,CreateTime
                               ,DelUserId
                               ,ISNULL(DelTime,'') DelTime
                               ,CompanyId  
                            FROM 
                                dbo.CarSupplier 
                            WHERE 
                            	SupplierId = @SupplierId";
            SqlParameter[] param ={
                                   new SqlParameter("@SupplierId",tId)
                                  };

            CarSupplierModel model = null;
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
                model = new CarSupplierModel();
                // id自增主键
                model.SupplierId = Convert.ToInt32(dr["SupplierId"]);

                // 供应商名称
                model.SupplierName = dr["SupplierName"].ToString();

                // 供应商代码
                model.SupplierNumber = dr["SupplierNumber"].ToString();

                // 发货联系人
                model.SenderName = dr["SenderName"].ToString();

                // 发货联系电话
                model.SenderPhone = dr["SenderPhone"].ToString();

                // 发货人所属省份
                model.SenderProvince = dr["SenderProvince"].ToString();

                // 发货人所属市
                model.SenderCity = dr["SenderCity"].ToString();

                // 发货人所属县
                model.SenderCounty = dr["SenderCounty"].ToString();

                // 发货人所属镇
                model.SenderTwon = dr["SenderTwon"].ToString();

                // 发货人详细地址
                model.SenderAddress = dr["SenderAddress"].ToString();

                // 创建时间 
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 创建机构Id
                model.CreateDepartmentId = Convert.ToInt32(dr["CreateDepartmentId"]);

                // 创建用户id
                model.CreateUserId = Convert.ToInt32(dr["CreateUserId"]);

                // 删除时间
                model.DelTime = Convert.ToDateTime(dr["DelTime"]);

                // 删除用户Id
                model.DelUserId = Convert.ToInt32(dr["DelUserId"]);

                // 系统公司id
                model.CompanyId = Convert.ToInt32(dr["CompanyId"]);
            }

            // 关闭
            dr.Close();

            // 返回model
            return model;
        }

        #endregion  

        #region 作废供应商
        /// <summary>
        /// 作废供应商
        /// </summary>
        /// <param name="tSupplierId">供应商ID</param>
        /// <param name="tDelUserId">删除用户Id</param>
        /// <returns></returns>
        public int InvalidState(int tSupplierId, int tDelUserId)
        {
            string sql = string.Format(@"Update 
                              dbo.CarSupplier
                           Set                     
                              State = 0         
                              ,DelUserId = {0}
                              ,DelTime =GETDATE()
                          WHERE
                              SupplierId = {1};
                          UPDATE dbo.CarPart 
                          SET 
                              State=0
                              ,DelUserId={0}
                              ,DelTime=GETDATE() 
                          WHERE 
                               SupplierId={0};
                          UPDATE dbo.CarPacking 
                          SET 
                               State=0
                               ,DelUserId={0}
                               ,DelTime=GETDATE() 
                          WHERE 
                                SupplierId={1}", "@DelUserId", "@SupplierId");

            SqlParameter[] param ={ 
            
                // ID主键
                new SqlParameter("@SupplierId",tSupplierId), 

                //删除用户Id
                new SqlParameter("@DelUserId",tDelUserId),

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
        #endregion 作废供应商

        #region 导出供应商

        /// <summary>
        /// 导出供应商
        /// </summary>
        /// <param name="tWhere"></param>
        /// <returns></returns>
        public List<CarSupplierModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                           SELECT
                               Row_Number() Over(Order By SupplierId ASC) AS 't'
                               ,SupplierId
                               ,SupplierName
                               ,SupplierNumber 
                               ,SenderName
                               ,SenderPhone
                               ,SenderProvince
                               ,SenderCity
                               ,SenderCounty
                               ,SenderTwon
                               ,SenderAddress
                               ,State
                               ,CASE State
                                   WHEN '0' THEN '删除'
                                   WHEN '1' THEN '有效'  
                               END AS StateName  
                               ,CreateTime
                           FROM dbo.CarSupplier
                           WHERE" + tWhere + @" 
                           )
                           Select * From TemTable";

            // 实例化数据集
            List<CarSupplierModel> list = new List<CarSupplierModel>();

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
                CarSupplierModel model = new CarSupplierModel();

                // id自增主键
                model.SupplierId = Convert.ToInt32(dr["SupplierId"]);

                // 供应商名称
                model.SupplierName = dr["SupplierName"].ToString();

                // 供应商代码
                model.SupplierNumber = dr["SupplierNumber"].ToString();

                // 发货联系人
                model.SenderName = dr["SenderName"].ToString();

                // 发货联系电话
                model.SenderPhone = dr["SenderPhone"].ToString();

                // 发货人所属省份
                model.SenderProvince = dr["SenderProvince"].ToString();

                // 发货人所属市
                model.SenderCity = dr["SenderCity"].ToString();

                // 发货人所属县
                model.SenderCounty = dr["SenderCounty"].ToString();

                // 发货人所属镇
                model.SenderTwon = dr["SenderTwon"].ToString();

                // 发货人详细地址
                model.SenderAddress = dr["SenderAddress"].ToString();

                // 创建时间 
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                //状态名称
                model.States = dr["StateName"].ToString();

                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }
        #endregion
    }
}
