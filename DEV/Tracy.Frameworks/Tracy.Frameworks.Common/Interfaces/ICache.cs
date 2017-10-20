using System;

namespace Tracy.Frameworks.Common.Interfaces
{
    /// <summary>
    /// 公用缓存接口
    /// </summary>
    public interface ICache
    {
        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值(string)</param>
        /// <param name="seconds">缓存时间(秒)，默认不过期</param>
        /// <returns></returns>
        bool Add(string key, string value, int seconds = 0);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值(对象)</param>
        /// <param name="seconds">缓存时间(秒)，默认不过期</param>
        /// <returns></returns>
        bool Add<T>(string key, T value, int seconds = 0) where T : class, new();
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间(秒)，默认不过期</param>
        /// <returns></returns>
        bool Update(string key, string value, int seconds = 0);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间(秒)，默认不过期</param>
        /// <returns></returns>
        bool Update<T>(string key, T value, int seconds = 0) where T : class, new();
        #endregion

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回string</returns>
        string Get(string key);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对象</returns>
        T Get<T>(string key) where T : class;

        /// <summary>
        /// 查询(不存在则新增)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="func">委托(string)</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        string GetOrAdd(string key, Func<string> func, int seconds = 0);

        /// <summary>
        /// 查询(不存在则新增)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="func">委托(对象)</param>
        /// <param name="seconds">缓存时间（秒）</param>
        /// <returns></returns>
        T GetOrAdd<T>(string key, Func<T> func, int seconds = 0) where T : class, new(); 
        #endregion
        
        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Remove(string key);
        #endregion

        #region 其它
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool IsExists(string key);
        #endregion
    }
}
