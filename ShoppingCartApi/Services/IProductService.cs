using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi.Services
{
    public interface IProductService
    {
        Task<string> GetProductsAsync();
        Task<string> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
    }
}
