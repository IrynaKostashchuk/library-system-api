using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;


namespace Application.Features.BookFeatures.Queries
{
    public class GetAllBooksQuery : IRequest<IEnumerable<Book>>
    {
        public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<Book>>
        {
            private readonly IApplicationDbContext _context;

            public GetAllBooksQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Book>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
            {
                var bookList = await _context.Books.ToListAsync();
                if (bookList == null)
                {
                    return null;
                }
                return bookList.AsReadOnly();
            }
        }
    }
}