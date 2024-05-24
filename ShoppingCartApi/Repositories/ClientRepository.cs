using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> GetClientByIdAsync(int clientId)
        {
            return await _context.Clients.FindAsync(clientId);
        }

        public async Task<Client> CreateUpdateAsync(Client client)
        {
            if (client.Id > 0)
            {
                _context.Clients.Update(client);
            }
            else
            {
                await _context.Clients.AddAsync(client);
            }
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> DeleteClientAsync(int clientId)
        {
            try
            {
                var client = await _context.Clients.FindAsync(clientId);
                if (client == null)
                {
                    return false;
                }
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
