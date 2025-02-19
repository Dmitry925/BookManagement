using AutoMapper;
using BookManagement.Core.Application.DTOs.BookDTOs;
using BookManagement.Core.Application.Infrastructure.Exceptions;
using BookManagement.Core.Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.BookFeatures.Queries
{
    public class GetBookByIdQuery
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
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
                var book = await _context.Books.FindAsync(request.Id);

                if (book == null)
                {
                    throw new NotFoundException(request.Id.ToString());
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
