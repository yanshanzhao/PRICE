//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018-04-25    1.0         MH        新建
//-------------------------------------------------------------------------
#region 参照
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using DBUtility;
using Model.Sys;
#endregion
/*********************************
 * 类名：SysDepartmentDAL
 * 功能描述：系统组织机构表 数据访问层类
 * ******************************/
namespace DAL.Sys
{
    public class SysDepartmentDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();


        #region 添加 系统组织机构表
        /// <summary>
        /// 添加 系统组织机构表
        /// </summary>
        public int AddSysDepartment(SysDepartmentModel model)
        {
            string sql = @"Insert 
                                  SysDepartment
                                  (
                                        DepartmentName ,
                                        DepartmentSpelling ,
                                        FatherId ,
                                        Address ,
                                        Tel ,
                                        Sort ,
                                        DeotNo ,
                                        CompanyId ,
                                        State,
                                        DeparType            
                                      
                                  )
                          Values
                                  (
                                        @DepartmentName ,
                                        @DepartmentSpelling ,
                                        @FatherId ,
                                        @Address ,
                                        @Tel ,
                                        @Sort ,
                                        @DeotNo ,
                                        @CompanyId ,
                                        @State ,
                                        @DeparType
                                  )";
            SqlParameter[] param ={
                                        new SqlParameter("@DepartmentName",model.DepartmentName) ,  //组织机构名称   
                                        new SqlParameter("@DepartmentSpelling",model.DepartmentSpelling) ,  //组织机构拼写   
                                        new SqlParameter("@FatherId",model.FatherId) ,  //父节点   
                                        new SqlParameter("@Address",model.Address) ,  //详细地址   
                                        new SqlParameter("@Tel",model.Tel) ,  //联系电话   
                                        new SqlParameter("@Sort",model.Sort) ,  //排序   
                                        new SqlParameter("@DeotNo",model.DeotNo) ,  //机构编号   
                                        new SqlParameter("@CompanyId",model.CompanyId) ,  //系统公司id   
                                        new SqlParameter("@State",model.State),   //状态：1-可用;0-无效   
                                        new SqlParameter("@DeparType",model.DeparType)   //组织机构类型：0-根；1-公司级别;2-普通部门;3-供应商部门
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        public int AddSysCompanyToSysDepartment(int CompanyId)
        {
            string sql = "INSERT INTO SysDepartment(DepartmentName,  DepartmentSpelling,FatherId,Address,Sort,Tel,DeotNo,CompanyId,State,DeparType)SELECT CompanyName DepartmentName,''DepartmentSpelling,0 AS FatherId,Address,100 Sort,''Tel,CompanyNo AS DeotNo,CompanyId,State,1 AS DeparType FROM dbo.SysCompany WHERE CompanyId=@CompanyId";
            SqlParameter[] param ={
                                        new SqlParameter("@CompanyId",CompanyId)   //系统公司表ID    
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }

        #endregion

        #region 修改 系统组织机构表
        /// <summary>
        /// 修改 系统组织机构表
        /// </summary>
        public int UpdateSysDepartment(SysDepartmentModel model)
        {
            string sql = @"Update 
                                  SysDepartment
                          Set
                                  DepartmentName = @DepartmentName ,
                                  DepartmentSpelling = @DepartmentSpelling ,
                                  FatherId = @FatherId ,
                                  Address = @Address ,
                                  Tel = @Tel ,
                                  Sort = @Sort ,
                                  DeotNo = @DeotNo ,
                                  CompanyId = @CompanyId ,
                                  State = @State 
                          Where         
                                  DepartmentId = @DepartmentId
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@DepartmentId",model.DepartmentId) ,  //组织机构id   
                                        new SqlParameter("@DepartmentName",model.DepartmentName) ,  //组织机构名称   
                                        new SqlParameter("@DepartmentSpelling",model.DepartmentSpelling) ,  //组织机构拼写   
                                        new SqlParameter("@FatherId",model.FatherId) ,  //父节点   
                                        new SqlParameter("@Address",model.Address) ,  //详细地址   
                                        new SqlParameter("@Tel",model.Tel) ,  //联系电话   
                                        new SqlParameter("@Sort",model.Sort) ,  //排序   
                                        new SqlParameter("@DeotNo",model.DeotNo) ,  //机构编号   
                                        new SqlParameter("@CompanyId",model.CompanyId) ,  //系统公司id   
                                        new SqlParameter("@State",model.State)  //状态：1-可用;0-无效   
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 变更 机构状态
        /// <summary>
        /// 递归模式更新状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="did"></param>
        /// <returns></returns>
        public int ChangeState(int state, string did)
        {
            string sql = @"   WITH cteTree  AS
                            (    
                              SELECT * FROM SysDepartment
                               WHERE DepartmentId = @ID  
                               UNION ALL
                              SELECT SysDepartment.*    
                                FROM cteTree INNER JOIN SysDepartment
                                  ON cteTree.DepartmentId = SysDepartment.FatherId
                             ) 
                              UPDATE dbo.SysDepartment 
                                 SET [State]=@State 
                                FROM dbo.SysDepartment s,cteTree
                               WHERE s.DepartmentId=cteTree.DepartmentId 
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@ID",did) ,  //用户id自增主键    
                                        new SqlParameter("@State",state)   //状态：0-无效;1-有效   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 删除 系统组织机构表

        public int SysDepartmentUserCount(string id)
        {
            string sql = @"   WITH cteTree  AS
                            (    
                              SELECT * FROM SysDepartment
                               WHERE DepartmentId = @ID  
                               UNION ALL
                              SELECT SysDepartment.*    
                                FROM cteTree INNER JOIN SysDepartment
                                  ON cteTree.DepartmentId = SysDepartment.FatherId
                             ) 
                              Select Count(d.DeotNo) From cteTree d,SysUser u
                               WHERE d.DepartmentId=u.DepartmentId 
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@ID",id)
                                  };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }

        /// <summary>
        /// 递归删除 系统组织机构表
        /// </summary>
        public int DeleteSysDepartmentByID(string id)
        {
            string sql = @"   WITH cteTree  AS
                            (    
                              SELECT * FROM SysDepartment
                               WHERE DepartmentId = @ID  
                               UNION ALL
                              SELECT SysDepartment.*    
                                FROM cteTree INNER JOIN SysDepartment
                                  ON cteTree.DepartmentId = SysDepartment.FatherId
                             ) 
                              Delete dbo.SysDepartment 
                                FROM dbo.SysDepartment s,cteTree
                               WHERE s.DepartmentId=cteTree.DepartmentId 
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@ID",id)
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 获取 系统组织机构表
        /// <summary>
        ///  获取 系统组织机构表
        /// </summary>
        public List<SysDepartmentModel> SysDepartmentPageList(int cid, string keyword)
        {
            string sql = @" Select 
                                  DepartmentId ,
                                  DepartmentName ,
                                  DepartmentSpelling ,
                                  FatherId ,
                                  Address ,
                                  Tel ,
                                  Sort ,
                                  DeotNo ,
                                  State ,isnull(DeparType,2) DeparType,CASE WHEN DeparType=0 THEN '跟' WHEN DeparType=1
								   THEN '公司级别'WHEN DeparType=2 THEN '普通部门'WHEN DeparType=3 THEN '供应商部门' ELSE '普通部门' END DeparTypeName
                            From
                                  SysDepartment
                            Where
                                  CompanyId=@CompanyId  And DepartmentName+DepartmentSpelling like '%" + keyword + "%'";

            SqlParameter[] param ={
                                   new SqlParameter("@CompanyId",cid)
                                  };

            List<SysDepartmentModel> list = new List<SysDepartmentModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysDepartmentModel model = new SysDepartmentModel();
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString()); //组织机构id
                model.DepartmentName = dr["DepartmentName"].ToString(); //组织机构名称
                model.DepartmentSpelling = dr["DepartmentSpelling"].ToString(); //组织机构拼写
                model.FatherId = Convert.ToInt32(dr["FatherId"].ToString()); //父节点
                model.Address = dr["Address"].ToString(); //详细地址
                model.Tel = dr["Tel"].ToString(); //联系电话
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序
                model.DeotNo = dr["DeotNo"].ToString(); //机构编号
                model.CompanyId = cid; //系统公司id
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：1-可用;0-无效
                model.DeparType = Convert.ToInt32(dr["DeparType"].ToString()); //组织机构类型：0-根；1-公司级别;2-普通部门;3-供应商部门
                model.DeparTypeName = dr["DeparTypeName"].ToString(); //组织机构类型：0-根；1-公司级别;2-普通部门;3-供应商部门
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 获取实体 系统组织机构表
        /// <summary>
        ///  获取实体 系统组织机构表
        /// </summary>
        public SysDepartmentModel GetModelByID(string id)
        {
            string sql = @" Select 
                                  DepartmentId ,
                                  DepartmentName ,
                                  DepartmentSpelling ,
                                  FatherId ,
                                  Address ,
                                  Tel ,
                                  Sort ,
                                  DeotNo ,
                                  CompanyId ,
                                  State  ,isnull(DeparType,2) as DeparType,CASE WHEN DeparType=0 THEN '跟' WHEN DeparType=1
								   THEN '公司级别'WHEN DeparType=2 THEN '普通部门'WHEN DeparType=3 THEN '供应商部门' ELSE '普通部门' END DeparTypeName
                            From
                                  SysDepartment
                            Where
                                  DepartmentId=@DepartmentId";
            SqlParameter[] param ={
                                   new SqlParameter("@DepartmentId",id)
                                  };

            SysDepartmentModel model = null;

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            if (dr.Read())
            {
                model = new SysDepartmentModel();
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString()); //组织机构id
                model.DepartmentName = dr["DepartmentName"].ToString(); //组织机构名称
                model.DepartmentSpelling = dr["DepartmentSpelling"].ToString(); //组织机构拼写
                model.FatherId = Convert.ToInt32(dr["FatherId"].ToString()); //父节点
                model.Address = dr["Address"].ToString(); //详细地址
                model.Tel = dr["Tel"].ToString(); //联系电话
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序
                model.DeotNo = dr["DeotNo"].ToString(); //机构编号
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：1-可用;0-无效
                model.DeparType = Convert.ToInt32(dr["DeparType"].ToString()); //组织机构类型：0-根；1-公司级别;2-普通部门;3-供应商部门
                model.DeparTypeName =dr["DeparTypeName"].ToString(); //组织机构类型：0-根；1-公司级别;2-普通部门;3-供应商部门
            }
            dr.Close();

            return model;
        }
        #endregion

        #region 是否存在相同机构
        /// <summary>
        /// 检查相同项
        /// </summary>
        /// <param name="cid">公司编号</param>
        /// <param name="id">主键</param>
        /// <param name="pid">父类编号</param>
        /// <param name="name">机构名称</param>
        /// <returns></returns>
        public int IsHasDep(int cid, int id, int pid, string name)
        {
            string sql = string.Empty;

            if (id == 0)
            {
                sql = "Select Count(*) From SysDepartment Where  CompanyId=@CompanyId And FatherId=@FatherId And DepartmentName=@DepartmentName";

                SqlParameter[] param ={
                                   new SqlParameter("@CompanyId",cid),
                                   new SqlParameter("@FatherId",pid),
                                   new SqlParameter("@DepartmentName",name.Trim())
                                  };
                object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
                if (obj == null)
                {
                    return 0;
                }
                return Convert.ToInt32(obj.ToString());
            }
            else
            {
                sql = "Select Count(*) From SysDepartment Where  CompanyId=@CompanyId And FatherId=@FatherId And DepartmentName=@DepartmentName And DepartmentId!=@DepartmentId";

                SqlParameter[] param ={
                                   new SqlParameter("@CompanyId",cid),
                                   new SqlParameter("@FatherId",pid),
                                   new SqlParameter("@DepartmentName",name.Trim()),
                                   new SqlParameter("@DepartmentId",id)
                                  };
                object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
                if (obj == null)
                {
                    return 0;
                }
                return Convert.ToInt32(obj.ToString());
            }
        }
        #endregion


        #region 分页列表 系统组织机构表
        /// <summary>
        ///  分页列表 系统组织机构表
        /// </summary>
        public List<SysDepartmentModel> SysDepartmentPageList(int index, int size, int cid, string keyword)
        {
            string sql = @" With TemTable As 
                            (
                                Select   Row_Number() Over(Order By d.Sort desc) AS 't',
                                         d.DepartmentId,
                                         d.DepartmentName,
                                         u.UserName,
                                         u.TrueName,
                                         u.UserId
                                From
                                         SysDepartment d,SysUser u
                                Where
                                      
                                       d.CompanyId=u.CompanyId  And d.CompanyId=@CompanyId And u.DepartmentId=d.DepartmentId And d.State=1 And d.DepartmentName+d.DepartmentSpelling like '%" + keyword + @"%'
                            )                                
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";

            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index),
                                   new SqlParameter("@CompanyId",cid)
                                  };


            List<SysDepartmentModel> list = new List<SysDepartmentModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysDepartmentModel model = new SysDepartmentModel();
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString()); //组织机构id
                model.DepartmentName = dr["DepartmentName"].ToString(); //组织机构名称
                model.UserName = dr["UserName"].ToString(); //用户账号
                model.TrueName = dr["TrueName"].ToString(); //真实姓名
                model.UserId = Convert.ToInt32(dr["UserId"].ToString());
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 分页总数 系统组织机构表
        /// <summary>
        ///  分页总数 系统组织机构表
        /// </summary>
        public int SysDepartmentPageCounts(int cid, string keyword)
        {
            string sql = @"   Select   
                                         Count(d.DepartmentId)
                                From
                                         SysDepartment d,SysUser u
                                Where
                                      
                                       d.CompanyId=u.CompanyId  And d.CompanyId=@CompanyId AND u.DepartmentId=d.DepartmentId And d.State=1 And d.DepartmentName+d.DepartmentSpelling like '%" + keyword + @"%'";

            SqlParameter[] param ={
                                   new SqlParameter("@CompanyId",cid)
                                  };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
            if (obj != null)
            {
                return Convert.ToInt32(obj.ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 列表式机构+真实姓名

        #region 分页列表 系统组织机构表
        /// <summary>
        ///  分页列表 系统组织机构表
        /// </summary>
        public List<SysDepartmentModel> SysDepartmentList(int index, int size, int cid, string keyword)
        {
            string sql = @" With TemTable As 
                            (
                                Select   Row_Number() Over(Order By d.Sort desc) AS 't',
                                         d.DepartmentId,
                                         d.DepartmentName,
                                         u.UserName,
                                         u.TrueName,
                                         u.UserId
                                From
                                         SysDepartment d,SysUser u
                                Where
                                      
                                       d.CompanyId=u.CompanyId  And d.CompanyId=@CompanyId And u.DepartmentId=d.DepartmentId And " + keyword + @"
                            )                                
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            
            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index),
                                   new SqlParameter("@CompanyId",cid)
                                  };


            List<SysDepartmentModel> list = new List<SysDepartmentModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysDepartmentModel model = new SysDepartmentModel();
                model.DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString()); //组织机构id
                model.DepartmentName = dr["DepartmentName"].ToString(); //组织机构名称
                model.UserName = dr["UserName"].ToString(); //用户账号
                model.TrueName = dr["TrueName"].ToString(); //真实姓名
                model.UserId = Convert.ToInt32(dr["UserId"].ToString());
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 分页总数 系统组织机构表
        /// <summary>
        ///  分页总数 系统组织机构表
        /// </summary>
        public int SysDepartmentCounts(int cid, string keyword)
        {
            string sql = @"   Select   
                                         Count(d.DepartmentId)
                                From
                                         SysDepartment d,SysUser u
                                Where
                                      
                                       d.CompanyId=u.CompanyId  And d.CompanyId=@CompanyId AND u.DepartmentId=d.DepartmentId And " + keyword + @""; 
            SqlParameter[] param ={
                                   new SqlParameter("@CompanyId",cid)
                                  };

            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
            if (obj != null)
            {
                return Convert.ToInt32(obj.ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion
        #endregion
    }
}

