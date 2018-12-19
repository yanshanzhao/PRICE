//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-08-16    1.0        FJK         新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：BasisLineModel
 * 功能描述：线路维护表 实体类  
 * ******************************/

namespace Model.Basis
{
   public class BasisLineModel
    {
        /// <summary>
        /// id自增主键
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 起始位置id
        /// </summary>
        public int BeginId { get; set; }

        /// <summary>
        /// 结束位置id
        /// </summary>
        public int EndId { get; set; }

        /// <summary>
        /// 状态：0-无效;1-有效
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 使用状态:0-未使用（默认）;1-已使用
        /// </summary>
        public int UseState { get; set; }

        /// <summary>
        /// 创建机构id
        /// </summary>
        public int CreateDepartmentId { get; set; }

        /// <summary>
        /// 创建账号id 
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 作废账号id
        /// </summary>
        public int DelUserId { get; set; }

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime DelTime { get; set; }

        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { get; set; }

        #region 临时字段

        /// <summary>
        /// 起始位置名称
        /// </summary>
        public string BeginName { get; set; }

        /// <summary>
        /// 结束位置名称
        /// </summary>
        public string EndName { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 使用状态名称
        /// </summary>
        public string UseStateName { get; set; }

        #endregion
    }
}
