using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;

namespace ShoppingCartApi.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId)
        {
            return await _context.OrderDetails.FindAsync(orderDetailId);
        }

        public async Task<OrderDetail> CreateUpdateAsync(OrderDetail orderDetail)
        {
            if (orderDetail.Id > 0)
            {
                _context.OrderDetails.Update(orderDetail);
            }
            else
            {
                await _context.OrderDetails.AddAsync(orderDetail);
            }
            await _context.SaveChangesAsync();
            return orderDetail;
        }

        public async Task<bool> DeleteOrderDetailAsync(int orderDetailId)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.FindAsync(orderDetailId);
                if (orderDetail == null)
                {
                    return false;
                }
                _context.OrderDetails.Remove(orderDetail);
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
