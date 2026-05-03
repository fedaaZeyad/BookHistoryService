using Microsoft.AspNetCore.Mvc;
using BookHistoryService.Models;
using BookHistoryService.Services;

namespace BookHistoryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet(Name = "GetAllBooks")]
        public ActionResult<IEnumerable<Book>> GetAll(
            [FromQuery] int? id,
            [FromQuery] string? author,
            [FromQuery] string? title,
            [FromQuery] string? sortBy,
            [FromQuery] bool sortDesc = false)
        {
            return Ok(_bookService.GetAll(id, author, title, sortBy, sortDesc));
        }

        [HttpPut("{id:int}", Name = "UpdateBook")]
        public ActionResult<Book> Update(int id, [FromBody] Book updated)
        {
            var book = _bookService.Update(id, updated);
            if (book is null)
                return NotFound(new { message = $"Book with id {id} not found." });
            return Ok(book);
        }

        [HttpGet("{id:int}/history", Name = "GetBookHistory")]
        public ActionResult<IEnumerable<BookHistoryEntry>> GetHistory(int id)
        {
            return Ok(_bookService.GetHistory(id));
        }
    }
}
