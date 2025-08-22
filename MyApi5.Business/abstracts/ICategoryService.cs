using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.abstracts
{
    public interface ICategoryService
    {
        Task Add(CategoryPostDto item);
        Task<List<CategoryGetDto>> GetAll(Expression<Func<Category, bool>> filter = null,params string[] includes);
        Task<CategoryGetDto> GetById(int id);
        Task<CategoryGetDto> GetByName(string name);
        Task Update(int id, CategoryUpdateDto item);
        Task Delete(int id);
    }
}
