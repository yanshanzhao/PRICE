//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/10/10    1.0        HDS        新建        
//-------------------------------------------------------------------------
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysImportModel
 * 功能描述：导入配置表 实体类  
 * ******************************/


namespace Model.Sys
{
    public class SysImportModel
    {
        /// <summary>
        /// 导入配置id
        /// </summary>
        public int ImportId { get; set; }

        /// <summary>
        /// 导入编号
        /// </summary>
        public string ImportNumber { get; set; }
        /// <summary>
        /// 导入表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 功能模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 状态:0-无效;1-有效
        /// </summary>
        public int ImportState { get; set; }

        /// <summary>
        /// 状态：0-初始；1-提交；10-作废;
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 创建机构id
        /// </summary>
        public int CreateDepartmentId { get; set; }
        /// <summary>
        /// 创建账号id
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 作废账号id
        /// </summary>
        public int DelUserId { get; set; }

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime DelTime { get; set; }
    }
}
