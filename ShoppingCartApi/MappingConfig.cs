using AutoMapper;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ClientDto, Client>();
                config.CreateMap<Client, ClientDto>();
                config.CreateMap<OrderDetailDto, OrderDetail>();
                config.CreateMap<OrderDetail, OrderDetailDto>();
                config.CreateMap<OrderDto, Order>();
                config.CreateMap<Order, OrderDto>();
                config.CreateMap<PaymentDto, Payment>();
                config.CreateMap<Payment, PaymentDto>();
            });

            return mappingConfig;
        }
    }
}
