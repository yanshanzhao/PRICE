//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-08-16    1.0        FJK        新建
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
using Model.Sys;
#endregion
/*********************************
 * 类名：BasisLineDAL
 * 功能描述：线路维护表 数据访问层类
 * ******************************/

namespace DAL.Basis
{
    public class BasisLineDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 新增 线路维护表 

        /// <summary>
        /// 新增 线路维护表 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>int</returns>
        public int AddLine(BasisLineModel tModel)
        {
            string sql = @"INSERT INTO BasisLine
                           (
                                LineName
                                ,BeginId
                                ,EndId
                                ,State
                                ,UseState
                                ,CreateDepartmentId
                                ,CreateUserId
                                ,CreateTime 
                                ,CompanyId
		                   )
                           VALUES
                           (
                                @LineName
                                ,@BeginId
                                ,@EndId
                                ,@State
                                ,@UseState
                                ,@CreateDepartmentId
                                ,@CreateUserId 
                                ,GETDATE() 
                                ,@CompanyId 
                           );select @@identity";
            SqlParameter[] param ={
                 // 线路名称
                new SqlParameter("@LineName",tModel.LineName),
                
                 // 起始位置id
                new SqlParameter("@BeginId",tModel.BeginId),

                 // 结束位置id
                new SqlParameter("@EndId",tModel.EndId),

                 // 状态
                new SqlParameter("@State",tModel.State),
               
                 // 使用状态 
                new SqlParameter("@UseState",tModel.UseState),

                 // 创建机构id
                new SqlParameter("@CreateDepartmentId",tModel.CreateDepartmentId),

                 // 创建账号id
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

        #region 分页列表 线路维护表 

        /// <summary>
        /// 分页列表 线路维护表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns>
        public List<BasisLineModel> LineList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                           SELECT
                               Row_Number() Over(Order By LineId ASC) AS 't'
                               ,LineId
                               ,LineName
                               ,BeginId 
                               ,SA1.AreaName BeginName
                               ,EndId
                               ,SA2.AreaName EndName
                               ,BL.CreateTime
                               ,BL.State
                               ,CASE BL.State
                                   WHEN '0' THEN '无效'
                                   WHEN '1' THEN '有效'  
                               END AS StateName  
                               ,UseState
                               ,CASE UseState
                                   WHEN '0' THEN '未使用'
                                   WHEN '1' THEN '已使用'  
                               END AS UseStateName  
                           FROM BasisLine BL
                           LEFT JOIN VAreas SA1 ON BL.BeginId =SA1.AreaId
                           LEFT JOIN VAreas SA2 ON BL.EndId =SA2.AreaId
                           WHERE" + tWhere + @" 
                           )
                           Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",tSize),
                                   new SqlParameter("@Index",tIndex)
                                  };

            // 实例化数据集
            List<BasisLineModel> list = new List<BasisLineModel>();

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
                BasisLineModel model = new BasisLineModel();

                // id自增主键
                model.LineId = Convert.ToInt32(dr["LineId"].ToString());

                // 线路名称
                model.LineName = dr["LineName"].ToString();

                // 起始位置
                model.BeginId = Convert.ToInt32(dr["BeginId"].ToString());

                // 起始位置名称
                model.BeginName = dr["BeginName"].ToString();

                // 结束位置
                model.EndId = Convert.ToInt32(dr["EndId"].ToString());

                // 结束位置名称
                model.EndName = dr["EndName"].ToString();

                // 创建时间 
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 状态名称
                model.StateName = dr["StateName"].ToString();

                // 使用状态
                model.UseState = Convert.ToInt32(dr["UseState"].ToString());

                // 使用状态名称
                model.UseStateName = dr["UseStateName"].ToString();

                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }

        #endregion  

        #region 分页总数 线路维护表

        /// <summary>
        /// 分页总数 线路维护表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int LineCount(string tWhere)
        {
            string sql = @"SELECT
                               Count(LineId) 
                           FROM BasisLine BL
                           LEFT JOIN SysAreas SA1 ON BL.BeginId =SA1.AreaId
                           LEFT JOIN SysAreas SA2 ON BL.EndId =SA2.AreaId
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

        #region 获取实体 线路维护表

        /// <summary>
        /// 获取实体 线路维护表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisLineModel GetModelByID(int tId)
        {
            string sql = @" SELECT 
                                LineId
                                ,LineName
                                ,BeginId
                                ,EndId
                                ,State
                                ,UseState
                                ,CreateDepartmentId
                                ,CreateUserId
                                ,CreateTime
                                ,DelUserId
                                ,ISNULL(DelTime,'') DelTime
                                ,CompanyId
                            FROM 
                                BasisLine 
                            WHERE 
                            	LineId = @LineId";
            SqlParameter[] param ={
                                   new SqlParameter("@LineId",tId)
                                  };

            BasisLineModel model = null;
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
                model = new BasisLineModel();

                // ID主键
                model.LineId = Convert.ToInt32(dr["LineId"].ToString());

                // 线路名称
                model.LineName = dr["LineName"].ToString();

                // 开始位置id
                model.BeginId = Convert.ToInt32(dr["BeginId"].ToString());

                // 结束位置id
                model.EndId = Convert.ToInt32(dr["EndId"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 使用状态 
                model.UseState = Convert.ToInt32(dr["UseState"].ToString());

                // 创建机构id
                model.CreateDepartmentId = Convert.ToInt32(dr["CreateDepartmentId"].ToString());

                // 创建账号id
                model.CreateUserId = Convert.ToInt32(dr["CreateUserId"].ToString());

                // 创建时间
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 作废账号id
                model.DelUserId = Convert.ToInt32(dr["DelUserId"].ToString());

                // 作废时间
                model.DelTime = Convert.ToDateTime(dr["DelTime"].ToString());

                // 系统公司id
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
            }

            // 关闭
            dr.Close();

            // 返回model
            return model;
        }

        #endregion  

        #region 编辑 线路维护表

        /// <summary>
        /// 编辑 线路维护表
        /// </summary>
        public int EditLine(BasisLineModel tModel)
        {
            string sql = @" UPDATE 
                                BasisLine 
                            SET
                                BeginId = @BeginId
                                ,EndId = @EndId 
                                ,LineName = @LineName 
                          WHERE
                                LineId = @LineId 
                          ";

            SqlParameter[] param ={  
                // ID主键
                new SqlParameter("@LineId",tModel.LineId),
                 
                // 起始位置id
                new SqlParameter("@BeginId",tModel.BeginId),
                 
                // 结束位置id
                new SqlParameter("@EndId",tModel.EndId),
                 
                // 线路名称
                new SqlParameter("@LineName",tModel.LineName)
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

        #region 变更状态 线路维护表

        /// <summary>
        /// 启用 线路维护表
        /// </summary>
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int EnableState(int tLineId)
        {
            string sql = @"Update 
                              BasisLine
                           Set                     
                              State = 1         
                              ,DelUserId = 0
                              ,DelTime =@DelTime
                          WHERE
                              LineId = @LineId";

            SqlParameter[] param ={ 
            
                // ID主键
                new SqlParameter("@LineId",tLineId), 
                // 作废时间
                new SqlParameter("@DelTime",DBNull.Value)
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

        /// <summary>
        /// 停用 线路维护表
        /// </summary>
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int DiscontinuationState(int tLineId, int delUserId)
        {
            string sql = @"Update 
                              BasisLine
                           Set                     
                              State = 0
                              ,DelTime = GETDATE()
                              ,DelUserId = @DelUserId
                          WHERE
                              LineId = @LineId";

            SqlParameter[] param ={ 
            
                // ID主键
                new SqlParameter("@LineId",tLineId),

                // 作废人ID
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

        /// <summary>
        /// 变更使用状态 线路维护表
        /// </summary>
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int ChangeUseState(int tLineId)
        {
            string sql = @"Update 
                              BasisLine
                           Set                     
                              UseState = 1         
                          WHERE
                              LineId = @LineId";

            SqlParameter[] param ={ 
            
                // ID主键
                new SqlParameter("@LineId",tLineId)
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

        #region 导出 线路维护表

        /// <summary>
        /// 导出 线路维护表
        /// </summary>
        /// <param name="tWhere"></param>
        /// <returns></returns>
        public List<BasisLineModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                           SELECT
                               Row_Number() Over(Order By LineId DESC) AS 't'
                               ,LineId
                               ,LineName
                               ,BeginId 
                               ,SA1.AreaName BeginName
                               ,EndId
                               ,SA2.AreaName EndName
                               ,BL.CreateTime
                               ,BL.State
                               ,CASE BL.State
                                   WHEN '0' THEN '无效'
                                   WHEN '1' THEN '有效'  
                               END AS StateName  
                               ,UseState
                               ,CASE UseState
                                   WHEN '0' THEN '未使用'
                                   WHEN '1' THEN '已使用'  
                               END AS UseStateName  
                               FROM BasisLine BL
                               LEFT JOIN SysAreas SA1 ON BL.BeginId =SA1.AreaId
                               LEFT JOIN SysAreas SA2 ON BL.EndId =SA2.AreaId
                           WHERE" + tWhere + @" 
                           )
                           Select * From TemTable";

            // 实例化数据集
            List<BasisLineModel> list = new List<BasisLineModel>();

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
                BasisLineModel model = new BasisLineModel();

                // id自增主键
                model.LineId = Convert.ToInt32(dr["LineId"].ToString());

                // 线路名称
                model.LineName = dr["LineName"].ToString();

                // 起始位置
                model.BeginId = Convert.ToInt32(dr["BeginId"].ToString());

                // 起始位置名称
                model.BeginName = dr["BeginName"].ToString();

                // 结束位置
                model.EndId = Convert.ToInt32(dr["EndId"].ToString());

                // 结束位置名称
                model.EndName = dr["EndName"].ToString();

                // 创建时间 
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 状态名称
                model.StateName = dr["StateName"].ToString();

                // 使用状态
                model.UseState = Convert.ToInt32(dr["UseState"].ToString());

                // 使用状态名称
                model.UseStateName = dr["UseStateName"].ToString();

                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }
        #endregion

        #region 区域基础表

        #region 分页列表 区域基础表 

        /// <summary>
        /// 分页列表 线路维护表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns>
        public List<SysAreasModel> AreasList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                           SELECT
                               Row_Number() Over(Order By AreaId ASC) AS 't'
                               ,AreaId
                               ,AreaName 
                           FROM VAreas  
                           WHERE" + tWhere + @" 
                           )
                           Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",tSize),
                                   new SqlParameter("@Index",tIndex)
                                  };

            // 实例化数据集
            List<SysAreasModel> list = new List<SysAreasModel>();

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
                SysAreasModel model = new SysAreasModel();

                // id自增主键
                model.AreaId = Convert.ToInt32(dr["AreaId"].ToString());

                // 位置名称
                model.AreaName = dr["AreaName"].ToString();

                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }

        #endregion  

        #region 分页总数 区域基础表

        /// <summary>
        /// 分页总数 区域基础表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int AreasCount(string tWhere)
        {
            string sql = @"SELECT
                               Count(AreaId) 
                           FROM VAreas
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

        #region 获取实体 线路维护表

        /// <summary>
        /// 获取实体 线路维护表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public SysAreasModel GetModelByAreaId(int tId)
        {
            string sql = @" SELECT 
                                AreaId
                                ,AreaName
                                ,ParentId
                                ,Sort
                                ,AreaCode
                                ,State
                                ,CreateTime
                                ,area01
                                ,areaname01
                                ,area02
                                ,areaname02
                                ,area03
                                ,areaname03
                                ,area04
                                ,areaname04
                                ,area05
                                ,areaname05
                                ,px
                            FROM 
                                VAreas 
                            WHERE 
                            	AreaId = @AreaId";
            SqlParameter[] param ={
                                   new SqlParameter("@AreaId",tId)
                                  };

            SysAreasModel model = null;
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
                model = new SysAreasModel();

                // ID主键
                model.AreaId = Convert.ToInt32(dr["AreaId"].ToString());

                // 线路名称
                model.AreaName = dr["AreaName"].ToString();
            }

            // 关闭
            dr.Close();

            // 返回model
            return model;
        }

        #endregion  

        #endregion
    }
}
