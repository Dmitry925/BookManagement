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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.BookFeatures.Commands
{
    public class CreateBooksBulkCommand
    {
        public class Request : List<CreateBookDto>, IRequest<Response>
        {

        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {

                RuleForEach(x => x).ChildRules(r =>
                {
                    r.RuleFor(v => v.Title)
                        .NotEmpty().WithMessage("Title is empty.")
                        .MinimumLength(Constants.MIN_TITLE_LENGTH).WithMessage($"Minimum title length is {Constants.MIN_TITLE_LENGTH} symbols")
                        .MaximumLength(Constants.MAX_TITLE_LENGTH).WithMessage($"Maximum title length is {Constants.MAX_TITLE_LENGTH} symbols");

                    r.RuleFor(v => v.PublicationYear)
                        .NotEmpty().WithMessage("Publication year is empty.")
                        .InclusiveBetween(Constants.MIN_PUBLICATION_YEAR, DateTime.Now.Year).WithMessage($"Publication year must be between {Constants.MIN_PUBLICATION_YEAR} and {DateTime.Now.Year}");

                    r.RuleFor(v => v.Author)
                        .NotEmpty().WithMessage("Author is empty.")
                        .MinimumLength(Constants.MIN_AUTHOR_NAME_LENGTH).WithMessage($"Minimum Author name length is {Constants.MIN_AUTHOR_NAME_LENGTH} symbols")
                        .MaximumLength(Constants.MAX_AUTHOR_NAME_LENGTH).WithMessage($"Maximum Author name length is {Constants.MAX_AUTHOR_NAME_LENGTH} symbols");
                });
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
                foreach (var req in request)
                {
                    if (await _context.Books.AnyAsync(b => b.Title == req.Title))
                    {
                        throw new AlreadyExistsException(req.Title);
                    }
                }

                var books = _mapper.Map<List<Book>>(request);
                var ids = new List<Guid>();

                foreach (var book in books)
                {
                    book.Id = Guid.NewGuid();
                    book.Views = 0;
                }

                ids.AddRange(books.Select(b => b.Id));
                await _context.Books.AddRangeAsync(books);
                await _context.SaveAsync();

                return new Response
                {
                    Ids = ids
                };
            }
        }

        public class Response
        {
            public List<Guid> Ids { get; set; }
        }
    }
}
