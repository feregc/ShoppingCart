
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repositories;

namespace ShoppingCartApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductService(IHttpClientFactory clientFactory, IMapper mapper, IProductRepository productRepository)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<string> GetProductsAsync()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://fakestoreapi.com/products");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error al obtener datos desde la API externa: {(int)response.StatusCode}");
            }
        }

        public async Task<string> GetProductByIdAsync(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://fakestoreapi.com/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error al obtener datos desde la API externa: {(int)response.StatusCode}");
            }
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            return await _productRepository.AddProductAsync(product);
        }
    }
}
