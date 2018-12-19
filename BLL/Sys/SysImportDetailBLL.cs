//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-10    1.0        HDS         新建
//-------------------------------------------------------------------------
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model.Sys;

using DAL.Sys;

#endregion
/*********************************
 * 类名：SysImportDetailBLL
 * 功能描述：导入配置明细表 业务逻辑层
 * ******************************/


namespace BLL.Sys
{
    public class SysImportDetailBLL
    {
       private SysImportDetailDAL dal = new SysImportDetailDAL();

        /// <summary>
        /// 根据导入配置编号返回导入配置明细
        /// </summary>
        /// <param name="ImportNumber">导入配置编号</param>
        /// <returns>导入配置明细</returns>
        public List<SysImportDetailModel> GetListModel(string ImportNumber)
        {
          return  dal.GetListModel(ImportNumber);
        }
         
    }
    public class SysImportDetaiViewlBLL
    {
        private SysImportDetaiViewlDal dal = new SysImportDetaiViewlDal();

        /// <summary>
        /// 根据导入配置编号返回导入配置列的明细
        /// </summary>
        /// <param name="ImportNumber">导入配置编号</param>
        /// <returns>导入配置明细</returns>
        public List<SysImportDetaiViewlModel> GetViewListl(string ImportNumber)
        {
            return dal.GetViewListl(ImportNumber);
        }
    }
}
