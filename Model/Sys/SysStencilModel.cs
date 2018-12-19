//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-11-01    1.0        ZBB        新建                
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.ComponentModel;
#endregion
/*********************************
 * 类名：SysStencilModel
 * 功能描述：模板维护 实体类  
 * ******************************/

namespace Model.Sys
{
    public class SysStencilModel
    {
        /// <summary>
        /// 模版Id 自增
        /// </summary>
        public int StencilId { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        public string StencilName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 状态：0-初始；1-提交；10-作废;20-删除
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public string StateName { get; set; }

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


        /// <summary>
        /// 系统公司id
        /// </summary>
        public string SysStencil { get; set; }
    }
}
