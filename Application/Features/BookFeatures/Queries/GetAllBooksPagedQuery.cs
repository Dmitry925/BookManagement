using BookManagement.Core.Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookManagement.Core.Application.Features.BookFeatures.Queries
{
    public class GetAllBooksPagedQuery
    {
        public class Request : IRequest<Response>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IApplicationDbContext _context;
            private readonly IBookService _bookService;

            public Handler(IApplicationDbContext context, IBookService bookService)
            {
                _context = context;
                _bookService = bookService;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var titles = _context.Books.AsEnumerable()
                    .OrderByDescending(b => _bookService.CalculatePopularityScore(b))
                    .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).Select(b => b.Title).ToList();
                
                if (titles.Count == 0)
                {
                    return null;
                }

                return new Response
                {
                    Titles = titles
                };
            }
        }

        public class Response
        {
            public List<string> Titles { get; set; }
        }
    }
}
