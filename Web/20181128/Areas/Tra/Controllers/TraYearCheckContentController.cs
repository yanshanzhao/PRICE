//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-13    1.0        FJK        新建             
//-------------------------------------------------------------------------
#region 参考 
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using SRM.Web.Controllers;
using SRM.Model.Basis;
using System.Web.Mvc;
using System;
using SRM.Model.Supplier;
using System.Linq;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using SRM.BLL.Supplier;
using System.Text;
#endregion
/*********************************
 * 类名：TraYearCheckContentController
 * 功能描述：运输年度考核内容表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraYearCheckContentController : Controller
    {
        //
        // GET: /Tra/TraYearCheckContent/

        // 运输年度考核内容BLL
        TraYearCheckContentBLL bll = new TraYearCheckContentBLL();

        // 运输年度考核附件BLL
        TraYearCheckAdjunctBLL TYCAbll = new TraYearCheckAdjunctBLL();

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
        [Operate(Name = OperateEnum.Assessment)]
        public ActionResult Add(string url, int checkId, int checkFromId)
        {
            ViewBag.url = url;
            ViewBag.CheckId = checkId;
            ViewBag.CheckFromId = checkFromId;
            return View();
        }

        /// <summary>
        /// ContentFile
        /// </summary>
        /// <returns></returns>  
        public ActionResult ContentFile(string url, int tId)
        {
            ViewBag.url = url;
            ViewBag.Id = tId;
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns> 
        [Operate(Name = OperateEnum.Assessment)]
        public ActionResult Edit(int checkId, int checkFromId)
        {
            // 附件
            //List<TempYearCheckAdjunctModel> fileList = TYCAbll.AdjunctListById(checkId);
            //ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);
            //List<string> oldlist = fileList.Select(p => p.FileName + p.ext).ToList<string>();
            //ViewBag.oldfiles = String.Join(",", oldlist);

            ViewBag.CheckId = checkId;
            ViewBag.CheckFromId = checkFromId;
            return View();
        }

        /// <summary>
        /// View
        /// </summary>  
        public ActionResult View(int tId)
        {
            // 获取数据
            TraYearCheckContentModel model = bll.GetModelByID(tId);

            return View(model);
        }

        #endregion

        #region 方法
         
        /// <summary>
        /// 数据集 运输年度考核元件
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult YearCheckComponentList(int checkFromId)
        {
            List<TraMonthCheckComponentModel> compnentList = TMCCbll.CompnentList(checkFromId);

            return Json(compnentList);
        }

        /// <summary>
        /// 数据集 运输年度考核内容
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult YearCheckContentList(int checkId, string type, int checkFromId)
        {
            // 根据CheckId 
            string where = " CheckId = " + checkId + " AND CheckFromId=" + checkFromId;

            // 运输年度考核内容List
            List<TraYearCheckContentModel> list = bll.YearCheckContentList(where);

            if (type == "index")
            {
                if (list.Count > 0)
                {
                    return Json(new { flag = "success" });
                }

                return Json(new { flag = "fail" });
            }
            return Json(list);
        }

        /// <summary>
        /// 数据记录数 运输年度考核内容
        /// </summary> 
        /// <returns></returns>
        public int YearCheckContentAmount(string supplierName, string year, string month, string state, string createTime)
        {
            // 只能查询本机构创建的考核的有效信息。
            string where = " TMC.CreateDepartmentId=" + Auxiliary.DepartmentId() + " AND TMC.State != 10";

            // 运输供应商名称 
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" And SupplierName like '%{0}%'", supplierName.Trim());
            }

            // 考核年  
            if (!string.IsNullOrEmpty(year))
            {
                where += string.Format(" And CheckYear like '%{0}%'", year.Trim());
            }

            // 考核月  
            if (!string.IsNullOrEmpty(month))
            {
                where += string.Format(" And CheckMonth = {0}", month.Trim());
            }

            // 状态 
            if (!string.IsNullOrEmpty(state))
            {
                where += string.Format(" And TMC.State = {0}", state.Trim());
            }

            // 创建时间 
            if (!string.IsNullOrEmpty(createTime))
            {
                where += string.Format(" And convert(varchar,CreateTime,120) like '%{0}%'", createTime.Trim());
            }

            return bll.YearCheckContentAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditYearCheckContent(TraYearCheckContentModel tModel)
        {
            // 编辑之前Model
            TraYearCheckContentModel beforeModel = bll.GetModelByCheckId(tModel.CheckYearId);

            // 影响行数
            int row = 0;

            if (!string.IsNullOrEmpty(tModel.ScoreList))
            {
                List<TempYearCheckContentModel> scoreList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempYearCheckContentModel>>(tModel.ScoreList);

                row = bll.AddYearCheckContentList(scoreList, tModel.CheckYearId);
            }

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 修改TraMonthCheck得分(根据CheckId)
                TMCbll.EditScore(tModel.CheckYearId, tModel.TotalScore);

                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    List<TempYearCheckAdjunctModel> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempYearCheckAdjunctModel>>(tModel.AdjunctList);

                    TYCAbll.AddContentAdjunctList(adjunctList, tModel.CheckYearId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel);

                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }
        #endregion
    }
}
