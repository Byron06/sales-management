using ApiVentas.Models;
using ApiVentas.Models.Dtos;
using AutoMapper;

namespace ApiVentas.AutoMappers
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // Customer
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, CustomerCreateDto>().ReverseMap();

            // Order
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderCreateDto>().ReverseMap();
        }
    }
}
