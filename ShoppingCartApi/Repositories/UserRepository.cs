using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShoppingCartApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public UserRepository(ApplicationDbContext context, IPasswordService passwordService, ITokenService tokenService)
        {
            _context = context;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<string> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(userName.ToLower()));
            if (user == null)
            {
                return "nouser";
            }
            else if (!_passwordService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return "wrongpassword";
            }
            else
            {
                return _tokenService.CreateToken(user);
            }
        }

        public async Task<string> Register(User user, string password)
        {
            if (await UserExist(user.UserName))
            {
                return "exist";
            }

            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return _tokenService.CreateToken(user);
        }

        public async Task<bool> UserExist(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower().Equals(username.ToLower()));
        }

    }
}
