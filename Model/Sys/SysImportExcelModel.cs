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
 * 类名：SysImportExcelModel
 * 功能描述：Excel导入批次 实体类  
 * ******************************/


namespace Model.Sys
{
   public class SysImportExcelModel
    {
        /// <summary>
        /// 导入Excel PC的id
        /// </summary>
        public int ExcelId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string ExcelName { get; set; }
        /// <summary>
        /// 文件存放地址
        /// </summary>
        public string ExcelUrl { get; set; }

        /// <summary>
        /// 状态：0-初始；1-验证异常；5-验证成功;10-导入成功;20-作废
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 状态：0-初始；1-验证异常；5-验证成功;10-导入成功;20-作废
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 导入编号
        /// </summary>
        public string ImportNumber { get; set; }

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

        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { get; set; }
    } 
}
