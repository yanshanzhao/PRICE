using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace DBUtility
{
    public abstract class SQLHelper
    {
        //SQL Server数据库的连接字符串
        //public static readonly string SQLServerConnStr = ConfigurationManager.ConnectionStrings["SQLServerConnStr"].ConnectionString;
        //哈希表用来储存缓存的参数，它可以储存任何的类型参数
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        ///执行一个不需要返回值的SqlCommand命令，通过指定专用的连接字符串。
        /// 使用参数数组形式提供参数列表 
        /// </summary>
        /// <remarks>
        /// 使用示例：
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            int val = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                   val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                   
                }
                catch (Exception ex)
                {
                    return -1;
                }
                return val;
            }
        }

        /// <summary>
        /// 返回SQL执行影响行数
        /// </summary>
        /// <param name="connString">一个有效的数据库连接字符串</param>
        /// <param name="acntStr"></param>
        /// <param name="arrLstParams"></param>
        /// <param name="isStProc"></param>
        /// <param name="aconn"></param>
        /// <returns></returns>
        public static int ExecuteCmd(string connString, string acntStr = null,
    ArrayList arrLstParams = null, bool isStProc = false, SqlConnection aconn = null)
        {
            int rows = 0;
            if (!string.IsNullOrEmpty(acntStr))
            {
                using (SqlConnection conn = new SqlConnection(acntStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(connString, conn);
                    if (arrLstParams != null) //如果是null不能AddRange
                    {
                        cmd.Parameters.AddRange(arrLstParams.ToArray(typeof(SqlParameter)));
                    }
                    if (isStProc)
                        cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        rows = cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                SqlCommand cmd = new SqlCommand(connString, aconn);
                if (arrLstParams != null) //如果是null不能AddRange
                {
                    cmd.Parameters.AddRange(arrLstParams.ToArray(typeof(SqlParameter)));
                }
                if (isStProc)
                    cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    rows = cmd.ExecuteNonQuery();
                }
                finally
                {
                    //conn.Close();
                }
            }
            return rows;
        }

        /// <summary>
        ///执行一条不返回结果的SqlCommand，通过一个已经存在的数据库连接 
        /// 使用参数数组提供参数
        /// </summary>
        public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// 执行一条不返回结果的SqlCommand，通过一个已经存在的数据库事务处理 
        /// 使用参数数组提供参数
        /// </summary>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// 执行一条返回结果集的SqlCommand命令，通过专用的连接字符串。
        /// 使用参数数组提供参数
        /// </summary>
        public static SqlDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connString);
            // 在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return reader;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /// <summary>
        /// 执行一条返回第一条记录第一列的SqlCommand命令，通过专用的连接字符串。 
        /// 使用参数数组提供参数
        /// </summary>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connString))
            {

                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }
        /// <summary>
        /// 执行一条返回第一条记录第一列的SqlCommand命令，通过已经存在的数据库连接。
        /// 使用参数数组提供参数
        /// </summary>
        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行一条返回第一条记录第一列的SqlCommand命令，通过已经存在的数据库连接。
        /// 使用参数数组提供参数
        /// </summary>
        public static object ExecuteScalar(SqlTransaction tran, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, tran.Connection, tran, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// 返回数据集集合，可返回多个表，通过专用的连接字符串
        /// </summary>
        public static DataSet GetDataSet(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }
        /// <summary>
        /// 返回数据集集合，可返回多个表，通过已经存在的数据库连接
        /// </summary>
        public static DataSet GetDataSet(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }
        /// <summary>
        /// 缓存参数数组
        /// </summary>
        /// <param name="cacheKey">参数缓存的键值</param>
        /// <param name="cmdParms">被缓存的参数列表</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }
        /// <summary>
        /// 获取被缓存的参数
        /// </summary>
        /// <param name="cacheKey">用于查找参数的KEY值</param>
        /// <returns>返回缓存的参数数组</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];
            if (cachedParms == null)
                return null;
            //新建一个参数的克隆列表
            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];
            //通过循环为克隆参数列表赋值
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                //使用clone方法复制参数列表中的参数
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms).Clone();
            return clonedParms;
        }
        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">SqlCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, params  SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.CommandText = cmdText;
            cmd.Connection = conn;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }


        #region 执行简单的SQL语句
     

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction事件</param>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(SqlConnection connection, SqlTransaction trans, string SQLString)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.Transaction = trans;
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction对象</param>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(SqlConnection connection, SqlTransaction trans, string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, trans, SQLString, cmdParms);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
            }
        }

      
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction事务</param>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(SqlConnection connection, SqlTransaction trans, string SQLString)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                command.SelectCommand.Transaction = trans;
                command.Fill(ds, "ds");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }

       
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction事务</param>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(SqlConnection connection, SqlTransaction trans, string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, trans, SQLString, cmdParms);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    //trans.Rollback();
                    throw e;
                }
            }
        }
        #endregion

        #region 执行带参数的SQL语句 
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        #endregion
    }
}
