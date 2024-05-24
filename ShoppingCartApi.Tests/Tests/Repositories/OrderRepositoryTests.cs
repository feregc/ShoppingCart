using Microsoft.EntityFrameworkCore;
using Moq;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Tests.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartApi.Tests.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _repository = new OrderRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsAllOrders()
        {
            var orders = new List<Order> { new Order { Id = 1, ClientId = 1, OrderDate = DateTime.Now }, new Order { Id = 2, ClientId = 2, OrderDate = DateTime.Now } };
            var queryableOrders = orders.AsQueryable();

            var mockSet = new Mock<DbSet<Order>>();
            mockSet.As<IAsyncEnumerable<Order>>().Setup(m => m.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<Order>(orders.GetEnumerator()));
            mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Order>(queryableOrders.Provider));
            mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(queryableOrders.Expression);
            mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(queryableOrders.ElementType);
            mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(queryableOrders.GetEnumerator());

            _mockContext.Setup(c => c.Orders).Returns(mockSet.Object);

            var result = await _repository.GetAllOrdersAsync();

            Xunit.Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOrder()
        {
            var order = new Order { Id = 1, ClientId = 1, OrderDate = DateTime.Now };
            _mockContext.Setup(c => c.Orders.FindAsync(1)).ReturnsAsync(order);

            var result = await _repository.GetOrderByIdAsync(1);

            Xunit.Assert.Equal(order, result);
        }

        [Fact]
        public async Task CreateUpdateAsync_AddsNewOrder()
        {
            var order = new Order { ClientId = 1, OrderDate = DateTime.Now };
            var mockSet = new Mock<DbSet<Order>>();
            _mockContext.Setup(c => c.Orders).Returns(mockSet.Object);

            await _repository.CreateUpdateAsync(order);

            mockSet.Verify(m => m.AddAsync(It.IsAny<Order>(), default), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task CreateUpdateAsync_UpdatesExistingOrder()
        {
            var order = new Order { Id = 1, ClientId = 1, OrderDate = DateTime.Now };
            var mockSet = new Mock<DbSet<Order>>();
            _mockContext.Setup(c => c.Orders).Returns(mockSet.Object);

            await _repository.CreateUpdateAsync(order);

            mockSet.Verify(m => m.Update(It.IsAny<Order>()), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task DeleteOrderAsync_DeletesOrder()
        {
            var order = new Order { Id = 1, ClientId = 1, OrderDate = DateTime.Now };
            _mockContext.Setup(c => c.Orders.FindAsync(1)).ReturnsAsync(order);

            var result = await _repository.DeleteOrderAsync(1);

            Xunit.Assert.True(result);
            _mockContext.Verify(c => c.Orders.Remove(It.IsAny<Order>()), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task DeleteOrderAsync_OrderNotFound()
        {
            _mockContext.Setup(c => c.Orders.FindAsync(1)).ReturnsAsync((Order)null);

            var result = await _repository.DeleteOrderAsync(1);

            Xunit.Assert.False(result);
        }
    }
}
