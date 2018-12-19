//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-08    1.0        zbb        新建               
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
    public class BasisMessageController : Controller
    {
        //
        // GET: /Basis/BasisMessage/

        //信息预登记BLL
        BasisMessageBLL bll = new BasisMessageBLL();

        //信息预登记Model
        BasisMessageModel model = new BasisMessageModel();

        //系统字典BLL
        BasisDictionaryBLL BDBLL = new BasisDictionaryBLL();

        // 附件BLL
        BasisMessageAdjunctBLL bmabll = new BasisMessageAdjunctBLL();

        #region 页面

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns> 
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns> 
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            model = bll.GetModelByID(tId);

            List<temfiles> filelist = bmabll.SuppFileList(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();
            ViewBag.oldfiles = String.Join(",", oldlist);
            return View(model);
        }

        /// <summary>
        /// 查看
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            model = bll.GetModelByID(tId);

            List<temfiles> filelist = bmabll.SuppFileList(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);
            return View(model);
        }
        #endregion

        #region 方法

        #region 数据集 信息预登记 

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="MessageType"></param>
        /// <param name="MessageState"></param> 
        /// <returns></returns>
        public ActionResult MessageList(int index, int size, string MessageType, string MessageState)
        {
            string where = string.Format("  mes.CompanyId={0} and mes.MessageState!=4", Auxiliary.CompanyID());

            //信息类型
            if (!string.IsNullOrEmpty(MessageType))
            {
                if (MessageType != "-1")
                {
                    where += string.Format(" And DictionaryId ={0}", MessageType.Trim());
                }
            }

            //信息状态
            if (!string.IsNullOrEmpty(MessageState))
            {
                where += string.Format(" And mes.MessageState ={0}", MessageState.Trim());
            }

            List<BasisMessageModel> list = bll.BasisMessagePageList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 信息预登记

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="MessageType"></param>
        /// <param name="MessageState"></param>
        /// <returns></returns>
        public int MessageCount(string MessageType, string MessageState)
        {
            string where = string.Format("  mes.CompanyId={0} and mes.MessageState!=4", Auxiliary.CompanyID());

            //信息类型
            if (!string.IsNullOrEmpty(MessageType))
            {
                if (MessageType != "-1")
                {
                    where += string.Format(" And DictionaryId ={0}", MessageType.Trim());
                }
            }

            //信息状态
            if (!string.IsNullOrEmpty(MessageState))
            {
                where += string.Format(" And mes.MessageState ={0}", MessageState.Trim());
            }

            return bll.BasisMessagePageCount(where);
        }
        #endregion

        #region 信息预登记新增

        /// <summary>
        /// 信息预登记新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Add)]
        public ActionResult AddMessage(BasisMessageModel model)
        {

            model.MessageState = 0;// 默认状态为初始状态0

            model.CompanyId = Auxiliary.CompanyID();//公司id

            model.AddDepartmentId = Auxiliary.DepartmentId();//新增机构id

            model.AddUserId = Auxiliary.UserID();//新增负责人id

            model.AddTime = DateTime.Now;//新增时间

            int MessageId = bll.AddBasisMessage(model);

            if (MessageId > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppFileLists))
                {
                    List<temfiles> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfiles>>(model.SuppFileLists);
                    bmabll.AddFilesForSupplier(fflist, MessageId, ref filenames);
                }
                model.SuppFileLists = filenames;

                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 信息预登记编辑

        /// <summary>
        /// 信息预登记编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns> 
        public ActionResult EditMessage(BasisMessageModel model)
        {
            model.CompanyId = Auxiliary.CompanyID();//公司id

            model.AddDepartmentId = Auxiliary.DepartmentId();//新增机构id

            model.AddUserId = Auxiliary.UserID();//新增负责人id

            model.AddTime = DateTime.Now;//新增时间

            BasisMessageModel beforeModel = bll.GetModelByID(model.MessageId);

            int result = bll.UpdateBasisMessage(model);

            if (result > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SuppFileLists))
                {
                    List<temfiles> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temfiles>>(model.SuppFileLists);
                    bmabll.AddFilesForSupplier(fflist, model.MessageId, ref filenames);
                }
                model.SuppFileLists = filenames;

                //补齐原有信息
                string[] tems = model.TemSelectData.Split(',');

                if (tems.Length == 3)
                {
                    beforeModel.DictionaryName = tems[0]; 
                } 
                 
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success", content = "编辑成功！" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 更改状态

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult EditState(string tId)
        {
            int row = bll.ChangeState(tId, 1);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 信息预登记作废

        /// <summary>
        /// 信息预登记作废
        /// </summary>
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult DelMessage(string Id)
        {
            model.DelDepartmentId = Auxiliary.DepartmentId();//作废机构id

            model.DelUserId = Auxiliary.UserID();//作废负责人id

            model.DelTime = DateTime.Now;//作废时间

            BasisMessageModel beforeModel = bll.GetModelByID(model.MessageId);

            int row = bll.ChangeState(Id, 4, model.DelDepartmentId, model.DelUserId, model.DelTime);
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 导出数据

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string MessageType, string MessageState)
        {
            string where = string.Format("  mes.CompanyId={0} and mes.MessageState!=4", Auxiliary.CompanyID());

            //信息类型
            if (!string.IsNullOrEmpty(MessageType))
            {
                if (MessageType != "-1")
                {
                    where += string.Format(" And DictionaryId ={0}", MessageType.Trim());
                }
            }

            //信息状态
            if (!string.IsNullOrEmpty(MessageState))
            {
                where += string.Format(" And mes.MessageState ={0}", MessageState.Trim());
            }

            System.Data.DataTable dt = bll.ExportDataTable(where);
            Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        #region 获取系统字典表数据

        /// <summary>
        /// 获取系统字典表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult MessageTypelist()
        {
            return Json(BDBLL.GetDictLists(Auxiliary.CompanyID()));
        }
        #endregion

        #endregion
    }
}
