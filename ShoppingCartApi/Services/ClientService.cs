using AutoMapper;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repositories;

namespace ShoppingCartApi.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<List<ClientDto>> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllClientsAsync();
            return _mapper.Map<List<ClientDto>>(clients);
        }

        public async Task<ClientDto> GetClientByIdAsync(int clientId)
        {
            var client = await _clientRepository.GetClientByIdAsync(clientId);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> CreateUpdateClientAsync(ClientDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            var updatedClient = await _clientRepository.CreateUpdateAsync(client);
            return _mapper.Map<ClientDto>(updatedClient);
        }

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            return await _clientRepository.DeleteClientAsync(clientId);
        }
    }
}
