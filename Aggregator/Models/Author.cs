namespace Aggregator.Models;

public class Author
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime? DateOfBirth { get; set; }

    // One-to-many relationship with books
    public List<Book>? Books { get; set; }
}