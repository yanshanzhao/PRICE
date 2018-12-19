//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-23    1.0        MH         新建               
//-------------------------------------------------------------------------
#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.BLL.Sys;
using SRM.Model.Sys;
#endregion
/*********************************
 * 类名：SysUserController
 * 功能描述：系统用户 控制器 
 * ******************************/
namespace SRM.Web.Controllers
{
    public class SysUserController : Controller
    {
        SysUserBLL bll = new SysUserBLL();
        SysDepartmentBLL depbll = new SysDepartmentBLL();
        /// <summary>
        ///用户列表页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 用户分页列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="keyword">关键词</param>
        public ActionResult UserList(int index, int size, string keyword)
        {

            string where = string.Format("  u.CompanyId={0} and u.State<2 ", Auxiliary.CompanyID());

            if (!string.IsNullOrEmpty(keyword))
            {
                where = string.Format("  u.CompanyId={0} And u.UserName+u.UserSpelling+u.TrueName+u.MobileNumber like '%{1}%'", Auxiliary.CompanyID(), keyword.Trim());
            }

            List<SysUserModel> list = bll.SysUserPageList(index, size, where);

            list.ForEach(p => p.CreatePerson = p.CreateTime.ToString("yyyy.MM.dd HH:mm:ss"));

            return Json(list);
        }

        /// <summary>
        ///用户列表数据总数
        /// </summary>
        public int UserCount(string keyword)
        {
            string where = string.Format("   u.CompanyId={0} and u.State<2 ", Auxiliary.CompanyID());

            if (!string.IsNullOrEmpty(keyword))
            {
                where = string.Format("  u.CompanyId={0} And u.UserName+u.UserSpelling+u.TrueName+u.MobileNumber like '%{1}%'", Auxiliary.CompanyID(), keyword.Trim());
            }

            return bll.SysUserPageCount(where);
        }


        /// <summary>
        ///用户新增页面
        /// </summary>
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        ///用户新增功能处理函数
        /// </summary>
        public ActionResult AddUser(SysUserModel model)
        {
            model.Password = DESEncrypt.Encrypt(model.Password);
            model.CreatePerson = Auxiliary.GetUserName();
            model.CreateTime = DateTime.Now;
            model.CompanyId = Auxiliary.CompanyID();
            model.UserSpelling = string.Empty;

            if (bll.IsHasUser(model.UserName.Trim()) != 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            if (bll.AddSysUser(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });

        }

        /// <summary>
        ///用户修改页面
        /// </summary>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(string ids)
        {
            SysUserModel model = bll.GetModelByID(ids);
            SysDepartmentModel depmodel = depbll.GetModelByID(model.DepartmentId.ToString());
            if (depmodel == null)
            {
                ViewBag.depname = "无";
            }
            else
            {
                ViewBag.depname = depmodel.DepartmentName;
            }


            return View(model);
        }
        /// <summary>
        ///用户修改处理函数
        /// </summary>
        public ActionResult ModifyUser(SysUserModel model)
        {
            model.CreatePerson = Auxiliary.GetUserName();
            model.CreateTime = DateTime.Now;
            model.CompanyId = Auxiliary.CompanyID();
            model.UserSpelling = string.Empty;

            SysUserModel editModel = bll.GetModelByID(model.UserId.ToString());

            int result = bll.UpdateSysUser(model);
            if (result > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, editModel, model);

                return Json(new { flag = "ok" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, editModel, model);
            return Json(new { flag = "fail" });


        }

        /// <summary>
        ///删除用户
        /// </summary>
        [Operate(Name = OperateEnum.Delete)]
        public ActionResult DelUser(string ids)
        {
            SysUserModel delmodel = bll.GetModelByID(ids); 
            //int i=  bll.DeleteSysUserByID(ids);
            int i = bll.ChangeState(2, ids);
            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Sucess, delmodel);
                return Json(new { flag = "ok" });
            }
            Auxiliary.Log(OperateEnum.Delete, ResultEnum.Fail, delmodel);
            return Json(new { flag = "fail" });
        }


        /// <summary>
        ///更改用户状态
        /// </summary>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult ChangeState(int state, string uid)
        {
            int i = bll.ChangeState(state, uid);
            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, new { detail = "更改状态", userid = uid, state = state });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, new { detail = "更改状态", userid = uid, state = state });
            return Json(new { flag = "fail" });
        }


        /// <summary>
        ///更改用户密码
        /// </summary>
        [Operate(Name = OperateEnum.ReSet)]
        public ActionResult ChangePwd(string uid, string pwd)
        {
            int i = bll.ChangePwd(DESEncrypt.Encrypt(pwd), uid);

            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.ReSet, ResultEnum.Sucess, new { detail = "重置密码", userid = uid, pwd = DESEncrypt.Encrypt(pwd) });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.ReSet, ResultEnum.Fail, new { detail = "重置密码", userid = uid, pwd = DESEncrypt.Encrypt(pwd) });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///更改用户密码
        /// </summary>
        public ActionResult ChangePwds(string pwd)
        {
            int i = bll.ChangePwd(DESEncrypt.Encrypt(pwd), Auxiliary.UserID().ToString());

            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.ReSet, ResultEnum.Sucess, new { detail = "重置密码", userid = Auxiliary.UserID(), pwd = DESEncrypt.Encrypt(pwd) });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.ReSet, ResultEnum.Fail, new { detail = "重置密码", userid = Auxiliary.UserID(), pwd = DESEncrypt.Encrypt(pwd) });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///头像上传处理
        /// </summary>
        [HttpPost]
        public ActionResult Upload()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count == 0) return Json(new { flag = "fail" });

            string FileEextension = System.IO.Path.GetExtension(files[0].FileName);
            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
            string virtualPath = string.Format("/upload/avata/{0}/{1}{2}", uploadDate, Guid.NewGuid().ToString("n"), FileEextension);
            string fullFileName = Server.MapPath(virtualPath);

            string path = System.IO.Path.GetDirectoryName(fullFileName);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            if (!System.IO.File.Exists(fullFileName))
            {
                files[0].SaveAs(fullFileName);
            }
            return Json(new { flag = "ok", path = virtualPath });
        }

        /// <summary>
        ///角色选择页面
        /// </summary>
        public ActionResult RoleList(string ids)
        {
            ViewBag.uid = ids;

            return View();
        }

        /// <summary>
        ///设置角色
        /// </summary>
        [Operate(Name = OperateEnum.Set)]
        public ActionResult UserRole(string uid, string ids, string names)
        {
            List<string> rolelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(ids);

            bool res = bll.InsertUserRole(uid, rolelist);

            bll.UpdateRoleInfo(uid, names ?? string.Empty);

            if (res)
            {
                Auxiliary.Log(OperateEnum.Set, ResultEnum.Sucess, new { detail = "设置角色", userid = Auxiliary.UserID(), roleids = ids });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Set, ResultEnum.Fail, new { detail = "设置角色", userid = Auxiliary.UserID(), roleids = ids });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///用户角色列表数据
        /// </summary>
        public ActionResult UserRoleList(string uid)
        {
            return Json(bll.UserRoleList(uid));
        }

        /// <summary>
        ///用户菜单列表
        /// </summary>
        public ActionResult UserModule()
        {
            string uid = Auxiliary.UserID().ToString();
            string deparType = Auxiliary.DeparType().ToString();
            return Json(bll.GetUserModule(uid, deparType));
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string keyword)
        {
            string where = string.Format("  u.CompanyId={0} and u.State<2", Auxiliary.CompanyID());

            if (!string.IsNullOrEmpty(keyword))
            {
                where = string.Format("  u.CompanyId={0} And u.UserName+u.UserSpelling+u.TrueName+u.MobileNumber like '%{1}%'", Auxiliary.CompanyID(), keyword.Trim());
            }

            System.Data.DataTable dt = bll.ExportDataTable(where);
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            return Json(new { flag = "ok", guid = url });
        }




    }
}
