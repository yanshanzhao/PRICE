//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-05    1.0         MH         新建
//2018-06-01    1.0         MH         新增流水号处理
//2018-06-09    1.0         MH         供应商日志
//2018-06-27    1.1         FJK         新增是否有匹配审核流程,是否有下一级审核流程
//2018-11-02    1.0         HDS         处理登录缓存问题    
//-------------------------------------------------------------------------
#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Collections;

using SRM.BLL.Sys;
using SRM.Model.Sys;
using System.Web.Security;
using System.ComponentModel;
using SRM.BLL.Supplier;
using SRM.Model.Supplier;
using SRM.BLL.Basis;
using SRM.Model.Basis;
using System.IO;
using Aspose.Cells;
using System.Data;
using SRM.Web.Controllers;
using System.Web.Caching;
using System.Configuration;
#endregion
namespace SRM.Web.Controllers
{/*********************************
 * 类名：Auxiliary
 * 功能描述：常用功能辅助类
 * ******************************/
    public class Auxiliary
    {
        /// <summary>
        /// 登陆处理
        /// </summary>
        /// <param name="username">登陆名称</param>
        /// <param name="userData">用户相关数据</param>
        public static void SetFormsAuthentication(string username, string userData)
        {
            userData = string.IsNullOrEmpty(userData) ? string.Empty : userData;

            //创建票证
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                username,
                DateTime.Now,
                DateTime.Now.AddMinutes(720),
                false,
                userData,
                FormsAuthentication.FormsCookiePath);

            // 加密票证
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // 创建cookie
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
            {
                HttpOnly = true,
                Path = FormsAuthentication.FormsCookiePath,
                Secure = false
            };
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 返回当前公司主键编号
        /// </summary>
        /// <returns>返回当前公司主键编号</returns>
        public static int CompanyID()
        {
            int i = 0;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string data = ((System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;

                if (!data.Contains("-"))
                {
                    return i;
                }
                string[] arrs = data.Split('-');

                if (arrs.Length == 5)
                {
                    int.TryParse(arrs[0], out i);
                }
            }
            return i;
        }
        /// <summary>
        /// 返回当前用户主键编号
        /// </summary>
        /// <returns>返回当前用户主键编号</returns>
        public static int UserID()
        {
            int i = 0;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string data = ((System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;

                if (!data.Contains("-"))
                {
                    return i;
                }
                string[] arrs = data.Split('-');

                if (arrs.Length == 5)
                {
                    int.TryParse(arrs[1], out i);
                }
            }
            return i;
        }
        /// <summary>
        /// 返回当前角色列表
        /// </summary>
        /// <returns></returns>
        public static string RoleIDS()
        {
            string res = string.Empty;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string data = ((System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;

                if (!data.Contains("-"))
                {
                    return res;
                }
                string[] arrs = data.Split('-');

                if (arrs.Length == 5)
                {
                    return arrs[2];
                }
            }
            return res;
        }
        /// <summary>
        /// 当前用户部门编号
        /// </summary>
        /// <returns></returns>
        public static int DepartmentId()
        {
            int i = 0;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string data = ((System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;

                if (!data.Contains("-"))
                {
                    return i;
                }
                string[] arrs = data.Split('-');

                if (arrs.Length == 5)
                {
                    int.TryParse(arrs[3], out i);
                }
            }
            return i;
        }
        /// <summary>
        /// 当前用户部门类型
        /// </summary>
        /// <returns></returns>
        public static int DeparType()
        {
            int i = 0;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string data = ((System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;

                if (!data.Contains("-"))
                {
                    return i;
                }
                string[] arrs = data.Split('-');

                if (arrs.Length == 5)
                {
                    int.TryParse(arrs[4], out i);
                }
            }
            return i;
        }
        /// <summary>
        /// 返回当前登陆名称
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            return HttpContext.Current.User.Identity.Name.Trim();
        }
        /// <summary>
        /// 退出登陆
        /// </summary>
        public static void LoginOut()
        {
            FormsAuthentication.SignOut();
        }
        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public static bool IsAdmin()
        {
            return new SysUserBLL().IsAdmin(CompanyID().ToString()) == 1 ? true : false;
        }

        /// <summary>
        /// 返回当前公司编码前缀流水号
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns>流水号</returns>
        public static string CurCompanyAutoNum(string prefix)
        {
            return new SysCompanyBLL().GetAutoNum(CompanyID().ToString(), prefix);
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="operate">操作按钮（枚举类型）</param>
        /// <param name="result">操作结果（</param>
        /// <param name="obj">实体类</param>
        public static void Log(OperateEnum operate, ResultEnum result, object obj)
        {
            SysLogModel model = new SysLogModel();
            model.CompanyId = CompanyID();
            model.CreateTime = DateTime.Now;
            model.Module = HttpContext.Current.Request.Cookies["modid"].Value ?? string.Empty;
            model.Operator = GetUserName();
            model.Result = result.GetDescription();
            model.Type = operate.GetDescription();
            model.Message = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            new SysLogBLL().AddSysLog(model);
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="operate">操作按钮（枚举类型）</param>
        /// <param name="result">操作结果（</param>
        /// <param name="obj1">实体类1</param>
        /// <param name="obj2">实体类2</param>
        public static void Log(OperateEnum operate, ResultEnum result, object obj1, object obj2)
        {
            SysLogModel model = new SysLogModel();
            model.CompanyId = CompanyID();
            model.CreateTime = DateTime.Now;
            model.Module = HttpContext.Current.Request.Cookies["modid"].Value ?? string.Empty;
            model.Operator = GetUserName();
            model.Result = result.GetDescription();
            model.Type = operate.GetDescription();
            model.Message = Newtonsoft.Json.JsonConvert.SerializeObject(new { data1 = "实体1", list1 = obj1, data2 = "实体2", list2 = obj2 });
            new SysLogBLL().AddSysLog(model);
        }


        /// <summary>
        /// 供应商模块自定义实体日志记录
        /// </summary>
        /// <param name="operate">操作按钮（枚举类型）</param>
        /// <param name="result">操作结果（</param>
        /// <param name="obj">实体类</param>
        public static void SupplierCustomLog(OperateEnum operate, ResultEnum result, object obj)
        {
            SupplierLogModel model = new SupplierLogModel();
            model.CompanyId = CompanyID();
            model.CreateTime = DateTime.Now;
            model.Module = HttpContext.Current.Request.Cookies["modid"].Value ?? string.Empty;
            model.UserId = Auxiliary.UserID();
            model.Result = result.GetDescription();
            model.Type = operate.GetDescription();
            model.Message = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            new SupplierLogBLL().AddSupplierLog(model);
        }
        /// <summary>
        /// 供应商模块日志记录(实体类中需有描述属性信息）
        /// </summary>
        /// <param name="operate">操作按钮（枚举类型）</param>
        /// <param name="result">操作结果（</param>
        /// <param name="obj">实体类</param>
        public static void SupplierLog<T>(OperateEnum operate, ResultEnum result, T obj)
        {
            SupplierLogModel model = new SupplierLogModel();
            model.CompanyId = CompanyID();
            model.CreateTime = DateTime.Now;
            model.Module = HttpContext.Current.Request.Cookies["modid"].Value ?? string.Empty;
            model.UserId = Auxiliary.UserID();
            model.Result = result.GetDescription();
            model.Type = operate.GetDescription();
            model.Message = new LogHelper().getProperties<T>(obj);
            new SupplierLogBLL().AddSupplierLog(model);
        }

        /// <summary>
        ///供应商模块编辑功能日志记录(实体类中需有描述属性信息）
        /// </summary>
        /// <param name="operate">操作按钮（枚举类型）</param>
        /// <param name="result">操作结果（</param>
        /// <param name="obj1">实体类1</param>
        /// <param name="obj2">实体类2</param>
        public static void SupplierLog<T>(OperateEnum operate, ResultEnum result, T obj1, T obj2)
        {
            SupplierLogModel model = new SupplierLogModel();
            model.CompanyId = CompanyID();
            model.CreateTime = DateTime.Now;
            model.Module = HttpContext.Current.Request.Cookies["modid"].Value ?? string.Empty;
            model.UserId = Auxiliary.UserID();
            model.Result = result.GetDescription();
            model.Type = operate.GetDescription();
            model.Message = new LogHelper().getProperties<T>(obj1, obj2);

            new SupplierLogBLL().AddSupplierLog(model);
        }

        #region 审核流程

        /// <summary>
        /// 是否有匹配审核流程
        /// </summary>
        /// <param name="auditRelationType">供应商审核关系类型</param>
        /// <param name="state">状态</param>
        /// <param name="beginNode">流程开始节点</param>
        /// <param name="userId">提起人员</param>
        /// <param name="departmentId">提起机构</param>
        /// <param name="companyId">系统公司</param>
        /// <returns></returns>
        public static BasisAuditRelationModel IsMatch(int auditRelationType, int state, int beginNode, int userId, int departmentId, int companyId)
        {
            BasisAuditRelationBLL bll = new BasisAuditRelationBLL();

            // 匹配审核流程Model
            BasisAuditRelationModel model = bll.IsMatchForAudit(auditRelationType, state, beginNode, userId, departmentId, companyId);
            return model;
        }

        /// <summary>
        /// 是否有下一级审核流程
        /// </summary>
        /// <param name="beforeId">供应商审核关系类型</param>
        /// <returns></returns>
        public static BasisAuditRelationModel IsRelationByBeforeId(int beforeId)
        {
            BasisAuditRelationBLL bll = new BasisAuditRelationBLL();

            // 下一级审核流程Model
            BasisAuditRelationModel model = bll.IsRelationByBeforeId(beforeId);
            return model;
        }

        /// <summary>
        /// 根据ID获取审核流程Model
        /// </summary>
        /// <param name="beforeId">供应商审核关系类型</param>
        /// <returns></returns>
        public static BasisAuditRelationModel GetAuditRelationById(int beforeId)
        {
            BasisAuditRelationBLL bll = new BasisAuditRelationBLL();

            // 审核流程Model
            BasisAuditRelationModel model = bll.GetModelByID(beforeId);
            return model;
        }

        #endregion

        #region 模板导出

        #region 文件变量

        /// <summary>
        /// Aspose - Workbook
        /// </summary>
        private static Workbook CurrentWorkbook;

        /// <summary>
        /// Worksheet
        /// </summary>
        private static Worksheet DetailSheet;
        #endregion

        /// <summary>
        /// 模板导出
        /// </summary>
        /// <param name="dtData">数据DataTable</param>
        /// <param name="templatePath">模板路径</param>
        /// <param name="startRegion">开始位置</param>
        /// <param name="guid">guid</param>
        public static void TemplateExport(System.Data.DataTable dtData, string templatePath, string startRegion, string type, ref string guid)
        {

            // guid
            guid = Guid.NewGuid().ToString("n");

            // 存放路径
            string uploadPath = System.Web.HttpContext.Current.Server.MapPath("/upload/export/") + DateTime.Now.ToString("yyyyMM");

            // 是否存在 否新增
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // 打开 Excel 模板
            CurrentWorkbook = System.IO.File.Exists(templatePath) ? new Workbook(templatePath) : new Workbook();

            // 打开第一个sheet
            DetailSheet = CurrentWorkbook.Worksheets[0];

            // 获得开始位置的行号                    
            int startRow = DetailSheet.Cells[startRegion].Row;

            // 获得开始位置的列号  
            int startColumn = DetailSheet.Cells[startRegion].Column;

            // 写入Excel 
            DetailSheet.Cells.ImportDataTable(dtData, false, startRow, startColumn, true, true);

            // 设置执行公式计算 - 如果代码中用到公式，需要设置计算公式，导出的报表中，公式才会自动计算
            CurrentWorkbook.CalculateFormula(true);

            // 生成的文件名称
            string ReportFileName = string.Format(@"{0}\{1}.xls", uploadPath, guid);

            // 供应商数量汇总
            if (type == "SuppNumberTotal")
            {
                // 获取最后一行
                DataRow row = dtData.Rows[dtData.Rows.Count - 1];

                // 配送
                decimal distribution = Convert.ToDecimal(row[3]) + Convert.ToDecimal(row[4]) + Convert.ToDecimal(row[5]);

                // 总计
                decimal total = Convert.ToDecimal(row[6]);

                // 调拨百分比
                Cell Allocation = DetailSheet.Cells["C" + (dtData.Rows.Count + 5)];
                if (total == 0)
                {
                    Allocation.PutValue(0);
                }
                else
                {
                    Allocation.PutValue(Convert.ToDecimal(row[2]) / Convert.ToDecimal(row[6]));
                }

                // 配送百分比
                Cell Distribution = DetailSheet.Cells["D" + (dtData.Rows.Count + 5)];
                if (total == 0)
                {
                    Distribution.PutValue(0);
                }
                else
                {
                    Distribution.PutValue((distribution) / Convert.ToDecimal(row[6]));
                }

                // 干线百分比
                Cell TrunkLine = DetailSheet.Cells["D" + (dtData.Rows.Count + 6)];
                if (distribution == 0)
                {
                    TrunkLine.PutValue(0);
                }
                else
                {
                    TrunkLine.PutValue(Convert.ToDecimal(row[3]) / (distribution));
                }

                // 终端百分比
                Cell BranchEnd = DetailSheet.Cells["E" + (dtData.Rows.Count + 6)];
                if (distribution == 0)
                {
                    BranchEnd.PutValue(0);
                }
                else
                {
                    BranchEnd.PutValue(Convert.ToDecimal(row[4]) / (distribution));
                }

                // 综合百分比
                Cell Comprehensive = DetailSheet.Cells["F" + (dtData.Rows.Count + 6)];
                if (distribution == 0)
                {
                    Comprehensive.PutValue(0);
                }
                else
                {
                    Comprehensive.PutValue(Convert.ToDecimal(row[5]) / (distribution));
                }

            }
            // 保存文件
            CurrentWorkbook.Save(ReportFileName, SaveFormat.Xlsx);
        }
        #endregion
    }
    /// <summary>
    /// 权限按钮枚举类
    /// </summary>
    public enum OperateEnum
    {
        /// <summary>
        /// 无操作
        /// </summary>
        [Description("无操作")]
        None,
        /// <summary>
        /// 查看操作
        /// </summary>
        [Description("查看")]
        View,
        /// <summary>
        /// 添加操作
        /// </summary>
        [Description("添加")]
        Add,
        /// <summary>
        /// 修改操作
        /// </summary>
        [Description("修改")]
        Edit,
        /// <summary>
        /// 删除操作
        /// </summary>
        [Description("删除")]
        Delete,
        /// <summary>
        /// 保存操作
        /// </summary>
        [Description("保存")]
        Save,
        /// <summary>
        /// 查询操作
        /// </summary>
        [Description("查询")]
        Search,
        /// <summary>
        /// 导入操作
        /// </summary>
        [Description("导入")]
        Import,
        /// <summary>
        /// 导出操作
        /// </summary>
        [Description("导出")]
        Export,
        /// <summary>
        /// 设置操作
        /// </summary>
        [Description("设置")]
        Set,
        /// <summary>
        /// 重置操作
        /// </summary>
        [Description("重置")]
        ReSet,
        /// <summary>
        /// 审核操作
        /// </summary>
        [Description("审核")]
        Check,
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject,
        /// <summary>
        /// 授权操作
        /// </summary>
        [Description("授权")]
        Auth,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        Submit,
        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        Invalid,
        /// <summary>
        /// 追加
        /// </summary>
        [Description("追加")]
        Append,
        /// <summary>
        /// 考核
        /// </summary>
        [Description("考核")]
        Assessment,
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        Sort,
        /// <summary>
        /// 分配
        /// </summary>
        [Description("分配")]
        Allot,
        /// <summary>
        /// 撤销
        /// </summary>
        [Description("撤销")]
        Revoke,
        /// <summary>
        /// 维护
        /// </summary>
        [Description("维护")]
        Maintain,
        [Description("线路")]
        Line,
        [Description("评估")]
        Assess,
        [Description("评估查看")]
        AssessView,
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Start,
        /// <summary>
        /// 明细
        /// </summary>
        [Description("明细")]
        Detail,
        /// <summary>
        ///通知
        /// </summary>
        [Description("通知")]
        Notice,
        /// <summary>
        ///结束
        /// </summary>
        [Description("结束")]
        End,
        /// <summary>
        /// 评分
        /// </summary>
        [Description("评分")]
        Score,
        /// <summary>
        /// 轨迹
        /// </summary>
        [Description("轨迹")]
        Trail,
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Deactivate,
        /// <summary>
        /// 拉黑
        /// </summary>
        [Description("拉黑")]
        Blacklisted,
        /// <summary>
        /// 验证
        /// </summary>
        [Description("验证")]
        Validated,
        /// <summary>
        /// 整改
        /// </summary>
        [Description("整改")]
        Reform,
        /// <summary>
        /// 整改
        /// </summary>
        [Description("下载")]
        DownLoad
    }
    /// <summary>
    /// 操作结果枚举类
    /// </summary>
    public enum ResultEnum
    {
        [Description("成功")]
        Sucess,
        [Description("失败")]
        Fail,
        [Description("重复")]
        Exist,
        [Description("依赖")]
        Depend
    }

    /// <summary>
    /// 权限辅助操作
    /// </summary>
    public class OperateAttribute : ActionFilterAttribute
    {
        public OperateEnum Name { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool res = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!res)
                {

                    filterContext.HttpContext.Response.Write("<script>top.layui.layer.msg('请先登录账号,然后再操作！');</script>");
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    JsonResult json = new JsonResult();
                    json.Data = new { flag = "noauth", content = "您没有操作权限,请及时联系管理员！" };
                    filterContext.Result = json;
                }
            }
            else
            {
                string modid = System.Web.HttpContext.Current.Request.Cookies["modid"].Value;

                int modids = 0;

                if (!string.IsNullOrEmpty(modid))
                {
                    modids = Convert.ToInt32(modid);
                }

                string roleids = Auxiliary.RoleIDS();

                string opername = string.Empty;

                if (Name == OperateEnum.None)
                {
                    opername = (filterContext.RouteData.Values["action"]).ToString();
                }
                else
                {
                    opername = Name.ToString();
                }

                if (modids == 0 || string.IsNullOrEmpty(roleids))
                {
                    ExecResults(res, filterContext);
                }
                bool isAuth = new SysUserBLL().IsHasOperateAuth(roleids, modid, opername);
                //var operate = CacheHelper.GetCache("OperateDate");
                //DataTable dt = CacheHelper.GetCache("OperateDate") as DataTable;
                //if (dt == null)
                //{
                //    SysRoleOperateBLL operateBll = new SysRoleOperateBLL();
                //    dt = operateBll.GetTable(" ");
                //    CacheHelper.SetCache("OperateDate", dt, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.Zero); 
                //} 
                //DataRow[] row = dt.Select(string.Format(" RoleId in ({0}) and ModuleId={1} and Code='{1}' ", roleids, modid, opername));
                //if(row==null)
                //{
                //    ExecResults(res, filterContext);
                //}

                if (!isAuth)
                {
                    ExecResults(res, filterContext);
                }
            }
        }

        public void ExecResults(bool isAjax, ActionExecutingContext filterContext)
        {
            if (!isAjax)
            {
                //JavaScriptResult js = new JavaScriptResult();
                //js.Script = "";

                // filterContext.Result = js;

                filterContext.HttpContext.Response.Write("<script>top.layui.layer.msg('您没有操作权限,请及时联系管理员！');</script>");
                //filterContext.HttpContext.Response.Write(filterContext.HttpContext.Request.Url.ToString());
                filterContext.HttpContext.Response.End();
                //HttpContext.Current.Response.Write("<script>top.layui.layer.msg('您没有操作权限,请及时联系管理员！')</script>");
            }
            else
            {
                JsonResult json = new JsonResult();
                json.Data = new { flag = "noauth", content = "您没有操作权限,请及时联系管理员！" };
                filterContext.Result = json;
            }
        }
    }

    /// <summary>
    /// 缓存处理类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        public static object GetCache(string CacheKey)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        /// <param name="objObject">要插入缓存中的对象</param>
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        /// <param name="objObject">要插入缓存中的对象</param>
        /// <param name="Timeout">最后一次访问所插入对象时与该对象到期时之间的时间间隔</param>
        public static void SetCache(string CacheKey, object objObject, TimeSpan Timeout)
        {
            string virtrulPath = string.Format("/upload/{0}/{1}", "cache", "CacheHelper.xml");
            string strPath = System.Web.HttpContext.Current.Server.MapPath(virtrulPath);   
            //创建缓存依赖
            CacheDependency dep = new CacheDependency(strPath);
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, dep, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        /// <param name="objObject">要插入缓存中的对象</param>
        /// <param name="absoluteExpiration">所插入对象将到期并被从缓存中移除的时间 注意：请使用 System.DateTime.UtcNow 而不是System.DateTime.Now 作为此参数值。如果使用绝对到期</param>
        /// <param name="slidingExpiration">最后一次访问所插入对象时与该对象到期时之间的时间间隔</param>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            try
            {
                string virtrulPath = string.Format("/upload/{0}/{1}", "cache", "CacheHelper.xml");
                string strPath = System.Web.HttpContext.Current.Server.MapPath(virtrulPath);
                //创建缓存依赖
                CacheDependency dep = new CacheDependency(strPath);
                System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                objCache.Insert(CacheKey, objObject, dep, absoluteExpiration, slidingExpiration);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        public static void RemoveAllCache(string CacheKey)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            _cache.Remove(CacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
    #region 日志处理
    public class LogHelper
    {
        public string getProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }

            List<LogEntity> list = new List<LogEntity>();

            foreach (System.Reflection.PropertyInfo item in properties)
            {
                DescriptionAttribute attr = ((DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)));
                if (attr == null)
                {
                    continue;
                }

                string des = attr.Description;// 属性值  

                if (!string.IsNullOrEmpty(des))
                {
                    object value = item.GetValue(t, null);  //值  

                    if (value == null) continue;

                    list.Add(new LogEntity { Name = des, Value = value.ToString() });
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        public string getProperties<T>(T t1, T t2)
        {
            string tStr = string.Empty;
            if (t1 == null || t2 == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] pinfo1 = t1.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (pinfo1.Length <= 0)
            {
                return tStr;
            }

            System.Reflection.PropertyInfo[] pinfo2 = t1.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (pinfo2.Length <= 0)
            {
                return tStr;
            }

            List<LogEditEntity> list = new List<LogEditEntity>();

            for (int i = 0; i < pinfo1.Length; i++)
            {
                object obj1 = pinfo1[i].GetValue(t1, null);
                object obj2 = pinfo1[i].GetValue(t2, null);

                if (obj1 == null || obj2 == null)
                {
                    continue;
                }

                if (!obj1.Equals(obj2))
                {
                    DescriptionAttribute attr = ((DescriptionAttribute)Attribute.GetCustomAttribute(pinfo1[i], typeof(DescriptionAttribute)));
                    if (attr == null)
                    {
                        continue;
                    }

                    string des = attr.Description;// 属性值  

                    if (!string.IsNullOrEmpty(des))
                    {
                        string objvalue1 = string.Empty;
                        string objvalue2 = string.Empty;

                        if (obj1 != null)
                        {
                            objvalue1 = obj1.ToString();
                        }
                        if (obj2 != null)
                        {
                            objvalue2 = obj2.ToString();
                        }

                        list.Add(new LogEditEntity
                        {
                            Name = des,
                            OldValue = objvalue1,
                            NewValue = objvalue2
                        });
                    }
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
    }

    public class LogEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class LogEditEntity
    {
        public string Name { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
    #endregion
}