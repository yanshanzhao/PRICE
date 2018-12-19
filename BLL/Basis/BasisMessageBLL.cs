//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/08    1.0        ZBB        新建   
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

using Model.Basis;
using DAL.Basis;
#endregion
/*********************************
 * 类名：BasisMessageBLL
 * 功能描述：信息预登记 业务逻辑层  
 * ******************************/

namespace BLL.Basis
{
    public class BasisMessageBLL
    {
        BasisMessageDAL dal = new BasisMessageDAL();

        #region 添加 信息预登记

        /// <summary>
        /// 添加 信息预登记
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int AddBasisMessage(BasisMessageModel model)
        {
            return dal.AddBasisMessage(model);
        }
        #endregion 

        #region 修改 信息预登记

        /// <summary>
        /// 修改 信息预登记
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int UpdateBasisMessage(BasisMessageModel model)
        {
            return dal.UpdateBasisMessage(model);
        }
        #endregion

        #region 分页列表 信息预登记表

        /// <summary>
        ///  分页列表 信息预登记表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public List<BasisMessageModel> BasisMessagePageList(int index, int size, string where)
        {
            return dal.BasisMessagePageList(index, size, where);
        }
        #endregion 

        #region 分页总数 信息预登记表

        /// <summary>
        /// 分页总数 信息预登记表
        /// </summary>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public int BasisMessagePageCount(string where)
        {
            return dal.BasisMessagePageCount(where);
        }
        #endregion

        #region 获取实体 信息预登记表  

        /// <summary>
        /// 获取实体 信息预登记表
        /// </summary>
        /// <param name="Id">信息预登记id</param>
        /// <returns></returns>
        public BasisMessageModel GetModelByID(int Id)
        {
            return dal.GetModelByID(Id);
        }
        #endregion

        #region 变更 信息预登记表中的信息状态

        /// <summary>
        /// 变更 信息预登记表中的信息状态
        /// <param name="Id">信息预登记id</param>
        /// <param name="State">状态</param>
        /// </summary> 
        public int ChangeState(string Id, int State)
        {
            return dal.ChangeState(Id, State);
        }
        #endregion
         
        #region 变更 信息预登记表

        /// <summary>
        /// 变更 信息预登记表
        /// </summary>
        /// <param name="Id">信息预登记id</param>
        /// <param name="State">状态</param>
        /// <param name="DelDepartmentId">作废机构id</param>
        /// <param name="DelUserId">作废负责人</param>
        /// <param name="DelTime">作废时间</param>
        /// <returns></returns>
        public int ChangeState(string Id, int State, int DelDepartmentId, int DelUserId, DateTime DelTime)
        {
            return dal.ChangeState(Id, State, DelDepartmentId, DelUserId, DelTime);
        }
        #endregion 

        #region 分页列表 信息预登记表

        /// <summary>
        ///  分页列表 信息预登记表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public List<BasisMessageModel> BasisMessageAuditPageList(int index, int size, string where)
        {
            return dal.BasisMessageAuditPageList(index, size, where);
        }
        #endregion 

        #region 分页总数 信息预登记表

        /// <summary>
        /// 分页总数 信息预登记表
        /// </summary>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public int BasisMessageAuditPageCount(string where)
        {
            return dal.BasisMessageAuditPageCount(where);
        }
        #endregion 

        #region 数据导出

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public DataTable ExportDataTable(string where)
        {
            System.Data.DataTable dt = new DataTable();
            dt.Columns.Add(new System.Data.DataColumn("信息类型", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("信息标题", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("信息置顶", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("有效期开始时间", typeof(System.DateTime)));
            dt.Columns.Add(new System.Data.DataColumn("有效结束时间", typeof(System.DateTime)));
            dt.Columns.Add(new System.Data.DataColumn("信息状态", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("新增时间", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("新增机构", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("审核时间", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("审核备注", typeof(System.String)));

            List<BasisMessageModel> list = dal.ExportData(where);
            //信息类型、信息标题、信息置顶、有效期开始时间、有效结束时间、信息状态、新增时间、新增机构、审核时间、审核备注
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.DictionaryName;
                dr[1] = item.MessageTitle;
                dr[2] = item.MessageTopName;
                dr[3] = item.BeginTime;
                dr[4] = item.EndTime;
                dr[5] = item.MessageStateName;
                dr[6] = item.AddTime;
                dr[7] = item.DepartmentName;
                dr[8] = item.ToTime;
                dr[9] = item.ToRemark;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion
    }
}
