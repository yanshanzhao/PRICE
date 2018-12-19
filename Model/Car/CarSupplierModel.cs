//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , PRICE
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-12-13    1.0         YSZ       新建 
//-------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*********************************
 * 类名：CarSupplierModel
 * 功能描述：汽车物流-供应商 实体类
 * ******************************/
namespace Model.Car
{
    public class CarSupplierModel
    {
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int SupplierId { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 供应商代码
        /// </summary>
        public string SupplierNumber { get; set; }
        /// <summary>
        /// 发货联系人
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// 发货联系电话
        /// </summary>
        public string SenderPhone { get; set; }
        /// <summary>
        /// 发货人所属省份
        /// </summary>
        public string SenderProvince { get; set; }
        /// <summary>
        /// 发货人所属市
        /// </summary>
        public string SenderCity { get; set; }
        /// <summary>
        /// 发货人所属县
        /// </summary>
        public string SenderCounty { get; set; }
        /// <summary>
        /// 发货人所属镇
        /// </summary>
        public string SenderTwon { get; set; }
        /// <summary>
        /// 发货人详细地址
        /// </summary>
        public string SenderAddress { get; set; }
        /// <summary>
        /// 状态：0-删除；1-有效;
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 创建机构Id
        /// </summary>
        public int CreateDepartmentId { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        public int CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 删除用户Id
        /// </summary>
        public int DelUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DelTime { get; set; }
        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int xh { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string States { get; set; }

    }
}
