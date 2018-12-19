//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-20    1.0                
//-------------------------------------------------------------------------
#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.BLL.Sys;
using SRM.Model.Sys;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：SysCompController
 * 功能描述：系统公司表 控制器  
 * ******************************/
namespace SRM.Web.Controllers
{
    public class SysCompController : Controller
    {
        SysCompanyBLL bll = new SysCompanyBLL();
        SysDepartmentBLL deBll = new SysDepartmentBLL();//组织机构Bll
        /// <summary>
        ///公司列表页
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 公司分页列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        public ActionResult List(int index, int size, string keyword)
        {

            string where = " SysDepartment.DeparType=1  and SysCompany.IsAdmin=0 ";

            if (!string.IsNullOrEmpty(keyword))
            {
                where = string.Format(" SysDepartment.DeparType=1  and SysCompany.IsAdmin=0 And SysCompany.CompanyName+SysCompany.CompanyNo+SysCompany.EmailAddress+SysCompany.MobileNumber like '%{1}%'", Auxiliary.CompanyID(), keyword.Trim());
            }
            List<SysCompanyModel> list = bll.SysCompanyPageList(index, size, where);
            return Json(list);
        }
        /// <summary>
        ///公司列表数据总数
        /// </summary>
        public int Count(string keyword)
        {
            string where = " SysDepartment.DeparType=1  and SysCompany.IsAdmin=0 ";
            if (!string.IsNullOrEmpty(keyword))
            {
                where = string.Format(" SysDepartment.DeparType=1  and SysCompany.IsAdmin=0 And SysCompany.CompanyName+SysCompany.CompanyNo+SysCompany.EmailAddress+SysCompany.MobileNumber like '%{1}%'", Auxiliary.CompanyID(), keyword.Trim());
            }
            return bll.SysCompanyPageCount(where);
        }
        /// <summary>
        ///新增页面
        /// </summary>
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        ///公司新增功能处理函数
        /// </summary>
        public ActionResult AddComp(SysCompanyModel model)
        {
            model.IsAdmin = 0;
            model.CreateTime = DateTime.Now;
            model.CompanyNo = string.Empty;
            model.EmailAddress = model.EmailAddress ?? string.Empty;
            model.MobileNumber = model.MobileNumber ?? string.Empty;
            model.PhoneNumber = model.PhoneNumber ?? string.Empty;
            model.OtherContact = model.OtherContact ?? string.Empty;
            model.Province = model.Province ?? string.Empty;
            model.City = model.City ?? string.Empty;
            model.Village = model.Village ?? string.Empty;
            model.Remark = model.Remark ?? string.Empty;
            model.Address = model.Address ?? string.Empty;
            model.Expertise = model.Expertise ?? string.Empty;
            if (bll.IsHasComp(0, model.CompanyName.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }
            int CompanyId = 0;
            CompanyId = bll.AddSysCompany(model);
            if (CompanyId > 0)//添加之后返回对应表的自增ID
            {
                deBll.AddSysCompanyToSysDepartment(CompanyId);//在组织机构表中添加一个公司的ID
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        /// <summary>
        ///公司编辑页面
        /// </summary>
        public ActionResult Edit(string ids)
        {
            SysCompanyModel model = bll.GetModelByID(ids);

            return View(model);
        }
        /// <summary>
        ///公司修改处理函数
        /// </summary>
        public ActionResult EditComp(SysCompanyModel model)
        {
            model.IsAdmin = 0;
            model.CreateTime = DateTime.Now;
            model.CompanyNo = string.Empty;
            model.EmailAddress = model.EmailAddress ?? string.Empty;
            model.MobileNumber = model.MobileNumber ?? string.Empty;
            model.PhoneNumber = model.PhoneNumber ?? string.Empty;
            model.OtherContact = model.OtherContact ?? string.Empty;
            model.Province = model.Province ?? string.Empty;
            model.City = model.City ?? string.Empty;
            model.Village = model.Village ?? string.Empty;
            model.Remark = model.Remark ?? string.Empty;
            model.Address = model.Address ?? string.Empty;
            model.Expertise = model.Expertise ?? string.Empty;
            if (bll.IsHasComp(model.CompanyId, model.CompanyName.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            SysCompanyModel editModel = bll.GetModelByID(model.CompanyId.ToString());

            if (bll.UpdateSysCompany(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, editModel, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, editModel, model);
            return Json(new { flag = "fail" });
        }
        /// <summary>
        ///删除公司
        /// </summary>
        public ActionResult DelComp(string ids)
        {
            SysCompanyModel model = bll.GetModelByID(ids);
            try
            {
                int i = bll.DeleteSysCompanyByID(ids);

                if (i > 0)
                {
                    Auxiliary.Log(OperateEnum.Delete, ResultEnum.Sucess, model);
                    return Json(new { flag = "ok" });
                }
            }
            catch
            {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Fail, model);
                return Json(new { flag = "fail" });
            }

            Auxiliary.Log(OperateEnum.Delete, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        /// <summary>
        ///公司默认信息设置
        /// </summary>
        public ActionResult Set(string ids, string isconfig, string depId)
        {
            if (!string.IsNullOrEmpty(isconfig) && isconfig == "0")
            {
                ViewBag.isconfig = isconfig;
                ViewBag.ids = ids;
                ViewBag.roleid = 0;
                ViewBag.depId = depId;

            }
            else if (!string.IsNullOrEmpty(isconfig) && isconfig == "1")
            {
                ViewBag.isconfig = isconfig;
                ViewBag.ids = ids;
                ViewBag.depId = depId;

                SysUserModel umodel = new SysUserBLL().GetDefaultInfo(ids);
                if (umodel != null)
                {
                    ViewBag.uid = umodel.UserId;
                    ViewBag.uname = umodel.UserName;
                }

                SysRoleModel rmodel = new SysRoleBLL().GetDefaultInfo(ids);
                if (rmodel != null)
                {
                    ViewBag.roleid = rmodel.RoleId;
                    ViewBag.rolename = rmodel.RoleName;
                    ViewBag.rolememo = rmodel.Remark;
                }


            }


            return View();
        }
        /// <summary>
        ///公司默认信息保存
        /// </summary>
        public ActionResult SaveInfo(string ids, string isconfig, string name, string pwd, string rolename, string rolememo, string modlist, string operlist,string depId)
        {
            if (!string.IsNullOrEmpty(isconfig) && isconfig == "0")
            {
                if (new SysUserBLL().IsHasUser(name.Trim()) != 0)
                {
                    Auxiliary.Log(OperateEnum.Set, ResultEnum.Exist, new { detail = "配置信息", res = "已存在用户账号", userid = name });
                    return Json(new { flag = "exist" });
                }

                int roleid = 0;
                int i = bll.AddDefaultInfo(name, DESEncrypt.Encrypt(pwd), rolename, rolememo, ids, depId, ref roleid);

                if (i > 0)
                {
                    SysRoleBLL rolebll = new SysRoleBLL();

                    List<string> modlists = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(modlist);

                    List<ModOperate> operlists = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModOperate>>(operlist);


                    bool res = rolebll.InsertAllAuthInfo(roleid.ToString(), modlists, operlists);

                    if (res)
                    {
                        Auxiliary.Log(OperateEnum.Set, ResultEnum.Sucess, new { detail = "配置信息", companyid = ids });
                        return Json(new { flag = "ok" });
                    }
                }
            }

            Auxiliary.Log(OperateEnum.Set, ResultEnum.Fail, new { detail = "配置信息", companyid = ids });
            return Json(new { flag = "fail" });
        }
        /// <summary>
        ///公司默认信息保存
        /// </summary>
        public ActionResult SaveInfos(string ids, string uid, string name, string roleid, string rolename, string rolememo, string modlist, string operlist, string depId)
        {
            if (new SysUserBLL().IsHasUsers(Convert.ToInt32(uid), name.Trim()) != 0)
            {
                Auxiliary.Log(OperateEnum.Set, ResultEnum.Exist, new { detail = "配置信息", res = "已存在用户账号", userid = name });
                return Json(new { flag = "exist" });
            }

            int i = bll.UpdateDefaultInfo(uid, name, roleid, rolename, rolememo);

            if (i > 0)
            {
                SysRoleBLL rolebll = new SysRoleBLL();

                List<string> modlists = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(modlist);

                List<ModOperate> operlists = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModOperate>>(operlist);


                bool res = rolebll.InsertAllAuthInfo(roleid.ToString(), modlists, operlists);

                if (res)
                {
                    Auxiliary.Log(OperateEnum.Set, ResultEnum.Sucess, new { detail = "配置信息", companyid = ids });
                    return Json(new { flag = "ok" });
                }
            }

            Auxiliary.Log(OperateEnum.Set, ResultEnum.Fail, new { detail = "配置信息", companyid = ids });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///修改状态
        /// </summary>
        public ActionResult ChangeState(int state, string uid)
        {
            int i = bll.ChangeState(state, uid);
            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, new { detail = "更改状态", companyid = uid, state = state });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, new { detail = "更改状态", companyid = uid, state = state });
            return Json(new { flag = "fail" });
        }
        /// <summary>
        ///公司弹窗选择页面
        /// </summary>
        public ActionResult Select(string url)
        {
            ViewBag.url = url;
            return View();
        }
    }
}
