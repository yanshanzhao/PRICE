//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-23    1.0         MH        新建
//2018-05-15    1.0         MH        用户角色处理
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
 * 类名：SysUserBLL
 * 功能描述：系统用户表 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
    public class SysUserBLL
    {
        SysUserDAL dal = new SysUserDAL();

        #region 添加 系统用户表
        /// <summary>
        /// 添加 系统用户表
        /// </summary>
        public int AddSysUser(SysUserModel model)
        {
            return dal.AddSysUser(model);
        }
        #endregion 

        #region 修改 系统用户表
        /// <summary>
        /// 修改 系统用户表
        /// </summary>
        public int UpdateSysUser(SysUserModel model)
        {
            return dal.UpdateSysUser(model);
        }
        #endregion

        #region 变更 系统用户状态
        public int ChangeState(int state, string uid)
        {
            return dal.ChangeState(state, uid);
        }
        #endregion

        #region 变更 系统用户密码
        public int ChangePwd(string pwd, string uid)
        {
            return dal.ChangePwd(pwd, uid);
        }
        #endregion

        #region 删除 系统用户表
        /// <summary>
        /// 删除 系统用户表
        /// </summary>
        public int DeleteSysUserByID(string id)
        {
            return dal.DeleteSysUserByID(id);
        }
        #endregion  

        #region 分页列表 系统用户表
        /// <summary>
        ///  分页列表 系统用户表
        /// </summary>
        public List<SysUserModel> SysUserPageList(int index, int size, string where)
        {
            return dal.SysUserPageList(index, size, where);
        }
        #endregion   

        #region 分页总数 系统用户表
        /// <summary>
        ///  分页总数 系统用户表
        /// </summary>
        public int SysUserPageCount(string where)
        {
            return dal.SysUserPageCount(where);
        }
        #endregion  

        #region 获取实体 系统用户表
        /// <summary>
        ///  获取实体 系统用户表
        /// </summary>
        public SysUserModel GetModelByID(string id)
        {
            return dal.GetModelByID(id);
        }
        #endregion

        #region 获取概要实体 系统用户表
        /// <summary>
        ///  获取概要实体 系统用户表
        /// </summary>
        public SysUserModel GetDetailModel(string id)
        {
            return dal.GetDetailModel(id);
        }
           #endregion

        #region 获取用户登陆信息
        /// <summary>
        /// 根据用户名和密码获取用户信息
        /// </summary>
        public SysUserModel UserLoginInfo(string user, string pwd)
        {
            return dal.UserLoginInfo(user, pwd);
        }
        #endregion

        #region 是否存在相同的用户名
        public int IsHasUser(string username)
        {
            return dal.IsHasUser(username);
        }

        public int IsHasUsers(int userid, string username)
        {
            return dal.IsHasUsers(userid, username);
        }
        #endregion

        #region 用户公司是否为后台账号
        public int IsAdmin(string cid)
        {
            return dal.IsAdmin(cid);
        }
        #endregion

        #region 批量添加角色信息
        public bool BulkCopy(DataTable dt, string tablename)
        {
            return dal.BulkCopy(dt, tablename);
        }
        public bool InsertUserRole(string uid, List<string> list)
        {
            dal.DeleteUserRoleInfo(uid);

            DataTable dt = GetRoleTable();

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    DataRow dr = dt.NewRow();
                    dr["UserId"] = Convert.ToInt32(uid);
                    dr["RoleId"] = Convert.ToInt32(item);

                    dt.Rows.Add(dr);
                }
            }

            return dal.BulkCopy(dt, "SysUserRole");
        }


        public DataTable GetRoleTable()
        {
            DataTable dt = new DataTable("SysUserRole");
            dt.Columns.Add("UserId", typeof(System.Int32));
            dt.Columns.Add("RoleId", typeof(System.Int32));
            return dt;
        }

        #endregion

        #region 更新用户角色信息
        public int UpdateRoleInfo(string uid, string roles)
        {
            return dal.UpdateRoleInfo(uid, roles);
        }
        #endregion

        #region 用户角色编号列表
         /// <summary>
         /// 用户角色编号列表
         /// </summary>
         /// <param name="uid">用户主键</param>
         /// <returns>角色编号列表</returns>
        public List<string> UserRoleList(string uid)
        {
            return dal.UserRoleList(uid);
        }
          #endregion

        #region 用户角色菜单
        public List<SysModuleModel> GetUserModule(string uid,string deparType)
        {
            return dal.GetUserModule(uid, deparType);
        }
        #endregion

        #region 判断用户是否拥有操作按钮权限
        public bool IsHasOperateAuth(string roleids, string modid, string opername)
        {
            return dal.IsHasOperateAuth(roleids, modid, opername)==0?false:true;
        }
        #endregion

        #region 获取实体 系统用户表默认账号
        /// <summary>
        ///  获取实体 系统用户表默认账号
        /// </summary>
        public SysUserModel GetDefaultInfo(string cid)
        {
            return dal.GetDefaultInfo(cid);
        }
        #endregion

        #region 数据导出
        public DataTable ExportDataTable(string where)
        {
            System.Data.DataTable dt = new DataTable();

            dt.Columns.Add(new System.Data.DataColumn("登陆名称", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("姓名", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("性别", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("手机号", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("邮箱", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("部门", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("角色", typeof(System.String)));
            dt.Columns.Add(new System.Data.DataColumn("是否为系统生成", typeof(System.String)));

            List<SysUserModel> list = dal.ExportData(where);

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();

                dr[0] = item.UserName;
                dr[1] = item.TrueName;
                dr[2] = item.Sex;
                dr[3] = item.MobileNumber;
                dr[4] = item.EmailAddress;
                dr[5] = item.DepartmentName;
                dr[6] = item.Attach == string.Empty ? "未设角色" : "已设角色";
                dr[7] = item.IsSystem == 1 ? "是" : "否";

                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion
    }
}

