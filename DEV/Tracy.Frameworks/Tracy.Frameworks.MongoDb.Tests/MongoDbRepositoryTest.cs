using System;
using System.Linq;
using Tracy.Frameworks.MongoDb.Tests.Entity;
using Tracy.Frameworks.MongoDb.Repository;
using System.Collections.Generic;
using MongoDB.Bson;
using NUnit.Framework;

namespace Tracy.Frameworks.MongoDb.Tests
{
    [TestFixture]
    public class MongoDbRepositoryTest
    {
        private string lastId = string.Empty;

        /// <summary>
        /// string key，需要手动给id赋值，如Guid.NewGuid().ToString()
        /// </summary>
        [Test]
        public void MongoDb_test()
        {
            var context = MongoRepository<OrderLog, string>.CreateRepository();
            context.RemoveAll();

            MongoDb_add_test(context);
            MongoDb_get_test(context);
            MongoDb_update_test(context);
            MongoDb_delete_test(context);
        }

        /// <summary>
        /// ObjectId，不需要手动赋值
        /// </summary>
        [Test]
        public void MongoDb_test_ObjectId()
        {
            var context = MongoRepository<Product, ObjectId>.CreateRepository();
            context.RemoveAll();

            MongoDb_add_ObjectId_test(context);

            //其它操作同string key相同......
        }

        [Test]
        public void MongoDb_test_BsonDocument()
        {
            //insert
            var context = MongoRepository<BsonDocument, ObjectId>.CreateRepository(MongoDBs.MongoTicketDB);
            var list = new List<Entity1>
            {
                new Entity1
                {
                    Name = "mcgradylu",
                    Description = "tmac",
                    Price= 0.50M,
                    CreateTime= DateTime.Now
                }
            };
            var json = list.ToJson();
            context.Insert(json);
        }

        [Test]
        public void MongoDb_test_WrapBusinessEntity()
        {
            //add
            var context = MongoRepository<BookingSearchRQMongo, ObjectId>.CreateRepository(MongoDBs.MongoTicketDB);
            context.RemoveAll();

            var rq = new BookingSearchRQMongo
            {
                BookingSearchRQ= new BookingSearchRQ
                {
                    IsReturnPagedData= false,
                    OutBoundFromAirport= "HKG",
                    OutBoundToAirport= "WUH",
                    InBoundFromAirport= "WUH",
                    InBoundToAirport= "HKG",
                    JourneyType= JourneyType.RoundTrip,
                    OutBoundDate= DateTime.Now,
                    InBoundDate= DateTime.Now.AddDays(7)
                }
            };
            context.Insert(rq);

            //get
            var result= context.GetBy(p => p.BookingSearchRQ.OutBoundFromAirport.Equals("HKG") && p.BookingSearchRQ.OutBoundToAirport.Equals("WUH")).FirstOrDefault();
            Assert.IsNotNull(result);
        }


        #region string key
        /// <summary>
        /// 新增
        /// </summary>
        private void MongoDb_add_test(MongoRepository<OrderLog, string> context)
        {
            //单个
            var orderLog = new OrderLog
            {
                Id = Guid.NewGuid().ToString(),
                OrderAmount= 6000,
                OrderId = 40000000,
                Title = "title0",
                Summary = "summary0",
                OrderDate = DateTime.Now
            };
            context.Insert(orderLog);

            var result = context.GetBy(p => p.Title.Equals("title0")).FirstOrDefault();
            Assert.IsNotNull(result);

            //批量
            var orderLogs = new List<OrderLog>();
            for (int i = 1; i < 6; i++)
            {
                var model = new OrderLog();
                model.Id = Guid.NewGuid().ToString();
                model.OrderId = 40000000 + i;
                model.OrderAmount = 6000 - i;
                model.Title = "title" + i;
                model.Summary = "summary" + i;
                model.OrderDate = DateTime.Now;
                orderLogs.Add(model);

                lastId = model.Id;
            }
            context.Insert(orderLogs);

            var count = context.GetAll().Count();
            Assert.AreEqual(6, count);

            //from json文件
            //TODO:暂时不行
            //var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\OrderLogs.txt");
            //var json = File.ReadAllText(filePath);

            //context.Insert(json);
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void MongoDb_get_test(MongoRepository<OrderLog, string> context)
        {
            //查询所有
            var total = context.GetAll().Count();
            Assert.AreEqual(6, total);

            //查询by id
            var orderLog = context.GetById(lastId);
            Assert.IsNotNull(orderLog);

            //查询by表达式
            var orderLogs = context.GetBy(p => p.OrderId <= 40000003).ToList();
            Assert.IsNotNull(orderLogs);

            //分页查询
            var orderby = new List<OrderBy<OrderLog>>();
            orderby.Add(new OrderBy<OrderLog>()
            {
                Field = p => p.OrderId,
                IsAscending = true //按订单号升序
            });
            orderby.Add(new OrderBy<OrderLog>()
            {
                Field= p=>p.OrderAmount,
                IsAscending= true //按订单金额升序
            });

            var orderLogsPaged = context.GetByPaging(p => p.Title.StartsWith("title"), 1, 3, orderby);//如何指定排序规则
            Assert.IsNotNull(orderLogsPaged);
        }

        /// <summary>
        /// 更新
        /// </summary>
        private void MongoDb_update_test(MongoRepository<OrderLog, string> context)
        {
            //单个
            var current = context.GetById(lastId);
            Assert.IsNotNull(current);
            current.Summary = "summary555";
            context.Update(current);

            //批量
            var orderLogs = context.GetAll().ToList();
            orderLogs.ForEach(item =>
            {
                item.UpdatedDate = item.OrderDate;
            });
            context.Update(orderLogs);

            //局部更新
            var dic = new Dictionary<string, object>();
            dic["Title"] = "title555";
            dic["Summary"] = "summary555";
            context.Update(p => true, dic);

            var last = context.GetAll().LastOrDefault();
            Assert.AreEqual("summary555", last.Summary);

            //更新，如果不存在则新增
            var model = new OrderLog()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = 30000006,
                Title = "title666",
                Summary = "summary666",
                OrderDate = DateTime.Now
            };
            context.UpdateOrAdd(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void MongoDb_delete_test(MongoRepository<OrderLog, string> context)
        {
            //by id
            var deleteCount = context.Delete(lastId);
            Assert.Greater(deleteCount, 0);

            //单个
            var orderLog = context.GetBy(p => p.OrderId == 30000004).FirstOrDefault();
            context.Delete(orderLog);

            //by表达式
            context.Delete(p => p.OrderId == 30000006 && p.Title.StartsWith("title"));

            //删除所有
            context.RemoveAll();
        }

        #endregion

        #region ObjectId key
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="context"></param>
        private void MongoDb_add_ObjectId_test(MongoRepository<Product, ObjectId> context)
        {
            //单个
            var product = new Product
            {
                Name = "name0",
                Description = "description0",
                Price = 0
            };
            context.Insert(product);

            var result = context.GetBy(p => p.Name.Equals("name0")).FirstOrDefault();
            Assert.IsNotNull(result);

        }
        #endregion

    }
}
