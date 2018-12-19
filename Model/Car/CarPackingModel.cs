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
   public class CarPackingModel
    {
        /// <summary>
        /// 包装Id
        /// </summary>
        public int PackingId { get; set; }

        /// <summary>
        /// 器具名称
        /// </summary>
        public string PackingName { get; set; }

        /// <summary>
        /// 包装代码
        /// </summary>
        public string PackingNumber { get; set; }

        /// <summary>
        /// 长度(单位米)
        /// </summary>
        public decimal Lengths { get; set; }

        /// <summary>
        /// 宽度(单位米)
        /// </summary>
        public decimal Widths { get; set; }

        /// <summary>
        /// 高度(单位米)
        /// </summary>
        public decimal Heights { get; set; }

        /// <summary>
        /// 体积(单位立方米)
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// 单价(单位 元/立方米)
        /// </summary>
        public decimal Prices { get; set; }

        /// <summary>
        /// 托体积系数
        /// </summary>
        public decimal DragDatio { get; set; }

        /// <summary>
        /// 托体积
        /// </summary>
        public decimal DragVolume { get; set; }

        /// <summary>
        /// 返空体积系数
        /// </summary>
        public decimal ReturnDatio { get; set; }

        /// <summary>
        /// 返空体积
        /// </summary>
        public decimal ReturnVolume { get; set; }

        /// <summary>
        /// 返空单价(单位 元/立方米)
        /// </summary>
        public decimal ReturnPrices { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public int SupplierId { get; set; }

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
        public string DelTime { get; set; }

        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { get; set; }

        #region 临时字段

        /// <summary>
        /// 供应商代码
        /// </summary>
        public string SupplierNumber { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 导出列表序号
        /// </summary>
        public int Number { get; set; }

        #endregion
    }
}
