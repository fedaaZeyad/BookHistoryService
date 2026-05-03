using BookHistoryService.Models;

namespace BookHistoryService.Services
{
    public interface IBookService
    {
        List<Book> GetAll(int? id, string? author, string? title, string? sortBy, bool sortDesc);
        Book? Update(int id, Book updated);
        List<BookHistoryEntry> GetHistory(int id);
    }
}
