using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyApi5.Business.abstracts;
using MyApi5.Business.Errors;
using MyApi5.Business.Extensions;
using MyApi5.Business.Helpers;
using MyApi5.DataAccess;
using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyApi5.Business.concretes
{
    public class CategoryManager : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryManager(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public async Task Add(CategoryPostDto item)
        {
            var category = await _context.Categories.Where(x => x.Name == item.Name && x.IsDeleted == false).FirstOrDefaultAsync();
            if (category != null)
            {
                //throw new DublicateException();
                throw new RestException(StatusCodes.Status409Conflict, "Bu categoriya adinda movcud element var.");
            }

            if (!item.FormFile.IsImage())
            {
                throw new RestException(StatusCodes.Status406NotAcceptable, "FormFile", "File is not a image.");
            }
            if (!item.FormFile.FileSize())
            {
                throw new RestException(StatusCodes.Status411LengthRequired, "FormFile", "File size is greatest from default size.");
            }
            string imageName = item.FormFile.PutPlace(_env);

            Category newcategory = _mapper.Map<Category>(item);


            await _context.Categories.AddAsync(newcategory);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var category = await _context.Categories.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();

            if (category is null)
            {
                //throw new ElementNotExistException("Bele bir element yoxdu ki isdeletesinde true eliyesen.");
                throw new RestException(StatusCodes.Status404NotFound, "Bele bir element yoxdu ki isdeletesinde true eliyesen");
            }
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryGetDto>> GetAll(Expression<Func<Category, bool>> filter = null, params string[] includes)
        {
            IQueryable<Category> query = _context.Categories.Where(x => !x.IsDeleted);

            //List<CategoryGetDto> getDtos = new List<CategoryGetDto>();

            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter is null)
            {
                var categories = await query.ToListAsync();
                //foreach (var category in categories)
                //{
                //    CategoryGetDto getDto = new CategoryGetDto();

                //    int a = 0;
                //    foreach (var product in category.Products)
                //    {
                //        getDto.ProductsName.Add(product.Name);
                //        a++;
                //    }
                //    getDto.ProductsCount = category.Products.Count;
                //    _mapper.Map(category, getDto);

                //    getDtos.Add(getDto);
                //}
                return _mapper.Map<List<CategoryGetDto>>(categories);

            }
            else
            {
                var categories = await query.Where(filter).ToListAsync();
                //foreach (var category in categories)
                //{
                //    CategoryGetDto getDto = new CategoryGetDto();
                //    if (category.Products.Count > 0)
                //    {
                //        getDto.ProductsCount = category.Products.Count;

                //    }
                //    else
                //    {
                //        getDto.ProductsCount = 0;
                //    }
                //    _mapper.Map(category, getDto);

                //    getDtos.Add(getDto);
                //}
                return _mapper.Map<List<CategoryGetDto>>(categories);
            }
        }

        public async Task<CategoryGetDto> GetById(int id)
        {
            var category = await _context.Categories.Include(x => x.Products).Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            if (category == null)
            {
                //throw new ElementNotExistException("This element not exist in our database");
                throw new RestException(StatusCodes.Status404NotFound, "This element not found in our database.");

            }

            CategoryGetDto getDto = new CategoryGetDto();

            //if (category.ImagePath == null)
            //{
            //    getDto.ImageUrl = null;
            //}

            //int a = 0;

            //foreach (var item in category.Products)
            //{
            //    //getDto.ProductsName.Add(item.Name);
            //    a++;
            //}
            //getDto.ProductsCount = a;

            return _mapper.Map(category, getDto);
        }

        public async Task<CategoryGetDto> GetByName(string name)
        {
            var category = await _context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Name == name && x.IsDeleted == false);
            if (category == null)
            {
                //throw new ElementNotExistException("This element not exist in our database.");
                throw new RestException(StatusCodes.Status404NotFound, "This element not found in our database.");
            }

            CategoryGetDto getDto = new CategoryGetDto();

            //if (category.ImagePath == null)
            //{
            //    getDto.ImageUrl = null;
            //}

            //int a = 0;
            //foreach (var item in category.Products)
            //{
            //    //getDto.ProductsName.Add(item.Name);
            //    a++;
            //}
            //getDto.ProductsCount = a;

            return _mapper.Map(category, getDto);
        }

        public async Task Update(int id, CategoryUpdateDto item)
        {
            var dbCategory = await _context.Categories.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            if (dbCategory == null)
            {
                //throw new ElementNotExistException("This element not exist in our database!");
                throw new RestException(StatusCodes.Status404NotFound, "This element not found in our database.");
            }
            if (item.Name == dbCategory.Name)
            {
                //throw new DublicateException("This item and this name is exist in our database.");
                throw new RestException(StatusCodes.Status409Conflict, "Name", "This item and this name is exist in our database.");

            }
            string imageNamee = null;
            if (item.FormFile != null)
            {
                if (!item.FormFile.IsImage())
                {
                    throw new RestException(StatusCodes.Status406NotAcceptable, "FormFile", "File is not a image.");
                }
                if (!item.FormFile.FileSize())
                {
                    throw new RestException(StatusCodes.Status411LengthRequired, "FormFile", "File size is greatest from default size.");
                }
                string imageName = item.FormFile.PutPlace(_env);
                imageNamee = imageNamee;
            }
            _mapper.Map(item, dbCategory);

            Helper.DeleteFile(_env.WebRootPath, "update/img", imageNamee);

            await _context.SaveChangesAsync();
        }

    }
}

