//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-20    1.0                
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysCompanyModel
 * 功能描述：系统公司表 实体类  
 * ******************************/
namespace Model.Sys
{
    public class SysCompanyModel
    {

        ///summary
        ///系统公司表id;主键
        ///summary
        public int CompanyId { get; set; }

        ///summary
        ///公司名称
        ///summary
        public string CompanyName { get; set; }

        ///summary
        ///公司编号(作为辅助使用;唯一性)
        ///summary
        public string CompanyNo { get; set; }

        ///summary
        ///创建时间
        ///summary
        public DateTime CreateTime { get; set; }

        ///summary
        ///Email
        ///summary
        public string EmailAddress { get; set; }

        ///summary
        ///手机号码
        ///summary
        public string MobileNumber { get; set; }

        ///summary
        ///电话号码
        ///summary
        public string PhoneNumber { get; set; }

        ///summary
        ///其他联系方式
        ///summary
        public string OtherContact { get; set; }

        ///summary
        ///省份
        ///summary
        public string Province { get; set; }

        ///summary
        ///城市
        ///summary
        public string City { get; set; }

        ///summary
        ///
        ///summary
        public int IsAdmin { get; set; }

        ///summary
        ///地区
        ///summary
        public string Village { get; set; }

        ///summary
        ///详细地址
        ///summary
        public string Address { get; set; }

        ///summary
        ///说明
        ///summary
        public string Remark { get; set; }

        ///summary
        ///状态：0-无效;1-有效
        ///summary
        public int State { get; set; }

        public int IsConfig { get; set; }

        /// <summary>
        /// 公司简介
        /// </summary>
        public string Expertise { get; set; }

        /// <summary>
        /// 机构id
        /// </summary>
        public int DepartmentId { get; set; }

    }
}

