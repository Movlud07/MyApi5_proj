using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.CategoryDtos;
using MyApi5.Entities.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.abstracts
{
    public interface IProductService
    {
        Task Add(ProductPostDto item);
        Task Update(int id ,ProductUpdateDto item);
        Task<ProductGetDto> GetById(int id);
        Task<ProductGetDto> GetByName(string name);
        Task<List<ProductGetDto>> GetAll(Expression<Func<Product, bool>> filter = null,params string[] includes); 
        Task Delete(int id);
    }
}
