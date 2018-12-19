//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-02    1.0         HDS         新建               
//-------------------------------------------------------------------------
#region 引用
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
 * 类名：SysRoleOperateDAL
 * 功能描述：角色-功能-按钮 数据访问层  
 * ******************************/

namespace DAL.Sys
{
   public class SysRoleOperateDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        /// <summary>
        /// 返回List对象
        /// </summary>
        /// <param name="aWhere">查询条件</param>
        /// <returns></returns>
        public DataTable GetTable(string aWhere)
        {
            string sql = @" SELECT RoleId,ModuleId,OperateId,Code   FROM [dbo].[VRoleOperate] "+ aWhere;  
            DataSet ds = SQLHelper.GetDataSet(conn, CommandType.Text, sql, null); 
            return ds.Tables[0]; 
        }
    }
}
