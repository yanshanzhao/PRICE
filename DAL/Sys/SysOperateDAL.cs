//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-08    1.0        MH         新建
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
 * 类名：SysOperateDAL
 * 功能描述：按钮表 数据访问层类
 * ******************************/
namespace DAL.Sys
{
 public class SysOperateDAL
    {  
     // 获取连接串
     string conn =DBUtility.ConnectionStringInfo.ConnectionString().ToString();
     
     
        #region 添加 按钮表
        /// <summary>
        /// 添加 按钮表
        /// </summary>
        public int AddSysOperate (SysOperateModel model)
        {
           string sql = @"Insert 
                                  SysOperate
                                  (
                                        OperateName ,
                                        Code ,
                                        Iconic ,
                                        Remark ,
                                        State 
                                      
                                  )
                          Values
                                  (
                                        @OperateName ,
                                        @Code ,
                                        @Iconic ,
                                        @Remark ,
                                        @State 
                                  )";
            SqlParameter[] param ={ 
                                        new SqlParameter("@OperateName",model.OperateName) ,  //按钮名称   
                                        new SqlParameter("@Code",model.Code) ,  //Cod   
                                        new SqlParameter("@Iconic",model.Iconic) ,  //图标   
                                        new SqlParameter("@Remark",model.Remark) ,  //说明   
                                        new SqlParameter("@State",model.State)   //状态：0-无效;1-有效   
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 修改 按钮表
        /// <summary>
        /// 修改 按钮表
        /// </summary>
        public int UpdateSysOperate (SysOperateModel model)
        {
           string sql = @"Update 
                                  SysOperate
                          Set
                                  OperateName = @OperateName ,
                                  Code = @Code ,
                                  Iconic = @Iconic ,
                                  Remark = @Remark ,
                                  State = @State 
                          Where         
                                  OperateId = @OperateId
 ";                                    
                                  
            SqlParameter[] param ={ 
                                        new SqlParameter("@OperateId",model.OperateId) ,  //主键id自增   
                                        new SqlParameter("@OperateName",model.OperateName) ,  //按钮名称   
                                        new SqlParameter("@Code",model.Code) ,  //Cod   
                                        new SqlParameter("@Iconic",model.Iconic) ,  //图标   
                                        new SqlParameter("@Remark",model.Remark) ,  //说明   
                                        new SqlParameter("@State",model.State)   //状态：0-无效;1-有效   
                       
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 删除 按钮表
        /// <summary>
        /// 删除 按钮表
        /// </summary>
        public int DeleteSysOperateByID (string id)
        {
           string sql = @"Delete 
                                  SysOperate
                          Where         
                                  OperateId = @OperateId
 ";                                    
                                  
            SqlParameter[] param ={ 
                                        new SqlParameter("@OperateId",id)     
                                  };
           int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
           return i;                               
        }
        #endregion

        #region 菜单按钮数量
        public int GetModuleOperateCount(string id)
        {
            var sql = @" Select Count(*) From SysModuleOperate Where OperateId=@OperateId";

            SqlParameter[] param ={
                                        new SqlParameter("@OperateId",id)
                                  };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }
        #endregion

        #region 列表 按钮表
        /// <summary>
        ///  列表 按钮表
        /// </summary>
        public List<SysOperateModel> SysOperateList ()
        {
            string sql = @" Select 
                                  OperateId ,
                                  OperateName ,
                                  Code ,
                                  Iconic ,
                                  Remark ,
                                  State 
                            From
                                  SysOperate Order By State";
            List<SysOperateModel> list = new List<SysOperateModel>();    

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            while (dr.Read())
            {
                SysOperateModel model = new SysOperateModel();
                model.OperateId = Convert.ToInt32(dr["OperateId"].ToString()); //主键id自增
                model.OperateName = dr["OperateName"].ToString(); //按钮名称
                model.Code = dr["Code"].ToString(); //Cod
                model.Iconic = dr["Iconic"].ToString(); //图标
                model.Remark = dr["Remark"].ToString(); //说明
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效

                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion  
    
        #region 获取实体 按钮表
        /// <summary>
        ///  获取实体 按钮表
        /// </summary>
        public SysOperateModel  GetModelByID (string id)
        {
           string sql = @" Select 
                                  OperateId ,
                                  OperateName ,
                                  Code ,
                                  Iconic ,
                                  Remark ,
                                  State 
                            From
                                  SysOperate
                            Where
                                  OperateId=@OperateId";
             SqlParameter[] param ={ 
                                   new SqlParameter("@OperateId",id)
                                  };
                                  
             SysOperateModel model=null;
             
             SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
             if (dr.Read())
              {
                     model = new SysOperateModel();
                     model.OperateId= Convert.ToInt32(dr["OperateId"].ToString()); //主键id自增
                     model.OperateName= dr["OperateName"].ToString(); //按钮名称
                     model.Code= dr["Code"].ToString(); //Cod
                     model.Iconic= dr["Iconic"].ToString(); //图标
                     model.Remark= dr["Remark"].ToString(); //说明
                     model.State= Convert.ToInt32(dr["State"].ToString()); //状态：0-无效;1-有效
            }           
             dr.Close();

             return model;
        }
        #endregion

        #region 是否存在相同的按钮
        public int IsHasOper(int id,string oper,string code)
        {
            if (id == 0)
            {
                var sql = "Select Count(*) From SysOperate Where Code=@Code ";
                SqlParameter[] param ={
                                // new SqlParameter("@OperateName",oper),
                                 new SqlParameter("@Code",code)
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
                var sql = "Select Count(*) From SysOperate Where  OperateId!=@OperateId And Code=@Code ";
                SqlParameter[] param ={
                             //    new SqlParameter("@OperateName",oper),
                                 new SqlParameter("@OperateId",id),
                                 new SqlParameter("@Code",code)
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

        #region 获取已选择菜单按钮
        public List<string> GetModOperList(string modid)
        {
            var sql = @"Select OperateId From SysModuleOperate WHERE ModuleId=@ModuleId";

            SqlParameter[] param ={
                                   new SqlParameter("@ModuleId",modid)
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
    }
}

