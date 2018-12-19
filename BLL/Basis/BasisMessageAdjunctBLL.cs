//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/25    1.0        ZBB        新建
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

using Model.Basis;
using DAL.Basis;
#endregion
/*********************************
 * 类名：BasisMessageAdjunctBLL
 * 功能描述：基本信息附件表 业务逻辑层
 * ******************************/

namespace BLL.Basis
{
    public class BasisMessageAdjunctBLL
    {
        BasisMessageAdjunctDAL dal = new BasisMessageAdjunctDAL();

        #region 添加 基本信息附件表

        /// <summary>
        /// 添加 基本信息附件表
        /// </summary>
        public int AddBasisMessageAdjunct(BasisMessageAdjunctModel model)
        {
            return dal.AddBasisMessageAdjunct(model);
        }
        #endregion
           
        #region 删除 基本信息附件表

        /// <summary>
        /// 删除 基本信息附件表
        /// </summary>
        public int DeleteBasisMessageAdjunctByID(string id)
        {
            return dal.DeleteBasisMessageAdjunctByID(id);
        }
        #endregion  

        #region  基本信息附件表

        /// <summary>
        ///   基本信息附件表
        /// </summary>
        public List<BasisMessageAdjunctModel> BasisMessageAdjunctList(int id)
        {
            return dal.BasisMessageAdjunctList(id);
        }

        public List<temfiles> SuppFileList(int id)
        {
            List<BasisMessageAdjunctModel> list = dal.BasisMessageAdjunctList(id);
            List<temfiles> temlist = new List<temfiles>();
            foreach (var item in list)
            {
                temfiles tem = new temfiles();
                tem.path = item.FileUrl;
                tem.id = item.MessageAdjunctId.ToString();

                int dot = item.FileName.LastIndexOf(".");

                string filename = item.FileName.Substring(0, dot);
                string ext = item.FileName.Substring(dot, item.FileName.Length - dot);

                tem.ext = ext;
                tem.filename = filename;

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
        /// <param name="messageid">基本信息编号</param> 
        /// <returns>入库文件数量</returns>
        public int AddFiles(List<temfiles> list, int messageid, ref string filenames)
        {
            int res = 0;

            List<string> filenamelist = new List<string>();

            if (list.Count != 0)
            {
                DeleteBasisMessageAdjunctByID(messageid.ToString());

                foreach (var item in list)
                {
                    BasisMessageAdjunctModel models = new BasisMessageAdjunctModel();
                    models.MessageId = messageid;
                    models.FileName = item.filename + item.ext;
                    models.FileUrl = item.path;

                    filenamelist.Add(models.FileName);

                    int temres = AddBasisMessageAdjunct(models);

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
        /// <param name="messageid">基本信息编号</param>
        /// <param name="filenames">附件名称</param>
        /// <returns></returns>
        public int AddFilesForSupplier(List<temfiles> list, int messageid, ref string filenames)
        {
            int res = 0;

            List<string> filenamelist = new List<string>();

            if (list.Count != 0)
            {
                // 删除附件
                DeleteBasisMessageAdjunctByID(messageid.ToString());

                foreach (var item in list)
                {
                    BasisMessageAdjunctModel models = new BasisMessageAdjunctModel();
                    models.MessageId = messageid;
                    models.FileName = item.filename + item.ext;
                    models.FileUrl = item.path;

                    filenamelist.Add(models.FileName);

                    // 新增附件
                    int temres = AddBasisMessageAdjunct(models);

                    if (temres > 0)
                    {
                        res++;
                    }
                }
            }
            else
            {
                // 删除附件
                DeleteBasisMessageAdjunctByID(messageid.ToString());
            }

            filenames = String.Join(",", filenamelist.ToArray());
            return res;
        }
        #endregion
    }
}
