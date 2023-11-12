using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Persistence.Context;

namespace Application.Features.ReviewFeatures.Commands
{
    public class CreateReviewCommand : IRequest<Guid>
    {
        public string Content { get; set; }
        public int Rating { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        
        public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
        {
            private readonly IApplicationDbContext _context;

            public CreateReviewCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Guid> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
            {
                var review = new Review
                {
                    Content = command.Content,
                    Rating = command.Rating,
                    UserId = command.UserId,
                    BookId = command.BookId
                };

                _context.Reviews.Add(review);
                await _context.SaveChanges();

                return review.Id;
            }
        }
    }
}