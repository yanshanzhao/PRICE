//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/10/10    1.0        HDS        新建        
//-------------------------------------------------------------------------
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Data;

using Model.Sys;
#endregion
/*********************************
 * 类名：SysImportDAL
 * 功能描述：导入配置表 数据访问层类  
 * ******************************/


namespace DAL.Sys
{
   public class SysImportDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        /// <summary>
        /// 通过导入编号 获取导入配置对象
        /// </summary>
        /// <param name="ImportNumber">导入编号</param>
        /// <returns></returns>
        public SysImportModel GetModel(string ImportNumber)
        {
            SysImportModel model=new SysImportModel();
            string sql = " SELECT ImportId,ImportNumber,TableName,ModuleName,ImportState,State,CreateDepartmentId,CreateUserId,CreateTime,DelUserId,DelTime  FROM dbo.SysImport WHERE ImportNumber=@ImportNumber AND ImportState=1 and State=1";
            SqlParameter[] param ={
                                   new SqlParameter("@ImportNumber",ImportNumber)
                                  };
            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param); 
            while (dr.Read())
            {  
                model.ImportId = Convert.ToInt32(dr["ImportId"].ToString()); //导入配置id
                model.ImportNumber = dr["ImportNumber"].ToString(); //导入编号
                model.TableName = dr["TableName"].ToString(); //导入表名称
                model.ModuleName = dr["ModuleName"].ToString(); //功能模块名称 
                model.ImportState = Convert.ToInt32(dr["ImportState"].ToString()); //状态:0-无效;1-有效
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-初始；1-提交；10-作废; 
            }
            dr.Close();
            return model;
        }

         
    }
}
