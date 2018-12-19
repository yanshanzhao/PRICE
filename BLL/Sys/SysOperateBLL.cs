//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-08    1.0        MH         新建
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
 * 类名：SysOperateBLL
 * 功能描述：按钮表 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
    public class SysOperateBLL
    {
        SysOperateDAL dal = new SysOperateDAL();

        #region 添加 按钮表
        /// <summary>
        /// 添加 按钮表
        /// </summary>
        public int AddSysOperate(SysOperateModel model)
        {
            return dal.AddSysOperate(model);
        }
        #endregion 

        #region 修改 按钮表
        /// <summary>
        /// 修改 按钮表
        /// </summary>
        public int UpdateSysOperate(SysOperateModel model)
        {
            return dal.UpdateSysOperate(model);
        }
        #endregion 

        #region 删除 按钮表
        /// <summary>
        /// 删除 按钮表
        /// </summary>
        public int DeleteSysOperateByID(string id)
        {
            return dal.DeleteSysOperateByID(id);
        }
        #endregion

        #region 菜单按钮数量
        public int GetModuleOperateCount(string id)
        {
            return dal.GetModuleOperateCount(id);
        }
        #endregion

        #region 列表 按钮表
        /// <summary>
        ///  列表 按钮表
        /// </summary>
        public List<SysOperateModel> SysOperateList()
        {
            return dal.SysOperateList();
        }
        #endregion   

        #region 获取实体 按钮表
        /// <summary>
        ///  获取实体 按钮表
        /// </summary>
        public SysOperateModel GetModelByID(string id)
        {
            return dal.GetModelByID(id);
        }
        #endregion

        #region 是否存在相同的按钮
        public int IsHasOper(int id, string oper, string code)
        {
            return dal.IsHasOper(id, oper, code);
        }
        #endregion

        #region 获取已选择菜单按钮
        public List<string> GetModOperList(string modid)
        {
            return dal.GetModOperList(modid);
        }
        #endregion
    }
}

