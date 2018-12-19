//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-04    1.0        zbb        新建               
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRM.Web.Controllers;
using SRM.BLL.Supplier;
using SRM.Model.Supplier;
using Newtonsoft.Json.Converters;
using SRM.Model.Basis;
#endregion
/*********************************
 * 类名：SupplierTurnoverLevelController
 * 功能描述：供应商规模级别维护控制器 
 * ******************************/

namespace SRM.Web.Areas.Supplier.Controllers
{
    public class SupplierTurnoverLevelController : Controller
    {
        //
        // GET: /Supplier/SupplierTurnoverLevel/

        SupplierTurnoverLevelBLL bll = new SupplierTurnoverLevelBLL();
        SupplierTurnoverLevelModel model = new SupplierTurnoverLevelModel();

        #region 页面

        #region Index
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Add
        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns> 
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }
        #endregion

        #region Edit
        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(string tId)
        { 
            // 获取数据
            model = bll.GetModelByID(tId);
            return View(model);
        }
        #endregion

        /// <summary>
        /// 查看
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(string tId)
        {
            // 获取数据
            model = bll.GetModelByID(tId);

            return View(model);
        }

        #endregion

        #region 方法

        #region 选择供应商规模信息

        /// <summary>
        /// 选择供应商规模信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult ChooseTurnoverLevel(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// 公司分页列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="companyName">关键词</param>
        /// <returns></returns>
        public ActionResult TurnoverLevelList(int index, int size, string companyName)
        {
            string where = string.Format("  stl.CompanyId={0}", Auxiliary.CompanyID());

            //公司名称
            if (!string.IsNullOrEmpty(companyName))
            {
                where += string.Format(" And com.CompanyName like '%{0}%'", companyName.Trim());
            }

            List<SupplierTurnoverLevelModel> list = bll.SupplierTurnoverLevelList(index, size, where);

            return Json(list);
        }

        #region 数据记录数

        /// <summary>
        /// 数据记录数
        /// </summary> 
        /// <param name="companyName">公司名称</param>
        /// <returns></returns>
        public int TurnoverLevelAmount(string companyName)
        {
            string where = string.Format("  stl.CompanyId={0}", Auxiliary.CompanyID());

            //公司名称
            if (!string.IsNullOrEmpty(companyName))
            {
                where += string.Format(" And com.CompanyName like '%{0}%'", companyName.Trim());
            }

            return bll.TurnoverLevelAmount(where);
        }

        #endregion

        #endregion

        #region 数据集

        /// <summary>
        /// 数据集 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="DictionaryName">供应商种类</param>
        /// <param name="SuppTurnoverName">规模级别名称</param>
        /// <param name="IsTurnover">营业额限制</param>
        /// <param name="SuppTurnoverState">状态</param>
        /// <returns></returns>
        public ActionResult SupplierTurnoverLevellist(int index, int size,string DictionaryName, string SuppTurnoverName, string IsTurnover, string SuppTurnoverState)
        {
            string where = string.Format("  com.CompanyId={0}", Auxiliary.CompanyID());

            //供应商种类
            if (!string.IsNullOrEmpty(DictionaryName))
            {
                where += string.Format(" And stl.SuppKindId={0}", DictionaryName.Trim());
            }

            //规模级别名称
            if (!string.IsNullOrEmpty(SuppTurnoverName))
            {
                where += string.Format(" And stl.SuppTurnoverName  like '%{0}%' ", SuppTurnoverName.Trim());
            }

            //营业额限制
            if (!string.IsNullOrEmpty(IsTurnover))
            {
                where += string.Format(" And stl.IsTurnover ={0}", IsTurnover.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(SuppTurnoverState))
            {
                where += string.Format(" And stl.SuppTurnoverState ={0}", SuppTurnoverState.Trim());
            }

            List<SupplierTurnoverLevelModel> list = bll.TurnoverLevelList(index, size, where);
            return Json(list);
        }
        #endregion

        #region 数据记录数

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="DictionaryName">供应商种类</param>
        /// <param name="SuppTurnoverName">规模级别名称</param>
        /// <param name="IsTurnover">营业额限制</param>
        /// <param name="SuppTurnoverState">状态</param>
        /// <returns></returns>
        public int TurnoverLevelCount(string DictionaryName, string SuppTurnoverName, string IsTurnover, string SuppTurnoverState)
        {
            string where = string.Format("  com.CompanyId={0}", Auxiliary.CompanyID());

            //供应商种类
            if (!string.IsNullOrEmpty(DictionaryName))
            {
                where += string.Format(" And stl.SuppKindId={0}", DictionaryName.Trim());
            }

            //规模级别名称 
            if (!string.IsNullOrEmpty(SuppTurnoverName))
            {
                where += string.Format(" And stl.SuppTurnoverName  like '%{0}%' ", SuppTurnoverName.Trim());
            }

            //营业额限制
            if (!string.IsNullOrEmpty(IsTurnover))
            {
                where += string.Format(" And stl.IsTurnover ={0}", IsTurnover.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(SuppTurnoverState))
            {
                where += string.Format(" And stl.SuppTurnoverState ={0}", SuppTurnoverState.Trim());
            }
            return bll.TurnoverLevelCount(where);
        }
        #endregion 

        #region 系统字典新增

        /// <summary>
        /// 系统字典新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Add)]
        public ActionResult AddSupplierTurnoverLeve(SupplierTurnoverLevelModel model)
        { 
            model.CompanyId = Auxiliary.CompanyID();//公司id

            if (bll.AddSupplierTurnoverLevel(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 系统字典编辑

        /// <summary>
        /// 系统字典编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult EditSupplierTurnoverLevel(SupplierTurnoverLevelModel model)
        {
            SupplierTurnoverLevelModel beforeModel = bll.GetModelByID(model.SuppTurnoverId.ToString());

            int result = bll.UpdateSupplierTurnoverLevel(model);

            if (result > 0)
            {
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success", content = "编辑成功！" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 系统字典作废

        /// <summary>
        /// 系统字典作废
        /// </summary>
        /// <param name="Id"></param> 
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult DelSupplierTurnoverLevel(string Id)
        {
            SupplierTurnoverLevelModel beforeModel = bll.GetModelByID(model.SuppTurnoverId.ToString());

            int row = bll.ChangeState(Id, 0);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 导出数据

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="DictionaryName">供应商种类</param>
        /// <param name="SuppTurnoverName">规模级别名称</param>
        /// <param name="IsTurnover">营业额限制</param>
        /// <param name="SuppTurnoverState">状态</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string DictionaryName, string SuppTurnoverName, string IsTurnover, string SuppTurnoverState)
        { 
            string where = string.Format("  com.CompanyId={0}", Auxiliary.CompanyID());

            //供应商种类
            if (!string.IsNullOrEmpty(DictionaryName))
            {
                where += string.Format(" And stl.SuppKindId={0}", DictionaryName.Trim());
            }

            //规模级别名称 
            if (!string.IsNullOrEmpty(SuppTurnoverName))
            {
                where += string.Format(" And stl.SuppTurnoverName  like '%{0}%' ", SuppTurnoverName.Trim());
            }

            //营业额限制
            if (!string.IsNullOrEmpty(IsTurnover))
            {
                where += string.Format(" And stl.IsTurnover ={0}", IsTurnover.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(SuppTurnoverState))
            {
                where += string.Format(" And stl.SuppTurnoverState ={0}", SuppTurnoverState.Trim());
            }

            System.Data.DataTable dt = bll.ExportDataTable(where);
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        /// <summary>
        /// 获取供应商种类
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns> 
        public ActionResult GetDictionaryName()
        {
            // 服务类型
            List<BasisDictionaryModel> listDictionaryName = bll.GetDictionaryName();
            return Json(listDictionaryName);
        }

        #endregion
    }
}
