//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/26    1.0        MH        
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
 * 类名：SysAdjunctTypeDAL
 * 功能描述：附件类型 数据访问层类
 * ******************************/
namespace DAL.Sys
{
    public class SysAdjunctTypeDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 附件类型
        /// <summary>
        /// 添加 附件类型
        /// </summary>
        public int AddSysAdjunctType(SysAdjunctTypeModel model)
        {
            string sql = @"Insert 
                                  SysAdjunctType
                                  (
                                        AdjunctId ,
                                        AdjunctName ,
                                        Sort ,
                                        State ,
                                        AdjunctType ,
                                        CreateTime ,
                                        CompanyId 
                                      
                                  )
                          Values
                                  (
                                        @AdjunctId ,
                                        @AdjunctName ,
                                        @Sort ,
                                        @State ,
                                        @AdjunctType ,
                                        GETDATE() ,
                                        @CompanyId 
                                  )";
            SqlParameter[] param ={
                                        new SqlParameter("@AdjunctId",model.AdjunctId) ,  //附件类型id   
                                        new SqlParameter("@AdjunctName",model.AdjunctName) ,  //附件名称   
                                        new SqlParameter("@Sort",model.Sort) ,  //排序   
                                        new SqlParameter("@State",model.State) ,  //状态：1-有效;0-无效   
                                        new SqlParameter("@AdjunctType",model.AdjunctType) ,  //附件类型：0-运输供应商附件   
                                        //new SqlParameter("@CreateTime",model.CreateTime) ,  //创建时间   
                                        new SqlParameter("@CompanyId",model.CompanyId)   //系统公司id   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 修改 附件类型
        /// <summary>
        /// 修改 附件类型
        /// </summary>
        public int EditSysAdjunctType(SysAdjunctTypeModel model)
        {
            string sql = @"Update 
                                  SysAdjunctType
                          Set 
                                  AdjunctName = @AdjunctName , 
                                  AdjunctType = @AdjunctType 
                          Where         
                                  AdjunctId = @AdjunctId"
                        ;

            SqlParameter[] param ={
                                        new SqlParameter("@AdjunctId",model.AdjunctId) ,//附件类型id   
                                        new SqlParameter("@AdjunctName",model.AdjunctName) ,//附件名称    
                                        new SqlParameter("@AdjunctType",model.AdjunctType) //附件类型：0-运输供应商附件   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 删除 附件类型
        /// <summary>
        /// 删除 附件类型
        /// </summary>
        public int DeleteSysAdjunctTypeByID(string id)
        {
            string sql = @"Delete 
                                  SysAdjunctType
                          Where         
                                  AdjunctId = @AdjunctId"
 ;

            SqlParameter[] param ={
                                        new SqlParameter("@AdjunctId",id)
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion 

        #region 列表 附件类型
        /// <summary>
        ///  列表 附件类型
        /// </summary>
        public List<SysAdjunctTypeModel> SysAdjunctTypePageList(string where)
        {
            string sql = @" With TemTable As 
                            (
                                Select Row_Number() Over(Order By Sort) AS 't',*
                                From
                                       SysAdjunctType
                                Where
                                       State=1 And " + where + @"
                            )
                                  
                            Select * From TemTable ";

            List<SysAdjunctTypeModel> list = new List<SysAdjunctTypeModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            while (dr.Read())
            {
                SysAdjunctTypeModel model = new SysAdjunctTypeModel();
                model.AdjunctId = Convert.ToInt32(dr["AdjunctId"].ToString()); //附件类型id
                model.AdjunctName = dr["AdjunctName"].ToString(); //附件名称
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：1-有效;0-无效
                model.AdjunctType = Convert.ToInt32(dr["AdjunctType"].ToString()); //附件类型：0-运输供应商附件
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 分页列表 附件类型 

        /// <summary>
        /// 分页列表 附件类型
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">条数</param>
        /// <param name="where">条件</param>
        /// <returns>list</returns> 
        public List<SysAdjunctTypeModel> SysAdjunctTypeList(int tIndex, int tSize, string tWhere)
        {
            string sql = @"With TemTable As 
                           (
                           SELECT
                               Row_Number() Over(Order By AdjunctId DESC) AS 't'
                               ,AdjunctId
                               ,AdjunctName
                               ,SAT.Sort
                               ,State
                               ,CASE State
                                   WHEN '0' THEN '无效'
                                   WHEN '1' THEN '有效'  
                               END AS StateName 
                               ,AdjunctType
							   ,DictionaryNumber
                               ,DictionaryName AdjunctTypeName 
                               ,CreateTime 
                               ,SAT.CompanyId 
                           FROM 
                               SysAdjunctType SAT
						   LEFT JOIN BasisDictionary BD ON SAT.AdjunctType = BD.DictionaryNumber
                           WHERE" + tWhere + @" 
                            )
                            Select * From TemTable Where t  Between @Size*(@Index-1)+1 And @Size*@Index";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",tSize),
                                   new SqlParameter("@Index",tIndex)
                                  };

            // 实例化数据集
            List<SysAdjunctTypeModel> list = new List<SysAdjunctTypeModel>();

            // 从数据库读取数据
            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            }
            catch (Exception ex)
            {
                return null;
            }

            // 循环数据加入到数据集中
            while (dr.Read())
            {
                SysAdjunctTypeModel model = new SysAdjunctTypeModel();

                // 附件类型id
                model.AdjunctId = Convert.ToInt32(dr["AdjunctId"].ToString());

                // 附件名称
                model.AdjunctName = dr["AdjunctName"].ToString();

                // 排序
                model.Sort = Convert.ToInt32(dr["Sort"].ToString());

                // 状态 
                model.State = Convert.ToInt32(dr["State"].ToString());

                // 状态名称
                model.StateName = dr["StateName"].ToString();

                // 附件类型 
                model.AdjunctType = Convert.ToInt32(dr["AdjunctType"].ToString());

                // 附件类型名称
                model.AdjunctTypeName = dr["AdjunctTypeName"].ToString();

                // 创建时间
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString());

                // 系统公司id
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());

                // 加入到数据集
                list.Add(model);
            }

            // 关闭
            dr.Close();

            // 返回数据集
            return list;
        }

        #endregion  

        #region 分页总数 附件类型

        /// <summary>
        /// 分页总数 附件类型
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>int</returns>
        public int SysAdjunctTypeCount(string tWhere)
        {
            string sql = @"SELECT
                               Count(AdjunctId) 
                           FROM 
                               SysAdjunctType SAT
						   LEFT JOIN BasisDictionary BD ON SAT.AdjunctType = BD.DictionaryNumber
                           WHERE" + tWhere;

            object obj = new object();

            try
            {
                obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, null);

                if (obj != null)
                {
                    return Convert.ToInt32(obj.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        #region 获取实体 附件类型
        /// <summary>
        ///  获取实体 附件类型
        /// </summary>
        public SysAdjunctTypeModel GetModelByID(int id)
        {
            string sql = @" Select 
                                  AdjunctId ,
                                  AdjunctName ,
                                  Sort ,
                                  State ,
                                  AdjunctType ,
                                  CreateTime ,
                                  CompanyId 
                            From
                                  SysAdjunctType
                            Where
                                  AdjunctId=@AdjunctId";
            SqlParameter[] param ={
                                   new SqlParameter("@AdjunctId",id)
                                  };

            SysAdjunctTypeModel model = null;

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            if (dr.Read())
            {
                model = new SysAdjunctTypeModel();
                model.AdjunctId = Convert.ToInt32(dr["AdjunctId"].ToString()); //附件类型id
                model.AdjunctName = dr["AdjunctName"].ToString(); //附件名称
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：1-有效;0-无效
                model.AdjunctType = Convert.ToInt32(dr["AdjunctType"].ToString()); //附件类型：0-运输供应商附件
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString()); //系统公司id
            }
            dr.Close();

            return model;
        }
        #endregion
    }
}

