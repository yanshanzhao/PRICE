//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-06-07    1.0        FJK        新建
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
 * 类名：BasisAuditRelationDAL
 * 功能描述：供应商审核关系维护表 数据访问层类
 * ******************************/

namespace DAL.Basis
{
    public class BasisAuditRelationDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 供应商审核关系维护表 

        /// <summary>
        /// 添加 供应商审核关系维护表 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>int</returns>
        public int AddAuditRelation(BasisAuditRelationModel tModel)
        {
            string sql = @"INSERT INTO BasisAuditRelation
                                 (
                                 AuditRelationType
                                 ,AuditRelationNumber
                                 ,AuditRelationName
                                 ,DepartmentId
                                 ,UserId
                                 ,ToDepartmentId
                                 ,ToUserId
                                 ,IsControl
                                 ,ReturnType
                                 ,EndAudit
                                 ,BeginNode
                                 ,BeforeId
                                 ,State
                                 ,CompanyId
                                 )
                           VALUES
                                 (
                                 @AuditRelationType
                                 ,@AuditRelationNumber
                                 ,@AuditRelationName
                                 ,@DepartmentId
                                 ,@UserId
                                 ,@ToDepartmentId
                                 ,@ToUserId
                                 ,@IsControl
                                 ,@ReturnType
                                 ,@EndAudit
                                 ,@BeginNode 
                                 ,@BeforeId 
                                 ,@State
                                 ,@CompanyId 
                                 )";
            SqlParameter[] param ={
                // 供应商审核关系类型
                new SqlParameter("@AuditRelationType",tModel.AuditRelationType),

                // 供应商审核关系编号(流程编号):以ARN开头
                new SqlParameter("@AuditRelationNumber",tModel.AuditRelationNumber),

                // 供应商审核关系名称
                new SqlParameter("@AuditRelationName",tModel.AuditRelationName),

                // 提起机构id
                new SqlParameter("@DepartmentId",tModel.DepartmentId),

                // 提起人员id
                new SqlParameter("@UserId",tModel.UserId),

                // 审核机构id
                new SqlParameter("@ToDepartmentId",tModel.ToDepartmentId),

                // 审核人员id
                new SqlParameter("@ToUserId",tModel.ToUserId),

                // 使用关键控制
                new SqlParameter("@IsControl",tModel.IsControl),

                // 退回类型 
                new SqlParameter("@ReturnType",tModel.ReturnType),

                // 结束审核 
                new SqlParameter("@EndAudit",tModel.EndAudit),

                // 流程开始节点 
                new SqlParameter("@BeginNode",tModel.BeginNode),

                // 上一个流程ID 
                new SqlParameter("@BeforeId",tModel.BeforeId),

                // 状态 
                new SqlParameter("@State",tModel.State), 

                // 系统公司id
                new SqlParameter("@CompanyId",tModel.CompanyId)

            };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }

        #endregion

        #region 分页列表 供应商审核关系维护表 

        /// <summary>
        /// 分页列表 供应商审核关系维护表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns>
        public List<BasisAuditRelationModel> AuditRelationList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                           SELECT 
                               Row_Number() Over(Order By AuditRelationId DESC) AS 't'
                               ,AuditRelationId
                               ,AuditRelationType
                               ,CASE AuditRelationType
                               	WHEN '1' THEN '仓储开发审核'
                               	WHEN '2' THEN '仓储选择审核'
                               	WHEN '3' THEN '运输开发审核'
                               	WHEN '4' THEN '运输选择审核'
                               	WHEN '5' THEN '运输选择评分审核'
                               	WHEN '6' THEN '运输运作准备审核'
                               	WHEN '7' THEN '配送人员审核'
                               END AS AuditRelationTypeName
                               ,AuditRelationNumber
                               ,AuditRelationName
                               ,bar.DepartmentId
                               ,sd1.DepartmentName AS DepartmentName
                               ,bar.UserId
                               ,su1.TrueName TrueName
                               ,ToDepartmentId
                               ,sd2.DepartmentName AS ToDepartmentName
                               ,ToUserId
                               ,su2.TrueName ToTrueName
                               ,IsControl     
                               ,CASE IsControl
                               	WHEN '0' THEN '否'
                               	WHEN '1' THEN '是' 
                               END AS IsControlName
                               ,ReturnType 
                               ,CASE ReturnType
                               	WHEN '0' THEN '无'
                               	WHEN '1' THEN '退回申请人' 
                               	WHEN '2' THEN '退回上一个人'
                               	WHEN '3' THEN '结束审核' 
                               END AS ReturnTypeName
                               ,EndAudit 
                               ,CASE EndAudit
                               	WHEN '0' THEN '否'
                               	WHEN '1' THEN '是' 
                               END AS EndAuditName
                               ,BeginNode 
                               ,CASE BeginNode
                               	WHEN '0' THEN '否'
                               	WHEN '1' THEN '是' 
                               END AS BeginNodeName
                               ,BeforeId 
                               ,bar.[State]
                               ,CASE bar.[State]
                               	WHEN '0' THEN '无效'
                               	WHEN '1' THEN '有效' 
                               END AS StateName
                               ,bar.CompanyId
                           FROM 
                               BasisAuditRelation bar
                           LEFT JOIN 
                               SysUser su1
                           ON 
                           	   su1.UserId = bar.UserId 
                           LEFT JOIN 
                           	   SysUser su2
                           ON 
                           	   su2.UserId = bar.ToUserId 
                           LEFT JOIN
                           	   SysDepartment sd1
                           ON 
                           	   sd1.DepartmentId = bar.DepartmentId 
                           LEFT JOIN
                           	SysDepartment sd2
                           ON 
                           	sd2.DepartmentId = bar.ToDepartmentId 
                           WHERE" + tWhere + @" 
                            )
                            Select * From TemTable Where t  Between @Size*(@Index-1)+1 And @Size*@Index";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",tSize),
                                   new SqlParameter("@Index",tIndex)
                                  };

            // 实例化数据集
            List<BasisAuditRelationModel> list = new List<BasisAuditRelationModel>();

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
                BasisAuditRelationModel model = new BasisAuditRelationModel();

                // 供应商审核关系id
                model.AuditRelationId = Convert.ToInt32(dr["AuditRelationId"].ToString());

                // 供应商审核关系类型id
                model.AuditRelationType = Convert.ToInt32(dr["AuditRelationType"].ToString());

                // 供应商审核关系类型名称
                model.AuditRelationTypeName = dr["AuditRelationTypeName"].ToString();

                // 供应商审核关系编号(流程编号):以ARN开头
                model.AuditRelationNumber = dr["AuditRelationNumber"].ToString();

                // 供应商审核关系名称
                model.AuditRelationName = dr["AuditRelationName"].ToString();

                // 提起机构id
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString());

                // 提起机构名称
                model.DepartmentName = dr["DepartmentName"].ToString();

                // 提起人员id
                model.UserId = Convert.ToInt32(dr["UserId"].ToString());

                // 提起人员名称
                model.TrueName = dr["TrueName"].ToString();

                // 审核机构id
                model.ToDepartmentId = Convert.ToInt32(dr["ToDepartmentId"].ToString());

                // 审核机构名称
                model.ToDepartmentName = dr["ToDepartmentName"].ToString();

                // 审核人员id
                model.ToUserId = Convert.ToInt32(dr["ToUserId"].ToString());

                // 审核人员名称
                model.ToTrueName = dr["ToTrueName"].ToString();

                // 使用关键控制
                model.IsControl = Convert.ToInt32(dr["IsControl"].ToString());

                // 关键控制名称
                model.IsControlName = dr["IsControlName"].ToString();

                // 退回类型 
                model.ReturnType = Convert.ToInt32(dr["ReturnType"].ToString());

                // 退回类型名称
                model.ReturnTypeName = dr["ReturnTypeName"].ToString();

                // 结束审核 
                model.EndAudit = Convert.ToInt32(dr["EndAudit"].ToString());

                // 结束审核名称
                model.EndAuditName = dr["EndAuditName"].ToString();

                // 流程开始节点 
                model.BeginNode = Convert.ToInt32(dr["BeginNode"].ToString());

                // 流程开始节点名称
                model.BeginNodeName = dr["BeginNodeName"].ToString();

                // 上一个流程ID 
                model.BeforeId = Convert.ToInt32(dr["BeforeId"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 状态名称
                model.StateName =dr["StateName"].ToString();

                // 系统公司id
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());

                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }

        #endregion  

        #region 分页总数 供应商审核关系维护表

        /// <summary>
        /// 分页总数 供应商审核关系维护表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int AuditRelationAmount(string tWhere)
        {
            string sql = @"SELECT
                               Count(AuditRelationId) 
                           FROM 
                               BasisAuditRelation bar
                           LEFT JOIN 
                               SysUser su1
                           ON 
                           	   su1.UserId = bar.UserId 
                           LEFT JOIN 
                           	   SysUser su2
                           ON 
                           	   su2.UserId = bar.ToUserId 
                           LEFT JOIN
                           	   SysDepartment sd1
                           ON 
                           	   sd1.DepartmentId = bar.DepartmentId 
                           LEFT JOIN
                           	SysDepartment sd2
                           ON 
                           	sd2.DepartmentId = bar.ToDepartmentId 
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
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #region 获取实体 供应商审核关系维护表

        /// <summary>
        /// 获取实体 供应商审核关系维护表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisAuditRelationModel GetModelByID(int tId)
        {
            string sql = @" SELECT 
                            	AuditRelationId
                            	,AuditRelationType
                            	,CASE AuditRelationType
                            		WHEN '1' THEN '仓储开发审核'
                            		WHEN '2' THEN '仓储选择审核'
                            		WHEN '3' THEN '运输开发审核'
                            		WHEN '4' THEN '运输选择审核'
                            		WHEN '5' THEN '运输选择评分审核'
                            		WHEN '6' THEN '运输运作准备审核'
                            		WHEN '7' THEN '配送人员审核'
                            	END AS AuditRelationTypeName
                            	,AuditRelationNumber
                            	,AuditRelationName 
                            	,bar.DepartmentId
                            	,sd1.DepartmentName AS DepartmentName
                            	,bar.UserId
                            	,su1.TrueName TrueName
                            	,ToDepartmentId
                            	,sd2.DepartmentName AS ToDepartmentName
                            	,ToUserId
                            	,su2.TrueName ToTrueName	
                            	,IsControl
                            	,ReturnType
                            	,CASE ReturnType
                            		WHEN '0' THEN '无'
                            		WHEN '1' THEN '退回申请人'
                            		WHEN '2' THEN '退回上一个人'
                            		WHEN '3' THEN '结束审核' 
                            	END AS ReturnTypeName
                            	,EndAudit
                            	,BeginNode
                            	,BeforeId
                            	,bar.State
                            	,bar.CompanyId
                            FROM BasisAuditRelation  bar
                            LEFT JOIN 
                            	SysUser su1
                            ON 
                            	su1.UserId = bar.UserId 
                            LEFT JOIN 
                            	SysUser su2
                            ON 
                            	su2.UserId = bar.ToUserId 
                            LEFT JOIN
                            	SysDepartment sd1
                            ON 
                            	sd1.DepartmentId = bar.DepartmentId 
                            LEFT JOIN
                            	SysDepartment sd2
                            ON 
                            	sd2.DepartmentId = bar.ToDepartmentId 
                            WHERE 
                            	AuditRelationId = @AuditRelationId";
            SqlParameter[] param ={
                                   new SqlParameter("@AuditRelationId",tId)
                                  };

            BasisAuditRelationModel model = null;
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
                model = new BasisAuditRelationModel();

                // 供应商审核关系id
                model.AuditRelationId = Convert.ToInt32(dr["AuditRelationId"].ToString());

                // 供应商审核关系类型id
                model.AuditRelationType = Convert.ToInt32(dr["AuditRelationType"].ToString());

                // 供应商审核关系类型名称
                model.AuditRelationTypeName = dr["AuditRelationTypeName"].ToString();

                // 供应商审核关系编号(流程编号):以ARN开头
                model.AuditRelationNumber = dr["AuditRelationNumber"].ToString();

                // 供应商审核关系名称
                model.AuditRelationName = dr["AuditRelationName"].ToString();

                // 提起机构id
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString());

                // 提起机构名称
                model.DepartmentName = dr["DepartmentName"].ToString();

                // 提起人员id
                model.UserId = Convert.ToInt32(dr["UserId"].ToString());

                // 提起人员名称
                model.TrueName = dr["TrueName"].ToString();

                // 审核机构id
                model.ToDepartmentId = Convert.ToInt32(dr["ToDepartmentId"].ToString());

                // 审核机构名称
                model.ToDepartmentName = dr["ToDepartmentName"].ToString();

                // 审核人员id
                model.ToUserId = Convert.ToInt32(dr["ToUserId"].ToString());

                // 审核人员名称
                model.ToTrueName = dr["ToTrueName"].ToString();

                // 使用关键控制
                model.IsControl = Convert.ToInt32(dr["IsControl"].ToString());

                // 退回类型 
                model.ReturnType = Convert.ToInt32(dr["ReturnType"].ToString());

                // 退回类型名称
                model.ReturnTypeName = dr["ReturnTypeName"].ToString();

                // 结束审核 
                model.EndAudit = Convert.ToInt32(dr["EndAudit"].ToString());

                // 流程开始节点 
                model.BeginNode = Convert.ToInt32(dr["BeginNode"].ToString());

                // 上一个流程ID 
                model.BeforeId = Convert.ToInt32(dr["BeforeId"].ToString());

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

        #region 修改 供应商审核关系维护表

        /// <summary>
        /// 修改 供应商审核关系维护表
        /// </summary>
        public int EditAuditRelation(BasisAuditRelationModel tModel)
        {
            string sql = @" UPDATE 
                              BasisAuditRelation
                           SET  
                              AuditRelationNumber = @AuditRelationNumber 
                              ,AuditRelationName = @AuditRelationName 
                              ,DepartmentId = @DepartmentId 
                              ,UserId = @UserId 
                              ,ToDepartmentId = @ToDepartmentId
                              ,ToUserId = @ToUserId  
                              ,IsControl = @IsControl  
                              ,ReturnType = @ReturnType   
                              ,EndAudit = @EndAudit     
                          WHERE
                              AuditRelationId = @AuditRelationId 
                          ";

            SqlParameter[] param ={ 
                // ID
                new SqlParameter("@AuditRelationId",tModel.AuditRelationId),
                  
                // 供应商审核关系编号(流程编号):以ARN开头
                new SqlParameter("@AuditRelationNumber", tModel.AuditRelationNumber), 

                // 供应商审核关系名称
                new SqlParameter("@AuditRelationName",tModel.AuditRelationName),

                // 提起机构id
                new SqlParameter("@DepartmentId",tModel.DepartmentId),

                // 提起人员id
                new SqlParameter("@UserId",tModel.UserId),

                // 审核机构id
                new SqlParameter("@ToDepartmentId",tModel.ToDepartmentId),

                // 审核人员id
                new SqlParameter("@ToUserId",tModel.ToUserId),

                // 使用关键控制
                new SqlParameter("@IsControl",tModel.IsControl),

                // 退回类型 
                new SqlParameter("@ReturnType",tModel.ReturnType), 
                 
                // 结束审核 
                new SqlParameter("@EndAudit",tModel.EndAudit)
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

        #region 作废 机构关系（供应商审核关系）维护状态

        /// <summary>
        /// 变更 机构关系（供应商审核关系）维护状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int ChangeState(string tIds)
        {
            string sql = @"Update 
                              BasisAuditRelation
                           Set                     
                              State = 0
                          WHERE
                              AuditRelationId in (" + tIds + @")";
              
            // 影响行数
            int row = 0;
            try
            {
                row = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, null);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return row;
        }

        #endregion

        #region 是否还有使用该流程编号的流程

        /// <summary>
        /// 是否还有使用该流程编号的流程
        /// </summary> 
        /// <returns>list</returns>
        public List<int> IsExistForNumber(string tNumber)
        {
            string sql = @" SELECT 
                            AuditRelationId 
                            FROM 
                            BasisAuditRelation 
                            WHERE 
                            AuditRelationNumber = " + tNumber + @"";

            // 实例化数据集
            List<int> list = new List<int>();

            // 从数据库读取数据
            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            }
            catch (Exception ex)
            {
                return null;
            }

            // 循环数据加入到数据集中
            while (dr.Read())
            {
                BasisAuditRelationModel model = new BasisAuditRelationModel();

                // 供应商审核关系id
                model.AuditRelationId = Convert.ToInt32(dr["AuditRelationId"].ToString());

                // 加入到数据集
                list.Add(model.AuditRelationId);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }

        #endregion  

        #region 审核表中是否有未审核数据(同流程编号)
        /// <summary>
        /// 审核表中是否有未审核数据(同流程编号)
        /// </summary>
        /// <param name="tNumber">流程编号</param>
        /// <returns></returns>
        public int IsExistForAudit(string tNumber)
        {
            var sql = "SELECT COUNT(0) FROM SupplierAudit WHERE State = 0 AND AuditRelationNumber = " + tNumber + @"";
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, null);
            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }
        #endregion

        #region 导出 供应商审核关系维护表

        /// <summary>
        /// 导出 供应商审核关系维护表
        /// </summary>
        /// <param name="tWhere"></param>
        /// <returns></returns>
        public List<BasisAuditRelationModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                           SELECT 
                               Row_Number() Over(Order By AuditRelationId ASC) AS 't'
                               ,AuditRelationId
                               ,CASE AuditRelationType
                               	WHEN '1' THEN '仓储开发审核'
                               	WHEN '2' THEN '仓储选择审核'
                               	WHEN '3' THEN '运输开发审核'
                               	WHEN '4' THEN '运输选择审核'
                               	WHEN '5' THEN '运输选择评分审核'
                               	WHEN '6' THEN '运输运作准备审核'
                               	WHEN '7' THEN '配送人员审核'
                               END AS AuditRelationTypeName
                               ,AuditRelationNumber
                               ,AuditRelationName 
                               ,sd1.DepartmentName AS DepartmentName 
                               ,su1.TrueName TrueName 
                               ,sd2.DepartmentName AS ToDepartmentName 
                               ,su2.TrueName ToTrueName 
                               ,CASE IsControl
                               	WHEN '0' THEN '否'
                               	WHEN '1' THEN '是' 
                               END AS IsControlName 
                               ,CASE ReturnType
                               	WHEN '0' THEN '无'
                               	WHEN '1' THEN '退回申请人' 
                               	WHEN '2' THEN '退回上一个人'
                               	WHEN '3' THEN '结束审核' 
                               END AS ReturnTypeName 
                               ,CASE EndAudit
                               	WHEN '0' THEN '否'
                               	WHEN '1' THEN '是' 
                               END AS EndAuditName 
                               ,CASE BeginNode
                               	WHEN '0' THEN '否'
                               	WHEN '1' THEN '是' 
                               END AS BeginNodeName 
                               ,CASE bar.[State]
                               	WHEN '0' THEN '无效'
                               	WHEN '1' THEN '有效' 
                               END AS StateName
                               ,bar.CompanyId
                           FROM 
                               BasisAuditRelation bar
                           LEFT JOIN 
                               SysUser su1
                           ON 
                           	   su1.UserId = bar.UserId 
                           LEFT JOIN 
                           	   SysUser su2
                           ON 
                           	   su2.UserId = bar.ToUserId 
                           LEFT JOIN
                           	   SysDepartment sd1
                           ON 
                           	   sd1.DepartmentId = bar.DepartmentId 
                           LEFT JOIN
                           	SysDepartment sd2
                           ON 
                           	sd2.DepartmentId = bar.ToDepartmentId 
                           WHERE" + tWhere + @" 
                            )
                            Select * From TemTable";


            // 实例化数据集
            List<BasisAuditRelationModel> list = new List<BasisAuditRelationModel>();

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
                BasisAuditRelationModel model = new BasisAuditRelationModel();

                // 供应商审核关系id
                model.AuditRelationId = Convert.ToInt32(dr["AuditRelationId"].ToString());
                 
                // 供应商审核关系类型名称
                model.AuditRelationTypeName = dr["AuditRelationTypeName"].ToString();

                // 供应商审核关系编号(流程编号):以ARN开头
                model.AuditRelationNumber = dr["AuditRelationNumber"].ToString();

                // 供应商审核关系名称
                model.AuditRelationName = dr["AuditRelationName"].ToString();
                 
                // 提起机构名称
                model.DepartmentName = dr["DepartmentName"].ToString();
                 
                // 提起人员名称
                model.TrueName = dr["TrueName"].ToString();
                 
                // 审核机构名称
                model.ToDepartmentName = dr["ToDepartmentName"].ToString();
                 
                // 审核人员名称
                model.ToTrueName = dr["ToTrueName"].ToString();
                 
                // 关键控制名称
                model.IsControlName = dr["IsControlName"].ToString();
                 
                // 退回类型名称
                model.ReturnTypeName = dr["ReturnTypeName"].ToString();
                 
                // 结束审核名称
                model.EndAuditName = dr["EndAuditName"].ToString();
                 
                // 流程开始节点名称
                model.BeginNodeName = dr["BeginNodeName"].ToString();
                 
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

        #region 获取实体 供应商审核关系维护表

        /// <summary>
        /// 获取实体 供应商审核关系维护表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisAuditRelationModel IsMatchForAudit(int auditRelationType, int state, int beginNode, int userId, int departmentId, int companyId)
        {
            string sql = @" SELECT 
                                TOP 1
                            	AuditRelationId
                            	,AuditRelationType 
                            	,AuditRelationNumber
                            	,AuditRelationName 
                            	,DepartmentId 
                            	,UserId 
                            	,ToDepartmentId 
                            	,ToUserId 
                            	,IsControl
                            	,ReturnType 
                            	,EndAudit
                            	,BeginNode
                            	,BeforeId
                            	,State
                            	,CompanyId
                            FROM BasisAuditRelation 
                            WHERE 
                            	AuditRelationType = @AuditRelationType
                            AND 
                            	State = @State
                            AND 
                            	BeginNode = @BeginNode
                            AND 
                            	UserId = @UserId
                            AND 
                            	DepartmentId = @DepartmentId
                            AND 
                            	CompanyId = @CompanyId
                            ORDER BY AuditRelationId DESC 
                            ";
            SqlParameter[] param ={
                                   new SqlParameter("@AuditRelationType",auditRelationType),
                                   new SqlParameter("@State",state),
                                   new SqlParameter("@BeginNode",beginNode),
                                   new SqlParameter("@UserId",userId),
                                   new SqlParameter("@DepartmentId",departmentId),
                                   new SqlParameter("@CompanyId",companyId)
                                  };

            BasisAuditRelationModel model = null;
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
                model = new BasisAuditRelationModel();

                // 供应商审核关系id
                model.AuditRelationId = Convert.ToInt32(dr["AuditRelationId"].ToString());

                // 供应商审核关系类型id
                model.AuditRelationType = Convert.ToInt32(dr["AuditRelationType"].ToString());
                 
                // 供应商审核关系编号(流程编号):以ARN开头
                model.AuditRelationNumber = dr["AuditRelationNumber"].ToString();

                // 供应商审核关系名称
                model.AuditRelationName = dr["AuditRelationName"].ToString();

                // 提起机构id
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString());
                 
                // 提起人员id
                model.UserId = Convert.ToInt32(dr["UserId"].ToString());
                 
                // 审核机构id
                model.ToDepartmentId = Convert.ToInt32(dr["ToDepartmentId"].ToString());
                 
                // 审核人员id
                model.ToUserId = Convert.ToInt32(dr["ToUserId"].ToString());
                 
                // 使用关键控制
                model.IsControl = Convert.ToInt32(dr["IsControl"].ToString());

                // 退回类型 
                model.ReturnType = Convert.ToInt32(dr["ReturnType"].ToString());
                 
                // 结束审核 
                model.EndAudit = Convert.ToInt32(dr["EndAudit"].ToString());

                // 流程开始节点 
                model.BeginNode = Convert.ToInt32(dr["BeginNode"].ToString());

                // 上一个流程ID 
                model.BeforeId = Convert.ToInt32(dr["BeforeId"].ToString());

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
        /// 获取实体 供应商审核关系维护表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisAuditRelationModel IsMatchForControl(int auditRelationType, int state, int beginNode, int userId, int departmentId, int companyId,string type)
        {
            string tWhere = " AND IsControl = 0";

            if (!string.IsNullOrEmpty(type))
            {
                tWhere = " AND IsControl = 1";
            } 

            string sql = @" SELECT 
                                TOP 1
                            	AuditRelationId
                            	,AuditRelationType 
                            	,AuditRelationNumber
                            	,AuditRelationName 
                            	,DepartmentId 
                            	,UserId 
                            	,ToDepartmentId 
                            	,ToUserId 
                            	,IsControl
                            	,ReturnType 
                            	,EndAudit
                            	,BeginNode
                            	,BeforeId
                            	,State
                            	,CompanyId
                            FROM BasisAuditRelation 
                            WHERE 
                            	AuditRelationType = @AuditRelationType
                            AND 
                            	State = @State
                            AND 
                            	BeginNode = @BeginNode
                            AND 
                            	UserId = @UserId
                            AND 
                            	DepartmentId = @DepartmentId
                            AND 
                            	CompanyId = @CompanyId "+ tWhere + @"
                            
                            ORDER BY AuditRelationId DESC 
                            ";
            SqlParameter[] param ={
                                   new SqlParameter("@AuditRelationType",auditRelationType),
                                   new SqlParameter("@State",state),
                                   new SqlParameter("@BeginNode",beginNode),
                                   new SqlParameter("@UserId",userId),
                                   new SqlParameter("@DepartmentId",departmentId),
                                   new SqlParameter("@CompanyId",companyId)
                                  };

            BasisAuditRelationModel model = null;
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
                model = new BasisAuditRelationModel();

                // 供应商审核关系id
                model.AuditRelationId = Convert.ToInt32(dr["AuditRelationId"].ToString());

                // 供应商审核关系类型id
                model.AuditRelationType = Convert.ToInt32(dr["AuditRelationType"].ToString());
                 
                // 供应商审核关系编号(流程编号):以ARN开头
                model.AuditRelationNumber = dr["AuditRelationNumber"].ToString();

                // 供应商审核关系名称
                model.AuditRelationName = dr["AuditRelationName"].ToString();

                // 提起机构id
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString());
                 
                // 提起人员id
                model.UserId = Convert.ToInt32(dr["UserId"].ToString());
                 
                // 审核机构id
                model.ToDepartmentId = Convert.ToInt32(dr["ToDepartmentId"].ToString());
                 
                // 审核人员id
                model.ToUserId = Convert.ToInt32(dr["ToUserId"].ToString());
                 
                // 使用关键控制
                model.IsControl = Convert.ToInt32(dr["IsControl"].ToString());

                // 退回类型 
                model.ReturnType = Convert.ToInt32(dr["ReturnType"].ToString());
                 
                // 结束审核 
                model.EndAudit = Convert.ToInt32(dr["EndAudit"].ToString());

                // 流程开始节点 
                model.BeginNode = Convert.ToInt32(dr["BeginNode"].ToString());

                // 上一个流程ID 
                model.BeforeId = Convert.ToInt32(dr["BeforeId"].ToString());

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

        #region 获取实体 供应商审核关系维护表

        /// <summary>
        /// 获取实体 供应商审核关系维护表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisAuditRelationModel IsRelationByBeforeId(int beforeId)
        {
            string sql = @" SELECT  
                            	AuditRelationId
                            	,AuditRelationType 
                            	,AuditRelationNumber
                            	,AuditRelationName 
                            	,DepartmentId 
                            	,UserId 
                            	,ToDepartmentId 
                            	,ToUserId 
                            	,IsControl
                            	,ReturnType 
                            	,EndAudit
                            	,BeginNode
                            	,BeforeId
                            	,State
                            	,CompanyId
                            FROM BasisAuditRelation 
                            WHERE 
                            	State = 1  
                            AND 
                            	BeforeId = @BeforeId  
                            ";
            SqlParameter[] param ={ 
                                   new SqlParameter("@BeforeId",beforeId)
                                  };

            BasisAuditRelationModel model = null;
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
                model = new BasisAuditRelationModel();

                // 供应商审核关系id
                model.AuditRelationId = Convert.ToInt32(dr["AuditRelationId"].ToString());

                // 供应商审核关系类型id
                model.AuditRelationType = Convert.ToInt32(dr["AuditRelationType"].ToString());
                 
                // 供应商审核关系编号(流程编号):以ARN开头
                model.AuditRelationNumber = dr["AuditRelationNumber"].ToString();

                // 供应商审核关系名称
                model.AuditRelationName = dr["AuditRelationName"].ToString();

                // 提起机构id
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString());
                 
                // 提起人员id
                model.UserId = Convert.ToInt32(dr["UserId"].ToString());
                 
                // 审核机构id
                model.ToDepartmentId = Convert.ToInt32(dr["ToDepartmentId"].ToString());
                 
                // 审核人员id
                model.ToUserId = Convert.ToInt32(dr["ToUserId"].ToString());
                 
                // 使用关键控制
                model.IsControl = Convert.ToInt32(dr["IsControl"].ToString());

                // 退回类型 
                model.ReturnType = Convert.ToInt32(dr["ReturnType"].ToString());
                 
                // 结束审核 
                model.EndAudit = Convert.ToInt32(dr["EndAudit"].ToString());

                // 流程开始节点 
                model.BeginNode = Convert.ToInt32(dr["BeginNode"].ToString());

                // 上一个流程ID 
                model.BeforeId = Convert.ToInt32(dr["BeforeId"].ToString());

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

    }
}
