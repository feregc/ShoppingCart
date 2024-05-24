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
    public class OrderDetailRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly OrderDetailRepository _repository;

        public OrderDetailRepositoryTests()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _repository = new OrderDetailRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllOrderDetailsAsync_ReturnsAllOrderDetails()
        {
            var orderDetails = new List<OrderDetail> { new OrderDetail { Id = 1 }, new OrderDetail { Id = 2 } };
            var queryableOrderDetail = orderDetails.AsQueryable();

            var mockSet = new Mock<DbSet<OrderDetail>>();
            mockSet.As<IAsyncEnumerable<OrderDetail>>().Setup(m => m.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<OrderDetail>(orderDetails.GetEnumerator()));
            mockSet.As<IQueryable<OrderDetail>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Client>(queryableOrderDetail.Provider));
            mockSet.As<IQueryable<OrderDetail>>().Setup(m => m.Expression).Returns(queryableOrderDetail.Expression);
            mockSet.As<IQueryable<OrderDetail>>().Setup(m => m.ElementType).Returns(queryableOrderDetail.ElementType);
            mockSet.As<IQueryable<OrderDetail>>().Setup(m => m.GetEnumerator()).Returns(queryableOrderDetail.GetEnumerator());
            
            _mockContext.Setup(c => c.OrderDetails).Returns(mockSet.Object);

            var result = await _repository.GetAllOrderDetailsAsync();

            Xunit.Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetOrderDetailByIdAsync_ReturnsOrderDetail()
        {
            var orderDetail = new OrderDetail { Id = 1 };
            _mockContext.Setup(c => c.OrderDetails.FindAsync(1)).ReturnsAsync(orderDetail);

            var result = await _repository.GetOrderDetailByIdAsync(1);

            Xunit.Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task CreateUpdateAsync_AddsNewOrderDetail()
        {
            var orderDetail = new OrderDetail();
            var mockSet = new Mock<DbSet<OrderDetail>>();
            _mockContext.Setup(c => c.OrderDetails).Returns(mockSet.Object);

            await _repository.CreateUpdateAsync(orderDetail);

            mockSet.Verify(m => m.AddAsync(It.IsAny<OrderDetail>(), default), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task CreateUpdateAsync_UpdatesExistingOrderDetail()
        {
            var orderDetail = new OrderDetail { Id = 1 };
            var mockSet = new Mock<DbSet<OrderDetail>>();
            _mockContext.Setup(c => c.OrderDetails).Returns(mockSet.Object);

            await _repository.CreateUpdateAsync(orderDetail);

            mockSet.Verify(m => m.Update(It.IsAny<OrderDetail>()), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task DeleteOrderDetailAsync_DeletesOrderDetail()
        {
            var orderDetail = new OrderDetail { Id = 1 };
            _mockContext.Setup(c => c.OrderDetails.FindAsync(1)).ReturnsAsync(orderDetail);

            var result = await _repository.DeleteOrderDetailAsync(1);

            Xunit.Assert.True(result);
            _mockContext.Verify(c => c.OrderDetails.Remove(It.IsAny<OrderDetail>()), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task DeleteOrderDetailAsync_OrderDetailNotFound()
        {
            _mockContext.Setup(c => c.OrderDetails.FindAsync(1)).ReturnsAsync((OrderDetail)null);

            var result = await _repository.DeleteOrderDetailAsync(1);

            Xunit.Assert.False(result);
        }
    }
}
