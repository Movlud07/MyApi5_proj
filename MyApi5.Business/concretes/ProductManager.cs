using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MyApi5.Business.abstracts;
using MyApi5.Business.Errors;
using MyApi5.DataAccess;
using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.concretes
{
    public class ProductManager : IProductService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public ProductManager(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(ProductPostDto item)
        {
            var product = await _context.Products.Where(x => x.Name == item.Name && x.IsDeleted == false).FirstOrDefaultAsync();
            if (product != null)
            {
                throw new RestException(StatusCodes.Status409Conflict, "This product is exists in our database.");
            }

            Product newProduct = _mapper.Map<Product>(item);
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var product = await _context.Products.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, $"This {id} element not exists");
            }
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductGetDto>> GetAll(Expression<Func<Product, bool>> filter = null,params string[] includes)
        {

            IQueryable<Product> query = _context.Products.Where(x => x.IsDeleted == false);

            if(includes is not null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (filter == null)
            {
                var products = query.ToList();
                return _mapper.Map<List<ProductGetDto>>(products);
            }
            else
            {
                var products = await query.Where(filter).ToListAsync();
                return _mapper.Map<List<ProductGetDto>>(products);
            }
        }

        public async Task<ProductGetDto> GetById(int id)
        {
            var product = await _context.Products.Include(x=>x.Category).Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, $"This {id} product element not exist in our database.");
            }
            return _mapper.Map<ProductGetDto>(product);
        }

        public async Task<ProductGetDto> GetByName(string name)
        {
            var product = await _context.Products.Include(x=>x.Category).Where(x => x.Name == name && x.IsDeleted == false).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, $"This {name} ed product not exist in our database");
            }
            return _mapper.Map<ProductGetDto>(product);
        }

        public async Task Update(int id, ProductUpdateDto item)
        {
            var dbProduct = await _context.Products.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
            if (dbProduct == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, "This element not exist in our database.");
            }

            _mapper.Map(item, dbProduct);

            await _context.SaveChangesAsync();
        }
    }
}
