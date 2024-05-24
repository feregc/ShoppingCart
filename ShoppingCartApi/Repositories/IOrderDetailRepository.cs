using ShoppingCartApi.Models;

namespace ShoppingCartApi.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<List<OrderDetail>> GetAllOrderDetailsAsync();
        Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId);
        Task<OrderDetail> CreateUpdateAsync(OrderDetail orderDetail);
        Task<bool> DeleteOrderDetailAsync(int orderDetailId);
    }
}
