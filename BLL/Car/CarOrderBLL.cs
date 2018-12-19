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

using System.Data.SqlClient;
using System.Data;

using Model.Car;
using DAL.Car;
#endregion
/*********************************
 * 类名：CarOrderBLL
 * 功能描述：订单录入表 业务逻辑层  
 * ******************************/

namespace BLL.Car
{
   public  class CarOrderBLL
    {
        CarOrderDAL dal = new CarOrderDAL();

        #region 添加 订单录入表

        /// <summary>
        /// 添加 订单录入表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int AddCarOrder(CarOrderModel model)
        {
            return dal.AddCarOrder(model);
        }
        #endregion 

        #region 修改 订单录入表

        /// <summary>
        /// 修改 订单录入表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int UpdateCarOrder(CarOrderModel model)
        {
            return dal.UpdateCarOrder(model);
        }
        #endregion

        #region 分页列表 订单录入表

        /// <summary>
        ///  分页列表 订单录入表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public List<CarOrderModel> CarOrderList(int index, int size, string where)
        {
            return dal.CarOrderList(index, size, where);
        }
        #endregion 

        #region 分页总数 订单录入表

        /// <summary>
        /// 分页总数 订单录入表
        /// </summary>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public int CarOrderCount(string where)
        {
            return dal.CarOrderCount(where);
        }
        #endregion

        #region 获取实体 订单录入表  

        /// <summary>
        /// 获取实体 订单录入表
        /// </summary>
        /// <param name="Id">订单录入表id</param>
        /// <returns></returns>
        public CarOrderModel GetModelByID(int Id)
        {
            return dal.GetModelByID(Id);
        }
        #endregion

        #region 变更 订单录入表表中的状态

        /// <summary>
        /// 变更 订单录入表表中的状态
        /// <param name="Id">订单录入表id</param>
        /// <param name="State">状态</param>
        /// </summary> 
        public int ChangeState(int Id, int State)
        {
            return dal.ChangeState(Id, State);
        }
        #endregion 

        #region 数据导出

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public DataTable ExportDataTable(string where)
        {
            System.Data.DataTable dt = new DataTable();
            dt.Columns.Add(new System.Data.DataColumn("交货单号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("供应商名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("供应商代码", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("取货日期", typeof(System.DateTime)));
            dt.Columns.Add(new System.Data.DataColumn("到货日期", typeof(System.DateTime)));
            dt.Columns.Add(new System.Data.DataColumn("箱数", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("拖数", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("交货数量", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("车牌号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("生成报价状态", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("取货总收入", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("返空总收入", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("总收入", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("收入计算状态", typeof(System.String))); 

            List<CarOrderModel> list = dal.ExportData(where); 
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.PartNo;
                dr[1] = item.SupplierName;
                dr[2] = item.SupplierNumber;
                dr[3] = item.TakeTime;
                dr[4] = item.ReachTime;
                dr[5] = item.BoxNumber;
                dr[6] = item.DragNumber;
                dr[7] = item.PartNumber;
                dr[8] = item.CarNumber;
                dr[9] = item.OfferTypesName;
                dr[10] = item.ReachToIncome;
                dr[11] = item.ReturnToIncome;
                dr[12] = item.TotalIncome;
                dr[13] = item.StateName;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion
    }
}
