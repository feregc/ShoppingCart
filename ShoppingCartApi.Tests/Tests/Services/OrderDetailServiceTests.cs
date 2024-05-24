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
    public class OrderDetailServiceTests
    {
        private readonly Mock<IOrderDetailRepository> _orderDetailRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OrderDetailService _orderDetailService;

        public OrderDetailServiceTests()
        {
            _orderDetailRepositoryMock = new Mock<IOrderDetailRepository>();
            _mapperMock = new Mock<IMapper>();
            _orderDetailService = new OrderDetailService(_orderDetailRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllOrderDetailsAsync_ReturnsListOfOrderDetailDtos()
        {
            var orderDetails = new List<OrderDetail> { new OrderDetail() };
            var orderDetailDtos = new List<OrderDetailDto> { new OrderDetailDto() };

            _orderDetailRepositoryMock.Setup(repo => repo.GetAllOrderDetailsAsync()).ReturnsAsync(orderDetails);
            _mapperMock.Setup(mapper => mapper.Map<List<OrderDetailDto>>(orderDetails)).Returns(orderDetailDtos);

            var result = await _orderDetailService.GetAllOrderDetailsAsync();

            Xunit.Assert.Equal(orderDetailDtos, result);
        }

        [Fact]
        public async Task GetOrderDetailByIdAsync_ReturnsOrderDetailDto()
        {
            var orderDetail = new OrderDetail();
            var orderDetailDto = new OrderDetailDto();

            _orderDetailRepositoryMock.Setup(repo => repo.GetOrderDetailByIdAsync(It.IsAny<int>())).ReturnsAsync(orderDetail);
            _mapperMock.Setup(mapper => mapper.Map<OrderDetailDto>(orderDetail)).Returns(orderDetailDto);

            var result = await _orderDetailService.GetOrderDetailByIdAsync(1);

            Xunit.Assert.Equal(orderDetailDto, result);
        }

        [Fact]
        public async Task CreateUpdateOrderDetailAsync_ReturnsOrderDetailDto()
        {
            var orderDetailDto = new OrderDetailDto();
            var orderDetail = new OrderDetail();
            var updatedOrderDetail = new OrderDetail();
            var updatedOrderDetailDto = new OrderDetailDto();

            _mapperMock.Setup(mapper => mapper.Map<OrderDetail>(orderDetailDto)).Returns(orderDetail);
            _orderDetailRepositoryMock.Setup(repo => repo.CreateUpdateAsync(orderDetail)).ReturnsAsync(updatedOrderDetail);
            _mapperMock.Setup(mapper => mapper.Map<OrderDetailDto>(updatedOrderDetail)).Returns(updatedOrderDetailDto);

            var result = await _orderDetailService.CreateUpdateOrderDetailAsync(orderDetailDto);

            Xunit.Assert.Equal(updatedOrderDetailDto, result);
        }

        [Fact]
        public async Task DeleteOrderDetailAsync_ReturnsTrue()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.DeleteOrderDetailAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _orderDetailService.DeleteOrderDetailAsync(1);

            Xunit.Assert.True(result);
        }
    }
}
