using Aggregator.Extensions;
using Aggregator.Models;

namespace Aggregator.Services;

public class ReadingListService: IReadingListService
{
    
    private readonly HttpClient _client;

    public ReadingListService(HttpClient client)
    {
        _client = client;
    }
    public async Task<ReadingListModel> GetReadingList(string userName)
    {
        var response = await _client.GetAsync($"/api/v1/ReadingList/{userName}");
        return await response.ReadContentAs<ReadingListModel>();
    }
}