using System;
using System.Configuration;
using MongoDB.Driver;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.MongoDb.Repository
{
    /// <summary>
    /// MongoDb基类
    /// 主要封装了创建数据库连接，获取数据库和获取集合的操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseMongoDB<T> where T : class
    {
        public BaseMongoDB(string connectionString)
            : this(connectionString, null)
        {
        }
        
        public BaseMongoDB(string connectionString, string collectionName)
        {
            if (connectionString.IsNullOrEmpty())
            {
                connectionString = ConfigurationManager.ConnectionStrings["MongoTicketDB"].ConnectionString;
            }

            if (connectionString.IsNullOrEmpty())
            {
                throw new Exception("mongodb connectionString can not be empty!");
            }

            var mongoUrl = new MongoUrl(connectionString);
            DB = GetDatabaseFromUrl(mongoUrl);
            if (!string.IsNullOrEmpty(collectionName))
            {
                this.CollectionName = collectionName;
                this.Collection = DB.GetCollection<T>(collectionName);
            }
        }

        /// <summary>
        /// 依据连接字符串获取数据库
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var client = new MongoClient(url);
            var db = client.GetDatabase(url.DatabaseName);
            return db;
        }

        /// <summary>
        /// 集合
        /// </summary>
        public IMongoCollection<T> Collection { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public IMongoDatabase DB { get; set; }

        /// <summary>
        /// 集合名称，默认为空
        /// </summary>
        //c#6.0自动只读属性默认初始化
        //public string CollectionName { get; } = string.Empty;

        public string CollectionName { get; set; }
    }
}