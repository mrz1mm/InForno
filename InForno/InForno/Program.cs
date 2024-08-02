using InForno.Models;
using InForno.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace InForno
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Home";
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Supplier, policy => policy.RequireRole("Supplier"));
                options.AddPolicy(Policies.Customer, policy => policy.RequireRole("Customer"));
                options.AddPolicy("SupplierOrCustomer", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            (c.Type == ClaimTypes.Role && c.Value == "Supplier") ||
                            (c.Type == ClaimTypes.Role && c.Value == "Customer"))));
            });

            builder.Services.AddDbContext<InFornoDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services
                .AddScoped<IAuthSvc, AuthSvc>()
                .AddScoped<ICartSvc, CartSvc>()
                .AddScoped<IImageSvc>(x => new ImageSvc(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")))
                .AddScoped<IIngredientSvc, IngredientSvc>()
                .AddScoped<IOrderSvc, OrderSvc>()
                .AddScoped<IProductSvc, ProductSvc>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
