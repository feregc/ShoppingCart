using ShoppingCartApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartApi.Tests.Tests.Services
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _passwordService;

        public PasswordServiceTests()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void CreatePasswordHash_ValidPassword_HashAndSaltGenerated()
        {
            var password = "testPassword";

            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            Xunit.Assert.NotNull(passwordHash);
            Xunit.Assert.NotNull(passwordSalt);
        }

        [Fact]
        public void VerifyPasswordHash_ValidPassword_ReturnsTrue()
        {
            var password = "testPassword";
            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var result = _passwordService.VerifyPasswordHash(password, passwordHash, passwordSalt);

            Xunit.Assert.True(result);
        }

        [Fact]
        public void VerifyPasswordHash_InvalidPassword_ReturnsFalse()
        {
            var password = "testPassword";
            var wrongPassword = "wrongPassword";
            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var result = _passwordService.VerifyPasswordHash(wrongPassword, passwordHash, passwordSalt);

            Xunit.Assert.False(result);
        }
    }
}
