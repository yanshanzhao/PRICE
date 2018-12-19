//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-07    1.0        FJK        新建      
//2018-06-27    1.1        FJK        新增BeforeId            
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：BasisAuditRelationModel
 * 功能描述：供应商审核关系维护表 实体类  
 * ******************************/


namespace Model.Basis
{
    public class BasisAuditRelationModel
    {
        /// <summary>
        /// id自增主键
        /// </summary>
        public int AuditRelationId { get; set; }

        /// <summary>
        /// 供应商审核关系类型id：1-仓储开发审核;2-仓储选择审核;3-运输开发审核;4-运输选择审核;5-运输选择评分审核;6-运输运作准备审核;
        /// </summary>
        public int AuditRelationType { get; set; }

        /// <summary>
        /// 供应商审核关系类型名称
        /// </summary>
        public string AuditRelationTypeName { get; set; }

        /// <summary>
        /// 供应商审核关系编号
        /// </summary>
        public string AuditRelationNumber { get; set; }

        /// <summary>
        /// 供应商审核关系名称：中心经理审核、总部供应链规划经理审核、总监审批、总监审核和总经理审批
        /// </summary>
        public string AuditRelationName { get; set; }

        /// <summary>
        /// 提起机构id
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// 提起机构名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 提起人员id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 提起人员名称
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 审核机构id
        /// </summary>
        public int ToDepartmentId { get; set; }

        /// <summary>
        /// 审核机构名称
        /// </summary>
        public string ToDepartmentName { get; set; }

        /// <summary>
        /// 审核人员id
        /// </summary>
        public int ToUserId { get; set; }

        /// <summary>
        /// 审核人员名称
        /// </summary>
        public string ToTrueName { get; set; }

        /// <summary>
        /// 关键控制：0-否;1-是
        /// </summary>
        public int IsControl { get; set; }

        /// <summary>
        /// 关键控制名称
        /// </summary>
        public string IsControlName { get; set; }

        /// <summary>
        /// 退回类型(0-无;1-退回申请人;2-退回上一个人；3-结束审核)
        /// </summary>
        public int ReturnType { get; set; }

        /// <summary>
        /// 退回类型名称
        /// </summary>
        public string ReturnTypeName { get; set; }

        /// <summary>
        /// 结束审核:0-否;1-是
        /// </summary>
        public int EndAudit { get; set; }

        /// <summary>
        /// 结束审核名称
        /// </summary>
        public string EndAuditName { get; set; }

        /// <summary>
        /// 流程开始节点：0-否;1-是
        /// </summary>
        public int BeginNode { get; set; }

        /// <summary>
        /// 流程开始节点名称
        /// </summary>
        public string BeginNodeName { get; set; }

        /// <summary>
        /// 上一个流程id
        /// </summary>
        public int BeforeId { get; set; }

        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { get; set; }
         
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; } 

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName { get; set; } 
    }
}
