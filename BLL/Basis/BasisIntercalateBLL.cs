//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-07    1.0        FJK        新建
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
 * 类名：BasisIntercalateBLL
 * 功能描述：部门考核设置表 业务逻辑层
 * ******************************/

namespace BLL.Basis
{
   public class BasisIntercalateBLL
    {
        // 部门考核设置DAL
        BasisIntercalateDAL dal = new BasisIntercalateDAL();

        #region 添加 部门考核设置表

        /// <summary>
        /// 添加 部门考核设置表
        /// </summary>
        public int AddIntercalate(BasisIntercalateModel tModel)
        {
            return dal.AddIntercalate(tModel);
        }

        #endregion 

        #region 分页列表 部门考核设置表

        /// <summary>
        ///  分页列表 部门考核设置表
        /// </summary>
        public List<BasisIntercalateModel> IntercalateList(int tIndex, int tSize, string tWhere)
        {
            return dal.IntercalateList(tIndex, tSize, tWhere);
        }

        #endregion

        #region 分页总数 部门考核设置表

        /// <summary>
        ///  分页总数 部门考核设置表
        /// </summary>
        public int IntercalateCount(string tWhere)
        {
            return dal.IntercalateCount(tWhere);
        }

        #endregion

        #region 获取实体 部门考核设置表 

        /// <summary>
        /// 获取实体根据主键ID 部门考核设置表 
        /// </summary> 
        public BasisIntercalateModel GetModelByID(int tId)
        {
            return dal.GetModelByID(tId);
        }

        /// <summary>
        /// 获取实体根据部门ID 部门考核设置表 
        /// </summary> 
        public BasisIntercalateModel GetModelByDepartmentId(int tId)
        {
            return dal.GetModelByDepartmentId(tId);
        }

        #endregion

        #region 修改 部门考核设置表

        /// <summary>
        /// 修改 部门考核设置表
        /// </summary>
        public int EditIntercalate(BasisIntercalateModel tModel)
        {
            return dal.EditIntercalate(tModel);
        }

        #endregion
         
        #region 导出 部门考核设置表

        /// <summary>
        /// 导出 部门考核设置表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable ExportDataTable(string where)
        {
            System.Data.DataTable dt = new DataTable();

            dt.Columns.Add(new System.Data.DataColumn("机构名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("考核最后日期", typeof(System.Int32)));
            dt.Columns.Add(new System.Data.DataColumn("创建时间", typeof(System.DateTime))); 
            dt.Columns.Add(new System.Data.DataColumn("状态", typeof(System.String)));

            List<BasisIntercalateModel> list = dal.ExportData(where);

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();

                dr[0] = item.DepartmentName;
                dr[1] = item.Days;
                dr[2] = item.CreateTime;
                dr[3] = item.StateName; 

                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion
    }
}
