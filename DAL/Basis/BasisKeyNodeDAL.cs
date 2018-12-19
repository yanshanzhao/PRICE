//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-05-30    1.0        FJK        新建
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
 * 类名：BasisKeyNodeDAL
 * 功能描述：关键节点表 数据访问层类
 * ******************************/

namespace DAL.Basis
{
    public class BasisKeyNodeDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 关键节点表 

        /// <summary>
        /// 添加 关键节点表 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>int</returns>
        public int AddKeyNode(BasisKeyNodeModel tModel)
        {
            string sql = @"INSERT INTO BasisKeyNode
                                 (
                                 Name
                                 ,Columns
                                 ,NodeName
                                 ,MinValue
                                 ,BeginTime
                                 ,EndTime
                                 ,State
                                 ,CompanyId
                                 ,UpdateTime
                                 )
                           VALUES
                                 (
                                 @Name
                                 ,@Columns
                                 ,@NodeName
                                 ,@MinValue
                                 ,@BeginTime
                                 ,@EndTime
                                 ,@State
                                 ,@CompanyId
                                 ,GETDATE() 
                                 )";
            SqlParameter[] param ={
                // 关键节点编号 
                //new SqlParameter("@Name",model.Name??string.Empty),
                new SqlParameter("@Name",tModel.Name),  

                // 关键节点字段 
                new SqlParameter("@Columns",tModel.Columns),   

                // 关键节点名称
                new SqlParameter("@NodeName",tModel.NodeName),  

                // 关键节点最小值(含)
                new SqlParameter("@MinValue",tModel.MinValue),

                // 使用开始时间 
                new SqlParameter("@BeginTime",tModel.BeginTime),   
                
                // 使用结束时间
                new SqlParameter("@EndTime",tModel.EndTime),  

                // 状态 
                new SqlParameter("@State",tModel.State),  

                // 系统公司id
                new SqlParameter("@CompanyId",tModel.CompanyId)
            };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }

        #endregion

        #region 分页列表 关键节点表 

        /// <summary>
        /// 分页列表 关键节点表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns>
        public List<BasisKeyNodeModel> BasisKeyNodeList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                        (
                            Select 
                                Row_Number() Over(Order By bkn.UpdateTime Desc) AS 't'
                                ,bkn.Id
                                ,bkn.Name
                                ,bkn.Columns
                                ,bkn.NodeName
                                ,bkn.MinValue
                                ,bkn.BeginTime
                                ,bkn.EndTime 
                                ,bkn.UpdateTime 
                                ,bkn.State
                                ,CASE bkn.State
                                    WHEN '0' THEN '初始'
                                    WHEN '1' THEN '使用'
                                ELSE '作废' END AS StateName
                                ,bkn.CompanyId
                                ,ISNULL(com.CompanyName,'无') As CompanyName
                            From
                                BasisKeyNode bkn Left Join SysCompany com
                            On
                                bkn.CompanyId=com.CompanyId  Where " + tWhere + @" 
                        )
                              
                        Select * From TemTable Where t  Between @Size*(@Index-1)+1 And @Size*@Index";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",tSize),
                                   new SqlParameter("@Index",tIndex)
                                  };

            // 实例化数据集
            List<BasisKeyNodeModel> list = new List<BasisKeyNodeModel>();

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
                BasisKeyNodeModel model = new BasisKeyNodeModel();

                // id自增主键
                model.Id = Convert.ToInt32(dr["Id"].ToString());

                // 关键节点编号
                model.Name = dr["Name"].ToString();

                // 关键节点字段
                model.Columns = dr["Columns"].ToString();

                // 关键节点名称
                model.NodeName = dr["NodeName"].ToString();

                // 关键节点最小值(含)
                model.MinValue = Convert.ToDecimal(dr["MinValue"].ToString());

                // 使用开始时间
                model.BeginTime = Convert.ToDateTime(dr["BeginTime"].ToString());

                // 使用结束时间
                model.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());

                // 系统公司 
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
                model.CompanyName = dr["CompanyName"].ToString();

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());
                model.StateName = dr["StateName"].ToString();

                // 更改时间
                model.UpdateTime = Convert.ToDateTime(dr["UpdateTime"].ToString());

                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }

        #endregion  

        #region 分页总数 关键节点表

        /// <summary>
        /// 分页总数 关键节点表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int KeyNodeCount(string tNodeName, string tCompanyName)
        {
            string sql = @"SELECT
                               Count(Id) 
                           FROM
                               BasisKeyNode
                           WHERE 
                               CompanyId IN ( SELECT CompanyId FROM SysCompany WHERE " + tCompanyName + @")
                           AND 
                               "+ tNodeName + @"";

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

        #region 获取实体 关键节点表

        /// <summary>
        /// 获取实体 关键节点表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisKeyNodeModel GetModelByID(int tId)
        {
            string sql = @" SELECT bkn.Id
                                ,bkn.Name
                                ,bkn.Columns
                                ,bkn.NodeName
                                ,bkn.MinValue
                                ,bkn.BeginTime
                                ,bkn.EndTime
                                ,bkn.State
                                ,bkn.CompanyId
                                ,ISNULL(com.CompanyName,'无') As CompanyName
                                ,bkn.UpdateTime
                            FROM 
                                BasisKeyNode bkn
                            LEFT JOIN 
                                SysCompany com
                            ON  
                                bkn.CompanyId=com.CompanyId
                            WHERE
                                bkn.Id=@Id";
            SqlParameter[] param ={
                                   new SqlParameter("@Id",tId)
                                  };

            BasisKeyNodeModel model = null;
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
                model = new BasisKeyNodeModel();

                // ID
                model.Id = Convert.ToInt32(dr["Id"].ToString());

                // 关键节点编号:使用表名称
                model.Name = dr["Name"].ToString();

                // 关键节点字段:字段名称
                model.Columns = dr["Columns"].ToString();

                // 关键节点名称
                model.NodeName = dr["NodeName"].ToString();

                // 关键节点最小值(含)
                model.MinValue = Convert.ToDecimal(dr["MinValue"].ToString());

                // 使用开始时间
                model.BeginTime = Convert.ToDateTime(dr["BeginTime"].ToString());

                // 使用结束时间
                model.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());

                // 状态：0-初始;1-使用;2-作废 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 系统公司id
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());

                // 公司名称
                model.CompanyName = dr["CompanyName"].ToString();

                // 更改时间
                model.UpdateTime = Convert.ToDateTime(dr["UpdateTime"].ToString());
            }

            // 关闭
            dr.Close();

            // 返回model
            return model;
        }

        /// <summary>
        /// 获取实体 关键节点表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisKeyNodeModel GetModelByName(string Name, string Columns, int CompanyId)
        {
            string sql = @" SELECT 
                                TOP 1
                                Id
                                ,Name
                                ,Columns
                                ,NodeName
                                ,MinValue
                                ,BeginTime
                                ,EndTime
                                ,State
                                ,CompanyId 
                                ,UpdateTime
                            FROM 
                                BasisKeyNode  
                            WHERE
                                Name=@Name
                            AND
                                Columns=@Columns
                            AND
                                CompanyId=@CompanyId
                            AND
                                State=1
                            AND	
								GETDATE() BETWEEN BeginTime AND EndTime
                            ORDER BY Id DESC 
                            ";
            SqlParameter[] param ={
                                   new SqlParameter("@Name",Name),
                                   new SqlParameter("@Columns",Columns),
                                   new SqlParameter("@CompanyId",CompanyId)
                                  };

            BasisKeyNodeModel model = null;
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
                model = new BasisKeyNodeModel();

                // ID
                model.Id = Convert.ToInt32(dr["Id"].ToString());

                // 关键节点编号:使用表名称
                model.Name = dr["Name"].ToString();

                // 关键节点字段:字段名称
                model.Columns = dr["Columns"].ToString();

                // 关键节点名称
                model.NodeName = dr["NodeName"].ToString();

                // 关键节点最小值(含)
                model.MinValue = Convert.ToDecimal(dr["MinValue"].ToString());

                // 使用开始时间
                model.BeginTime = Convert.ToDateTime(dr["BeginTime"].ToString());

                // 使用结束时间
                model.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());

                // 状态：0-初始;1-使用;2-作废 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 系统公司id
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
                 
                // 更改时间
                model.UpdateTime = Convert.ToDateTime(dr["UpdateTime"].ToString());
            }

            // 关闭
            dr.Close();

            // 返回model
            return model;
        }

        #endregion  

        #region 修改 关键节点表

        /// <summary>
        /// 修改 关键节点表
        /// </summary>
        public int EditKeyNode(BasisKeyNodeModel tModel)
        {
            string sql = @" UPDATE 
                              BasisKeyNode
                           SET Name = @Name 
                              ,Columns = @Columns 
                              ,NodeName = @NodeName 
                              ,MinValue = @MinValue 
                              ,BeginTime = @BeginTime 
                              ,EndTime = @EndTime
                              ,CompanyId = @CompanyId 
                              ,UpdateTime = GETDATE() 
                          WHERE         
                              Id = @Id
                          AND State = 0
                          ";

            SqlParameter[] param ={
                // ID
                new SqlParameter("@Id",tModel.Id),
                 
                // 关键节点编号:使用表名称
               new SqlParameter("@Name",tModel.Name),  

                // 关键节点字段:字段名称
                new SqlParameter("@Columns",tModel.Columns),   

                // 关键节点名称
                new SqlParameter("@NodeName",tModel.NodeName),  

                // 关键节点最小值(含)
                new SqlParameter("@MinValue",tModel.MinValue),

                // 使用开始时间 
                new SqlParameter("@BeginTime",tModel.BeginTime),   
                
                // 使用结束时间
                new SqlParameter("@EndTime",tModel.EndTime),  
                  
                // 系统公司id
                new SqlParameter("@CompanyId",tModel.CompanyId)
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

        #region 变更状态 关键节点表

        /// <summary>
        /// 变更状态 关键节点表
        /// </summary>
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int ChangeState(int tId, int tState)
        {
            string sql = @"Update 
                              BasisKeyNode
                           Set                     
                              State = @State
                           Where         
                              Id = @Id ";

            SqlParameter[] param ={ 
                // ID
                new SqlParameter("@Id",tId) ,  
                
                // 状态   
                new SqlParameter("@State",tState)
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

        #region 导出 关键节点表

        /// <summary>
        /// 导出 关键节点表
        /// </summary>
        /// <param name="tWhere"></param>
        /// <returns></returns>
        public List<BasisKeyNodeModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                        (
                            Select 
                                Row_Number() Over(Order By ID ASC) AS 't'
                                ,bkn.Id
                                ,bkn.Name
                                ,bkn.Columns
                                ,bkn.NodeName
                                ,bkn.MinValue
                                ,bkn.BeginTime
                                ,bkn.EndTime   
                                ,CASE bkn.State
                                    WHEN '0' THEN '初始'
                                    WHEN '1' THEN '使用'
                                ELSE '作废' END AS StateName 
                                ,ISNULL(com.CompanyName,'无') As CompanyName
                            From
                                BasisKeyNode bkn Left Join SysCompany com
                            On
                                bkn.CompanyId=com.CompanyId  Where " + tWhere + @" 
                        ) 
                        Select * From TemTable ";


            // 实例化数据集
            List<BasisKeyNodeModel> list = new List<BasisKeyNodeModel>();

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
                BasisKeyNodeModel model = new BasisKeyNodeModel();

                // id自增主键
                model.Id = Convert.ToInt32(dr["Id"].ToString());

                // 关键节点编号
                model.Name = dr["Name"].ToString();

                // 关键节点字段
                model.Columns = dr["Columns"].ToString();

                // 关键节点名称
                model.NodeName = dr["NodeName"].ToString();

                // 关键节点最小值(含)
                model.MinValue = Convert.ToDecimal(dr["MinValue"].ToString());

                // 使用开始时间
                model.BeginTime = Convert.ToDateTime(dr["BeginTime"].ToString());

                // 使用结束时间
                model.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());

                // 公司名称 
                model.CompanyName = dr["CompanyName"].ToString();

                // 状态  
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
