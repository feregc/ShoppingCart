using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<OrderDto> CreateUpdateOrderAsync(OrderDto orderDto);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}
