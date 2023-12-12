using System.Text;
using Book.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Book.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public BookController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _libraryService.GetBooksAsync();
            if (books == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No books in database.");
            }

            return StatusCode(StatusCodes.Status200OK, books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooks(Guid id)
        {
            Models.Book book = await _libraryService.GetBookAsync(id);

            if (book == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No book found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, book);
        }
        
        private void PublishToMessageQueue(string integrationEvent, string eventData)
        {
            // TOOO: Reuse and close connections and channel, etc, 
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: "book_exchange",
                routingKey: integrationEvent,
                basicProperties: null,
                body: body);
        }

        [HttpPost]
        public async Task<ActionResult<Models.Book>> AddBook(Models.Book book)
        {
            var dbBook = await _libraryService.AddBookAsync(book);

            if (dbBook == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{book.Title} could not be added.");
            }
            
            var integrationEventData = JsonConvert.SerializeObject(new
            {
                id = book.Id,
                title = book.Title,
                subtitle = book.Subtitle,
                description = book.Description,
                genre = book.Genre,
                publisher = book.Publisher,
                isbn = book.ISBN,
                rating = book.Rating,
                releaseDate = book.ReleaseDate,
                authorId = book.AuthorId
            });
            
            PublishToMessageQueue("book.add", integrationEventData);

            return CreatedAtAction("GetBooks", new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, Models.Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            Models.Book dbBook = await _libraryService.UpdateBookAsync(book);

            if (dbBook == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{book.Title} could not be updated");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var book = await _libraryService.GetBookAsync(id);
            (bool status, string message) = await _libraryService.DeleteBookAsync(book);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, book);
        }
    }
}