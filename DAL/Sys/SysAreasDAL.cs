//-------------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2018 
//-------------------------------------------------------------------------
//作成日　　    版本　　　作成者　　　meto
//2018/06/23    1.0        MH        
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
 * 类名：SysAreasDAL
 * 功能描述：区域基础表 数据访问层类
 * ******************************/
namespace DAL.Sys
{
    public class SysAreasDAL
    {
        // 获取连接串
        string conn = DBUtility.ConnectionStringInfo.ConnectionString().ToString();


        #region 添加 区域基础表
        /// <summary>
        /// 添加 区域基础表
        /// </summary>
        public int AddSysAreas(SysAreasModel model)
        {
            string sql = @"Insert 
                                  SysAreas
                                  (
                                        AreaName ,
                                        ParentId ,
                                        Sort ,
                                        AreaCode ,
                                        State ,
                                        CreateTime 
                                      
                                  )
                          Values
                                  (
                                        @AreaName ,
                                        @ParentId ,
                                        @Sort ,
                                        @AreaCode ,
                                        @State ,
                                        @CreateTime 
                                  )";
            SqlParameter[] param ={
                                        new SqlParameter("@AreaName",model.AreaName) ,  //位置名称   
                                        new SqlParameter("@ParentId",model.ParentId) ,  //父节点id   
                                        new SqlParameter("@Sort",model.Sort) ,  //排序   
                                        new SqlParameter("@AreaCode",model.AreaCode) ,  //行政编码   
                                        new SqlParameter("@State",model.State) ,  //状态：0-无效；1-有效   
                                        new SqlParameter("@CreateTime",model.CreateTime)   //创建时间   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 修改 区域基础表
        /// <summary>
        /// 修改 区域基础表
        /// </summary>
        public int UpdateSysAreas(SysAreasModel model)
        {
            string sql = @"Update 
                                  SysAreas
                          Set
                                  AreaName = @AreaName ,
                                  ParentId = @ParentId ,
                                  AreaCode = @AreaCode ,
                                  Sort = @Sort 
                          Where         
                                  AreaId = @AreaId"
 ;

            SqlParameter[] param ={
                                        new SqlParameter("@AreaId",model.AreaId) ,  //位置id   
                                        new SqlParameter("@AreaName",model.AreaName) ,  //位置名称   
                                        new SqlParameter("@ParentId",model.ParentId) ,  //父节点id   
                                        new SqlParameter("@Sort",model.Sort) ,  //排序   
                                        new SqlParameter("@AreaCode",model.AreaCode) ,  //行政编码                        
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
                              SELECT * FROM SysAreas
                               WHERE AreaId = @ID  
                               UNION ALL
                              SELECT SysAreas.*    
                                FROM cteTree INNER JOIN SysAreas
                                  ON cteTree.AreaId = SysAreas.ParentId
                             ) 
                              UPDATE dbo.SysAreas 
                                 SET [State]=@State 
                                FROM dbo.SysAreas s,cteTree
                               WHERE s.AreaId=cteTree.AreaId 
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@ID",did) ,  //用户id自增主键    
                                        new SqlParameter("@State",state)   //状态：0-无效;1-有效   
                       
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion

        #region 删除 区域基础表
        /// <summary>
        /// 删除 区域基础表
        /// </summary>
        public int DeleteSysAreasByID(string id)
        {
            string sql = @"  WITH cteTree  AS
                            (    
                              SELECT * FROM SysAreas
                               WHERE AreaId = @ID  
                               UNION ALL
                              SELECT SysAreas.*    
                                FROM cteTree INNER JOIN SysAreas
                                  ON cteTree.AreaId = SysAreas.ParentId
                             ) 
                            Delete dbo.SysAreas 
                                FROM dbo.SysAreas s,cteTree
                               WHERE s.AreaId=cteTree.AreaId 
 ";

            SqlParameter[] param ={
                                        new SqlParameter("@ID",id)
                                  };
            int i = SQLHelper.ExecuteNonQuery(conn, CommandType.Text, sql, param);
            return i;
        }
        #endregion 

        #region 分页列表 区域基础表
        /// <summary>
        ///  分页列表 区域基础表
        /// </summary>
        public List<SysAreasModel> SysAreasPageList(int index, int size, string where)
        {
            string sql = @" With TemTable As 
                            (
                                Select Row_Number() Over(Order By MakerTime) AS 't',*
                                From
                                       SysAreas
                                Where
                                       " + where + @"
                            )
                                  
                            Select * From TemTable Where t Between @Size*(@Index-1)+1 And @Size*@Index ";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };

            List<SysAreasModel> list = new List<SysAreasModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            while (dr.Read())
            {
                SysAreasModel model = new SysAreasModel();
                model.AreaId = Convert.ToInt32(dr["AreaId"].ToString()); //位置id
                model.AreaName = dr["AreaName"].ToString(); //位置名称
                model.ParentId = Convert.ToInt32(dr["ParentId"].ToString()); //父节点id
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序
                model.AreaCode = Convert.ToInt32(dr["AreaCode"].ToString()); //行政编码
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效；1-有效
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion  

        #region 分页总数 区域基础表
        /// <summary>
        ///  分页总数 区域基础表
        /// </summary>
        public int SysAreasPageCount(string where)
        {
            string sql = @" Select 
                                    Count(*) 
                            From
                                    SysAreas
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

        #region 获取实体 区域基础表
        /// <summary>
        ///  获取实体 区域基础表
        /// </summary>
        public SysAreasModel GetModelByID(string id)
        {
            string sql = @" Select 
                                  AreaId ,
                                  AreaName ,
                                  ParentId ,
                                  Sort ,
                                  AreaCode ,
                                  State ,
                                  CreateTime 
                            From
                                  SysAreas
                            Where
                                  AreaId=@AreaId";
            SqlParameter[] param ={
                                   new SqlParameter("@AreaId",id)
                                  };

            SysAreasModel model = null;

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, param);
            if (dr.Read())
            {
                model = new SysAreasModel();
                model.AreaId = Convert.ToInt32(dr["AreaId"].ToString()); //位置id
                model.AreaName = dr["AreaName"].ToString(); //位置名称
                model.ParentId = Convert.ToInt32(dr["ParentId"].ToString()); //父节点id
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序
                model.AreaCode = Convert.ToInt32(dr["AreaCode"].ToString()); //行政编码
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效；1-有效
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间
            }
            dr.Close();

            return model;
        }

        public List<SysAreasModel> AreasList(string keyword)
        {
            string sql = @" Select 
                                  AreaId ,
                                  AreaName ,
                                  ParentId ,
                                  Sort ,
                                  AreaCode ,
                                  State ,
                                  CreateTime 
                            From
                                  SysAreas
                            Where
                                  AreaName like '%" + keyword + "%'";


            List<SysAreasModel> list = new List<SysAreasModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            while (dr.Read())
            {
                SysAreasModel model = new SysAreasModel();
                model.AreaId = Convert.ToInt32(dr["AreaId"].ToString()); //位置id
                model.AreaName = dr["AreaName"].ToString(); //位置名称
                model.ParentId = Convert.ToInt32(dr["ParentId"].ToString()); //父节点id
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序
                model.AreaCode = Convert.ToInt32(dr["AreaCode"].ToString()); //行政编码
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效；1-有效
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间

                list.Add(model);
            }
            dr.Close();

            return list;
        }

        public List<SysAreasModel> ALLAreasList()
        {
            string sql = @" Select 
                                  AreaId ,
                                  AreaName ,
                                  ParentId ,
                                  Sort ,
                                  AreaCode ,
                                  State ,
                                  CreateTime 
                            From
                                  SysAreas";


            List<SysAreasModel> list = new List<SysAreasModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, null);
            while (dr.Read())
            {
                SysAreasModel model = new SysAreasModel();
                model.AreaId = Convert.ToInt32(dr["AreaId"].ToString()); //位置id
                model.AreaName = dr["AreaName"].ToString(); //位置名称
                model.ParentId = Convert.ToInt32(dr["ParentId"].ToString()); //父节点id
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序
                model.AreaCode = Convert.ToInt32(dr["AreaCode"].ToString()); //行政编码
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效；1-有效
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间

                list.Add(model);
            }
            dr.Close();

            return list;
        }

        public List<SysAreasModel> AreaListByParentId(int pid)
        {
            string sql = @" Select 
                                  AreaId ,
                                  AreaName ,
                                  ParentId ,
                                  Sort ,
                                  AreaCode ,
                                  State ,
                                  CreateTime 
                            From
                                  SysAreas Where ParentId=@ParentId ";

            SqlParameter[] parm =
            {
                new SqlParameter("@ParentId",pid)
            };

            List<SysAreasModel> list = new List<SysAreasModel>();

            SqlDataReader dr = SQLHelper.ExecuteReader(conn, CommandType.Text, sql, parm);
            while (dr.Read())
            {
                SysAreasModel model = new SysAreasModel();
                model.AreaId = Convert.ToInt32(dr["AreaId"].ToString()); //位置id
                model.AreaName = dr["AreaName"].ToString(); //位置名称
                model.ParentId = Convert.ToInt32(dr["ParentId"].ToString()); //父节点id
                model.Sort = Convert.ToInt32(dr["Sort"].ToString()); //排序
                model.AreaCode = Convert.ToInt32(dr["AreaCode"].ToString()); //行政编码
                model.State = Convert.ToInt32(dr["State"].ToString()); //状态：0-无效；1-有效
                model.CreateTime = Convert.ToDateTime(dr["CreateTime"].ToString()); //创建时间

                list.Add(model);
            }
            dr.Close();

            return list;
        }
        #endregion

        #region 是否存在相同区域
        /// <summary>
        /// 检查相同项
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="pid">父类编号</param>
        /// <param name="name">区域名称</param>
        /// <returns></returns>
        public int IsHasAreas(int id, int pid, string name)
        {
            string sql = string.Empty;

            if (id == 0)
            {
                sql = "Select Count(*) From SysAreas Where  ParentId=@ParentId And AreaName=@AreaName";

                SqlParameter[] param ={
                                   new SqlParameter("@ParentId",pid),
                                   new SqlParameter("@AreaName",name.Trim())
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
                sql = "Select Count(*) From SysAreas Where ParentId=@ParentId And AreaName=@AreaName And AreaId!=@AreaId";

                SqlParameter[] param ={
                                   new SqlParameter("@ParentId",pid),
                                   new SqlParameter("@AreaName",name.Trim()),
                                   new SqlParameter("@AreaId",id)
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




        #region VArea 获取位置信息

        public int VAreasRowCount(string where)
        {
            string sql = " SELECT COUNT(1)  FROM dbo.VAreas  " + where;
            object obj = SQLHelper.ExecuteScalar(conn, CommandType.Text, sql, null);
            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj.ToString());
        }
        public List<VAreas> VAreasLiset(int index, int size, string where)
        {
            List<VAreas> list = new List<VAreas>();
            string sql = " WITH EmployeePage AS (     SELECT ROW_NUMBER() OVER(ORDER BY px) AS RowNumber,AreaId,AreaName,ParentId,Sort,AreaCode,State,CreateTime,area01,areaname01,area02,areaname02,area03,areaname03,area04,areaname04    FROM dbo.VAreas      " + where + ")   SELECT* FROM EmployeePage WHERE RowNumber >= @Size * (@Index - 1) + 1 AND RowNumber< @Size * @Index";
            SqlParameter[] param ={
                                   new SqlParameter("@Size",size),
                                   new SqlParameter("@Index",index)
                                  };
            DataSet ds = SQLHelper.GetDataSet(conn, CommandType.Text, sql, param);//获取查询对象
            if (ds != null)
            {

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    VAreas model = DataRowToModel(row);
                    if (model != null)
                        list.Add(model);
                }
            }
            ds.Clear();//清理所有数据
            return list;

        }

        private VAreas DataRowToModel(DataRow dr)
        {
            Model.Sys.VAreas model = null;
            if (dr != null)
            {
                model = new Model.Sys.VAreas();
                int tryInt;
                if (dr.Table.Columns.Contains("AreaId"))
                {
                    if (int.TryParse(dr["AreaId"].ToString(), out tryInt))
                    {
                        model.AreaId = tryInt;
                    }
                }
                if (dr.Table.Columns.Contains("AreaName"))
                {
                    model.AreaName = dr["AreaName"].ToString();
                }
                if (dr.Table.Columns.Contains("ParentId"))
                {
                    if (int.TryParse(dr["ParentId"].ToString(), out tryInt))
                    {
                        model.ParentId = tryInt;
                    }
                }
                if (dr.Table.Columns.Contains("AreaCode"))
                {

                    model.AreaCode = dr["AreaCode"].ToString();
                }
                if (dr.Table.Columns.Contains("area01"))
                {
                    if (int.TryParse(dr["area01"].ToString(), out tryInt))
                    {
                        model.area01 = tryInt;
                    }
                }
                if (dr.Table.Columns.Contains("areaname01"))
                { 
                        model.areaname01 = dr["areaname01"].ToString(); 
                }
                if (dr.Table.Columns.Contains("area01"))
                {
                    if (int.TryParse(dr["area02"].ToString(), out tryInt))
                    {
                        model.area02 = tryInt;
                    }
                }
                if (dr.Table.Columns.Contains("areaname02"))
                {
                    model.areaname02 = dr["areaname02"].ToString();
                }
                if (dr.Table.Columns.Contains("area03"))
                {
                    if (int.TryParse(dr["area03"].ToString(), out tryInt))
                    {
                        model.area03 = tryInt;
                    }
                }
                if (dr.Table.Columns.Contains("areaname03"))
                {
                    model.areaname03 = dr["areaname03"].ToString();
                }
                if (dr.Table.Columns.Contains("area04"))
                {
                    if (int.TryParse(dr["area04"].ToString(), out tryInt))
                    {
                        model.area04 = tryInt;
                    }
                }
                if (dr.Table.Columns.Contains("areaname04"))
                {
                    model.areaname04 = dr["areaname04"].ToString();
                }  
            }
            return model;
        }

        #endregion
    }
}

