//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-13    1.0        zbb        新建               
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Model.Basis;
using Web.Controllers;
using BLL.Basis;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：BasisMessageController
 * 功能描述：信息预登记控制器 
 * ******************************/

namespace Web.Areas.Basis.Controllers
{
    public class BasisMessageAuditController : Controller
    {
        //
        // GET: /Basis/BasisMessageAudit/

        //信息预登记审核BLL
        BasisMessageAuditBLL bll = new BasisMessageAuditBLL();

        //信息预登记审核Model
        BasisMessageAuditModel model = new BasisMessageAuditModel();

        // 附件BLL
        BasisMessageAdjunctBLL sabll = new BasisMessageAdjunctBLL();

        public ActionResult Index()
        {
            return View();
        }
        #region 页面
        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns> 
        [Operate(Name = OperateEnum.Check)]
        public ActionResult Audit(string tId)
        {
            // 获取数据
            model = bll.GetModelByID(tId);

            List<temfiles> filelist = sabll.SuppFileList(model.MessageId);

            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 查看
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(string tId)
        {
            // 获取数据
            model = bll.GetModelByID(tId);

            List<temfiles> filelist = sabll.SuppFileList(model.MessageId);

            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }
        #endregion


        #region 方法

        #region 数据集 信息预登记审核

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="MessageType"></param>
        /// <param name="MessageState"></param> 
        /// <returns></returns>
        public ActionResult MessageAuditList(int index, int size, string MessageType, string MessageState)
        {
            string where = string.Format("  mes.CompanyId={0} and (mes.MessageState=1 OR mes.MessageState=2 OR mes.MessageState=3)", Auxiliary.CompanyID());

            //信息类型
            if (!string.IsNullOrEmpty(MessageType))
            {
                if (MessageType != "-1")
                {
                    where += string.Format(" And mes.MessageType ={0}", MessageType.Trim());
                }
            }

            //信息状态
            if (!string.IsNullOrEmpty(MessageState))
            {
                where += string.Format(" And mes.MessageState ={0}", MessageState.Trim());
            }

            List<BasisMessageAuditModel> list = bll.BasisMessageAuditPageList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 信息预登记审核

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="MessageType"></param>
        /// <param name="MessageState"></param>
        /// <returns></returns>
        public int MessageAuditCount(string MessageType, string MessageState)
        {
            string where = string.Format("   mes.CompanyId={0} and (mes.MessageState=1 OR mes.MessageState=2 OR mes.MessageState=3)", Auxiliary.CompanyID());

            //信息预登记类型
            if (!string.IsNullOrEmpty(MessageType))
            {
                if (MessageType != "-1")
                {
                    where += string.Format(" And mes.MessageType ={0}", MessageType.Trim());
                }
            }

            //信息预登记状态
            if (!string.IsNullOrEmpty(MessageState))
            {
                where += string.Format(" And mes.MessageState={0}", MessageState.Trim());
            }
            return bll.BasisMessageAuditPageCount(where);
        }
        #endregion

        #region 信息预登记编辑

        /// <summary>
        /// 信息预登记编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns> 
        public ActionResult EditMessageAudit(BasisMessageAuditModel model)
        {
            model.CompanyId = Auxiliary.CompanyID();//公司id

            model.ToDepartmentId = Auxiliary.DepartmentId();//审核机构id

            model.ToUserId = Auxiliary.UserID();//审核负责人

            model.ToTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");//审核时间

            BasisMessageAuditModel beforeModel = bll.GetModelByID(model.MessageId.ToString());

            int result = bll.UpdateBasisMessage(model);

            if (result > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success", content = "编辑成功！" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 导出数据

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="MessageType"></param>
        /// <param name="MessageState"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string MessageType, string MessageState)
        {
            string where = string.Format("   mes.CompanyId={0} and (mes.MessageState=1 OR mes.MessageState=2 OR mes.MessageState=3)", Auxiliary.CompanyID());
             
            //信息预登记类型
            if (!string.IsNullOrEmpty(MessageType))
            {
                if (MessageType != "-1")
                {
                    where += string.Format(" And mes.MessageType ={0}", MessageType.Trim());
                }
            }

            //信息预登记状态
            if (!string.IsNullOrEmpty(MessageState))
            {
                where += string.Format(" And mes.MessageState={0}", MessageState.Trim());
            }
            System.Data.DataTable dt = bll.ExportDataTable(where);
            Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            return Json(new { flag = "success", guid = url });
        }

        #endregion

        #endregion
    }
}
