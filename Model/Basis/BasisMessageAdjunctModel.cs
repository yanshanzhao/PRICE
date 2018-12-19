//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/25    1.0        ZBB         新建
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：BasisMessageAdjunctModel
 * 功能描述：基本信息附件表 实体类  
 * ******************************/

namespace Model.Basis
{
    public class BasisMessageAdjunctModel
    {
        ///summary
        ///系统信息附件id
        ///summary
        public int MessageAdjunctId { get; set; }

        ///summary
        ///系统信息id
        ///summary
        public int MessageId { get; set; }

        ///summary
        ///系统信息附件文件名称
        ///summary
        public string FileName { get; set; }

        ///summary
        ///系统信息附件文件存放地址
        ///summary
        public string FileUrl { get; set; }
    }

    /// <summary>
    /// 后台接收附件列表串后进行反序列化处理
    /// </summary>
    public class temfiles
    {
        /// <summary>
        /// 前台自动生成的编号
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 文件名称中不包含扩展名（前台变更文件名称时使用）
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// 文件存储路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 文件扩展名（eg  .jpg)
        /// </summary>
        public string ext { get; set; }
    }
}
