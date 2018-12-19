//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/25    1.0        ZBB        新建
//-------------------------------------------------------------------------
#region 参照
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
 * 类名：BasisMessageAdjunctDAL
 * 功能描述：基本信息附件表 数据访问层类
 * ******************************/

namespace DAL.Basis
{
    public class BasisMessageAdjunctDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();


        #region 添加 基本信息附件表
        /// <summary>
        /// 添加 基本信息附件表
        /// </summary>
        public int AddBasisMessageAdjunct(BasisMessageAdjunctModel model)
        {
            string sql = @"Insert 
                                  BasisMessageAdjunct
                                  (
                                        MessageId , 
                                        FileName ,
                                        FileUrl 
                                      
                                  )
                          Values
                                  (
                                        @MessageId , 
                                        @FileName ,
                                        @FileUrl 
                                  )";
            SqlParameter[] param ={ 
                                        new SqlParameter("@MessageId",model.MessageId) ,  //系统信息id  
                                        new SqlParameter("@FileName",model.FileName) ,  //系统信息附件文件名称   
                                        new SqlParameter("@FileUrl",model.FileUrl)   //系统信息附件文件存放地址   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 删除 基本信息附件表
        /// <summary>
        /// 删除 基本信息附件表
        /// </summary>
        public int DeleteBasisMessageAdjunctByID(string id)
        {
            // 将条件SupplierId修改为OtherId (方敬坤)

            string sql = @"Delete 
                                  BasisMessageAdjunct
                          Where         
                                  MessageId = @MessageId"
;

            SqlParameter[] param ={
                                        new SqlParameter("@MessageId",id)
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion 

        #region  基本信息附件表
        /// <summary>
        ///   基本信息附件表
        /// </summary>
        public List<BasisMessageAdjunctModel> BasisMessageAdjunctList(int id)
        {
            string sql = @" Select 
                                  MessageAdjunctId ,
                                  MessageId ,  
                                  FileName ,
                                  FileUrl 
                            From
                                  BasisMessageAdjunct
                            Where
                                  MessageId=@MessageId
                         ";
            SqlParameter[] param ={
                                   new SqlParameter("@MessageId",id)
                                  };

            List<BasisMessageAdjunctModel> list = new List<BasisMessageAdjunctModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                BasisMessageAdjunctModel model = new BasisMessageAdjunctModel();
                model.MessageAdjunctId = Convert.ToInt32(dr["MessageAdjunctId"].ToString()); //供应商附件附件Id
                model.MessageId = Convert.ToInt32(dr["MessageId"].ToString()); //供应商id
                model.FileName = dr["FileName"].ToString(); //系统信息附件文件名称
                model.FileUrl = dr["FileUrl"].ToString(); //系统信息附件文件存放地址
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion   
    }
}
