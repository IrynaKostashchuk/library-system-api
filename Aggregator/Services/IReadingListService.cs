using Aggregator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Services;

public interface IReadingListService
{
    Task<ReadingListModel> GetReadingList(string userName);
}