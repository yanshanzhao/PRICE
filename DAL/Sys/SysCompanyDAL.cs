//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-20    1.0        MH         新建
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
 * 类名：SysCompanyDAL
 * 功能描述：系统公司表 数据访问层类
 * ******************************/
namespace DAL.Sys
{
    public class SysCompanyDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();


        #region 添加 系统公司表
        /// <summary>
        /// 添加 系统公司表
        /// </summary>
        public int AddSysCompany(SysCompanyModel model)
        {
            string sql = @"Insert 
                                  SysCompany
                                  (
                                        CompanyName ,
                                        CompanyNo ,
                                        CreateTime ,
                                        EmailAddress ,
                                        MobileNumber ,
                                        PhoneNumber ,
                                        OtherContact ,
                                        Province ,
                                        City ,
                                        IsAdmin ,
                                        Village ,
                                        Address ,
                                        Remark ,
                                        State ,Expertise

                                      
                                  )
                          Values
                                  (
                                        @CompanyName ,
                                        @CompanyNo ,
                                        @CreateTime ,
                                        @EmailAddress ,
                                        @MobileNumber ,
                                        @PhoneNumber ,
                                        @OtherContact ,
                                        @Province ,
                                        @City ,
                                        @IsAdmin ,
                                        @Village ,
                                        @Address ,
                                        @Remark ,
                                        @State , @Expertise
                                  );SELECT SCOPE_IDENTITY() ";
            SqlParameter[] param ={
                                        new SqlParameter("@CompanyName",model.CompanyName) ,  //公司名称   
                                        new SqlParameter("@CompanyNo",model.CompanyNo) ,  //公司编号(作为辅助使用;唯一性)   
                                        new SqlParameter("@CreateTime",model.CreateTime) ,  //创建时间   
                                        new SqlParameter("@EmailAddress",model.EmailAddress) ,  //Email   
                                        new SqlParameter("@MobileNumber",model.MobileNumber) ,  //手机号码   
                                        new SqlParameter("@PhoneNumber",model.PhoneNumber) ,  //电话号码   
                                        new SqlParameter("@OtherContact",model.OtherContact) ,  //其他联系方式   
                                        new SqlParameter("@Province",model.Province) ,  //省份   
                                        new SqlParameter("@City",model.City) ,  //城市   
                                        new SqlParameter("@IsAdmin",model.IsAdmin) ,  //   
                                        new SqlParameter("@Village",model.Village) ,  //地区   
                                        new SqlParameter("@Address",model.Address) ,  //详细地址   
                                        new SqlParameter("@Remark",model.Remark) ,  //说明   
                                        new SqlParameter("@State",model.State),   //状态：0-无效;1-有效  
                                        new SqlParameter("@Expertise",model.Expertise)   //公司简介
                                  };
            object obj = new object(); 
            try
            {
                obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param); 
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

        #region 修改 系统公司表
        /// <summary>
        /// 修改 系统公司表
        /// </summary>
        public int UpdateSysCompany(SysCompanyModel model)
        {
            string sql = @"Update 
                                  SysCompany
                          Set
                                  CompanyName = @CompanyName ,
                                  CompanyNo = @CompanyNo ,
                                  CreateTime = @CreateTime ,
                                  EmailAddress = @EmailAddress ,
                                  MobileNumber = @MobileNumber ,
                                  PhoneNumber = @PhoneNumber ,
                                  OtherContact = @OtherContact ,
                                  Province = @Province ,
                                  City = @City ,
                                  IsAdmin = @IsAdmin ,
                                  Village = @Village ,
                                  Address = @Address ,
                                  Remark = @Remark ,
                                  State = @State ,Expertise=@Expertise
                          Where         
                                  CompanyId = @CompanyId
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@CompanyId",model.CompanyId) ,  //系统公司表id;主键   
                                        new SqlParameter("@CompanyName",model.CompanyName) ,  //公司名称   
                                        new SqlParameter("@CompanyNo",model.CompanyNo) ,  //公司编号(作为辅助使用;唯一性)   
                                        new SqlParameter("@CreateTime",model.CreateTime) ,  //创建时间   
                                        new SqlParameter("@EmailAddress",model.EmailAddress) ,  //Email   
                                        new SqlParameter("@MobileNumber",model.MobileNumber) ,  //手机号码   
                                        new SqlParameter("@PhoneNumber",model.PhoneNumber) ,  //电话号码   
                                        new SqlParameter("@OtherContact",model.OtherContact) ,  //其他联系方式   
                                        new SqlParameter("@Province",model.Province) ,  //省份   
                                        new SqlParameter("@City",model.City) ,  //城市   
                                        new SqlParameter("@IsAdmin",model.IsAdmin) ,  //   
                                        new SqlParameter("@Village",model.Village) ,  //地区   
                                        new SqlParameter("@Address",model.Address) ,  //详细地址   
                                        new SqlParameter("@Remark",model.Remark) ,  //说明   
                                        new SqlParameter("@State",model.State),   //状态：0-无效;1-有效   
                                          new SqlParameter("@Expertise",model.Expertise)   //公司简介
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 删除 系统公司表
        /// <summary>
        /// 删除 系统公司表
        /// </summary>
        public int DeleteSysCompanyByID(string id)
        {
            string sql = @"Delete 
                                  SysCompany
                          Where         
                                  CompanyId = @CompanyId
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@CompanyId",id)
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion 

        #region 分页列表 系统公司表
        /// <summary>
        ///  分页列表 系统公司表
        /// </summary>
        public List<SysCompanyModel> SysCompanyPageList(int index, int size, string where)
        {
            string sql = @" With TemTable As 
                            (
                                Select Row_Number() Over(Order By CreateTime) AS 't',SysCompany.*,SysDepartment.DepartmentId
                                From
                                       SysCompany LEFT JOIN dbo.SysDepartment
									   ON   dbo.SysCompany.CompanyId =dbo.SysDepartment.CompanyId 
                                Where
                                       " + where + @"
                            )
                                  
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };
            List<SysCompanyModel> list = new List<SysCompanyModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysCompanyModel model = new SysCompanyModel();
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司表id;主键
                model.CompanyName = dr["CompanyName"].ToString(); //公司名称
                model.CompanyNo = dr["CompanyNo"].ToString(); //公司编号(作为辅助使用;唯一性)
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                model.EmailAddress = dr["EmailAddress"].ToString(); //Email
                model.MobileNumber = dr["MobileNumber"].ToString(); //手机号码
                model.PhoneNumber = dr["PhoneNumber"].ToString(); //电话号码
                model.OtherContact = dr["OtherContact"].ToString(); //其他联系方式
                model.Province = dr["Province"].ToString(); //省份
                model.City = dr["City"].ToString(); //城市
                model.IsAdmin = Convert.ToInt32(dr["IsAdmin"].ToString()); //
                model.Village = dr["Village"].ToString(); //地区
                model.Address = dr["Address"].ToString(); //详细地址
                model.Remark = dr["Remark"].ToString(); //说明
                model.IsConfig = Convert.ToInt32(dr["IsConfig"].ToString());
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效
                model.Expertise = dr["Expertise"].ToString(); //公司简介
                model.DepartmentId= Convert.ToInt32(dr["DepartmentId"].ToString()); //公司id
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion  

        #region 分页总数 系统公司表
        /// <summary>
        ///  分页总数 系统公司表
        /// </summary>
        public int SysCompanyPageCount(string where)
        {
            string sql = @" Select 
                                    Count(1) 
                            From
                                     SysCompany LEFT JOIN dbo.SysDepartment
									   ON  dbo.SysCompany.CompanyId =dbo.SysDepartment.CompanyId 
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

        #region 获取实体 系统公司表
        /// <summary>
        ///  获取实体 系统公司表
        /// </summary>
        public SysCompanyModel GetModelByID(string id)
        {
            string sql = @" Select 
                                  CompanyId ,
                                  CompanyName ,
                                  CompanyNo ,
                                  CreateTime ,
                                  EmailAddress ,
                                  MobileNumber ,
                                  PhoneNumber ,
                                  OtherContact ,
                                  Province ,
                                  City ,
                                  IsAdmin ,
                                  Village ,
                                  Address ,
                                  Remark ,
                                  State ,Expertise
                            From
                                  SysCompany
                            Where
                                  CompanyId=@CompanyId";
            SqlParameter[] param ={
                                   new SqlParameter("@CompanyId",id)
                                  };

            SysCompanyModel model = null;

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            if (dr.Read())
            {
                model = new SysCompanyModel();
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司表id;主键
                model.CompanyName = dr["CompanyName"].ToString(); //公司名称
                model.CompanyNo = dr["CompanyNo"].ToString(); //公司编号(作为辅助使用;唯一性)
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                model.EmailAddress = dr["EmailAddress"].ToString(); //Email
                model.MobileNumber = dr["MobileNumber"].ToString(); //手机号码
                model.PhoneNumber = dr["PhoneNumber"].ToString(); //电话号码
                model.OtherContact = dr["OtherContact"].ToString(); //其他联系方式
                model.Province = dr["Province"].ToString(); //省份
                model.City = dr["City"].ToString(); //城市
                model.IsAdmin = Convert.ToInt32(dr["IsAdmin"].ToString()); //
                model.Village = dr["Village"].ToString(); //地区
                model.Address = dr["Address"].ToString(); //详细地址
                model.Remark = dr["Remark"].ToString(); //说明
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效
                model.Expertise = dr["Expertise"].ToString(); //状态：0-无效;1-有效
            }
            dr.Close();

            return model;
        }
        #endregion

        #region 是否存在相同的企业
        public int IsHasComp(int id, string comp)
        {
            if (id == 0)
            {
                var sql = "Select Count(*) From SysCompany Where CompanyName=@CompanyName ";
                SqlParameter[] param ={
                                 new SqlParameter("@CompanyName",comp)
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
                var sql = "Select Count(*) From SysCompany Where CompanyName=@CompanyName And CompanyId!=@CompanyId";
                SqlParameter[] param ={
                                 new SqlParameter("@CompanyName",comp),
                                 new SqlParameter("@CompanyId",id)
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

        #region 添加默认账号默认角色
        public int SetDefaultInfo(SysUserModel model, SysRoleModel roleModel, string cid, ref int roleid)
        {
            int userid = 0;
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();

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
                                        State ,
                                        IsSystem
                                      
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
                                        @State ,
                                        @IsSystem
                                  );select @@identity";
                SqlParameter[] param ={
                                        new SqlParameter("@UserName",model.UserName) ,  //登录用户   
                                        new SqlParameter("@UserSpelling",model.UserSpelling) ,  //用户拼写   
                                        new SqlParameter("@Password",model.Password) ,  //登录密码   
                                        new SqlParameter("@TrueName",model.TrueName??string.Empty) ,  //真实名称   
                                        new SqlParameter("@MobileNumber",model.MobileNumber) ,  //手机号码   
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
                                        new SqlParameter("@State",model.State)  , //状态：0-无效;1-有效  ;2-作废 
                                        new SqlParameter("@IsSystem",model.IsSystem)

                                  };
                object i = SQLHelper.ExecuteScalar(tran, CommandType.Text, sql, param);

                if (i == null)
                {
                    tran.Rollback();
                    return 0;
                }
                else
                {
                    userid = Convert.ToInt32(i.ToString());
                }

                string sqls = @"Insert 
                                  SysRole
                                  (
                                        RoleName ,
                                        State ,
                                        CreateTime ,
                                        CreatePerson ,
                                        Remark ,
                                        CompanyId ,
                                        IsSystem
                                      
                                  )
                          Values
                                  (
                                        @RoleName ,
                                        @State ,
                                        @CreateTime ,
                                        @CreatePerson ,
                                        @Remark ,
                                        @CompanyId ,
                                        @IsSystem
                                  );select @@identity";
                SqlParameter[] paramss ={
                                        new SqlParameter("@RoleName",roleModel.RoleName) ,  //角色名称   
                                        new SqlParameter("@State",roleModel.State) ,  //状态：0-无效;1-有效   
                                        new SqlParameter("@CreateTime",roleModel.CreateTime) ,  //创建时间   
                                        new SqlParameter("@CreatePerson",roleModel.CreatePerson) ,  //创建人   
                                        new SqlParameter("@Remark",roleModel.Remark) ,  //说明   
                                        new SqlParameter("@CompanyId",model.CompanyId),   //系统公司id   
                                        new SqlParameter("@IsSystem",model.IsSystem)

                                  };
                object obj = SQLHelper.ExecuteScalar(tran, CommandType.Text, sqls, paramss);
                if (obj == null)
                {
                    tran.Rollback();
                    return 0;
                }
                else
                {
                    roleid = Convert.ToInt32(obj.ToString());
                }


                if (roleid != 0 && userid != 0)
                {
                    var userrolesql = "Insert SysUserRole (UserId,RoleId) Values (@UserId,@RoleId)";

                    SqlParameter[] userrolepara ={
                                                    new SqlParameter("@UserId",userid),
                                                    new SqlParameter("@RoleId",roleid)
                                                };
                    int ress = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, userrolesql, userrolepara);
                    if (ress < 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }


                string updatesql = @"Update 
                                  SysCompany
                          Set
                                  IsConfig = 1 
                                 
                          Where         
                                  CompanyId = @CompanyId

 ";
                SqlParameter[] uppara ={
                                        new SqlParameter("@CompanyId",model.CompanyId) //系统公司表id;主键                                                      
                                  };
                int res = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, updatesql, uppara);
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

        #region 更改默认账号默认角色
        public int UpdateDefaultInfo(string uid, string uname, string roleid, string rolename, string rolememo)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();

                var sql = "Update SysUser Set UserName=@UserName Where UserId=@UserId";

                SqlParameter[] parm ={
                                        new SqlParameter("@UserName",uname),
                                        new SqlParameter("@UserId",uid)
                                    };

                int res = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, sql, parm);
                if (res < 1)
                {
                    tran.Rollback();
                    return 0;
                }

                string sqls = @"Update 
                                  SysRole
                          Set
                                  RoleName = @RoleName ,
                                  Remark = @Remark

                          Where         
                                  RoleId = @RoleId
 ";

                SqlParameter[] param ={
                                        new SqlParameter("@RoleId",roleid) ,  //角色id自增   
                                        new SqlParameter("@RoleName",rolename) ,  //角色名称     
                                        new SqlParameter("@Remark",rolename) //说明    
                       
                                  };
                int i = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, sqls, param);
                if (i < 1)
                {
                    tran.Rollback();
                    return 0;
                }


                tran.Commit();
                return 1;
            }
        }
        #endregion

        #region 变更 企业状态
        public int ChangeState(int state, string uid)
        {

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();

                var sql = "Update SysCompany Set State=@State Where CompanyId=@CompanyId";

                SqlParameter[] parm ={
                                        new SqlParameter("@State",state),
                                        new SqlParameter("@CompanyId",uid)
                                    };

                int res = SQLHelper.ExecuteNonQuery(tran, CommandType.Text, sql, parm);
                if (res < 1)
                {
                    tran.Rollback();
                    return 0;
                }

                string sqls = @"Update 
                                      SysUser
                                Set                     
                                      State = @State 
                                Where         
                                      CompanyId = @CompanyId
 ";

                SqlParameter[] param ={
                                        new SqlParameter("@CompanyId",uid) ,  //用户id自增主键    
                                        new SqlParameter("@State",state)   //状态：0-无效;1-有效   
                       
                                  };
                SQLHelper.ExecuteNonQuery(tran, CommandType.Text, sqls, param);

                tran.Commit();
                return 1;
            }
        }
        #endregion

        #region 当前账套前缀流水号
        /// <summary>
        /// 当前账套前缀流水号
        /// </summary>
        /// <param name="cid">系统公司id</param>
        /// <param name="prefix">编号</param>
        /// <returns></returns>
        public string GetAutoNum(string cid, string prefix)
        {
            SqlParameter[] parm =
            {
                new SqlParameter("@CharNo",prefix),
                new SqlParameter("@CompanyId",cid)
            };
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "SysNewNumber", parm);

            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        #endregion
    }
}

