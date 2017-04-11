using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Services
{
    public static class SqlConn
    {
        private readonly static string sqlconnection = System.Configuration.ConfigurationSettings.AppSettings["SqlCon"].ToString();
        private readonly static string testsqlconnection = System.Configuration.ConfigurationSettings.AppSettings["TestSqlCon"].ToString();

        /// <summary>
        /// FaoPS 
        /// </summary>
        /// <returns></returns>
        public static SqlConnection OpenConnectionPS()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            connection.Open();
            return connection;
        }

        public static SqlConnection OpenTestConnection()
        {
            SqlConnection connection = new SqlConnection(testsqlconnection);
            connection.Open();
            return connection;
        }

    }
}
