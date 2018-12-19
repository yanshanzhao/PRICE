//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2017 , SayWeb
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/4/17    1.0        MH        
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

using Model.CMS;
using DAL.CMS;
#endregion
/*********************************
 * 类名：CMSSchoolInfoBLL
 * 功能描述： 签约学校预审业务逻辑层
 * ******************************/
namespace BLL.CMS
{
 public class CMSSchoolInfoBLL
    {  
        CMSSchoolInfoDAL dal=new CMSSchoolInfoDAL ();
        #region 添加 
        /// <summary>
        /// 添加 
        /// </summary>
        public int AddCMSSchoolInfo (CMSSchoolInfoModel model)
        {
             return dal.AddCMSSchoolInfo (model) ;                      
        }
        #endregion 

        #region 修改 
        /// <summary>
        /// 修改 
        /// </summary>
        public int UpdateCMSSchoolInfo (CMSSchoolInfoModel model)
        {
             return dal.UpdateCMSSchoolInfo(model);                             
        }
        #endregion 

        #region 变更状态
        public int UpdateState(CMSSchoolInfoModel model)
        {
            return dal.UpdateState(model);
        }
        #endregion

        #region 删除 
        /// <summary>
        /// 删除 
        /// </summary>
        public int DeleteCMSSchoolInfoByID (string id)
        {
             return dal.DeleteCMSSchoolInfoByID (id);                         
        }
        #endregion  

        #region 分页列表 
        /// <summary>
        ///  分页列表 
        /// </summary>
        public   List<CMSSchoolInfoModel>  CMSSchoolInfoPageList (int index, int size, string where)
        {
             return dal.CMSSchoolInfoPageList (index,size,where);                
        }
        #endregion   

        #region 分页总数 
        /// <summary>
        ///  分页总数 
        /// </summary>
        public int  CMSSchoolInfoPageCount (string where)
        {
             return dal.CMSSchoolInfoPageCount(where);              
        }
        #endregion  

        #region 获取实体 
        /// <summary>
        ///  获取实体 
        /// </summary>
        public CMSSchoolInfoModel  GetModelByID (string id)
        {
             return dal.GetModelByID(id);
        }
        #endregion  
     }
}

