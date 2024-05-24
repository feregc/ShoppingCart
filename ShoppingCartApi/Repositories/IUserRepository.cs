using ShoppingCartApi.Models;

namespace ShoppingCartApi.Repositories
{
    public interface IUserRepository
    {
        Task<string> Login(string userName, string password);
        Task<string> Register(User user, string password);
        Task<bool> UserExist(string username);
    }
}
