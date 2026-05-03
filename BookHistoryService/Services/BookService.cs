using System.Text.Json;
using BookHistoryService.Models;

namespace BookHistoryService.Services
{
    public abstract class BookService : IBookService
    {
        private readonly string _dataPath;
        private readonly string _historyPath;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

        protected BookService(string dataFile, string historyFile)
        {
            _dataPath    = Path.Combine(AppContext.BaseDirectory, "Data", dataFile);
            _historyPath = Path.Combine(AppContext.BaseDirectory, "Data", historyFile);
        }

        public List<Book> GetAll(int? id, string? author, string? title, string? sortBy, bool sortDesc)
        {
            var books = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(_dataPath), _jsonOptions) ?? [];

            if (id.HasValue)
                books = books.Where(b => b.Id == id.Value).ToList();

            if (!string.IsNullOrWhiteSpace(author))
                books = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(title))
                books = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

            books = sortBy?.ToLower() switch
            {
                "id"          => sortDesc ? books.OrderByDescending(b => b.Id).ToList()          : books.OrderBy(b => b.Id).ToList(),
                "author"      => sortDesc ? books.OrderByDescending(b => b.Author).ToList()      : books.OrderBy(b => b.Author).ToList(),
                "title"       => sortDesc ? books.OrderByDescending(b => b.Title).ToList()       : books.OrderBy(b => b.Title).ToList(),
                "publishdate" => sortDesc ? books.OrderByDescending(b => b.PublishDate).ToList() : books.OrderBy(b => b.PublishDate).ToList(),
                _             => books
            };

            return books;
        }

        public Book? Update(int id, Book updated)
        {
            var books = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(_dataPath), _jsonOptions) ?? [];
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book is null)
                return null;

            var fieldChanges = new List<rowChange>();

            if (!string.Equals(book.Title, updated.Title, StringComparison.Ordinal))
                fieldChanges.Add(new rowChange { Field = "Title", OldValue = book.Title, NewValue = updated.Title });

            if (!string.Equals(book.Author, updated.Author, StringComparison.Ordinal))
                fieldChanges.Add(new rowChange { Field = "Author", OldValue = book.Author, NewValue = updated.Author });

            if (!string.Equals(book.PublishDate, updated.PublishDate, StringComparison.Ordinal))
                fieldChanges.Add(new rowChange { Field = "PublishDate", OldValue = book.PublishDate, NewValue = updated.PublishDate });

            if (fieldChanges.Count > 0)
            {
                book.Title       = updated.Title;
                book.Author      = updated.Author;
                book.PublishDate = updated.PublishDate;

                File.WriteAllText(_dataPath, JsonSerializer.Serialize(books, _jsonOptions));

                var history = JsonSerializer.Deserialize<List<BookHistoryEntry>>(File.ReadAllText(_historyPath), _jsonOptions) ?? [];
                history.Add(new BookHistoryEntry { BookId = id, ChangedAt = DateTime.UtcNow, Changes = fieldChanges });
                File.WriteAllText(_historyPath, JsonSerializer.Serialize(history, _jsonOptions));
            }

            return book;
        }

        public List<BookHistoryEntry> GetHistory(int id)
        {
            var history = JsonSerializer.Deserialize<List<BookHistoryEntry>>(File.ReadAllText(_historyPath), _jsonOptions) ?? [];
            return [.. history.Where(h => h.BookId == id).OrderByDescending(h => h.ChangedAt)];
        }
    }
}
