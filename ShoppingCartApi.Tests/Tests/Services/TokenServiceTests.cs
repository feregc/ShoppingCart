using Microsoft.Extensions.Configuration;
using Moq;
using ShoppingCartApi.Models;
using ShoppingCartApi.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartApi.Tests.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(config => config["Jwt:Key"]).Returns("rlTQCYEXddRaIOhNvMUYMFhkKxhcptvH");
            _tokenService = new TokenService(_configurationMock.Object);
        }

        [Fact]
        public void CreateToken_ReturnsToken_ForValidUser()
        {
            var user = new User { Id = 1, UserName = "testuser" };

            var token = _tokenService.CreateToken(user);

            Xunit.Assert.NotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token) as JwtSecurityToken;
            Xunit.Assert.NotNull(jwtToken);
            Xunit.Assert.Equal("testuser", jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value);
            Xunit.Assert.Equal("1", jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
