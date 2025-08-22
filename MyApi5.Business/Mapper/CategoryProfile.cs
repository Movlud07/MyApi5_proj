using AutoMapper;
using Microsoft.AspNetCore.Http;
using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Mapper
{
    public class CategoryProfile : Profile
    {
        public readonly IHttpContextAccessor _access;

        public CategoryProfile(IHttpContextAccessor access)
        {
            _access = access;

            CreateMap<CategoryPostDto, Category>()
                .ForMember(dest => dest.ImagePath, s => s.MapFrom(x => x.FormFile.FileName));

            CreateMap<CategoryUpdateDto, Category>();


            //Bu birinci yazılışıdı düzgün url yaratmaq üçün
            //CreateMap<Category, CategoryGetDto>()
            //    .ForMember(dest => dest.ProductsName, s => s.MapFrom(s => s.Products.Select(x => x.Name)))
            //    .ForMember(dest => dest.ImageUrl, s => s.MapFrom(x => _access.HttpContext.Request.Scheme + "://" + _access.HttpContext.Request.Host.Host + ":" + _access.HttpContext.Request.Host.Port + "/uploads/img/" + x.ImagePath));
            //https://localhost:7007/swagger/index.html
            //httpslocalhost7007/uploads/imghqdefault.jpg


            //Bu isə ikinci yazılışıdı
            //var uriBuilder = new UriBuilder(_access.HttpContext.Request.Scheme, _access.HttpContext.Request.Host.Host, _access.HttpContext.Request.Host.Port ?? -1);
            //if (uriBuilder.Uri.IsDefaultPort)
            //{
            //    uriBuilder.Port = -1;
            //}

            //string baseUrl = uriBuilder.Uri.AbsoluteUri;

            //CreateMap<Category, CategoryGetDto>()
            //    .ForMember(dest => dest.ProductsName, s => s.MapFrom(s => s.Products.Select(x => x.Name)))
            //    .ForMember(dest => dest.ImageUrl, s => s.MapFrom(x => (x.ImagePath != null ? baseUrl + "uploads/img/" + x.ImagePath : null)));


            var request = _access.HttpContext.Request;

            string baseUrl = $"{request.Scheme}://{request.Host.Value}/";

            CreateMap<Category, CategoryGetDto>()
                .ForMember(dest => dest.ProductsName, s => s.MapFrom(src => src.Products.Select(p => p.Name)))
                .ForMember(dest => dest.ImageUrl, s => s.MapFrom(src =>
                    src.ImagePath != null
                        ? $"{baseUrl}uploads/img/{src.ImagePath}"
                        : null
                ));
        }
    }
}
