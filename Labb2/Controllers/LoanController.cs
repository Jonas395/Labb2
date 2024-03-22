using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labb2.DTOs;
using Labb2.Entities;
using Microsoft.AspNetCore.Mvc.Routing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Labb2.Controllers
{
    [ApiController]
    [Route("Loans")]
    public class LoanController : ControllerBase
    {
        [HttpGet("List all loans")]
        public async Task<IEnumerable<LoanDto>> Get()
        {
            using var db = new ApplicationDbContext();

            var loans = await db.Loans.ToListAsync();

            return loans.Select(x => new LoanDto
            {
                LoanId = x.LoanId,
                BookId = x.BookId,
                BorrowerId = x.BorrowerId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            });
        }

        [HttpPost("Borrow a book")]
        public async Task<IActionResult> Post(CreateLoanDto createLoanDto)
        {
            using var db = new ApplicationDbContext();

            

            var entity = new Loan()
            {
                
                BookId = createLoanDto.BookId,
                BorrowerId = createLoanDto.BorrowerId,
                StartDate = DateOnly.FromDateTime(DateTime.Now),

            };



            var borrower = await db.Borrowers.FindAsync(createLoanDto.BorrowerId);

            if (borrower == null)
            {
                return BadRequest("Borrower does not exist");
            }

            var book = await db.Books.FindAsync(createLoanDto.BookId);

            if (book == null)
            {
                return BadRequest("Book does not exist");
            }

            if (book.IsAvailable == false)
            {
                return BadRequest("Chosen book is not available right now");
            }
            

            if (book != null)
            {
                book.IsAvailable = false;
            }

            await db.AddAsync(entity);
            await db.SaveChangesAsync();
            return Ok();
        }

        
        [HttpPost("Close a loan and return the book")]
        public async Task<IActionResult> ReturnBook(int loanId)
        {
            using var db = new ApplicationDbContext();

            var loan = await db.Loans.FindAsync(loanId);

            if (loan == null)
            {
                return NotFound();
            }

            loan.EndDate = DateOnly.FromDateTime(DateTime.Now);

            var book = await db.Books.FindAsync(loan.BookId);
            book.IsAvailable = true;


            db.Loans.Update(loan);

            await db.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("Delete a loan")]
        public async Task<IActionResult> Delete(int id)
        {
            using var db = new ApplicationDbContext();

            var loan = await db.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }
            if (loan.EndDate == null)
            {
                return BadRequest("Loan must have ended before it can be deleted");
            }

            db.Loans.Remove(loan);

            await db.SaveChangesAsync();

            return NoContent();
        }
    }

}
