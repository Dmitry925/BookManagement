using BookManagement.Core.Domain.Enums;
using BookManagement.Core.Domain.Models;
using BookManagement.Infrastructure.Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure.Persistence.Context
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedEssentialsAsync(RoleManager<IdentityRole<Guid>> roleManager, UserManager<User> userManager)
        {
            var roles = Enum.GetNames(typeof(BaseRole));
            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            foreach (var role in roleManager.Roles.ToList())
            {
                if (!roles.Contains(role.Name))
                {
                    await roleManager.DeleteAsync(role);
                }
            }

            var usersList = DemoUsers.DemoUsersList;
            foreach (var demoUsers in usersList)
            {
                foreach (var user in demoUsers.Value)
                {
                    if (!await userManager.Users.AnyAsync(x => x.UserName.Equals(user.UserName)))
                    {
                        var createdUser = await userManager.CreateAsync(user, DemoUsers.DefaultPassword);
                        if (createdUser.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, demoUsers.Key.ToString());
                        }
                    }
                }
            }
        }

        public static async Task SeedEBooksAsync(ApplicationDbContext context)
        {
            var booksList = DemoBooks.DemoBooksList;
            foreach (var book in booksList)
            {
                if (!await context.Books.AnyAsync(b => b.Title.Equals(book.Title)))
                {
                    await context.Books.AddAsync(book);
                }
            }
            context.SaveAsync();
        }
    }
}
