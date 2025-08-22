using Microsoft.Extensions.DependencyInjection;
using MyApi5.Business.abstracts;
using MyApi5.Business.concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Extensions
{
    public static class ServiceExtension 
    {
        public static IServiceCollection AppServiceProvider(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddScoped<IUserService, UserManager>();



            return services;
        }
    }
}
