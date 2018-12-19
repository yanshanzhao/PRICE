//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/4/17    1.0        MH        
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：CMSSchoolInfoModel
 * 功能描述： 实体类  
 * ******************************/
namespace Model.CMS
{
 public class CMSSchoolInfoModel
    {  
            
            ///summary
            ///学校主键
            ///summary
            public string SchoolID {get;set;} 
            
            ///summary
            ///学校名称
            ///summary
            public string SchoolName {get;set;} 
            
            ///summary
            ///校长
            ///summary
            public string SchoolMaster {get;set;} 
            
            ///summary
            ///电话
            ///summary
            public string SchoolTel {get;set;} 
            
            ///summary
            ///省
            ///summary
            public string Province {get;set;} 
            
            ///summary
            ///市
            ///summary
            public string City {get;set;} 
            
            ///summary
            ///区
            ///summary
            public string Area {get;set;} 
            
            ///summary
            ///学校地址
            ///summary
            public string SchoolAddress {get;set;} 
            
            ///summary
            ///时间限制
            ///summary
            public string TimeLimit {get;set;} 
            
            ///summary
            ///业务员
            ///summary
            public string Maker {get;set;} 
            
            ///summary
            ///业务员主键
            ///summary
            public string MakerID {get;set;} 
            
            ///summary
            ///操作时间
            ///summary
            public DateTime MakerTime {get;set;} 
            
            ///summary
            ///状态(0 未审核 1 审核失败 2 通过）
            ///summary
            public int SchoolState {get;set;} 
            
            ///summary
            ///学校应用集合
            ///summary
            public string SchoolAppCodes {get;set;} 
            
            ///summary
            ///审核批注
            ///summary
            public string Remarks {get;set;} 
            
            ///summary
            ///预留
            ///summary
            public string Memo {get;set;} 
            
            ///summary
            ///邮箱
            ///summary
            public string Email {get;set;} 
            
            ///summary
            ///手机
            ///summary
            public string Mobile {get;set;} 
       
     }
}

