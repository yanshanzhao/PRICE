//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/23    1.0        MH        
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
 * 类名：SysAreasBLL
 * 功能描述：区域基础表 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
 public class SysAreasBLL
    {  
        SysAreasDAL dal=new SysAreasDAL ();
        #region 添加 区域基础表
        /// <summary>
        /// 添加 区域基础表
        /// </summary>
        public int AddSysAreas (SysAreasModel model)
        {
             return dal.AddSysAreas (model) ;                      
        }
        #endregion 

        #region 修改 区域基础表
        /// <summary>
        /// 修改 区域基础表
        /// </summary>
        public int UpdateSysAreas (SysAreasModel model)
        {
             return dal.UpdateSysAreas(model);                             
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

        #region 删除 区域基础表
        /// <summary>
        /// 删除 区域基础表
        /// </summary>
        public int DeleteSysAreasByID (string id)
        {
             return dal.DeleteSysAreasByID (id);                         
        }
        #endregion  

        #region 分页列表 区域基础表
        /// <summary>
        ///  分页列表 区域基础表
        /// </summary>
        public List<SysAreasModel> SysAreasPageList (int index, int size, string where)
        {
             return dal.SysAreasPageList (index,size,where);                
        }
        #endregion   

        #region 分页总数 区域基础表
        /// <summary>
        ///  分页总数 区域基础表
        /// </summary>
        public int  SysAreasPageCount (string where)
        {
             return dal.SysAreasPageCount(where);              
        }
        #endregion  

        #region 获取实体 区域基础表
        /// <summary>
        ///  获取实体 区域基础表
        /// </summary>
        public SysAreasModel  GetModelByID (string id)
        {
             return dal.GetModelByID(id);
        }

        public List<SysAreasModel> AreasList(string keyword)
        {
            return dal.AreasList(keyword);
        }

        public List<SysAreasModel> ALLAreasList()
        {
            return dal.ALLAreasList();
        }
        #endregion

        #region 是否存在相同区域
        /// <summary>
        /// 检查相同项
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="pid">父类编号</param>
        /// <param name="name">区域名称</param>
        /// <returns></returns>
        public int IsHasAreas(int id, int pid, string name)
        {
            return dal.IsHasAreas(id, pid, name);
        }
        #endregion

        #region 树形列表
        public List<TreeModel> AreasTreeList(int cid)
        {
            List<SysAreasModel> list = dal.AreasList("");

            return list.Where(p => p.State == 1).Select(p => new TreeModel { pid = p.ParentId.ToString(), id = p.AreaId.ToString(), name = p.AreaName }).ToList();
        }

        public List<TreeModel> AreaTreeByPid(int pid)
        {
            List<SysAreasModel> list = dal.AreaListByParentId(pid);

            return list.Where(p => p.State == 1).Select(p => new TreeModel { pid = p.ParentId.ToString(), id = p.AreaId.ToString(), name = p.AreaName }).ToList();
        }
        #endregion

        #region 树形列表
        public List<TreeModel> AreasTreeLists(int cid)
        {
            List<SysAreasModel> list = dal.AreasList("");

            return list.Select(p => new TreeModel { pid = p.ParentId.ToString(), id = p.AreaId.ToString(), name = p.AreaName }).ToList();
        }
        #endregion


        /// <summary>
        /// VAreas 数量
        /// </summary>
        /// <param name="where">查询数据</param>
        /// <returns></returns>
        public int VAreasRowCount(string where)
        {
            return dal.VAreasRowCount(where);
        }

        /// <summary>
        /// VAreas 列表
        /// </summary>
        /// <param name="index">第几页</param>
        /// <param name="size">每页行数</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<VAreas> VAreasLiset(int index, int size, string where)
        {
            return dal.VAreasLiset( index,  size, where);
        }
    }
}

