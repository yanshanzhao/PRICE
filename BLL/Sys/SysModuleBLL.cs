//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-29    1.0        MH         新建
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
 * 类名：SysModuleBLL
 * 功能描述：系统功能(菜单) 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
    public class SysModuleBLL
    {
        SysModuleDAL dal = new SysModuleDAL();

        #region 添加 系统功能(菜单)
        /// <summary>
        /// 添加 系统功能(菜单)
        /// </summary>
        public int AddSysModule(SysModuleModel model)
        {
            return dal.AddSysModule(model);
        }
        #endregion 

        #region 修改 系统功能(菜单)
        /// <summary>
        /// 修改 系统功能(菜单)
        /// </summary>
        public int UpdateSysModule(SysModuleModel model)
        {
            return dal.UpdateSysModule(model);
        }
        #endregion

        #region 删除 系统功能(菜单)
        public int SysRoleMuduleCount(string id)
        {
            return dal.SysRoleMuduleCount(id);
        }

        /// <summary>
        /// 删除 系统功能(菜单)
        /// </summary>
        public int DeleteSysModuleByID(string id)
        {
            return dal.DeleteSysModuleByID(id);
        }
        #endregion  

        #region 后台管理员菜单列表
        /// <summary>
        ///  后台管理员菜单列表
        /// </summary>
        public List<SysModuleModel> SysModulePageList(string where)
        {
            return dal.SysModulePageList(where);
        }
        #endregion

        #region 非后台管理员菜单列表
        /// <summary>
        ///  非后台管理员菜单列表
        /// </summary>
        public List<SysModuleModel> SysModulePageList()
        {
            return dal.SysModulePageList();
        }
        #endregion

        #region 获取实体 系统功能(菜单)
        /// <summary>
        ///  获取实体 系统功能(菜单)
        /// </summary>
        public SysModuleModel GetModelByID(string id)
        {
            return dal.GetModelByID(id);
        }
        #endregion

        #region 树形列表
        public List<TreeModel> DepTreeList()
        {
            List<SysModuleModel> list = dal.SysModulePageList("");

            return list.Select(p => new TreeModel { pid = p.ParentId.ToString(), id = p.ModuleId.ToString(), name = p.ModuleName }).ToList();
        }
        #endregion

        #region 是否含有相同菜单
        public int IsHasModule(int id, int pid, string name)
        {
            return dal.IsHasModule(id, pid, name);
        }
        #endregion

        #region 添加菜单按钮
        public bool AddModuleOperate(string modid, string ids)
        {
            dal.DeleteModuleOperate(modid);

            DataTable dt = GetDataTable();

            string[] arr = ids.Split(',');

            int modids = Convert.ToInt32(modid);


            foreach (var item in arr)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    DataRow dr = dt.NewRow();
                    dr["ModuleId"] = modids;
                    dr["OperateId"] = Convert.ToInt32(item);

                    dt.Rows.Add(dr);
                }
            }

            return dal.BulkCopy(dt, "SysModuleOperate");

        }

        public DataTable GetDataTable()
        {
            DataTable dt = new DataTable("SysModuleOperate");
            dt.Columns.Add("ModuleId", typeof(System.Int32));
            dt.Columns.Add("OperateId", typeof(System.Int32));
            return dt;
        }
        #endregion

        #region 已选择菜单按钮列表
        public List<TreeModel> GetSelModuleOperList()
        {
            return dal.GetSelModuleOperList();
        }
        public List<TreeModel> GetSelModuleOperLists()
        {
            return dal.GetSelModuleOperLists();
        }
        #endregion

        #region 变更 菜单状态
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
        #region 变更 菜单状态
        /// <summary>
        /// 递归模式更新公示状态
        /// </summary>
        /// <param name="type"></param>
        /// <param name="did"></param>
        /// <returns></returns>
        public int ChangeType(int type, string did)
        {
            return dal.ChangeType(type, did);
        }
        #endregion
    }
}
