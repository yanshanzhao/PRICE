//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 , SRM
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-07-10    1.0        HDS         新建               
//-------------------------------------------------------------------------
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRM.Model.Tra;
using SRM.BLL.Tra;
using System.Text;
using SRM.Web.Controllers;
using Newtonsoft.Json.Converters;
using System.Data;
using SRM.Model.Basis;
using SRM.Model.Supplier;
using SRM.BLL.Basis;
using SRM.BLL.Supplier;
#endregion
/*********************************
 * 类名：TraDistributorAdjustController
 * 功能描述：配送员更换 控制器 
 * ******************************/

namespace SRM.Web.Areas.Tra.Controllers
{
    public class TraDistributorAdjustController : Controller
    {
        //
        // GET: /Tra/TraDistributorAdjust/
        #region 成员变量
        TraDistributorAdjustBLL bll = new TraDistributorAdjustBLL();
        TraDistributorAdjustDetailBLL detailBLL = new TraDistributorAdjustDetailBLL();
        TraDistributorBLL distributorBLL = new TraDistributorBLL();
        #endregion
        #region 首页 
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        #region 查询       
        /// <summary>
        /// 查询配送人员变更行数
        /// </summary>
        /// <param name="starttime">申请开始时间</param>
        /// <param name="endtime">申请结束时间</param>
        /// <param name="suppname">供应商名称</param> 
        /// <param name="state">状态</param>
        /// <param name="substarttime">提交开始时间</param>
        /// <param name="subendtime">提交开始时间</param>
        /// <returns></returns>
        public int RowCount(string starttime, string endtime,string suppname,int state,string substarttime,string subendtime)
        {
            StringBuilder sbStr = new StringBuilder();
            sbStr.Append(" where butorAdjust.CreateDepartmentId=" + Auxiliary.DepartmentId());//只允许查询本机构
            sbStr.Append(" and butorAdjust.UserAdjustState<=20  ");//有效的信息
            if(!string.IsNullOrEmpty(starttime))//申请开始时间
            { 
                sbStr.Append("and butorAdjust.CreateTime>='"+ starttime + "'");
            }
            if (!string.IsNullOrEmpty(endtime))//申请结束时间
            {
                DateTime times = Convert.ToDateTime(endtime).AddDays(1);
                sbStr.Append("and butorAdjust.CreateTime<'" + times + "'");
            }
            if (!string.IsNullOrEmpty(suppname))//供应商名称
            {
                sbStr.Append("and Supp.SupplierName like '%" + suppname + "%'");
            }
            if (!string.IsNullOrEmpty(substarttime))//提交开始时间
            {
                sbStr.Append("and butorAdjust.SubmTime>='" + substarttime + "'");
            }
            if (!string.IsNullOrEmpty(subendtime))//提交结束时间
            {
                DateTime times = Convert.ToDateTime(subendtime).AddDays(1);
                sbStr.Append("and butorAdjust.SubmTime<'" + times.Date + "'");
            } 
            if (state != -1)//状态
            {
                sbStr.Append("and butorAdjust.UserAdjustState='" + state + "'");
            }
            return bll.TraDistributorAdjustPageCount(sbStr.ToString());
        }

        /// <summary>
        /// 查询配送人员变更信息
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="starttime">申请开始时间</param>
        /// <param name="endtime">申请结束时间</param>
        /// <param name="suppname">供应商名称</param> 
        /// <param name="state">状态</param> 
        /// <param name="substarttime">提交开始时间</param>
        /// <param name="subendtime">提交开始时间</param>
        /// <returns></returns>
        public ActionResult list(int index,int size, string starttime, string endtime, string suppname,   int state, string substarttime, string subendtime)
        {
            StringBuilder sbStr = new StringBuilder();
            sbStr.Append(" where butorAdjust.CreateDepartmentId=" + Auxiliary.DepartmentId());//只允许查询本机构
            sbStr.Append(" and butorAdjust.UserAdjustState<=20  ");//有效的信息
            if (!string.IsNullOrEmpty(starttime))//申请开始时间
            {
                sbStr.Append("and butorAdjust.CreateTime>='" + starttime + "'");
            }
            if (!string.IsNullOrEmpty(endtime))//申请结束时间
            {
                DateTime times = Convert.ToDateTime(endtime).AddDays(1);
                sbStr.Append("and butorAdjust.CreateTime<'" + times + "'");
            }
            if (!string.IsNullOrEmpty(suppname))//供应商名称
            {
                sbStr.Append("and Supp.SupplierName like '%" + suppname + "%'");
            }
            if (!string.IsNullOrEmpty(substarttime))//提交开始时间
            {
                sbStr.Append("and butorAdjust.SubmTime>='" + substarttime + "'");
            }
            if (!string.IsNullOrEmpty(subendtime))//提交结束时间
            {
                DateTime times = Convert.ToDateTime(subendtime).AddDays(1);
                sbStr.Append("and butorAdjust.SubmTime<'" + times.Date + "'");
            }
            if (state != -1)//状态
            {
                sbStr.Append("and butorAdjust.UserAdjustState='" + state + "'");
            } 
            List<TraDistributorAdjustModel> listModel = bll.TraDistributorAdjustPageList(index, size, sbStr.ToString());
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();//定义显示时间样式
            timeFormat.DateTimeFormat = "yyyy-MM-dd";
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(listModel, Newtonsoft.Json.Formatting.Indented, timeFormat)); 
        }
        #endregion
        #region 新增
        [Operate(Name = OperateEnum.Add)] // 新增按钮权限控制
        public ActionResult Add()
        { 
            return View();
        }

        /// <summary>
        /// 添加调整信息
        /// </summary>
        /// <param name="userid">调整id</param>
        /// <param name="name">调整姓名</param> 
        /// <param name="number">调整配送人ID</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public ActionResult AddDistributorAdjust(int userid, string name,  string remark)
        {
            ResultEnum ResultEnums ;
            string flagStr;
            string contentStr = "新增失败";
            TraDistributorModel model = distributorBLL.GetModelById(userid);
            TraDistributorAdjustModel adjustModel = new TraDistributorAdjustModel();
            adjustModel.DistributorUserId = userid;//配送人员id
            adjustModel.CreateDepartmentId = Auxiliary.DepartmentId();//新增机构id
            adjustModel.CreateTime = DateTime.Now;//新增时间
            adjustModel.CreateUserId = Auxiliary.UserID();//新增用户id
            adjustModel.UserAdjustRemark = remark;//申请备注
            adjustModel.UserAdjustState = 0;//申请状态
            adjustModel.UserAdjustState = 0;//申请状态
            adjustModel.CompanyId = Auxiliary.CompanyID();//新增系统公司id
            int adjustId = bll.AddDistributorAdjust(adjustModel);
            if (adjustId > 0)
            {
                List<TraDistributorAdjustDetailModel> lisetDetilModel = new List<TraDistributorAdjustDetailModel>();
                //调整姓名
                if (name != "")
                {
                    TraDistributorAdjustDetailModel detilModel = new TraDistributorAdjustDetailModel();
                    detilModel.UserAdjustId = adjustId;//调整表id
                    detilModel.AdjustNumber = "DistributorName";//调整的字段
                    detilModel.BeforeRealityValue = model.DistributorName;//调整前实际内容
                    detilModel.BeforeShowValue = model.DistributorName;//调整前显示内容
                    detilModel.AfterRealityValue = name;// 调整后实际内容
                    detilModel.AfterShowValue = name;//调整后显示内容
                    lisetDetilModel.Add(detilModel);
                }  
                ////调整配送人员ID
                //if (number != "")
                //{
                //    TraDistributorAdjustDetailModel detilModel = new TraDistributorAdjustDetailModel();
                //    detilModel.UserAdjustId = adjustId;//调整表id
                //    detilModel.AdjustNumber = "DistributorNumber";//调整的字段
                //    detilModel.BeforeRealityValue = model.DistributorNumber;//调整前实际内容
                //    detilModel.BeforeShowValue = model.DistributorNumber;//调整前显示内容
                //    detilModel.AfterRealityValue = number;// 调整后实际内容
                //    detilModel.AfterShowValue = number;//调整后显示内容
                //    lisetDetilModel.Add(detilModel);
                //}
                int RowCounts = detailBLL.LisrtAddDistributorAdjustDetail(lisetDetilModel);
                if(RowCounts>0)
                {
                    //添加成功
                    ResultEnums = ResultEnum.Sucess;
                    flagStr = "success";
                    contentStr = "新增成功";
                }
                else
                {
                    //添加失败
                    ResultEnums = ResultEnum.Fail;
                    flagStr = "fail"; 
                }
            }
            else
            {
                //添加失败
                ResultEnums = ResultEnum.Fail; 
                flagStr = "fail"; 
            } 
            //记录系统日志
            Auxiliary.Log(OperateEnum.Add, ResultEnums, adjustModel);
            return Json(new { flag = flagStr, content = contentStr  }); 
        }
        #endregion
        #region 编辑
        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="adjustId">调整id</param>
        /// <param name="userid">配送人员表ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Edit)] // 编辑按钮权限
        public ActionResult Edit(int adjustId, int userid)
        {
            string where = " DistributorUserId  ="+ userid + "  ";
            TraDistributorModel model = distributorBLL.GetModelByWhere(where);// 查询配送人员信息
            TraDistributorAdjustModel adjustModel = bll.GetModelById(adjustId);// 查询调整配送人员信息
            List<TraDistributorAdjustDetailModel> listModel = detailBLL.ListModel(adjustId); // 查询调整配送人员详细信息
            foreach (TraDistributorAdjustDetailModel detailModel in listModel)
            {
                if(detailModel.AdjustNumber== "DistributorName")//配送员姓名
                {
                    ViewBag.name = detailModel.AfterShowValue; 
                }
                if (detailModel.AdjustNumber == "DistributorNumber")//配送员ID
                {
                    ViewBag.number = detailModel.AfterShowValue; 
                } 
            }
            ViewBag.adjustId = adjustId;//申请id
            ViewBag.userid = userid;//配送员id
            ViewBag.remark = adjustModel.UserAdjustRemark;//申请备注
            return View(model);
        }

        /// <summary>
        /// 保存编辑操作数据
        /// </summary>
        /// <param name="adjustId">调整id</param>
        /// <param name="userid">配送员id</param>
        /// <param name="name">调整后配送员姓名</param>
        /// <param name="number">调整后配送员ID</param> 
        /// <param name="remark">申请备注</param>
        /// <returns></returns>
        public ActionResult EditDistributorAdjust(int adjustId,int userid, string name, string remark)
        {
            ResultEnum ResultEnums;
            string flagStr;
            string contentStr = "编辑失败";  
            TraDistributorAdjustModel adjustModel = bll.GetModelById(adjustId); 
            int rowCout = bll.UpdateDistributorAdjustRemark(remark, adjustId);
            if(rowCout>0)
            {
                rowCout = detailBLL.DelDistributorAdjustDetail(adjustId);
                if(rowCout>0)
                {
                    TraDistributorModel model = distributorBLL.GetModelById(userid);
                    List<TraDistributorAdjustDetailModel> lisetDetilModel = new List<TraDistributorAdjustDetailModel>();
                    //调整姓名
                    if (name != "")
                    {
                        TraDistributorAdjustDetailModel detilModel = new TraDistributorAdjustDetailModel();
                        detilModel.UserAdjustId = adjustId;//调整表id
                        detilModel.AdjustNumber = "DistributorName";//调整的字段
                        detilModel.BeforeRealityValue = model.DistributorName;//调整前实际内容
                        detilModel.BeforeShowValue = model.DistributorName;//调整前显示内容
                        detilModel.AfterRealityValue = name;// 调整后实际内容
                        detilModel.AfterShowValue = name;//调整后显示内容
                        lisetDetilModel.Add(detilModel);
                    }
                    ////调整配送人员ID
                    //if (number != "")
                    //{
                    //    TraDistributorAdjustDetailModel detilModel = new TraDistributorAdjustDetailModel();
                    //    detilModel.UserAdjustId = adjustId;//调整表id
                    //    detilModel.AdjustNumber = "DistributorNumber";//调整的字段
                    //    detilModel.BeforeRealityValue = model.DistributorNumber;//调整前实际内容
                    //    detilModel.BeforeShowValue = model.DistributorNumber;//调整前显示内容
                    //    detilModel.AfterRealityValue = number;// 调整后实际内容
                    //    detilModel.AfterShowValue = number;//调整后显示内容
                    //    lisetDetilModel.Add(detilModel);
                    //} 
                   rowCout = detailBLL.LisrtAddDistributorAdjustDetail(lisetDetilModel);
                    if(rowCout>0)
                    {
                        //修改成功
                        ResultEnums = ResultEnum.Sucess;
                        flagStr = "success";
                        contentStr = "编辑成功";
                    }
                    else
                    {
                        //修改失败
                        ResultEnums = ResultEnum.Fail; 
                        flagStr = "fail";
                    }
                }else
                {
                    //修改失败
                    ResultEnums = ResultEnum.Fail;
                    flagStr = "fail"; 
                }
            }else
            {
                //修改失败
                ResultEnums = ResultEnum.Fail;
                flagStr = "fail"; 
            }
            //记录系统日志
            Auxiliary.Log(OperateEnum.Edit, ResultEnums, adjustModel);
            return Json(new { flag = flagStr, content = contentStr});
        }
        #endregion
        #region 提交

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="adjustId">调整id</param>
        /// <param name="userid">配送人员表ID</param>
        /// <returns></returns>
        [Operate(Name = OperateEnum.Submit)] // 提交按钮权限
        public ActionResult SubmitState(int adjustId,int userid)
        {
            TraDistributorAdjustModel adjustModel = bll.GetModelById(adjustId);
            ResultEnum ResultEnums= ResultEnum.Sucess;
            string flagStr= "success";
            string contentStr = "提交成功!";
            int rowCouts=bll.Submit(adjustId, userid);
            if(rowCouts==0)
            {
                ResultEnums = ResultEnum.Fail;
                contentStr = "提交失败!";
                flagStr = "fail";
            } 
            //记录系统日志
            Auxiliary.Log(OperateEnum.Submit, ResultEnums, adjustModel);
            return Json(new { flag = flagStr, content= contentStr });
        }

        #endregion
        #region 查看
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="adjustId">调整id</param>
        /// <param name="userid">配送人员表ID</param>
        /// <returns></returns>
        public ActionResult View(int adjustId, int userid)
        {
            string where = " DistributorUserId  =" + userid + "  ";
            TraDistributorModel model = distributorBLL.GetModelByWhere(where);// 查询配送人员信息
            TraDistributorAdjustModel adjustModel = bll.GetModelById(adjustId);// 查询调整配送人员信息
            List<TraDistributorAdjustDetailModel> listModel = detailBLL.ListModel(adjustId); // 查询调整配送人员详细信息
            foreach (TraDistributorAdjustDetailModel detailModel in listModel)
            {
                if (detailModel.AdjustNumber == "DistributorName")//配送员姓名
                {
                    ViewBag.name = detailModel.AfterShowValue;
                }
                if (detailModel.AdjustNumber == "DistributorNumber")//配送员ID
                {
                    ViewBag.number = detailModel.AfterShowValue;
                }
            }
            ViewBag.adjustId = adjustId;//申请id
            ViewBag.userid = userid;//配送员id
            ViewBag.remark = adjustModel.UserAdjustRemark;//申请备注
            return View(model);
        }
        #endregion
        #region 作废
        [Operate(Name = OperateEnum.Invalid)]
        public ActionResult Del(int useradjustid, int state)
        {
            ResultEnum ResultEnums;
            string flagStr = string.Empty;
            string contentStr = "作废失败";
            TraDistributorAdjustModel delModel = bll.GetModelById(useradjustid);
            if(state!= delModel.UserAdjustState)//状态不一致
            {
                //作废失败
                ResultEnums = ResultEnum.Fail;
                flagStr = "noauth";
            }else
            {
                int RowCounts = bll.DelTraDistributorAdjust(Auxiliary.UserID(), DateTime.Now.ToString(), 50, useradjustid);
                if (RowCounts > 0)
                { //作废成功
                    ResultEnums = ResultEnum.Sucess;
                    flagStr = "success";
                    contentStr = "作废成功";
                }
                else
                { //作废失败
                    ResultEnums = ResultEnum.Fail;
                    flagStr = "noauth";
                }
            }
            //记录系统日志
            Auxiliary.Log(OperateEnum.Invalid, ResultEnums, delModel);
            return Json(new { flag = flagStr, content= contentStr });

        }
        #endregion
        #region 导出
        /// <summary>
        /// 导出操作
        /// </summary>
        /// <param name="starttime">申请开始时间</param>
        /// <param name="endtime">申请结束时间</param>
        /// <param name="suppname">供应商名称</param> 
        /// <param name="state">状态</param>
        /// <param name="substarttime">提交开始时间</param>
        /// <param name="subendtime">提交开始时间</param>
        /// <returns></returns>
        public ActionResult Export(string starttime, string endtime, string suppname,   int state,string substarttime,string subendtime)
        {
            StringBuilder sbStr = new StringBuilder();
            sbStr.Append(" where butorAdjust.CreateDepartmentId=" + Auxiliary.DepartmentId());//只允许查询本机构
            sbStr.Append(" and butorAdjust.UserAdjustState<=20  ");//有效的信息
            if (!string.IsNullOrEmpty(starttime))//申请开始时间
            {
                sbStr.Append("and butorAdjust.CreateTime>='" + starttime + "'");
            }
            if (!string.IsNullOrEmpty(endtime))//申请结束时间
            {
                DateTime times = Convert.ToDateTime(endtime).AddDays(1);
                sbStr.Append("and butorAdjust.CreateTime<='" + times + "'");
            }
            if (!string.IsNullOrEmpty(substarttime))//提交开始时间
            {
                sbStr.Append("and butorAdjust.SubmTime>='" + substarttime + "'");
            }
            if (!string.IsNullOrEmpty(subendtime))//提交结束时间
            {
                DateTime times = Convert.ToDateTime(subendtime).AddDays(1);
                sbStr.Append("and butorAdjust.SubmTime<='" + times.Date + "'");
            }
            if (!string.IsNullOrEmpty(suppname))//供应商名称
            {
                sbStr.Append("and Supp.SupplierName like '%" + suppname + "%'");
            } 
            if (state>0)//状态
            {
                sbStr.Append("and butorAdjust.UserAdjustState='" + state + "'");
            }
            DataTable dt = bll.ExportDataTable(sbStr.ToString());//获取导出样式和导出数据
            SRM.Common.ExcelHelper excel = new Common.ExcelHelper();
            string url = excel.ExcelToDisk(dt);
            return Json(new { flag = "ok", guid = url });
        }
        #endregion
        #region 查询配送人信息
        public ActionResult SelectDistributor(string url)
        {
            ViewBag.url = url;
            return View();
        }
        /// <summary>
        /// 配送员数量
        /// </summary>
        /// <param name="suppname">运输供应商名称</param>
        /// <param name="lineName">运作开始时间的开始</param>
        /// <param name="suppscope">运作开始时间的结束</param> 
        /// <param name="name">配送人员</param> 
        /// <returns></returns>
        public int DistributorRowCount(string suppname, string starttime, string endtime,string name)
        {
            StringBuilder strB = new StringBuilder();//拼接查询SQL语句
            strB.Append(" traDis.CreateDepartmentId= " + Auxiliary.DepartmentId());//本机构创建 
            strB.Append(" and traDis.DistributorState=10 ");//非作废信息 
            strB.Append(" and traDis.CreateDepartmentId=" + Auxiliary.DepartmentId());//查询本机构信息
            strB.Append(" and  traDis.CompanyId=" + Auxiliary.CompanyID());//查询本公司信息
            strB.Append("   and  traDis.DistributorUserId not in (SELECT DistributorUserId FROM TraDistributorAdjust WHERE  UserAdjustState=0) ");//申请状态为申请状态除去
            if (!string.IsNullOrEmpty(suppname))//运输供应商名称
            {
                strB.Append(" And supp.SupplierName like '%" + suppname.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(starttime))//运作开始时间的开始
            {
                strB.Append(" And traDis.SubmitTime >='" + starttime + "' ");
            }
            if (!string.IsNullOrEmpty(starttime))//运作开始时间的结束
            {
                DateTime time = Convert.ToDateTime(endtime);
                strB.Append(" And traDis.SubmitTime <'" + time.Date + "' ");
            }
            if (!string.IsNullOrEmpty(name))//配送人员
            {
                strB.Append(" And traDis.DistributorName like '%" + name.Trim() + "%' ");
            }
            return distributorBLL.TraDistributorPageCount(strB.ToString()); 
        }

        #region 配送员列表数据
        /// <summary>
        /// 配送员数量
        /// </summary>
        /// <param name="index">当前页号</param>
        /// <param name="size">每页条数</param>
        /// <param name="name">运输供应商名称</param>
        /// <param name="lineName">运作开始时间的开始</param>
        /// <param name="suppscope">运作开始时间的结束</param> 
        /// <param name="name">配送人员</param> 
        /// <returns></returns>
        public ActionResult DistributorList(int index, int size, string suppname, string starttime, string endtime, string name)
        {
            StringBuilder strB = new StringBuilder();//拼接查询SQL语句
            strB.Append(" traDis.CreateDepartmentId= "+Auxiliary.DepartmentId());//本机构创建 
            strB.Append(" and traDis.DistributorState=10 ");//非作废信息 
            strB.Append(" and traDis.CreateDepartmentId=" + Auxiliary.DepartmentId());//查询本机构信息
            strB.Append(" and  traDis.CompanyId=" + Auxiliary.CompanyID());//查询本公司信息
            strB.Append("   and  traDis.DistributorUserId not in (SELECT DistributorUserId FROM TraDistributorAdjust WHERE  UserAdjustState=0) ");//申请状态为申请状态除去
            if (!string.IsNullOrEmpty(suppname))//运输供应商名称
            {
                strB.Append(" And supp.SupplierName like '%" + suppname.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(starttime))//运作开始时间的开始
            {
                strB.Append(" And traDis.SubmitTime >='" + starttime + "' ");
            }
            if (!string.IsNullOrEmpty(starttime))//运作开始时间的结束
            {
                DateTime time = Convert.ToDateTime(endtime);
                strB.Append(" And traDis.SubmitTime <'" + time.Date + "' ");
            }
            if (!string.IsNullOrEmpty(name))//配送人员
            {
                strB.Append(" And traDis.DistributorName like '%" + name.Trim() + "%' ");
            }
            List<TraDistributorModel> listModel = distributorBLL.TraDistributorPageList(index, size, strB.ToString());
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();//定义显示时间样式
            timeFormat.DateTimeFormat = "yyyy-MM-dd";
            string contentString = Newtonsoft.Json.JsonConvert.SerializeObject(listModel, Newtonsoft.Json.Formatting.Indented, timeFormat);
            return Content(contentString);
        }
        #endregion
        #endregion
    }
}
