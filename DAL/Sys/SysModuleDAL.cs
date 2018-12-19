//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-09    1.0         MH        新建
//2018-09-19    1.1         HDS       增加字段ModuleType 
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
 * 类名：SysModuleDAL
 * 功能描述：系统功能(菜单) 数据访问层类
 * ******************************/
namespace DAL.Sys
{
    public class SysModuleDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();


        #region 添加 系统功能(菜单)
        /// <summary>
        /// 添加 系统功能(菜单)
        /// </summary>
        public int AddSysModule(SysModuleModel model)
        {
            string sql = @"INSERT INTO
                                  SysModule
                                  (
                                        ModuleName ,
                                        EnglishName ,
                                        ParentId ,
                                        Url ,
                                        IsAdmin,
                                        Iconic ,
                                        Sort ,
                                        Remark ,
                                        CreatePerson ,
                                        CreateTime,ModuleType
                                      
                                  )
                          Values
                                  (
                                        @ModuleName ,
                                        @EnglishName ,
                                        @ParentId ,
                                        @Url ,
                                        @IsAdmin,
                                        @Iconic ,
                                        @Sort ,
                                        @Remark ,
                                        @CreatePerson ,
                                        @CreateTime ,@ModuleType
                                  )";
            SqlParameter[] param ={
                                        new SqlParameter("@IsAdmin",model.IsAdmin) ,
                                        new SqlParameter("@ModuleName",model.ModuleName) ,  //功能名称   
                                        new SqlParameter("@EnglishName",model.EnglishName) ,  //功能别名   
                                        new SqlParameter("@ParentId",model.ParentId) ,  //上级ID   
                                        new SqlParameter("@Url",model.Url) ,  //链接   
                                        new SqlParameter("@Iconic",model.Iconic) ,  //图标   
                                        new SqlParameter("@Sort",model.Sort) ,  //排序号   
                                        new SqlParameter("@Remark",model.Remark) ,  //说明   
                                        new SqlParameter("@CreatePerson",model.CreatePerson) ,  //创建人   
                                        new SqlParameter("@CreateTime",model.CreateTime),   //创建时间   
                                        new SqlParameter("@ModuleType",model.ModuleType)   //功能类型:0-系统菜单;1-公示菜单
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 修改 系统功能(菜单)
        /// <summary>
        /// 修改 系统功能(菜单)
        /// </summary>
        public int UpdateSysModule(SysModuleModel model)
        {
            string sql = @"Update 
                                  SysModule
                          Set
                                  ModuleName = @ModuleName ,
                                  EnglishName = @EnglishName ,
                                  ParentId = @ParentId ,
                                  Url = @Url ,
                                  Iconic = @Iconic ,
                                  Sort = @Sort ,
                                  Remark = @Remark ,
                                  CreatePerson = @CreatePerson ,
                                  CreateTime = @CreateTime ,ModuleType=@ModuleType
                          Where         
                                  ModuleId = @ModuleId
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@ModuleId",model.ModuleId) ,  //主键;自增   
                                        new SqlParameter("@ModuleName",model.ModuleName) ,  //功能名称   
                                        new SqlParameter("@EnglishName",model.EnglishName) ,  //功能别名   
                                        new SqlParameter("@ParentId",model.ParentId) ,  //上级ID   
                                        new SqlParameter("@Url",model.Url) ,  //链接   
                                        new SqlParameter("@Iconic",model.Iconic) ,  //图标   
                                        new SqlParameter("@Sort",model.Sort) ,  //排序号   
                                        new SqlParameter("@Remark",model.Remark) ,  //说明   
                                        new SqlParameter("@CreatePerson",model.CreatePerson) ,  //创建人   
                                        new SqlParameter("@CreateTime",model.CreateTime),   //创建时间   
                                        new SqlParameter("@ModuleType",model.ModuleType)   //功能类型:0-系统菜单;1-公示菜单
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 删除 系统功能(菜单)

        public int SysRoleMuduleCount(string id)
        {
            string sql = @"   WITH cteTree  AS
                            (    
                              SELECT * FROM SysModule
                               WHERE ModuleId = @ID  
                               UNION ALL
                              SELECT SysModule.*    
                                FROM cteTree INNER JOIN SysModule
                                  ON cteTree.ModuleId = SysModule.ParentId
                             ) 
                              Select Count(d.ModuleId) From cteTree d,SysRoleOperate u
                               WHERE d.ModuleId=u.ModuleId 
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@ID",id)
                                  };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }

        /// <summary>
        /// 删除 系统功能(菜单)
        /// </summary>
        public int DeleteSysModuleByID(string id)
        {

            SqlParameter[] param ={
                                        new SqlParameter("@ID",id)
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "DeleteSysModule", param);
            return i;
        }
        #endregion 

        #region 后台管理员菜单列表
        /// <summary>
        ///  后台管理员菜单列表
        /// </summary>
        public List<SysModuleModel> SysModulePageList(string where)
        {
            string sql = @" Select 
                                  ModuleId ,
                                  ModuleName ,
                                  EnglishName ,
                                  ParentId ,
                                  Url ,
                                  Iconic ,
                                  Sort ,
                                  Remark ,
                                  CreatePerson ,
                                  CreateTime ,
                                  IsAdmin,isnull(ModuleType,0) as ModuleType 
                            From
                                  SysModule
                            Where  
                                  ModuleName+EnglishName like '%" + where + "%'  Order by ParentId,Sort";

            List<SysModuleModel> list = new List<SysModuleModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            while (dr.Read())
            {
                SysModuleModel model = new SysModuleModel();
                model.ModuleId = Convert.ToInt32(dr["ModuleId"].ToString()); //主键;自增
                model.ModuleName = dr["ModuleName"].ToString(); //功能名称
                model.EnglishName = dr["EnglishName"].ToString(); //功能别名
                model.ParentId = Convert.ToInt32(dr["ParentId"].ToString()); //上级ID
                model.Url = dr["Url"].ToString(); //链接
                model.Iconic = dr["Iconic"].ToString(); //图标
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序号
                model.Remark = dr["Remark"].ToString(); //说明
                model.CreatePerson = dr["CreatePerson"].ToString(); //创建人
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                model.IsAdmin = Convert.ToInt32(dr["IsAdmin"].ToString());
                model.ModuleType = Convert.ToInt32(dr["ModuleType"].ToString());//功能类型:0-系统菜单;1-公示菜单
               
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 非后台管理员菜单列表
        /// <summary>
        ///  非后台管理员菜单列表
        /// </summary>
        public List<SysModuleModel> SysModulePageList()
        {
            string sql = @" Select 
                                  ModuleId ,
                                  ModuleName ,
                                  EnglishName ,
                                  ParentId ,
                                  Url ,
                                  Iconic ,
                                  Sort ,
                                  Remark ,
                                  CreatePerson ,
                                  CreateTime ,
                                  IsAdmin,isnull(ModuleType,0) as ModuleType 
                            From
                                  SysModule
                            Where  
                                  IsAdmin=0  Order by ParentId,Sort";

            List<SysModuleModel> list = new List<SysModuleModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            while (dr.Read())
            {
                SysModuleModel model = new SysModuleModel();
                model.ModuleId = Convert.ToInt32(dr["ModuleId"].ToString()); //主键;自增
                model.ModuleName = dr["ModuleName"].ToString(); //功能名称
                model.EnglishName = dr["EnglishName"].ToString(); //功能别名
                model.ParentId = Convert.ToInt32(dr["ParentId"].ToString()); //上级ID
                model.Url = dr["Url"].ToString(); //链接
                model.Iconic = dr["Iconic"].ToString(); //图标
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序号
                model.Remark = dr["Remark"].ToString(); //说明
                model.CreatePerson = dr["CreatePerson"].ToString(); //创建人
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                model.IsAdmin = Convert.ToInt32(dr["IsAdmin"].ToString());
                model.ModuleType = Convert.ToInt32(dr["ModuleType"].ToString());//功能类型:0-系统菜单;1-公示菜单
                

                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 获取实体 系统功能(菜单)
        /// <summary>
        ///  获取实体 系统功能(菜单)
        /// </summary>
        public SysModuleModel GetModelByID(string id)
        {
            string sql = @" Select 
                                  ModuleId ,
                                  ModuleName ,
                                  EnglishName ,
                                  ParentId ,
                                  Url ,
                                  Iconic ,
                                  Sort ,
                                  Remark ,
                                  CreatePerson ,
                                  CreateTime ,isnull(ModuleType,0) as ModuleType 
                            From
                                  SysModule
                            Where
                                  ModuleId=@ModuleId";
            SqlParameter[] param ={
                                   new SqlParameter("@ModuleId",id)
                                  };

            SysModuleModel model = null;

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            if (dr.Read())
            {
                model = new SysModuleModel();
                model.ModuleId = Convert.ToInt32(dr["ModuleId"].ToString()); //主键;自增
                model.ModuleName = dr["ModuleName"].ToString(); //功能名称
                model.EnglishName = dr["EnglishName"].ToString(); //功能别名
                model.ParentId = Convert.ToInt32(dr["ParentId"].ToString()); //上级ID
                model.Url = dr["Url"].ToString(); //链接
                model.Iconic = dr["Iconic"].ToString(); //图标
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序号
                model.Remark = dr["Remark"].ToString(); //说明
                model.CreatePerson = dr["CreatePerson"].ToString(); //创建人
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                model.ModuleType = Convert.ToInt32(dr["ModuleType"].ToString());//功能类型:0-系统菜单;1-公示菜单
                
            }
            dr.Close();

            return model;
        }
        #endregion

        #region 是否含有相同菜单
        public int IsHasModule(int id, int pid, string name)
        {
            string sql = string.Empty;

            if (id == 0)
            {
                sql = "Select Count(*) From SysModule Where ParentId=@ParentId And ModuleName=@ModuleName";

                SqlParameter[] param ={
                                   new SqlParameter("@ParentId",pid),
                                   new SqlParameter("@ModuleName",name.Trim())
                                  };
                object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
                if (obj == null)
                {
                    return 0;
                }
                return Convert.ToInt32(obj.ToString());
            }
            else
            {
                sql = "Select Count(*) From SysModule Where  ParentId=@ParentId And ModuleName=@ModuleName And ModuleId!=@ModuleId";

                SqlParameter[] param ={
                                   new SqlParameter("@ParentId",pid),
                                   new SqlParameter("@ModuleName",name.Trim()),
                                   new SqlParameter("@ModuleId",id)
                                  };
                object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
                if (obj == null)
                {
                    return 0;
                }
                return Convert.ToInt32(obj.ToString());
            }
        }
        #endregion

        #region 批量添加菜单按钮
        public bool BulkCopy(DataTable dt, string tablename)
        {
            using (SqlConnection conns = new SqlConnection(conn))
            {
                conns.Open();
                using (SqlTransaction tran = conns.BeginTransaction())
                {
                    using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conns, SqlBulkCopyOptions.FireTriggers, tran))
                    {
                        bulkcopy.BatchSize = 2000;
                        bulkcopy.BulkCopyTimeout = 60;
                        bulkcopy.DestinationTableName = tablename;


                        try
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                bulkcopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                            }

                            bulkcopy.WriteToServer(dt);
                            tran.Commit();

                            return true;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            return false;
                        }
                        finally
                        {
                            conns.Close();
                        }
                    }
                }
            }
        }

        #endregion

        #region 删除菜单按钮
        public int DeleteModuleOperate(string modid)
        {
            var sql = "Delete From SysModuleOperate Where ModuleId=@ModuleId";

            SqlParameter[] param ={
                                   new SqlParameter("@ModuleId",modid)
                                  };

            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 已选择菜单按钮列表
        public List<TreeModel> GetSelModuleOperList()
        {
            var sql = "Select mo.ModuleId,o.OperateName  From SysModuleOperate mo,SysOperate o Where mo.OperateId=o.OperateId Order By o.State";

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);

            List<TreeModel> list = new List<TreeModel>();

            while (dr.Read())
            {
                TreeModel model = new TreeModel();
                model.id = dr["ModuleId"].ToString();
                model.name = dr["OperateName"].ToString();

                list.Add(model);
            }
            dr.Close();

            return list;
        }

        public List<TreeModel> GetSelModuleOperLists()
        {
            var sql = "Select mo.ModuleId,o.OperateId,o.OperateName  From SysModuleOperate mo,SysOperate o Where mo.OperateId=o.OperateId Order By o.State";

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);

            List<TreeModel> list = new List<TreeModel>();

            while (dr.Read())
            {
                TreeModel model = new TreeModel();
                model.pid = dr["ModuleId"].ToString();
                model.id = dr["OperateId"].ToString();
                model.name = dr["OperateName"].ToString();

                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 变更 菜单状态
        /// <summary>
        /// 递归模式更新状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="did"></param>
        /// <returns></returns>
        public int ChangeState(int state, string did)
        {
            string sql = @"   WITH cteTree  AS
                            (    
                              SELECT * FROM SysModule
                               WHERE ModuleId = @ID  
                               UNION ALL
                              SELECT SysModule.*    
                                FROM cteTree INNER JOIN SysModule
                                  ON cteTree.ModuleId = SysModule.ParentId
                             ) 
                              UPDATE dbo.SysModule 
                                 SET [IsAdmin]=@State 
                                FROM dbo.SysModule s,cteTree
                               WHERE s.ModuleId=cteTree.ModuleId 
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@ID",did) ,  //用户id自增主键    
                                        new SqlParameter("@State",state)   //状态：0-无效;1-有效   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion


        #region 变更 菜单状态
        /// <summary>
        /// 递归模式更新公示状态
        /// </summary>
        /// <param name="type"></param>
        /// <param name="did"></param>
        /// <returns></returns>
        public int ChangeType(int type, string did)
        {
            string sql = @"   WITH cteTree  AS
                            (    
                              SELECT * FROM SysModule
                               WHERE ModuleId = @ID  
                               UNION ALL
                              SELECT SysModule.*    
                                FROM cteTree INNER JOIN SysModule
                                  ON cteTree.ModuleId = SysModule.ParentId
                             ) 
                              UPDATE dbo.SysModule 
                                 SET [ModuleType]=@ModuleType 
                                FROM dbo.SysModule s,cteTree
                               WHERE s.ModuleId=cteTree.ModuleId 
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@ID",did) ,  //用户id自增主键    
                                        new SqlParameter("@ModuleType",type)   //状态：0-无效;1-有效   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion
    }
}
