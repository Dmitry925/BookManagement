using AutoMapper;
using BookManagement.Core.Application.DTOs.BookDTOs;
using BookManagement.Core.Application.Infrastructure;
using BookManagement.Core.Application.Infrastructure.Exceptions;
using BookManagement.Core.Application.Interfaces;
using BookManagement.Core.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.BookFeatures.Commands
{
    public class UpdateBookCommand
    {
        public class Request : CreateBookDto, IRequest<Response>
        {
            [JsonIgnore]
            public Guid Id { get; set; }
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
            private readonly UserManager<User> _userManager;

            public Handler(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var book = await _context.Books.FindAsync(request.Id);

                if (book == null)
                {
                    throw new NotFoundException(nameof(book));
                }

                _mapper.Map(request, book);
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
