//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-08-07    1.0        FJK        新建
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using DBUtility;
using Model.Basis;
#endregion
/*********************************
 * 类名：BasisIntercalateDAL
 * 功能描述：部门考核设置表 数据访问层类
 * ******************************/

namespace DAL.Basis
{
    public class BasisIntercalateDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 部门考核设置表 

        /// <summary>
        /// 添加 部门考核设置表 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>int</returns>
        public int AddIntercalate(BasisIntercalateModel tModel)
        {
            string sql = @"INSERT INTO BasisIntercalate
                                 (
                                 DepartmentId
                                 ,Days
                                 ,CreateUserId
                                 ,CreateTime
                                 ,CompanyId
                                 ,State
                                 )
                           VALUES
                                 (
                                 @DepartmentId
                                 ,@Days
                                 ,@CreateUserId
                                 ,GETDATE() 
                                 ,@CompanyId
                                 ,@State 
                                 )";
            SqlParameter[] param ={
                // 系统部门id  
                new SqlParameter("@DepartmentId",tModel.DepartmentId),  

                // 考核最后日期 
                new SqlParameter("@Days",tModel.Days),   

                // 创建用户id
                new SqlParameter("@CreateUserId",tModel.CreateUserId),  
                 
                // 状态 
                new SqlParameter("@State",tModel.State),  

                // 系统公司id
                new SqlParameter("@CompanyId",tModel.CompanyId)
            };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }

        #endregion

        #region 分页列表 部门考核设置表 

        /// <summary>
        /// 分页列表 部门考核设置表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns>
        public List<BasisIntercalateModel> IntercalateList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                          (
                            Select 
                                Row_Number() Over(Order By IntercalateId Desc) AS 't'
                                ,IntercalateId
                                ,DepartmentName
                                ,Days
                                ,CreateTime
                                ,BI.State
                                ,CASE BI.State
                                    WHEN '0' THEN '无效'
                                    WHEN '1' THEN '有效'
                                END AS StateName
                            FROM BasisIntercalate BI
                            LEFT JOIN SysDepartment SD ON BI.DepartmentId = SD.DepartmentId
                            Where " + tWhere + @" 
                          ) 
                          Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",tSize),
                                   new SqlParameter("@Index",tIndex)
                                  };

            // 实例化数据集
            List<BasisIntercalateModel> list = new List<BasisIntercalateModel>();

            // 从数据库读取数据
            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            }
            catch (Exception)
            {
                return null;
            }

            // 循环数据加入到数据集中
            while (dr.Read())
            {
                BasisIntercalateModel model = new BasisIntercalateModel();

                // id自增主键
                model.IntercalateId = Convert.ToInt32(dr["IntercalateId"].ToString());

                // 部门名称
                model.DepartmentName = dr["DepartmentName"].ToString();

                // 考核最后日期
                model.Days = Convert.ToInt32(dr["Days"].ToString());

                // 创建时间
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 状态名称
                model.StateName = dr["StateName"].ToString();

                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }

        #endregion  

        #region 分页总数 部门考核设置表

        /// <summary>
        /// 分页总数 部门考核设置表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int IntercalateCount(string tWhere)
        {
            string sql = @"SELECT
                               Count(IntercalateId) 
                           FROM BasisIntercalate BI
                           LEFT JOIN SysDepartment SD ON BI.DepartmentId = SD.DepartmentId
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
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #region 获取实体 部门考核设置表

        /// <summary>
        /// 获取实体根据主键ID 部门考核设置表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisIntercalateModel GetModelByID(int tId)
        {
            string sql = @" SELECT 
                                IntercalateId
                                ,DepartmentId
                                ,Days
                                ,CreateUserId
                                ,CreateTime
                                ,CompanyId
                                ,State
                            FROM 
                                BasisIntercalate
                            WHERE
                                IntercalateId=@IntercalateId
                            ";

            SqlParameter[] param ={
                                   new SqlParameter("@IntercalateId",tId)
                                  };

            BasisIntercalateModel model = null;
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
                model = new BasisIntercalateModel();

                // ID自增主键
                model.IntercalateId = Convert.ToInt32(dr["IntercalateId"].ToString());

                // 系统部门ID
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString());

                // 考核最后日期
                model.Days = Convert.ToInt32(dr["Days"].ToString());

                // 创建人员ID
                model.CreateUserId = Convert.ToInt32(dr["CreateUserId"].ToString());

                // 创建时间
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 系统公司id
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
            }

            // 关闭
            dr.Close();

            // 返回model
            return model;
        }

        /// <summary>
        /// 获取实体根据部门ID 部门考核设置表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisIntercalateModel GetModelByDepartmentId(int tId)
        {
            string sql = @" SELECT 
                                IntercalateId
                                ,DepartmentId
                                ,Days
                                ,CreateUserId
                                ,CreateTime
                                ,CompanyId
                                ,State
                            FROM 
                                BasisIntercalate
                            WHERE
                                DepartmentId=@DepartmentId 
                            AND
                                State =1 
                            ";

            SqlParameter[] param ={
                // 部门ID
                new SqlParameter("@DepartmentId",tId)
                                  };

            BasisIntercalateModel model = null;
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
                model = new BasisIntercalateModel();

                // ID自增主键
                model.IntercalateId = Convert.ToInt32(dr["IntercalateId"].ToString());

                // 系统部门ID
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString());

                // 考核最后日期
                model.Days = Convert.ToInt32(dr["Days"].ToString());

                // 创建人员ID
                model.CreateUserId = Convert.ToInt32(dr["CreateUserId"].ToString());

                // 创建时间
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 系统公司id
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
            }

            // 关闭
            dr.Close();

            // 返回model
            return model;
        }

        #endregion  

        #region 编辑 部门考核设置表

        /// <summary>
        /// 编辑 部门考核设置表
        /// </summary>
        public int EditIntercalate(BasisIntercalateModel tModel)
        {
            string sql = @" UPDATE 
                                BasisIntercalate
                            SET Days = @Days 
                                ,State = @State
                            WHERE         
                                IntercalateId = @IntercalateId 
                          ";

            SqlParameter[] param ={
                // ID
                new SqlParameter("@IntercalateId",tModel.IntercalateId),
                 
                // 考核最后日期
               new SqlParameter("@Days",tModel.Days),  
                
                // 状态
                new SqlParameter("@State",tModel.State)
            };

            // 影响行数
            int row = 0;
            try
            {
                row = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);

                return row;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #region 导出 部门考核设置表

        /// <summary>
        /// 导出 部门考核设置表
        /// </summary>
        /// <param name="tWhere"></param>
        /// <returns></returns>
        public List<BasisIntercalateModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                          (
                            Select 
                                Row_Number() Over(Order By IntercalateId Desc) AS 't'
                                ,IntercalateId
                                ,DepartmentName
                                ,Days
                                ,CreateTime
                                ,BI.State
                                ,CASE BI.State
                                    WHEN '0' THEN '无效'
                                    WHEN '1' THEN '有效'
                                END AS StateName
                            FROM BasisIntercalate BI
                            LEFT JOIN SysDepartment SD ON BI.DepartmentId = SD.DepartmentId
                            Where " + tWhere + @" 
                          ) 
                          Select * From TemTable";

            // 实例化数据集
            List<BasisIntercalateModel> list = new List<BasisIntercalateModel>();

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
                BasisIntercalateModel model = new BasisIntercalateModel();

                // id自增主键
                model.IntercalateId = Convert.ToInt32(dr["IntercalateId"].ToString());

                // 部门名称
                model.DepartmentName = dr["DepartmentName"].ToString();

                // 考核最后日期
                model.Days = Convert.ToInt32(dr["Days"].ToString());

                // 创建时间
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 状态名称
                model.StateName = dr["StateName"].ToString();

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
