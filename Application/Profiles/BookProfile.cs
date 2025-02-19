using AutoMapper;
using BookManagement.Core.Application.Features.BookFeatures.Commands;
using BookManagement.Core.Application.Features.BookFeatures.Queries;
using BookManagement.Core.Domain.Models;

namespace BookManagement.Core.Application.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<CreateBookCommand.Request, Book>();
            CreateMap<CreateBooksBulkCommand.Request, Book>();

            CreateMap<UpdateBookCommand.Request, Book>();

            CreateMap<Book, GetBookByIdQuery.Response>();
            CreateMap<Book, GetBookByTitleQuery.Response>();
        }
    }
}
