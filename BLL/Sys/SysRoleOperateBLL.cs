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

using DAL.Sys;
using Model.Sys;
using System.Data;
#endregion
/*********************************
 * 类名：SysRoleOperateBLL
 * 功能描述：角色-功能-按钮 业务逻辑层  
 * ******************************/
namespace BLL.Sys
{
    public class SysRoleOperateBLL
    {
        private SysRoleOperateDAL dal = new SysRoleOperateDAL();
        /// <summary>
        /// 返回List对象
        /// </summary>
        /// <param name="aWhere">查询条件</param>
        /// <returns></returns>
        public DataTable GetTable(string aWhere)
        {
            return dal.GetTable(aWhere);
        }
    }

}
