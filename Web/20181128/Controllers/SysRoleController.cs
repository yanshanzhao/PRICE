//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-12    1.0        MH         新建               
//-------------------------------------------------------------------------
#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.BLL.Sys;
using SRM.Model.Sys;
using System.Web.Security;
#endregion
/*********************************
 * 类名：SysRoleController
 * 功能描述：系统角色 控制器
 * ******************************/
namespace SRM.Web.Controllers
{
    public class SysRoleController : Controller
    {
        SysRoleBLL bll = new SysRoleBLL();

        /// <summary>
        ///角色列表页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 角色列表数据
        /// </summary>
        public ActionResult RoleList(string keyword)
        {
            keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
            int cid = Auxiliary.CompanyID();
            return Json(bll.SysRoleList(cid.ToString(), keyword));
        }

        /// <summary>
        ///角色新增页面
        /// </summary>
        [Operate(Name=OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        ///角色授权页面
        /// </summary>
        [Operate(Name = OperateEnum.Auth)]
        public ActionResult Auth(string id,string name)
        {
            ViewBag.roleid = id;
            ViewBag.rolename = name;

            return View();
        }
        /// <summary>
        ///角色新增处理函数
        /// </summary>
        public ActionResult AddRole(SysRoleModel model)
        {
            model.Remark = model.Remark ?? string.Empty;
            model.CompanyId = Auxiliary.CompanyID();
            model.CreatePerson = Auxiliary.GetUserName();
            model.CreateTime = DateTime.Now;

            if (bll.IsHasRole(0, model.RoleName.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            if (bll.AddSysRole(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///角色修改页面
        /// </summary>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(string ids)
        {
            SysRoleModel model = bll.GetModelByID(ids);

            ViewBag.rolename = model.RoleName;
            ViewBag.state = model.State;
            ViewBag.remark = model.Remark;
            ViewBag.id = model.RoleId;
            return View();
        }

        /// <summary>
        ///角色修改处理函数
        /// </summary>
        public ActionResult EditRole(SysRoleModel model)
        {       
            model.Remark = model.Remark ?? string.Empty;
            model.CompanyId = Auxiliary.CompanyID();
            model.CreatePerson = Auxiliary.GetUserName();
            model.CreateTime = DateTime.Now;

            if (bll.IsHasRole(model.RoleId, model.RoleName.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            SysRoleModel editModel=bll.GetModelByID(model.RoleId.ToString());

            if (bll.UpdateSysRole(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, editModel, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, editModel, model);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///更改角色状态
        /// </summary>
        [Operate(Name=OperateEnum.Edit)]
        public ActionResult ChangeState(int state, string rid)
        {
            int i=bll.ChangeState(state, rid);

            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, new { detail="更改状态",roleid=rid,state=state });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, new { detail = "更改状态", roleid = rid, state = state });
            return Json(new { flag = "fail" });  
        }

        /// <summary>
        ///删除角色
        /// </summary>
        [Operate(Name=OperateEnum.Delete)]
        public ActionResult Delete(string ids)
        {
           SysRoleModel delmodel = bll.GetModelByID(ids);
           int i= bll.GetUserRoleCountById(ids);

          if (i > 0)
          {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Depend, delmodel);
                return Json(new { flag = "exist" }); 
          }

          int res = bll.DeleteRole(ids);

          if (res == 1)
          {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Sucess, delmodel);
                return Json(new { flag = "ok" });
          }

            Auxiliary.Log(OperateEnum.Delete, ResultEnum.Fail, delmodel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///角色授权保存
        /// </summary>
        public ActionResult SaveAuth(string roleid,string modlist,string  operlist)
        {
            List<string> modlists = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(modlist);

            List<ModOperate> operlists = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModOperate>>(operlist);


            bool res=bll.InsertAllAuthInfo(roleid, modlists, operlists);

            if (res)
            {
                Auxiliary.Log(OperateEnum.Auth, ResultEnum.Sucess, new { roleid = roleid, modlist = modlist, operlist = operlist });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Auth, ResultEnum.Fail, new { roleid = roleid, modlist = modlist, operlist = operlist });
            return Json(new { flag = "fail" });
        }
        /// <summary>
        ///授权角色列表页面
        /// </summary>
        public ActionResult AuthRoleList()
        {
            string cid = Auxiliary.CompanyID().ToString();

           return  Json(bll.AuthRoleList(cid));
        }
        /// <summary>
        ///授权角色列表数据
        /// </summary>
        public ActionResult AuthAllList(string rid)
        {
        
            return Json(
                new
                {
                    modlist=bll.AuthRoleModuleList(rid),
                    operlist=bll.AuthRoleModuleOperList(rid)
                }
                );
        }
    }
}
