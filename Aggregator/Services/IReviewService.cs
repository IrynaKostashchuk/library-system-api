using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Services;

public interface IReviewService
{
    Task<IActionResult> GetAll();
    Task<IActionResult> GetById(Guid id);
}