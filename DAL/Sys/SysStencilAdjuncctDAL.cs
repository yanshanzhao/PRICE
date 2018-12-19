//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/11/10    1.0        ZBB        新建   
//-------------------------------------------------------------------------
#region 参数
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
 * 类名：SysStencilAdjuncctDAL
 * 功能描述：模板维护附件 数据访问层类
 * ******************************/

namespace DAL.Sys
{
    public class SysStencilAdjuncctDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 模板维护附件

        /// <summary>
        /// 添加 模板维护附件
        /// <param name="model">实体model</param>
        /// </summary>
        public int AddSysStencilAdjuncct(SysStencilAdjuncctModel model)
        {
            string sql = @"Insert 
                                  SysStencil
                                  (   
                                        FileName, 
                                        Url 
                                      
                                  )
                          Values
                                  ( 
                                        @FileName,
                                        @Url 
                                  )";
            SqlParameter[] param ={

                                        new SqlParameter("@FileName",model.FileName) ,  //系统信息附件文件存放地址   

                                        new SqlParameter("@Url",model.Url)   //系统信息附件文件存放地址   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 删除 模板维护附件

        /// <summary>
        /// 删除 模板维护附件
        /// <param name="tId">主键id</param>
        /// </summary>
        public int DeleteSysStencilAdjuncctByID(string id)
        {
            string sql = @"Delete 
                                  SysStencil
                          Where         
                                  StencilId = @StencilId"
;

            SqlParameter[] param ={
                                        new SqlParameter("@StencilId",id)//选择id
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion 

        #region  模板维护附件

        /// <summary>
        ///   模板维护附件
        /// <param name="tId">主键id</param>
        /// </summary>
        public List<SysStencilAdjuncctModel> SysStencilAdjuncctList(int id)
        {
            string sql = @" Select  
                                  StencilId,FileName,Url 
                            From
                                  SysStencil
                            Where
                                  StencilId=@StencilId
                         ";
            SqlParameter[] param ={
                                   new SqlParameter("@StencilId",id)
                                  };

            List<SysStencilAdjuncctModel> list = new List<SysStencilAdjuncctModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysStencilAdjuncctModel model = new SysStencilAdjuncctModel();

                model.StencilId = Convert.ToInt32(dr["StencilId"].ToString()); //系统信息附件文件存放地址

                model.FileName = dr["FileName"].ToString(); //系统信息附件文件存放地址

                model.Url = dr["Url"].ToString(); //系统信息附件文件存放地址

                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion  

        #region 修改 信息预登记表

        /// <summary>
        /// 修改 信息预登记表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int EditSysStencilAdjunt(SysStencilAdjuncctModel model)
        {
            string sql = @"Update 
                                  SysStencil
                          Set 
                                 FileName = @FileName ,
                                 Url= @Url
                          Where         
                                  StencilId = @StencilId";
            SqlParameter[] param ={
                                        new SqlParameter("@StencilId",model.StencilId) ,  //系统信息id  

                                        new SqlParameter("@FileName",model.FileName) ,  //信息类型   

                                        new SqlParameter("@Url",model.Url) ,  //信息标题 
                       
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
    }
}
