using AutoMapper;
using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductPostDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductGetDto>()
                .ForMember(dest => dest.CategoryName, s => s.MapFrom(src => src.Category.Name));
        }
    }
}
