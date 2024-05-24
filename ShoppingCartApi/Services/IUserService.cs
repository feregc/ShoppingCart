using ShoppingCartApi.Models;

namespace ShoppingCartApi.Services
{
    public interface IUserService
    {
        Task<string> Login(string userName, string password);
        Task<string> Register(User user, string password);
        Task<bool> UserExist(string username);
    }
}
