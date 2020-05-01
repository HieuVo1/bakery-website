using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.AdminApp.Service.Categorys;
using eShopSolution.AdminApp.Service.Users;
using eShopSolution.AdminApp.Service.Languages;
using eShopSolution.ViewModel.System.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using eShopSolution.AdminApp.Service.Products;
using eShopSolution.AdminApp.Service.ImageProducts;
using Microsoft.AspNetCore.Identity;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace eShopSolution.AdminApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Login/index/";
                   options.AccessDeniedPath = "/User/Forbidden/";
               });

            services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserAPIClient, UserAPIClient>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<IImageProductService, ImageProductService>();
            services.AddTransient<IProductService, ProductService>();
            IMvcBuilder builder = services.AddRazorPages();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

#if DEBUG
            if (environment == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                //index product
                endpoints.MapControllerRoute(name: "product",
                pattern: "product/{categoryUrl}",
                defaults: new { controller = "product", action = "Index" });
                //create product
                endpoints.MapControllerRoute(name: "newproduct",
                pattern: "product/{categoryUrl}/newproduct",
                defaults: new { controller = "product", action = "newproduct" });
                //edit product
                endpoints.MapControllerRoute(name: "editProduct",
                pattern: "product/{categoryUrl}/edit/{productId}/{languageId}",
                defaults: new { controller = "product", action = "edit" });
                //get image product
                endpoints.MapControllerRoute(name: "imageProduct",
                pattern: "product/{productId}/Images",
                defaults: new { controller = "imageProduct", action = "index" });
                //update image product
                endpoints.MapControllerRoute(name: "UpdateImageProduct",
                pattern: "product/{productId}/Images/update/{imageId}",
                defaults: new { controller = "imageProduct", action = "update" });
                //remove image
                endpoints.MapControllerRoute(name: "RemoveImageProduct",
                pattern: "product/{productId}/Images/delete/{imageId}",
                defaults: new { controller = "imageProduct", action = "delete" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
