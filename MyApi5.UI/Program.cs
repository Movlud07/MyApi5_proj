using FluentValidation.AspNetCore;
using MyApi5.Business.Validation.FluentValidation;
using MyApi5.UI.Filters;

namespace MyApi5.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            builder.Services.AddFluentValidation(fv =>
            {
                // Validasiya siniflərini qeydə alır
                fv.RegisterValidatorsFromAssembly(typeof(CategoryPostValidation).Assembly);
            });


            builder.Services.AddHttpClient();

            //---------------------------------------
            builder.Services.AddScoped<AuthFilter>();
            //---------------------------------------



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            //app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapDefaultControllerRoute();
            });

            app.Run();
        }
    }
}
