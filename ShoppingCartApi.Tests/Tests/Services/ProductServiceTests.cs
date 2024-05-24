using AutoMapper;
using Moq;
using Moq.Protected;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartApi.Tests.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _productService = new ProductService(_clientFactoryMock.Object, _mapperMock.Object, _productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ReturnsProductsJson()
        {
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"id\":1,\"name\":\"Product 1\"}]")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           It.IsAny<HttpRequestMessage>(),
                           It.IsAny<CancellationToken>())
                       .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(handlerMock.Object);
            _clientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = await _productService.GetProductsAsync();

            Xunit.Assert.Equal("[{\"id\":1,\"name\":\"Product 1\"}]", result);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsProductJson()
        {
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"id\":1,\"name\":\"Product 1\"}")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           It.IsAny<HttpRequestMessage>(),
                           It.IsAny<CancellationToken>())
                       .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(handlerMock.Object);
            _clientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = await _productService.GetProductByIdAsync(1);

            Xunit.Assert.Equal("{\"id\":1,\"name\":\"Product 1\"}", result);
        }

        [Fact]
        public async Task AddProductAsync_ReturnsProduct()
        {
            var product = new Product();
            _productRepositoryMock.Setup(repo => repo.AddProductAsync(product)).ReturnsAsync(product);

            var result = await _productService.AddProductAsync(product);

            Xunit.Assert.Equal(product, result);
        }
    }
}
