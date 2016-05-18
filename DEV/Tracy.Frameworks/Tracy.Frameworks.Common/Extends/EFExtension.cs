using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data;
using System.Data.Entity;
using Tracy.Frameworks.Common.Result;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/7/16 10:02:51
 * 描述说明：EF的扩展
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Extends
{
    public static class EFExtension
    {
        /// <summary>
        /// 当EF自动代理类关闭时，在结果集中包含直接被查询对象的指定导航属性的数据
        /// </summary>
        /// <typeparam name="T">被查询对象的类型</typeparam>
        /// <param name="dbSet">EF对象集</param>
        /// <param name="exp">指定导航属性的属性表达式，可以包含First、FirstOrDefault等Lambda方法</param>
        /// <returns>返回包含指定导航属性数据的查询集</returns>
        public static IQueryable<T> Include<T>(this IDbSet<T> dbSet, Expression<Func<T, dynamic>> exp)
            where T : class
        {
            return dbSet.Include(GetStrForInclude(exp));
        }

        /// <summary>
        /// 当EF自动代理类关闭时，在结果集中包含直接被查询对象的指定导航属性的数据
        /// </summary>
        /// <typeparam name="T">被查询对象的类型</typeparam>
        /// <param name="query">原始查询集</param>
        /// <param name="exp">指定导航属性的属性表达式，可以包含First、FirstOrDefault等Lambda方法</param>
        /// <returns>返回包含指定导航属性数据的查询集</returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> query, Expression<Func<T, dynamic>> exp)
            where T : class
        {
            return query.Include(GetStrForInclude(exp));
        }

        private static string GetStrForInclude<T>(Expression<Func<T, dynamic>> exp)
            where T : class
        {
            string str = string.Empty;
            Expression expression = exp.Body;
            if (!(expression is MemberExpression))
            {
                throw new ArgumentException("Must be 'MemberExpression'.");
            }
            while (expression is MemberExpression || expression is MethodCallExpression)
            {
                var memberExp = expression as MemberExpression;
                if (memberExp != null)
                {
                    str = string.Concat(memberExp.Member.Name, ".", str);
                    expression = memberExp.Expression;
                }
                else
                {
                    var callExp = expression as MethodCallExpression;
                    if (callExp == null || callExp.Arguments == null || callExp.Arguments.Count == 0)
                    {
                        throw new ArgumentException("Not a right format expression.");
                    }
                    expression = callExp.Arguments[0];
                }
            }
            //if (!string.IsNullOrEmpty(str)) 
            //    str = str.Substring(str.Split('.')[0].Length).TrimStart('.').TrimEnd('.');
            return str.TrimEnd('.');
        }

        /// <summary>
        /// 集合翻页
        /// </summary>
        /// <typeparam name="T">source 中的元素的类型</typeparam>
        /// <param name="source">要翻页的IEnumerable</param>
        /// <param name="totalCount">輸出總記錄數</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>指定页子集合</returns>
        public static IQueryable<T> Paging<T>(this IQueryable<T> source, out int totalCount, int pageIndex = 1, int pageSize = 20)
        {
            totalCount = source.Count();
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 20;
            return source.Skip(((pageIndex - 1) * pageSize) < totalCount ? ((pageIndex - 1) * pageSize) : totalCount - (totalCount % pageSize)).Take(pageSize);
        }

        /// <summary>
        /// 集合翻页
        /// </summary>
        /// <typeparam name="T">source 中的元素的类型</typeparam>
        /// <param name="source">要翻页的IEnumerable</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>指定页子集合</returns>
        public static PagingResult<T> Paging<T>(this IQueryable<T> source, int pageIndex = 1, int pageSize = 20)
        {
            var totalCount = source.Count();
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 20;
            return new PagingResult<T>(totalCount, pageIndex, pageSize,
                source.Skip(((pageIndex - 1) * pageSize) < totalCount ? ((pageIndex - 1) * pageSize) : totalCount - (totalCount % pageSize)).Take(pageSize));
        }

        /// <summary>
        /// 基于谓词筛选特定对象
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型</typeparam>
        /// <param name="source">要筛选的 System.Data.Entity.DbSet</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数</param>
        /// <returns>source类型的对象，若未找到任何对象，返回空值</returns>
        [Obsolete("請直接用.FirstOrDefault(p=>p.123==567)")]
        public static TSource Find<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
            where TSource : class
        {
            return source.Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// 基于谓词筛选特点对象
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型</typeparam>
        /// <param name="source">要筛选的 System.Data.Entity.DbSet</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数</param>
        /// <returns>source类型的对象，若未找到任何对象，返回空值</returns>
        [Obsolete("請直接用.FirstOrDefault(p=>p.123==567)")]
        public static TSource Find<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            where TSource : class
        {
            return source.Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// 基于谓词筛选对象列表
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型</typeparam>
        /// <param name="source">要筛选的 System.Data.Entity.DbSet</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数</param>
        /// <returns>source类型的对象列表</returns>
        [Obsolete("請直接用.Where(p=>p.123==567)")]
        public static IEnumerable<TSource> FindMore<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
            where TSource : class
        {
            return source.Where(predicate);
        }

        /// <summary>
        /// 基于谓词筛选对象列表
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型</typeparam>
        /// <param name="source">要筛选的 System.Data.Entity.DbSet</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数</param>
        /// <returns>source类型的对象列表</returns>
        [Obsolete("請直接用.Where(p=>p.123==567)")]
        public static IEnumerable<TSource> FindMore<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            where TSource : class
        {
            return source.Where(predicate);
        }

        /// <summary>
        /// 基于谓词筛选对象列表
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型</typeparam>
        /// <param name="source">要筛选的 System.Data.Entity.DbSet</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数</param>
        /// <returns>source类型的对象列表</returns>
        [Obsolete("請直接用.Where(p=>p.123==567)")]
        public static IQueryable<TSource> QueryMore<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
            where TSource : class
        {
            return source.Where(predicate);
        }

        /// <summary>
        /// 向上下文中插入新对象
        /// </summary>
        /// <typeparam name="TEntity">entity 中的元素的类型</typeparam>
        /// <param name="context">EF数据访问上下文</param>
        /// <param name="entity">插入的对象</param>
        /// <returns>插入的对象</returns>
        [Obsolete("請使用原生寫法")]
        public static TEntity Insert<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 向上下文中批量插入新对象
        /// </summary>
        /// <typeparam name="TEntity">entities 中的元素的类型</typeparam>
        /// <param name="context">EF数据访问上下文</param>
        /// <param name="entities">插入的对象集合</param>
        /// <returns>插入的对象</returns>
        [Obsolete("請使用原生寫法，或者參考TicketDB里的BulkInsert")]
        public static IEnumerable<TEntity> InsertBatch<TEntity>(this DbContext context, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            var set = context.Set<TEntity>();
            if (null != entities && entities.Count() > 0)
            {
                foreach (var entity in entities)
                {
                    set.Add(entity);
                }
            }
            context.SaveChanges();
            return entities;
        }

        /// <summary>
        /// 向上下文中更新对象
        /// </summary>
        /// <typeparam name="TEntity">entity 中的元素的类型</typeparam>
        /// <param name="context">EF数据访问上下文</param>
        /// <param name="entity">更新的对象</param>
        /// <returns>更新的对象</returns>
        [Obsolete("請使用原生寫法，在當前上下文查出數據并修改后，EF會自動監控到，直接SaveChange就行")]
        public static TEntity Update<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class
        {
            //执行验证业务
            //context.Entry<TSource>(entity).GetValidationResult();
            //if (context.Entry<TEntity>(entity).State == EntityState.Modified)
            context.Entry<TEntity>(entity).State = EntityState.Modified; //單個插入不論entity是否更改都執行保存，因為可能存在其關聯導航屬性有改動需要保存
            context.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 向上下文中批量更新对象
        /// </summary>
        /// <typeparam name="TEntity">entities 中的元素的类型</typeparam>
        /// <param name="context">EF数据访问上下文</param>
        /// <param name="entities">更新的对象集合</param>
        /// <returns>更新的对象</returns>
        [Obsolete("沒什麽用，只是看有數據就執行SaveChange")]
        public static IEnumerable<TEntity> UpdateBatch<TEntity>(this DbContext context, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            //bool flag = false;
            if (entities.Any())
            {
                //foreach (var entity in entities)
                //{
                //    if (context.Entry<TEntity>(entity).State == EntityState.Modified)
                //        flag = true;
                //}
                //if (flag) 
                context.SaveChanges();
            }
            return entities;
        }

        /// <summary>
        /// 从上下文中删除对象
        /// </summary>
        /// <typeparam name="TEntity">entity 中的元素的类型</typeparam>
        /// <param name="context">EF数据访问上下文</param>
        /// <param name="entity">删除的对象</param>
        [Obsolete("請自行標識Delete 并SaveChange")]
        public static void Delete<TEntity>(this DbContext context, TEntity entity)
            where TEntity : class
        {
            context.Set<TEntity>().Remove(entity);
            context.Entry<TEntity>(entity).State = EntityState.Deleted;
            context.SaveChanges();
        }

        /// <summary>
        /// 从上下文中批量删除对象
        /// </summary>
        /// <typeparam name="TEntity">entities 中的元素的类型</typeparam>
        /// <param name="context">EF数据访问上下文</param>
        /// <param name="entities">删除的对象</param>
        [Obsolete("請自行標識Delete 并SaveChange")]
        public static void DeleteBatch<TEntity>(this DbContext context, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            if (entities.Any())
            {
                var set = context.Set<TEntity>();
                List<TEntity> list;
                if (entities is List<TEntity>)
                    list = entities as List<TEntity>;
                else
                    list = entities.ToList();
                if (entities.Any())
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        context.Entry<TEntity>(list[i]).State = EntityState.Deleted;
                        set.Remove(list[i]);
                    }
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 执行数据操作,用以兼容存储过程，请勿滥用,如果要直接使用此方法請自行釋放Connection
        /// </summary>
        /// <param name="database"></param>
        /// <param name="functionName"></param>
        /// <param name="parameters"></param>
        public static void ExecuteNonQuery(this Database database, string functionName, DbParameter[] parameters)
        {
            if (null == database || database.Connection == null)
                throw new ArgumentNullException("database");
            var cmd = new SqlCommand(functionName);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            ExecuteNonQuery(database, cmd);
        }

        /// <summary>
        /// 执行数据操作,用以兼容存储过程，请勿滥用,如果要直接使用此方法請自行釋放Connection
        /// </summary>
        /// <param name="database"></param>
        /// <param name="cmd"></param>
        public static void ExecuteNonQuery(this Database database, DbCommand cmd)
        {
            if (null == database || database.Connection == null)
                throw new ArgumentNullException("database");
            if (null == cmd)
                throw new ArgumentNullException("cmd");
            if (cmd.Connection == null)
                cmd.Connection = new SqlConnection(database.Connection.ConnectionString);
            try
            {
                Monitor.Enter(cmd.Connection);
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //database.HandleException(e, "ExecuteNonQuery", new System.Diagnostics.StackTrace().ToString());
                throw;
            }
            finally
            {
                cmd.Connection.Close();
                Monitor.Exit(cmd.Connection);
            }
        }

        /// <summary>
        /// 执行数据操作并返回结果集流,用以兼容存储过程，请勿滥用,如果要直接使用此方法請自行釋放Connection
        /// </summary>
        /// <param name="database"></param>
        /// <param name="functionName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(this Database database, string functionName, DbParameter[] parameters)
        {
            if (null == database || database.Connection == null)
                throw new ArgumentNullException("database");
            var cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            return ExecuteReader(database, ref cmd);
        }

        /// <summary>
        /// 执行数据操作,用以兼容存储过程，请勿滥用,如果要直接使用此方法請自行釋放Connection
        /// </summary>
        /// <param name="database"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(this Database database, ref DbCommand cmd)
        {
            if (null == database || database.Connection == null)
                throw new ArgumentNullException("database");
            if (null == cmd)
                throw new ArgumentNullException("cmd");
            if (cmd.Connection == null)
                cmd.Connection = new SqlConnection(database.Connection.ConnectionString);
            try
            {
                Monitor.Enter(cmd.Connection);
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                //database.HandleException(e, "ExecuteReader", new System.Diagnostics.StackTrace().ToString());
                cmd.Connection.Close();
                throw;
            }
            finally
            {
                //cmd.Connection.Close();
                Monitor.Exit(cmd.Connection);
            }
        }

        /// <summary>
        /// 执行数据操作,用以兼容存储过程，请勿滥用,如果要直接使用此方法請自行釋放Connection
        /// </summary>
        /// <param name="database"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(this Database database, ref SqlCommand cmd)
        {
            if (null == database || database.Connection == null)
                throw new ArgumentNullException("database");
            if (null == cmd)
                throw new ArgumentNullException("cmd");
            if (cmd.Connection == null)
                cmd.Connection = new SqlConnection(database.Connection.ConnectionString);
            try
            {
                Monitor.Enter(cmd.Connection);
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                //database.HandleException(e, "ExecuteReader", new System.Diagnostics.StackTrace().ToString());
                cmd.Connection.Close();
                throw;
            }
            finally
            {
                //cmd.Connection.Close();
                Monitor.Exit(cmd.Connection);
            }

        }

        //處理異常
        //private static void HandleException(this Database database, Exception e, string methodName, string upCallTrace)
        //{
        //    new DataAccessExceptionLogger(database.Connection.ConnectionString)
        //        .LogException(e, methodName, upCallTrace);
        //}

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString(this DbContext context)
        {
            if (context == null || context.Database == null || context.Database.Connection == null)
                return null;
            return context.Database.Connection.ConnectionString;
        }
    }
}
