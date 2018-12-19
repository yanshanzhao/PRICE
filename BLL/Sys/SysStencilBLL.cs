//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-11-01    1.0        ZBB        新建 
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
 * 类名：SysStencilBLL
 * 功能描述：模板维护 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
   public  class SysStencilBLL
    {
        SysStencilDAL dal = new SysStencilDAL();

        #region 添加 模板维护  
        /// <summary>
        /// 添加 模板维护
        /// </summary>
        /// <param name="tModel">实体model</param>
        /// <returns></returns>
        public int AddSysStencil(SysStencilModel tModel)
        {
            return dal.AddSysStencil(tModel);
        }
        #endregion 

        #region 分页列表 模板维护

        /// <summary>
        /// 分页列表 模板维护
        /// </summary>
        /// <param name="tIndex">页面索引</param>
        /// <param name="tSize">页面条数</param>
        /// <param name="tWhere">检索条件</param>
        /// <returns></returns>
        public List<SysStencilModel> SysStencilList(int tIndex, int tSize, string tWhere)
        {
            return dal.SysStencilList(tIndex, tSize, tWhere);
        }

        #endregion

        #region 分页总数 模板维护

        /// <summary>
        /// 分页总数 模板维护
        /// </summary>
        /// <param name="tWhere">条件</param>
        /// <returns></returns>
        public int SysStencilCount(string tWhere)
        {
            return dal.SysStencilCount(tWhere);
        }

        #endregion

        #region 获取实体 模板维护 

        /// <summary>
        /// 获取实体 模板维护 
        /// </summary>
        /// <param name="tId">主键id</param>
        /// <returns></returns>
        public SysStencilModel GetModelByID(int tId)
        {
            return dal.GetModelByID(tId);
        }

        #endregion

        #region 修改 模板维护

        /// <summary>
        /// 修改 模板维护
        /// </summary>
        /// <param name="tModel">实体model</param>
        /// <returns></returns>
        public int EditSysStencil(SysStencilModel tModel)
        {
            return dal.EditSysStencil(tModel);
        }

        #endregion

        #region 作废 状态

        /// <summary>
        /// 作废 状态
        /// </summary>
        /// <param name="StencilId">模版Id</param>
        /// <param name="delUserId">作废人id</param>
        /// <returns></returns>
        public int InvalidState(int StencilId, int delUserId)
        {
            return dal.InvalidState(StencilId, delUserId);
        }
        #endregion

        #region 删除 状态

        /// <summary>
        /// 删除 状态
        /// </summary>
        /// <param name="StencilId">模版Id</param>
        /// <param name="delUserId">作废人id</param>
        /// <returns></returns>
        public int DeleteState(int StencilId, int delUserId)
        {
            return dal.DeleteState(StencilId, delUserId);
        }
        #endregion

        #region 提交 状态
        /// <summary>
        /// 提交 状态
        /// </summary>
        /// <param name="StencilId">模版Id</param>
        /// <returns></returns>
        public int SubmitSysStencil(int StencilId)
        {
            return dal.SubmitSysStencil(StencilId);
        }
        #endregion
    }
}
