//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-25    1.0        HDS        新建                
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：TreeModel
 * 功能描述：树形列表 实体类  
 * ******************************/
namespace Model.Sys
{
    public class TreeModel
    {

       public string id { get; set; }    
       public string pid { get; set; }
       public string name { get; set; }
    }

    //菜单按钮临时类
    public class ModOperate
    {
        public string id { get; set; }
        public List<string> child { get; set; }
    }

    public class ModOperates
    {
        public string modid { get; set; }
        public string operid { get; set; }
    }
}
