
namespace ReadingList.Api.Repositories;

public interface IReadingListRepository
{
    Task<Entities.ReadingList> GetReadingList(string userName);
    Task<Entities.ReadingList> UpdateReadingList(Entities.ReadingList basket);
    Task DeleteReadingList(string userName);
}