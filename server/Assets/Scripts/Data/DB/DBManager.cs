using System;
using Framework.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.DB
{
    public class DBManager : MonoSingleton<DBManager>
    {
        public static string connStr;// = "mongodb://127.0.0.1:27017";
        public static string DBName = "BillingDB";
        
        static MongoClient client;
        static IMongoDatabase db;
        
        static IMongoCollection<BsonDocument> BillingCollection;
        
        
        public int Init()
        {
            // 1. 创建 MongoClientSettings 实例
            var settings = MongoClientSettings.FromConnectionString(connStr);
            
            // 2. 设置连接池相关参数
            settings.MaxConnectionPoolSize = 20; // 最大连接数，可以根据实际情况调整
            settings.MinConnectionPoolSize = 5;  // 最小连接数
            settings.WaitQueueSize = 500;         // 等待队列大小
            settings.WaitQueueTimeout = TimeSpan.FromSeconds(30); // 队列超时时间
            
            // 3. 创建 MongoClient 实例
            client = new MongoClient(settings);

            // 使用 client 的 GetDatabase 方法获取数据库，即使该数据库不存在，也会自动创建；
            db = client.GetDatabase(DBName);
            
            // 预支的数据集
            BillingCollection = db.GetCollection<BsonDocument>("Billing");
            
            return 0;
        }
    }
}