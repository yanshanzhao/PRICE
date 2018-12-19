//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-12    1.0        HDS        新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysRoleModel
 * 功能描述：系统角色 实体类  
 * ******************************/
namespace Model.Sys
{
 public class SysRoleModel
    {  
            
            ///summary
            ///角色id自增
            ///summary
            public int RoleId {get;set;} 
            
            ///summary
            ///角色名称
            ///summary
            public string RoleName {get;set;} 
            
            ///summary
            ///状态：0-无效;1-有效
            ///summary
            public int State {get;set;} 
            
            ///summary
            ///创建时间
            ///summary
            public DateTime CreateTime {get;set;} 
            
            ///summary
            ///创建人
            ///summary
            public string CreatePerson {get;set;} 
            
            ///summary
            ///说明
            ///summary
            public string Remark {get;set;} 
            
            ///summary
            ///系统公司id
            ///summary
            public int CompanyId {get;set;}

            public int IsSystem { get; set; }
       
     }
}

