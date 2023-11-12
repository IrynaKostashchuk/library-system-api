using Domain.Entities;
using MediatR;
using Persistence.Context;

namespace Application.Features.ReviewFeatures.Queries
{
    public class GetReviewByIdQuery : IRequest<Review>
    {
        public Guid Id { get; set; }

        public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, Review>
        {
            private readonly IApplicationDbContext _context;

            public GetReviewByIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Review> Handle(GetReviewByIdQuery query, CancellationToken cancellationToken)
            {
                var review = _context.Reviews
                    .Where(r => r.Id == query.Id)
                    .FirstOrDefault();

                if (review == null)
                {
                    return null;
                }

                return review;
            }
        }
    }
}