using AutoMapper;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repositories;

namespace ShoppingCartApi.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDetailDto>> GetAllOrderDetailsAsync()
        {
            var orderDetails = await _orderDetailRepository.GetAllOrderDetailsAsync();
            return _mapper.Map<List<OrderDetailDto>>(orderDetails);
        }

        public async Task<OrderDetailDto> GetOrderDetailByIdAsync(int orderDetailId)
        {
            var orderDetail = await _orderDetailRepository.GetOrderDetailByIdAsync(orderDetailId);
            return _mapper.Map<OrderDetailDto>(orderDetail);
        }

        public async Task<OrderDetailDto> CreateUpdateOrderDetailAsync(OrderDetailDto orderDetailDto)
        {
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
            var updatedOrderDetail = await _orderDetailRepository.CreateUpdateAsync(orderDetail);
            return _mapper.Map<OrderDetailDto>(updatedOrderDetail);
        }

        public async Task<bool> DeleteOrderDetailAsync(int orderDetailId)
        {
            return await _orderDetailRepository.DeleteOrderDetailAsync(orderDetailId);

        }
    }
}
