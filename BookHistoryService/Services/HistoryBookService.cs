namespace BookHistoryService.Services
{
    public class HistoryBookService : BookService
    {
        public HistoryBookService() : base("historyBooks.json", "historyBookHistory.json") { }
    }
}
