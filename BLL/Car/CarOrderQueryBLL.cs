//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/12/14    1.0        HDS        
//-------------------------------------------------------------------------

#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Model.Car;
using DAL.Car;
#endregion
/*********************************
 * 类名：CarOrderQueryBLL
 * 功能描述：订单查询 业务逻辑层类
 * ******************************/

namespace BLL.Car
{
    #region 订单查询
    public class CarOrderQueryBLL
    {
        //订单查询 数据访问层类
        private CarOrderQueryDAL dal = new CarOrderQueryDAL();

        /// <summary>
        /// 分页总数 订单查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int PageCouts(string where)
        {
            return dal.PageCouts(where);
        }

        /// <summary>
        /// 分页列表 订单查询
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">每页行数</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<CarOrderQueryModel> PageList(int index, int size, string where)
        {
            return dal.PageList(index, size, where);
        }

        /// <summary>
        /// 导出Excel的List<Model>对象
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public DataTable ExcelLiset(string where)
        { 
            DataTable dt = new DataTable();
            dt.Columns.Add(new System.Data.DataColumn("承运商", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("交货单号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("供应商名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("供应商代码", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("取货日期", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("到货日期", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("工厂地址", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("交货数量", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("箱数", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("拖数", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("生成收入方式", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("取货收入", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("返空收入", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("车牌号", typeof(System.String)));
            List<CarOrderQueryModel> list = dal.ExcelLiset(where);
            if (list != null)
            {
                foreach (var item in list)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.TransportName;                 //承运商
                    dr[1] = item.PartNo;                        //交货单号
                    dr[2] = item.SupplierName;                  //供应商名称
                    dr[3] = item.SupplierNumber;                //供应商代码
                    dr[4] = item.TakeTime.ToString();           //取货日期
                    dr[5] = item.ReachTime.ToString();          //到货日期
                    dr[6] = item.FactoryAddress;                //工厂地址
                    dr[7] = item.SumPartNumber.ToString();      //交货数量
                    dr[8] = item.SumBoxNumber.ToString();       //箱数
                    dr[9] = item.SumDragNumber.ToString();      //拖数
                    dr[10] = item.OfferTypesName;               //生成收入方式
                    dr[11] = item.ReachToIncome.ToString();     //取货收入
                    dr[12] = item.ReturnToIncome.ToString();    //返空收入
                    dr[13] = item.CarNumber;                    //车牌号
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
    #endregion

    #region 订单明细查询
    public class CarOrderDetailQueryBLL
    {
        //订单查询 数据访问层类
        private CarOrderDetailDAL dal = new CarOrderDetailDAL();

        /// <summary>
        /// 分页总数 订单查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public int PageCouts(string where)
        {
            return dal.PageCouts(where);
        }

        /// <summary>
        /// 分页列表 订单查询
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="size">每页行数</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<CarOrderDetailQueryModel> PageList(int index, int size, string where)
        {
            return dal.PageList(index, size, where);
        }

        /// <summary>
        /// 导出Excel的List<Model>对象
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public DataTable ExcelLiset(string where)
        { 
            DataTable dt = new DataTable();
            dt.Columns.Add(new System.Data.DataColumn("	交货单号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	订单号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	零件号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	零件名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	包装代码", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	SNP", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	交货数量", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	箱数", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	托数", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	实收箱数", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	实收托数", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	取货收入", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	返空收入", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	总收入", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	取货单价", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("	返空单价", typeof(System.String)));

            List<CarOrderDetailQueryModel> list = dal.ExcelLiset(where);
            if (list != null)
            {
                foreach (var item in list)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.PartNo;                        //	交货单号
                    dr[1] = item.OrderNumber;                   //订单号
                    dr[2] = item.PartName;                      //零件号
                    dr[3] = item.PartNumberString;              //零件名称
                    dr[4] = item.PackingNumber;                 //包装代码
                    dr[5] = item.SNP.ToString();                //SNP
                    dr[6] = item.PartNumber.ToString();         //交货数量
                    dr[7] = item.BoxNumber.ToString();          //箱数
                    dr[8] = item.DragNumber.ToString();         //托数
                    dr[9] = item.ReceiptBoxNumber.ToString();   //实收箱数
                    dr[10] = item.ReceiptDragNumber.ToString(); //实收托数
                    dr[11] = item.ReachIncome.ToString();       //取货收入
                    dr[12] = item.ReturnIncome.ToString();      //返空收入
                    dr[13] = item.Income.ToString();            //总收入
                    dr[14] = item.ReachPrices.ToString();       //取货单价
                    dr[15] = item.ReturnPrices.ToString();    	//返空单价      
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
    #endregion


}
