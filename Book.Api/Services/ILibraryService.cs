using Book.Api.Models;

namespace Book.Api.Services;

public interface ILibraryService
{
    // Author Services
    Task<List<Author>> GetAuthorsAsync(); // GET All Authors
    Task<Author> GetAuthorAsync(Guid id, bool includeBooks = false); // GET Single Author
    Task<Author> AddAuthorAsync(Author author); // POST New Author
    Task<Author> UpdateAuthorAsync(Author author); // PUT Author
    Task<(bool, string)> DeleteAuthorAsync(Author author); // DELETE Author

    // Book Services
    Task<List<Models.Book>> GetBooksAsync(); // GET All Books
    Task<Models.Book> GetBookAsync(Guid id); // Get Single Book
    Task<Models.Book> AddBookAsync(Models.Book book); // POST New Book
    Task<Models.Book> UpdateBookAsync(Models.Book book); // PUT Book
    Task<(bool, string)> DeleteBookAsync(Models.Book book); // DELETE Book
}