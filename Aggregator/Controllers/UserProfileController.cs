using Aggregator.Models;
using Aggregator.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserProfileController: ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IReadingListService _readingListService;

    public UserProfileController(IReviewService reviewService, IReadingListService readingListService)
    {
        _reviewService = reviewService;
        _readingListService = readingListService;
    }

    [HttpGet("{userName}", Name = "GetProfile")]
    public async Task<ActionResult<ProfileModel>> GetProfile(string userName)
    {
        try
        {
            var readingList = await _readingListService.GetReadingList(userName);
            var reviews = await _reviewService.GetByUserName(userName);

            var profileModel = new ProfileModel
            {
                UserName = userName,
                ReadingList = readingList,
                Reviews = reviews,
            };

            return Ok(profileModel);
        }
        catch (Exception ex)
        {
            // Log the exception details
            Console.WriteLine($"Exception in GetProfile: {ex.Message}");
            Console.WriteLine(ex.InnerException);
            return StatusCode(500, "Internal Server Error");
        }
    }
}