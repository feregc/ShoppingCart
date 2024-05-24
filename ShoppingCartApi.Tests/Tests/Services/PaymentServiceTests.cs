using AutoMapper;
using Moq;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Models;
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
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _mapperMock = new Mock<IMapper>();
            _paymentService = new PaymentService(_paymentRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllPaymentsAsync_ReturnsListOfPaymentDtos()
        {
            var payments = new List<Payment> { new Payment() };
            var paymentDtos = new List<PaymentDto> { new PaymentDto() };

            _paymentRepositoryMock.Setup(repo => repo.GetAllPaymentsAsync()).ReturnsAsync(payments);
            _mapperMock.Setup(mapper => mapper.Map<List<PaymentDto>>(payments)).Returns(paymentDtos);

            var result = await _paymentService.GetAllPaymentsAsync();

            Xunit.Assert.Equal(paymentDtos, result);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ReturnsPaymentDto()
        {
            var payment = new Payment();
            var paymentDto = new PaymentDto();

            _paymentRepositoryMock.Setup(repo => repo.GetPaymentByIdAsync(It.IsAny<int>())).ReturnsAsync(payment);
            _mapperMock.Setup(mapper => mapper.Map<PaymentDto>(payment)).Returns(paymentDto);

            var result = await _paymentService.GetPaymentByIdAsync(1);

            Xunit.Assert.Equal(paymentDto, result);
        }

        [Fact]
        public async Task CreateUpdatePaymentAsync_ReturnsPaymentDto()
        {
            var paymentDto = new PaymentDto();
            var payment = new Payment();
            var updatedPayment = new Payment();
            var updatedPaymentDto = new PaymentDto();

            _mapperMock.Setup(mapper => mapper.Map<Payment>(paymentDto)).Returns(payment);
            _paymentRepositoryMock.Setup(repo => repo.CreateUpdateAsync(payment)).ReturnsAsync(updatedPayment);
            _mapperMock.Setup(mapper => mapper.Map<PaymentDto>(updatedPayment)).Returns(updatedPaymentDto);

            var result = await _paymentService.CreateUpdatePaymentAsync(paymentDto);

            Xunit.Assert.Equal(updatedPaymentDto, result);
        }

        [Fact]
        public async Task DeletePaymentAsync_ReturnsTrue()
        {
            _paymentRepositoryMock.Setup(repo => repo.DeletePaymentAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _paymentService.DeletePaymentAsync(1);

            Xunit.Assert.True(result);
        }
    }
}
