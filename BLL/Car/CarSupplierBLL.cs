//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , PRICE
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-12-13    1.0         YSZ       新建 
//-------------------------------------------------------------------------
using DAL.Car;
using Model.Car;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
/*********************************
 * 类名：CarSupplierBLL
 * 功能描述：汽车物流-供应商 业务逻辑层
 * ******************************/
namespace BLL.Car
{
    public class CarSupplierBLL
    {
        //供应商DAL
        CarSupplierDAL dal = new CarSupplierDAL();
        /// <summary>
        /// 新增供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddSupplier(CarSupplierModel model)
        {
            return dal.AddSupplier(model);
        }
        /// <summary>
        /// 分页列表 供应商表
        /// </summary>
        /// <param name="tIndex"></param>
        /// <param name="tSize"></param>
        /// <param name="tWhere"></param>
        /// <returns></returns>
        public List<CarSupplierModel> SupplierList(int tIndex, int tSize, string tWhere)
        {
            return dal.SupplierList(tIndex, tSize, tWhere);
        }
        /// <summary>
        /// 分页总数
        /// </summary>
        /// <param name="tWhere"></param>
        /// <returns></returns>
        public int SupplierCount(string tWhere)
        {
            return dal.SupplierCount(tWhere);
        }
        /// <summary>
        /// 编辑供应商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int EditSupplier(CarSupplierModel model)
        {
            return dal.EditSupplier(model);
        }
        /// <summary>
        /// 作废供应商
        /// </summary>
        /// <param name="tSupplierId">删除用户Id</param>
        /// <param name="tDelUserId">删除用户Id</param>
        /// <returns></returns>
        public int InvalidState(int tSupplierId, int tDelUserId)
        {
            return dal.InvalidState(tSupplierId, tDelUserId);
        }
        /// <summary>
        /// 导出供应商
        /// </summary>
        /// <param name="tWhere"></param>
        /// <returns></returns>
        public DataTable ExportDataTable(string tWhere)
        {

            System.Data.DataTable dt = new DataTable();

            dt.Columns.Add(new System.Data.DataColumn("供应商名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("供应商代码", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("发货联系人", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("发货联系电话", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("发货人所属省份", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("发货人所属市", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("发货人所属县", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("发货人所属镇", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("发货人详细地址", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("创建时间", typeof(System.DateTime)));
            dt.Columns.Add(new System.Data.DataColumn("状态", typeof(System.String)));

            List<CarSupplierModel> list = dal.ExportData(tWhere);

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();

                dr[0] = item.SupplierName;
                dr[1] = item.SupplierNumber;
                dr[2] = item.SenderName;
                dr[3] = item.SenderPhone;
                dr[4] = item.SenderProvince;
                dr[5] = item.SenderCity;
                dr[6] = item.SenderCounty;
                dr[7] = item.SenderTwon;
                dr[8] = item.SenderAddress;
                dr[9] = item.CreateTime;
                dr[10] = item.States;

                dt.Rows.Add(dr);
            }

            return dt;
        }
        /// <summary>
        /// 根据ID返回供应商实体
        /// </summary>
        /// <param name="tId">供应商ID</param>
        /// <returns></returns>
        public CarSupplierModel GetModelByID(int tId)
        {
            return dal.GetModelByID(tId);
        }
    }
}
