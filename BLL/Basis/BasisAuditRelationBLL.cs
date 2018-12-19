//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-07    1.0         FJK        新建 
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
 * 类名：BasisAuditRelationBLL
 * 功能描述：供应商审核关系维护表 业务逻辑层
 * ******************************/
  
namespace BLL.Basis
{
    public class BasisAuditRelationBLL
    {
        BasisAuditRelationDAL dal = new BasisAuditRelationDAL();

        #region 添加 供应商审核关系维护表

        /// <summary>
        /// 添加 供应商审核关系维护表
        /// </summary>
        public int AddAuditRelation(BasisAuditRelationModel tModel)
        {
            return dal.AddAuditRelation(tModel);
        }

        #endregion

        #region 分页列表 供应商审核关系维护表

        /// <summary>
        ///  分页列表 供应商审核关系维护表
        /// </summary>
        public List<BasisAuditRelationModel> AuditRelationList(int tIndex, int tSize, string tWhere)
        {
            return dal.AuditRelationList(tIndex, tSize, tWhere);
        }

        #endregion

        #region 分页总数 供应商审核关系维护表

        /// <summary>
        ///  分页总数 供应商审核关系维护表
        /// </summary>
        public int AuditRelationAmount(string tWhere)
        {
            return dal.AuditRelationAmount(tWhere);
        }

        #endregion

        #region 获取实体 供应商审核关系维护表 

        /// <summary>
        /// 获取实体 供应商审核关系维护表 
        /// </summary> 
        public BasisAuditRelationModel GetModelByID(int tId)
        {
            return dal.GetModelByID(tId);
        }

        #endregion

        #region 修改 供应商审核关系维护表

        /// <summary>
        /// 修改 供应商审核关系维护表
        /// </summary>
        public int EditAuditRelation(BasisAuditRelationModel tModel)
        {
            return dal.EditAuditRelation(tModel);
        }

        #endregion

        #region 审核流程表中同流程编号的ID集合

        /// <summary>
        /// 审核流程表中同流程编号的ID集合
        /// </summary>
        public List<int> IsExistForNumber(string tNumber)
        {
            return dal.IsExistForNumber(tNumber);
        }

        #endregion

        #region 审核表中是否有同流程编号并未审核(State = 0)数据

        /// <summary>
        /// 审核表中是否有同流程编号并未审核(State = 0)数据
        /// </summary>
        public int IsExistForAudit(string tNumber)
        {
            return dal.IsExistForAudit(tNumber);
        }

        #endregion

        #region 变更状态 供应商审核关系维护表

        /// <summary>
        /// 变更状态 供应商审核关系维护表
        /// </summary> 
        public int ChangeState(string tId)
        {
            return dal.ChangeState(tId);
        }
        #endregion

        #region 导出 供应商审核关系维护表

        /// <summary>
        /// 导出 供应商审核关系维护表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable ExportDataTable(string where)
        {
            System.Data.DataTable dt = new DataTable();

            dt.Columns.Add(new System.Data.DataColumn("类型名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("流程编号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("流程操作名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("提起机构", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("提起人员", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("审核机构", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("审核人员", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("关键控制", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("退回类型", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("结束审核", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("流程开始", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("流程状态", typeof(System.String))); 

            List<BasisAuditRelationModel> list = dal.ExportData(where);

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();

                dr[0] = item.AuditRelationTypeName;
                dr[1] = item.AuditRelationNumber;
                dr[2] = item.AuditRelationName;
                dr[3] = item.DepartmentName;
                dr[4] = item.TrueName;
                dr[5] = item.ToDepartmentName;
                dr[6] = item.ToTrueName;
                dr[7] = item.IsControlName;
                dr[8] = item.ReturnTypeName;
                dr[9] = item.EndAuditName;
                dr[10] = item.BeginNodeName;
                dr[11] = item.StateName; 

                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion

        #region 是否有匹配审核流程Model 供应商审核关系维护表 

        /// <summary>
        /// 是否有匹配审核流程Model 供应商审核关系维护表 
        /// </summary> 
        public BasisAuditRelationModel IsMatchForAudit(int auditRelationType, int state, int beginNode, int userId, int departmentId, int companyId)
        {
            return dal.IsMatchForAudit(auditRelationType, state, beginNode, userId, departmentId, companyId);
        }

        /// <summary>
        /// 是否有匹配审核流程Model 供应商审核关系维护表 
        /// </summary> 
        public BasisAuditRelationModel IsMatchForControl(int auditRelationType, int state, int beginNode, int userId, int departmentId, int companyId,string type)
        {
            return dal.IsMatchForControl(auditRelationType, state, beginNode, userId, departmentId, companyId, type);
        }
         
        #endregion

        #region 是否有下一级审核流程Model 供应商审核关系维护表 

        /// <summary>
        /// 是否有下一级审核流程Model 供应商审核关系维护表 
        /// </summary> 
        public BasisAuditRelationModel IsRelationByBeforeId(int beforeId)
        {
            return dal.IsRelationByBeforeId(beforeId);
        } 
        #endregion

    }
}
