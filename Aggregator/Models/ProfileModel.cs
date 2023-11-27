namespace Aggregator.Models;

public class ProfileModel
{
    public string UserName { get; set; }
    
    public ReadingListModel ReadingList { get; set; }
    
    public IEnumerable<ReviewModel> Reviews { get; set; }
}