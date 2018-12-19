// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-11-01    1.0        ZBB       新建                    
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Web.Controllers;

using System.Web.Mvc;
using System;
using BLL.Sys;
using Model.Sys;
using System.Linq;
using System.Text;
using System.IO;
using Aspose.Cells;
#endregion
/*********************************
 * 类名：SysStencilController
 * 功能描述：模板维护 控制器 
 * ******************************/

namespace Web.Controllers
{
    public class SysStencilController : Controller
    {
        //
        // GET: /Sys/SysStencil/

        //运作要求维护
        SysStencilBLL bll = new SysStencilBLL();

        SysStencilAdjuncctBLL ssbll = new SysStencilAdjuncctBLL();

        #region 页面

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
            SysStencilModel model = bll.GetModelByID(tId);

            List<temSysStencilAdjuncct> filelist = ssbll.SysStencilAdjuntFileList(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();
            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
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

        #endregion

        #region 方法 

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddSysStencil(SysStencilModel model)
        {

            model.State = 0;// 状态 初始 

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID 

            int SysStencilId = bll.AddSysStencil(model);

            if (SysStencilId > 0)
            {
                //string filenames = string.Empty;
                //if (!string.IsNullOrEmpty(model.SysStencil))
                //{
                //    List<temSysStencilAdjuncct> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temSysStencilAdjuncct>>(model.SysStencil);
                //    ssbll.AddFilesForSupplier(fflist, SysStencilId, ref filenames);
                //}
                //model.SysStencil = filenames;

                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        #endregion

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
        public ActionResult SysStencilList(int index, int size,string state)
        {
            string where = string.Format(" toc.State != 20 and toc.CompanyId={0}", Auxiliary.CompanyID());

           
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
            string where = string.Format(" toc.State != 20 and toc.CompanyId={0}", Auxiliary.CompanyID());
             
            // 提交状态
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And toc.State = {0}", state.Trim());
            } 
            return bll.SysStencilCount(where);
        }
        #endregion

        #region 编辑运作维护数据
        /// <summary>
        /// 编辑运作维护数据
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditSysStencil(SysStencilModel model)
        {

            model.CreateDepartmentId = Auxiliary.DepartmentId(); // 机构ID

            model.CreateUserId = Auxiliary.UserID();// 用户ID 

            model.CompanyId = Auxiliary.CompanyID(); // 公司ID

            SysStencilModel beforeModel = bll.GetModelByID(model.StencilId);

            int result = bll.EditSysStencil(model);

            if (result > 0)
            {
                string filenames = string.Empty;
                if (!string.IsNullOrEmpty(model.SysStencil))
                {
                    List<temSysStencilAdjuncct> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temSysStencilAdjuncct>>(model.SysStencil);
                    ssbll.AddFilesForSupplier(fflist, model.StencilId, ref filenames);
                }
                model.SysStencil = filenames;
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 提交按钮逻辑

        /// <summary>
        /// 提交按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitSysStencil(int tId)
        {
            int row = bll.SubmitSysStencil(tId);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail", content = "提交失败！" });
        }
        #endregion

        #region 作废按钮逻辑

        /// <summary>
        /// 作废按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult InvalidState(int tId)
        {
            SysStencilModel beforeModel = bll.GetModelByID(tId);

            int row = 0;//行数

            int delUserId = Auxiliary.UserID();//作废负责人

            int state = beforeModel.State;//提交状态：0-未提交；1-已提交;10-作废;20-删除

            if (state == 0)
            {
                row = bll.DeleteState(tId, delUserId);//作废之后 状态变为删除状态
            }
            if (state == 1)
            {
                row = bll.InvalidState(tId, delUserId);//作废之后 状态变为作废状态
            }
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail", content = "作废失败！" });
        }
        #endregion

        #endregion
    }
}
