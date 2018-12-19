//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-06-19    1.0        HDS         新建               
//-------------------------------------------------------------------------
#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using SRM.BLL.Tra;
using SRM.Model.Tra;
using SRM.Web.Controllers;
using Newtonsoft.Json.Converters;
using SRM.Model.Basis;
using System.Data;
#endregion
/*********************************
 * 类名：TraDistributorController
 * 功能描述：配送员 控制器 
 * ******************************/


namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraDistributorController : Controller
    {
        //
        // GET: /Tra/TraDistributor/
        #region 成员变量
        TraDistributorBLL bll = new TraDistributorBLL();// 业务逻辑变量
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        #region  查询条件列表
        /// <summary>
        /// 查询条件列表 （从系统字典中获取）
        /// </summary>
        /// <returns></returns>
        public ActionResult DictList()
        {
            string DictionaryType = "3";//供应商范围
            return Json(bll.GetDictLists(Auxiliary.CompanyID(), DictionaryType));
        }
        #endregion

        #region 配送员数量
        /// <summary>
        /// 配送员数量
        /// </summary>
        /// <param name="name">运输供应商名称</param>
        /// <param name="state">状态</param>
        /// <param name="createstarttime">创建时间的开始</param>
        /// <param name="createendtime">创建时间的结束</param>
        /// <param name="submittarttime">运作开始时间的开始</param>
        /// <param name="submitendtime">运作开始时间的结束</param>
        /// <param name="deltarttime">运作结束时间的开始</param>
        /// <param name="delendtime">运作结束时间的结束</param>
        /// <returns></returns>
        public int RowCount(string name,  int state, string createstarttime, string createendtime, string submittarttime, string submitendtime, string deltarttime, string delendtime)
        {
            StringBuilder strB = new StringBuilder();//拼接查询SQL语句
            strB.Append(" traDis.DistributorState>0 ");//非作废信息
            strB.Append(" and traDis.CreateDepartmentId=" + Auxiliary.DepartmentId());//查询本机构信息
            strB.Append(" and  traDis.CompanyId=" + Auxiliary.CompanyID());//查询本公司信息
            DateTime endTime;
            #region 查询条件         
            if (!string.IsNullOrEmpty(name))//运输供应商名称
            {
                strB.Append(" And supp.SupplierName like '%" + name.Trim() + "%' ");
            }
            if (state != -1)//信息状态
            {
                strB.Append(" And traDis.DistributorState=" + state);
            }
            if (!string.IsNullOrEmpty(createstarttime))//创建时间的开始
            {
                strB.Append(" And traDis.CreateTime >='" + createstarttime + "' ");
            }
            if (!string.IsNullOrEmpty(createendtime))//创建时间的结束
            {
                  endTime = Convert.ToDateTime(createendtime);
                strB.Append(" And traDis.CreateTime <'" + endTime.AddDays(1).Date.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(submittarttime))//运作开始时间的开始
            {
                strB.Append(" And traDis.SubmitTime >='" + submittarttime + "' ");
            }
            if (!string.IsNullOrEmpty(submitendtime))//运作开始时间的结束
            {
                  endTime = Convert.ToDateTime(submitendtime);
                strB.Append(" And traDis.SubmitTime <'" + endTime.AddDays(1).Date.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(deltarttime))//运作结束时间的开始
            {
                strB.Append(" And traDis.DelTime >='" + deltarttime + "' ");
            }
            if (!string.IsNullOrEmpty(delendtime))//运作结束时间的结束
            {
                  endTime = Convert.ToDateTime(delendtime);
                strB.Append(" And traDis.DelTime <'" + endTime.AddDays(1).Date.ToString() + "'");
            }
            #endregion
            return bll.TraDistributorPageCount(strB.ToString());
        }
        #endregion

        #region 配送员列表数据
        /// <summary>
        /// 配送员列表数据
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="name">运输供应商名称</param>
        /// <param name="state">状态</param>
        /// <param name="createstarttime">创建时间的开始</param>
        /// <param name="createendtime">创建时间的结束</param>
        /// <param name="submittarttime">运作开始时间的开始</param>
        /// <param name="submitendtime">运作开始时间的结束</param>
        /// <param name="deltarttime">运作结束时间的开始</param>
        /// <param name="delendtime">运作结束时间的结束</param>
        /// <returns></returns>
        public ActionResult List(int index, int size, string name, int state, string createstarttime, string createendtime, string submittarttime, string submitendtime, string deltarttime, string delendtime)
        {
            StringBuilder strB = new StringBuilder();//拼接查询SQL语句 
            strB.Append(" traDis.DistributorState>0 ");//非作废信息
            strB.Append(" and traDis.CreateDepartmentId=" + Auxiliary.DepartmentId());//查询本机构信息
            #region 查询条件        
            DateTime endTime;
            if (!string.IsNullOrEmpty(name))//运输供应商名称
            {
                strB.Append(" And supp.SupplierName like '%" + name.Trim() + "%' ");
            }
            if (state != -1)//信息状态
            {
                strB.Append(" And traDis.DistributorState=" + state);
            }
            if (!string.IsNullOrEmpty(createstarttime))//创建时间的开始
            {
                strB.Append(" And traDis.CreateTime >='" + createstarttime + "' ");
            }
            if (!string.IsNullOrEmpty(createendtime))//创建时间的结束
            {
                endTime = Convert.ToDateTime(createendtime);
                strB.Append(" And traDis.CreateTime <'" + endTime.AddDays(1).Date.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(submittarttime))//运作开始时间的开始
            {
                strB.Append(" And traDis.SubmitTime >='" + submittarttime + "' ");
            }
            if (!string.IsNullOrEmpty(submitendtime))//运作开始时间的结束
            {
                endTime = Convert.ToDateTime(submitendtime);
                strB.Append(" And traDis.SubmitTime <'" + endTime.AddDays(1).Date.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(deltarttime))//运作结束时间的开始
            {
                strB.Append(" And traDis.DelTime >='" + deltarttime + "' ");
            }
            if (!string.IsNullOrEmpty(delendtime))//运作结束时间的结束
            {
                endTime = Convert.ToDateTime(delendtime);
                strB.Append(" And traDis.DelTime <'" + endTime.AddDays(1).Date.ToString() + "'");
            }
            #endregion
            List<TraDistributorModel> listModel = bll.TraDistributorPageList(index, size, strB.ToString());
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();//定义显示时间样式
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(listModel, Newtonsoft.Json.Formatting.Indented, timeFormat));
        }
        #endregion

        #region 新增页面 
        [Operate(Name = OperateEnum.Add)] // 新增按钮权限控制
        public ActionResult Add()
       {
            return View();
        }

        public ActionResult AddDistributor(TraDistributorModel addModel)
        {
            ResultEnum ResultEnums = ResultEnum.Sucess;
            string flagStr = "success";
            string contentStr = "添加成功";
            int rowCouts = bll.IsDistributorNumber(Auxiliary.DepartmentId(), addModel.DistributorNumber, "");
            if (rowCouts == 0)
            {
                // 配送员ID(系统自动TDN开头生成8位流水号)       
                addModel.DistributorNumber = Auxiliary.CurCompanyAutoNum("TDN");
                if (string.IsNullOrEmpty(addModel.DistributorSex))//性别
                {
                    addModel.DistributorSex = "  ";
                }
                if (string.IsNullOrEmpty(addModel.Grade))//当前星级
                {
                    addModel.Grade = "  ";
                }
                if (string.IsNullOrEmpty(addModel.Unloading))//卸货地址
                {
                    addModel.Unloading = "  ";
                }
                if (string.IsNullOrEmpty(addModel.Route))//路由地点
                {
                    addModel.Route = "  ";
                }
                if (string.IsNullOrEmpty(addModel.ExtractType))//提货方式
                {
                    addModel.ExtractType = "  ";
                }
                if (string.IsNullOrEmpty(addModel.StoreIssue))//配送发料
                {
                    addModel.StoreIssue = "  ";
                }
                if (string.IsNullOrEmpty(addModel.DistributorRemark))//备注
                {
                    addModel.DistributorRemark = "  ";
                }

                // 公司ID
                addModel.CompanyId = Auxiliary.CompanyID();
                // 创建机构ID
                addModel.CreateDepartmentId = Auxiliary.DepartmentId();
                // 创建人ID
                addModel.CreateUserId = Auxiliary.UserID();
                //创建时间
                addModel.CreateTime = DateTime.Now;
                // 状态默认初始状态1
                addModel.DistributorState = 1;
                //所属部门
                addModel.DepartmentId = Auxiliary.DepartmentId();
                //作废时间
                addModel.DelTime = " ";
                //录入方式：0-正常录入;1-系统导入
                addModel.EntryState = 0; 
                int RowCounts = bll.AddTraDistributor(addModel); 
               
                if (RowCounts <= 0)
                { 
                    //添加失败
                    ResultEnums = ResultEnum.Fail;
                    flagStr = "fail";
                    contentStr = "添加失败";
                }
            }else
            {
                //添加失败
                ResultEnums = ResultEnum.Fail;
                flagStr = "fail";
                contentStr = "本部门内存在配送员ID为"+ addModel.DistributorNumber+"的信息";
            }
            //记录系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnums, addModel);
            return Json(new { flag = flagStr, content= contentStr });

        }
        #endregion

        #region 编辑
        [Operate(Name = OperateEnum.Edit)]
        public ActionResult Edit(int userId)
        {
            // 获取数据
            TraDistributorModel model = bll.GetModelById(userId);
            return View(model);
        }

        public ActionResult EditState(int userId)
        {
            // 获取数据
            TraDistributorModel model = bll.GetModelById(userId);
            return View(model);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        public ActionResult EditDistributor(TraDistributorModel editModel)
        {
            
            ResultEnum ResultEnums = ResultEnum.Sucess;
            string flagStr = "success";
            string contentStr = "修改成功";
            int rowCouts = bll.IsDistributorNumber(Auxiliary.DepartmentId(), editModel.DistributorNumber, " AND DistributorUserId <> "+ editModel.DistributorUserId);
            if (rowCouts==0)
            {
                if (string.IsNullOrEmpty(editModel.DistributorSex))//性别
                {
                    editModel.DistributorSex = "  ";
                }
                if (string.IsNullOrEmpty(editModel.Grade))//当前星级
                {
                    editModel.Grade = "  ";
                }
                if (string.IsNullOrEmpty(editModel.Unloading))//卸货地址
                {
                    editModel.Unloading = "  ";
                }
                if (string.IsNullOrEmpty(editModel.Route))//路由地点
                {
                    editModel.Route = "  ";
                }
                if (string.IsNullOrEmpty(editModel.ExtractType))//提货方式
                {
                    editModel.ExtractType = "  ";
                }
                if (string.IsNullOrEmpty(editModel.StoreIssue))//配送发料
                {
                    editModel.StoreIssue = "  ";
                }
                if (string.IsNullOrEmpty(editModel.DistributorRemark))//备注
                {
                    editModel.DistributorRemark = "  ";
                } 
                int RowCounts = bll.UpdateTraDistributor(editModel); 
                if (RowCounts <=0)
                {
                    //修改成功
                    ResultEnums = ResultEnum.Fail;
                    flagStr = "fail";
                    contentStr = "修改失败";
                }
            }
            else
            {
                //修改失败
                ResultEnums = ResultEnum.Fail;
                flagStr = "fail";
                contentStr = "配送人员ID重复！";
            }
            //记录系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnums, editModel);
            return Json(new { flag = flagStr, content= contentStr });
        }
        #endregion

        #region 提交状态
        [Operate(Name = OperateEnum.Submit)]
        public ActionResult SubmitState(int userId)
        {
            //TraDistributorModel submitModel = bll.GetModelById(userId);
            //submitModel.DistributorState = 10;//状态变为启用状态
            //submitModel.SubmitTime = DateTime.Now.ToString();//启用时间
            //if(submitModel.SubmitTime ==null)
            //    submitModel.SubmitTime = DateTime.Now.ToString();//启用时间
            int RowCounts = bll.SubmitState(userId);
            ResultEnum ResultEnums;
            string flagStr = string.Empty;
            string StateStr = "提交";
            string contentStr = string.Empty;
            if (RowCounts > 0)
            {
                //作废成功
                ResultEnums = ResultEnum.Sucess;
                flagStr = "success"; 
                contentStr = "提交成功";
            }
            else
            {
                //作废失败
                ResultEnums = ResultEnum.Fail;
                flagStr = "noauth";
                contentStr = "提交失败";
            }
            //记录系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnums, new { Detail = "提交", Id = userId, State = StateStr });
            return Json(new { flag = flagStr, content= contentStr });
        }
        #endregion

        #region 作废
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult Del(int userId,int state)
        {
            TraDistributorModel Delmodel = bll.GetModelById(userId);
            if (state == 1)
                state = 0;
            else
                state = 20; 
            int RowCounts = bll.DelTraDistributor(Auxiliary.UserID(), DateTime.Now.ToString(), state, userId, DateTime.Now.ToString());
            ResultEnum ResultEnums;
            string flagStr = string.Empty;
            string contentStr = string.Empty;
            if (RowCounts > 0)
            {
                //作废成功
                ResultEnums = ResultEnum.Sucess;
                flagStr = "success";
                contentStr = "作废成功";
            }
            else
            {
                //作废失败
                ResultEnums = ResultEnum.Fail;
                flagStr = "noauth";
                contentStr = "作废失败";
            }
            //记录系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnums, Delmodel);
            return Json(new { flag = flagStr, content= contentStr });
        }

        #endregion

        #region 查看
        [Operate(Name = OperateEnum.View)]
        public ActionResult View(int userId)
        {
            // 获取数据
            TraDistributorModel model = bll.GetModelById(userId);
            return View(model);
        }

        #endregion

        #region 运输供应商线路信息
        public ActionResult TraSupplier(string url)
        {
            ViewBag.url = url;
            return View();
        }
        /// <summary>
        /// 分页列表 运输供应商线路信息
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="supplierName">运输供应商名称</param> 
        /// <returns></returns>
        public ActionResult TraSupplierList(int index, int size, string supplierName)
        {
            // 查询本机构的配送环节的供应商
            string where = "    AND a.DepartmentId=" + Auxiliary.DepartmentId();
            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" AND b.SupplierName LIKE '%{0}%'", supplierName.Trim());
            }
            // 运输供应商信息List
            List<SupplierLine> list = bll.TraSupplierList(index, size, where);
            return Json(list);
        }

        /// <summary>
        /// 分页总数 运输供应商信息
        /// </summary> 
        /// <param name="supplierName">运输供应商名称</param> 
        /// <returns></returns>
        public int TraSupplierCount(string supplierName)
        {
            // 查询本机构的配送环节的供应商
            string where = "    AND a.DepartmentId=" + Auxiliary.DepartmentId();
            // 供应商名称
            if (!string.IsNullOrEmpty(supplierName))
            {
                where += string.Format(" AND b.SupplierName LIKE '%{0}%'", supplierName.Trim());
            } 
            return bll.TraSupplierListCount(where);
        }
        #endregion

        #region 导出
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="name">运输供应商名称</param>
        /// <param name="state">状态</param>
        /// <param name="createstarttime">创建时间的开始</param>
        /// <param name="createendtime">创建时间的结束</param>
        /// <param name="submittarttime">运作开始时间的开始</param>
        /// <param name="submitendtime">运作开始时间的结束</param>
        /// <param name="deltarttime">运作结束时间的开始</param>
        /// <param name="delendtime">运作结束时间的结束</param>
        /// <returns></returns>
        public ActionResult Export(string name, int state, string createstarttime, string createendtime, string submittarttime, string submitendtime, string deltarttime, string delendtime)
        {

            StringBuilder strB = new StringBuilder();//拼接查询SQL语句 
            strB.Append(" traDis.DistributorState>0 ");//非作废信息
            strB.Append(" and traDis.CreateDepartmentId=" + Auxiliary.DepartmentId());//查询本机构信息
            #region 查询条件        
            DateTime endTime;
            if (!string.IsNullOrEmpty(name))//运输供应商名称
            {
                strB.Append(" And supp.SupplierName like '%" + name.Trim() + "%' ");
            }
            if (state != -1)//信息状态
            {
                strB.Append(" And traDis.DistributorState=" + state);
            }
            if (!string.IsNullOrEmpty(createstarttime))//创建时间的开始
            {
                strB.Append(" And traDis.CreateTime >='" + createstarttime + "' ");
            }
            if (!string.IsNullOrEmpty(createendtime))//创建时间的结束
            {
                endTime = Convert.ToDateTime(createendtime);
                strB.Append(" And traDis.CreateTime <'" + endTime.AddDays(1).Date.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(submittarttime))//运作开始时间的开始
            {
                strB.Append(" And traDis.EndTime >='" + submittarttime + "' ");
            }
            if (!string.IsNullOrEmpty(submitendtime))//运作开始时间的结束
            {
                endTime = Convert.ToDateTime(submitendtime);
                strB.Append(" And traDis.EndTime <'" + endTime.AddDays(1).Date.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(deltarttime))//运作结束时间的开始
            {
                strB.Append(" And traDis.DelTime >='" + deltarttime + "' ");
            }
            if (!string.IsNullOrEmpty(delendtime))//运作结束时间的结束
            {
                endTime = Convert.ToDateTime(delendtime);
                strB.Append(" And traDis.DelTime <'" + endTime.AddDays(1).Date.ToString() + "'");
            }
            #endregion
            DataTable dt = bll.ExportDataTable(strB.ToString());//获取导出样式和导出数据
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);
            return Json(new { flag = "ok", guid = url });
        }
        #endregion
    }
}
