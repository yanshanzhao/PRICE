//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-30    1.0        FJK        新建               
//-------------------------------------------------------------------------
#region 参考
using System.Collections.Generic;
using System.Web.Mvc;

using Model.Basis;
using Web.Controllers;
using BLL.Basis;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：BasisKeyNodeController
 * 功能描述：关键节点 控制器 
 * ******************************/

namespace Web.Areas.Basis.Controllers
{
    public class BasisKeyNodeController : Controller
    {
        //
        // GET: /Basis/BasisKeyNode/

        // 关键节点BLL
        BasisKeyNodeBLL bll = new BasisKeyNodeBLL(); 
         
        #region 页面

        /// <summary>
        /// Index
        /// </summary> 
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add
        /// </summary> 
        [Operate(Name=OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Edit
        /// </summary> 
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int tId)
        {
            // 获取数据
            BasisKeyNodeModel model = bll.GetModelByID(tId); 

            return View(model);
        }

        /// <summary>
        /// View
        /// </summary>
        public ActionResult View(int tId)
        { 
            // 获取数据
             BasisKeyNodeModel model = bll.GetModelByID(tId);

            return View(model);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 数据集
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="nodeName">关键点名称</param>
        /// <param name="companyName">公司名称</param>
        /// <returns></returns>
        public ActionResult KeyNodeList(int index, int size, string nodeName,string companyName)
        { 
            // 非作废状态
             string where = " bkn.State != 2"; 

            // 关键点名称
            if (!string.IsNullOrEmpty(nodeName))
            {
                where += string.Format(" And bkn.NodeName like '%{0}%'", nodeName.Trim()); 
            }

            // 公司名称
            if (!string.IsNullOrEmpty(companyName))
            {
                where += string.Format(" And com.CompanyName like '%{0}%'", companyName.Trim());
            }

            // 关键节点List
            List<BasisKeyNodeModel> list = bll.BasisKeyNodeList(index, size, where);

            // DateTime类型字段转换
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss"; 
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        /// <summary>
        /// 数据记录数
        /// </summary>
        /// <param name="nodeName">关键点名称</param>
        /// <param name="companyName">公司名称</param>
        /// <returns></returns>
        public int KeyNodeCount(string nodeName, string companyName)
        {
            // 非作废状态
            string tNodeName = " State != 2";

            // 关键点名称
            if (!string.IsNullOrEmpty(nodeName))
            {
                tNodeName += string.Format(" And NodeName like '%{0}%' ", nodeName.Trim());
            }

            // 公司名称
            string tCompanyName = " 1=1";  
            if (!string.IsNullOrEmpty(companyName))
            {
                tCompanyName  += string.Format(" And CompanyName  like '%{0}%' ", companyName.Trim());
            }

            return bll.KeyNodeCount(tNodeName,tCompanyName); 
        }

        /// <summary>
        /// 关键节点新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddKeyNode(BasisKeyNodeModel tModel)
        {
            // 默认状态为初始状态0
            tModel.State = 0;

            // 新增
            if (bll.AddKeyNode(tModel) > 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Sucess, tModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnum.Fail, tModel); 
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 关键节点编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditKeyNode(BasisKeyNodeModel tModel)
        {
            // 编辑之前Model
            BasisKeyNodeModel beforeModel = bll.GetModelByID(tModel.Id);

            // 编辑
            int result = bll.EditKeyNode(tModel);

            // 若影响行数>O(修改成功)
            if (result > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Edit, ResultEnum.Sucess, beforeModel, tModel); 
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnum.Fail, beforeModel, tModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult EditState(int tId)
        {
            // 提交(更改状态)
            int row = bll.ChangeState(tId, 1);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "提交", Id = tId, State = 1 });
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "提交", Id = tId, State = 0 });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 关键节点作废
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult DeleteKeyNode(int tId)
        {
            // 作废之前Model
            BasisKeyNodeModel beforeModel = bll.GetModelByID(tId);

            // 作废(更改状态)
            int row = bll.ChangeState(tId, 2);

            // 若影响行数>O(修改成功)
            if (row > 0)
            {
                // 系统日志
                Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Sucess, beforeModel);
                return Json(new { flag = "success" });
            }

            // 系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnum.Fail, beforeModel);
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="nodeName">关键点名称</param>
        /// <param name="companyName">公司名称</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Export)]
        public ActionResult Export(string nodeName, string companyName)
        {
            // 非作废状态
            string where = " bkn.State != 2";

            // 关键点名称
            if (!string.IsNullOrEmpty(nodeName))
            {
                where += string.Format(" And bkn.NodeName like '%{0}%'", nodeName.Trim());
            }

            // 公司名称
            if (!string.IsNullOrEmpty(companyName))
            {
                where += string.Format(" And com.CompanyName like '%{0}%'", companyName.Trim());
            }

            // DataTable
            System.Data.DataTable dt = bll.ExportDataTable(where);

            // Excel
            Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);
             
            // 系统日志
            Auxiliary.Log(OperateEnum.Export, ResultEnum.Fail, new{Detail = "导出",UserId = Auxiliary.UserID(),ExportTime = System.DateTime.Now}); 
            return Json(new { flag = "success", guid = url });
        }
        #endregion 
    }
}
