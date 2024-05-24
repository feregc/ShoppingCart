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

namespace ShoppingCartApi.Tests.Tests.Repositories
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private IOrderRepository _orderRepository;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _orderRepository = new OrderRepository(_mockContext.Object);
        }

        [Test]
        public async Task GetAllOrdersAsync_ReturnsListOfOrders()
        {
            var orders = new List<Order> { new Order(), new Order() };
            var mockDbSet = TestUtils.GetQueryableMockDbSet(orders);
            _mockContext.Setup(x => x.Orders).Returns(mockDbSet.Object);

            var result = await _orderRepository.GetAllOrdersAsync();

            Assert.Equals(orders.Count, result.Count);
        }

        [Test]
        public async Task GetOrderByIdAsync_ReturnsOrder()
        {
            var order = new Order { Id = 1 };
            _mockContext.Setup(x => x.Orders.FindAsync(1)).ReturnsAsync(order);

            var result = await _orderRepository.GetOrderByIdAsync(1);

            Assert.Equals(order.Id, result.Id);
        }

        [Test]
        public async Task CreateUpdateAsync_CreatesNewOrder()
        {
            var order = new Order { Id = 0 };
            _mockContext.Setup(x => x.Orders.AddAsync(order, default));

            var result = await _orderRepository.CreateUpdateAsync(order);

            _mockContext.Verify(x => x.Orders.AddAsync(order, default), Times.Once);
            Assert.Equals(order, result);
        }

        [Test]
        public async Task DeleteOrderAsync_DeletesOrder()
        {
            var order = new Order { Id = 1 };
            _mockContext.Setup(x => x.Orders.FindAsync(1)).ReturnsAsync(order);
            _mockContext.Setup(x => x.Orders.Remove(order));

            var result = await _orderRepository.DeleteOrderAsync(1);

            _mockContext.Verify(x => x.Orders.Remove(order), Times.Once);
            Assert.That(result);
        }
    }
}
