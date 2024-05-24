using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi.Services
{
    public interface IOrderDetailService
    {
        Task<List<OrderDetailDto>> GetAllOrderDetailsAsync();
        Task<OrderDetailDto> GetOrderDetailByIdAsync(int orderDetailId);
        Task<OrderDetailDto> CreateUpdateOrderDetailAsync(OrderDetailDto orderDetailDto);
        Task<bool> DeleteOrderDetailAsync(int orderDetailId);
    }
}
