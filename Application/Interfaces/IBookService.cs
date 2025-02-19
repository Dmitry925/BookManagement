using BookManagement.Core.Domain.Models;

namespace BookManagement.Core.Application.Interfaces
{
    public interface IBookService
    {
        double CalculatePopularityScore(Book book);
    }
}
