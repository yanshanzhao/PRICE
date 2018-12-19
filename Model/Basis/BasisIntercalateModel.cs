//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-06    1.0        FJK         新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：BasisIntercalateModel
 * 功能描述：部门考核设置表 实体类  
 * ******************************/

namespace Model.Basis
{
   public class BasisIntercalateModel
    {
        /// <summary>
        /// id自增主键
        /// </summary>
        public int IntercalateId { get; set; }

        /// <summary>
        /// 系统部门id
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// 考核最后日期
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 创建用户id
        /// </summary>
        public int CreateUserId { get; set; }
         
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 状态：0-无效;1-有效
        /// </summary>
        public int State { get; set; }

        #region 临时字段

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName { get; set; }

        #endregion
    }
}
