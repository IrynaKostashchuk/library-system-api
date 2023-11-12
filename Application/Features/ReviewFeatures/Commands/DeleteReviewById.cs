using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.ReviewFeatures.Commands
{
    public class DeleteReviewByIdCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        
        public class DeleteReviewByIdCommandHandler : IRequestHandler<DeleteReviewByIdCommand, Guid>
        {
            private readonly IApplicationDbContext _context;

            public DeleteReviewByIdCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Guid> Handle(DeleteReviewByIdCommand command, CancellationToken cancellationToken)
            {
                var review = await _context.Reviews
                    .Where(r => r.Id == command.Id)
                    .FirstOrDefaultAsync();

                if (review == null)
                {
                    return default; // Review not found.
                }

                _context.Reviews.Remove(review);
                await _context.SaveChanges();
                return review.Id;
            }
        }
    }
}