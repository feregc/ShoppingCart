using Microsoft.EntityFrameworkCore;
using Moq;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartApi.Tests.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _repository = new ProductRepository(_mockContext.Object);
        }

        [Fact]
        public async Task AddProductAsync_AddsProduct()
        {
            var product = new Product
            {
                id = 1,
                title = "Test Product",
                price = 10.00f,
                description = "Test Description",
                category = "Test Category",
                image = "test_image.jpg"
            };

            var mockSet = new Mock<DbSet<Product>>();
            _mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var result = await _repository.AddProductAsync(product);

            mockSet.Verify(m => m.AddAsync(It.IsAny<Product>(), default), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
            Xunit.Assert.Equal(product, result);
        }
    }
}
