using AutoMapper;
using BookManagement.Core.Application.DTOs.BookDTOs;
using BookManagement.Core.Application.Infrastructure;
using BookManagement.Core.Application.Infrastructure.Exceptions;
using BookManagement.Core.Application.Interfaces;
using BookManagement.Core.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.BookFeatures.Commands
{
    public class CreateBookCommand
    {
        public class Request : CreateBookDto, IRequest<Response>
        {

        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(v => v.Title)
                    .NotEmpty().WithMessage("Title is empty.")
                    .MinimumLength(Constants.MIN_TITLE_LENGTH).WithMessage($"Minimum title length is {Constants.MIN_TITLE_LENGTH} symbols")
                    .MaximumLength(Constants.MAX_TITLE_LENGTH).WithMessage($"Maximum title length is {Constants.MAX_TITLE_LENGTH} symbols");

                RuleFor(v => v.PublicationYear)
                    .NotEmpty().WithMessage("Publication year is empty.")
                    .InclusiveBetween(Constants.MIN_PUBLICATION_YEAR, DateTime.Now.Year).WithMessage($"Publication year must be between {Constants.MIN_PUBLICATION_YEAR} and {DateTime.Now.Year}");

                RuleFor(v => v.Author)
                    .NotEmpty().WithMessage("Author is empty.")
                    .MinimumLength(Constants.MIN_AUTHOR_NAME_LENGTH).WithMessage($"Minimum Author name length is {Constants.MIN_AUTHOR_NAME_LENGTH} symbols")
                    .MaximumLength(Constants.MAX_AUTHOR_NAME_LENGTH).WithMessage($"Maximum Author name length is {Constants.MAX_AUTHOR_NAME_LENGTH} symbols");
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (await _context.Books.AnyAsync(b => b.Title == request.Title))
                {
                    throw new AlreadyExistsException(request.Title);
                }
                
                var book = _mapper.Map<Book>(request);
                book.Id = Guid.NewGuid();
                book.Views = 0;
                
                await _context.Books.AddAsync(book);
                await _context.SaveAsync();

                return new Response
                {
                    Id = book.Id
                };
            }
        }

        public class Response
        {
            public Guid Id { get; set; }
        }
    }
}
