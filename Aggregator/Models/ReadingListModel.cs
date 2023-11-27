namespace Aggregator.Models;

public class ReadingListModel
{
    public string UserName { get; set; }
    public List<BookModel> Books { get; set; } = new List<BookModel>();
}