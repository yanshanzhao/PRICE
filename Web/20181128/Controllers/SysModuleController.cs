//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-09    1.0        MH         新建               
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.BLL.Sys;
using SRM.Model.Sys;
#endregion
/*********************************
 * 类名：SysModuleController
 * 功能描述：系统功能(菜单) 控制器 
 * ******************************/
namespace SRM.Web.Controllers
{
    public class SysModuleController : Controller
    {

        SysModuleBLL bll = new SysModuleBLL();
        /// <summary>
        ///菜单列表页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///菜单新增页面
        /// </summary>
        [Operate(Name=OperateEnum.Add)]
        public ActionResult Add(string id, string name)
        {
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(name))
            {
                ViewBag.pid = 0;
                ViewBag.pname = "根目录";
            }
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
            {
                ViewBag.pid = id;
                ViewBag.pname = Server.HtmlDecode(name);
            }
            return View();
        }
        /// <summary>
        ///菜单列表数据
        /// </summary>
        public ActionResult List(string keyword)
        {
            keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
            return Json(bll.SysModulePageList(keyword));
        }
        /// <summary>
        ///菜单授权页面
        /// </summary>
        public ActionResult AuthList()
        {
            bool res = Auxiliary.IsAdmin();

            if (res)
            {
                return Json(bll.SysModulePageList(string.Empty));
            }
            else
            {
                return Json(bll.SysModulePageList());
            }
        }
        /// <summary>
        ///菜单树列表
        /// </summary>
        public ActionResult TreeList()
        {
            List<TreeModel> list = bll.DepTreeList();

           list.Insert(0, new TreeModel { id = "-1", pid = "0", name = "根目录" });
 
           return Json(list);
        }
        /// <summary>
        ///菜单树列表
        /// </summary>
        public ActionResult Tree(string url)
        {
            ViewBag.url = url;

            return View();
        }
        /// <summary>
        ///菜单新增功能处理函数
        /// </summary>
        public ActionResult AddModule(SysModuleModel model)
        {
            model.EnglishName = string.Empty;
            model.Url = model.Url ?? string.Empty;
            model.Iconic = model.Iconic ?? string.Empty;
            model.Remark = model.Remark ?? string.Empty;
            model.CreatePerson = HttpContext.User.Identity.Name;
            model.CreateTime = DateTime.Now;

            if (bll.IsHasModule(0, model.ParentId, model.ModuleName.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            if (bll.AddSysModule(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///菜单删除处理函数
        /// </summary>
        [Operate(Name=OperateEnum.Delete)]
        public ActionResult DelModule(string ids)
        {
            SysModuleModel delmodel = bll.GetModelByID(ids);
            if (bll.SysRoleMuduleCount(ids) > 0)
            {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Depend, delmodel);
                return Json(new { flag = "exsit" });
            }
            int i= bll.DeleteSysModuleByID(ids);
            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Sucess, delmodel);
                return Json(new { flag = "ok" });
            }
            Auxiliary.Log(OperateEnum.Delete, ResultEnum.Fail, delmodel);
            return Json(new { flag = "fail" });
        }
        /// <summary>
        ///菜单修改显示页面
        /// </summary>
        [Operate(Name=OperateEnum.Edit)]
        public ActionResult Edit(string ids)
        {
            SysModuleModel model = bll.GetModelByID(ids);

            if (model.ParentId == 0)
            {
                model.ModulePName = "根目录";
            }
            else
            {
                model.ModulePName = bll.GetModelByID(model.ParentId.ToString()).ModuleName;
            }

            return View(model);
        }
        /// <summary>
        ///菜单修改处理函数
        /// </summary>
        public ActionResult ModifyModule(SysModuleModel model)
        {

            model.EnglishName = string.Empty;
            model.Url = model.Url ?? string.Empty;
            model.Iconic = model.Iconic ?? string.Empty;
            model.Remark = model.Remark ?? string.Empty;
            model.CreatePerson = HttpContext.User.Identity.Name;
            model.CreateTime = DateTime.Now;

            int result = bll.UpdateSysModule(model);

            SysModuleModel editModel = bll.GetModelByID(model.ModuleId.ToString());

            if (result > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess,editModel, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, editModel, model);

            return Json(new { flag = "fail" });
        }
        /// <summary>
        /// 更改菜单状态
        /// </summary>

        [Operate(Name = OperateEnum.Edit)]
        public ActionResult ChangeState(int state, string did)
        {
          int i= bll.ChangeState(state, did);

          if (i > 0)
          {
              Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, new { detail = "更改状态", moduleid = did, state = state });
              return Json(new { flag = "ok" });
          }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, new { detail = "更改状态", moduleid = did, state = state });
            return Json(new { flag = "fail" });
        }
        /// <summary>
        /// 更改菜单状态
        /// </summary>

        [Operate(Name = OperateEnum.Edit)]
        public ActionResult ChangeType(int type, string did)
        {
            int i = bll.ChangeType(type, did);

            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, new { detail = "更改状态", moduleid = did, Type = type });
                return Json(new { flag = "ok" });
            } 
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, new { detail = "更改状态", moduleid = did, Type = type });
            return Json(new { flag = "fail" });
        }
        /// <summary>
        /// 菜单按钮处理
        /// </summary>
        /// <param name="ids">按钮编号</param>
        /// <param name="modid">菜单编号</param>
        public ActionResult ModuleOperate(string ids,string modid)
        {
            if (bll.AddModuleOperate(modid, ids))
            {
                Auxiliary.Log(OperateEnum.Set, ResultEnum.Sucess, new { detail = "设置按钮", moduleid = modid, operids = ids });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Set, ResultEnum.Fail, new { detail = "设置按钮", moduleid = modid, operids = ids });
            return Json(new { flag = "fail" });
        }
        /// <summary>
        ///菜单已设按钮信息
        /// </summary>
        public ActionResult SelModOperList()
        {
            return Json(bll.GetSelModuleOperList());
        }
        /// <summary>
        ///菜单已设按钮信息
        /// </summary>
        public ActionResult SelModOperLists()
        {
            return Json(bll.GetSelModuleOperLists());
        }
    }
}
