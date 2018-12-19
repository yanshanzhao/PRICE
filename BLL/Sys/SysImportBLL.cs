//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-17    1.0        HDS         新建
//-------------------------------------------------------------------------
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Model.Sys;
using DAL.Sys;
#endregion
/*********************************
 * 类名：SysImportBLL
 * 功能描述：导入配置表 业务逻辑层
 * ******************************/

namespace BLL.Sys
{
   public class SysImportBLL
    {
        private SysImportDAL dal = new SysImportDAL();

        /// <summary>
        /// 根据导入配置编号返回导入配置明细
        /// </summary>
        /// <param name="ImportNumber">导入配置编号</param>
        /// <returns>导入配置明细</returns>
        public SysImportModel GetModel(string ImportNumber)
        {
            return dal.GetModel(ImportNumber);
        } 

    }
}
