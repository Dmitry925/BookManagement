using BookManagement.Core.Application.Interfaces;
using BookManagement.Core.Domain.Models;
using BookManagement.Infrastructure.Persistence.Context;
using BookManagement.Infrastructure.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookManagement.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();

            services.AddIdentity<User, IdentityRole<Guid>>(opt =>
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequiredLength = 8;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireNonAlphanumeric = true;
                    opt.Password.RequireUppercase = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
