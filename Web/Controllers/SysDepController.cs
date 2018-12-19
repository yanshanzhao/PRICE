//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-25    1.0        HDS        新建               
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
 * 类名：SysDepController
 * 功能描述：系统组织机构 控制器  
 * ******************************/
namespace Web.Controllers
{ 
    public class SysDepController : Controller
    {
        SysDepartmentBLL bll = new SysDepartmentBLL();
        /// <summary>
        /// 机构列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 机构列表数据（无分页）
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>机构列表数据</returns>
        public ActionResult List(string keyword)
        {
            keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
            int cid = Auxiliary.CompanyID();
            return Json(bll.SysDepartmentPageList(cid, keyword));
        }
        /// <summary>
        /// 机构树处理
        /// </summary>
        public ActionResult TreeList(string url)
        {
            int cid = Auxiliary.CompanyID();

            List<TreeModel> list = bll.DepTreeList(cid);

            if (url.Contains("sysdep/index"))
            {              
               list.Insert(0,new TreeModel { id="-1",pid="0",name="根目录" });
            }

             return Json(list);                    
        }
        /// <summary>
        /// 机构树处理
        /// </summary>
        public ActionResult TreeLists(string url)
        {
            int cid = Auxiliary.CompanyID();

            List<TreeModel> list = bll.DepTreeLists(cid);

            if (url.Contains("sysdep/index"))
            {
                list.Insert(0, new TreeModel { id = "-1", pid = "0", name = "根目录" });
            }

            return Json(list);
        }
        /// <summary>
        /// 机构树显示页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult Tree(string url)
        {
            ViewBag.url = url;

            return View();
        }

        /// <summary>
        /// 新增机构页面
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="name">名称</param>
        [Operate(Name=OperateEnum.Add)]
        public ActionResult Add(string id,string name)
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
        /// 新增机构处理函数
        /// </summary>
        public ActionResult AddDep(SysDepartmentModel model)
        {
            model.DepartmentSpelling = string.Empty;
            model.CompanyId = Auxiliary.CompanyID();

            model.Address = model.Address ?? string.Empty;
            model.Tel = model.Tel ?? string.Empty;
            model.DeotNo = model.DeotNo ?? string.Empty;

            if(bll.IsHasDep(model.CompanyId, 0, model.FatherId, model.DepartmentName.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            if (bll.AddSysDepartment(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///更改机构状态
        /// </summary>
        [Operate(Name=OperateEnum.Edit)]
        public ActionResult ChangeState(int state, string did)
        {
            int i= bll.ChangeState(state, did);

            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, new {detail="更改状态",depid=did,state=state });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, new { detail = "更改状态", depid = did, state = state });
            return Json(new { flag = "fail" });  
        }

        /// <summary>
        /// 删除机构
        /// </summary>
       [Operate(Name = OperateEnum.Delete)]
        public ActionResult DelDep(string ids)
        {
            SysDepartmentModel delmodel = bll.GetModelByID(ids);
            if (bll.SysDepartmentUserCount(ids) > 0)
            {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Depend, delmodel);
                return Json(new { flag = "exsit" });
            }
            int i= bll.DeleteSysDepartmentByID(ids);
            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Sucess, delmodel);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Delete, ResultEnum.Fail, delmodel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 机构显示页面
        /// </summary>
        public ActionResult ViewDep(string ids)
        {
            SysDepartmentModel model = bll.GetModelByID(ids);

            if (model.FatherId == 0)
            {
                model.DepartmentPName = "根目录";
            }
            else
            {
                model.DepartmentPName = bll.GetModelByID(model.FatherId.ToString()).DepartmentName;
            }

            return Json(model);
        }
        /// <summary>
        ///机构编辑页面
        /// </summary>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(string ids)
        {
            SysDepartmentModel model = bll.GetModelByID(ids);

            if (model.FatherId == 0)
            {
                model.DepartmentPName = "根目录";
            }
            else
            {
                model.DepartmentPName = bll.GetModelByID(model.FatherId.ToString()).DepartmentName;
            }

            return View(model);
        }
        /// <summary>
        /// 机构修改处理函数
        /// </summary>
        public ActionResult ModifyDep(SysDepartmentModel model)
        {
            SysDepartmentModel beforeModel = bll.GetModelByID(model.DepartmentId.ToString());

            model.DepartmentSpelling = string.Empty;
            model.CompanyId = Auxiliary.CompanyID();

            model.Address = model.Address ?? string.Empty;
            model.Tel = model.Tel ?? string.Empty;
            model.DeotNo = model.DeotNo ?? string.Empty;

            model.CompanyId = Auxiliary.CompanyID();

            int result = bll.UpdateSysDepartment(model);
            if (result > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        /// <summary>
        /// 机构用户选择页面
        /// </summary>
        /// <param name="url">功能调用页面地址（需和菜单中url一致)</param>
        public ActionResult DepUser(string url)
        {
            ViewBag.url = url;
            return View();
        }
        /// <summary>
        /// 机构用户选择分页列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="keyword">关键词</param>
        public ActionResult SelectList(int index, int size, string keyword)
        {
            int cid = Auxiliary.CompanyID();

            return Json(bll.SysDepartmentPageList(index, size, cid, keyword));
        }
        /// <summary>
        ///机构用户选择列表数据总数
        /// </summary>
        public int SelectCount(string keyword)
        {
            int cid = Auxiliary.CompanyID();

            return bll.SysDepartmentPageCounts(cid, keyword);
        }

        #region 列表式机构+真实姓名
        /// <summary>
        /// 机构+真实姓名
        /// </summary>
        /// <param name="url">功能调用页面地址（需和菜单中url一致)</param>
        public ActionResult DepPersonnel(string url)
        {
            ViewBag.url = url;
            return View();
        }
        /// <summary>
        /// 机构用户选择分页列表
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="keyword">关键词</param>
        public ActionResult SysDepartmentList(int index, int size, string departmentName)
        {
            int cid = Auxiliary.CompanyID(); 

            string where = " d.State=1";
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And d.DepartmentName+d.DepartmentSpelling like '%{0}%'", departmentName.Trim());
            }

            return Json(bll.SysDepartmentList(index, size, cid, where));
        }
        /// <summary>
        ///机构用户选择列表数据总数
        /// </summary>
        public int SysDepartmentCounts(string departmentName)
        {
            int cid = Auxiliary.CompanyID();

            string where = " d.State=1";
            if (!string.IsNullOrEmpty(departmentName))
            {
                where += string.Format(" And d.DepartmentName+d.DepartmentSpelling like '%{0}%'", departmentName.Trim());
            }

            return bll.SysDepartmentCounts(cid, where);
        }

        #endregion
    }
}
