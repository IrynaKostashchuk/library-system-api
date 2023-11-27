using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadingList.Api.Entities;
using ReadingList.Api.Repositories;

namespace ReadingList.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ReadingListController : ControllerBase
{
    private readonly IReadingListRepository _repository;
    public ReadingListController(IReadingListRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    [HttpGet("{userName}", Name = "GetReadingList")]
    [ProducesResponseType(typeof(Entities.ReadingList), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Entities.ReadingList>> GetReadingList(string userName)
    {
        var list = await _repository.GetReadingList(userName);
        return Ok(list ?? new Entities.ReadingList(userName));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Entities.ReadingList), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Entities.ReadingList>> UpdateReadingList([FromBody] Entities.ReadingList list)
    {
        return Ok(await _repository.UpdateReadingList(list));
    }
    
    [HttpDelete("{userName}", Name = "DeleteReadingList")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteReadingList(string userName)
    {
        await _repository.DeleteReadingList(userName);
        return Ok();
    }
}