//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-23    1.0        MH         新建
//2018-05-15    1.0        MH         用户角色处理
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
 * 类名：SysUserDAL
 * 功能描述：系统用户表 数据访问层类
 * ******************************/
namespace DAL.Sys
{
 public class SysUserDAL
    {  
     // 获取连接串
     string conn =DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 批量添加用户角色
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

        #region 添加 系统用户表
        /// <summary>
        /// 添加 系统用户表
        /// </summary>
        public int AddSysUser (SysUserModel model)
        {
           string sql = @"Insert 
                                  SysUser
                                  (
                                        UserName ,
                                        UserSpelling ,
                                        Password ,
                                        TrueName ,
                                        MobileNumber ,
                                        PhoneNumber ,
                                        QQ ,
                                        EmailAddress ,
                                        OtherContact ,
                                        Province ,
                                        City ,
                                        Village ,
                                        Address ,
                                        Sex ,
                                        Birthday ,
                                        JoinDate ,
                                        Native ,
                                        DepartmentId ,
                                        Expertise ,
                                        JobState ,
                                        Photo ,
                                        Attach ,
                                        CompanyId ,
                                        CreateTime ,
                                        CreatePerson ,
                                        State 
                                      
                                  )
                          Values
                                  (
                                        @UserName ,
                                        @UserSpelling ,
                                        @Password ,
                                        @TrueName ,
                                        @MobileNumber ,
                                        @PhoneNumber ,
                                        @QQ ,
                                        @EmailAddress ,
                                        @OtherContact ,
                                        @Province ,
                                        @City ,
                                        @Village ,
                                        @Address ,
                                        @Sex ,
                                        @Birthday ,
                                        @JoinDate ,
                                        @Native ,
                                        @DepartmentId ,
                                        @Expertise ,
                                        @JobState ,
                                        @Photo ,
                                        @Attach ,
                                        @CompanyId ,
                                        @CreateTime ,
                                        @CreatePerson ,
                                        @State 
                                  )";
            SqlParameter[] param ={   
                                        new SqlParameter("@UserName",model.UserName) ,  //登录用户   
                                        new SqlParameter("@UserSpelling",model.UserSpelling) ,  //用户拼写   
                                        new SqlParameter("@Password",model.Password) ,  //登录密码   
                                        new SqlParameter("@TrueName",model.TrueName??string.Empty) ,  //真实名称   
                                        new SqlParameter("@MobileNumber",model.MobileNumber??string.Empty) ,  //手机号码   
                                        new SqlParameter("@PhoneNumber",model.PhoneNumber??string.Empty) ,  //电话号码   
                                        new SqlParameter("@QQ",model.QQ??string.Empty) ,  //QQ   
                                        new SqlParameter("@EmailAddress",model.EmailAddress??string.Empty) ,  //Email   
                                        new SqlParameter("@OtherContact",model.OtherContact??string.Empty) ,  //其他联系方式   
                                        new SqlParameter("@Province",model.Province??string.Empty) ,  //省份   
                                        new SqlParameter("@City",model.City??string.Empty) ,  //城市   
                                        new SqlParameter("@Village",model.Village??string.Empty) ,  //地区   
                                        new SqlParameter("@Address",model.Address??string.Empty) ,  //详细地址   
                                        new SqlParameter("@Sex",model.Sex??string.Empty) ,  //型别   
                                        new SqlParameter("@Birthday",model.Birthday??string.Empty) ,  //出生日期   
                                        new SqlParameter("@JoinDate",model.JoinDate??string.Empty) ,  //入职日期   
                                        new SqlParameter("@Native",model.Native??string.Empty) ,  //籍贯   
                                        new SqlParameter("@DepartmentId",model.DepartmentId) ,  //组织机构id   
                                        new SqlParameter("@Expertise",model.Expertise??string.Empty) ,  //个人简介   
                                        new SqlParameter("@JobState",model.JobState) ,  //在职状况1在职，0离职   
                                        new SqlParameter("@Photo",model.Photo??string.Empty) ,  //照片   
                                        new SqlParameter("@Attach",model.Attach??string.Empty) ,  //附件   
                                        new SqlParameter("@CompanyId",model.CompanyId) ,  //系统公司id   
                                        new SqlParameter("@CreateTime",model.CreateTime) ,  //创建时间   
                                        new SqlParameter("@CreatePerson",model.CreatePerson) ,  //创建人   
                                        new SqlParameter("@State",model.State)   //状态：0-无效;1-有效 ;2-作废  
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 修改 系统用户表
        /// <summary>
        /// 修改 系统用户表
        /// </summary>
        public int UpdateSysUser (SysUserModel model)
        {
           string sql = @"Update 
                                  SysUser
                          Set
                                  TrueName = @TrueName ,
                                  MobileNumber = @MobileNumber ,
                                  PhoneNumber = @PhoneNumber ,
                                  QQ = @QQ ,
                                  EmailAddress = @EmailAddress ,
                                  OtherContact = @OtherContact ,
                                  Province = @Province ,
                                  City = @City ,
                                  Village = @Village ,
                                  Address = @Address ,
                                  Sex = @Sex ,
                                  Birthday = @Birthday ,
                                  JoinDate = @JoinDate ,
                                  Native = @Native ,
                                  DepartmentId = @DepartmentId ,
                                  Expertise = @Expertise ,
                                  JobState = @JobState ,
                                  Photo = @Photo ,
                                  Attach = @Attach ,
                                  CreateTime = @CreateTime ,
                                  CreatePerson = @CreatePerson ,
                                  State = @State 
                          Where         
                                  UserId = @UserId
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@UserId",model.UserId) ,  //用户id自增主键    
                                        new SqlParameter("@TrueName",model.TrueName??string.Empty) ,  //真实名称   
                                        new SqlParameter("@MobileNumber",model.MobileNumber??string.Empty) ,  //手机号码   
                                        new SqlParameter("@PhoneNumber",model.PhoneNumber??string.Empty) ,  //电话号码   
                                        new SqlParameter("@QQ",model.QQ??string.Empty) ,  //QQ   
                                        new SqlParameter("@EmailAddress",model.EmailAddress??string.Empty) ,  //Email   
                                        new SqlParameter("@OtherContact",model.OtherContact??string.Empty) ,  //其他联系方式   
                                        new SqlParameter("@Province",model.Province??string.Empty) ,  //省份   
                                        new SqlParameter("@City",model.City??string.Empty) ,  //城市   
                                        new SqlParameter("@Village",model.Village??string.Empty) ,  //地区   
                                        new SqlParameter("@Address",model.Address??string.Empty) ,  //详细地址   
                                        new SqlParameter("@Sex",model.Sex??string.Empty) ,  //型别   
                                        new SqlParameter("@Birthday",model.Birthday??string.Empty) ,  //出生日期   
                                        new SqlParameter("@JoinDate",model.JoinDate??string.Empty) ,  //入职日期   
                                        new SqlParameter("@Native",model.Native??string.Empty) ,  //籍贯   
                                        new SqlParameter("@DepartmentId",model.DepartmentId) ,  //组织机构id   
                                        new SqlParameter("@Expertise",model.Expertise??string.Empty) ,  //个人简介   
                                        new SqlParameter("@JobState",model.JobState) ,  //在职状况1在职，0离职   
                                        new SqlParameter("@Photo",model.Photo??string.Empty) ,  //照片   
                                        new SqlParameter("@Attach",model.Attach??string.Empty) ,  //附件   
                                        new SqlParameter("@CreateTime",model.CreateTime) ,  //创建时间   
                                        new SqlParameter("@CreatePerson",model.CreatePerson) ,  //创建人   
                                        new SqlParameter("@State",model.State)   //状态：0-无效;1-有效;2-作废   
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 变更 系统用户状态
        public int ChangeState(int state,string uid)
        {
            string sql = @"Update 
                                  SysUser
                          Set                     
                                  State = @State 
                          Where         
                                  UserId = @UserId
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@UserId",uid) ,  //用户id自增主键    
                                        new SqlParameter("@State",state)   //状态：0-无效;1-有效   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 变更 系统用户密码
        public int ChangePwd(string pwd, string uid)
        {
            string sql = @"Update 
                                  SysUser
                          Set                     
                                  Password = @Password 
                          Where         
                                  UserId = @UserId
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@UserId",uid) ,  //用户id自增主键    
                                        new SqlParameter("@Password",pwd)   //状态：0-无效;1-有效   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 删除 系统用户表
        /// <summary>
        /// 删除 系统用户表
        /// </summary>
        public int DeleteSysUserByID (string id)
        {
           string sql = @"Delete 
                                  SysUser
                          Where         
                                  UserId = @UserId
 ";                                    
                                  
            SqlParameter[] param ={ 
                                        new SqlParameter("@UserId",id)     
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion 

        #region 分页列表 系统用户表
        /// <summary>
        ///  分页列表 系统用户表
        /// </summary>
        public List<SysUserModel> SysUserPageList(int index, int size, string where)
        {
           string sql = @" With TemTable As 
                            (
                                Select Row_Number() Over(Order By UserID Desc) AS 't',u.*, ISNULL(dep.DepartmentName,'无') As DepartmentName
                                From
                                       SysUser u Left Join SysDepartment dep
                                On
                                       u.DepartmentId=dep.DepartmentId  Where    " + where + @"
                            )
                                  
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            SqlParameter[] param ={ 
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };
            List<SysUserModel> list = new List<SysUserModel>();
            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while(dr.Read())
            {
                SysUserModel model = new SysUserModel();
                model.DepartmentName = dr["DepartmentName"].ToString();
                model.UserId = Convert.ToInt32(dr["UserId"].ToString()); //用户id自增主键
                model.UserName = dr["UserName"].ToString(); //登录用户
               // model.UserSpelling = dr["UserSpelling"].ToString(); //用户拼写
              //  model.Password = dr["Password"].ToString(); //登录密码
                model.TrueName = dr["TrueName"].ToString(); //真实名称
                model.MobileNumber = dr["MobileNumber"].ToString(); //手机号码
                model.PhoneNumber = dr["PhoneNumber"].ToString(); //电话号码
             //   model.QQ = dr["QQ"].ToString(); //QQ
                model.EmailAddress = dr["EmailAddress"].ToString(); //Email
              //  model.OtherContact = dr["OtherContact"].ToString(); //其他联系方式
             //   model.Province = dr["Province"].ToString(); //省份
             //   model.City = dr["City"].ToString(); //城市
             //   model.Village = dr["Village"].ToString(); //地区
             //   model.Address = dr["Address"].ToString(); //详细地址
                model.Sex = dr["Sex"].ToString(); //型别
                //model.Birthday = Convert.ToDateTime(dr["Birthday"].ToString()); //出生日期
                //model.JoinDate = Convert.ToDateTime(dr["JoinDate"].ToString()); //入职日期
                model.Native = dr["Native"].ToString(); //籍贯
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString()); //组织机构id
             //   model.Expertise = dr["Expertise"].ToString(); //个人简介
                model.JobState = Convert.ToInt32(dr["JobState"].ToString()); //在职状况1在职，0离职
              //  model.Photo = dr["Photo"].ToString(); //照片
                model.Attach = dr["Attach"].ToString(); //附件
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
             //   model.CreatePerson = dr["CreatePerson"].ToString(); //创建人
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效;2-作废
                model.IsSystem = Convert.ToInt32(dr["IsSystem"].ToString());
                list.Add(model);
            }
            dr.Close();

            return list;                
        }
        #endregion  

        #region 分页总数 系统用户表
        /// <summary>
        ///  分页总数 系统用户表
        /// </summary>
        public int  SysUserPageCount (string where)
        {
           string sql = @" Select 
                                    Count(u.UserId) 
                            From
                                    SysUser u
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

        #region 获取实体 系统用户表
        /// <summary>
        ///  获取实体 系统用户表
        /// </summary>
        public SysUserModel  GetModelByID (string id)
        {
           string sql = @" Select 
                                  UserId ,
                                  UserName ,
                                  UserSpelling ,
                                  Password ,
                                  TrueName ,
                                  MobileNumber ,
                                  PhoneNumber ,
                                  QQ ,
                                  EmailAddress ,
                                  OtherContact ,
                                  Province ,
                                  City ,
                                  Village ,
                                  Address ,
                                  Sex ,
                                  Birthday ,
                                  JoinDate ,
                                  Native ,
                                  DepartmentId ,
                                  Expertise ,
                                  JobState ,
                                  Photo ,
                                  Attach ,
                                  CompanyId ,
                                  CreateTime ,
                                  CreatePerson ,
                                  State 
                            From
                                  SysUser
                            Where
                                  UserId=@UserId";
             SqlParameter[] param ={ 
                                   new SqlParameter("@UserId",id)
                                  };
                                  
             SysUserModel model=null;
             
             SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
             if (dr.Read())
              {
                     model = new SysUserModel();
                     model.UserId= Convert.ToInt32(dr["UserId"].ToString()); //用户id自增主键
                     model.UserName= dr["UserName"].ToString(); //登录用户
                     model.UserSpelling= dr["UserSpelling"].ToString(); //用户拼写
                     model.Password= dr["Password"].ToString(); //登录密码
                     model.TrueName= dr["TrueName"].ToString(); //真实名称
                     model.MobileNumber= dr["MobileNumber"].ToString(); //手机号码
                     model.PhoneNumber= dr["PhoneNumber"].ToString(); //电话号码
                     model.QQ= dr["QQ"].ToString(); //QQ
                     model.EmailAddress= dr["EmailAddress"].ToString(); //Email
                     model.OtherContact= dr["OtherContact"].ToString(); //其他联系方式
                     model.Province= dr["Province"].ToString(); //省份
                     model.City= dr["City"].ToString(); //城市
                     model.Village= dr["Village"].ToString(); //地区
                     model.Address= dr["Address"].ToString(); //详细地址
                     model.Sex= dr["Sex"].ToString(); //型别                   
                     model.Birthday =dr["Birthday"].ToString(); //出生日期
                     model.JoinDate =dr["JoinDate"].ToString(); //入职日期                 
                     model.Native= dr["Native"].ToString(); //籍贯
                     model.DepartmentId= Convert.ToInt32(dr["DepartmentId"].ToString()); //组织机构id
                     model.Expertise= dr["Expertise"].ToString(); //个人简介
                     model.JobState= Convert.ToInt32(dr["JobState"].ToString()); //在职状况1在职，0离职
                     model.Photo= dr["Photo"].ToString(); //照片
                     model.Attach= dr["Attach"].ToString(); //附件
                     model.CompanyId= Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
                     model.CreateTime= Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                     model.CreatePerson= dr["CreatePerson"].ToString(); //创建人
                     model.State= Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效;2-作废
            }           
             dr.Close();

             return model;
        }
        #endregion   

        #region 获取概要实体 系统用户表
        /// <summary>
        ///  获取概要实体 系统用户表
        /// </summary>
        public SysUserModel GetDetailModel(string id)
        {
            string sql = @" Select 
                                  u.UserName ,
                                  u.TrueName ,       
                                  ISNULL(d.DepartmentName,'无') As DepartmentName    ,                          
                                  u.Photo ,
                                  c.CompanyName,
                                  u.IsSystem
                            From
                                  SysUser u left join SysCompany c on u.CompanyId=c.CompanyId left join SysDepartment d
                            on
                                  u.DepartmentId=d.DepartmentId 
                            Where u.UserId=@UserId ";
            SqlParameter[] param ={ 
                                   new SqlParameter("@UserId",id)
                                  };

            SysUserModel model = null;

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            if (dr.Read())
            {
                model = new SysUserModel();
                model.UserName = dr["UserName"].ToString(); //登录用户
                model.TrueName = dr["TrueName"].ToString(); //真实名称
                model.DepartmentName =dr["DepartmentName"].ToString(); //组织机构
                model.Photo = dr["Photo"].ToString().Trim(); //照片 
                model.CompanyName = dr["CompanyName"].ToString(); //系统公司id
                model.IsSystem = Convert.ToInt32(dr["IsSystem"].ToString()); //状态：0-无效;1-有效
            }
            dr.Close();

            return model;
        }
        #endregion   


        #region 获取用户登陆信息
         /// <summary>
         /// 根据用户名和密码获取用户信息
         /// </summary>
        public SysUserModel UserLoginInfo(string user, string pwd)
        {
            string sql = @" Select 
                                  UserId,
                                  SysUser.CompanyId ,
                                  CASE WHEN dbo.SysCompany.State=0 THEN 40  WHEN dbo.SysDepartment.State=0 THEN 30 WHEN dbo.SysUser.State=0 THEN 20 WHEN dbo.SysUser.State=2 THEN 20 ELSE 1 END  AS State,
                                  SysUser.DepartmentId,
								  isnull(SysDepartment.DeparType,2) as DeparType
                            From
                                  SysUser left JOIN dbo.SysDepartment 
								  ON dbo.SysUser.DepartmentId = dbo.SysDepartment.DepartmentId left JOIN dbo.SysCompany  ON dbo.SysUser.CompanyId=dbo.SysCompany.CompanyId         Where
                                  SysUser.UserName=@UserName And SysUser.Password=@Password And SysUser.State<2";
            SqlParameter[] param ={ 
                                   new SqlParameter("@UserName",user),
                                   new SqlParameter("@Password",pwd)
                                  };

            SysUserModel model = null;

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            if (dr.Read())
            {
                model = new SysUserModel();
                model.UserId = Convert.ToInt32(dr["UserId"].ToString());//主键
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效;2-作废
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString());
                model.DeparType = Convert.ToInt32(dr["DeparType"].ToString()); //组织机构类型：0 - 根；1 - 公司级别; 2 - 普通部门; 3 - 供应商部门
            }
            dr.Close();

            return model;
        }
        #endregion

        #region 是否存在相同的用户名
        public int IsHasUser(string username)
        {
            var sql = "Select Count(1) From SysUser Where UserName=@UserName and State<>2 ";
            SqlParameter[] param ={
                                 new SqlParameter("@UserName",username)
                                 };
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }

        public int IsHasUsers(int userid, string username)
        {
            if (userid == 0)
            {
                var sql = "Select Count(1) From SysUser Where UserName=@UserName and State<>2 ";
                SqlParameter[] param ={
                                 new SqlParameter("@UserName",username)
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
                var sql = "Select Count(1) From SysUser Where UserName=@UserName And UserID!=@UserID and State<>2";
                SqlParameter[] param ={
                                 new SqlParameter("@UserName",username),
                                 new SqlParameter("@UserID",userid)
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
       
        #region 用户公司是否为后台账号
        public int IsAdmin(string cid)
        {
            var sql = "Select IsAdmin From SysCompany Where CompanyId=@CompanyId";

            SqlParameter[] parm =
            {
                new SqlParameter("@CompanyId",cid)
            };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, parm);

            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());

        }
        #endregion

        #region 更新用户角色信息
        public int UpdateRoleInfo(string uid,string roles)
        {
            string sql = @"Update 
                                  SysUser
                          Set
                                  Attach = @Attach 
                          Where         
                                  UserId = @UserId
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@UserId",uid) ,  //用户id自增主键    
                                       
                                        new SqlParameter("@Attach",roles)    
                                       
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 用户角色编号列表
         /// <summary>
         /// 用户角色编号列表
         /// </summary>
         /// <param name="uid">用户主键</param>
         /// <returns>角色编号列表</returns>
        public List<string> UserRoleList(string uid)
        {
            var sql = @"Select 
                              ur.RoleId
                        From
                              SysUser u,SysUserRole ur
                        Where 
                              u.UserId=ur.UserId And u.UserId= @UserId";

            SqlParameter[] param ={
                                 new SqlParameter("@UserId",uid)
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

        #region 删除已有用户角色信息
        public int DeleteUserRoleInfo(string uid)
        {
            var sql = "Delete SysUserRole Where UserId=@UserId";

            SqlParameter[] parm = { new SqlParameter("@UserId", uid) };

            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parm);

            return i;
        }
        #endregion

        #region 用户角色菜单
        public List<SysModuleModel> GetUserModule(string uid,string deparType)
        {
            string sql = "";
            if (deparType != "3")//组织机构类型：0-根；1-公司级别;2-普通部门;3-供应商部门
            {
                sql=@"
                            With temRole As
                            (
                                Select 
                                       Distinct rm.Moduleid
                                From
                                       SysUserRole ur,SysRoleModule rm
                                Where
                                       rm.RoleId=ur.RoleId And ur.UserId=@UserId
                            ) 
                           Select 
                                  m.ModuleId ,
                                  m.ModuleName ,
                                  m.ParentId ,
                                  m.Url ,
                                  m.Iconic 
                            From
                                  SysModule m,temRole tem
                            Where  
                                  m.ModuleId=tem.ModuleId   Order By m.ParentId,m.Sort";
            }else
            {
                sql = @"
                            With temRole As
                            (
                                Select 
                                       Distinct rm.Moduleid
                                From
                                       SysUserRole ur,SysRoleModule rm
                                Where
                                       rm.RoleId=ur.RoleId And ur.UserId=@UserId
                            ) 
                           Select 
                                  m.ModuleId ,
                                  m.ModuleName ,
                                  m.ParentId ,
                                  m.Url ,
                                  m.Iconic 
                            From
                                  SysModule m,temRole tem
                            Where  
                                  m.ModuleId=tem.ModuleId and m.ModuleType=1  Order By m.ParentId,m.Sort";
            }

            SqlParameter[] parm ={
                                    new SqlParameter("@UserId",uid)
                                };

            List<SysModuleModel> list = new List<SysModuleModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, parm);
            while (dr.Read())
            {
                SysModuleModel model = new SysModuleModel();
                model.ModuleId = Convert.ToInt32(dr["ModuleId"].ToString()); //主键;自增
                model.ModuleName = dr["ModuleName"].ToString(); //功能名称
                model.ParentId = Convert.ToInt32(dr["ParentId"].ToString()); //上级ID
                model.Url = dr["Url"].ToString(); //链接
                model.Iconic = dr["Iconic"].ToString(); //图标

                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 判断用户是否拥有操作按钮权限
        public int IsHasOperateAuth(string roleids,string modid,string opername)
        {
            var sql= @"Select   
                            Count(ro.ModuleId)
                      From  
                            SysRoleOperate ro,SysOperate o
                      Where
                            ro.OperateId=o.OperateId And o.Code=@Code And ro.ModuleId=@ModuleId And ro.RoleId in ("+roleids+")";

            SqlParameter[] parm =
            {
                new SqlParameter("@Code",opername),
                new SqlParameter("@ModuleId",modid)
            };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, parm);
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

        #region 获取实体 系统用户表默认账号
        /// <summary>
        ///  获取实体 系统用户表默认账号
        /// </summary>
        public SysUserModel GetDefaultInfo(string cid)
        {
            string sql = @" Select 
                                  UserId ,
                                  UserName ,
                                  Password 
                            From
                                  SysUser
                            Where
                                  CompanyId=@CompanyId And IsSystem=1";
            SqlParameter[] param ={ 
                                   new SqlParameter("@CompanyId",cid)
                                  };

            SysUserModel model = null;

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            if (dr.Read())
            {
                model = new SysUserModel();
                model.UserId = Convert.ToInt32(dr["UserId"].ToString()); //用户id自增主键
                model.UserName = dr["UserName"].ToString(); //登录用户
                model.Password = dr["Password"].ToString(); //登录密码          
            }
            dr.Close();

            return model;
        }
        #endregion

        #region 导出Excel数据
        public List<SysUserModel> ExportData(string where)
        {
            string sql = @" With TemTable As 
                            (
                                Select Row_Number() Over(Order By UserID Desc) AS 't',u.*, ISNULL(dep.DepartmentName,'无') As DepartmentName
                                From
                                       SysUser u Left Join SysDepartment dep
                                On
                                       u.DepartmentId=dep.DepartmentId  Where    " + where + @"
                            )
                                  
                            Select * From TemTable ";

            List<SysUserModel> list = new List<SysUserModel>();
            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            while (dr.Read())
            {
                SysUserModel model = new SysUserModel();
                model.DepartmentName = dr["DepartmentName"].ToString();
                model.UserId = Convert.ToInt32(dr["UserId"].ToString()); //用户id自增主键
                model.UserName = dr["UserName"].ToString(); //登录用户
                model.TrueName = dr["TrueName"].ToString(); //真实名称
                model.MobileNumber = dr["MobileNumber"].ToString(); //手机号码
                model.PhoneNumber = dr["PhoneNumber"].ToString(); //电话号码
                model.EmailAddress = dr["EmailAddress"].ToString(); //Email
                model.Sex = dr["Sex"].ToString(); //型别
                model.Native = dr["Native"].ToString(); //籍贯
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString()); //组织机构id
                model.JobState = Convert.ToInt32(dr["JobState"].ToString()); //在职状况1在职，0离职      
                model.Attach = dr["Attach"].ToString(); //附件
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效;2-作废
                model.IsSystem = Convert.ToInt32(dr["IsSystem"].ToString());
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion
    }
}

