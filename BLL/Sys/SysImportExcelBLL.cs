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
using DAL.Sys;
using System.Data;
#endregion
/*********************************
 * 类名：SysImportExcelBLL
 * 功能描述：导入批次表 业务逻辑层
 * ******************************/

namespace BLL.Sys
{
    public class SysImportExcelBLL
    {
        private SysImportExcelDAL dal = new SysImportExcelDAL();
        #region 新增导入批次
        /// <summary>
        /// 传入Excel导入批次实体 返回新增的ID
        /// </summary>
        /// <param name="model">Excel导入批次实体</param>
        /// <returns>新增的ID  -1 异常;0-插入异常;</returns>
        public int AddModel(SysImportExcelModel model)
        {
            return dal.AddModel(model);
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
            return dal.UpdateModel(State, DelUserId, DelTime, ExcelId);
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
            return dal.SysImportExcelAmount(tWhere);
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
            return dal.SysImportExcelList(twhere, tSize, tIndex);
        }
        #endregion

        #region 导出Excel

        public DataTable ExportDataTable(string twhere)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new System.Data.DataColumn("导入文件", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("导入状态", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("导入时间", typeof(System.String)));
            List<SysImportExcelModel> SysImportExcelList = dal.SysImportExcelList(twhere);
            if (SysImportExcelList != null)
            {
                foreach (var item in SysImportExcelList)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.ExcelName;
                    dr[1] = item.StateName;
                    dr[2] = item.CreateTime;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion

        #region 通过执行导入SQL  返回影响行数 
        /// <summary>
        /// 通过执行导入SQL  返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            return dal.ExecuteSql(sql);
        }
        #endregion


        /// <summary>
        /// Excel 导入验证 返回 处理结果
        /// </summary>
        /// <param name="ExcelId">导入PCid</param>
        /// <param name="CompanyId">系统公司ID</param>
        /// <param name="ProcName">存储过程名称</param>
        /// <returns>处理结果</returns>
        public string ExcelValidated(int ExcelId, int CompanyId, string ProcName)
        {
            return dal.ExcelValidated(ExcelId, CompanyId, ProcName);
        }


        /// <summary>
        /// Excel 导入 返回 处理结果
        /// </summary>
        /// <param name="ExcelId">导入PCid</param> 
        /// <param name="ProcName">存储过程名称</param>
        /// <returns>导入结果 0-导入成功;1-导入异常;-1 导入异常</returns>
        public int ExcelImport(int ExcelId, string ProcName)
        {
            return dal.ExcelImport(ExcelId, ProcName);
        }

        /// <summary>
        /// 导入明细显示（只显示前200行）
        /// </summary>
        /// <param name="ExcelId">导入PCid</param>
        /// <param name="ImportNumber">导入编号</param> 
        /// <returns></returns>
        public DataTable ExcelView(int ExcelId, string ImportNumber)
        {
           return dal.ExcelDetailView(ExcelId, ImportNumber,0);//返回表内容
        }


        /// <summary>
        ///  Excel 明细导出列表
        /// </summary>
        /// <param name="ExcelId">导入PCid</param>
        /// <param name="ImportNumber">导入编号</param> 
        /// <returns></returns>
        public DataTable ExcelDetailView(int ExcelId, string ImportNumber)
        { 
            DataTable dt = new DataTable();
            List<SysImportDetaiViewlModel> lisetDetailView = new List<SysImportDetaiViewlModel>();
            SysImportDetaiViewlBLL DetailViewBLL = new SysImportDetaiViewlBLL();
            lisetDetailView = DetailViewBLL.GetViewListl(ImportNumber);//返回表头的列
            if (lisetDetailView.Count > 0)
            {
                for (int i = 0; i < lisetDetailView.Count; i++)
                {
                    dt.Columns.Add(new System.Data.DataColumn(lisetDetailView[i].ViewColumn, typeof(System.String)));
                }
            }
            DataTable RowTable= dal.ExcelDetailView(ExcelId, ImportNumber,1);//返回表内容
            if (RowTable.Rows.Count>0)
            {
                foreach (DataRow row in RowTable.Rows)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < lisetDetailView.Count; i++)
                    {
                        dr[i] = row[i];
                    }
                    dt.Rows.Add(dr);
                }
            } 
            return dt;
        }


        /// <summary>
        /// Excel 批量执行SQL语句
        /// </summary>
        /// <param name="model">导入PC信息</param>
        /// <param name="importDetailSql">导入Excel的SQL语句</param>
        /// <returns></returns>
        public int importDetailSql(SysImportExcelModel model, List<string> importDetailSql)
        {
            return dal.importDetailSql(model, importDetailSql);//返回表内容
        }
    }
}
