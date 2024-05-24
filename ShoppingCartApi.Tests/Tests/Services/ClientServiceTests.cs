using AutoMapper;
using Moq;
using NUnit.Framework;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Services;

namespace ShoppingCartApi.Tests.Services
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _mockClientRepository;
        private Mock<IMapper> _mockMapper;
        private IClientService _clientService;

        [SetUp]
        public void Setup()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _mockMapper = new Mock<IMapper>();
            _clientService = new ClientService(_mockClientRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetAllClientsAsync_ReturnsListOfClients()
        {
            var clients = new List<Client> { new Client(), new Client() };
            var clientDtos = new List<ClientDto> { new ClientDto(), new ClientDto() };

            _mockClientRepository.Setup(repo => repo.GetAllClientsAsync()).ReturnsAsync(clients);
            _mockMapper.Setup(mapper => mapper.Map<List<ClientDto>>(clients)).Returns(clientDtos);

            var result = await _clientService.GetAllClientsAsync();

            Assert.Equals(clientDtos, result);
        }

        [Test]
        public async Task GetClientByIdAsync_ReturnsClientDto()
        {
            // Arrange
            var clientId = 1;
            var client = new Client();
            var clientDto = new ClientDto();

            _mockClientRepository.Setup(repo => repo.GetClientByIdAsync(clientId)).ReturnsAsync(client);
            _mockMapper.Setup(mapper => mapper.Map<ClientDto>(client)).Returns(clientDto);

            // Act
            var result = await _clientService.GetClientByIdAsync(clientId);

            // Assert
            Assert.Equals(clientDto, result);
        }
    }
}
