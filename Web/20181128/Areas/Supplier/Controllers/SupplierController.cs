//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-06    1.0        MH         新建               
//-------------------------------------------------------------------------
#region 参考
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using SRM.BLL.Supplier;
using SRM.Model.Supplier;
using SRM.Web.Controllers;
using Newtonsoft.Json.Converters;
#endregion
/*********************************
 * 类名：SupplierController
 * 功能描述：供应商 控制器 
 * ******************************/
namespace SRM.Web.Areas.Supplier.Controllers
{
    public class SupplierController : Controller
    {
        SupplierBLL bll = new SupplierBLL();
        SupplierCyclBLL scbll = new SupplierCyclBLL();
        /// <summary>
        /// 供应商主页面
        /// </summary>
        public ActionResult Index()
        {
            ViewBag.depid = Auxiliary.DepartmentId();
            return View();
        }
        /// <summary>
        /// 供应商列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="name">供应商名称</param>
        /// <param name="type1">供应商类型</param>
        /// <param name="type2">企业类型</param>
        /// <param name="state">状态</param>
        public ActionResult List(int index, int size, string name, int type1, int type2, int state)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" 1=1 ");

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And s.SupplierName like '%" + name.Trim() + "%' ");
            }

            if (type1 != -1)
            {
                sb.Append(string.Format(" And s.SupplierType={0}  ", type1));
            }

            if (type2 != -1)
            {
                sb.Append(string.Format(" And s.EnterpriseType={0}  ", type2));
            }

            if (state != -1)
            {
                sb.Append(string.Format(" And s.State={0}  ", state));
            }
            sb.Append(" And s.State!=10");

            List<SupplierModel> list = bll.SupplierPageList(index, size, sb.ToString(), Auxiliary.CompanyID().ToString(), Auxiliary.DepartmentId().ToString());

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        ///<summary>供应商数量</summary>
        /// <param name="name">供应商名称</param>
        /// <param name="type1">供应商类型</param>
        /// <param name="type2">企业类型</param>
        /// <param name="state">状态</param>
        public int Count(string name, int type1, int type2, int state)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" 1=1 ");

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And s.SupplierName like '%" + name.Trim() + "%' ");
            }

            if (type1 != -1)
            {
                sb.Append(string.Format(" And s.SupplierType={0}  ", type1));
            }

            if (type2 != -1)
            {
                sb.Append(string.Format(" And s.EnterpriseType={0}  ", type2));
            }

            if (state != -1)
            {
                sb.Append(string.Format(" And s.State={0}  ", state));
            }

            sb.Append(" And s.State!=10");

            return bll.SupplierPageCount(sb.ToString(), Auxiliary.CompanyID().ToString(), Auxiliary.DepartmentId().ToString());
        }

        /// <summary>
        /// 获取系统字典表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DictList()
        {
            return Json(bll.GetDictList(Auxiliary.CompanyID()));
        }

        /// <summary>
        /// 供应商添加页面
        /// </summary>
        [Operate(Name = OperateEnum.Add)]
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// 供应商添加功能
        /// </summary>
        public ActionResult AddInfo(SupplierModel model)
        {
            //  model.PapersType = model.PapersTypes;
            model.SupplierNumber = Auxiliary.CurCompanyAutoNum("SUP");
            model.OtherNumber = model.OtherNumber ?? string.Empty;
            model.PapersNumber = model.PapersNumber ?? string.Empty;
            model.ContactsPosition = model.ContactsPosition ?? string.Empty;
            model.ContactsPhone = model.ContactsPhone ?? string.Empty;
            model.InformationUser = model.InformationUser ?? string.Empty;
            model.InformationType = model.InformationType ?? string.Empty;
            model.InformationTime = model.InformationTime ?? string.Empty;
            //  model.EmployType = 0;
            model.State = 0;
            model.CreateDepartmentId = Auxiliary.DepartmentId();
            model.CreateTime = DateTime.Now;
            model.CreateUserId = Auxiliary.UserID();
            model.DelTime = DateTime.Now;
            model.DelUserId = 0;
            model.CompanyId = Auxiliary.CompanyID();


            SupplierDepartmentModel models = new SupplierDepartmentModel();
            models.DepartmentId = model.CreateDepartmentId;

            models.Type = 1;
            models.State = 1;

            if (bll.IsSupplierName(model.SupplierName.Trim(), model.SupplierId) != 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            if (bll.AddSupplier(model, models) > 0)
            {
                Auxiliary.SupplierLog<SupplierModel>(OperateEnum.Add, ResultEnum.Sucess, model);
                return Json(new { flag = "ok" });
            }
            else
            {
                Auxiliary.SupplierLog<SupplierModel>(OperateEnum.Add, ResultEnum.Fail, model);
                return Json(new { flag = "fail" });
            }
        }
        /// <summary>
        /// 供应商编辑页面
        /// </summary>
        /// <param name="ids">供应商主键</param>
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(string ids)
        {
            SupplierModel model = bll.GetModelByID(ids);

            return View(model);
        }
        /// <summary>
        /// 供应商编辑功能
        /// </summary>
        public ActionResult EditInfo(SupplierModel model)
        {
            model.OtherNumber = model.OtherNumber ?? string.Empty;
            model.PapersNumber = model.PapersNumber ?? string.Empty;
            model.ContactsPosition = model.ContactsPosition ?? string.Empty;
            model.ContactsPhone = model.ContactsPhone ?? string.Empty;
            model.InformationUser = model.InformationUser ?? string.Empty;
            model.InformationType = model.InformationType ?? string.Empty;
            model.InformationTime = model.InformationTime ?? string.Empty;
            model.State = 0;

            model.CreateTime = DateTime.Now;

            SupplierModel oldModel = bll.GetModelByID(model.SupplierId.ToString());

            if (bll.IsSupplierName(model.SupplierName.Trim(), model.SupplierId) != 0)
            {
                Auxiliary.Log(OperateEnum.Add, ResultEnum.Exist, model);
                return Json(new { flag = "exist" });
            }

            if (bll.UpdateSupplier(model) > 0)
            {   //补齐原有信息
                string[] tems = model.TemSelectData.Split(',');

                if (tems.Length == 3)
                {
                    oldModel.SupplierTypeName = tems[0];
                    oldModel.EnterpriseTypeName = tems[1];
                    oldModel.PapersTypeName = tems[2];
                }

                Auxiliary.SupplierLog<SupplierModel>(OperateEnum.Edit, ResultEnum.Sucess, oldModel, model);
                return Json(new { flag = "ok" });
            }
            else
            {
                Auxiliary.SupplierLog<SupplierModel>(OperateEnum.Edit, ResultEnum.Fail, oldModel, model);
                return Json(new { flag = "fail" });
            }
        }

        /// <summary>
        /// 作废数据
        /// </summary>
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult Delete(string ids)
        {
            // int state= bll.StateInfo(Auxiliary.CompanyID().ToString(), Auxiliary.DepartmentId().ToString());

            int res1 = bll.IsUseSuppInfo(ids, 1);
            int res2 = bll.IsUseSuppInfo(ids, 2);

            SupplierModel oldModel = bll.GetModelByID(ids);

            if (res1 > 0 || res2 > 0)
            {
                Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Depend, new { Type = "作废", Id = ids, Name = oldModel.SupplierName, Number = oldModel.SupplierNumber });
                return Json(new { flag = "use" });
            }


            if (res2 == 0 && res1 == 0)
            {

                if (bll.DelInfo(ids, Auxiliary.UserID().ToString(), Auxiliary.DepartmentId().ToString(), Auxiliary.CompanyID().ToString()) > 0)
                {
                    Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Sucess, new { Type = "作废", Id = ids, Name = oldModel.SupplierName, Code = oldModel.SupplierNumber });
                    return Json(new { flag = "ok" });
                }
            }
            Auxiliary.SupplierCustomLog(OperateEnum.Invalid, ResultEnum.Fail, new { Type = "作废", Id = ids, Name = oldModel.SupplierName, Number = oldModel.SupplierNumber });
            return Json(new { flag = "fail" });
        }

        /// <summary>
        /// 维护页面
        /// </summary>
        [Operate(Name = OperateEnum.Maintain)]
        public ActionResult Maintain(string ids)
        {
            SupplierModel model = bll.GetModelByID(ids);

            return View(model);
        }
        /// <summary>
        /// 机构列表
        /// </summary>
        public ActionResult DepList(string ids)
        {
            return Json(bll.DepList(ids));
        }
        /// <summary>
        /// 作废数据
        /// </summary>
        public ActionResult MaintainDep(string ids)
        {

            int i = bll.MaintainDep(ids, 0);

            if (i > 0)
            {
                Auxiliary.SupplierCustomLog(OperateEnum.Maintain, ResultEnum.Sucess, new { Type = "作废", Name = "供应商机构", Id = ids });

                return Json(new { flag = "ok" });
            }
            Auxiliary.SupplierCustomLog(OperateEnum.Maintain, ResultEnum.Fail, new { Type = "作废", Name = "供应商机构", Id = ids });
            return Json(new { flag = "fail" });
        }

        #region 提交按钮逻辑

        /// <summary>
        /// 提交按钮逻辑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult Submitsupplier(int tId)
        {
            int row = bll.Submitsupplier(tId);
            if (row > 0)
            {
                Auxiliary.Log(OperateEnum.Submit, ResultEnum.Sucess, new { Detail = "更改状态", Id = tId });
                return Json(new { flag = "success", content = "提交成功！" });
            }

            Auxiliary.Log(OperateEnum.Submit, ResultEnum.Fail, new { Detail = "更改状态", Id = tId });
            return Json(new { flag = "fail", content = "提交失败！" });
        }
        #endregion

        /// <summary>
        /// 拉黑
        /// </summary>
        /// <param name="tId">主键ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Blacklisted)]
        public ActionResult BlacklistedState(string tId)
        {

            SupplierModel model = bll.GetModelByID(tId);

            // 拉黑(更改状态)
            int row = bll.BlacklistedState(tId);

            // 运输供应商
            SupplierTransportModel stModel = bll.TransportIdList(tId);

            if (stModel != null)
            {
                //将对应的运输供应商的状态从备选、合格、待运作和运作变为拉黑状态
                new SupplierTransportBLL().BlackTransportState(stModel.TransportId);
            }

            // 仓储供应商
            List<SupplierStorageModel> list = bll.StorageIDList(tId);

            if (list != null)
            {
                foreach (var item in list)
                {
                    //将对应的仓储供应商的状态从备选、合格、待运作和运作变为拉黑状态
                    new SupplierStorageBLL().BlackStorageState(item.StorageId);
                }
            }

            // 若影响行数>O(修改成功)
            if (row > 0)
            {

                //将系统用户的状态更新为0
                bll.UpdateUserState(tId);

                //将TraSupperMatching（供应商用户匹配）表的状态变为作废
                bll.UpdateMatchingState(tId);
                 
                if (model.SupplierType == 1)
                {
                    //结束对应供应商的生命周期（SupplierCycl表【供应商周期】）  
                    scbll.UdateEndTypes(model.SupplierId, 1, 2);
                }

                else if (model.SupplierType == 0)
                {
                    //结束对应供应商的生命周期（SupplierCycl表【供应商周期】） 
                    scbll.UdateEndTypes(model.SupplierId, 0, 2);
                }

                // 供应商日志
                Auxiliary.SupplierCustomLog(OperateEnum.Blacklisted, ResultEnum.Sucess, new { Type = "拉黑", Id = tId });
                return Json(new { flag = "success" });
            }

            // 供应商日志
            Auxiliary.SupplierCustomLog(OperateEnum.Blacklisted, ResultEnum.Fail, new { Type = "拉黑", Id = tId });
            return Json(new { flag = "fail" });
        }
        /// <summary>
        /// 重新启用
        /// </summary>
        public ActionResult Resetmain(string ids)
        {
            int i = bll.MaintainDep(ids, 1);

            if (i > 0)
            {
                Auxiliary.SupplierCustomLog(OperateEnum.Maintain, ResultEnum.Sucess, new { Type = "启用", Name = "供应商机构", Id = ids });
                return Json(new { flag = "ok" });
            }
            Auxiliary.SupplierCustomLog(OperateEnum.Maintain, ResultEnum.Fail, new { Type = "启用", Name = "供应商机构", Id = ids });
            return Json(new { flag = "fail" });
        }

        public ActionResult AddDep(string ids, string pkid)
        {
            int i = bll.IsHasDep(pkid, ids);

            if (i > 0)
            {
                Auxiliary.SupplierCustomLog(OperateEnum.Maintain, ResultEnum.Exist, new { Type = "新增机构", Name = "供应商机构", Id = ids });
                return Json(new { flag = "exsit" });
            }
            else
            {
                int iss = bll.AddDepInfo(pkid, ids);

                if (iss > 0)
                {
                    Auxiliary.SupplierCustomLog(OperateEnum.Maintain, ResultEnum.Sucess, new { Type = "新增机构", Name = "供应商机构", Id = ids });
                    return Json(new { flag = "ok" });
                }
            }
            Auxiliary.SupplierCustomLog(OperateEnum.Maintain, ResultEnum.Fail, new { Type = "新增机构", Name = "供应商机构", Id = ids });
            return Json(new { flag = "fail" });
        }
        /// <summary>
        /// 查看页面
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.View)]
        public ActionResult Views(string ids)
        {
            SupplierModel model = bll.GetModelByID(ids);

            return View(model);
        }

        public ActionResult Infos(string ids, int type)
        {
            SupplierModel model = bll.GetModelByID(ids);

            int res = bll.IsHasSupp(type, model.SupplierId, Auxiliary.DepartmentId());
            if (res > 0)
            {
                model.Flag = "exist";
            }
            else
            {
                model.Flag = "ok";
            }
            return Json(model);
        }
        /// <summary>
        /// 供应商查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Select(string url, string type)
        {
            ViewBag.url = url;
            ViewBag.type = type;
            return View();
        }
        /// <summary>
        /// 供应商列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="name">供应商名称</param>
        /// <param name="type1">供应商类型</param>
        /// <param name="type2">企业类型</param>
        /// <param name="state">状态</param>
        public ActionResult SelList(int index, int size, string name, string type)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" 1=1 ");

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And s.SupplierName like '%" + name.Trim() + "%' ");
            }

            string types = "1";
            if (string.IsNullOrEmpty(type))
            {
                types = "1";
            }
            else
            {
                types = type;
            }
            sb.Append(" And s.SupplierType=" + types + " ");
            sb.Append(" And s.State=1 ");

            if (types == "2")
            {
                sb.Append(" AND s.SupplierId NOT IN(SELECT SupplierId FROM SupplierTransport WHERE State = 1) ");
            }

            List<SupplierModel> list = bll.SupplierPageList(index, size, sb.ToString(), Auxiliary.CompanyID().ToString(), Auxiliary.DepartmentId().ToString());

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }

        ///<summary>供应商数量</summary>
        /// <param name="name">供应商名称</param>
        /// <param name="type1">供应商类型</param>
        /// <param name="type2">企业类型</param>
        /// <param name="state">状态</param>
        public int SelCount(string name, string type)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" 1=1 ");

            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" And s.SupplierName like '%" + name.Trim() + "%' ");
            }

            string types = "1";
            if (string.IsNullOrEmpty(type))
            {
                types = "1";
            }
            else
            {
                types = type;
            }
            sb.Append(" And s.SupplierType=" + types + " ");
            sb.Append(" And s.State=1 ");

            if (types == "2")
            {
                sb.Append(" AND s.SupplierId NOT IN(SELECT SupplierId FROM SupplierTransport WHERE State = 1) ");
            }

            return bll.SupplierPageCount(sb.ToString(), Auxiliary.CompanyID().ToString(), Auxiliary.DepartmentId().ToString());
        }
    }
}
