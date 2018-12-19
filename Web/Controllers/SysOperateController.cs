//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-08    1.0         HDS        新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BLL.Sys;
using Model.Sys;
#endregion
/*********************************
 * 类名：SysOperateController
 * 功能描述：按钮表 控制器  
 * ******************************/
namespace Web.Controllers
{
    public class SysOperateController : Controller
    {
        SysOperateBLL bll = new SysOperateBLL();
        /// <summary>
        /// 按钮列表页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///按钮列表数据
        /// </summary>
        public ActionResult OperateList()
        {
           return Json(bll.SysOperateList());
        }
        /// <summary>
        ///按钮已选列表
        /// </summary>
        public ActionResult OperateSelectList(string modid)
        {
            return Json(new {list= bll.SysOperateList(),sellist=bll.GetModOperList(modid) });
        }
        /// <summary>
        ///按钮新增页面
        /// </summary>
        [Operate(Name=OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        ///按钮修改页面
        /// </summary>
        [Operate(Name=OperateEnum.Edit)]
        public ActionResult Edit(string ids)
        {
            SysOperateModel model = bll.GetModelByID(ids);

            return View(model);
        }
        /// <summary>
        ///按钮修改处理函数
        /// </summary>
        public ActionResult ModifyOper(SysOperateModel model)
        {
            model.Iconic = string.Empty;
            model.Remark = string.Empty;

            if (bll.IsHasOper(model.OperateId, model.OperateName.Trim(),model.Code.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            SysOperateModel editModel = bll.GetModelByID(model.OperateId.ToString());

            if (bll.UpdateSysOperate(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess,editModel , model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail,editModel, model);

            return Json(new { flag = "fail" });
        }
        /// <summary>
        ///按钮新增处理函数
        /// </summary>
        public ActionResult AddOper(SysOperateModel model)
        {  
            model.Iconic = string.Empty;
            model.Remark = string.Empty;

            if (bll.IsHasOper(0, model.OperateName.Trim(),model.Code.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            if (bll.AddSysOperate(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        /// <summary>
        /// 选择按钮列表页
        /// </summary>
        [Operate(Name=OperateEnum.Set)]
        public ActionResult Select(string ids)
        {
            ViewBag.moduleid = ids;

            return View();
        }

        /// <summary>
        ///删除按钮
        /// </summary>
        [Operate(Name=OperateEnum.Delete)]
        public ActionResult Delete(string id)
        {
            SysOperateModel delmodel = bll.GetModelByID(id);
            if (bll.GetModuleOperateCount(id) > 0)
            {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Depend, delmodel);
                return Json(new { flag = "exist" });
            }

            int i = bll.DeleteSysOperateByID(id);

            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Sucess, delmodel);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Delete, ResultEnum.Fail, delmodel);
            return Json(new { flag = "fail" });
        }
    }
}
