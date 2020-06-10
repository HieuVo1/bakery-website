using eShopSolution.Application.Catelog.Carts;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.System.Users;
using eShopSolution.WebApp.Services.Blogs;
using eShopSolution.WebApp.Services.Carts;
using eShopSolution.WebApp.Services.Categorys;
using eShopSolution.WebApp.Services.Comments;
using eShopSolution.WebApp.Services.Contacts;
using eShopSolution.WebApp.Services.Emails;
using eShopSolution.WebApp.Services.ImageProducts;
using eShopSolution.WebApp.Services.Languages;
using eShopSolution.WebApp.Services.Location;
using eShopSolution.WebApp.Services.Orders;
using eShopSolution.WebApp.Services.products;
using eShopSolution.WebApp.Services.Reviews;
using eShopSolution.WebApp.Services.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eShopSolution.WebApp
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
                  options.Cookie.Name = "UserLoginCookie";
                  options.LoginPath = "/Login/index/";
                  options.AccessDeniedPath = "/User/Forbidden/";
              });

            services.AddDbContext<EShopDbContext>();
            services.AddIdentity<UserApp, RoleApp>()
                .AddEntityFrameworkStores<EShopDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
            services.AddAuthentication()
           .AddGoogle(options =>
           {
               IConfigurationSection googleAuthNSection =
                Configuration.GetSection("Authentication:Google");
               options.ClientId = googleAuthNSection["ClientId"];
               options.ClientSecret = googleAuthNSection["ClientSecret"];
               options.ClaimActions.MapJsonKey("picture", "picture", "url");
               options.ClaimActions.MapJsonKey("locale", "locale", "string");
               options.SaveTokens = true;
           })
           .AddFacebook(options =>
           {
               IConfigurationSection faceBookAuthNSection =
                Configuration.GetSection("Authentication:FaceBook");
               options.AppId = faceBookAuthNSection["AppId"];
               options.AppSecret = faceBookAuthNSection["AppSecret"];
               options.Fields.Add("picture");
               options.Events = new OAuthEvents
               {
                   OnCreatingTicket = context =>
                   {
                       var identity = (ClaimsIdentity)context.Principal.Identity;
                       var profileImg = context.User.GetProperty("picture").GetProperty("data").GetProperty("url").ToString();
                       identity.AddClaim(new Claim("picture", profileImg));
                       return Task.CompletedTask;
                   }
               };
           });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserAPIClient, UserAPIClient>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IImageProductService, ImageProductService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddTransient<SignInManager<UserApp>, SignInManager<UserApp>>();
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
                app.UseDeveloperExceptionPage();
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
                defaults: new { controller = "product", action = "GetByURl" });

                endpoints.MapControllerRoute(name: "detaileBlog",
                pattern: "blog/detail/{blogId}/",
                defaults: new { controller = "blog", action = "detail" });

                endpoints.MapControllerRoute(name: "detaileBlog",
                pattern: "blog/detail/{blogId}/",
                defaults: new { controller = "blog", action = "detail" });

                endpoints.MapControllerRoute(name: "deletecmt",
                pattern: "blog/deletecomment/{commentId}/{blogId}/",
                defaults: new { controller = "blog", action = "deletecomment" });

                endpoints.MapControllerRoute(name: "blog",
                pattern: "blog/{categoryUrl?}",
                defaults: new { controller = "blog", action = "index" });

                endpoints.MapControllerRoute(name: "sendMail",
               pattern: "login/sendmail/{email}",
               defaults: new { controller = "login", action = "sendmail" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
