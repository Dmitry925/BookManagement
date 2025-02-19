using AutoMapper;
using BookManagement.Core.Application.DTOs.BookDTOs;
using BookManagement.Core.Application.Infrastructure;
using BookManagement.Core.Application.Infrastructure.Exceptions;
using BookManagement.Core.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.BookFeatures.Queries
{
    public class GetBookByTitleQuery
    {
        public class Request : IRequest<Response>
        {
            public string Title { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(v => v.Title)
                    .NotEmpty().WithMessage("Title is empty.")
                    .MinimumLength(Constants.MIN_TITLE_LENGTH).WithMessage($"Minimum title length is {Constants.MIN_TITLE_LENGTH} symbols")
                    .MaximumLength(Constants.MAX_TITLE_LENGTH).WithMessage($"Maximum title length is {Constants.MAX_TITLE_LENGTH} symbols");
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IBookService _bookService;

            public Handler(IApplicationDbContext context, IMapper mapper, IBookService bookService)
            {
                _context = context;
                _mapper = mapper;
                _bookService = bookService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var book = await _context.Books.SingleOrDefaultAsync(b => b.Title.Equals(request.Title));

                if (book == null)
                {
                    throw new NotFoundException(request.Title);
                }

                book.Views++;
                await _context.SaveAsync();

                var response = _mapper.Map<Response>(book);
                response.PopularityScore = _bookService.CalculatePopularityScore(book);
                return response;
            }
        }

        public class Response : BookDetailsDto
        {

        }
    }
}
