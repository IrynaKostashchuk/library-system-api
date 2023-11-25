using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Services;

public interface IBookService
{
    Task<IActionResult> GetBooks();
    
}