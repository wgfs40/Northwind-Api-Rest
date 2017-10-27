using AutoMapper;
using Northwind.Api.Entities;
using Northwind.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Api
{
    public static class MapperGeneric
    {
        public static void CofigurationMapper(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Customer, CustomerDto>()
                   .ForMember(dest => dest.ContactFull, opt => opt.MapFrom(src => $"{src.ContactName} {src.ContactTitle}")
                   ).ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.CreationTime.Year));

            cfg.CreateMap<Order, OrderDto>();

            cfg.CreateMap<CustomerForCreationDto, Customer>();

            cfg.CreateMap<OrderForCreationDto, Order>();

            cfg.CreateMap<UpdateOrderDto, Order>();

            cfg.CreateMap<Order, UpdateOrderDto>();
            
        }
    }
}
