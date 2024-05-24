using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi.Services
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetAllClientsAsync();
        Task<ClientDto> GetClientByIdAsync(int clientId);
        Task<ClientDto> CreateUpdateClientAsync(ClientDto clientDto);
        Task<bool> DeleteClientAsync(int clientId);
    }
}
