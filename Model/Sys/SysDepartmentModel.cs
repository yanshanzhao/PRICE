//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-25    1.0        HDS        新建  
//2018-09-19    1.1        HDS        新增字段 DeparType              
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：SysDepartmentModel
 * 功能描述：系统组织机构表 实体类  
 * ******************************/
namespace Model.Sys
{
    public class SysDepartmentModel
    {

        ///summary
        ///组织机构id
        ///summary
        public int DepartmentId { get; set; }

        ///summary
        ///组织机构名称
        ///summary
        public string DepartmentPName { get; set; }
        ///summary
        ///组织机构名称
        ///summary
        public string DepartmentName { get; set; }

        ///summary
        ///组织机构拼写
        ///summary
        public string DepartmentSpelling { get; set; }

        ///summary
        ///父节点
        ///summary
        public int FatherId { get; set; }

        ///summary
        ///详细地址
        ///summary
        public string Address { get; set; }

        ///summary
        ///联系电话
        ///summary
        public string Tel { get; set; }

        ///summary
        ///机构编号
        ///summary
        public string DeotNo { get; set; }
        ///summary
        ///排序
        ///summary
        public int Sort { get; set; }

        ///summary
        ///系统公司id
        ///summary
        public int CompanyId { get; set; }

        ///summary
        ///状态：1-可用;0-无效
        ///summary
        public int State { get; set; }

        ///summary
        ///用户id自增主键
        ///summary
        public int UserId { get; set; }

        ///summary
        ///登录用户
        ///summary
        public string UserName { get; set; }

        ///summary
        ///真实名称
        ///summary
        public string TrueName { get; set; }
        /// <summary>
        /// 组织机构类型：0-根；1-公司级别;2-普通部门;3-供应商部门
        /// </summary>
        public int DeparType { get; set; }

        /// <summary>
        /// 组织机构类型：0-根；1-公司级别;2-普通部门;3-供应商部门
        /// </summary>
        public string DeparTypeName { get; set; }


    }
}

