using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Review
{
    [Key]
    public Guid Id { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }

    // Many-to-one relationship with User (the reviewer)
    public Guid UserId { get; set; }
    public User User { get; set; }

    // Many-to-one relationship with Book
    public Guid BookId { get; set; }
    public Book Book { get; set; }
}