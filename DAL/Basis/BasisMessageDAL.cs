//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/08    1.0        ZBB        新建   
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
 * 类名：BasisMessageDAL
 * 功能描述：信息预登记 数据访问层类
 * ******************************/
namespace DAL.Basis
{
    public class BasisMessageDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 信息预登记表

        /// <summary>
        /// 添加 信息预登记表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int AddBasisMessage(BasisMessageModel model)
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
                                       GETDATE(),
                                       @AddDepartmentId,
                                       @AddUserId,  
                                       @CompanyId
                                  );select @@identity";
            SqlParameter[] param ={

                                        new SqlParameter("@MessageType",model.DictionaryId) ,  //信息类型   

                                        new SqlParameter("@MessageTitle",model.MessageTitle) ,  //信息标题

                                        new SqlParameter("@MessageContent",model.MessageContent) ,  //信息内容  
                                         
                                        new SqlParameter("@MessageRemark",model.MessageRemark??string.Empty) ,  //信息备注   

                                        new SqlParameter("@MessageTop",model.MessageTop) ,  //信息置顶

                                        new SqlParameter("@BeginTime",model.BeginTime),   //有效期开始时间

                                        new SqlParameter("@EndTime",model.EndTime),   //有效结束时间

                                        new SqlParameter("@MessageState",model.MessageState),   //系状态：0-初始;1-提交;2-审核通过;3-审核未通过;4-作废

                                        new SqlParameter("@AddTime",model.AddTime),   //新增时间 

                                        new SqlParameter("@AddDepartmentId",model.AddDepartmentId),   //新增机构Id 

                                        new SqlParameter("@AddUserId",model.AddUserId),   //新增人员id  

                                        new SqlParameter("@CompanyId",model.CompanyId),   //系统公司id 
                                  };
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);

            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }
        #endregion

        #region 修改 信息预登记表

        /// <summary>
        /// 修改 信息预登记表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int UpdateBasisMessage(BasisMessageModel model)
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
                                 EndTime= @EndTime
                          Where         
                                  MessageId = @MessageId";
            SqlParameter[] param ={
                                        new SqlParameter("@MessageId",model.MessageId) ,  //系统信息id  

                                        new SqlParameter("@MessageType",model.MessageType) ,  //信息类型   

                                        new SqlParameter("@MessageTitle",model.MessageTitle) ,  //信息标题

                                        new SqlParameter("@MessageContent",model.MessageContent) ,  //信息内容 
                                          
                                        new SqlParameter("@MessageRemark",model.MessageRemark) ,  //信息备注  
                                         
                                        new SqlParameter("@MessageTop",model.MessageTop) ,  //信息置顶

                                        new SqlParameter("@BeginTime",model.BeginTime),   //有效期开始时间

                                        new SqlParameter("@EndTime",model.EndTime),   //有效结束时间
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 修改 信息预登记审核
        /// <summary>
        ///  修改 信息预登记审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateBasisMessageAudit(BasisMessageModel model)
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
                                 MessageState= @MessageState
                          Where         
                                  MessageId = @MessageId";
            SqlParameter[] param ={
                                        new SqlParameter("@MessageId",model.MessageId) ,  //系统信息id  

                                        new SqlParameter("@MessageType",model.MessageType) ,  //信息类型   

                                        new SqlParameter("@MessageTitle",model.MessageTitle) ,  //信息标题

                                        new SqlParameter("@MessageContent",model.MessageContent) ,  //信息内容   

                                        new SqlParameter("@MessageRemark",model.MessageRemark??string.Empty) ,  //信息备注  
                                         
                                        new SqlParameter("@MessageTop",model.MessageTop) ,  //信息置顶

                                        new SqlParameter("@BeginTime",model.BeginTime),   //有效期开始时间

                                        new SqlParameter("@EndTime",model.EndTime),   //有效结束时间

                                        new SqlParameter("@MessageState",model.MessageState),   //系状态：0-初始;1-提交;2-审核通过;3-审核未通过;4-作废
                                              
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
        public List<BasisMessageModel> BasisMessagePageList(int index, int size, string where)
        {
            string sql = @" With TemTable As 
                            ( 
                                 Select
                                 ROW_NUMBER() Over(Order By mes.MessageId Desc) AS 't',
                                 mes.MessageId,
                                 mes.MessageType ,
                                 dic.DictionaryId,
                                 mes.MessageTitle,
                                 mes.MessageContent,
                                 mes.MessageRemark,
                                 mes.BeginTime,
                                 mes.EndTime,
                                 mes.AddTime,
                                 mes.AddDepartmentId,
                                 mes.AddUserId,
                                 dep.DepartmentName,
                                 mes.ToTime,
                                 mes.ToRemark,
                                 mes.DelTime,
                                 mes.DelDepartmentId,
                                 mes.DelUserId,
                                 mes.MessageState,
                                  CASE mes.MessageState
                                   WHEN '0' THEN '初始'
                                   WHEN '1' THEN '提交'
                                   WHEN '2' THEN '审核通过'
                                   WHEN '3' THEN '审核未通过'
                                   WHEN '4' THEN '作废'
                                 END AS MessageStateName,
                                 mes.MessageTop,
                                 CASE mes.MessageTop
                                   WHEN '0' THEN '是'
                                   WHEN '1' THEN '否' 
                                 END AS MessageTopName,
                                 dic.DictionaryName
                                 FROM dbo.BasisMessage mes 
                                 LEFT JOIN SysCompany com ON mes.CompanyId = com.CompanyId
                                 LEFT JOIN SysDepartment dep ON mes.AddDepartmentId=dep.DepartmentId
                                 LEFT JOIN SysUser sysuser ON mes.ToUserId=sysuser.UserId  
                                 LEFT JOIN dbo.BasisDictionary dic ON dic.DictionaryId=mes.MessageType
                                 Where    " + where + @"
                            ) 
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };
            List<BasisMessageModel> list = new List<BasisMessageModel>();
            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                BasisMessageModel model = new BasisMessageModel();

                model.MessageId = Convert.ToInt32(dr["MessageId"].ToString()); //信息id 

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString()); //字典id 

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

                list.Add(model);//加入到数据集
            }
            dr.Close();
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

        #region 获取实体 信息预登记表

        /// <summary>
        /// 获取实体 信息预登记表
        /// </summary>
        /// <param name="id">信息预登记id</param>
        /// <returns>model</returns>
        public BasisMessageModel GetModelByID(int Id)
        {
            string sql = @"  Select
                             Row_Number() Over(Order By mes.MessageId Desc) AS 't',
                             mes.MessageId,
                             mes.MessageType ,
                             DictionaryId,
                             mes.MessageTitle,
                             mes.MessageContent,
                             mes.MessageRemark,
                             mes.BeginTime,
                             mes.EndTime,
                             mes.AddTime,
                             mes.AddDepartmentId,
                             mes.AddUserId,
                             dep.DepartmentName,
                             mes.ToTime,
                             mes.ToRemark,
                             mes.DelTime,
                             mes.DelDepartmentId,
                             mes.DelUserId,
                             mes.MessageState,
                              CASE mes.MessageState
                               WHEN '0' THEN '初始'
                               WHEN '1' THEN '提交'
                               WHEN '2' THEN '审核通过'
                               WHEN '3' THEN '审核未通过'
                               WHEN '4' THEN '作废'
                              END AS MessageStateName,
                             mes.MessageTop,
                              CASE mes.MessageTop
                               WHEN '0' THEN '是'
                               WHEN '1' THEN '否' 
                              END AS MessageTopName,
                             dic.DictionaryName
                             FROM dbo.BasisMessage mes 
                             LEFT JOIN SysCompany com ON mes.CompanyId = com.CompanyId
                             LEFT JOIN SysDepartment dep ON mes.AddDepartmentId=dep.DepartmentId
                             LEFT JOIN SysUser sysuser ON mes.ToUserId=sysuser.UserId  
                             LEFT JOIN dbo.BasisDictionary dic ON dic.DictionaryId=mes.MessageType
                             WHERE mes.[MessageId]=@MessageId";
            SqlParameter[] param ={
                                   new SqlParameter("@MessageId",Id)
                                  };
            BasisMessageModel model = null;
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
                model = new BasisMessageModel();

                model.MessageId = Convert.ToInt32(dr["MessageId"].ToString()); //信息id 

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString()); //字典id 

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

                model.MessageTopName = dr["MessageTopName"].ToString(); //信息置顶名称    

                model.MessageStateName = dr["MessageStateName"].ToString(); //信息显示状态  

                model.DictionaryName = dr["DictionaryName"].ToString(); //字典名称  

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
        /// <param name="Id">信息预登记id</param>
        /// <param name="State">状态</param>
        /// <param name="DelDepartmentId">作废机构id</param>
        /// <param name="DelUserId">作废负责人</param>
        /// <param name="DelTime">作废时间</param>
        /// <returns></returns>
        public int ChangeState(string Id, int State, int DelDepartmentId, int DelUserId, DateTime DelTime)
        {
            string sql = @"Update 
                                  BasisMessage
                          Set                     
                                  MessageState = @MessageState ,
                                  DelDepartmentId = @DelDepartmentId ,
                                  DelTime = GETDATE() ,
                                  DelUserId = @DelUserId 
                          Where         
                                  MessageId = @MessageId
                         ";
            SqlParameter[] param ={

                new SqlParameter("@MessageId",Id) ,  // 信息预登记ID 

                new SqlParameter("@MessageState",State), // 信息状态   

                new SqlParameter("@DelDepartmentId",DelDepartmentId), // 作废机构id   

                new SqlParameter("@DelUserId",DelUserId), // 作废人id  

                new SqlParameter("@DelTime",DelTime) // 作废时间
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

        #region 变更 信息预登记表中的信息状态

        /// <summary>
        /// 变更 信息预登记表中的信息状态
        /// </summary>
        /// <param name="Id">信息预登记id</param>
        /// <param name="State">状态</param>
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

                new SqlParameter("@MessageState",State), // 信息状态   
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

        #region 分页列表 信息预登记表

        /// <summary>
        /// 分页列表 信息预登记表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public List<BasisMessageModel> BasisMessageAuditPageList(int index, int size, string where)
        {
            string sql = @" With TemTable As 
                            ( 
                                    SELECT  
                                    ROW_NUMBER() OVER ( ORDER BY mes.MessageId DESC ) AS 't' ,
                                    mes.MessageId ,
                                    mes.MessageType ,
                                    DictionaryId ,
                                    mes.MessageTitle ,
                                    mes.MessageContent ,
                                    mes.MessageRemark ,
                                    mes.BeginTime ,
                                    mes.EndTime ,
                                    mes.AddTime ,
                                    mes.AddDepartmentId ,
                                    mes.AddUserId ,
                                    dep.DepartmentName ,
                                    mes.ToTime ,
                                    mes.ToRemark ,
                                    mes.DelTime ,
                                    mes.DelDepartmentId ,
                                    mes.DelUserId ,
                                    mes.MessageState ,
                                    CASE mes.MessageState
                                      WHEN '0' THEN '初始'
                                      WHEN '1' THEN '提交'
                                      WHEN '2' THEN '审核通过'
                                      WHEN '3' THEN '审核未通过'
                                      WHEN '4' THEN '作废'
                                    END AS MessageStateName ,
                                    mes.MessageTop ,
                                    CASE mes.MessageTop
                                      WHEN '0' THEN '是'
                                      WHEN '1' THEN '否'
                                    END AS MessageTopName ,
                                    dic.DictionaryName
                                    FROM     dbo.BasisMessage mes 
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
            List<BasisMessageModel> list = new List<BasisMessageModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);

            while (dr.Read())
            {
                BasisMessageModel model = new BasisMessageModel();

                model.MessageId = Convert.ToInt32(dr["MessageId"].ToString()); //信息id 

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString()); //字典id 

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

                list.Add(model);//加入到数据集
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
        public int BasisMessageAuditPageCount(string where)
        {
            string sql = @" Select 
                                    Count(mes.MessageId) 
                            From
                                    BasisMessage mes
                            LEFT JOIN dbo.BasisDictionary dic ON dic.DictionaryId=mes.MessageType 
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

        #region 导出Excel数据

        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="tWhere">检索条件</param>
        /// <returns></returns>
        public List<BasisMessageModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                        (
                                   SELECT
                                   ROW_NUMBER() OVER ( ORDER BY mes.MessageId DESC ) AS 't' ,
                                   mes.MessageId ,
                                   mes.MessageType ,
                                   dic.DictionaryId ,
                                   mes.MessageTitle ,
                                   mes.MessageContent ,
                                   mes.MessageRemark ,
                                   mes.BeginTime ,
                                   mes.EndTime ,
                                   mes.AddTime ,
                                   mes.AddDepartmentId ,
                                   mes.AddUserId ,
                                   dep.DepartmentName ,
                                   mes.ToTime ,
                                   mes.ToRemark ,
                                   mes.DelTime ,
                                   mes.DelDepartmentId ,
                                   mes.DelUserId ,
                                   mes.MessageState ,
                                   CASE mes.MessageState
                                     WHEN '0' THEN '初始'
                                     WHEN '1' THEN '提交'
                                     WHEN '2' THEN '审核通过'
                                     WHEN '3' THEN '审核未通过'
                                     WHEN '4' THEN '作废'
                                   END AS MessageStateName ,
                                   mes.MessageTop ,
                                   CASE mes.MessageTop
                                     WHEN '0' THEN '是'
                                     WHEN '1' THEN '否'
                                   END AS MessageTopName ,
                                   dic.DictionaryName 
                                   FROM    dbo.BasisMessage mes 
                                   LEFT JOIN SysCompany com ON mes.CompanyId = com.CompanyId
                                   LEFT JOIN SysDepartment dep ON mes.AddDepartmentId = dep.DepartmentId
                                   LEFT JOIN SysUser sysuser ON mes.ToUserId = sysuser.UserId
                                   LEFT JOIN dbo.BasisDictionary dic ON dic.DictionaryId = mes.MessageType
                                  Where    " + tWhere + @"
                        ) 
                        Select * From TemTable ";
            List<BasisMessageModel> list = new List<BasisMessageModel>();   // 实例化数据集

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
                BasisMessageModel model = new BasisMessageModel();

                model.MessageId = Convert.ToInt32(dr["MessageId"].ToString()); //信息id 

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString()); //字典id 

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

                model.MessageTopName = dr["MessageTopName"].ToString(); //信息置顶名称 

                model.MessageStateName = dr["MessageStateName"].ToString(); //信息显示状态   

                model.DictionaryName = dr["DictionaryName"].ToString(); //字典名称  

                model.MessageContent = dr["MessageContent"].ToString(); //信息内容  

                model.MessageRemark = dr["MessageRemark"].ToString(); //信息备注   

                model.AddUserId = Convert.ToInt32(dr["AddUserId"].ToString()); //新增用户id  

                list.Add(model);// 加入到数据集
            }
            dr.Close();// 关闭 

            return list;// 返回数据集
        }
        #endregion
    }
}
