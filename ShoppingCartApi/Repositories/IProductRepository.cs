using ShoppingCartApi.Models;

namespace ShoppingCartApi.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
    }
}
