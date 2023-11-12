

namespace ReadingList.Api.Entities;

public class ReadingList
{
    public string UserName { get; set; }
    public List<Book> Books { get; set; } = new List<Book>();
    
    public ReadingList()
    {
    }
    public ReadingList(string userName)
    {
        UserName = userName;
    }
    
}