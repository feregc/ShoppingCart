using Microsoft.EntityFrameworkCore;
using Moq;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ShoppingCartApi.Tests.Tests.Helpers;

namespace ShoppingCartApi.Tests.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _contextMock = new Mock<ApplicationDbContext>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _userService = new UserService(_contextMock.Object, _passwordServiceMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Login_ReturnsToken_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "testuser", PasswordHash = new byte[0], PasswordSalt = new byte[0] };
            var usersDbSetMock = new Mock<DbSet<User>>();
            var users = new List<User> { user }.AsQueryable();
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _contextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

            _passwordServiceMock.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            _tokenServiceMock.Setup(x => x.CreateToken(It.IsAny<User>())).Returns("testtoken");

            // Act
            var result = await _userService.Login("testuser", "password");

            // Assert
            Xunit.Assert.Equal("testtoken", result);
        }

        [Fact]
        public async Task Register_ReturnsToken_WhenUserIsRegistered()
        {
            // Arrange
            var user = new User { UserName = "newuser" };
            var usersDbSetMock = new Mock<DbSet<User>>();
            var users = new List<User>().AsQueryable();
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _contextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

            _passwordServiceMock.Setup(x => x.CreatePasswordHash(It.IsAny<string>(), out It.Ref<byte[]>.IsAny, out It.Ref<byte[]>.IsAny));
            _tokenServiceMock.Setup(x => x.CreateToken(It.IsAny<User>())).Returns("testtoken");

            // Act
            var result = await _userService.Register(user, "password");

            // Assert
            Xunit.Assert.Equal("testtoken", result);
        }

        [Fact]
        public async Task UserExist_ReturnsTrue_WhenUserExists()
        {
            // Arrange
            var user = new User { UserName = "existinguser" };
            var usersDbSetMock = new Mock<DbSet<User>>();
            var users = new List<User> { user }.AsQueryable();
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            usersDbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _contextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

            // Act
            var result = await _userService.UserExist("existinguser");

            // Assert
            Xunit.Assert.True(result);
        }
    }
}
