using BookManagement.Core.Application.Interfaces;
using BookManagement.Core.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<Book> Books { get; set; }

        public Task<int> SaveAsync()
        {
            return base.SaveChangesAsync();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasIndex(b => b.Title).IsUnique();
            modelBuilder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
