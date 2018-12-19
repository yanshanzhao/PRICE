//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-17    1.0        MH         新建
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
 * 类名：SysLogBLL
 * 功能描述：系统日志 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
 public class SysLogBLL
    {  
        SysLogDAL dal=new SysLogDAL ();
        #region 添加 系统日志
        /// <summary>
        /// 添加 系统日志
        /// </summary>
        public int AddSysLog (SysLogModel model)
        {
             return dal.AddSysLog (model) ;                      
        }
        #endregion 

        #region 修改 系统日志
        /// <summary>
        /// 修改 系统日志
        /// </summary>
        public int UpdateSysLog (SysLogModel model)
        {
             return dal.UpdateSysLog(model);                             
        }
        #endregion 

        #region 删除 系统日志
        /// <summary>
        /// 删除 系统日志
        /// </summary>
        public int DeleteSysLogByID (string id)
        {
             return dal.DeleteSysLogByID (id);                         
        }
        #endregion  

        #region 分页列表 系统日志
        /// <summary>
        ///  分页列表 系统日志
        /// </summary>
        public List<SysLogModel> SysLogPageList(int index, int size, string where)
        {
             return dal.SysLogPageList (index,size,where);                
        }

        public List<SysLogModel> SysLogPageLists(int index, int size, string where)
        {
            return dal.SysLogPageLists(index, size, where);
        }

        public List<SysLogModel> SysLogPageLists(int index, int size, string where,string moduleid)
        {
            return dal.SysLogPageLists(index, size, where,moduleid);
        }
        #endregion   

        #region 分页总数 系统日志
        /// <summary>
        ///  分页总数 系统日志
        /// </summary>
        public int  SysLogPageCount (string where)
        {
             return dal.SysLogPageCount(where);              
        }

        public int SysLogPageCounts(string where)
        {
            return dal.SysLogPageCounts(where);
        }

        public int SysLogPageCounts(string where,string moduleid)
        {
            return dal.SysLogPageCounts(where,moduleid);
        }
        #endregion  

        #region 获取实体 系统日志
        /// <summary>
        ///  获取实体 系统日志
        /// </summary>
        public SysLogModel  GetModelByID (string id)
        {
             return dal.GetModelByID(id);
        }
        #endregion

        #region 登陆日志分页列表
        public List<SysLogModel> SysLoginPageList(int index, int size, string where)
        {
            return dal.SysLoginPageList(index, size, where);
        }
        #endregion

        #region 登陆日志分页总数
        public int SysLoginPageCount(string where)
        {
            return dal.SysLoginPageCount(where);
        }
        #endregion
    }
}

