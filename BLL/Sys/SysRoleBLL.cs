//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-12    1.0        HDS        新建       
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
 * 类名：SysRoleBLL
 * 功能描述：系统角色 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
    public class SysRoleBLL
    {
        SysRoleDAL dal = new SysRoleDAL();

        #region 批量添加角色信息
        public bool BulkCopy(DataTable dt, string tablename)
        {
            return dal.BulkCopy(dt, tablename);
        }

        public bool InsertAllAuthInfo(string roleid, List<string> list, List<ModOperate> operlist)
        {
            dal.DeleteRoleOfAuth(roleid);

            bool res=InsertRoleModule(roleid, list);
            if (operlist.Count > 0)
            {
              bool temres=InsertModuleOperateList(roleid, operlist);

                if (temres && res)
                {
                    return true;
                }
                return false;
            }
            return res;
        }

        public bool InsertModuleOperateList(string roleid,List<ModOperate> list)
        {
            DataTable dt = GetModuleOPerateTable();

            foreach (var item in list)
            {
                foreach (var oper in item.child)
                {
                    DataRow dr = dt.NewRow();
                    dr["ModuleId"] = Convert.ToInt32(item.id);
                    dr["RoleId"] = Convert.ToInt32(roleid);
                    dr["OperateId"] = Convert.ToInt32(oper);
                    dt.Rows.Add(dr);
                }                                 
            }

            return dal.BulkCopy(dt, "SysRoleOperate");
        }

        public bool InsertRoleModule(string roleid, List<string> list)
        {
            DataTable dt = GetModuleTable();

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    DataRow dr = dt.NewRow();
                    dr["ModuleId"] = Convert.ToInt32(item);
                    dr["RoleId"] = Convert.ToInt32(roleid);

                    dt.Rows.Add(dr);
                }
            }

            return dal.BulkCopy(dt, "SysRoleModule");
        }


        public DataTable GetModuleTable()
        {
            DataTable dt = new DataTable("SysRoleModule");
            dt.Columns.Add("ModuleId", typeof(System.Int32));
            dt.Columns.Add("RoleId", typeof(System.Int32));
            return dt;
        }

        public DataTable GetModuleOPerateTable()
        {
            DataTable dt = new DataTable("SysRoleOperate");
            dt.Columns.Add("ModuleId", typeof(System.Int32));
            dt.Columns.Add("RoleId", typeof(System.Int32));
            dt.Columns.Add("OperateId", typeof(System.Int32));
            return dt;
        }
        #endregion

        #region 添加 系统角色
        /// <summary>
        /// 添加 系统角色
        /// </summary>
        public int AddSysRole(SysRoleModel model)
        {
            return dal.AddSysRole(model);
        }
        #endregion 

        #region 修改 系统角色
        /// <summary>
        /// 修改 系统角色
        /// </summary>
        public int UpdateSysRole(SysRoleModel model)
        {
            return dal.UpdateSysRole(model);
        }
        #endregion 

        #region 删除 系统角色
        /// <summary>
        /// 删除 系统角色
        /// </summary>
        public int DeleteSysRoleByID(string id)
        {
            return dal.DeleteSysRoleByID(id);
        }
        #endregion  

        #region 列表 系统角色
        /// <summary>
        /// 列表 系统角色
        /// </summary>
        public List<SysRoleModel> SysRoleList(string cid, string where)
        {
            return dal.SysRoleList(cid, where);
        }
        #endregion

        #region 获取实体 系统角色
        /// <summary>
        ///  获取实体 系统角色
        /// </summary>
        public SysRoleModel GetModelByID(string id)
        {
            return dal.GetModelByID(id);
        }
        #endregion

        #region 是否存在相同的角色
        public int IsHasRole(int id, string role)
        {
            return dal.IsHasRole(id, role);
        }
        #endregion

        #region 用户使用角色数量
        public int GetUserRoleCountById(string roleid)
        {
            return dal.GetUserRoleCountById(roleid);
        }
        #endregion

        #region 角色删除(仅授权信息）
        public int DeleteRoleOfAuth(string roleid)
        {
            return dal.DeleteRoleOfAuth(roleid);
        }
        #endregion

        #region 角色删除（所有信息)
        public int DeleteRole(string roleid)
        {
            return dal.DeleteRole(roleid);
        }
        #endregion

        #region 授权角色主键列表
        public List<string> AuthRoleList(string cid)
        {
            return dal.AuthRoleList(cid);
        }
        #endregion

        #region 已授权角色菜单列表
        public List<string> AuthRoleModuleList(string cid)
        {
            return dal.AuthRoleModuleList(cid);
        }
        #endregion

        #region 已授权角色按钮列表
        public List<ModOperates> AuthRoleModuleOperList(string cid)
        {
            return dal.AuthRoleModuleOperList(cid);
        }
        #endregion

        #region 变更 角色状态
        public int ChangeState(int state, string rid)
        {
            return dal.ChangeState(state, rid);
        }
             #endregion

        #region 获取实体 系统角色默认角色
        /// <summary>
        ///  获取实体 系统角色默认角色
        /// </summary>
        public SysRoleModel GetDefaultInfo(string cid)
        {
            return dal.GetDefaultInfo(cid);
        }
         #endregion
    }
}
