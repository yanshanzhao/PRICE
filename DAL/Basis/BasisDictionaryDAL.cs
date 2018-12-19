//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/05/30    1.0        ZBB        新建   
//-------------------------------------------------------------------------
#region 参数
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using Model.Basis;
#endregion
/*********************************
 * 类名：BasisDictionaryDAL
 * 功能描述：系统字典表 数据访问层类  
 * ******************************/
namespace DAL.Basis
{
    public class BasisDictionaryDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();

        #region 添加 系统字典表

        /// <summary>
        /// 添加 系统字典表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int AddBasisDictionary(BasisDictionaryModel model)
        {
            string sql = @"Insert into
                                  BasisDictionary
                                  (
                                       DictionaryName,
                                       DictionaryNumber,
                                       DictionaryType,
                                       Sort,
                                       DictionaryState,
                                       UseType,
                                       CompanyId
                                  )
                          Values
                                  (
                                       @DictionaryName,
                                       @DictionaryNumber,
                                       @DictionaryType,
                                       @Sort,
                                       @DictionaryState,
                                       @UseType,
                                       @CompanyId
                                  )";
            SqlParameter[] param ={
                                        new SqlParameter("@DictionaryName",model.DictionaryName) ,  //字典名称   

                                        new SqlParameter("@DictionaryNumber",model.DictionaryNumber) ,  //字典序号

                                        new SqlParameter("@DictionaryType",model.DictionaryType) ,  //字典类型   

                                        new SqlParameter("@Sort",model.Sort) ,  //字典排序   

                                        new SqlParameter("@DictionaryState",model.DictionaryState) ,  //字典状态:0-无效;1-有效
                                           
                                        new SqlParameter("@UseType",model.UseType),   //使用类型：0-系统统一;1-公司使用

                                        new SqlParameter("@CompanyId",model.CompanyId)   //系统公司id 
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 修改 系统字典表

        /// <summary>
        /// 修改 系统字典表
        /// </summary>
        /// <param name="model">数据model</param>
        /// <returns></returns>
        public int UpdateBasisDictionary(BasisDictionaryModel model)
        {
            string sql = @"Update 
                                  BasisDictionary
                          Set
                                  DictionaryName = @DictionaryName ,
                                  DictionaryNumber = @DictionaryNumber ,
                                  DictionaryType = @DictionaryType ,
                                  Sort = @Sort ,
                                  DictionaryState = @DictionaryState ,
                                  UseType = @UseType,
                                  CompanyId = @CompanyId 
                          Where         
                                  DictionaryId = @DictionaryId";
            SqlParameter[] param ={
                                        new SqlParameter("@DictionaryId",model.DictionaryId) ,  //字典id

                                        new SqlParameter("@DictionaryName",model.DictionaryName) ,  //字典名称   

                                        new SqlParameter("@DictionaryNumber",model.DictionaryNumber) ,  //字典序号   

                                        new SqlParameter("@DictionaryType",model.DictionaryType) ,  //字典类型

                                        new SqlParameter("@Sort",model.Sort) ,  //字典排序   

                                        new SqlParameter("@DictionaryState",model.DictionaryState) ,  //字典状态:0-无效;1-有效 

                                        new SqlParameter("@UseType",model.UseType) ,  //使用类型：0-系统统一;1-公司使用

                                        new SqlParameter("@CompanyId",model.CompanyId)   //系统公司id    
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);

            return i;
        }
        #endregion

        #region 删除 系统字典表

        /// <summary>
        /// 删除 系统字典表
        /// </summary>
        /// <param name="id">系统字典id</param>
        /// <returns></returns>
        public int DeleteBasisDictionaryByID(string id)
        {
            string sql = @"Delete 
                                  BasisDictionary
                          Where         
                                  DictionaryId = @DictionaryId";
            SqlParameter[] param ={
                                        new SqlParameter("@DictionaryId",id)
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 选择供应商种类信息

        #region 分页列表 选择供应商种类信息

        /// <summary>
        /// 分页列表 选择供应商种类信息
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <returns></returns>
        public List<BasisDictionaryModel> ChooseDictionaryList(int index, int size)
        {
            string sql = @"With TemTable As 
                           (SELECT  ROW_NUMBER() OVER ( ORDER BY DictionaryId DESC ) AS 't' ,
                                    *
                            FROM    ( SELECT    DictionaryId ,
                                                DictionaryName
                                      FROM      dbo.BasisDictionary
                                      WHERE     DictionaryType = 1
                                                AND UseType = 0
                                                AND DictionaryState = 1
                                      UNION ALL
                                      SELECT    DictionaryId ,
                                                DictionaryName
                                      FROM      dbo.BasisDictionary
                                      WHERE     DictionaryType = 1
                                                AND UseType = 1
                                                AND CompanyId = 1
                                                AND DictionaryState = 1
                                    ) AS a
                        )
                              
                        Select * From TemTable Where t  Between @Size*(@Index-1)+1 And @Size*@Index";

            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };

            List<BasisDictionaryModel> list = new List<BasisDictionaryModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);

            while (dr.Read())
            {
                BasisDictionaryModel model = new BasisDictionaryModel();

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString()); // 字典Id

                model.DictionaryName = dr["DictionaryName"].ToString(); // 字典名称
                list.Add(model);// 加入到数据集
            }
            dr.Close();//关闭 
            return list;// 返回数据集
        }
        #endregion  

        #region 分页总数 选择供应商种类信息

        /// <summary>
        /// 分页总数 选择供应商种类信息
        /// </summary>
        /// <returns></returns>
        public int ChooseDictionaryCount()
        {
            string sql = @" SELECT  Count(DictionaryId)  FROM dbo.BasisDictionary WHERE DictionaryType=1 AND UseType=0   AND DictionaryState=1";

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
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #endregion

        #region 选择信息类型

        #region 分页列表 选择信息类型

        /// <summary>
        /// 分页列表 选择信息类型
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <returns></returns>
        public List<BasisDictionaryModel> ChooseMessageTypeList(int index, int size)
        {
            string sql = @"With TemTable As 
                           (SELECT  ROW_NUMBER() OVER ( ORDER BY DictionaryId DESC ) AS 't' ,
                                    *
                            FROM    ( SELECT    DictionaryId ,
                                                DictionaryName
                                      FROM      dbo.BasisDictionary
                                      WHERE     DictionaryType = 8
                                                AND UseType = 0
                                                AND DictionaryState = 1
                                      UNION ALL
                                      SELECT    DictionaryId ,
                                                DictionaryName
                                      FROM      dbo.BasisDictionary
                                      WHERE     DictionaryType = 8
                                                AND UseType = 1
                                                AND CompanyId = 1
                                                AND DictionaryState = 1
                                    ) AS a
                        )
                              
                        Select * From TemTable Where t  Between @Size*(@Index-1)+1 And @Size*@Index";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };
            List<BasisDictionaryModel> list = new List<BasisDictionaryModel>();
            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                BasisDictionaryModel model = new BasisDictionaryModel();

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString()); // 字典Id

                model.DictionaryName = dr["DictionaryName"].ToString(); // 字典名称

                list.Add(model);// 加入到数据集
            }

            dr.Close();//关闭 

            return list;// 返回数据集
        }
        #endregion

        #region 分页总数 选择信息类型
        /// <summary>
        /// 分页总数 选择信息类型
        /// </summary>
        /// <returns></returns>
        public int ChooseMessageTypeCount()
        {
            string sql = @" SELECT  Count(DictionaryId)  FROM dbo.BasisDictionary WHERE DictionaryType=8 AND UseType=0   AND DictionaryState=1";

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
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion

        #endregion

        #region 分页列表 系统字典表

        /// <summary>
        /// 分页列表 系统字典表
        /// </summary>
        /// <param name="index">页面索引</param>
        /// <param name="size">页面条数</param>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public List<BasisDictionaryModel> BasisDictionaryPageList(int index, int size, string where)
        {
            string sql = @" With TemTable As 
                            ( 
                              Select
                                Row_Number() Over(Order By DictionaryId Desc) AS 't'
                                ,dic.DictionaryId
                                ,dic.DictionaryName
                                ,dic.DictionaryNumber
                                ,dic.DictionaryType
                                ,dic.Sort
                                ,dic.DictionaryState 
                                , CASE dic.DictionaryState
                                   WHEN '0' THEN '无效'
                                   WHEN '1' THEN '有效'
                                 END AS StateName
                                 , CASE dic.DictionaryType
                                   WHEN '0' THEN '供应商类别'
                                   WHEN '1' THEN '供应商种类'
                                   WHEN '2' THEN '合作层级'
                                   WHEN '3' THEN '供应商服务类型'
                                   WHEN '4' THEN '供应商状态'
                                   WHEN '5' THEN '仓储供应商附件'
                                   WHEN '6' THEN '仓储证件类型'
                                   WHEN '7' THEN '培养期望'
                                   WHEN '8' THEN '通知类型'
                                   WHEN '9' THEN '企业性质'
                                   WHEN '10' THEN '配送线路类型'
                                   WHEN '11' THEN '不合格品类型'
                                 END AS DictionaryTypeName
                                ,dic.UseType
                                , CASE dic.UseType
                                   WHEN '0' THEN '系统统一'
                                   WHEN '1' THEN '公司使用'
                                 END AS UseTypeName
                                ,dic.CompanyId
                                ,ISNULL(com.CompanyName,'无') As CompanyName 
                                From
                                BasisDictionary dic Left Join SysCompany com
                                on
                                dic.CompanyId=com.CompanyId    Where    " + where + @"
                            ) 
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";

            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };

            List<BasisDictionaryModel> list = new List<BasisDictionaryModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);

            while (dr.Read())
            {
                BasisDictionaryModel model = new BasisDictionaryModel();

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString()); //字典id

                model.DictionaryName = dr["DictionaryName"].ToString(); //字典名称 

                model.DictionaryNumber = dr["DictionaryNumber"].ToString(); //字典序号 

                model.DictionaryType = Convert.ToInt32(dr["DictionaryType"].ToString()); //字典类型 

                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //字典排序  

                model.DictionaryState = Convert.ToInt32(dr["DictionaryState"].ToString()); //字典状态 

                model.StateName = dr["StateName"].ToString(); // 显示状态

                model.UseType = Convert.ToInt32(dr["UseType"].ToString()); //使用类型

                model.DictionaryTypeName = dr["DictionaryTypeName"].ToString(); //字典类型名称 

                model.UseTypeName = dr["UseTypeName"].ToString(); //字典类型名称 

                if (dr["CompanyId"].ToString().Length > 0 && dr["CompanyId"].ToString() != "0")
                {
                    model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());//系统公司id

                    model.CompanyName = dr["CompanyName"].ToString(); //公司名称 
                }
                list.Add(model);// 加入到数据集
            }
            dr.Close();//关闭 
            return list;// 返回数据集
        }
        #endregion  

        #region 分页总数 系统字典表

        /// <summary>
        /// 分页总数 系统字典表
        /// </summary>
        /// <param name="where">检索条件</param>
        /// <returns></returns>
        public int BasisDictionaryPageCount(string where)
        {
            string sql = @" Select 
                                    Count(dic.DictionaryId) 
                            From
                                    BasisDictionary dic
                            Where
                                    " + where + "";
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, null);
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

        #region 获取实体 系统字典表

        /// <summary>
        /// 获取实体 系统字典表
        /// </summary>
        /// <param name="Id">系统字典</param>
        /// <returns></returns>
        public BasisDictionaryModel GetModelByID(string Id)
        {
            string sql = @" SELECT dic.[DictionaryId]
                                ,dic.[DictionaryName]
                                ,dic.[DictionaryNumber]
                                ,dic.[DictionaryType]
                                ,dic.[Sort]
                                ,dic.[DictionaryState]
                                ,dic.[UseType]
                                ,dic.[CompanyId]  
                               , CASE dic.DictionaryType
                                   WHEN '0' THEN '供应商类别'
                                   WHEN '1' THEN '供应商种类'
                                   WHEN '2' THEN '合作层级'
                                   WHEN '3' THEN '供应商服务类型'
                                   WHEN '4' THEN '供应商状态'
                                   WHEN '5' THEN '仓储供应商附件'
                                   WHEN '6' THEN '仓储证件类型'
                                   WHEN '7' THEN '培养期望'
                                   WHEN '8' THEN '通知类型'
                                   WHEN '9' THEN '企业性质'
                                   WHEN '10' THEN '配送线路类型'
                                   WHEN '11' THEN '不合格品类型'
                                 END AS DictionaryTypeName
                               , CASE dic.UseType
                                   WHEN '0' THEN '系统统一'
                                   WHEN '1' THEN '公司使用'
                                 END AS UseTypeName
                                ,ISNULL(com.CompanyName,'无') As CompanyName
                            FROM 
                                [dbo].[BasisDictionary] dic
                            LEFT JOIN 
                                SysCompany com
                            ON  
                                dic.CompanyId=com.CompanyId
                            WHERE
                                dic.[DictionaryId]=@DictionaryId";

            SqlParameter[] param ={
                                   new SqlParameter("@DictionaryId",Id)
                                  };

            BasisDictionaryModel model = null;

            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            }
            catch (Exception)
            {
                return null;
            }
            if (dr.Read())
            {
                model = new BasisDictionaryModel();

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString());// 字典id 

                model.DictionaryName = dr["DictionaryName"].ToString();// 字典名称 

                model.DictionaryNumber = dr["DictionaryNumber"].ToString();// 字典序号 

                model.DictionaryType = Convert.ToInt32(dr["DictionaryType"].ToString()); // 字典类型 

                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //字典排序  

                model.DictionaryState = Convert.ToInt32(dr["DictionaryState"].ToString()); //字典状态 

                model.UseType = Convert.ToInt32(dr["UseType"].ToString());//使用类型 

                if (dr["CompanyId"].ToString().Length > 0 && dr["CompanyId"].ToString() != "0")
                {
                    model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());//系统公司id 

                    model.CompanyName = dr["CompanyName"].ToString();   // 公司名称
                } 
                model.DictionaryTypeName = dr["DictionaryTypeName"].ToString(); //字典类型名称  

                model.UseTypeName = dr["UseTypeName"].ToString(); //使用类型名称 
            }

            dr.Close(); // 关闭

            return model;// 返回model
        }

        #endregion

        #region 变更 系统字典状态

        /// <summary>
        /// 变更 系统字典状态
        /// </summary>
        /// <param name="Id">系统字典id</param>
        /// <param name="State">系统字典状态</param>
        /// <returns></returns>
        public int ChangeState(string Id, int State)
        {
            string sql = @"Update 
                                  BasisDictionary
                          Set                     
                                  DictionaryState = @DictionaryState 
                          Where         
                                  DictionaryId = @DictionaryId
                         ";
            SqlParameter[] param ={

                new SqlParameter("@DictionaryId",Id) ,  // 字典ID 

                new SqlParameter("@DictionaryState",State) // 系统字典状态   
            };
            // 影响行数
            int row = 0;
            try
            {
                row = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            }
            catch (Exception)
            {
                return 0;
            }
            return row;
        }

        #endregion

        #region 是否存在相同的字典编号

        /// <summary>
        /// 是否存在相同的字典编号
        /// </summary>
        /// <param name="Number">字典编号</param>
        /// <param name="type">字典类型</param>
        /// <returns></returns>
        public int IsDictionaryNumber(string Number, int type)
        {
            var sql = @"Select Count(*) 
                           From BasisDictionary
                           Where DictionaryNumber=@DictionaryNumber  AND DictionaryType=@DictionaryType ";

            SqlParameter[] param ={
                                 new SqlParameter("@DictionaryNumber",Number),//字典编号

                                 new SqlParameter("@DictionaryType",type)//字典类型
                                 };
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, param);
            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }
        #endregion  

        #region 导出Excel数据

        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="tWhere">检索条件</param>
        /// <returns></returns>
        public List<BasisDictionaryModel> ExportData(string tWhere)
        {
            string sql = @"With TemTable As 
                        (
                            Select
                                Row_Number() Over(Order By DictionaryId Desc) AS 't'
                                ,dic.DictionaryId
                                ,dic.DictionaryName
                                ,dic.DictionaryNumber
                                ,dic.DictionaryType
                                ,dic.Sort
                                ,dic.DictionaryState 
                                , CASE dic.DictionaryState
                                   WHEN '0' THEN '无效'
                                   WHEN '1' THEN '有效'
                                 END AS StateName
                                 , CASE dic.DictionaryType
                                   WHEN '0' THEN '供应商类别'
                                   WHEN '1' THEN '供应商种类'
                                   WHEN '2' THEN '合作层级'
                                   WHEN '3' THEN '供应商服务类型'
                                   WHEN '4' THEN '供应商状态'
                                   WHEN '5' THEN '仓储供应商附件'
                                   WHEN '6' THEN '仓储证件类型'
                                   WHEN '7' THEN '培养期望'
                                   WHEN '8' THEN '通知类型'
                                   WHEN '9' THEN '企业性质'
                                   WHEN '10' THEN '配送线路类型'
                                   WHEN '11' THEN '不合格品类型'
                                 END AS DictionaryTypeName
                                ,dic.UseType
                                , CASE dic.UseType
                                   WHEN '0' THEN '系统统一'
                                   WHEN '1' THEN '公司使用'
                                 END AS UseTypeName
                                ,dic.CompanyId
                                ,ISNULL(com.CompanyName,'无') As CompanyName 
                                From
                                BasisDictionary dic Left Join SysCompany com
                                on
                                dic.CompanyId=com.CompanyId    Where    " + tWhere + @"
                        ) 
                        Select * From TemTable ";
            List<BasisDictionaryModel> list = new List<BasisDictionaryModel>();   // 实例化数据集
            // 从数据库读取数据
            SqlDataReader dr = null;
            try
            {
                dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            }
            catch (Exception)
            {
                return null;
            }
            // 循环数据加入到数据集中
            while (dr.Read())
            {
                BasisDictionaryModel model = new BasisDictionaryModel();

                model.DictionaryId = Convert.ToInt32(dr["DictionaryId"].ToString()); //字典id

                model.DictionaryName = dr["DictionaryName"].ToString(); //字典名称 

                model.DictionaryNumber = dr["DictionaryNumber"].ToString(); //字典序号 

                model.DictionaryType = Convert.ToInt32(dr["DictionaryType"].ToString()); //字典类型 

                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //字典排序  

                model.DictionaryState = Convert.ToInt32(dr["DictionaryState"].ToString()); //字典状态 

                model.StateName = dr["StateName"].ToString(); // 显示状态

                model.UseType = Convert.ToInt32(dr["UseType"].ToString()); //使用类型

                model.DictionaryTypeName = dr["DictionaryTypeName"].ToString(); //字典类型名称 

                model.UseTypeName = dr["UseTypeName"].ToString(); //字典类型名称 

                if (dr["CompanyId"].ToString().Length > 0 && dr["CompanyId"].ToString() != "0")
                {
                    model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());//系统公司id

                    model.CompanyName = dr["CompanyName"].ToString(); //公司名称 
                }
                list.Add(model);// 加入到数据集
            }

            dr.Close();// 关闭 

            return list;// 返回数据集
        }
        #endregion

        #region 获取字典表中的字典类型数据
         
        /// <summary>
        /// 获取字典表中的字典类型数据
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <returns></returns>
        public List<Dicts> GetDictLists(int companyid)
        { 
            var sql = @"SELECT  ROW_NUMBER() OVER ( ORDER BY DictionaryId DESC ) AS 't' ,
                         * 
                           FROM    ( SELECT    DictionaryId ,
                                     DictionaryName,DictionaryType
                           FROM      dbo.BasisDictionary
                           WHERE     DictionaryType = 8
                                     AND UseType = 0
                                     AND DictionaryState = 1
                           UNION ALL
                           SELECT    DictionaryId ,
                                     DictionaryName,DictionaryType
                           FROM      dbo.BasisDictionary
                           WHERE     DictionaryType = 8
                                     AND UseType = 1
                                     AND CompanyId = @CompanyId
                                     AND DictionaryState = 1
                          ) AS a";
            SqlParameter[] parm ={
                                    new SqlParameter("@CompanyId",companyid)
                                };

            List<Dicts> list = new List<Dicts>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, parm);

            while (dr.Read())
            {
                Dicts dict = new Dicts();
                 
                dict.Id = dr[1].ToString();//字典id

                dict.Name = dr[2].ToString();//字典名称

                dict.Type = Convert.ToInt32(dr[3].ToString());//字典类型

                list.Add(dict);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 获取字典表中的字典类型数据

        /// <summary>
        /// 获取字典表中的字典类型数据
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <returns></returns>
        public List<Dicts> GetWorkingOrderlist(int companyid)
        {
            var sql = @"SELECT  ROW_NUMBER() OVER ( ORDER BY DictionaryId DESC ) AS 't' ,
                         * 
                           FROM    ( SELECT    DictionaryId ,
                                     DictionaryName,DictionaryType
                           FROM      dbo.BasisDictionary
                           WHERE     DictionaryType = 11
                                     AND UseType = 0
                                     AND DictionaryState = 1
                           UNION ALL
                           SELECT    DictionaryId ,
                                     DictionaryName,DictionaryType
                           FROM      dbo.BasisDictionary
                           WHERE     DictionaryType = 11
                                     AND UseType = 1
                                     AND CompanyId = @CompanyId
                                     AND DictionaryState = 1
                          ) AS a";
            SqlParameter[] parm ={
                                    new SqlParameter("@CompanyId",companyid)
                                };

            List<Dicts> list = new List<Dicts>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, parm);

            while (dr.Read())
            {
                Dicts dict = new Dicts();

                dict.Id = dr[1].ToString();//字典id

                dict.Name = dr[2].ToString();//字典名称

                dict.Type = Convert.ToInt32(dr[3].ToString());//字典类型

                list.Add(dict);
            }
            dr.Close();

            return list;
        }
        #endregion
    }
}
