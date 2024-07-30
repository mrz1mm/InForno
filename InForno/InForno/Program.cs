using Microsoft.AspNetCore.Authentication.Cookies;
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

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Home";
            });

            builder.Services
            .AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Supplier, policy => policy.RequireRole("Admin"));
                options.AddPolicy(Policies.Customer, policy => policy.RequireRole("User"));
                options.AddPolicy("SupllierOrCustomer", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            (c.Type == ClaimTypes.Role && c.Value == "User") ||
                            (c.Type == ClaimTypes.Role && c.Value == "Admin"))));
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
