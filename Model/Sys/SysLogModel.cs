//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-17    1.0        HDS        新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysLogModel
 * 功能描述：系统日志 实体类  
 * ******************************/
namespace Model.Sys
{
 public class SysLogModel
    {  
            
            ///summary
            ///主键id
            ///summary
            public int LogId {get;set;} 
            
            ///summary
            ///操作人
            ///summary
            public string Operator {get;set;} 
            
            ///summary
            ///信息
            ///summary
            public string Message {get;set;} 
            
            ///summary
            ///结果
            ///summary
            public string Result {get;set;} 
            
            ///summary
            ///类型（按钮）
            ///summary
            public string Type {get;set;} 
            
            ///summary
            ///功能菜单
            ///summary
            public string Module {get;set;} 
            
            ///summary
            ///操作时间
            ///summary
            public DateTime CreateTime {get;set;} 
            
            ///summary
            ///系统公司id
            ///summary
            public int CompanyId {get;set;}

            public string CompanyName { get; set; }
       
     }
}

