//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-10    1.0        HDS         新建
//-------------------------------------------------------------------------
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Sys;
using System.Data.SqlClient;
using DBUtility;
using System.Data;
#endregion

/*********************************
 * 类名：SysImportDetailDAL
 * 功能描述：导入配置明细表 数据访问层类
 * ******************************/

namespace DAL.Sys
{
    public class SysImportDetailDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        /// <summary>
        /// 根据导入配置编号返回导入配置明细
        /// </summary>
        /// <param name="ImportNumber">导入配置编号</param>
        /// <returns>导入配置明细</returns>
        public List<SysImportDetailModel> GetListModel(string ImportNumber)
        {
            List<SysImportDetailModel> listModel = new List<SysImportDetailModel>();
            string sql = "SELECT DetailId,ImportId,Dbcolumn,Excelcolumn,Columntype,Columnlength,Sort FROM dbo.SysImportDetail WHERE ImportId IN (SELECT ImportId FROM dbo.SysImport WHERE ImportNumber=@ImportNumber AND ImportState=1)";
            SqlParameter[] param ={
                                   new SqlParameter("@ImportNumber",ImportNumber)
                                  };
            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            SysImportDetailModel model;
            while (dr.Read())
            {
                model = new SysImportDetailModel();
                model.DetailId = Convert.ToInt32(dr["DetailId"].ToString()); //导入配置明明细Id
                model.ImportId = Convert.ToInt32(dr["ImportId"].ToString()); //导入配置id
                model.Dbcolumn = dr["Dbcolumn"].ToString(); //数据库中的列名
                model.Excelcolumn = dr["Excelcolumn"].ToString(); //excel 中的列名
                model.Columntype = Convert.ToInt32(dr["Columntype"].ToString()); //列类型：0-无;1-int;2-decimal;3-varchar;4-nvarchar;5-datetime
                model.Columnlength = Convert.ToInt32(dr["Columnlength"].ToString()); //列限长:0-无 
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //序号
                listModel.Add(model);
            }
            dr.Close();
            return listModel;
        }

        ///// <summary>
        ///// 根据导入配置编号返回导入配置明细
        ///// </summary>
        ///// <param name="ImportNumber">导入配置编号</param>
        ///// <returns>导入配置明细</returns>
        //public List<SysImportDetailModel> GetViewListl(string ImportNumber)
        //{
        //    List<SysImportDetailModel> listModel = new List<SysImportDetailModel>();
        //    string sql = "SELECT Excelcolumn FROM dbo.SysImportDetail WHERE ImportId IN (SELECT ImportId FROM dbo.SysImport WHERE ImportNumber=@ImportNumber AND ImportState=1)";
        //    SqlParameter[] param ={
        //                           new SqlParameter("@ImportNumber",ImportNumber)
        //                          };
        //    SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
        //    SysImportDetailModel model;
        //    while (dr.Read())
        //    {
        //        model = new SysImportDetailModel();
        //        model.Excelcolumn = dr["Excelcolumn"].ToString(); //页面显示
        //        listModel.Add(model);
        //    }
        //    if (listModel.Count > 0)
        //    {
        //        model = new SysImportDetailModel();
        //        model.Excelcolumn = "异常说明"; //增加异常说明 
        //        listModel.Add(model);
        //    }
        //    dr.Close();
        //    return listModel;
        //}
    }
    public class SysImportDetaiViewlDal
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();
        /// <summary>
        /// 根据导入配置编号返回导入配置明细
        /// </summary>
        /// <param name="ImportNumber">导入配置编号</param>
        /// <returns>导入配置明细</returns>
        public List<SysImportDetaiViewlModel> GetViewListl(string ImportNumber)
        {
            List<SysImportDetaiViewlModel> listModel = new List<SysImportDetaiViewlModel>();
            string sql = "SELECT Excelcolumn FROM dbo.SysImportDetail WHERE ImportId IN (SELECT ImportId FROM dbo.SysImport WHERE ImportNumber=@ImportNumber AND ImportState=1) order by Sort";
            SqlParameter[] param ={
                                   new SqlParameter("@ImportNumber",ImportNumber)
                                  };
            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            SysImportDetaiViewlModel model;
            while (dr.Read())
            {
                model = new SysImportDetaiViewlModel();
                model.ViewColumn = dr["Excelcolumn"].ToString(); //页面显示
                listModel.Add(model);
            }
            if (listModel.Count > 0)
            {
                model = new SysImportDetaiViewlModel();
                model.ViewColumn = "异常说明"; //增加异常说明 
                listModel.Add(model);
            }
            dr.Close();
            return listModel;
        }

    }
}
