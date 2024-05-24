using Moq;
using NUnit.Framework;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Tests.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartApi.Tests.Tests.Repositories
{
    [TestFixture]
    public class ClientRepositoryTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private IClientRepository _clientRepository;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _clientRepository = new ClientRepository(_mockContext.Object);
        }

        [Test]
        public async Task GetAllClientsAsync_ReturnsListOfClients()
        {
            var clients = new List<Client> { new Client(), new Client() };
            var mockDbSet = TestUtils.GetQueryableMockDbSet(clients);
            _mockContext.Setup(x => x.Clients).Returns(mockDbSet.Object);

            var result = await _clientRepository.GetAllClientsAsync();

            Assert.Equals(clients.Count, result.Count);
        }

        [Test]
        public async Task GetClientByIdAsync_ReturnsClient()
        {
            var client = new Client { Id = 1 };
            _mockContext.Setup(x => x.Clients.FindAsync(1)).ReturnsAsync(client);

            var result = await _clientRepository.GetClientByIdAsync(1);

            Assert.Equals(client.Id, result.Id);
        }

        [Test]
        public async Task CreateUpdateAsync_CreatesNewClient()
        {
            var client = new Client { Id = 0 };
            _mockContext.Setup(x => x.Clients.AddAsync(client, default));

            var result = await _clientRepository.CreateUpdateAsync(client);

            _mockContext.Verify(x => x.Clients.AddAsync(client, default), Times.Once);
            Assert.Equals(client, result);
        }

        [Test]
        public async Task DeleteClientAsync_DeletesClient()
        {
            var client = new Client { Id = 1 };
            _mockContext.Setup(x => x.Clients.FindAsync(1)).ReturnsAsync(client);
            _mockContext.Setup(x => x.Clients.Remove(client));

            var result = await _clientRepository.DeleteClientAsync(1);

            _mockContext.Verify(x => x.Clients.Remove(client), Times.Once);
            Assert.That(result);
        }
    }
}
