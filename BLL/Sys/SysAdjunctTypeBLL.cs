//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/26    1.0        MH        
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
 * 类名：SysAdjunctTypeBLL
 * 功能描述：附件类型 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
 public class SysAdjunctTypeBLL
    {  
        SysAdjunctTypeDAL dal=new SysAdjunctTypeDAL ();

        #region 添加 附件类型
        /// <summary>
        /// 添加 附件类型
        /// </summary>
        public int AddSysAdjunctType (SysAdjunctTypeModel model)
        {
            return dal.AddSysAdjunctType(model);                   
        }
        #endregion 

        #region 修改 附件类型
        /// <summary>
        /// 修改 附件类型
        /// </summary>
        public int EditSysAdjunctType(SysAdjunctTypeModel model)
        {
             return dal.EditSysAdjunctType(model);                             
        }
        #endregion 

        #region 删除 附件类型
        /// <summary>
        /// 删除 附件类型
        /// </summary>
        public int DeleteSysAdjunctTypeByID (string id)
        {
             return dal.DeleteSysAdjunctTypeByID (id);                         
        }
        #endregion  

        #region 列表 附件类型
        /// <summary>
        ///  列表 附件类型
        /// </summary>
        public List<SysAdjunctTypeModel> SysAdjunctTypePageList ( string where)
        {
             return dal.SysAdjunctTypePageList (where);                
        }
        #endregion

        #region 分页列表 附件类型

        /// <summary>
        ///  分页列表 附件类型
        /// </summary>
        public List<SysAdjunctTypeModel> SysAdjunctTypeList(int tIndex, int tSize, string tWhere)
        {
            return dal.SysAdjunctTypeList(tIndex, tSize, tWhere);
        }

        #endregion

        #region 分页总数 附件类型

        /// <summary>
        ///  分页总数 附件类型
        /// </summary>
        public int SysAdjunctTypeCount(string tWhere)
        {
            return dal.SysAdjunctTypeCount(tWhere);
        }

        #endregion

        #region 获取实体 附件类型
        /// <summary>
        ///  获取实体 附件类型
        /// </summary>
        public SysAdjunctTypeModel  GetModelByID (int id)
        {
             return dal.GetModelByID(id);
        }
        #endregion  
     }
}

