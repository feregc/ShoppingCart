using Microsoft.EntityFrameworkCore;
using Moq;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Tests.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartApi.Tests.Tests.Repositories
{
    public class ClientRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly ClientRepository _repository;

        public ClientRepositoryTests()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _repository = new ClientRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllClientsAsync_ReturnsAllClients()
        {
            var clients = new List<Client> { new Client { Id = 1, Name = "Client1" }, new Client { Id = 2, Name = "Client2" } };
            var queryableClients = clients.AsQueryable();

            var mockSet = new Mock<DbSet<Client>>();
            mockSet.As<IAsyncEnumerable<Client>>().Setup(m => m.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<Client>(clients.GetEnumerator()));
            mockSet.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Client>(queryableClients.Provider));
            mockSet.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(queryableClients.Expression);
            mockSet.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(queryableClients.ElementType);
            mockSet.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(queryableClients.GetEnumerator());

            _mockContext.Setup(c => c.Clients).Returns(mockSet.Object);

            var result = await _repository.GetAllClientsAsync();

            Xunit.Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetClientByIdAsync_ReturnsClient()
        {
            var client = new Client { Id = 1, Name = "Client1" };
            _mockContext.Setup(c => c.Clients.FindAsync(1)).ReturnsAsync(client);

            var result = await _repository.GetClientByIdAsync(1);

            Xunit.Assert.Equal("Client1", result.Name);
        }

        [Fact]
        public async Task CreateUpdateAsync_AddsNewClient()
        {
            var client = new Client { Name = "New Client" };
            var mockSet = new Mock<DbSet<Client>>();
            _mockContext.Setup(c => c.Clients).Returns(mockSet.Object);

            await _repository.CreateUpdateAsync(client);

            mockSet.Verify(m => m.AddAsync(It.IsAny<Client>(), default), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task CreateUpdateAsync_UpdatesExistingClient()
        {
            var client = new Client { Id = 1, Name = "Updated Client" };
            var mockSet = new Mock<DbSet<Client>>();
            _mockContext.Setup(c => c.Clients).Returns(mockSet.Object);

            await _repository.CreateUpdateAsync(client);

            mockSet.Verify(m => m.Update(It.IsAny<Client>()), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task DeleteClientAsync_DeletesClient()
        {
            var client = new Client { Id = 1, Name = "Client1" };
            _mockContext.Setup(c => c.Clients.FindAsync(1)).ReturnsAsync(client);

            var result = await _repository.DeleteClientAsync(1);

            Xunit.Assert.True(result);
            _mockContext.Verify(c => c.Clients.Remove(It.IsAny<Client>()), Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task DeleteClientAsync_ClientNotFound()
        {
            _mockContext.Setup(c => c.Clients.FindAsync(1)).ReturnsAsync((Client)null);

            var result = await _repository.DeleteClientAsync(1);

            Xunit.Assert.False(result);
        }
    }
}
