//All Rights Reserved , Copyright (C) 2018 , PRICE
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto 
//2018-12-13    1.0        FJK        新增 
//-------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*********************************
 * 类名：CarPackingModel
 * 功能描述：汽车物流-包装器具 实体类  
 * ******************************/

namespace SRM.Model.Car
{
    public class CarPartModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int PartId { get; set; }

        /// <summary>
        /// 零件名称
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 零件号
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// 供应商id
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// 件数报价单价
        /// </summary>
        public decimal NumberPrices { get; set; }

        /// <summary>
        /// 件数报价单位
        /// </summary>
        public string NumberUnit { get; set; }

        /// <summary>
        /// 体积报价单价
        /// </summary>
        public decimal VolumePrices { get; set; }

        /// <summary>
        /// 体积报价单位
        /// </summary>
        public string VolumeUnit { get; set; }

        /// <summary>
        /// 报价方式:0-件数报价;1-体积报价
        /// </summary>
        public int AuotTypes { get; set; }

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

        #region 临时字段

        #endregion
    }
}
