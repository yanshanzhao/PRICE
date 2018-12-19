//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-30    1.0        zbb        新建               
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Model.Basis;
using Web.Controllers;
using BLL.Basis;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：BasisDictionaryController
 * 功能描述：系统字典控制器 
 * ******************************/

namespace Web.Areas.Basis.Controllers
{
    public class BasisDictionaryController : Controller
    {
        //
        // GET: /Basis/BasisDictionary/

        //系统字典BLL
        BasisDictionaryBLL bll = new BasisDictionaryBLL();

        //系统字典Model
        BasisDictionaryModel model = new BasisDictionaryModel();

        #region 页面

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns> 
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(string tId, string tOperate)
        {
            // 获取数据
            model = bll.GetModelByID(tId);
            return View(model);
        }

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

        #region 新增选择供应商规模信息

        /// <summary>
        /// 选择供应商规模信息
        /// </summary>
        public ActionResult ChooseDictionary(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// 公司分页列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        public ActionResult ChooseDictionaryList(int index, int size)
        {
            List<BasisDictionaryModel> list = bll.ChooseDictionaryList(index, size);
            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="nodeName">关键点名称</param>
        /// <param name="companyName">公司名称</param>
        /// <returns></returns>
        public int ChooseDictionaryCount()
        {
            return bll.ChooseDictionaryCount();
        }
        #endregion

        #region 新增选择信息类型
        /// <summary>
        /// 选择供应商规模信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult ChooseMessageType(string url)
        {
            ViewBag.url = url;
            return View();
        }

        /// <summary>
        /// 公司分页列表数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ActionResult ChooseMessageTypeList(int index, int size)
        {
            List<BasisDictionaryModel> list = bll.ChooseMessageTypeList(index, size);
            return Json(list);
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <returns></returns>
        public int ChooseMessageTypeCount()
        {
            return bll.ChooseMessageTypeCount();
        }
        #endregion

        #region 数据集

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="Dictionary"></param>
        /// <returns></returns>  
        public ActionResult DictionaryList(int index, int size, string DictionaryName, string DictionaryTypeName)
        {
            string where = "  1=1 ";

            //字典名称
            if (!string.IsNullOrEmpty(DictionaryName))
            {
                where += string.Format(" And dic.DictionaryName  like '%{0}%' ", DictionaryName.Trim());
            }

            //字典类型名称
            if (!string.IsNullOrEmpty(DictionaryTypeName))
            {
                where += string.Format(" And dic.DictionaryType ={0}", DictionaryTypeName.Trim());
            }

            List<BasisDictionaryModel> list = bll.BasisDictionaryPageList(index, size, where);

            return Json(list);
        }
        #endregion

        #region 数据记录数

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="Dictionary"></param>
        /// <returns></returns>
        public int DictionaryCount(string DictionaryName, string DictionaryTypeName)
        {
            string where = "  1=1 ";

            //字典名称
            if (!string.IsNullOrEmpty(DictionaryName))
            {
                where += string.Format(" And dic.DictionaryName  like '%{0}%' ", DictionaryName.Trim());
            }

            //字典类型名称
            if (!string.IsNullOrEmpty(DictionaryTypeName))
            {
                where += string.Format(" And dic.DictionaryType ={0}", DictionaryTypeName.Trim());
            }

            return bll.BasisDictionaryPageCount(where);
        }
        #endregion

        #region 系统字典新增

        /// <summary>
        /// 系统字典新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Add)]
        public ActionResult AddDictionary(BasisDictionaryModel model)
        {
            if (bll.IsDictionaryNumber(model.DictionaryNumber.Trim(), model.DictionaryType) != 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }


            if (bll.AddBasisDictionary(model) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "保存成功！" });
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
        public ActionResult EditDictionary(BasisDictionaryModel model)
        {
            BasisDictionaryModel beforeModel = bll.GetModelByID(model.DictionaryId.ToString());

            int result = bll.UpdateBasisDictionary(model);

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
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult DelDictionary(string Id)
        {
            BasisDictionaryModel beforeModel = bll.GetModelByID(model.DictionaryId.ToString());
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

        #region 导出按钮逻辑

        /// <summary>
        /// 导出按钮逻辑
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string DictionaryName, string DictionaryTypeName)
        {
            string where = "  1=1 ";

            //字典名称
            if (!string.IsNullOrEmpty(DictionaryName))
            {
                where += string.Format(" And dic.DictionaryName  like '%{0}%' ", DictionaryName.Trim());
            }

            //字典类型名称
            if (!string.IsNullOrEmpty(DictionaryTypeName))
            {
                where += string.Format(" And dic.DictionaryType ={0}", DictionaryTypeName.Trim());
            }

            System.Data.DataTable dt = bll.ExportDataTable(where);

            Common.ExcelHelper excel = new Common.ExcelHelper();

            string url = excel.ExcelToDisk(dt);

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        #endregion
    }
}