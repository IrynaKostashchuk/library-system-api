using MediatR;
using Domain.Entities;
using Domain.Enums;
using Persistence.Context;


namespace Application.Features.BookFeatures.Commands
{
    public class CreateBookCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public double Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Guid AuthorId { get; set; }

        public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
        {
            private readonly IApplicationDbContext _context;

            public CreateBookCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
            {
                var book = new Book
                {
                    Title = command.Title,
                    Subtitle = command.Subtitle,
                    Description = command.Description,
                    Genre = command.Genre,
                    Publisher = command.Publisher,
                    ISBN = command.ISBN,
                    Rating = command.Rating,
                    ReleaseDate = command.ReleaseDate,
                    AuthorId = command.AuthorId
                };

                _context.Books.Add(book);
                await _context.SaveChanges();

                return book.Id;
            }
        }
    }
}