using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labb2.DTOs;
using Labb2.Entities;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Labb2.Controllers
{
    [ApiController]
    [Route("Books")]
    public class BookController : ControllerBase
    {
        [HttpGet("List books")]
        public async Task<IEnumerable<BookDto>> Get(bool ListonlyAvailableBooks = false)
        {
            using var db = new ApplicationDbContext();

            IQueryable<Book> booksQuery = db.Books;

            if (ListonlyAvailableBooks)
            {
                booksQuery = booksQuery.Where(b => b.IsAvailable);
            }

            var books = await booksQuery.ToListAsync();

            return books.Select(x => new BookDto
            {
                BookId = x.BookId,
                Title = x.Title,
                ISBN = x.ISBN,
                ReleaseYear = x.ReleaseYear,
                IsAvailable = x.IsAvailable,
            });
        }

        [HttpGet("Look up specific book")]
        public async Task<ActionResult<BookDto>> GetSingleBook(int id)
        {
            using var db = new ApplicationDbContext();

            var book = await db.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var bookDto = new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                ISBN = book.ISBN,
                ReleaseYear = book.ReleaseYear,
                IsAvailable = book.IsAvailable,
            };

            return bookDto;
        }

        [HttpPost("Add book")]
        public async Task<IActionResult> Post(CreateBookDto createBookDto)
        {
            using var db = new ApplicationDbContext();


            if (!IsValidIsbnFormat(createBookDto.ISBN))
            {
                return BadRequest("Invalid ISBN format. It must match the XXX-X-XX-XXXXXX-X format where X must be a digit.");
            }
            
            bool IsValidIsbnFormat(string isbn)
            {
                string isbnPattern = @"^\d{3}-\d{1}-\d{2}-\d{6}-\d{1}$";
                return Regex.IsMatch(isbn, isbnPattern);
            }


            int currentYear = DateTime.Now.Year;
            if (createBookDto.ReleaseYear <= 0 || createBookDto.ReleaseYear > currentYear)
            {
                return BadRequest("Release year must be greater than 0 and less than or equal to the current year. If you do want to add a book released in the BC era, use year 0 as a placeholder instead");
            }
            

            var entity = new Book()
            {
                Title = createBookDto.Title,
                ISBN = createBookDto.ISBN,
                ReleaseYear = createBookDto.ReleaseYear, 
                IsAvailable = true
            };

            await db.AddAsync(entity);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete(("Remove book"))]
        public async Task<IActionResult> Delete(int id)
        {
            using var db = new ApplicationDbContext();

            var book = await db.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(); 
            }

            db.Books.Remove(book);
            await db.SaveChangesAsync();

            return NoContent(); 
        }

    }

}
