//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-30    1.0         FJK        新建 
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
 
using DAL.Basis;
using Model.Basis;
#endregion
/*********************************
 * 类名：BasisKeyNodeBLL
 * 功能描述：关键节点表 业务逻辑层
 * ******************************/

namespace BLL.Basis
{
    public class BasisKeyNodeBLL
    {
        BasisKeyNodeDAL dal = new BasisKeyNodeDAL();

        #region 添加 关键节点表

        /// <summary>
        /// 添加 关键节点表
        /// </summary>
        public int AddKeyNode(BasisKeyNodeModel tModel)
        {
            return dal.AddKeyNode(tModel);
        }

        #endregion 

        #region 分页列表 关键节点表

        /// <summary>
        ///  分页列表 关键节点表
        /// </summary>
        public List<BasisKeyNodeModel> BasisKeyNodeList(int tIndex, int tSize, string tWhere)
        {
            return dal.BasisKeyNodeList(tIndex, tSize, tWhere);
        }

        #endregion

        #region 分页总数 关键节点表

        /// <summary>
        ///  分页总数 关键节点表
        /// </summary>
        public int KeyNodeCount(string tNodeName, string tCompanyName)
        {
            return dal.KeyNodeCount(tNodeName, tCompanyName);
        }

        #endregion

        #region 获取实体 关键节点表 

        /// <summary>
        /// 获取实体 关键节点表 
        /// </summary> 
        public BasisKeyNodeModel GetModelByID(int tId)
        {
            return dal.GetModelByID(tId);
        }

        /// <summary>
        /// 获取实体(根据表名称,字段名称) 关键节点表 
        /// </summary>
        /// <param name="Name">表名称</param>
        /// <param name="Columns">字段名称</param>
        /// <param name="CompanyId">公司ID</param>
        /// <returns></returns>
        public BasisKeyNodeModel GetModelByName(string Name,string Columns,int CompanyId)
        {
            return dal.GetModelByName(Name, Columns, CompanyId);
        }

        #endregion

        #region 修改 关键节点表

        /// <summary>
        /// 修改 关键节点表
        /// </summary>
        public int EditKeyNode(BasisKeyNodeModel tModel)
        {
            return dal.EditKeyNode(tModel);
        }

        #endregion

        #region 变更状态 关键节点表

        /// <summary>
        /// 变更状态 关键节点表
        /// </summary> 
        public int ChangeState(int tId, int tState)
        { 
            return dal.ChangeState(tId, tState);
        }
        #endregion

        #region 导出 关键节点表

        /// <summary>
        /// 导出 关键节点表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable ExportDataTable(string where)
        {
            System.Data.DataTable dt = new DataTable();

            dt.Columns.Add(new System.Data.DataColumn("关键节点编号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("关键节点字段", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("关键节点名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("关键节点最小值(含)", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("使用开始时间", typeof(System.DateTime)));
            dt.Columns.Add(new System.Data.DataColumn("使用结束时间", typeof(System.DateTime)));
            dt.Columns.Add(new System.Data.DataColumn("公司名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("状态", typeof(System.String)));

            List<BasisKeyNodeModel> list = dal.ExportData(where);

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();

                dr[0] = item.Name;
                dr[1] = item.Columns;
                dr[2] = item.NodeName;
                dr[3] = item.MinValue;
                dr[4] = item.BeginTime;
                dr[5] = item.EndTime;
                dr[6] = item.CompanyName;
                dr[7] = item.StateName;

                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion 
    }
}
