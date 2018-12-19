using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DBUtility
{
    public class ConnectionStringInfo
    {
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string ConnectionString()
        {
            string str = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            return str;
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string ConnectionStrings()
        {
            string str = ConfigurationManager.ConnectionStrings["connectionStrings"].ToString();
            return str;
        }
        #region 根据key值获取连接串
        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName">连接字符串key值</param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigurationManager.AppSettings[configName];

            return connectionString;
        }
        #endregion
    }
}
