//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/13    1.0        ZBB        新建   
//2018/07/26    1.1        FJK        新增公司消息通知  
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
 * 类名：BasisMessageAuditBLL
 * 功能描述：信息审核 业务逻辑层  
 * ******************************/

namespace BLL.Basis
{
    public class BasisMessageAuditBLL
    {
        BasisMessageAuditDAL dal = new BasisMessageAuditDAL();

        #region 添加 信息预登记

        /// <summary>
        /// 添加 信息预登记
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int AddBasisMessage(BasisMessageAuditModel model)
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
        public int UpdateBasisMessage(BasisMessageAuditModel model)
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
        public List<BasisMessageAuditModel> BasisMessageAuditPageList(int index, int size, string where)
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
            return dal.BasisMessagePageCount(where);
        }
        #endregion

        #region 获取实体 信息预登记表 

        /// <summary>
        /// 获取实体 信息预登记表
        /// </summary>
        /// <param name="Id">主键ID</param>
        /// <returns></returns>
        public BasisMessageAuditModel GetModelByID(string Id)
        {
            return dal.GetModelByID(Id);
        }
        #endregion

        #region 变更 信息预登记表

        /// <summary>
        /// 变更 信息预登记表
        /// <param name="Id">信息预登记ID</param>
        /// <param name="State">信息状态</param>
        /// </summary> 
        public int ChangeState(string Id, int State)
        {
            return dal.ChangeState(Id, State);
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
            dt.Columns.Add(new System.Data.DataColumn("有效期开始时间", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("有效结束时间", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("信息状态", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("新增时间", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("新增机构", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("审核时间", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("审核备注", typeof(System.String)));

            List<BasisMessageAuditModel> list = dal.ExportData(where); 

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

        #region 分页列表 信息预登记表

        /// <summary>
        /// 分页列表 信息预登记表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<BasisMessageAuditModel> GetMessageList(string where)
        {
            return dal.GetMessageList(where);
        }
        #endregion
    }
}
