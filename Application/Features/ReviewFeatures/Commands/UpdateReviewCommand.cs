using MediatR;
using Persistence.Context;

namespace Application.Features.ReviewFeatures.Commands
{
    public class UpdateReviewCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }

        public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Guid>
        {
            private readonly IApplicationDbContext _context;

            public UpdateReviewCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Guid> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
            {
                var review = _context.Reviews
                    .Where(r => r.Id == command.Id)
                    .FirstOrDefault();

                if (review == null)
                {
                    return default; // Review not found.
                }
                else
                {
                    review.Content = command.Content;
                    review.Rating = command.Rating;
                    await _context.SaveChanges();
                    return review.Id;
                }
            }
        }
    }
}