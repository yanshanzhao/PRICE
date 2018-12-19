//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-12    1.0        HDS        新建               
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
 * 类名：SysRoleDAL
 * 功能描述：系统角色 数据访问层类
 * ******************************/
namespace DAL.Sys
{
 public class SysRoleDAL
    {  
     // 获取连接串
     string conn =DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 批量添加角色信息
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

        #region 添加 系统角色
        /// <summary>
        /// 添加 系统角色
        /// </summary>
        public int AddSysRole (SysRoleModel model)
        {
           string sql = @"Insert 
                                  SysRole
                                  (
                                        RoleName ,
                                        State ,
                                        CreateTime ,
                                        CreatePerson ,
                                        Remark ,
                                        CompanyId 
                                      
                                  )
                          Values
                                  (
                                        @RoleName ,
                                        @State ,
                                        @CreateTime ,
                                        @CreatePerson ,
                                        @Remark ,
                                        @CompanyId 
                                  )";
            SqlParameter[] param ={ 
                                        new SqlParameter("@RoleName",model.RoleName) ,  //角色名称   
                                        new SqlParameter("@State",model.State) ,  //状态：0-无效;1-有效   
                                        new SqlParameter("@CreateTime",model.CreateTime) ,  //创建时间   
                                        new SqlParameter("@CreatePerson",model.CreatePerson) ,  //创建人   
                                        new SqlParameter("@Remark",model.Remark) ,  //说明   
                                        new SqlParameter("@CompanyId",model.CompanyId)   //系统公司id   
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 修改 系统角色
        /// <summary>
        /// 修改 系统角色
        /// </summary>
        public int UpdateSysRole (SysRoleModel model)
        {
           string sql = @"Update 
                                  SysRole
                          Set
                                  RoleName = @RoleName ,
                                  State = @State ,
                                  CreateTime = @CreateTime ,
                                  CreatePerson = @CreatePerson ,
                                  Remark = @Remark ,
                                  CompanyId = @CompanyId 
                          Where         
                                  RoleId = @RoleId
 ";                                    
                                  
            SqlParameter[] param ={ 
                                        new SqlParameter("@RoleId",model.RoleId) ,  //角色id自增   
                                        new SqlParameter("@RoleName",model.RoleName) ,  //角色名称   
                                        new SqlParameter("@State",model.State) ,  //状态：0-无效;1-有效   
                                        new SqlParameter("@CreateTime",model.CreateTime) ,  //创建时间   
                                        new SqlParameter("@CreatePerson",model.CreatePerson) ,  //创建人   
                                        new SqlParameter("@Remark",model.Remark) ,  //说明   
                                        new SqlParameter("@CompanyId",model.CompanyId)   //系统公司id   
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 删除 系统角色
        /// <summary>
        /// 删除 系统角色
        /// </summary>
        public int DeleteSysRoleByID (string id)
        {
           string sql = @"Delete 
                                  SysRole
                          Where         
                                  RoleId = @RoleId
 ";                                    
                                  
            SqlParameter[] param ={ 
                                        new SqlParameter("@RoleId",id)     
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion 

        #region 列表 系统角色
        /// <summary>
        /// 列表 系统角色
        /// </summary>
        public List<SysRoleModel> SysRoleList(string cid,string where)
        {
            string sql = @" Select 
                                  RoleId ,
                                  RoleName ,
                                  State ,
                                  CreateTime ,
                                  CreatePerson ,
                                  Remark ,
                                  CompanyId ,
                                  IsSystem
                            From
                                  SysRole
                            Where
                                  CompanyId=@CompanyId  And  Rolename like'%" + where + "%'";

            SqlParameter[] param ={
                                   new SqlParameter("@CompanyId",cid)
                                  };

            List<SysRoleModel> list = new List<SysRoleModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while(dr.Read())
            {
                SysRoleModel model = new SysRoleModel();
                model.RoleId = Convert.ToInt32(dr["RoleId"].ToString()); //角色id自增
                model.RoleName = dr["RoleName"].ToString(); //角色名称
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                model.CreatePerson = dr["CreatePerson"].ToString(); //创建人
                model.Remark = dr["Remark"].ToString(); //说明
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
                model.IsSystem = Convert.ToInt32(dr["IsSystem"].ToString());
                list.Add(model);
            }
            dr.Close();

            return list;  
        }
        #endregion  

        #region 获取实体 系统角色
        /// <summary>
        ///  获取实体 系统角色
        /// </summary>
        public SysRoleModel  GetModelByID (string id)
        {
           string sql = @" Select 
                                  RoleId ,
                                  RoleName ,
                                  State ,
                                  CreateTime ,
                                  CreatePerson ,
                                  Remark ,
                                  CompanyId 
                            From
                                  SysRole
                            Where
                                  RoleId=@RoleId";
             SqlParameter[] param ={ 
                                   new SqlParameter("@RoleId",id)
                                  };
                                  
             SysRoleModel model=null;
             
             SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
             if (dr.Read())
              {
                     model = new SysRoleModel();
                     model.RoleId= Convert.ToInt32(dr["RoleId"].ToString()); //角色id自增
                     model.RoleName= dr["RoleName"].ToString(); //角色名称
                     model.State= Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效
                     model.CreateTime= Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                     model.CreatePerson= dr["CreatePerson"].ToString(); //创建人
                     model.Remark= dr["Remark"].ToString(); //说明
                     model.CompanyId= Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
            }           
             dr.Close();

             return model;
        }
        #endregion

        #region 是否存在相同的角色
        public int IsHasRole(int id, string role)
        {
            if (id == 0)
            {
                var sql = "Select Count(*) From SysRole Where RoleName=@RoleName ";
                SqlParameter[] param ={
                                 new SqlParameter("@RoleName",role)
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
                var sql = "Select Count(*) From SysRole Where RoleName=@RoleName And RoleId!=@RoleId";
                SqlParameter[] param ={
                                 new SqlParameter("@RoleName",role),
                                 new SqlParameter("@RoleId",id)
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

        #region 用户使用角色数量
        public int GetUserRoleCountById(string roleid)
        {
            var sql = "Select Count(*) From SysUserRole Where RoleId=@id";

            SqlParameter[] parm =
            {
                new SqlParameter("@id",roleid)
            };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, parm);
            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }
        #endregion

        #region 角色删除(仅授权信息）
        public int DeleteRoleOfAuth(string roleid)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();

                var sql = "Delete SysRoleModule Where RoleId=@RoleId";
                SqlParameter[] param = { new SqlParameter("@RoleId", roleid) };
                int obj = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, sql, param);

                string del = @"Delete SysRoleOperate Where RoleId=@RoleId";
                int result = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, del, param);

                tran.Commit();
                return 1;
            }
        }
        #endregion

        #region 角色删除（所有)
        public int DeleteRole(string roleid)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();

                var sql = "Delete SysRoleModule Where RoleId=@RoleId";
                SqlParameter[] param = { new SqlParameter("@RoleId", roleid) };
                int obj = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, sql, param);

                //if (obj < 1)
                //{
                //    tran.Rollback();
                //    return 0;
                //}

                string del = @"Delete SysRoleOperate Where RoleId=@RoleId";
                int result = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, del, param);
                //if (result < 1)
                //{
                //    tran.Rollback();
                //    return 0;
                //}

                string sqls = @"Delete SysRole Where RoleId=@RoleId";
                int res = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, sqls, param);
                if (res < 1)
                {
                    tran.Rollback();
                    return 0;
                }

                tran.Commit();
                return 1;
            }
        }
        #endregion

        #region 授权角色主键列表
        public List<string> AuthRoleList(string cid)
        {
            var sql = @"Select 
                            Distinct r.RoleId
                      From
                            SysRole r,SysRoleModule rm
                      Where 
                            r.RoleId=rm.RoleId And r.CompanyId= @CompanyId";

            SqlParameter[] param ={
                                 new SqlParameter("@CompanyId",cid)
                                 };

            List<string> list = new List<string>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);

            while (dr.Read())
            {
                list.Add(dr[0].ToString());
            }

            dr.Close();

            return list;
        }
        #endregion

        #region 已授权角色菜单列表
        public List<string> AuthRoleModuleList(string cid)
        {
            var sql = @"Select 
                             rm.ModuleId
                      From
                            SysRole r,SysRoleModule rm
                      Where 
                            r.RoleId=rm.RoleId And r.RoleId= @RoleId";

            SqlParameter[] param ={
                                 new SqlParameter("@RoleId",cid)
                                 };

            List<string> list = new List<string>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);

            while (dr.Read())
            {
                list.Add(dr[0].ToString());
            }

            dr.Close();

            return list;
        }
        #endregion

        #region 已授权角色按钮列表
        public List<ModOperates> AuthRoleModuleOperList(string cid)
        {
            var sql = @"Select 
                             ro.OperateId,ro.ModuleId
                      From
                            SysRole r,SysRoleOperate ro
                      Where 
                            r.RoleId=ro.RoleId And r.RoleId= @RoleId";

            SqlParameter[] param ={
                                 new SqlParameter("@RoleId",cid)
                                 };

            List<ModOperates> list = new List<ModOperates>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);

            while (dr.Read())
            {
                ModOperates model = new ModOperates();

                model.modid = dr[1].ToString();
                model.operid = dr[0].ToString();

                list.Add(model);
            }

            dr.Close();

            return list;
        }
        #endregion

        #region 变更 角色状态
        public int ChangeState(int state, string rid)
        {
            string sql = @"Update 
                                  SysRole
                          Set                     
                                  State = @State 
                          Where         
                                  RoleId = @RoleId
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@RoleId",rid) ,  //用户id自增主键    
                                        new SqlParameter("@State",state)   //状态：0-无效;1-有效   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 获取实体 系统角色默认角色
        /// <summary>
        ///  获取实体 系统角色默认角色
        /// </summary>
        public SysRoleModel GetDefaultInfo(string cid)
        {
            string sql = @" Select 
                                  RoleId ,
                                  RoleName ,
                                  Remark 
                            From
                                  SysRole
                            Where
                                  CompanyId=@CompanyId And IsSystem=1";
            SqlParameter[] param ={ 
                                   new SqlParameter("@CompanyId",cid)
                                  };

            SysRoleModel model = null;

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            if (dr.Read())
            {
                model = new SysRoleModel();
                model.RoleId = Convert.ToInt32(dr["RoleId"].ToString()); //角色id自增
                model.RoleName = dr["RoleName"].ToString(); //角色名称                  
                model.Remark = dr["Remark"].ToString(); //说明
            }
            dr.Close();

            return model;
        }
        #endregion
    }
}

