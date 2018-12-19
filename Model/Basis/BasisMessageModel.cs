//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/08    1.0        ZBB        新建        
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：BasisMessageModel
 * 功能描述：信息预登记 实体类  
 * ******************************/

namespace Model.Basis
{
    public class BasisMessageModel
    {
        /// <summary>
        /// 系统信息id
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// 信息类型
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 信息标题
        /// </summary>
        public string MessageTitle { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// 信息备注
        /// </summary>
        public string MessageRemark { get; set; }

        /// <summary>
        /// 信息置顶
        /// </summary>
        public int MessageTop { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 有效结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态：0-初始;1-提交;2-审核通过;3-审核未通过;4-作废
        /// </summary>
        public int MessageState { get; set; }

        /// <summary>
        /// 新增时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 新增机构Id
        /// </summary>
        public int AddDepartmentId { get; set; }

        /// <summary>
        /// 新增人员id
        /// </summary>
        public int AddUserId { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string ToTime { get; set; }

        /// <summary>
        /// 审核机构id
        /// </summary>
        public int ToDepartmentId { get; set; }

        /// <summary>
        /// 审核人员id
        /// </summary>
        public int ToUserId { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string ToRemark { get; set; }

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime DelTime { get; set; }

        /// <summary>
        /// 作废机构
        /// </summary>
        public int DelDepartmentId { get; set; }

        /// <summary>
        /// 作废人员id
        /// </summary>
        public int DelUserId { get; set; }

        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 新增机构名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictionaryName { get; set; }


        /// <summary>
        /// 字典id
        /// </summary>
        public int DictionaryId { get; set; }


        /// <summary>
        /// 置顶名称
        /// </summary>
        public string MessageTopName { get; set; }

        /// <summary>
        /// 信息显示
        /// </summary>
        public string MessageStateName { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary> 
        public string SuppFileLists { get; set; }
        
        /// <summary>
        /// 下拉框数据
        /// </summary> 
        public string TemSelectData { get; set; }
    }
}
