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
    public class OrderDetailRepositoryTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private IOrderDetailRepository _orderDetailRepository;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _orderDetailRepository = new OrderDetailRepository(_mockContext.Object);
        }

        [Test]
        public async Task GetAllOrderDetailsAsync_ReturnsListOfOrderDetails()
        {
            var orderDetails = new List<OrderDetail> { new OrderDetail(), new OrderDetail() };
            var mockDbSet = TestUtils.GetQueryableMockDbSet(orderDetails);
            _mockContext.Setup(x => x.OrderDetails).Returns(mockDbSet.Object);

            var result = await _orderDetailRepository.GetAllOrderDetailsAsync();

            Assert.Equals(orderDetails.Count, result.Count);
        }

        [Test]
        public async Task GetOrderDetailByIdAsync_ReturnsOrderDetail()
        {
            var orderDetail = new OrderDetail { Id = 1 };
            _mockContext.Setup(x => x.OrderDetails.FindAsync(1)).ReturnsAsync(orderDetail);

            var result = await _orderDetailRepository.GetOrderDetailByIdAsync(1);

            Assert.Equals(orderDetail.Id, result.Id);
        }

        [Test]
        public async Task CreateUpdateAsync_CreatesNewOrderDetail()
        {
            var orderDetail = new OrderDetail { Id = 0 };
            _mockContext.Setup(x => x.OrderDetails.AddAsync(orderDetail, default));

            var result = await _orderDetailRepository.CreateUpdateAsync(orderDetail);

            _mockContext.Verify(x => x.OrderDetails.AddAsync(orderDetail, default), Times.Once);
            Assert.Equals(orderDetail, result);
        }

        [Test]
        public async Task DeleteOrderDetailAsync_DeletesOrderDetail()
        {
            var orderDetail = new OrderDetail { Id = 1 };
            _mockContext.Setup(x => x.OrderDetails.FindAsync(1)).ReturnsAsync(orderDetail);
            _mockContext.Setup(x => x.OrderDetails.Remove(orderDetail));

            var result = await _orderDetailRepository.DeleteOrderDetailAsync(1);

            _mockContext.Verify(x => x.OrderDetails.Remove(orderDetail), Times.Once);
            Assert.That(result);
        }
    }
}
