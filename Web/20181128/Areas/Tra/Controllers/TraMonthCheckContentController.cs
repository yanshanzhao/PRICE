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
 * 类名：TraMonthCheckContentController
 * 功能描述：运输月度考核内容表 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra
{
    public class TraMonthCheckContentController : Controller
    {
        //
        // GET: /Tra/TraMonthCheckContent/

        // 运输月度考核内容BLL
        TraMonthCheckContentBLL bll = new TraMonthCheckContentBLL();

        // 运输月度考核元件BLL
        TraMonthCheckComponentBLL TMCCbll = new TraMonthCheckComponentBLL();

        // 运输月度考核附件BLL
        TraMonthCheckAdjunctBLL TMCAbll = new TraMonthCheckAdjunctBLL();

        // 运输月度考核BLL
        TraMonthCheckBLL TMCbll = new TraMonthCheckBLL();

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
            List<TempMonthCheckAdjunctModel> fileList = TMCAbll.AdjunctListById(checkId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(fileList);
            List<string> oldlist = fileList.Select(p => p.FileName + p.ext).ToList<string>(); 
              
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
            TraMonthCheckContentModel model = bll.GetModelByID(tId);

            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult AddContentScoreList(TraMonthCheckContentModel tModel)
        {
            int row = 0;

            if (!string.IsNullOrEmpty(tModel.ScoreList))
            {
                List<TempMonthCheckContentModel> scoreList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempMonthCheckContentModel>>(tModel.ScoreList);

                row = bll.AddContentScoreList(scoreList, tModel.CheckId);
            }
            if (row>0)
            {
                // 修改TraMonthCheck得分(根据CheckId)
                TMCbll.EditScore(tModel.CheckId, tModel.TotalScore);

                // 如果同checkID所对应的修改整改状态
                // TMCbll.ChangeIsNorm(tModel.CheckId, 1);

                // 附件List
                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    List<TempMonthCheckAdjunctModel> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempMonthCheckAdjunctModel>>(tModel.AdjunctList);

                    TMCAbll.AddContentAdjunctList(adjunctList, tModel.CheckId);
                }

                // 系统日志
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
            return Json(new { flag = "success" });

            }
             
            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 数据集 运输月度考核元件
        /// </summary> 
        /// <returns>Json</returns>
        public ActionResult CompnentList(int checkFromId)
        {
            List<TraMonthCheckComponentModel> compnentList = TMCCbll.CompnentList(checkFromId);

            return Json(compnentList);
        }

        /// <summary>
        /// 数据集 运输月度考核内容
        /// </summary>
        /// <param name="checkId">月度考核id</param> 
        /// <returns>Json</returns>
        public ActionResult MonthCheckContentList(int checkId,string type,int checkFromId)
        {
            // 根据CheckId 
            string where = " CheckId = " + checkId + " AND CheckFromId=" + checkFromId;
             
            // 运输月度考核内容List
            List<TraMonthCheckContentModel> list = bll.MonthCheckContentList(where);

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
        /// 数据记录数
        /// </summary>
        /// <param name="checkComponentNumber">元件编号</param>
        /// <param name="checkComponentName">元件名称</param>
        /// <param name="checkComponentType">考核类型</param>
        /// <param name="project">项目</param>
        /// <param name="isFormula">公式计算</param>
        /// <param name="state">状态</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        public int MonthCheckContentAmount(string supplierName, string year, string month, string state, string createTime)
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

            return bll.MonthCheckContentAmount(where);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public ActionResult EditMonthCheckContent(TraMonthCheckContentModel tModel)
        {
            // 编辑之前Model
            TraMonthCheckContentModel beforeModel = bll.GetModelByCheckId(tModel.CheckId);

            // 影响行数
            int row =0;

            if (!string.IsNullOrEmpty(tModel.ScoreList))
            {
                List<TempMonthCheckContentModel> scoreList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempMonthCheckContentModel>>(tModel.ScoreList);

                row = bll.AddContentScoreList(scoreList, tModel.CheckId);
            }

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 修改TraMonthCheck得分(根据CheckId)
                TMCbll.EditScore(tModel.CheckId, tModel.TotalScore);

                if (!string.IsNullOrEmpty(tModel.AdjunctList))
                {
                    List<TempMonthCheckAdjunctModel> adjunctList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TempMonthCheckAdjunctModel>>(tModel.AdjunctList);

                    TMCAbll.AddContentAdjunctList(adjunctList, tModel.CheckId);
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
