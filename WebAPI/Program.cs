using BookManagement.Core.Application.Infrastructure.Exceptions;
using BookManagement.Core.Domain.Models;
using BookManagement.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BookManagement.Presentation.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                var context = services.GetService<ApplicationDbContext>();

                if (context == null)
                {
                    throw new NotFoundException("Context");
                }
                await context.Database.MigrateAsync();

                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                    var userManager = services.GetRequiredService<UserManager<User>>();

                    await ApplicationDbContextSeed.SeedEssentialsAsync(roleManager, userManager);
                    await ApplicationDbContextSeed.SeedEBooksAsync(context);
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
