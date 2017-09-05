using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Helpers
{
    /// <summary>
    /// sql helper，封装ado.net操作
    /// </summary>
    public class SqlHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string connStr
        {
            get { return ConfigHelper.GetAppSetting("ConnStr"); }
        }

        public SqlHelper() { }

        /// <summary>
        /// 执行增，删，改【常用】
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType"></param>
        /// <param name="commandText">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            int result = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, commandType, commandText, paras);
                result = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();//清空参数
            }
            return result;
        }

        /// <summary>
        /// 执行增删改（对现有的数据库连接）【不常用】
        /// </summary>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, commandType, commandText, paras);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行增，删，改(多条sql语句list+事务)【常用】
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="listSql">sql语句集合</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, List<string> listSql)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            int count = 0;
            PrepareCommand(cmd, conn, trans, CommandType.Text, null, null);
            try
            {
                for (int n = 0; n < listSql.Count; n++)
                {
                    string strSql = listSql[n];
                    if (strSql.Trim().Length > 1)
                    {
                        cmd.CommandText = strSql;
                        count += cmd.ExecuteNonQuery();
                    }
                }
                trans.Commit();
                cmd.Parameters.Clear();
            }
            catch
            {
                trans.Rollback();
                cmd.Parameters.Clear();
                return 0;
            }
            finally
            {
                conn.Close();
                if (trans != null)
                    trans.Dispose();
            }
            return count;
        }

        /// <summary>
        /// 执行增，删，改(多条sql语句hashtable+事务)【常用】
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="sqlStringList">sql语句集合</param>
        public static void ExecuteNonQuery(string connectionString, Hashtable sqlStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        foreach (DictionaryEntry item in sqlStringList)
                        {
                            string cmdText = item.Key.ToString();   //要执行的sql语句
                            SqlParameter[] cmdParas = (SqlParameter[])item.Value;  //sql语句对应的参数
                            PrepareCommand(cmd, conn, trans, CommandType.Text, cmdText, cmdParas);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        if (sqlStringList.Count > 0)
                            trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (trans != null)
                            trans.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 返回第一行第一列信息（可能是字符串 所以返回类型是object）【常用】
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型(存储过程或sql语句)</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns>结果集中第一行的第一列</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            object result = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, commandType, commandText, paras);
                result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            return result;
        }

        /// <summary>
        /// 返回第一行第一列信息（针对现有的数据库连接）【不常用】
        /// </summary>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, commandType, commandText, paras);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns>datatable数据表</returns>
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable dt = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, commandType, commandText, paras);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    dt = new DataTable();
                    da.Fill(dt);
                }
            }
            return dt;
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns>dataset结果集</returns>
        public static DataSet ExecuteDataSet(string connectionString, CommandType commandType, string commandText, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, commandType, commandText, paras);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    ds = new DataSet();
                    da.Fill(ds);
                }
            }
            return ds;
        }

        /// <summary>
        /// 返回只读数据集
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cmdText">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReader(string connectionString, CommandType commandType, string cmdText, params SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                PrepareCommand(cmd, conn, null, commandType, cmdText, paras);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            catch
            {
                conn.Close();
                throw;
            }
            return reader;
        }

        /// <summary>
        /// 准备一个待执行的SqlCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="paras"></param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType commandType, string commandText, params SqlParameter[] paras)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Close();
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandType = commandType;     //这里设置执行的是T-Sql语句还是存储过程
            if (commandText != null)
                cmd.CommandText = commandText;
            if (trans != null)
                cmd.Transaction = trans;
            if (paras != null && paras.Length > 0)
            {
                //预处理SqlParameter参数数组，将为NULL的参数赋值为DBNull.Value;
                foreach (SqlParameter parameter in paras)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                }
                cmd.Parameters.AddRange(paras);
            }
        }

        /// <summary>
        /// 关键字过滤
        /// </summary>
        /// <param name="originalString">原始字符串</param>
        /// <returns>返回True就是找到了可能sql注入的关键字</returns>
        public static bool IsFilterKeyWords(string originalString)
        {
            //参考：technet.microsoft.com/zh-cn/library/ms161953.aspx
            if (originalString.IndexOf(";") != -1 || originalString.IndexOf("'") != -1 || originalString.IndexOf("--") != -1 || originalString.IndexOf("/*") != -1 || originalString.IndexOf("*/") != -1 || originalString.IndexOf("xp_cmdshell") != -1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取分页数据（单表分页）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">要取的列名（逗号分开）</param>
        /// <param name="order">排序</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="where">查询条件</param>
        /// <param name="totalCount">总记录数</param>
        public static DataTable GetPager(string tableName, string columns, string order, int pageSize, int pageIndex, string where, out int totalCount)
        {
            SqlParameter[] paras = { 
                                   new SqlParameter("@tablename",SqlDbType.VarChar,100),
                                   new SqlParameter("@columns",SqlDbType.VarChar,500),
                                   new SqlParameter("@order",SqlDbType.VarChar,100),
                                   new SqlParameter("@pageSize",SqlDbType.Int),
                                   new SqlParameter("@pageIndex",SqlDbType.Int),
                                   new SqlParameter("@where",SqlDbType.VarChar,2000),
                                   new SqlParameter("@totalCount",SqlDbType.Int)
                                   };
            paras[0].Value = tableName;
            paras[1].Value = columns;
            paras[2].Value = order;
            paras[3].Value = pageSize;
            paras[4].Value = pageIndex;
            paras[5].Value = where;
            paras[6].Direction = ParameterDirection.Output;   //输出参数

            DataTable dt = SqlHelper.ExecuteDataTable(SqlHelper.connStr, CommandType.StoredProcedure, "sp_Pager", paras);
            totalCount = Convert.ToInt32(paras[6].Value);   //赋值输出参数，即当前记录总数
            return dt;
        }
    }
}
