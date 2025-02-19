using BookManagement.Core.Application.Infrastructure.Exceptions;
using BookManagement.Core.Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.BookFeatures.Commands
{
    public class SoftDeleteBookCommand
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;

            public Handler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var book = await _context.Books.FindAsync(request.Id);

                if (book == null)
                {
                    throw new NotFoundException(request.Id.ToString());
                }

                book.IsDeleted = true;
                await _context.SaveAsync();

                return new Response { };
            }
        }

        public class Response
        {
        }
    }
}
