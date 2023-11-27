using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Features.ReviewFeatures.Commands;
using Application.Features.ReviewFeatures.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    public class ReviewController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllReviewsQuery()));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetReviewByIdQuery { Id = id };
            var review = await Mediator.Send(query);
            
            if (review == null)
            {
                return NotFound(); // Review not found.
            }
            
            return Ok(review);
        }
        
        [HttpGet("username/{userName}")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            /*var query = new GetReviewByUserNameQuery { UserName = userName };
            var review = await Mediator.Send(query);

            if (review == null)
            {
                return NotFound(); // Review not found.
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Add other options as needed
            };

            var jsonString = JsonSerializer.Serialize(review, options);
            return Ok(jsonString);*/
            var query = new GetReviewByUserNameQuery { UserName = userName };
            var review = await Mediator.Send(query);

            if (review == null)
            {
                return NotFound(); // Review not found.
            }

            return Ok(review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateReviewCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var result = await Mediator.Send(command);

            if (result == null)
            {
                return NotFound(); // Review not found.
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteReviewByIdCommand { Id = id };
            var result = await Mediator.Send(command);
            
            if (result == Guid.Empty)
            {
                return NotFound(); // Review not found.
            }

            return NoContent(); // Successfully deleted.
        }
    }
}