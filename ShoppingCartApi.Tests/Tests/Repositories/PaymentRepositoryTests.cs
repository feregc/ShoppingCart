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
    public class PaymentRepositoryTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private IPaymentRepository _paymentRepository;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _paymentRepository = new PaymentRepository(_mockContext.Object);
        }

        [Test]
        public async Task GetAllPaymentsAsync_ReturnsListOfPayments()
        {
            var payments = new List<Payment> { new Payment(), new Payment() };
            var mockDbSet = TestUtils.GetQueryableMockDbSet(payments);
            _mockContext.Setup(x => x.Payments).Returns(mockDbSet.Object);

            var result = await _paymentRepository.GetAllPaymentsAsync();

            Assert.Equals(payments.Count, result.Count);
        }

        [Test]
        public async Task GetPaymentByIdAsync_ReturnsPayment()
        {
            var payment = new Payment { Id = 1 };
            _mockContext.Setup(x => x.Payments.FindAsync(1)).ReturnsAsync(payment);

            var result = await _paymentRepository.GetPaymentByIdAsync(1);

            Assert.Equals(payment.Id, result.Id);
        }

        [Test]
        public async Task CreateUpdateAsync_CreatesNewPayment()
        {
            var payment = new Payment { Id = 0 };
            _mockContext.Setup(x => x.Payments.AddAsync(payment, default));

            var result = await _paymentRepository.CreateUpdateAsync(payment);

            _mockContext.Verify(x => x.Payments.AddAsync(payment, default), Times.Once);
            Assert.Equals(payment, result);
        }

        [Test]
        public async Task DeletePaymentAsync_DeletesPayment()
        {
            var payment = new Payment { Id = 1 };
            _mockContext.Setup(x => x.Payments.FindAsync(1)).ReturnsAsync(payment);
            _mockContext.Setup(x => x.Payments.Remove(payment));

            var result = await _paymentRepository.DeletePaymentAsync(1);

            _mockContext.Verify(x => x.Payments.Remove(payment), Times.Once);
            Assert.That(result);
        }
    }
}
