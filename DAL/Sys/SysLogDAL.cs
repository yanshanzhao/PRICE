//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-17    1.0        MH         新建
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
 * 类名：SysLogDAL
 * 功能描述：系统日志 数据访问层类
 * ******************************/
namespace DAL.Sys
{
 public class SysLogDAL
    {  
     // 获取连接串
     string conn =DBUtility.ConnectionStringInfo.ConnectionString().ToString();
     
     
        #region 添加 系统日志
        /// <summary>
        /// 添加 系统日志
        /// </summary>
        public int AddSysLog (SysLogModel model)
        {
           string sql = @"Insert 
                                  SysLog
                                  (
                                        Operator ,
                                        Message ,
                                        Result ,
                                        Type ,
                                        Module ,
                                        CreateTime ,
                                        CompanyId 
                                      
                                  )
                          Values
                                  (
                                        @Operator ,
                                        @Message ,
                                        @Result ,
                                        @Type ,
                                        @Module ,
                                        @CreateTime ,
                                        @CompanyId 
                                  )";
            SqlParameter[] param ={ 
                                        new SqlParameter("@Operator",model.Operator) ,  //操作人   
                                        new SqlParameter("@Message",model.Message) ,  //信息   
                                        new SqlParameter("@Result",model.Result) ,  //结果   
                                        new SqlParameter("@Type",model.Type) ,  //类型（按钮）   
                                        new SqlParameter("@Module",model.Module) ,  //功能菜单   
                                        new SqlParameter("@CreateTime",model.CreateTime) ,  //操作时间   
                                        new SqlParameter("@CompanyId",model.CompanyId)   //系统公司id   
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 修改 系统日志
        /// <summary>
        /// 修改 系统日志
        /// </summary>
        public int UpdateSysLog (SysLogModel model)
        {
           string sql = @"Update 
                                  SysLog
                          Set
                                  LogId = @LogId ,
                                  Operator = @Operator ,
                                  Message = @Message ,
                                  Result = @Result ,
                                  Type = @Type ,
                                  Module = @Module ,
                                  CreateTime = @CreateTime ,
                                  CompanyId = @CompanyId 
                          Where         
                                  LogId = @LogId
 ";                                    
                                  
            SqlParameter[] param ={ 
                                        new SqlParameter("@LogId",model.LogId) ,  //主键id   
                                        new SqlParameter("@Operator",model.Operator) ,  //操作人   
                                        new SqlParameter("@Message",model.Message) ,  //信息   
                                        new SqlParameter("@Result",model.Result) ,  //结果   
                                        new SqlParameter("@Type",model.Type) ,  //类型（按钮）   
                                        new SqlParameter("@Module",model.Module) ,  //功能菜单   
                                        new SqlParameter("@CreateTime",model.CreateTime) ,  //操作时间   
                                        new SqlParameter("@CompanyId",model.CompanyId)   //系统公司id   
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 删除 系统日志
        /// <summary>
        /// 删除 系统日志
        /// </summary>
        public int DeleteSysLogByID (string id)
        {
           string sql = @"Delete 
                                  SysLog
                          Where         
                                  LogId = @LogId
 ";                                    
                                  
            SqlParameter[] param ={ 
                                        new SqlParameter("@LogId",id)     
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion 

        #region 分页列表 系统日志
        /// <summary>
        ///  分页列表 系统日志
        /// </summary>
        public List<SysLogModel> SysLogPageList(int index, int size, string where)
        {
           string sql = @" With TemTable As 
                            (
                                Select Row_Number() Over(Order By CreateTime desc) AS 't',*
                                From
                                       SysLog
                                Where
                                       " + where + @"
                            )
                                  
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            SqlParameter[] param ={ 
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };

            List<SysLogModel> list = new List<SysLogModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysLogModel model = new SysLogModel();
                model.LogId = Convert.ToInt32(dr["LogId"].ToString()); //主键id
                model.Operator = dr["Operator"].ToString(); //操作人
               // model.Message = dr["Message"].ToString(); //信息
                model.Result = dr["Result"].ToString(); //结果
                model.Type = dr["Type"].ToString(); //类型（按钮）
                model.Module = dr["Module"].ToString(); //功能菜单
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //操作时间
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id

                list.Add(model);
            }
            dr.Close();

            return list;
        }
        public List<SysLogModel> SysLogPageLists(int index, int size, string where,string moduleid)
        {
            var sql = @" WITH cteTree  AS
                    (    
                               SELECT ModuleId,ParentId, CAST(moduleid as nvarchar(80)) as module FROM SysModule
                               WHERE ModuleId = @moduleid 
                               UNION ALL
                               SELECT SysModule.ModuleId,SysModule.ParentId,CAST(SysModule.ModuleId as nvarchar(80)) as module   
                               FROM cteTree INNER JOIN SysModule
                               ON cteTree.ModuleId = SysModule.ParentId
                    ) ,
                    TemTable As 
                            (
                                Select Row_Number() Over(Order By l.CreateTime desc) AS 't',l.*,c.CompanyName
                                From
                                       SysLog l,SysCompany c,cteTree
                                Where
                                       l.CompanyId=c.CompanyId and l.module=cteTree.module and   " + where + @" 
                            )
                                  
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";

            SqlParameter[] param ={ 
                                   new SqlParameter("@moduleid",moduleid),
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };

            List<SysLogModel> list = new List<SysLogModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysLogModel model = new SysLogModel();
                model.LogId = Convert.ToInt32(dr["LogId"].ToString()); //主键id
                model.Operator = dr["Operator"].ToString(); //操作人
                // model.Message = dr["Message"].ToString(); //信息
                model.Result = dr["Result"].ToString(); //结果
                model.Type = dr["Type"].ToString(); //类型（按钮）
                model.Module = dr["Module"].ToString(); //功能菜单
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //操作时间
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
                model.CompanyName = dr["CompanyName"].ToString();
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        public List<SysLogModel> SysLogPageLists(int index, int size, string where)
        {

            string sql = @" With TemTable As 
                            (
                                Select Row_Number() Over(Order By l.CreateTime desc) AS 't',l.*,c.CompanyName
                                From
                                       SysLog l,SysCompany c
                                Where
                                       l.CompanyId=c.CompanyId and   " + where + @" 
                            )
                                  
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            SqlParameter[] param ={ 
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };

            List<SysLogModel> list = new List<SysLogModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysLogModel model = new SysLogModel();
                model.LogId = Convert.ToInt32(dr["LogId"].ToString()); //主键id
                model.Operator = dr["Operator"].ToString(); //操作人
                // model.Message = dr["Message"].ToString(); //信息
                model.Result = dr["Result"].ToString(); //结果
                model.Type = dr["Type"].ToString(); //类型（按钮）
                model.Module = dr["Module"].ToString(); //功能菜单
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //操作时间
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
                model.CompanyName = dr["CompanyName"].ToString();
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion  

        #region 分页总数 系统日志
        /// <summary>
        ///  分页总数 系统日志
        /// </summary>
        public int  SysLogPageCounts(string where)
        {
            string sql = @" Select 
                                    Count(l.CompanyId) 
                            From
                                      SysLog l,SysCompany c
                                Where
                                       l.CompanyId=c.CompanyId and  " + where;

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

        public int SysLogPageCounts(string where,string moduleid)
        {
            string sql = @" WITH cteTree  AS
                    (    
                               SELECT ModuleId,ParentId, CAST(moduleid as nvarchar(80)) as module FROM SysModule
                               WHERE ModuleId = @moduleid 
                               UNION ALL
                               SELECT SysModule.ModuleId,SysModule.ParentId,CAST(SysModule.ModuleId as nvarchar(80)) as module   
                               FROM cteTree INNER JOIN SysModule
                               ON cteTree.ModuleId = SysModule.ParentId
                    ) 
                           Select 
                                    Count(l.CompanyId) 
                           From
                                    SysLog l,SysCompany c,cteTree
                           Where
                                    l.CompanyId=c.CompanyId and l.module=cteTree.module and  " + where;

            SqlParameter[] parms = {
                                       new SqlParameter("@moduleid",moduleid)
                                   };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, parms);
            if (obj != null)
            {
                return Convert.ToInt32(obj.ToString());
            }
            else
            {
                return 0;
            }            
        }

        public int SysLogPageCount(string where)
        {
            string sql = @" Select 
                                    Count(*) 
                            From
                                    SysLog
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

        #region 获取实体 系统日志
        /// <summary>
        ///  获取实体 系统日志
        /// </summary>
        public SysLogModel  GetModelByID (string id)
        {
           string sql = @" Select 
                                  LogId ,
                                  Operator ,
                                  Message ,
                                  Result ,
                                  Type ,
                                  Module ,
                                  CreateTime ,
                                  CompanyId 
                            From
                                  SysLog
                            Where
                                  LogId=@LogId";
             SqlParameter[] param ={ 
                                   new SqlParameter("@LogId",id)
                                  };
                                  
             SysLogModel model=null;
             
             SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
             if (dr.Read())
              {
                     model = new SysLogModel();
                     model.LogId= Convert.ToInt32(dr["LogId"].ToString()); //主键id
                     model.Operator= dr["Operator"].ToString(); //操作人
                     model.Message= dr["Message"].ToString(); //信息
                     model.Result= dr["Result"].ToString(); //结果
                     model.Type= dr["Type"].ToString(); //类型（按钮）
                     model.Module= dr["Module"].ToString(); //功能菜单
                     model.CreateTime= Convert.ToDateTime(dr["CreateTime"].ToString()); //操作时间
                     model.CompanyId= Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
            }           
             dr.Close();

             return model;
        }
        #endregion

        #region 登陆日志分页列表
        public List<SysLogModel> SysLoginPageList(int index, int size, string where)
        {
            string sql = @" With TemTable As 
                            (
                                Select Row_Number() Over(Order By l.CreateTime desc) AS 't',l.*,isnull(c.CompanyName,'无') as CompanyName
                                From
                                       SysLog l left join SysCompany c
                                on
                                       l.CompanyId=c.CompanyId Where " + where + @"
                            )
                                  
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };

            List<SysLogModel> list = new List<SysLogModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysLogModel model = new SysLogModel();
                model.LogId = Convert.ToInt32(dr["LogId"].ToString()); //主键id
                model.Operator = dr["Operator"].ToString(); //操作人
                model.Result = dr["Result"].ToString(); //结果
                model.Type = dr["Type"].ToString(); //类型（按钮）
                model.Module = dr["Module"].ToString(); //功能菜单
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //操作时间
                model.CompanyName = dr["CompanyName"].ToString(); //系统公司

                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 登陆日志分页总数
        public int SysLoginPageCount(string where)
        {
            string sql = @" Select 
                                    Count(*) 
                            From
                                    SysLog l left join SysCompany c
                            on
                                    l.CompanyId=c.CompanyId Where " + where + "";
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
    }
}

