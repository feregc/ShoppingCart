using ShoppingCartApi.Models;

namespace ShoppingCartApi.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
