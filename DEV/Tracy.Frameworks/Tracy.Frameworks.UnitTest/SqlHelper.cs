using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.UnitTest
{
    /// <summary>
    /// 数据访问层
    /// </summary>
    public class SqlHelper : IDisposable
    {
        #region 初始化
        private readonly IDbConnection _conn;

        public SqlHelper(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
        }

        public SqlHelper(IDbConnection conn)
        {
            _conn = conn;
        }
        #endregion

        #region 批量插入
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="dt">源数据</param>
        public void BulkCopy(DataTable dt, string tableName)
        {
            using (_conn)
            {
                if (_conn.State != ConnectionState.Open)
                    _conn.Open();

                using (var sqlbulkcopy = new SqlBulkCopy((SqlConnection)_conn))
                {
                    sqlbulkcopy.DestinationTableName = tableName;
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                    }
                    sqlbulkcopy.WriteToServer(dt);
                }
            }
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="list">源数据</param>
        public void BulkCopy<T>(List<T> list, string tableName)
        {
            var dt = list.ToDataTable();

            BulkCopy(dt, tableName);
        }
        #endregion

        #region 取列表

        /// <summary>
        /// 取列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="iDbTransaction"></param>
        /// <returns></returns>
        public List<TResult> List<TResult>(string sql, object param, IDbTransaction iDbTransaction = null)
        {
            var tResult = _conn.Query<TResult>(sql, param, iDbTransaction).ToList();
            return tResult;
        }
        #endregion

        #region 取单条数据

        /// <summary>
        /// 取单条数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="iDbTransaction"></param>
        /// <returns></returns>
        public TResult Get<TResult>(string sql, object param, IDbTransaction iDbTransaction = null)
        {
            var tResult = _conn.QuerySingleOrDefault<TResult>(sql, param, iDbTransaction);
            return tResult;
        }
        #endregion

        #region 执行Sql

        /// <summary>
        /// 执行Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="iDbTransaction"></param>
        /// <returns></returns>
        public int Execute(string sql, object param, IDbTransaction iDbTransaction = null)
        {
            return _conn.Execute(sql, param, iDbTransaction);
        }
        #endregion

        #region 释放资源
        public void Dispose()
        {
            if (_conn != null)
                _conn.Dispose();
        }
        #endregion
    }
}
