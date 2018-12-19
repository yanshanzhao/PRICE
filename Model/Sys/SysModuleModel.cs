//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-09    1.0        MH         新建
//2018-09-19    1.1        HDS        新增字段是否为公示菜单          
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysModuleModel
 * 功能描述：系统功能(菜单) 实体类  
 * ******************************/
namespace Model.Sys
{
    public class SysModuleModel
    {

        ///summary
        ///主键;自增
        ///summary
        public int ModuleId { get; set; }

        ///summary
        ///功能名称
        ///summary
        public string ModuleName { get; set; }
        public string ModulePName { get; set; }

        ///summary
        ///功能别名
        ///summary
        public string EnglishName { get; set; }

        ///summary
        ///上级ID
        ///summary
        public int ParentId { get; set; }

        ///summary
        ///链接
        ///summary
        public string Url { get; set; }

        ///summary
        ///图标
        ///summary
        public string Iconic { get; set; }

        ///summary
        ///排序号
        ///summary
        public int Sort { get; set; }
        /// <summary>
        /// 是否为后台菜单
        /// </summary>
        public int IsAdmin { get; set; }
        ///summary
        ///说明
        ///summary
        public string Remark { get; set; }

        ///summary
        ///创建人
        ///summary
        public string CreatePerson { get; set; }

        ///summary
        ///创建时间
        ///summary
        public DateTime CreateTime { get; set; }



        ///summary
        ///是否为公示菜单（0 否，1是）
        ///summary
        public int ModuleType { get; set; }

        
    }
}

