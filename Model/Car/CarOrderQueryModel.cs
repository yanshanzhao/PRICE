//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/12/13    1.0        HDS        
//-------------------------------------------------------------------------
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion 
/*********************************
 * 类名：CarOrderQueryModel
 * 功能描述：订单查询 实体类  
 * ******************************/
namespace Model.Car
{
    /// <summary>
    /// 订单实体类
    /// </summary>
    public class CarOrderQueryModel
    {
        /// <summary>
		/// 订单ID
		/// </summary>
		public int OrderId { set; get; }

        /// <summary>
        /// 交货单号
        /// </summary>
        public string PartNo { set; get; }

        /// <summary>
        /// 订单状态(1-加急；0-非加急)
        /// </summary>
        public int OrderState { set; get; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStateName { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { set; get; }

        /// <summary>
        /// 承运商
        /// </summary>
        public string TransportName { set; get; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public int SupplierId { set; get; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { set; get; }

        /// <summary>
        /// 供应商代码
        /// </summary>
        public string SupplierNumber { set; get; }

        /// <summary>
        /// 取货日期
        /// </summary>
        public DateTime TakeTime { set; get; }

        /// <summary>
        /// 到货日期
        /// </summary>
        public DateTime ReachTime { set; get; }

        /// <summary>
        /// 工厂代码
        /// </summary>
        public string FactoryNumber { set; get; }

        /// <summary>
        /// 工厂地址
        /// </summary>
        public string FactoryAddress { set; get; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNumber { set; get; }

        /// <summary>
        /// 取货总收入
        /// </summary>
        public decimal ReachToIncome { set; get; }

        /// <summary>
        /// 返空总收入
        /// </summary>
        public decimal ReturnToIncome { set; get; }

        /// <summary>
        /// 总收入
        /// </summary>
        public decimal TotalIncome { set; get; }

        /// <summary>
        /// 状态：0-待计算；1-已计算;10-删除;20-作废
        /// </summary>
        public int State { set; get; }

        /// <summary>
        /// 创建机构Id
        /// </summary>
        public int CreateDepartmentId { set; get; }

        /// <summary>
        /// 创建用户id
        /// </summary>
        public int CreateUserId { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 操作报价Id
        /// </summary>
        public int HandleId { set; get; }

        /// <summary>
        /// 删除用户Id
        /// </summary>
        public int DelUserId { set; get; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DelTime { set; get; }

        /// <summary>
        /// 系统公司id
        /// </summary>
        public int CompanyId { set; get; }

        /// <summary>
        /// 交货数量
        /// </summary>
        public int SumPartNumber { set; get; }

        /// <summary>
        /// 箱数
        /// </summary>
        public int SumBoxNumber { set; get; }

        /// <summary>
        /// 拖数
        /// </summary>
        public int SumDragNumber { set; get; }

        /// <summary>
        /// 报价类型：0-待计算;1-件数收入;2-取货体积收入;3-返空体积收入
        /// </summary>
        public int OfferTypes { get; set; }

        /// <summary>
        /// 报价类型
        /// </summary>
        public string OfferTypesName { get; set; } 
    }

    public class CarOrderDetailQueryModel
    { 
        /// <summary>
        /// 交货单号
        /// </summary>
        public string PartNo { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { set; get; }

        /// <summary>
		/// 零件名称
		/// </summary>
		public string PartName { set; get; }

        /// <summary>
        /// 零件代码
        /// </summary>
        public string PartNumberString { set; get; }

        /// <summary>
        /// 包装代码
        /// </summary>
        public string PackingNumber { set; get; }

        /// <summary>
        /// SNP
        /// </summary>
        public decimal SNP { set; get; }

        /// <summary>
        /// 交货数量
        /// </summary>
        public int PartNumber { set; get; }

        /// <summary>
        /// 箱数
        /// </summary>
        public int BoxNumber { set; get; }

        /// <summary>
        /// 拖数
        /// </summary>
        public int DragNumber { set; get; }

        /// <summary>
        /// 实收箱数
        /// </summary>
        public int ReceiptBoxNumber { set; get; }

        /// <summary>
        /// 实收拖数
        /// </summary>
        public int ReceiptDragNumber { set; get; }

        /// <summary>
        /// 取货收入
        /// </summary>
        public decimal ReachIncome { set; get; }

        /// <summary>
        /// 返空收入
        /// </summary>
        public decimal ReturnIncome { set; get; }

        /// <summary>
        /// 总收入
        /// </summary>
        public decimal Income { set; get; }

        /// <summary>
        /// 取货单价
        /// </summary>
        public decimal ReachPrices { set; get; }

        /// <summary>
        /// 返空单价
        /// </summary>
        public decimal ReturnPrices { set; get; }

    }
}
