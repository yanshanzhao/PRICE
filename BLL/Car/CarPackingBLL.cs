
//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , PRICE
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-12-12    1.0         FJK       新建 
//-------------------------------------------------------------------------
using DAL.Car;
using Model.Car;
using SRM.Model.Car;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
/*********************************
 * 类名：CarPackingBLL
 * 功能描述：汽车物流-包装器具 业务逻辑层
 * ******************************/

namespace BLL.Car
{
   public class CarPackingBLL
    {
        CarPackingDAL dal = new CarPackingDAL();

        #region 新增 汽车物流-包装器具表

        /// <summary>
        /// 添加 汽车物流-包装器具表
        /// </summary>
        public int AddCarPacking(CarPackingModel tModel)
        {
            return dal.AddCarPacking(tModel);
        }
         
        #endregion

        #region 查询列表 汽车物流-包装器具表

        /// <summary>
        ///  分页列表 汽车物流-包装器具表
        /// </summary>
        public List<CarPackingModel> CarPackingList(int tIndex, int tSize, string tWhere)
        {
            return dal.CarPackingList(tIndex, tSize, tWhere);
        }

        #endregion

        #region 查询总行数 汽车物流-包装器具表

        /// <summary>
        ///  分页总数 汽车物流-包装器具表
        /// </summary>
        public int CarPackingCount(string tWhere)
        {
            return dal.CarPackingCount(tWhere);
        }

        #endregion

        #region 获取实体 汽车物流-包装器具表 

        /// <summary>
        /// 获取实体 汽车物流-包装器具表 
        /// </summary> 
        public CarPackingModel GetModelByID(int tId)
        {
            return dal.GetModelByID(tId);
        }

        #endregion

        #region 变更状态 汽车物流-包装器具表

        /// <summary>
        /// 作废状态 汽车物流-包装器具表
        /// </summary> 
        public int InvalidState(int packingId, int delUserId)
        {
            return dal.InvalidState(packingId, delUserId);
        }
        #endregion

        #region 编辑 汽车物流-包装器具表

        /// <summary>
        /// 编辑 汽车物流-包装器具表
        /// </summary>
        public int EditCarPacking(CarPackingModel tModel)
        {
            return dal.EditCarPacking(tModel);
        }
        #endregion

        #region 导出 汽车物流-包装器具表

        /// <summary>
        /// 导出 汽车物流-包装器具表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable ExportDataTable(string where)
        {
            System.Data.DataTable dt = new DataTable();

            dt.Columns.Add(new System.Data.DataColumn("器具名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("包装代码", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("长度", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("宽度", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("高度", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("体积", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("单价", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("拖体积系数", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("拖体积", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("返空体积系数", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("返空体积", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("返空单价", typeof(System.Decimal)));
            dt.Columns.Add(new System.Data.DataColumn("供应商代码", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("供应商名称", typeof(System.String))); 

            List<CarPackingModel> list = dal.ExportData(where);

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();

                dr[0] = item.Number;
                dr[1] = item.PackingName;
                dr[2] = item.PackingNumber;
                dr[3] = item.Lengths;
                dr[4] = item.Widths;
                dr[5] = item.Heights;
                dr[6] = item.Volume;
                dr[7] = item.Prices;
                dr[8] = item.DragDatio;
                dr[9] = item.DragVolume;
                dr[10] = item.ReturnDatio;
                dr[11] = item.ReturnVolume;
                dr[12] = item.ReturnPrices;
                dr[13] = item.SupplierNumber;
                dr[14] = item.SupplierName;

                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion

        #region 供应商信息

        #region 查询列表 汽车物流-供应商信息表

        /// <summary>
        ///  分页列表 汽车物流-供应商信息表
        /// </summary>
        public List<CarSupplierModel> CarSupplierList(int tIndex, int tSize, string tWhere)
        {
            return dal.CarSupplierList(tIndex, tSize, tWhere);
        }

        #endregion

        #region 查询总行数 汽车物流-供应商信息表

        /// <summary>
        ///  分页总数 汽车物流-供应商信息表
        /// </summary>
        public int CarSupplierCount(string tWhere)
        {
            return dal.CarSupplierCount(tWhere);
        }

        #endregion

        #endregion
    }
}
