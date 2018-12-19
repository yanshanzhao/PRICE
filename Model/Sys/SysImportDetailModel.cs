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
 * 类名：SysImportDetailModel
 * 功能描述：导入配置明细表 实体类  
 * ******************************/
namespace Model.Sys
{

    public class SysImportDetailModel
    {
        /// <summary>
        /// 导入配置明明细Id
        /// </summary>
        public int DetailId { get; set; }

        /// <summary>
        /// 导入配置id
        /// </summary>
        public int ImportId { get; set; }
        /// <summary>
        /// 数据库中的列名
        /// </summary>
        public string Dbcolumn { get; set; }

        /// <summary>
        /// excel 中的列名
        /// </summary>
        public string Excelcolumn { get; set; }

        /// <summary>
        /// 列类型：0-无;1-int;2-decimal;3-varchar;4-nvarchar;5-datetime
        /// </summary>
        public int Columntype { get; set; }

        /// <summary>
        /// 列限长:0-无
        /// </summary>
        public int Columnlength { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
    }

    public class SysImportDetaiViewlModel
    {
        /// <summary>
        /// 页面列名
        /// </summary>
        public string ViewColumn { get; set; }
    }

    public class SysImportDetaiSQLlModel
    {
        /// <summary>
        /// 列名称
        /// </summary>
        public string Dbcolumn { get; set; }
    }

  
}
