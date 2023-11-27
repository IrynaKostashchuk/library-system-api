using System.Collections;
using System.Text.Json;
using Aggregator.Extensions;
using Aggregator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Services;

public class ReviewService: IReviewService
{
    private readonly HttpClient _client;
    
    public ReviewService(HttpClient client)
    {
        _client = client ?? throw new System.ArgumentNullException(nameof(client));
    }

    public async Task<IEnumerable<ReviewModel>> GetByUserName(string userName)
    {
        var response = await _client.GetAsync($"/api/v1/Review/username/{userName}");
        
        return await response.ReadContentAs<List<ReviewModel>>();
    
    }
}