//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-08    1.0         HDS        新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysOperateModel
 * 功能描述：按钮表 实体类  
 * ******************************/
namespace Model.Sys
{
 public class SysOperateModel
    {  
            
            ///summary
            ///主键id自增
            ///summary
            public int OperateId {get;set;} 
            
            ///summary
            ///按钮名称
            ///summary
            public string OperateName {get;set;} 
            
            ///summary
            ///Cod
            ///summary
            public string Code {get;set;} 
            
            ///summary
            ///图标
            ///summary
            public string Iconic {get;set;} 
            
            ///summary
            ///说明
            ///summary
            public string Remark {get;set;} 
            
            ///summary
            ///状态：0-无效;1-有效
            ///summary
            public int State {get;set;} 
       
     }
}

