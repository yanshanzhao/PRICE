//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-11-01    1.0       ZBB       新建
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using Model.Sys;
#endregion
/*********************************
 * 类名：SysStencilDAL
 * 功能描述：模板维护 数据访问层类
 * ******************************/

namespace DAL.Sys
{
    public class SysStencilDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 模板维护 

        /// <summary>
        /// 添加 模板维护 
        /// </summary>
        /// <param name="model">实体model</param>
        /// <returns>int</returns>
        public int AddSysStencil(SysStencilModel tModel)
        {
            string sql = @"INSERT INTO SysStencil
                                 (     
                                 StencilName  
                                 ,Remark 
                                 ,FileName 
                                 ,Url 
                                 ,State  
                                 ,CreateDepartmentId  
                                 ,CreateUserId  
                                 ,CreateTime  
                                 ,CompanyId  
		                         )
                           VALUES
                                 (    
                                 @StencilName  
                                 ,@Remark 
                                 ,@FileName 
                                 ,@Url 
                                 ,@State  
                                 ,@CreateDepartmentId  
                                 ,@CreateUserId  
                                 ,GETDATE()
                                 ,@CompanyId  
                                 );select @@identity";
            SqlParameter[] param ={

                new SqlParameter("@StencilName",tModel.StencilName),// 运作编号 
                
                new SqlParameter("@Remark",tModel.Remark??string.Empty),// 培训负责人 
                
                new SqlParameter("@FileName",tModel.FileName??string.Empty),// 文件名称 
                 
                new SqlParameter("@Url",tModel.Url),// 文件地址 

                new SqlParameter("@State",tModel.State),// 培训计划结束时间  
                 
                new SqlParameter("@CreateDepartmentId",tModel.CreateDepartmentId),// 机构id  

                new SqlParameter("@CreateUserId",tModel.CreateUserId),// 用户ID  

                new SqlParameter("@CompanyId",tModel.CompanyId),// 供应商id 
                 
            };
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);

            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }

        #endregion 

        #region 分页列表 模板维护 

        /// <summary>
        /// 分页列表 模板维护
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns>
        public List<SysStencilModel> SysStencilList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                          SELECT 
                               Row_Number() Over(Order By toc.StencilId Desc) AS 't',
                               toc.StencilId,
                               toc.StencilName,   
                               toc.State,
                                 CASE toc.State
                                    WHEN '0' THEN '未提交'
                                   WHEN '1' THEN '已提交'  
                                   WHEN '10' THEN '作废'  
                                   WHEN '20' THEN '删除'
                                 END AS StateName, 
								toc.Remark, 
								toc.Url,
								toc.FileName,
								toc.CreateTime
                               FROM dbo.SysStencil toc   
                           WHERE" + tWhere + @" 
                            )
                            Select * From TemTable Where t  Between @Size*(@Index-1)+1 And @Size*@Index";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",tSize),
                                   new SqlParameter("@Index",tIndex)
                                  };

            // 实例化数据集
            List<SysStencilModel> list = new List<SysStencilModel>();

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
                SysStencilModel model = new SysStencilModel();

                model.StencilId = Convert.ToInt32(dr["StencilId"].ToString()); // 运作要求id

                model.StencilName = dr["StencilName"].ToString();// 运作编号 

                model.State = Convert.ToInt32(dr["State"].ToString()); // 提交状态：0-未提交；1-已提交;10-作废;20-删除

                model.StateName = dr["StateName"].ToString();// 显示提交状态

                model.Remark = dr["Remark"].ToString();// 备注

                model.FileName = dr["FileName"].ToString();//文件名称

                model.Url = dr["Url"].ToString();// 文件地址

                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());// 创建时间  

                list.Add(model);// 加入到数据集
            }

            dr.Close();// 关闭 

            return list;// 返回数据集
        }

        #endregion  

        #region 分页总数 模板维护

        /// <summary>
        /// 分页总数 模板维护
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int SysStencilCount(string tWhere)
        {
            string sql = @"SELECT
                               Count(StencilId) 
                           FROM
                               SysStencil toc 
                           WHERE " + tWhere + @"";

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

        #region 获取实体 模板维护

        /// <summary>
        /// 获取实体 模板维护
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public SysStencilModel GetModelByID(int tId)
        {
            string sql = @" SELECT 
                               Row_Number() Over(Order By toc.StencilId Desc) AS 't',
                               toc.StencilId,
                               toc.StencilName,   
                               toc.State,
                                 CASE toc.State
                                    WHEN '0' THEN '未提交'
                                   WHEN '1' THEN '已提交'  
                                   WHEN '10' THEN '作废'  
                                   WHEN '20' THEN '删除'
                                 END AS StateName, 
								toc.Remark, 
								toc.Url,
								toc.FileName,
								toc.CreateTime
                               FROM dbo.SysStencil toc   
                            WHERE 
                            	StencilId = @StencilId";
            SqlParameter[] param ={
                                   new SqlParameter("@StencilId",tId)
                                  };

            SysStencilModel model = null;

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
                model = new SysStencilModel();

                model.StencilId = Convert.ToInt32(dr["StencilId"].ToString()); // 运作要求id

                model.StencilName = dr["StencilName"].ToString();// 运作编号 

                model.State = Convert.ToInt32(dr["State"].ToString()); // 提交状态：0-未提交；1-已提交;10-作废;20-删除

                model.StateName = dr["StateName"].ToString();// 显示提交状态

                model.Remark = dr["Remark"].ToString();// 显示使用方式

                model.Url = dr["Url"].ToString();// 培训主题

                model.FileName = dr["FileName"].ToString();// 文件地址

                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());// 创建时间  
            }

            // 关闭
            dr.Close();

            // 返回model
            return model;
        }

        #endregion

        #region 修改 模板维护

        /// <summary>
        /// 修改 模板维护
        /// </summary>
        /// <param name="tModel">实体model</param>
        /// <returns></returns>
        public int EditSysStencil(SysStencilModel tModel)
        {
            string sql = @" UPDATE 
                                SysStencil 
                            SET       
                              StencilName=@StencilName
                             ,Remark=@Remark   
                          WHERE
                                StencilId = @StencilId 
                          ";

            SqlParameter[] param ={

                new SqlParameter("@StencilId",tModel.StencilId),// 模板id

                new SqlParameter("@StencilName",tModel.StencilName),// 模板名称
                
                new SqlParameter("@Remark",tModel.Remark??string.Empty),// 备注 
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

        #region 作废状态  模板维护
        /// <summary>
        /// 变更 模板维护
        /// </summary>
        /// <param name="StencilId">运作维护id</param>
        /// <param name="delUserId">作废人id</param>
        /// <returns></returns>
        public int InvalidState(int StencilId, int delUserId)
        {
            string sql = @"Update 
                              SysStencil
                           Set                     
                              State = 10
                              ,DelTime = GETDATE()
                              ,DelUserId = @DelUserId
                          WHERE
                              StencilId = @StencilId";

            SqlParameter[] param ={

                new SqlParameter("@StencilId",StencilId),// 仓储供应商选择ID 
                
                new SqlParameter("@DelUserId",delUserId) // 作废人ID
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

        #region 作废状态  模板维护
        /// <summary>
        /// 变更 模板维护
        /// </summary>
        /// <param name="StencilId">运作维护id</param>
        /// <param name="delUserId">作废人id</param>
        /// <returns></returns>
        public int DeleteState(int StencilId, int delUserId)
        {
            string sql = @"Update 
                              SysStencil
                           Set                     
                              State = 20
                              ,DelTime = GETDATE()
                              ,DelUserId = @DelUserId
                          WHERE
                              StencilId = @StencilId";

            SqlParameter[] param ={

                new SqlParameter("@StencilId",StencilId),// 仓储供应商选择ID 
                
                new SqlParameter("@DelUserId",delUserId) // 作废人ID
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

        #region 提交状态 模板维护
        /// <summary>
        /// 变更 模板维护
        /// </summary>
        /// <param name="StencilId">运作维护id</param> 
        /// <returns></returns>
        public int SubmitSysStencil(int StencilId)
        {
            string sql = @"Update 
                              SysStencil
                           Set                     
                              State = 1
                          WHERE
                              StencilId = @StencilId";

            SqlParameter[] param ={ 
            
                // ID主键
                new SqlParameter("@StencilId",StencilId)
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
    }
}
