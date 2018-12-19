//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-02    1.0         HDS         新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysRoleOperateModel
 * 功能描述：角色-功能-按钮 实体类  
 * ******************************/

namespace Model.Sys
{
   public class SysRoleOperateModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 功能菜单ID
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// 按钮ID
        /// </summary>
        public int OperateId { get; set; }

        /// <summary>
        /// 按钮Cod
        /// </summary>
        public string Code { get; set; }

    }
}
