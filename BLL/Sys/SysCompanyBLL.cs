//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-05-20    1.0         MH        新建
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

using Model.Sys;
using DAL.Sys;
#endregion
/*********************************
 * 类名：SysCompanyBLL
 * 功能描述：系统公司表 业务逻辑层
 * ******************************/
namespace BLL.Sys
{
    public class SysCompanyBLL
    {
        SysCompanyDAL dal = new SysCompanyDAL();



        #region 添加 系统公司表
        /// <summary>
        /// 添加 系统公司表
        /// </summary>
        public int AddSysCompany(SysCompanyModel model)
        {
            return dal.AddSysCompany(model);
        }
        #endregion 

        #region 修改 系统公司表
        /// <summary>
        /// 修改 系统公司表
        /// </summary>
        public int UpdateSysCompany(SysCompanyModel model)
        {
            return dal.UpdateSysCompany(model);
        }
        #endregion 

        #region 删除 系统公司表
        /// <summary>
        /// 删除 系统公司表
        /// </summary>
        public int DeleteSysCompanyByID(string id)
        {
            return dal.DeleteSysCompanyByID(id);
        }
        #endregion  

        #region 分页列表 系统公司表
        /// <summary>
        ///  分页列表 系统公司表
        /// </summary>
        public List<SysCompanyModel> SysCompanyPageList(int index, int size, string where)
        {
            return dal.SysCompanyPageList(index, size, where);
        }
        #endregion   

        #region 分页总数 系统公司表
        /// <summary>
        ///  分页总数 系统公司表
        /// </summary>
        public int SysCompanyPageCount(string where)
        {
            return dal.SysCompanyPageCount(where);
        }
        #endregion  

        #region 获取实体 系统公司表
        /// <summary>
        ///  获取实体 系统公司表
        /// </summary>
        public SysCompanyModel GetModelByID(string id)
        {
            return dal.GetModelByID(id);
        }
        #endregion

        #region 是否存在相同的企业
        public int IsHasComp(int id, string comp)
        {
            return dal.IsHasComp(id, comp);
        }
        #endregion

        #region 添加默认账号默认角色
        public int AddDefaultInfo(string name, string pwd, string rolename, string rolememo, string cid,string depId, ref int roleid)
        {
            SysUserModel model = new SysUserModel();

            model.UserName = name;//登录用户
            model.UserSpelling = string.Empty;//用户拼写
            model.Password = pwd; //登录密码
            model.TrueName = string.Empty; //真实名称
            model.MobileNumber = string.Empty;//手机号码
            model.PhoneNumber = string.Empty; //电话号码
            model.QQ = string.Empty; //QQ
            model.EmailAddress = string.Empty; //Email
            model.OtherContact = string.Empty; //其他联系方式
            model.Province = string.Empty; //省份
            model.City = string.Empty;//城市
            model.Village = string.Empty; //地区
            model.Address = string.Empty; //详细地址
            model.Sex = string.Empty; //型别                   
            model.Birthday = string.Empty;//出生日期
            model.JoinDate = string.Empty; //入职日期                 
            model.Native = string.Empty; //籍贯
            model.DepartmentId = 0; //组织机构id
            model.Expertise = string.Empty; ; //个人简介
            model.JobState = 1; //在职状况1在职，0离职
            model.Photo = string.Empty; ; //照片
            model.Attach = "默认角色"; //附件
            model.CompanyId = Convert.ToInt32(cid); //系统公司id
            model.CreateTime = DateTime.Now; //创建时间
            model.CreatePerson = string.Empty;//创建人
            model.State = 1; //状态：0-无效;1-有效;2-作废
            model.IsSystem = 1;
            if(!string.IsNullOrEmpty(depId))
            model.DepartmentId =Convert.ToInt32(depId);//机构id
            SysRoleModel roleModel = new SysRoleModel();

            roleModel.RoleName = rolename; //角色名称
            roleModel.State = 1; //状态：0-无效;1-有效
            roleModel.CreateTime = DateTime.Now; //创建时间
            roleModel.CreatePerson = string.Empty; //创建人
            roleModel.Remark = rolememo; //说明
            roleModel.CompanyId = Convert.ToInt32(cid); //系统公司id
            roleModel.IsSystem = 1;

            return dal.SetDefaultInfo(model, roleModel, cid, ref roleid);
        }
        #endregion

        #region 更改默认账号默认角色
        public int UpdateDefaultInfo(string uid, string uname, string roleid, string rolename, string rolememo)
        {
            return dal.UpdateDefaultInfo(uid, uname, roleid, rolename, rolememo);
        }
        #endregion

        #region 变更 企业状态
        public int ChangeState(int state, string uid)
        {
            return dal.ChangeState(state, uid);
        }
        #endregion

        #region 当前账套前缀流水号
        public string GetAutoNum(string cid, string prefix)
        {
            return dal.GetAutoNum(cid, prefix);
        }
        #endregion
    }
}

