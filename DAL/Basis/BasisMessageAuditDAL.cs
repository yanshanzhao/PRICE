//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/13    1.0        ZBB        新建   
//2018/07/26    1.1        FJK        新增公司消息通知  
//-------------------------------------------------------------------------
#region 参数
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
 * 类名：BasisMessageAuditDAL
 * 功能描述：信息审核 数据访问层类
 * ******************************/

namespace DAL.Basis
{
    public class BasisMessageAuditDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 信息预登记表

        /// <summary>
        /// 添加 信息预登记表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int AddBasisMessage(BasisMessageAuditModel model)
        {
            string sql = @"Insert into
                                  BasisMessage
                                  (
                                       MessageType,
                                       MessageTitle,
                                       MessageContent,
                                       MessageRemark,
                                       MessageTop,
                                       BeginTime,
                                       EndTime,
                                       MessageState,
                                       AddTime,
                                       AddDepartmentId,
                                       AddUserId, 
                                       ToTime,
                                       ToDepartmentId,
                                       ToUserId,
                                       ToRemark, 
                                       DelDepartmentId,
                                       DelUserId,
                                       CompanyId
                                  )
                          Values
                                  ( 
                                       @MessageType,
                                       @MessageTitle,
                                       @MessageContent,
                                       @MessageRemark,
                                       @MessageTop,
                                       @BeginTime,
                                       @EndTime,
                                       @MessageState,
                                       @AddTime,
                                       @AddDepartmentId,
                                       @AddUserId, 
                                       @ToTime,
                                       @ToDepartmentId,
                                       @ToUserId,
                                       @ToRemark, 
                                       @DelDepartmentId,
                                       @DelUserId,
                                       @CompanyId
                                  )";
            SqlParameter[] param ={
                                        new SqlParameter("@MessageType",model.DictionaryId) ,  //信息类型   

                                        new SqlParameter("@MessageTitle",model.MessageTitle) ,  //信息标题

                                        new SqlParameter("@MessageContent",model.MessageContent) ,  //信息内容  
                                         
                                        new SqlParameter("@MessageRemark",model.MessageRemark) ,  //信息备注   

                                        new SqlParameter("@MessageTop",model.MessageTop) ,  //信息置顶

                                        new SqlParameter("@BeginTime",model.BeginTime),   //有效期开始时间

                                        new SqlParameter("@EndTime",model.EndTime),   //有效结束时间

                                        new SqlParameter("@MessageState",model.MessageState),   //系状态：0-初始;1-提交;2-审核通过;3-审核未通过;4-作废

                                        new SqlParameter("@AddTime",model.AddTime),   //新增时间 

                                        new SqlParameter("@AddDepartmentId",model.AddDepartmentId),   //新增机构Id 

                                        new SqlParameter("@AddUserId",model.AddUserId),   //新增人员id 

                                        new SqlParameter("@ToTime",model.ToTime),   //审核时间 

                                        new SqlParameter("@ToDepartmentId",model.ToDepartmentId),   //审核机构id 

                                        new SqlParameter("@ToUserId",model.ToUserId),   //审核人员id 

                                        new SqlParameter("@ToRemark",model.ToRemark),   //审核意见 

                                        new SqlParameter("@DelTime",model.DelTime),   //作废时间 

                                        new SqlParameter("@DelDepartmentId",model.DelDepartmentId),   //作废机构 

                                        new SqlParameter("@DelUserId",model.DelUserId),   //作废人员id 

                                        new SqlParameter("@CompanyId",model.CompanyId),   //系统公司id 
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 修改 信息预登记表

        /// <summary>
        /// 修改 信息预登记表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int UpdateBasisMessage(BasisMessageAuditModel model)
        {
            string sql = @"Update 
                                  BasisMessage
                          Set 
                                 MessageType = @MessageType ,
                                 MessageTitle= @MessageTitle,
                                 MessageContent= @MessageContent,
                                 MessageRemark= @MessageRemark,
                                 MessageTop= @MessageTop,
                                 BeginTime= @BeginTime,
                                 EndTime= @EndTime,
                                 MessageState= @MessageState,  
                                 ToTime= @ToTime,
                                 ToDepartmentId= @ToDepartmentId,
                                 ToUserId= @ToUserId,
                                 ToRemark= @ToRemark, 
                                 CompanyId= @CompanyId
                          Where         
                                  MessageId = @MessageId";
            SqlParameter[] param ={
                                        new SqlParameter("@MessageId",model.MessageId) ,  //系统信息id  

                                        new SqlParameter("@MessageType",model.DictionaryId) ,  //信息类型   

                                        new SqlParameter("@MessageTitle",model.MessageTitle) ,  //信息标题

                                        new SqlParameter("@MessageContent",model.MessageContent) ,  //信息内容  
                                         
                                        new SqlParameter("@MessageRemark",model.MessageRemark) ,  //信息备注   

                                        new SqlParameter("@MessageTop",model.MessageTop) ,  //信息置顶

                                        new SqlParameter("@BeginTime",model.BeginTime),   //有效期开始时间

                                        new SqlParameter("@EndTime",model.EndTime),   //有效结束时间

                                        new SqlParameter("@MessageState",model.MessageState),   //系状态：0-初始;1-提交;2-审核通过;3-审核未通过;4-作废

                                        new SqlParameter("@ToTime",model.ToTime),   //审核时间 

                                        new SqlParameter("@ToDepartmentId",model.ToDepartmentId),   //审核机构id 

                                        new SqlParameter("@ToUserId",model.ToUserId),   //审核人员id 

                                        new SqlParameter("@ToRemark",model.ToRemark??string.Empty),   //审核意见 
                                         
                                        new SqlParameter("@CompanyId",model.CompanyId),   //系统公司id  
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);

            return i;
        }
        #endregion

        #region 分页列表 信息预登记表

        /// <summary>
        /// 分页列表 信息预登记表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public List<BasisMessageAuditModel> BasisMessageAuditPageList(int index, int size, string where)
        {
            string sql = @" With TemTable As 
                            ( 
                                SELECT  ROW_NUMBER() OVER ( ORDER BY mes.MessageId DESC ) AS 't' ,
                                mes.MessageId ,
                                mes.MessageType ,
                                DictionaryId ,
                                mes.MessageTitle ,
                                mes.MessageContent ,
                                mes.MessageRemark ,
                                mes.MessageTop ,
                                mes.BeginTime ,
                                mes.EndTime ,
                                mes.MessageState ,
                                mes.AddTime ,
                                mes.AddDepartmentId ,
                                mes.AddUserId ,
                                dep.DepartmentName ,
                                mes.ToTime ,
                                mes.ToRemark ,
                                mes.DelTime ,
                                mes.DelDepartmentId ,
                                mes.DelUserId ,
                                CASE mes.MessageState
                                  WHEN '0' THEN '初始'
                                  WHEN '1' THEN '提交'
                                  WHEN '2' THEN '审核通过'
                                  WHEN '3' THEN '审核未通过'
                                  WHEN '4' THEN '作废'
                                END AS MessageStateName ,
                                dic.DictionaryName ,
                                CASE mes.MessageTop
                                  WHEN '0' THEN '是'
                                  WHEN '1' THEN '否'
                                END AS MessageTopName
                                FROM    dbo.BasisMessage mes
                                LEFT JOIN dbo.BasisMessageAdjunct mesa ON mes.MessageId = mesa.MessageId
                                LEFT JOIN SysCompany com ON mes.CompanyId = com.CompanyId
                                LEFT JOIN SysDepartment dep ON mes.AddDepartmentId = dep.DepartmentId
                                LEFT JOIN SysUser sysuser ON mes.ToUserId = sysuser.UserId
                                LEFT JOIN dbo.BasisDictionary dic ON dic.DictionaryId = mes.MessageType
                                 Where    " + where + @"
                            ) 
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };
            List<BasisMessageAuditModel> list = new List<BasisMessageAuditModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);

            // 循环数据加入到数据集中
            while (dr.Read())
            {
                BasisMessageAuditModel model = new BasisMessageAuditModel();

                model.MessageId = Convert.ToInt32(dr["MessageId"].ToString()); //信息id 

                model.MessageType = Convert.ToInt32(dr["MessageType"].ToString()); //信息类型 

                model.MessageTitle = dr["MessageTitle"].ToString(); //信息标题 

                model.MessageTop = Convert.ToInt32(dr["MessageTop"].ToString()); //信息置顶 

                model.BeginTime = Convert.ToDateTime(dr["BeginTime"].ToString());//有效开始时间

                model.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());//有效结束时间

                model.MessageState = Convert.ToInt32(dr["MessageState"].ToString()); //信息状态  

                model.AddTime = Convert.ToDateTime(dr["AddTime"].ToString()); //新增时间 

                model.AddDepartmentId = Convert.ToInt32(dr["AddDepartmentId"].ToString()); //新增机构  

                model.ToTime = dr["ToTime"].ToString(); //审核时间  

                model.ToRemark = dr["ToRemark"].ToString(); //审核备注  

                model.DepartmentName = dr["DepartmentName"].ToString(); //新增机构名称 

                model.DictionaryName = dr["DictionaryName"].ToString(); //字典名称 

                model.MessageTopName = dr["MessageTopName"].ToString(); //信息置顶名称 

                model.MessageStateName = dr["MessageStateName"].ToString(); //信息显示状态   
                 
                model.MessageContent = dr["MessageContent"].ToString(); //信息内容  

                model.MessageRemark = dr["MessageRemark"].ToString(); //信息备注   

                model.AddUserId = Convert.ToInt32(dr["AddUserId"].ToString()); //新增用户id 

                list.Add(model); //加入到数据集
            }

            dr.Close();//关闭 

            return list;//返回数据集
        }
        #endregion  

        #region 分页总数 信息预登记表
        /// <summary>
        /// 分页总数 信息预登记表
        /// </summary>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public int BasisMessagePageCount(string where)
        {
            string sql = @" Select 
                                    Count(mes.MessageId) 
                            From
                                    BasisMessage mes
                            LEFT JOIN dbo.BasisDictionary dic ON dic.DictionaryId=mes.MessageType 
                            Where " + where + "";

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

        #region 获取实体 信息预登记表

        /// <summary>
        /// 获取实体 信息预登记表
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>model</returns>
        public BasisMessageAuditModel GetModelByID(string Id)
        {
            string sql = @"  SELECT  ROW_NUMBER() OVER ( ORDER BY mes.MessageId DESC ) AS 't' ,
                              mes.MessageId ,
                              mes.MessageType ,
                              DictionaryId ,
                              mes.MessageTitle ,
                              mes.MessageContent ,
                              mes.MessageRemark ,
                              mes.MessageTop ,
                              mes.BeginTime ,
                              mes.EndTime ,
                              mes.MessageState ,
                              mes.AddTime ,
                              mes.AddDepartmentId ,
                              mes.AddUserId ,
                              dep.DepartmentName ,
                              mes.ToTime ,
                              mes.ToRemark ,
                              mes.ToUserId ,
                              mes.DelTime ,
                              mes.DelDepartmentId ,
                              mes.DelUserId ,
                              CASE mes.MessageState
                                WHEN '0' THEN '初始'
                                WHEN '1' THEN '提交'
                                WHEN '2' THEN '审核通过'
                                WHEN '3' THEN '审核未通过'
                                WHEN '4' THEN '作废'
                              END AS MessageStateName ,
                              dic.DictionaryName ,
                              CASE mes.MessageTop
                                WHEN '0' THEN '是'
                                WHEN '1' THEN '否'
                              END AS MessageTopName
                              FROM    dbo.BasisMessage mes
                              LEFT JOIN dbo.BasisMessageAdjunct mesa ON mes.MessageId = mesa.MessageId
                              LEFT JOIN SysCompany com ON mes.CompanyId = com.CompanyId
                              LEFT JOIN SysDepartment dep ON mes.AddDepartmentId = dep.DepartmentId
                              LEFT JOIN SysUser sysuser ON mes.ToUserId = sysuser.UserId
                              LEFT JOIN dbo.BasisDictionary dic ON dic.DictionaryId = mes.MessageType
                            WHERE mes.[MessageId]=@MessageId";

            SqlParameter[] param ={
                                   new SqlParameter("@MessageId",Id)
                                  };

            BasisMessageAuditModel model = null;

            SqlDataReader dr = null;// // 从数据库读取数据

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
                model = new BasisMessageAuditModel();

                model.MessageId = Convert.ToInt32(dr["MessageId"].ToString()); //信息id 

                model.MessageType = Convert.ToInt32(dr["MessageType"].ToString()); //信息类型 

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString()); //字典id 

                model.MessageTitle = dr["MessageTitle"].ToString(); //信息标题 

                model.MessageTop = Convert.ToInt32(dr["MessageTop"].ToString()); //信息置顶 

                model.BeginTime = Convert.ToDateTime(dr["BeginTime"].ToString());//有效开始时间

                model.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());//有效结束时间

                model.MessageState = Convert.ToInt32(dr["MessageState"].ToString()); //信息状态  

                model.AddTime = Convert.ToDateTime(dr["AddTime"].ToString()); //新增时间 

                model.AddDepartmentId = Convert.ToInt32(dr["AddDepartmentId"].ToString()); //新增机构  

                model.ToTime = dr["ToTime"].ToString(); //审核时间  

                model.ToRemark = dr["ToRemark"].ToString(); //审核备注  

                model.DepartmentName = dr["DepartmentName"].ToString(); //新增机构名称 

                model.DictionaryName = dr["DictionaryName"].ToString(); //字典名称 

                model.MessageTopName = dr["MessageTopName"].ToString(); //信息置顶名称 

                model.MessageStateName = dr["MessageStateName"].ToString(); //信息显示状态 
                   
                model.MessageContent = dr["MessageContent"].ToString(); //信息内容  

                model.MessageRemark = dr["MessageRemark"].ToString(); //信息备注
                   
                model.AddUserId = Convert.ToInt32(dr["AddUserId"].ToString()); //新增用户id 
            }
            dr.Close(); // 关闭

            return model;// 返回model
        }

        #endregion   

        #region 变更 信息预登记表状态

        /// <summary>
        /// 变更 信息预登记表状态
        /// </summary>
        /// <param name="Id">信息预登记ID</param>
        /// <param name="State">信息状态</param>
        /// <returns></returns>
        public int ChangeState(string Id, int State)
        {
            string sql = @"Update 
                                  BasisMessage
                          Set                     
                                  MessageState = @MessageState 
                          Where         
                                  MessageId = @MessageId
                         ";
            SqlParameter[] param ={

                new SqlParameter("@MessageId",Id) ,  // 信息预登记ID 

                new SqlParameter("@MessageState",State) //信息状态   
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

        #region 导出Excel数据

        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="tWhere">检索条件</param>
        /// <returns></returns>
        public List<BasisMessageAuditModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                        (
                             SELECT ROW_NUMBER() OVER ( ORDER BY mes.MessageId DESC ) AS 't' ,
                             mes.MessageId ,
                             mes.MessageType ,
                             DictionaryId ,
                             mes.MessageTitle ,
                             mes.MessageContent ,
                             mes.MessageRemark ,
                             mes.MessageTop ,
                             mes.BeginTime ,
                             mes.EndTime ,
                             mes.MessageState ,
                             mes.AddTime ,
                             mes.AddDepartmentId ,
                             mes.AddUserId ,
                             dep.DepartmentName ,
                             mes.ToTime ,
                             mes.ToRemark ,
                             mes.DelTime ,
                             mes.DelDepartmentId ,
                             mes.DelUserId ,
                             CASE mes.MessageState
                               WHEN '0' THEN '初始'
                               WHEN '1' THEN '提交'
                               WHEN '2' THEN '审核通过'
                               WHEN '3' THEN '审核未通过'
                               WHEN '4' THEN '作废'
                             END AS MessageStateName ,
                             dic.DictionaryName ,
                             CASE mes.MessageTop
                               WHEN '0' THEN '是'
                               WHEN '1' THEN '否'
                             END AS MessageTopName
                             FROM   dbo.BasisMessage mes
                             LEFT JOIN dbo.BasisMessageAdjunct mesa ON mes.MessageId = mesa.MessageId
                             LEFT JOIN SysCompany com ON mes.CompanyId = com.CompanyId
                             LEFT JOIN SysDepartment dep ON mes.AddDepartmentId = dep.DepartmentId
                             LEFT JOIN SysUser sysuser ON mes.ToUserId = sysuser.UserId
                             LEFT JOIN dbo.BasisDictionary dic ON dic.DictionaryId = mes.MessageType
                                  Where    " + tWhere + @"
                        ) 
                        Select * From TemTable ";
            List<BasisMessageAuditModel> list = new List<BasisMessageAuditModel>();   // 实例化数据集 
            SqlDataReader dr = null; // 从数据库读取数据
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
                BasisMessageAuditModel model = new BasisMessageAuditModel();

                model.MessageId = Convert.ToInt32(dr["MessageId"].ToString()); //信息id 

                model.MessageType = Convert.ToInt32(dr["MessageType"].ToString()); //信息类型 

                model.MessageTitle = dr["MessageTitle"].ToString(); //信息标题 

                model.MessageTop = Convert.ToInt32(dr["MessageTop"].ToString()); //信息置顶 

                model.BeginTime = Convert.ToDateTime(dr["BeginTime"].ToString());//有效开始时间

                model.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());//有效结束时间

                model.MessageState = Convert.ToInt32(dr["MessageState"].ToString()); //信息状态  

                model.AddTime = Convert.ToDateTime(dr["AddTime"].ToString()); //新增时间 

                model.AddDepartmentId = Convert.ToInt32(dr["AddDepartmentId"].ToString()); //新增机构  

                model.ToTime = dr["ToTime"].ToString(); //审核时间  

                model.ToRemark = dr["ToRemark"].ToString(); //审核备注  

                model.DepartmentName = dr["DepartmentName"].ToString(); //新增机构名称 

                model.DictionaryName = dr["DictionaryName"].ToString(); //字典名称 

                model.MessageTopName = dr["MessageTopName"].ToString(); //信息置顶名称 

                model.MessageStateName = dr["MessageStateName"].ToString(); //信息显示状态
                    
                model.MessageContent = dr["MessageContent"].ToString(); //信息内容  

                model.MessageRemark = dr["MessageRemark"].ToString(); //信息备注   

                model.AddUserId = Convert.ToInt32(dr["AddUserId"].ToString()); //新增用户id 

                list.Add(model); // 加入到数据集
            }
            dr.Close(); // 关闭 

            return list;// 返回数据集
        }
        #endregion

        #region 分页列表 信息预登记表

        /// <summary>
        /// 分页列表 信息预登记表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public List<BasisMessageAuditModel> GetMessageList(string where)
        {
            string sql = @"SELECT
                               DictionaryName
                               ,MessageTitle  
                               ,MessageTop  
                           FROM BasisMessage BM
                           LEFT JOIN BasisDictionary BD ON BM.MessageType = BD.DictionaryId
                           WHERE " + where + @"
                           AND MessageState = 1
                           AND GETDATE() >= BeginTime AND GETDATE() <= EndTime
                           ORDER BY MessageType ASC,MessageTop DESC";
             
            // 实例化数据集
            List<BasisMessageAuditModel> list = new List<BasisMessageAuditModel>();

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
                BasisMessageAuditModel model = new BasisMessageAuditModel();
                 
                model.DictionaryName = dr["DictionaryName"].ToString(); // 字典名称
                 
                model.MessageTitle = dr["MessageTitle"].ToString();// 信息标题
                 
                model.MessageTop = Convert.ToInt32(dr["MessageTop"].ToString());// 信息标题
                 
                list.Add(model);  // 加入到数据集
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }
        #endregion
    }
}
