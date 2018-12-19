//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-25    1.0        MH         新建
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

using Model.Sys;
using DAL.Sys;
#endregion
/*********************************
 * 类名：SysDepartmentBLL
 * 功能描述：系统组织机构表 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
    public class SysDepartmentBLL
    {
        SysDepartmentDAL dal = new SysDepartmentDAL();

        #region 添加 系统组织机构表
        /// <summary>
        /// 添加 系统组织机构表
        /// </summary>
        public int AddSysDepartment(SysDepartmentModel model)
        {
            return dal.AddSysDepartment(model);
        }
        public int AddSysCompanyToSysDepartment(int CompanyId)
        {
            return dal.AddSysCompanyToSysDepartment(CompanyId);
        }
        #endregion 

        #region 修改 系统组织机构表
        /// <summary>
        /// 修改 系统组织机构表
        /// </summary>
        public int UpdateSysDepartment(SysDepartmentModel model)
        {
            return dal.UpdateSysDepartment(model);
        }
        #endregion

        #region 变更 机构状态
        /// <summary>
        /// 递归模式更新状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="did"></param>
        /// <returns></returns>
        public int ChangeState(int state, string did)
        {
            return dal.ChangeState(state, did);
        }
        #endregion

        #region 删除 系统组织机构表

        public int SysDepartmentUserCount(string id)
        {
            return dal.SysDepartmentUserCount(id);
        }
        /// <summary>
        /// 删除 系统组织机构表
        /// </summary>
        public int DeleteSysDepartmentByID(string id)
        {
            return dal.DeleteSysDepartmentByID(id);
        }
        #endregion  

        #region 获取 系统组织机构表
        /// <summary>
        ///  获取 系统组织机构表
        /// </summary>
        public List<SysDepartmentModel> SysDepartmentPageList(int cid, string keyword)
        {
            return dal.SysDepartmentPageList(cid, keyword);
        }
        #endregion   
     
        #region 获取实体 系统组织机构表
        /// <summary>
        ///  获取实体 系统组织机构表
        /// </summary>
        public SysDepartmentModel GetModelByID(string id)
        {
            return dal.GetModelByID(id);
        }
        #endregion

        #region 是否存在相同机构
        /// <summary>
        /// 检查相同项
        /// </summary>
        /// <param name="cid">公司编号</param>
        /// <param name="id">主键</param>
        /// <param name="pid">父类编号</param>
        /// <param name="name">机构名称</param>
        /// <returns></returns>
        public int IsHasDep(int cid, int id, int pid, string name)
        {
            return dal.IsHasDep(cid, id, pid, name);
        }
        #endregion

        #region 树形列表
        public List<TreeModel> DepTreeList(int cid)
        {
            List<SysDepartmentModel> list= dal.SysDepartmentPageList(cid, "");

            return list.Where(p => p.State == 1).Select(p => new TreeModel {pid=p.FatherId.ToString(), id = p.DepartmentId.ToString(), name = p.DepartmentName }).ToList();
        }
        #endregion

        #region 树形列表
        public List<TreeModel> DepTreeLists(int cid)
        {
            List<SysDepartmentModel> list = dal.SysDepartmentPageList(cid, "");

            return list.Select(p => new TreeModel { pid = p.FatherId.ToString(), id = p.DepartmentId.ToString(), name = p.DepartmentName }).ToList();
        }
        #endregion

        #region 分页列表 系统组织机构表
        /// <summary>
        ///  分页列表 系统组织机构表
        /// </summary>
        public List<SysDepartmentModel> SysDepartmentPageList(int index, int size, int cid, string keyword)
        {
            return dal.SysDepartmentPageList(index, size, cid, keyword);
        }
        #endregion

        #region 分页总数 系统组织机构表
        /// <summary>
        ///  分页总数 系统组织机构表
        /// </summary>
        public int SysDepartmentPageCounts(int cid, string keyword)
        {
            return dal.SysDepartmentPageCounts(cid, keyword);
        }
        #endregion

        #region 列表式机构+真实姓名

        #region 分页列表 系统组织机构表
        /// <summary>
        ///  分页列表 系统组织机构表
        /// </summary>
        public List<SysDepartmentModel> SysDepartmentList(int index, int size, int cid, string keyword)
        {
            return dal.SysDepartmentList(index, size, cid, keyword);
        }
        #endregion

        #region 分页总数 系统组织机构表
        /// <summary>
        ///  分页总数 系统组织机构表
        /// </summary>
        public int SysDepartmentCounts(int cid, string keyword)
        {
            return dal.SysDepartmentCounts(cid, keyword);
        }
        #endregion

        #endregion
    }
}

