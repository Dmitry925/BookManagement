using BookManagement.Core.Application.Interfaces;
using BookManagement.Infrastructure.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookManagement.Infrastructure.Service
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
        }
    }
}
