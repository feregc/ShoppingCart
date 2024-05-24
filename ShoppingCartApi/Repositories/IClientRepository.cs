
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi.Repositories
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllClientsAsync();
        Task<Client> GetClientByIdAsync(int clientId);
        Task<Client> CreateUpdateAsync(Client client);
        Task<bool> DeleteClientAsync(int clientId);
    }
}
