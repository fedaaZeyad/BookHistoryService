namespace BookHistoryService.Models
{
    public class BookHistoryEntry
    {
        public int BookId { get; set; }
        public DateTime ChangedAt { get; set; }
        public List<rowChange> Changes { get; set; } = [];
    }

    public class rowChange
    {
        public string Field { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
    }
}
