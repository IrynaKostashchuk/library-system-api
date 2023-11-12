using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ReadingList.Api.Repositories;

public class ReadingListRepository: IReadingListRepository
{
    private readonly IDistributedCache _redisCache;
    public ReadingListRepository(IDistributedCache cache)
    {
        _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
    }
    public async Task<Entities.ReadingList> GetReadingList(string userName)
    {
        var list = await _redisCache.GetStringAsync(userName);
        if (String.IsNullOrEmpty(list))
            return null;
        return JsonConvert.DeserializeObject<Entities.ReadingList>(list);
    }
         
    public async Task<Entities.ReadingList> UpdateReadingList(Entities.ReadingList list)
    {
        await _redisCache.SetStringAsync(list.UserName, JsonConvert.SerializeObject(list));
             
        return await GetReadingList(list.UserName);
    }
    public async Task DeleteReadingList(string userName)
    {
        await _redisCache.RemoveAsync(userName);
    }
}