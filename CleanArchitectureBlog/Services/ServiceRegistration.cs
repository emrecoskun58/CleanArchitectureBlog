using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scrutor;

namespace CleanArchitectureBlog.Services
{
    public static class ServiceRegistration
    {
        public static void AddService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CleanArchitectureDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("localhost")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<CleanArchitectureDbContext>()
            .AddDefaultTokenProviders();

            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }



    }
}
