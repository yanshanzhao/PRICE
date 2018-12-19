//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-16    1.0        FJK        新建
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
using Model.Sys;
#endregion
/*********************************
 * 类名：BasisLineBLL
 * 功能描述：线路维护表 业务逻辑层
 * ******************************/

namespace BLL.Basis
{
    public class BasisLineBLL
    {
        // 线路维护DAL
        BasisLineDAL dal = new BasisLineDAL();

        #region 新增 线路维护表

        /// <summary>
        /// 新增 线路维护表
        /// </summary>
        public int AddLine(BasisLineModel tModel)
        {
            return dal.AddLine(tModel);
        }

        #endregion 

        #region 分页列表 线路维护表

        /// <summary>
        ///  分页列表 线路维护表
        /// </summary>
        public List<BasisLineModel> LineList(int tIndex, int tSize, string tWhere)
        {
            return dal.LineList(tIndex, tSize, tWhere);
        }

        #endregion

        #region 分页总数 线路维护表

        /// <summary>
        ///  分页总数 线路维护表
        /// </summary>
        public int LineCount(string tWhere)
        {
            return dal.LineCount(tWhere);
        }

        #endregion

        #region 获取实体 线路维护表 

        /// <summary>
        /// 获取实体根据主键ID 线路维护表 
        /// </summary> 
        public BasisLineModel GetModelByID(int tId)
        {
            return dal.GetModelByID(tId);
        }

        #endregion

        #region 编辑 线路维护表

        /// <summary>
        /// 编辑 线路维护表
        /// </summary>
        public int EditLine(BasisLineModel tModel)
        {
            return dal.EditLine(tModel);
        }

        #endregion

        #region 变更状态 线路维护表

        /// <summary>
        /// 启用 线路维护表
        /// </summary>
        /// <param name="tLineId">主键ID</param> 
        /// <returns></returns>
        public int EnableState(int tLineId)
        {
            return dal.EnableState(tLineId);
        }

        /// <summary>
        /// 停用 线路维护表
        /// </summary>
        /// <param name="tLineId">主键ID</param>
        /// <param name="delUserId">作废账号ID</param>
        /// <returns></returns>
        public int DiscontinuationState(int tLineId, int delUserId)
        {
            return dal.DiscontinuationState(tLineId, delUserId);
        }

        /// <summary>
        /// 变更使用状态 线路维护表
        /// </summary>
        /// <param name="tLineId">主键ID</param> 
        /// <returns></returns>
        public int ChangeUseState(int tLineId)
        {
            return dal.ChangeUseState(tLineId);
        } 
        #endregion 

        #region 导出 线路维护表

        /// <summary>
        /// 导出 线路维护表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable ExportDataTable(string where)
        {
            System.Data.DataTable dt = new DataTable();

            dt.Columns.Add(new System.Data.DataColumn("线路名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("起始位置", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("结束位置", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("创建时间", typeof(System.DateTime)));
            dt.Columns.Add(new System.Data.DataColumn("状态", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("使用状态", typeof(System.String)));

            List<BasisLineModel> list = dal.ExportData(where);

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();

                dr[0] = item.LineName;
                dr[1] = item.BeginName;
                dr[2] = item.EndName;
                dr[3] = item.CreateTime;
                dr[4] = item.StateName;
                dr[5] = item.UseStateName;

                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion

        #region 区域基础表

        #region 分页列表 区域基础表

        /// <summary>
        /// 分页列表 区域基础表
        /// </summary>
        public List<SysAreasModel> AreasList(int tIndex, int tSize, string tWhere)
        {
            return dal.AreasList(tIndex, tSize, tWhere);
        }

        #endregion

        #region 分页总数 区域基础表

        /// <summary>
        /// 分页总数 区域基础表
        /// </summary>
        public int AreasCount(string tWhere)
        {
            return dal.AreasCount(tWhere);
        }

        #endregion

        #region 获取实体 区域基础表 

        /// <summary>
        /// 获取实体根据主键ID 区域基础表 
        /// </summary> 
        public SysAreasModel GetModelByAreaId(int tId)
        {
            return dal.GetModelByAreaId(tId);
        }

        #endregion

        #endregion
    }
}
