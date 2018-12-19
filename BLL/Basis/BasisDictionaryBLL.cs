//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/05/30    1.0        ZBB        新建   
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

using Model.Basis;
using DAL.Basis;
#endregion
/*********************************
 * 类名：BasisDictionaryBLL
 * 功能描述：系统字典表 业务逻辑层  
 * ******************************/
namespace BLL.Basis
{
    public class BasisDictionaryBLL
    {
        BasisDictionaryDAL dal = new BasisDictionaryDAL();

        #region 添加 系统字典

        /// <summary>
        /// 添加 系统字典
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int AddBasisDictionary(BasisDictionaryModel model)
        {
            return dal.AddBasisDictionary(model);
        }
        #endregion 

        #region 修改 系统字典

        /// <summary>
        /// 修改 系统字典
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int UpdateBasisDictionary(BasisDictionaryModel model)
        {
            return dal.UpdateBasisDictionary(model);
        }
        #endregion

        #region 删除 系统字典

        /// <summary>
        /// 删除 系统字典
        /// </summary>
        /// <param name="id">系统字典id</param>
        /// <returns></returns>
        public int DeleteBasisDictionaryByID(string id)
        {
            return dal.DeleteBasisDictionaryByID(id);
        }
        #endregion

        #region 选择供应商种类信息

        #region 分页列表 选择供应商种类信息

        /// <summary>
        /// 分页列表 选择供应商种类信息
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <returns></returns>
        public List<BasisDictionaryModel> ChooseDictionaryList(int index, int size)
        {
            return dal.ChooseDictionaryList(index, size);
        }
        #endregion   

        #region 分页总数 选择供应商种类信息

        /// <summary>
        ///  分页总数 选择供应商种类信息
        /// </summary>
        public int ChooseDictionaryCount()
        {
            return dal.ChooseDictionaryCount();
        }
        #endregion

        #endregion

        #region 选择信息类型

        #region 分页列表 选择信息类型

        /// <summary>
        /// 分页列表 选择信息类型
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <returns></returns>
        public List<BasisDictionaryModel> ChooseMessageTypeList(int index, int size)
        {
            return dal.ChooseMessageTypeList(index, size);
        }
        #endregion   

        #region 分页总数 选择信息类型

        /// <summary>
        ///  分页总数 选择信息类型
        /// </summary>
        public int ChooseMessageTypeCount()
        {
            return dal.ChooseMessageTypeCount();
        }
        #endregion

        #endregion

        #region 分页列表 系统字典表

        /// <summary>
        /// 分页列表 系统字典表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public List<BasisDictionaryModel> BasisDictionaryPageList(int index, int size, string where)
        {
            return dal.BasisDictionaryPageList(index, size, where);
        }
        #endregion

        #region 分页总数 系统字典表

        /// <summary>
        /// 分页总数 系统字典表
        /// </summary>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public int BasisDictionaryPageCount(string where)
        {
            return dal.BasisDictionaryPageCount(where);
        }
        #endregion

        #region 获取实体 系统字典表  

        /// <summary>
        /// 获取实体 系统字典表 
        /// </summary>
        /// <param name="Id">系统字典id</param>
        /// <returns></returns>
        public BasisDictionaryModel GetModelByID(string Id)
        {
            return dal.GetModelByID(Id);
        }
        #endregion

        #region 变更 系统字典状态

        /// <summary>
        /// 变更 系统字典状态
        /// </summary>
        /// <param name="Id">系统字典id</param>
        /// <param name="State">字典状态</param>
        /// <returns></returns>
        public int ChangeState(string Id, int State)
        {
            return dal.ChangeState(Id, State);
        }
        #endregion

        #region 是否存在相同的字典编号

        /// <summary>
        /// 是否存在相同的字典编号
        /// </summary>
        /// <param name="Number">字典编号</param>
        /// <param name="type">字典类型</param>
        /// <returns></returns>
        public int IsDictionaryNumber(string Number, int type)
        {
            return dal.IsDictionaryNumber(Number, type);
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
            dt.Columns.Add(new System.Data.DataColumn("字典名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("字典编号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("字典类型", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("字典排序", typeof(System.Int32)));
            dt.Columns.Add(new System.Data.DataColumn("字典状态", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("使用类型", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("公司名称", typeof(System.String)));

            List<BasisDictionaryModel> list = dal.ExportData(where);

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.DictionaryName;
                dr[1] = item.DictionaryNumber;
                dr[2] = item.DictionaryTypeName;
                dr[3] = item.Sort;
                dr[4] = item.StateName;
                dr[5] = item.UseTypeName;
                dr[6] = item.CompanyName;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region 获取字典表中的字典类型数据 

        /// <summary>
        /// 获取字典表中的字典类型数据
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <returns></returns>
        public List<Dicts> GetDictLists(int companyid)
        {
            return dal.GetDictLists(companyid);
        }
        #endregion

        #region 获取字典表中的字典类型数据 

        /// <summary>
        /// 获取字典表中的字典类型数据
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <returns></returns>
        public List<Dicts> GetWorkingOrderlist(int companyid)
        {
            return dal.GetWorkingOrderlist(companyid);
        }
        #endregion
    }
}
