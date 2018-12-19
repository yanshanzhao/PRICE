//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-30    1.0        FJK         新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：BasisKeyNodeModel
 * 功能描述：关键节点表 实体类  
 * ******************************/

namespace Model.Basis
{
    public class BasisKeyNodeModel
    {
        /// <summary>
        /// id自增主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 关键节点编号:使用表名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 关键节点字段:字段名称
        /// </summary>
        public string Columns { get; set; }

        /// <summary>
        /// 关键节点名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 关键节点最小值(含)
        /// </summary>
        public decimal MinValue { get; set; }

        /// <summary>
        /// 使用开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 使用结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态：0-初始;1-使用;2-作废
        /// </summary>
        public int State { get; set; }
         
        /// <summary>
        /// 状态显示
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 系统公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 更改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
