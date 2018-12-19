//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/11/10    1.0        ZBB        新建   
//-------------------------------------------------------------------------
#region 参数
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
 * 类名：SysStencilAdjuncctBLL
 * 功能描述：模板维护附件 业务逻辑层  
 * ******************************/

namespace BLL.Sys
{
    public class SysStencilAdjuncctBLL
    {
        SysStencilAdjuncctDAL dal = new SysStencilAdjuncctDAL();

        #region 添加 模板维护附件

        /// <summary>
        /// 添加 模板维护附件
        /// <param name="model">实体model</param>
        /// </summary>
        public int AddSysStencilAdjunt(SysStencilAdjuncctModel model)
        {
            return dal.AddSysStencilAdjuncct(model);
        }
        /// <summary>
        /// 修改 模板维护
        /// </summary>
        /// <param name="tModel">实体model</param>
        /// <returns></returns>
        public int EditSysStencilAdjunt(SysStencilAdjuncctModel tModel)
        {
            return dal.EditSysStencilAdjunt(tModel);
        }
        #endregion 

        #region 删除 模板维护附件

        /// <summary>
        /// 删除 模板维护附件
        /// <param name="tId">主键id</param>
        /// </summary>
        public int DeleteSysStencilAdjuntByID(string id)
        {
            return dal.DeleteSysStencilAdjuncctByID(id);
        }
        #endregion  

        #region  模板维护附件

        /// <summary>
        ///   模板维护附件
        /// <param name="tId">主键id</param>
        /// </summary>
        public List<SysStencilAdjuncctModel> SysStencilAdjuntList(int id)
        {
            return dal.SysStencilAdjuncctList(id);
        }

        public List<temSysStencilAdjuncct> SysStencilAdjuntFileList(int id)
        {
            List<SysStencilAdjuncctModel> list = dal.SysStencilAdjuncctList(id);
            List<temSysStencilAdjuncct> temlist = new List<temSysStencilAdjuncct>();
            foreach (var item in list)
            {
                temSysStencilAdjuncct tem = new temSysStencilAdjuncct();
                tem.path = item.Url;
                tem.id = item.StencilId.ToString();

                int dot = item.FileName.LastIndexOf(".");

                if (dot == -1)
                {
                    tem.ext = string.Empty;
                    tem.filename = item.FileName;
                }
                else
                {
                    string filename = item.FileName.Substring(0, dot);
                    string ext = item.FileName.Substring(dot, item.FileName.Length - dot);

                    tem.ext = ext;
                    tem.filename = filename;
                }

                temlist.Add(tem);
            }

            return temlist;
        }
        #endregion

        #region 批量增加附件

        /// <summary>
        /// 批量增加附件
        /// </summary>
        /// <param name="list">附件列表</param>
        /// <param name="trachooseid">招标id</param> 
        /// <returns>入库文件数量</returns>
        public int AddFiles(List<temSysStencilAdjuncct> list, int trachooseid, ref string filenames)
        {
            int res = 0;

            List<string> filenamelist = new List<string>();

            if (list.Count != 0)
            {
                DeleteSysStencilAdjuntByID(trachooseid.ToString());

                foreach (var item in list)
                {
                    SysStencilAdjuncctModel models = new SysStencilAdjuncctModel();
                    models.TraChooseId = trachooseid;
                    models.FileName = item.filename + item.ext;
                    models.Url = item.path;

                    filenamelist.Add(models.Url);

                    int temres = AddSysStencilAdjunt(models);

                    if (temres > 0)
                    {
                        res++;
                    }
                }
            }

            filenames = String.Join(",", filenamelist.ToArray());

            return res;
        }
        #endregion

        #region 新增附件

        /// <summary>
        /// 新增附件
        /// </summary>
        /// <param name="list">附件列表</param>
        /// <param name="trachooseid">招标id</param>
        /// <param name="filenames">附件名称</param>
        /// <returns></returns>
        public int AddFilesForSupplier(List<temSysStencilAdjuncct> list, int trachooseid, ref string filenames)
        {
            int res = 0;

            List<string> filenamelist = new List<string>();

            if (list.Count != 0)
            {
                // 删除附件
                //DeleteSysStencilAdjuntByID(trachooseid.ToString());

                foreach (var item in list)
                {
                    SysStencilAdjuncctModel models = new SysStencilAdjuncctModel();
                    models.StencilId = trachooseid;
                    models.FileName = item.filename + item.ext;
                    models.Url = item.path;

                    filenamelist.Add(models.Url);

                    // 新增附件
                    int temres = EditSysStencilAdjunt(models);

                    if (temres > 0)
                    {
                        res++;
                    }
                }
            }
            else
            {
                // 删除附件
                DeleteSysStencilAdjuntByID(trachooseid.ToString());
            }
            filenames = String.Join(",", filenamelist.ToArray());
            return res;
        }
        #endregion
    }
}
