//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-23    1.0        MH         新建               
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
 * 类名：SysAreasController
 * 功能描述：区域 控制器  
 * ******************************/

namespace SRM.Web.Controllers
{
    public class SysAreasController : Controller
    {
        SysAreasBLL bll = new SysAreasBLL();

        /// <summary>
        /// 区域列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 区域列表数据（无分页）
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>区域列表数据</returns>
        public ActionResult List(string keyword)
        {
            keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
            int cid = Auxiliary.CompanyID();
            return Json(bll.AreasList(keyword));
        }
        /// <summary>
        /// 区域树处理
        /// </summary>
        public ActionResult TreeList(string url)
        {
            int cid = Auxiliary.CompanyID();

            List<TreeModel> list = bll.AreasTreeList(cid);

            if (url.Contains("sysareas/index"))
            {
                list.Insert(0, new TreeModel { id = "-1", pid = "0", name = "根目录" });
            }

            return Json(list);
        }
        /// <summary>
        /// 区域树处理
        /// </summary>
        public ActionResult TreeLists(string url)
        {
            int cid = Auxiliary.CompanyID();

            List<TreeModel> list = bll.AreasTreeLists(cid);

            if (url.Contains("sysareas/index"))
            {
                list.Insert(0, new TreeModel { id = "-1", pid = "0", name = "根目录" });
            }

            return Json(list);
        }
        /// <summary>
        /// 区域树显示页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult Tree(string url)
        {
            ViewBag.url = url;

            return View();
        }
        public ActionResult TreesList(int id)
        {
            List<TreeModel> list = bll.AreaTreeByPid(id);
             
            //if (url.Contains("sysareas/index"))
            //{
            //    list.Insert(0, new TreeModel { id = "-1", pid = "0", name = "根目录" });
            //}

            return Json(list);
        }
        /// <summary>
        /// 区域树显示页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult Trees(string url)
        {
            ViewBag.url = url;

            return View();
        }



        public ActionResult AreaSelectView(string url)
        {
            ViewBag.url = url;

            return View();
        }

        /// <summary>
        /// 位置信息数量
        /// </summary>
        /// <param name="areaName">位置名称</param>
        /// <returns></returns>
        public int VAreasRowCount(string areaName)
        {
            string sql = " Where State=1 ";
            if(areaName.Trim().Length>0)
            {
                sql += " and AreaName LIKE '%" + areaName + "%' ";
            }
            return bll.VAreasRowCount(sql);
        }

        /// <summary>
        /// 位置信息
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="areaName">位置名称</param>
        /// <returns></returns>
        public ActionResult VAreasLiset(int index, int size, string areaName)
        {
            string sql = " Where State=1 ";
            if (areaName.Trim().Length > 0)
            { 
                sql += " and AreaName LIKE '%" + areaName + "%' ";
            }
            List<VAreas> lisetVarea = bll.VAreasLiset(index, size, sql);         
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(lisetVarea));
        }



        /// <summary>
        /// 新增区域页面
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="name">名称</param>
        [Operate(Name = OperateEnum.Add)]
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
        /// 新增区域处理函数
        /// </summary>
        public ActionResult AddAreas(SysAreasModel model)
        {
            model.CreateTime = DateTime.Now;

            if (bll.IsHasAreas(0, model.ParentId, model.AreaName.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            if (bll.AddSysAreas(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        ///更改区域状态
        /// </summary>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult ChangeState(int state, string did)
        {
            int i = bll.ChangeState(state, did);

            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, new { detail = "更改状态", depid = did, state = state });
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, new { detail = "更改状态", depid = did, state = state });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 删除区域
        /// </summary>
        [Operate(Name = OperateEnum.Delete)]
        public ActionResult DelAreas(string ids)
        {
            SysAreasModel delmodel = bll.GetModelByID(ids);
            
            int i = bll.DeleteSysAreasByID(ids);
            if (i > 0)
            {
                Auxiliary.Log(OperateEnum.Delete, ResultEnum.Sucess, delmodel);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Delete, ResultEnum.Fail, delmodel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 区域显示页面
        /// </summary>
        public ActionResult ViewAreas(string ids)
        {
            SysAreasModel model = bll.GetModelByID(ids);

            if (model.ParentId == 0)
            {
                model.AreaPName = "根目录";
            }
            else
            {
                model.AreaPName = bll.GetModelByID(model.ParentId.ToString()).AreaPName;
            }

            return Json(model);
        }
        /// <summary>
        ///区域编辑页面
        /// </summary>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(string ids)
        {
            SysAreasModel model = bll.GetModelByID(ids);

            if (model.ParentId == 0)
            {
                model.AreaPName = "根目录";
            }
            else
            {
                model.AreaPName = bll.GetModelByID(model.ParentId.ToString()).AreaName;
            }

            return View(model);
        }
        /// <summary>
        /// 区域修改处理函数
        /// </summary>
        public ActionResult ModifyAreas(SysAreasModel model)
        {
            SysAreasModel beforeModel = bll.GetModelByID(model.AreaId.ToString());

            if (bll.IsHasAreas(model.AreaId, model.ParentId, model.AreaName.Trim()) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            int result = bll.UpdateSysAreas(model);
            if (result > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "ok" });
            }

            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }

    }
}
