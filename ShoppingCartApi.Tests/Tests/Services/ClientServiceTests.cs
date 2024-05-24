using AutoMapper;
using Moq;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Services;
using Xunit;

namespace ShoppingCartApi.Tests.Services
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ClientService _clientService;

        public ClientServiceTests()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _mapperMock = new Mock<IMapper>();
            _clientService = new ClientService(_clientRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllClientsAsync_ReturnsListOfClientDtos()
        {
            var clients = new List<Client> { new Client() };
            var clientDtos = new List<ClientDto> { new ClientDto() };

            _clientRepositoryMock.Setup(repo => repo.GetAllClientsAsync()).ReturnsAsync(clients);
            _mapperMock.Setup(mapper => mapper.Map<List<ClientDto>>(clients)).Returns(clientDtos);

            var result = await _clientService.GetAllClientsAsync();

            Xunit.Assert.Equal(clientDtos, result);
        }

        [Fact]
        public async Task GetClientByIdAsync_ReturnsClientDto()
        {
            var client = new Client();
            var clientDto = new ClientDto();

            _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(It.IsAny<int>())).ReturnsAsync(client);
            _mapperMock.Setup(mapper => mapper.Map<ClientDto>(client)).Returns(clientDto);

            var result = await _clientService.GetClientByIdAsync(1);

            Xunit.Assert.Equal(clientDto, result);
        }

        [Fact]
        public async Task CreateUpdateClientAsync_ReturnsClientDto()
        {
            var clientDto = new ClientDto();
            var client = new Client();
            var updatedClient = new Client();
            var updatedClientDto = new ClientDto();

            _mapperMock.Setup(mapper => mapper.Map<Client>(clientDto)).Returns(client);
            _clientRepositoryMock.Setup(repo => repo.CreateUpdateAsync(client)).ReturnsAsync(updatedClient);
            _mapperMock.Setup(mapper => mapper.Map<ClientDto>(updatedClient)).Returns(updatedClientDto);

            var result = await _clientService.CreateUpdateClientAsync(clientDto);

            Xunit.Assert.Equal(updatedClientDto, result);
        }

        [Fact]
        public async Task DeleteClientAsync_ReturnsTrue()
        {
            _clientRepositoryMock.Setup(repo => repo.DeleteClientAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _clientService.DeleteClientAsync(1);

            Xunit.Assert.True(result);
        }
    }
}
