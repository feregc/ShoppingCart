using AutoMapper;
using Moq;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartApi.Tests.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _orderService = new OrderService(_orderRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsListOfOrderDtos()
        {
            var orders = new List<Order> { new Order() };
            var orderDtos = new List<OrderDto> { new OrderDto() };

            _orderRepositoryMock.Setup(repo => repo.GetAllOrdersAsync()).ReturnsAsync(orders);
            _mapperMock.Setup(mapper => mapper.Map<List<OrderDto>>(orders)).Returns(orderDtos);

            var result = await _orderService.GetAllOrdersAsync();

            Xunit.Assert.Equal(orderDtos, result);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOrderDto()
        {
            var order = new Order();
            var orderDto = new OrderDto();

            _orderRepositoryMock.Setup(repo => repo.GetOrderByIdAsync(It.IsAny<int>())).ReturnsAsync(order);
            _mapperMock.Setup(mapper => mapper.Map<OrderDto>(order)).Returns(orderDto);

            var result = await _orderService.GetOrderByIdAsync(1);

            Xunit.Assert.Equal(orderDto, result);
        }

        [Fact]
        public async Task CreateUpdateOrderAsync_ReturnsOrderDto()
        {
            var orderDto = new OrderDto();
            var order = new Order();
            var updatedOrder = new Order();
            var updatedOrderDto = new OrderDto();

            _mapperMock.Setup(mapper => mapper.Map<Order>(orderDto)).Returns(order);
            _orderRepositoryMock.Setup(repo => repo.CreateUpdateAsync(order)).ReturnsAsync(updatedOrder);
            _mapperMock.Setup(mapper => mapper.Map<OrderDto>(updatedOrder)).Returns(updatedOrderDto);

            var result = await _orderService.CreateUpdateOrderAsync(orderDto);

            Xunit.Assert.Equal(updatedOrderDto, result);
        }

        [Fact]
        public async Task DeleteOrderAsync_ReturnsTrue()
        {
            _orderRepositoryMock.Setup(repo => repo.DeleteOrderAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _orderService.DeleteOrderAsync(1);

            Xunit.Assert.True(result);
        }
    }
}
