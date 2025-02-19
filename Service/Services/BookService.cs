using BookManagement.Core.Application.Interfaces;
using BookManagement.Core.Domain.Models;
using System;

namespace BookManagement.Infrastructure.Service.Services
{
    public class BookService : IBookService
    {
        public double CalculatePopularityScore(Book book)
        {
            var yearsSincePublished = DateTime.Now.Year - book.PublicationYear;
            var result = (book.Views * 0.5) / Math.Pow(1.05, yearsSincePublished);

            return result;
        }
    }
}
