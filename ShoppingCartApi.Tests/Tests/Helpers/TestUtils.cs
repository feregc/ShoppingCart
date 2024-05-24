//using Microsoft.EntityFrameworkCore;
//using Moq;
//using ShoppingCartApi.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShoppingCartApi.Tests.Tests.Helpers
//{
//    public class TestUtils
//    {
//        public static Client CreateTestClient(int id)
//        {
//            return new Client
//            {
//                Id = id,
//                Name = "Test Client",
//                Email = "testclient@example.com",
//            };
//        }

//        public static Order CreateTestOrder(int id)
//        {
//            return new Order
//            {
//                Id = id,
//                OrderDate = DateTime.Now,
//                ClientId = 1,
//            };
//        }

//        public static Payment CreateTestPayment(int id)
//        {
//            return new Payment
//            {
//                Id = id,
//                Amount = 10.0f,
//                PaymentDate = DateTime.Now,
//            };
//        }

//        public static OrderDetail CreateTestOrderDetail(int id)
//        {
//            return new OrderDetail
//            {
//                Id = id,
//                OrderId = 1,
//                ProductId = 1,
//                Quantity = 2,
//            };
//        }

//        public static Product CreateTestProduct(int id)
//        {
//            return new Product
//            {
//                id = id,
//                title = "Test Product",
//                price = 25.00f,
//                description = "Test Description",
//                category = "Test Category",
//                image = "Test Image"
//            };
//        }

//        public static User CreateTestUser(int id)
//        {
//            return new User
//            {
//                Id = id,
//                UserName = "testuser",
//                PasswordHash = new byte[0],
//                PasswordSalt = new byte[0]
//            };
//        }

//        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
//        {
//            var queryable = sourceList.AsQueryable();
//            var dbSet = new Mock<DbSet<T>>();

//            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
//            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
//            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
//            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

//            dbSet.Setup(d => d.AddAsync(It.IsAny<T>(), default)).Callback<T, CancellationToken>((s, _) => sourceList.Add(s));
//            dbSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>(s => sourceList.Remove(s));
//            dbSet.Setup(d => d.Update(It.IsAny<T>())).Callback<T>(s =>
//            {
//                var existing = sourceList.FirstOrDefault(e => e.AreEqual(s));
//                if (existing != null)
//                {
//                    sourceList.Remove(existing);
//                    sourceList.Add(s);
//                }
//            });

//            return dbSet;
//        }
//    }
//}
