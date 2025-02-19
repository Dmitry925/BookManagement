using BookManagement.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Book> Books { get; set; }

        Task<int> SaveAsync();
    }
}
