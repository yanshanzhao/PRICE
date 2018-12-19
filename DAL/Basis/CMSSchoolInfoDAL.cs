//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/4/17    1.0        MH        
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using DBUtility;
using Model.CMS;
#endregion
/*********************************
 * 类名：CMSSchoolInfoDAL
 * 功能描述： 数据访问层类
 * ******************************/
namespace DAL.CMS
{
 public class CMSSchoolInfoDAL
    {  
     // 获取连接串
     string conn =DBUtility.ConnectionStringInfo.ConnectionString().ToString();
     
     
        #region 添加 
        /// <summary>
        /// 添加 
        /// </summary>
        public int AddCMSSchoolInfo (CMSSchoolInfoModel model)
        {
           string sql = @"Insert 
                                  CMS_School
                                  (
                                        SchoolID ,
                                        SchoolName ,
                                        SchoolMaster ,
                                        SchoolTel ,
                                        Province ,
                                        City ,
                                        Area ,
                                        SchoolAddress ,
                                        TimeLimit ,
                                        Maker ,
                                        MakerID ,
                                        MakerTime ,
                                        SchoolState ,
                                        SchoolAppCodes ,
                                        Remarks ,
                                        Memo ,
                                        Email ,
                                        Mobile 
                                      
                                  )
                          Values
                                  (
                                        @SchoolID ,
                                        @SchoolName ,
                                        @SchoolMaster ,
                                        @SchoolTel ,
                                        @Province ,
                                        @City ,
                                        @Area ,
                                        @SchoolAddress ,
                                        @TimeLimit ,
                                        @Maker ,
                                        @MakerID ,
                                        @MakerTime ,
                                        @SchoolState ,
                                        @SchoolAppCodes ,
                                        @Remarks ,
                                        @Memo ,
                                        @Email ,
                                        @Mobile 
                                  )";
            SqlParameter[] param ={ 
                                        new SqlParameter("@SchoolID",model.SchoolID) ,  //学校主键   
                                        new SqlParameter("@SchoolName",model.SchoolName) ,  //学校名称   
                                        new SqlParameter("@SchoolMaster",model.SchoolMaster) ,  //校长   
                                        new SqlParameter("@SchoolTel",model.SchoolTel) ,  //电话   
                                        new SqlParameter("@Province",model.Province) ,  //省   
                                        new SqlParameter("@City",model.City) ,  //市   
                                        new SqlParameter("@Area",model.Area) ,  //区   
                                        new SqlParameter("@SchoolAddress",model.SchoolAddress) ,  //学校地址   
                                        new SqlParameter("@TimeLimit",model.TimeLimit) ,  //时间限制   
                                        new SqlParameter("@Maker",model.Maker) ,  //业务员   
                                        new SqlParameter("@MakerID",model.MakerID) ,  //业务员主键   
                                        new SqlParameter("@MakerTime",model.MakerTime) ,  //操作时间   
                                        new SqlParameter("@SchoolState",model.SchoolState) ,  //状态(0 未审核 1 审核失败 2 通过）   
                                        new SqlParameter("@SchoolAppCodes",model.SchoolAppCodes) ,  //学校应用集合   
                                        new SqlParameter("@Remarks",model.Remarks) ,  //审核批注   
                                        new SqlParameter("@Memo",model.Memo) ,  //预留   
                                        new SqlParameter("@Email",model.Email) ,  //邮箱   
                                        new SqlParameter("@Mobile",model.Mobile)   //手机   
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 修改 
        /// <summary>
        /// 修改 
        /// </summary>
        public int UpdateCMSSchoolInfo (CMSSchoolInfoModel model)
        {
           string sql = @"Update 
                                  CMS_School
                          Set
                                  SchoolID = @SchoolID ,
                                  SchoolName = @SchoolName ,
                                  SchoolMaster = @SchoolMaster ,
                                  SchoolTel = @SchoolTel ,
                                  Province = @Province ,
                                  City = @City ,
                                  Area = @Area ,
                                  SchoolAddress = @SchoolAddress ,
                                  TimeLimit = @TimeLimit ,
                                  Maker = @Maker ,
                                  MakerID = @MakerID ,
                                  MakerTime = @MakerTime ,
                                  SchoolState = @SchoolState ,
                                  SchoolAppCodes = @SchoolAppCodes ,
                                  Remarks = @Remarks ,
                                  Memo = @Memo ,
                                  Email = @Email ,
                                  Mobile = @Mobile 
                          Where         
                                  SchoolID = @SchoolID
 ";                                    
                                  
            SqlParameter[] param ={ 
                                        new SqlParameter("@SchoolID",model.SchoolID) ,  //学校主键   
                                        new SqlParameter("@SchoolName",model.SchoolName) ,  //学校名称   
                                        new SqlParameter("@SchoolMaster",model.SchoolMaster) ,  //校长   
                                        new SqlParameter("@SchoolTel",model.SchoolTel) ,  //电话   
                                        new SqlParameter("@Province",model.Province) ,  //省   
                                        new SqlParameter("@City",model.City) ,  //市   
                                        new SqlParameter("@Area",model.Area) ,  //区   
                                        new SqlParameter("@SchoolAddress",model.SchoolAddress) ,  //学校地址   
                                        new SqlParameter("@TimeLimit",model.TimeLimit) ,  //时间限制   
                                        new SqlParameter("@Maker",model.Maker) ,  //业务员   
                                        new SqlParameter("@MakerID",model.MakerID) ,  //业务员主键   
                                        new SqlParameter("@MakerTime",model.MakerTime) ,  //操作时间   
                                        new SqlParameter("@SchoolState",model.SchoolState) ,  //状态(0 未审核 1 审核失败 2 通过）   
                                        new SqlParameter("@SchoolAppCodes",model.SchoolAppCodes) ,  //学校应用集合   
                                        new SqlParameter("@Remarks",model.Remarks) ,  //审核批注   
                                        new SqlParameter("@Memo",model.Memo) ,  //预留   
                                        new SqlParameter("@Email",model.Email) ,  //邮箱   
                                        new SqlParameter("@Mobile",model.Mobile)   //手机   
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 变更状态
        public int UpdateState(CMSSchoolInfoModel model)
        {
            string sql = @"Update 
                                  CMS_School
                          Set     
                                  MakerTime =getdate() ,
                                  SchoolState = @SchoolState ,                 
                                  Remarks = @Remarks 
                          Where         
                                  SchoolID = @SchoolID
 ";

            SqlParameter[] param ={ 
                                        new SqlParameter("@SchoolID",model.SchoolID) ,  //学校主键    
                                        new SqlParameter("@SchoolState",model.SchoolState) ,  //状态(0 未审核 1 审核失败 2 通过）   
                                        new SqlParameter("@Remarks",model.Remarks) ,  //审核批注                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除 
        /// </summary>
        public int DeleteCMSSchoolInfoByID (string id)
        {
           string sql = @"Delete 
                                  CMS_School
                          Where         
                                  SchoolID = @SchoolID
 ";                                    
                                  
            SqlParameter[] param ={ 
                                        new SqlParameter("@SchoolID",id)     
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion 

        #region 分页列表 
        /// <summary>
        ///  分页列表 
        /// </summary>
        public   List<CMSSchoolInfoModel>  CMSSchoolInfoPageList (int index, int size, string where)
        {
           string sql = @" With TemTable As 
                            (
                                Select Row_Number() Over(Order By MakerTime desc) AS 't',*
                                From
                                       CMS_School
                                Where
                                       " + where + @"
                            )
                                  
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            SqlParameter[] param ={ 
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };

            List<CMSSchoolInfoModel> list = new List<CMSSchoolInfoModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);

            while (dr.Read())
            {
                CMSSchoolInfoModel model = new CMSSchoolInfoModel();
                model.SchoolID = dr["SchoolID"].ToString(); //学校主键
                model.SchoolName = dr["SchoolName"].ToString(); //学校名称
                model.SchoolMaster = dr["SchoolMaster"].ToString(); //校长
                model.SchoolTel = dr["SchoolTel"].ToString(); //电话
                model.Province = dr["Province"].ToString(); //省
                model.City = dr["City"].ToString(); //市
                model.Area = dr["Area"].ToString(); //区
                model.SchoolAddress = dr["SchoolAddress"].ToString(); //学校地址
                model.TimeLimit = dr["TimeLimit"].ToString(); //时间限制
                model.Maker = dr["Maker"].ToString(); //业务员
                model.MakerID = dr["MakerID"].ToString(); //业务员主键
                model.MakerTime = Convert.ToDateTime(dr["MakerTime"].ToString()); //操作时间
                model.SchoolState = Convert.ToInt32(dr["SchoolState"].ToString()); //状态(0 未审核 1 审核失败 2 通过）
                model.SchoolAppCodes = dr["SchoolAppCodes"].ToString(); //学校应用集合
                model.Remarks = dr["Remarks"].ToString(); //审核批注
                model.Memo = dr["Memo"].ToString(); //预留
                model.Email = dr["Email"].ToString(); //邮箱
                model.Mobile = dr["Mobile"].ToString(); //手机

                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion  

        #region 分页总数 
        /// <summary>
        ///  分页总数 
        /// </summary>
        public int  CMSSchoolInfoPageCount (string where)
        {
           string sql = @" Select 
                                    Count(*) 
                            From
                                    CMS_School
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

        #region 获取实体 
        /// <summary>
        ///  获取实体 
        /// </summary>
        public CMSSchoolInfoModel  GetModelByID (string id)
        {
           string sql = @" Select 
                                  SchoolID ,
                                  SchoolName ,
                                  SchoolMaster ,
                                  SchoolTel ,
                                  Province ,
                                  City ,
                                  Area ,
                                  SchoolAddress ,
                                  TimeLimit ,
                                  Maker ,
                                  MakerID ,
                                  MakerTime ,
                                  SchoolState ,
                                  SchoolAppCodes ,
                                  Remarks ,
                                  Memo ,
                                  Email ,
                                  Mobile 
                            From
                                  CMS_School
                            Where
                                  SchoolID=@SchoolID";
             SqlParameter[] param ={ 
                                   new SqlParameter("@SchoolID",id)
                                  };
                                  
             CMSSchoolInfoModel model=null;
             
             SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
             if (dr.Read())
              {
                     model = new CMSSchoolInfoModel();
                     model.SchoolID= dr["SchoolID"].ToString(); //学校主键
                     model.SchoolName= dr["SchoolName"].ToString(); //学校名称
                     model.SchoolMaster= dr["SchoolMaster"].ToString(); //校长
                     model.SchoolTel= dr["SchoolTel"].ToString(); //电话
                     model.Province= dr["Province"].ToString(); //省
                     model.City= dr["City"].ToString(); //市
                     model.Area= dr["Area"].ToString(); //区
                     model.SchoolAddress= dr["SchoolAddress"].ToString(); //学校地址
                     model.TimeLimit= dr["TimeLimit"].ToString(); //时间限制
                     model.Maker= dr["Maker"].ToString(); //业务员
                     model.MakerID= dr["MakerID"].ToString(); //业务员主键
                     model.MakerTime= Convert.ToDateTime(dr["MakerTime"].ToString()); //操作时间
                     model.SchoolState= Convert.ToInt32(dr["SchoolState"].ToString()); //状态(0 未审核 1 审核失败 2 通过）
                     model.SchoolAppCodes= dr["SchoolAppCodes"].ToString(); //学校应用集合
                     model.Remarks= dr["Remarks"].ToString(); //审核批注
                     model.Memo= dr["Memo"].ToString(); //预留
                     model.Email= dr["Email"].ToString(); //邮箱
                     model.Mobile= dr["Mobile"].ToString(); //手机
            }           
             dr.Close();

             return model;
        }
        #endregion   
     }
}

