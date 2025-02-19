using BookManagement.Core.Application.Infrastructure.Exceptions;
using BookManagement.Core.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.BookFeatures.Commands
{
    public class SoftDeleteBooksBulkCommand
    {
        public class Request : IRequest<Response>
        {
            public List<Guid> Ids { get; set; }
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
                foreach (var id in request.Ids)
                {
                    if (await _context.Books.FindAsync(id) == null)
                    {
                        throw new NotFoundException(id.ToString());
                    }
                }

                var books = await _context.Books.Where(b => request.Ids.Contains(b.Id)).ToListAsync();

                foreach (var book in books)
                {
                    book.IsDeleted = true;
                }
                await _context.SaveAsync();

                return new Response { };
            }
        }

        public class Response
        {
        }
    }
}
