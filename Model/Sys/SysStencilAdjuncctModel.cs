// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-11-10    1.0        MY        新增 
//-------------------------------------------------------------------------
#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

/*********************************
 * 类名：SysStencilAdjuncctModel
 * 功能描述：模板维护附件 实体类  
 * ******************************/

namespace Model.Sys
{
   public  class SysStencilAdjuncctModel
    {
        /// <summary>
        /// 运输供应商选择申请附件iId
        /// </summary>
        public int StencilId { get; set; }

        /// <summary>
        /// 运输供应商id
        /// </summary>
        public int TraChooseId { get; set; }

        /// <summary>
        /// 系统信息附件文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 系统信息附件文件存放地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string StencilName { get; set; }
    }
    /// <summary>
    /// 后台接收附件列表串后进行反序列化处理
    /// </summary>
    public class temSysStencilAdjuncct
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
