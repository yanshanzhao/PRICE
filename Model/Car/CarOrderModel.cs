//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/12/13    1.0        ZBB        新建        
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
/*********************************
 * 类名：CarOrderModel
 * 功能描述：订单记录 实体类  
 * ******************************/

namespace Model.Car
{
    public class CarOrderModel
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 交货单号
        /// </summary>
        public string PartNo { get; set; }

        /// <summary>
        /// 订单状态(1-加急；0-非加急)
        /// </summary>
        public int OrderState { get; set; }

        /// <summary>
        /// 显示订单状态
        /// </summary>
        public string OrderStateName { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        public string TransportName { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// 取货日期
        /// </summary>
        public DateTime TakeTime { get; set; }

        /// <summary>
        /// 到货日期
        /// </summary>
        public DateTime ReachTime { get; set; }

        /// <summary>
        /// 工厂代码
        /// </summary>
        public string FactoryNumber { get; set; }

        /// <summary>
        /// 工厂地址
        /// </summary>
        public string FactoryAddress { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNumber { get; set; }

        /// <summary>
        /// 取货总收入
        /// </summary>
        public decimal ReachToIncome { get; set; }

        /// <summary>
        /// 返空总收入
        /// </summary>
        public decimal ReturnToIncome { get; set; }

        /// <summary>
        /// 总收入
        /// </summary>
        public decimal TotalIncome { get; set; }

        /// <summary>
        /// 状态：0-待计算；1-已计算;10-删除;20-作废
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public string StateName { get; set; }

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
        /// 操作报价Id
        /// </summary>
        public int HandleId { get; set; }

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
        /// 订单明细id
        /// </summary>
        public int OrderDetailId { get; set; }

        /// <summary>
        /// 零件Id
        /// </summary>
        public int PartId { get; set; }

        /// <summary>
        /// 包装ID
        /// </summary>
        public int PackingId { get; set; }

        /// <summary>
        /// SNP
        /// </summary>
        public decimal SNP { get; set; }

        /// <summary>
        /// 交货数量
        /// </summary>
        public int PartNumber { get; set; }

        /// <summary>
        /// 箱数
        /// </summary>
        public int BoxNumber { get; set; }

        /// <summary>
        /// 拖数
        /// </summary>
        public int DragNumber { get; set; }

        /// <summary>
        /// 实收箱数
        /// </summary>
        public int ReceiptBoxNumber { get; set; }

        /// <summary>
        /// 实收拖数
        /// </summary>
        public int ReceiptDragNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 取货收入
        /// </summary>
        public decimal ReachIncome { get; set; }

        /// <summary>
        /// 返空收入
        /// </summary>
        public decimal ReturnIncome { get; set; }

        /// <summary>
        /// 取货单价
        /// </summary>
        public decimal ReachPrices { get; set; }

        /// <summary>
        /// 返空单价
        /// </summary>
        public decimal ReturnPrices { get; set; }

        /// <summary>
        /// 报价类型:1-件数收入;2-取货体积收入;3-返空体积收入
        /// </summary>
        public int OfferTypes { get; set; }

        /// <summary>
        /// 显示报价类型
        /// </summary>
        public string OfferTypesName { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商代码
        /// </summary>
        public string SupplierNumber { get; set; }
    }
}
