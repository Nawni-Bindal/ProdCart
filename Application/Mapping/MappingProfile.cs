using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateItemDto, Item>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Item, ItemDto>().ReverseMap();

            CreateMap<CreateProductDto, Product>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<UpdateProductDto, Product>().ForMember(dest => dest.Items, opt => opt.Ignore());

            CreateMap<UpdateItemDto, Item>();
        }
    }
}