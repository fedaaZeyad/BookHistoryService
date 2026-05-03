namespace BookHistoryService.Services
{
    public class ScienceBookService : BookService
    {
        public ScienceBookService() : base("scienceBooks.json", "scienceBookHistory.json") { }
    }
}
