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
    public class PaymentRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly PaymentRepository _repository;

        public PaymentRepositoryTests()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _repository = new PaymentRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllPaymentsAsync_ReturnsAllPayments()
        {
            var payments = new List<Payment> { new Payment { Id = 1, OrderId = 1, Amount = 100.00f, PaymentDate = DateTime.Now }, new Payment { Id = 2, OrderId = 2, Amount = 150.00f, PaymentDate = DateTime.Now } };
            var queryablePayments = payments.AsQueryable();

            var mockSet = new Mock<DbSet<Payment>>();
            mockSet.As<IAsyncEnumerable<Payment>>().Setup(m => m.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<Payment>(payments.GetEnumerator()));
            mockSet.As<IQueryable<Payment>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Payment>(queryablePayments.Provider));
            mockSet.As<IQueryable<Payment>>().Setup(m => m.Expression).Returns(queryablePayments.Expression);
            mockSet.As<IQueryable<Payment>>().Setup(m => m.ElementType).Returns(queryablePayments.ElementType);
            mockSet.As<IQueryable<Payment>>().Setup(m => m.GetEnumerator()).Returns(queryablePayments.GetEnumerator());

            _mockContext.Setup(c => c.Payments).Returns(mockSet.Object);

            var result = await _repository.GetAllPaymentsAsync();

            Xunit.Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ReturnsPayment()
        {
            var payment = new Payment { Id = 1, OrderId = 1, Amount = 100.00f, PaymentDate = DateTime.Now };
            _mockContext.Setup(c => c.Payments.FindAsync(1)).ReturnsAsync(payment);

            var result = await _repository.GetPaymentByIdAsync(1);

            Xunit.Assert.Equal(payment, result);
        }

        [Fact]
        public async Task CreateUpdateAsync_AddsNewPayment()
        {
            var payment = new Payment { OrderId = 1, Amount = 100.00f, PaymentDate = DateTime.Now };
            var mockSet = new Mock<DbSet<Payment>>();
            _mockContext.Setup(c => c.Payments).Returns(mockSet.Object);

            await _repository.CreateUpdateAsync(payment);

            mockSet.Verify(m => m.AddAsync(It.IsAny<Payment>(), default), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task CreateUpdateAsync_UpdatesExistingPayment()
        {
            var payment = new Payment { Id = 1, OrderId = 1, Amount = 150.00f, PaymentDate = DateTime.Now };
            var mockSet = new Mock<DbSet<Payment>>();
            _mockContext.Setup(c => c.Payments).Returns(mockSet.Object);

            await _repository.CreateUpdateAsync(payment);

            mockSet.Verify(m => m.Update(It.IsAny<Payment>()), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task DeletePaymentAsync_DeletesPayment()
        {
            var payment = new Payment { Id = 1, OrderId = 1, Amount = 100.00f, PaymentDate = DateTime.Now };
            _mockContext.Setup(c => c.Payments.FindAsync(1)).ReturnsAsync(payment);

            var result = await _repository.DeletePaymentAsync(1);

            Xunit.Assert.True(result);
            _mockContext.Verify(c => c.Payments.Remove(It.IsAny<Payment>()), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task DeletePaymentAsync_PaymentNotFound()
        {
            _mockContext.Setup(c => c.Payments.FindAsync(1)).ReturnsAsync((Payment)null);

            var result = await _repository.DeletePaymentAsync(1);

            Xunit.Assert.False(result);
        }
    }
}
