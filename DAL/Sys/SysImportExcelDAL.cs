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
using Model.Sys;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
#endregion
/*********************************
 * 类名：SysImportExcelModel
 * 功能描述：Excel导入批次 数据访问层  
 * ******************************/


namespace DAL.Sys
{
    public class SysImportExcelDAL
    {

        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 新增导入批次
        /// <summary>
        /// 传入Excel导入批次实体 返回新增的ID
        /// </summary>
        /// <param name="model">Excel导入批次实体</param>
        /// <returns>新增的ID  -1 异常;0-插入异常;</returns>
        public int AddModel(SysImportExcelModel model)
        {
            string sql = "INSERT INTO [dbo].[SysImportExcel](ExcelName,ExcelUrl,State,ImportNumber,CreateDepartmentId,CreateUserId,CreateTime,CompanyId) VALUES(@ExcelName,@ExcelUrl,@State,@ImportNumber,@CreateDepartmentId,@CreateUserId,@CreateTime,@CompanyId);SELECT SCOPE_IDENTITY();";
            SqlParameter[] param ={
                                   new SqlParameter("@ExcelName",model.ExcelName),// 文件名称
                                   new SqlParameter("@ExcelUrl",model.ExcelUrl),//文件存放地址
                                   new SqlParameter("@State",model.State),//状态：0-初始；1-验证异常；5-验证成功;10-导入成功
                                   new SqlParameter("@ImportNumber",model.ImportNumber),//导入编号
                                   new SqlParameter("@CreateDepartmentId",model.CreateDepartmentId),//创建机构id
                                   new SqlParameter("@CreateUserId",model.CreateUserId),//创建账号id
                                   new SqlParameter("@CreateTime",model.CreateTime),//创建时间
                                   new SqlParameter("@CompanyId",model.CompanyId)//系统公司id
                                  };
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
            try
            {
                if (obj != null)
                {
                    return Convert.ToInt32(obj.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                return -1;
            }

        }
        #endregion

        #region 修改导入批次状态
        /// <summary>
        /// 导入批次更新
        /// </summary>
        /// <param name="State">状态：0-初始；1-验证异常；5-验证成功;10-导入成功;20-作废</param>
        /// <param name="DelUserId">作废用户id</param>
        /// <param name="DelTime">作废时间</param>
        /// <param name="ExcelId">导入批次ID</param>
        /// <returns></returns>
        public int UpdateModel(int State, int DelUserId, DateTime DelTime, int ExcelId)
        {
            string sql = string.Format(" UPDATE SysImportExcel SET State={0},DelUserId={1},DelTime='{2}' WHERE ExcelId={3}; ", State, DelUserId, DelTime, ExcelId);
            return SQLHelper.ExecuteCmd(sql, conn);
        }
        #endregion



        #region 查询--分页总数
        /// <summary>
        /// 分页总数 导出批次
        /// </summary>
        /// <param name="tWhere"></param>
        /// <returns></returns>
        public int SysImportExcelAmount(string tWhere)
        {
            string sql = " SELECT COUNT(1) FROM dbo.SysImportExcel WHERE " + tWhere;
            object obj = new object();
            try
            {
                obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, null);

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

        #region 查询--分页集合 
        /// <summary>
        /// 查询--分页集合
        /// </summary>
        /// <param name="twhere">查询条件</param>
        /// <param name="tSize">每页行数</param>
        /// <param name="tIndex">第几页</param>
        /// <returns></returns>
        public List<SysImportExcelModel> SysImportExcelList(string twhere, int tSize, int tIndex)
        {
            List<SysImportExcelModel> list = new List<SysImportExcelModel>();
            string sql = "   With TemTable As (SELECT  Row_Number() Over( Order By ExcelId DESC ) AS 't' ,ExcelId,ExcelName,ExcelUrl,State,CASE WHEN  State=0 THEN '初始'WHEN State = 1 THEN '验证异常'WHEN State = 5 THEN '验证成功'WHEN State = 10 THEN '导入成功' END StateName, ImportNumber,CreateDepartmentId,CreateUserId,CreateTime,DelUserId,DelTime,CompanyId  FROM dbo.SysImportExcel  WHERE "
                + twhere + @" ) Select* From TemTable Where t Between @Size * (@Index - 1) + 1 And @Size*@Index";
            SqlParameter[] param ={
                new SqlParameter("@Size",tSize),
                new SqlParameter("@Index",tIndex)
                                  };
            // 从数据库读取数据
            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                return null;
            }
            // 循环数据加入到数据集中
            while (dr.Read())
            {
                SysImportExcelModel model = new SysImportExcelModel();
                model.ExcelId = Convert.ToInt32(dr["ExcelId"].ToString());//导入Excel PC的id
                model.ExcelName = dr["ExcelName"].ToString();//文件名称
                model.ExcelUrl =dr["ExcelUrl"].ToString();//文件存放地址
                model.State = Convert.ToInt32(dr["State"].ToString());//状态：0-初始；1-验证异常；5-验证成功;10-导入成功;20-作废
                model.StateName = dr["StateName"].ToString();//状态：0-初始；1-验证异常；5-验证成功;10-导入成功；20-作废
                model.ImportNumber = dr["ImportNumber"].ToString();//导入编号
                model.CreateTime =Convert.ToDateTime(dr["CreateTime"].ToString());//创建时间 
                list.Add(model);
            }
            // 关闭
            dr.Close(); 
            // 返回数据集
            return list; 
        }
        #endregion

        #region 查询--导出
        public List<SysImportExcelModel> SysImportExcelList(string twhere)
        {
            List<SysImportExcelModel> list = new List<SysImportExcelModel>();
            string sql = "  SELECT  ExcelId,ExcelName,ExcelUrl,State,CASE WHEN  State=0 THEN '初始'WHEN State = 1 THEN '验证异常'WHEN State = 5 THEN '验证成功'WHEN State = 10 THEN '导入成功' END StateName, ImportNumber,CreateDepartmentId,CreateUserId,CreateTime,DelUserId,DelTime,CompanyId  FROM dbo.SysImportExcel  WHERE "
                + twhere ; 
            // 从数据库读取数据
            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            }
            catch (Exception ex)
            {
                return null;
            }
            // 循环数据加入到数据集中
            while (dr.Read())
            {
                SysImportExcelModel model = new SysImportExcelModel();
                model.ExcelId = Convert.ToInt32(dr["ExcelId"].ToString());//导入Excel PC的id
                model.ExcelName = dr["ExcelName"].ToString();//文件名称
                model.ExcelUrl = dr["ExcelUrl"].ToString();//文件存放地址
                model.State = Convert.ToInt32(dr["State"].ToString());//状态：0-初始；1-验证异常；5-验证成功;10-导入成功;20-作废
                model.StateName = dr["StateName"].ToString();//状态：0-初始；1-验证异常；5-验证成功;10-导入成功；20-作废
                model.ImportNumber = dr["ImportNumber"].ToString();//导入编号
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());//创建时间 
                list.Add(model);
            }
            // 关闭
            dr.Close();
            // 返回数据集
            return list;
        }
        #endregion

        /// <summary>
        /// 通过执行导入SQL  返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            return SQLHelper.ExecuteCmd(sql, conn);
        }


        /// <summary>
        /// Excel 导入验证 返回 处理结果
        /// </summary>
        /// <param name="ExcelId">导入PCid</param>
        /// <param name="CompanyId">系统公司ID</param>
        /// <param name="ProcName">存储过程名称</param>
        /// <returns>处理结果</returns>
        public string ExcelValidated(int ExcelId, int CompanyId,string ProcName)
        {
            SqlParameter[] parm =
           {
                new SqlParameter("@ExcelId",ExcelId),
                new SqlParameter("@CompanyId",CompanyId)
            };
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.StoredProcedure, ProcName, parm);

            if (obj == null)
            {
                return "验证异常！请重新验证！";
            }
            else
            {
                return obj.ToString();
            }
        }


        /// <summary>
        /// Excel 导入 返回 处理结果
        /// </summary>
        /// <param name="ExcelId">导入PCid</param> 
        /// <param name="ProcName">存储过程名称</param>
        /// <returns>导入结果 0-导入成功;1-导入异常;-1 导入异常</returns>
        public int ExcelImport(int ExcelId,  string ProcName)
        {
            SqlParameter[] parm =
           {
                new SqlParameter("@ExcelId",ExcelId)
            };
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.StoredProcedure, ProcName, parm);

            if (obj == null)
            {
                return -1;
            }
            else
            {
                return Convert.ToInt32(obj.ToString());
            }
        }
 

        /// <summary>
        ///  Excel 导入明细
        /// </summary>
        /// <param name="ExcelId">导入PCid</param>
        /// <param name="ImportNumber">导入编号</param> 
        /// <returns></returns>
        public DataTable ExcelDetailView(int ExcelId, string ImportNumber,int type)
        {
            SqlParameter[] parm =
           {
                new SqlParameter("@ExcelId",ExcelId),
                new SqlParameter("@ImportNumber",ImportNumber),
                new  SqlParameter("@Type",type),
            };
            DataSet set = SQLHelper.GetDataSet(conn, CommandType.StoredProcedure, "PExcelView", parm);
            if (set == null)
            {
                return null;
            }
            else
            {
                return set.Tables[0];
            }
        }


        /// <summary>
        /// Excel 批量执行SQL语句
        /// </summary>
        /// <param name="model">导入PC信息</param>
        /// <param name="importDetailSql">导入Excel的SQL语句</param>
        /// <returns></returns>
        public int importDetailSql(SysImportExcelModel model, List<string> importDetailSql)
        { 
            int successCount = 0;
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();
                string sql = "INSERT INTO [dbo].[SysImportExcel](ExcelName,ExcelUrl,State,ImportNumber,CreateDepartmentId,CreateUserId,CreateTime,CompanyId) VALUES(@ExcelName,@ExcelUrl,@State,@ImportNumber,@CreateDepartmentId,@CreateUserId,@CreateTime,@CompanyId);SELECT SCOPE_IDENTITY();";
                SqlParameter[] param ={
                                   new SqlParameter("@ExcelName",model.ExcelName),// 文件名称
                                   new SqlParameter("@ExcelUrl",model.ExcelUrl),//文件存放地址
                                   new SqlParameter("@State",model.State),//状态：0-初始；1-验证异常；5-验证成功;10-导入成功
                                   new SqlParameter("@ImportNumber",model.ImportNumber),//导入编号
                                   new SqlParameter("@CreateDepartmentId",model.CreateDepartmentId),//创建机构id
                                   new SqlParameter("@CreateUserId",model.CreateUserId),//创建账号id
                                   new SqlParameter("@CreateTime",model.CreateTime),//创建时间
                                   new SqlParameter("@CompanyId",model.CompanyId)//系统公司id
                                  };
                int exclId = 0;
                object obj = SQLHelper.ExecuteScalar(tran, CommandType.Text, sql, param);
                if (obj == null)
                {
                    tran.Rollback();
                    importDetailSql.Clear();
                    return successCount;
                }
                else
                {
                    int.TryParse(obj.ToString(), out exclId);
                    if (exclId == 0)
                    {
                        tran.Rollback();
                        importDetailSql.Clear();
                        return successCount;
                    }
                }
                string excelSql = "";
                for (int i = 0; i < importDetailSql.Count; i++)
                {
                    excelSql = string.Format(importDetailSql[i], exclId);
                    successCount= successCount+SQLHelper.ExecuteNonQuery(tran, CommandType.Text, excelSql, null);
                }
                if (successCount==0)
                {
                    tran.Rollback();
                    importDetailSql.Clear();
                    return 0;
                } 
                tran.Commit(); 
            }
            importDetailSql.Clear();
            return successCount;
        }


    }
}
