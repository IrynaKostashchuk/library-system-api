using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.ReviewFeatures.Queries;

public class GetReviewByUserNameQuery : IRequest<IEnumerable<Review>>
{
    public string UserName { get; set; }

    public class GetReviewByUserNameQueryHandler : IRequestHandler<GetReviewByUserNameQuery, IEnumerable<Review>>
    {
        private readonly IApplicationDbContext _context;

        public GetReviewByUserNameQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> Handle(GetReviewByUserNameQuery query, CancellationToken cancellationToken)
        {
            var reviews = _context.Reviews
                .Include(r => r.User)  // Include User entity to access UserName
                .Where(r => r.User.UserName == query.UserName)
                .ToList();

            return reviews;
        }
    }
}