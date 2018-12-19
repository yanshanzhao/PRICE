//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/26    1.0        MH        
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysAdjunctTypeModel
 * 功能描述：附件类型 实体类  
 * ******************************/
namespace Model.Sys
{
    public class SysAdjunctTypeModel
    { 
        ///summary
        ///附件类型id
        ///summary
        public int AdjunctId { get; set; }

        ///summary
        ///附件名称
        ///summary
        public string AdjunctName { get; set; }

        ///summary
        ///排序
        ///summary
        public int Sort { get; set; }

        ///summary
        ///状态：1-有效;0-无效
        ///summary
        public int State { get; set; }

        ///summary
        ///附件类型：0-运输供应商附件
        ///summary
        public int AdjunctType { get; set; }

        ///summary
        ///创建时间
        ///summary
        public DateTime CreateTime { get; set; }

        ///summary
        ///系统公司id
        ///summary
        public int CompanyId { get; set; }

        #region 临时字段

        /// <summary>
        /// 附件类型名称
        /// </summary>
        public string AdjunctTypeName { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName { get; set; }

        #endregion
    }
}

