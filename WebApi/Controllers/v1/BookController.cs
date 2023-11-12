using Application.Features.BookFeatures.Commands;
using Application.Features.BookFeatures.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

public class BookController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
 
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllBooksQuery()));
    }
}