//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-20    1.0      
//2018-11-02    1.0         HDS         处理登录缓存问题
//-------------------------------------------------------------------------
#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BLL.Sys;
using Model.Sys;
using System.Web.Security;
using Model.Basis;
using BLL.Basis;


using System.Data;
#endregion
/*********************************
 * 类名：IndexController
 * 功能描述：登陆、用户中心 控制器  
 * ******************************/
namespace Web.Controllers
{
    public class IndexController : Controller
    {
        SysUserBLL bll = new SysUserBLL();
        SysDepartmentBLL depbll = new SysDepartmentBLL();
        SysLogBLL logbll = new SysLogBLL();

        // 信息预登记bll
        BasisMessageAuditBLL BMAbll = new BasisMessageAuditBLL();

        //// 供应商审核bll
        //SupplierAuditBLL SAbll = new SupplierAuditBLL();

        /// <summary>
        ///用户中心页
        /// </summary>
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///用户信息
        /// </summary>
        public ActionResult Base()
        {
            string ids = Auxiliary.UserID().ToString();
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
        ///登陆页
        /// </summary>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        ///登陆操作
        /// </summary>
        public ActionResult Logins(string user, string pwd)
        {
            

            SysUserModel model = null;

            SysLogModel logModel = new SysLogModel();
            logModel.Module = "login";
            logModel.CreateTime = DateTime.Now;
            logModel.Operator = user;
            logModel.Message = string.Empty;
            try
            {
                logModel.Type = Request.UserHostAddress.ToString();
            }
            catch
            {
                logModel.Type = "ip出错";
            }
            try
            {
                model = bll.UserLoginInfo(user, DESEncrypt.Encrypt(pwd));
            }
            catch
            {
                logModel.Result = "数据库连接错误";
                logModel.CompanyId = 0;
                logbll.AddSysLog(logModel);

                return Json(new
                {
                    flag = "fail",
                    content = "后台数据库不能连接,请及时联系管理员"
                });
            }

            if (model == null)
            {
                logModel.Result = "用户名或密码错误";
                logModel.CompanyId = 0;
                logbll.AddSysLog(logModel);
                return Json(new
                {
                    flag = "fail",
                    content = "用户名或密码错误"
                });
            }
            else
            {

                if (model.State > 10)//  账号禁用分为 40- 系统停用;30-部门无效;20-账号禁用
                {
                    string result = "";
                    // logModel.Result = "账号禁用";  登录无效分为多种处理 
                    if (model.State == 40)
                        result = "系统停用";
                    else if (model.State == 30)
                        result = "部门无效";
                    else if (model.State == 20)
                        result = "账号禁用";
                    result = result + ",请联系管理员!";
                    logModel.CompanyId = model.CompanyId; 
                    logModel.Result = result;
                    logbll.AddSysLog(logModel);                    
                    return Json(new
                    {
                        flag = "disable",
                        content = result
                    });
                }
                else
                {
                    List<string> rolelist = bll.UserRoleList(model.UserId.ToString());
                    if (rolelist.Count == 0)
                    {
                        logModel.Result = "本账户无权限！请联系管理员！";
                        logModel.CompanyId = 0;
                        logbll.AddSysLog(logModel);
                        return Json(new
                        {
                            flag = "fail",
                            content = "本账户无权限！请联系管理员！"
                        });
                    }
                    else
                    {
                        string roleids = string.Join(",", rolelist.ToArray());
                        Auxiliary.SetFormsAuthentication(user, string.Format("{0}-{1}-{2}-{3}-{4}", model.CompanyId, model.UserId, roleids, model.DepartmentId, model.DeparType));
                        logModel.Result = "登陆成功";
                        logModel.CompanyId = model.CompanyId;
                        logbll.AddSysLog(logModel);
                        /*
                         登录成功之后，将所有权限等信息添加到缓存中 添加时间：20181102 添加人：HDS
                         */
                        //DataTable dt= CacheHelper.GetCache("OperateDate") as DataTable;//权限的缓存处理


                        //if (dt==null)//无权限缓存
                        //{
                        //    SysRoleOperateBLL operateBll = new SysRoleOperateBLL(); 
                        //    dt = operateBll.GetTable(" ");
                        //    CacheHelper.SetCache("OperateDate", dt, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.Zero);
                        //    dt.Clear();
                        //}
                        return Json(new
                        {
                            flag = "ok",
                            content = "登陆成功"
                        });
                    }
                }
            }
        }
        /// <summary>
        ///注销
        /// </summary>
        public ActionResult LoginOut()
        {
            Auxiliary.LoginOut();

            return Content("ok");
        }
        /// <summary>
        ///用户相关信息
        /// </summary>
        public ActionResult UserInfo()
        {
            return Json(bll.GetDetailModel(Auxiliary.UserID().ToString()));
        }

        /// <summary>
        /// 公共文件上传
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(string type)
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count == 0) return Json(new { flag = "fail" });

            string filename = System.IO.Path.GetFileNameWithoutExtension(files[0].FileName);
            string ext = System.IO.Path.GetExtension(files[0].FileName);

            string folder = "all";  
            if (type.Length>0)
            {
                folder = type;
            }   
            string uploadDate = DateTime.Now.ToString("yyyyMMdd");

            string virtrulPath = string.Format("/upload/{0}/{1}/{2}{3}", folder, uploadDate, Guid.NewGuid().ToString("n"), ext);


            string fullFileName = Server.MapPath(virtrulPath);

            string path = System.IO.Path.GetDirectoryName(fullFileName);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            if (!System.IO.File.Exists(fullFileName))
            {
                files[0].SaveAs(fullFileName);
            }
            return Json(new { flag = "ok", path = virtrulPath, filename = filename, ext = ext });
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="url">文件url</param>
        public ActionResult Down(string url, string name, string ext)
        {
            string filenames = string.Format("{0}.{1}", name, ext);

            if (!string.IsNullOrEmpty(url))
            {
                string path = HttpContext.Server.MapPath(url);

                if (Request.Browser.Browser.ToString().Contains("IE"))
                {
                    filenames = Server.UrlEncode(filenames);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + filenames + "");
                }
                return File(path, "application/octet-stream", filenames);
            }
            return View();
        }

        #region 首页

        /// <summary>
        /// 数据集 公司消息通知
        /// </summary> 
        /// <returns></returns>
        public ActionResult GetMessageList()
        {
            // 查询本公司内所有的公司消息通知信息
            string where = " BM.CompanyId =" + Auxiliary.CompanyID();

            // 公司消息通知List
            List<BasisMessageAuditModel> list = BMAbll.GetMessageList(where);

            return Json(list);
        }

        /// <summary>
        /// 数据集 待审核消息通知
        /// </summary> 
        /// <returns></returns>
        //public int GetAuditCount()
        //{
        //    // 查询本公司内所有的运输供应商服务等级信息
        //    string where = " AND AuditUserId =" + Auxiliary.UserID();

        //    return SAbll.GetAuditCount(where);
        //}
        #endregion
    }
}
