using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/6/20 14:57:32
 * 描述说明：分页结果集实现
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Result
{
    /// <summary>
    /// 分页结果接口
    /// </summary>
    public interface IPagingResult
    {
        /// <summary>
        /// 满足查询条件的总记录数
        /// </summary>
        int TotalCount { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        int PageIndex { get; set; }
    }

    /// <summary>
    /// 分页结果接口(泛型)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPagingResult<TEntity> : IPagingResult
    {
        /// <summary>
        /// 结果记录集
        /// </summary>
        List<TEntity> Entities { get; set; }
    }

    /// <summary>
    /// 分頁結果
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [DataContract]
    public class PagingResult<TEntity> : IPagingResult<TEntity>
    {
        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        public PagingResult() { }

        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        /// <param name="totalCount">符合查詢條件的種記錄數</param>
        /// <param name="entities">分頁查詢結果集</param>
        public PagingResult(int totalCount, IQueryable<TEntity> entities)
        {
            this.TotalCount = totalCount;
            this.Entities = entities.ToList(); //ToList執行EF查詢得到結果集，為免WCF不能正常序列化IQueryable
        }

        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        /// <param name="totalCount">符合查詢條件的種記錄數</param>
        /// <param name="entities">分頁查詢結果集</param>
        public PagingResult(int totalCount, IEnumerable<TEntity> entities)
        {
            this.TotalCount = totalCount;
            this.Entities = entities.ToList(); //ToList防止IEnumerable的實際類型是IQueryable而導致的問題
        }

        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        /// <param name="totalCount">符合查詢條件的種記錄數</param>
        /// <param name="entities">分頁查詢結果集</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagingResult(int totalCount, int pageIndex, int pageSize, IQueryable<TEntity> entities)
        {
            this.TotalCount = totalCount;
            this.Entities = entities.ToList(); //ToList執行EF查詢得到結果集，為免WCF不能正常序列化IQueryable
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// 初始化分頁結果的新實例
        /// </summary>
        /// <param name="totalCount">符合查詢條件的種記錄數</param>
        /// <param name="entities">分頁查詢結果集</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagingResult(int totalCount, int pageIndex, int pageSize, IEnumerable<TEntity> entities)
        {
            this.TotalCount = totalCount;
            this.Entities = entities.ToList(); //ToList防止IEnumerable的實際類型是IQueryable而導致的問題
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// 获取或设置符合查詢條件的種記錄數
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }

        /// <summary>
        /// 获取或设置分頁頁面大小
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// 获取或设置分頁頁索引（從1開始）
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }

        /// <summary>
        /// 获取或设置分頁查詢結果集
        /// </summary>
        [DataMember]
        public List<TEntity> Entities { get; set; }
    }
}
