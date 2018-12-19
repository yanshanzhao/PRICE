//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-10-10    1.0        zbb        新建               
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SRM.Model.Tra;
using SRM.Web.Controllers;
using SRM.BLL.Tra;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：TraCultivateResourceController
 * 功能描述：培训资料维护控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraCultivateResourceController : Controller
    {
        //
        // GET: /Tra/TraCultivateResource/

        //培训资料维护bll
        TraCultivateResourceBLL bll = new TraCultivateResourceBLL();

        //培训资料维护model
        TraCultivateResourceModel model = new TraCultivateResourceModel(); 

        TraCultivateResourceAdjuncctBLL bmabll = new TraCultivateResourceAdjuncctBLL(); 

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
            ViewBag.ResourceNumber = Auxiliary.CurCompanyAutoNum("TCR");
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            TraCultivateResourceModel model = bll.GetModelByID(tId);

            List<temTraCultivateResourceAdjunt> filelist = bmabll.TraCultivateResourceAdjuntFileList(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();
            ViewBag.oldfiles = String.Join(",", oldlist);

            return View(model);
        }

        /// <summary>
        /// 查看
        /// </summary> 
        [Operate(Name = OperateEnum.View)]
        public ActionResult Check(int tId)
        {
            // 获取数据
            model = bll.GetModelByID(tId);

            List<temTraCultivateResourceAdjunt> filelist = bmabll.TraCultivateResourceAdjuntFileList(tId);
            ViewBag.files = Newtonsoft.Json.JsonConvert.SerializeObject(filelist);

            List<string> oldlist = filelist.Select(p => p.filename + p.ext).ToList<string>();

            ViewBag.oldfiles = String.Join(",", oldlist);
            return View(model);
        }
        #endregion

        #region 方法

        #region 数据集 培训资料维护 

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="ResourceName">培训资料名称</param>
        /// <param name="State">状态</param> 
        /// <param name="CreateTime">创建时间</param> 
        /// <returns></returns>
        public ActionResult TraCultivateResourceList(int index, int size, string ResourceName, string State, string CreateTime)
        {   
            string where = " (TCR.State = 0 OR TCR.State = 1) and TCR.CreateDepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + "))";
            //培训资料名称
            if (!string.IsNullOrEmpty(ResourceName))
            {
                where += string.Format(" And TCR.ResourceName  like '%{0}%' ", ResourceName.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And TCR.State ={0}", State.Trim());
            }

            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += string.Format(" And convert(varchar,TCR.CreateTime,120) like '%{0}%'", CreateTime.Trim());
            }

            List<TraCultivateResourceModel> list = bll.TraCultivateResourcePageList(index, size, where);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 数据记录数 培训资料维护

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="ResourceName">培训资料名称</param>
        /// <param name="State">状态</param> 
        /// <param name="CreateTime">创建时间</param> 
        /// <returns></returns>
        public int TraCultivateResourceCount(string ResourceName, string State, string CreateTime)
        {
            string where = " (TCR.State = 0 OR TCR.State = 1) and TCR.CreateDepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + "))";

            //培训资料名称
            if (!string.IsNullOrEmpty(ResourceName))
            {
                where += string.Format(" And TCR.ResourceName  like '%{0}%' ", ResourceName.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And TCR.State ={0}", State.Trim());
            }

            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += string.Format(" And convert(varchar,TCR.CreateTime,120) like '%{0}%'", CreateTime.Trim());
            }

            return bll.TraCultivateResourcePageCount(where);
        }
        #endregion

        #region 培训资料维护新增

        /// <summary>
        /// 培训资料维护新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns> 
        public ActionResult AddTraCultivateResource(TraCultivateResourceModel model)
        { 
            model.State = 0;// 默认状态为初始状态0

            model.CompanyId = Auxiliary.CompanyID();//公司id

            model.CreateDepartmentId = Auxiliary.DepartmentId();//新增机构id

            model.ResourceNumber = Auxiliary.CurCompanyAutoNum("TCR"); 

            model.CreateUserId = Auxiliary.UserID();//新增负责人id

            model.CreateTime = DateTime.Now;//新增时间

            int ResourceId = bll.AddTraCultivateResource(model);

            if (ResourceId > 0)
            {
                //string filenames = string.Empty;
                //if (!string.IsNullOrEmpty(model.TraCultivateResource))
                //{
                //    List<temTraCultivateResourceAdjunt> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temTraCultivateResourceAdjunt>>(model.TraCultivateResource);
                //    bmabll.AddFilesForSupplier(fflist, model.ResourceId, ref filenames);
                //}
                //model.TraCultivateResource = filenames;

                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "success", content = "添加成功！" });
            }
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 培训资料维护编辑

        /// <summary>
        /// 培训资料维护编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns> 
        public ActionResult EditTraCultivateResource(TraCultivateResourceModel model)
        {
            model.CompanyId = Auxiliary.CompanyID();//公司id

            model.CreateDepartmentId = Auxiliary.DepartmentId();//新增机构id

            model.CreateUserId = Auxiliary.UserID();//新增负责人id

            model.CreateTime = DateTime.Now;//新增时间

            TraCultivateResourceModel beforeModel = bll.GetModelByID(model.ResourceId);

            int result = bll.UpdateTraCultivateResource(model);

            if (result > 0)
            {
                //string filenames = string.Empty;
                //if (!string.IsNullOrEmpty(model.TraCultivateResource))
                //{
                //    List<temTraCultivateResourceAdjunt> fflist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<temTraCultivateResourceAdjunt>>(model.TraCultivateResource);
                //    bmabll.AddFilesForSupplier(fflist, model.ResourceId, ref filenames);
                //}
                //model.TraCultivateResource = filenames;

                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, model);
                return Json(new { flag = "success", content = "编辑成功！" });
            }
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, model);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 培训资料维护作废

        /// <summary>
        /// 培训资料维护作废
        /// </summary>
        /// <param name="state"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult DelTraCultivateResource(string Id)
        {

            model.DelUserId = Auxiliary.UserID();//作废负责人id

            model.DelTime = DateTime.Now;//作废时间

            TraCultivateResourceModel beforeModel = bll.GetModelByID(model.ResourceId);
            int row = 0;

            if (model.State == 0)
            {
                row = bll.ChangeState(Id, 20);
            }
            else if (model.State == 0)
            {
                row = bll.ChangeState(Id, 10);
            }
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success", content = "作废成功！" });
            }
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 更改状态

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitTraCultivateResource(string tId)
        {
            int row = bll.SubmitTraCultivateResource(tId, 1);

            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId, State = 1 });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId, State = 0 });
            return Json(new { flag = "fail" });
        }
        #endregion

        #region 导出数据

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="ResourceName">培训资料名称</param>
        /// <param name="State">状态</param> 
        /// <param name="CreateTime">创建时间</param> 
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string ResourceName, string State, string CreateTime)
        {
            string where = " (TCR.State = 0 OR TCR.State = 1) and TCR.CreateDepartmentId IN (SELECT DepartmentId FROM FUN_GetDepartmentId(" + Auxiliary.DepartmentId() + "))";

            //培训资料名称
            if (!string.IsNullOrEmpty(ResourceName))
            {
                where += string.Format(" And TCR.ResourceName  like '%{0}%' ", ResourceName.Trim());
            }

            //状态
            if (!string.IsNullOrEmpty(State))
            {
                where += string.Format(" And TCR.State ={0}", State.Trim());
            }

            //创建时间
            if (!string.IsNullOrEmpty(CreateTime))
            {
                where += string.Format(" And convert(varchar,TCR.CreateTime,120) like '%{0}%'", CreateTime.Trim());
            }

            System.Data.DataTable dt = bll.ExportDataTable(where);
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);

            return Json(new { flag = "success", guid = url });
        }
        #endregion

        #endregion
    }
}
