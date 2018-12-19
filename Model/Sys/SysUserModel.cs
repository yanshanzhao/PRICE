//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-23    1.0        HDS        新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysUserModel
 * 功能描述：系统用户表 实体类  
 * ******************************/
namespace Model.Sys
{
 public class SysUserModel
    {  
            
            ///summary
            ///用户id自增主键
            ///summary
            public int UserId {get;set;} 
            
            ///summary
            ///登录用户
            ///summary
            public string UserName {get;set;} 
            
            ///summary
            ///用户拼写
            ///summary
            public string UserSpelling {get;set;} 
            
            ///summary
            ///登录密码
            ///summary
            public string Password {get;set;} 
            
            ///summary
            ///真实名称
            ///summary
            public string TrueName {get;set;} 
            
            ///summary
            ///手机号码
            ///summary
            public string MobileNumber {get;set;} 
            
            ///summary
            ///电话号码
            ///summary
            public string PhoneNumber {get;set;} 
            
            ///summary
            ///QQ
            ///summary
            public string QQ {get;set;} 
            
            ///summary
            ///Email
            ///summary
            public string EmailAddress {get;set;} 
            
            ///summary
            ///其他联系方式
            ///summary
            public string OtherContact {get;set;} 
            
            ///summary
            ///省份
            ///summary
            public string Province {get;set;} 
            
            ///summary
            ///城市
            ///summary
            public string City {get;set;} 
            
            ///summary
            ///地区
            ///summary
            public string Village {get;set;} 
            
            ///summary
            ///详细地址
            ///summary
            public string Address {get;set;} 
            
            ///summary
            ///型别
            ///summary
            public string Sex {get;set;} 
            
            ///summary
            ///出生日期
            ///summary
            public string Birthday {get;set;} 
            
            ///summary
            ///入职日期
            ///summary
            public string JoinDate {get;set;} 
            
            ///summary
            ///籍贯
            ///summary
            public string Native {get;set;} 
            
            ///summary
            ///组织机构id
            ///summary
            public int DepartmentId {get;set;} 
            
            ///summary
            ///个人简介
            ///summary
            public string Expertise {get;set;} 
            
            ///summary
            ///在职状况1在职，0离职
            ///summary
            public int JobState {get;set;} 
            
            ///summary
            ///照片
            ///summary
            public string Photo {get;set;} 
            
            ///summary
            ///附件
            ///summary
            public string Attach {get;set;} 
            
            ///summary
            ///系统公司id
            ///summary
            public int CompanyId {get;set;} 
            
            ///summary
            ///创建时间
            ///summary
            public DateTime CreateTime {get;set;} 
            
            ///summary
            ///创建人
            ///summary
            public string CreatePerson {get;set;} 
            
            ///summary
            ///状态：0-无效;1-有效;2-作废
            ///summary
            public int State {get;set;}
        ///summary
        ///组织机构名称
        ///summary
        public string DepartmentName { get; set; }
        public int IsSystem { get; set; }
        public string CompanyName { get; set; }

        /// <summary>
        /// 组织机构类型：0-根；1-公司级别;2-普通部门;3-供应商部门
        /// </summary>
        public int DeparType { get; set; }
    }
}

