using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.ReviewFeatures.Queries
{
    public class GetAllReviewsQuery : IRequest<IEnumerable<Review>>
    {
        public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, IEnumerable<Review>>
        {
            private readonly IApplicationDbContext _context;

            public GetAllReviewsQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Review>> Handle(GetAllReviewsQuery query, CancellationToken cancellationToken)
            {
                var reviewList = await _context.Reviews.ToListAsync();

                if (reviewList == null)
                {
                    return null;
                }

                return reviewList.AsReadOnly();
            }
        }
    }
}