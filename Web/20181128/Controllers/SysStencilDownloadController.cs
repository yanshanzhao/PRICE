// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-11-08    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.BLL.Sys;
using SRM.Model.Sys;
using System.Linq;
using System.Text;
using System.IO;
using Aspose.Cells;
#endregion
/*********************************
 * 类名：SysStencilDownloadController
 * 功能描述：模板下载 控制器 
 * ******************************/

namespace SRM.Web.Controllers
{
    public class SysStencilDownloadController : Controller
    {
        //运作要求维护
        SysStencilBLL bll = new SysStencilBLL();

        SysStencilAdjuncctBLL ssbll = new SysStencilAdjuncctBLL();

        //
        // GET: /Sys/SysStencilDownload/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Check
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            SysStencilModel model = bll.GetModelByID(tId);

            List<temSysStencilAdjuncct> filelist = ssbll.SysStencilAdjuntFileList(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();
            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        #region 选择线路窗口

        /// <summary>
        /// 选择线路窗口
        /// </summary>
        [Operate(Name = OperateEnum.DownLoad)]
        public ActionResult SysDownloadData(string tId)
        {
            // 获取数据
            SysStencilModel model = bll.GetModelByID(Convert.ToInt32(tId));

            List<temSysStencilAdjuncct> filelist = ssbll.SysStencilAdjuntFileList(Convert.ToInt32(tId));
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();
            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }
        #endregion

        #region 方法

        #region index数据集

        /// <summary>
        /// index数据集
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="theme">运作主题</param>
        /// <param name="beginTimes">开始时间</param>
        /// <param name="endTimes">结束时间</param>
        /// <param name="state">提交状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public ActionResult SysStencilList(int index, int size, string state)
        {
            string where = string.Format(" toc.State = 1 and toc.CompanyId={0}", Auxiliary.CompanyID());
             
            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And toc.State = {0}", state.Trim());
            }
             
            List<SysStencilModel> list = bll.SysStencilList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        #endregion

        #region 数据记录数

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="theme">运作主题</param>
        /// <param name="beginTimes">开始时间</param>
        /// <param name="endTimes">结束时间</param>
        /// <param name="state">提交状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int SysStencilCount(string state)
        {
            string where = string.Format(" toc.State = 1 and toc.CompanyId={0}", Auxiliary.CompanyID());
  
            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And toc.State = {0}", state.Trim());
            }
 
            return bll.SysStencilCount(where);
        }
        #endregion

        #endregion
    }
}
