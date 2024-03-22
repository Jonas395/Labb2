using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Labb2.DTOs;
using Labb2.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Labb2.Controllers
{
    [ApiController]
    [Route("Borrowers")]
    public class BorrowerController : ControllerBase
    {
        [HttpGet("List all borrowers")]
        public async Task<IEnumerable<BorrowerDto>> Get()
        {
            using var db = new ApplicationDbContext();

            var borrowers = await db.Borrowers.ToListAsync();

            return borrowers.Select(x => new BorrowerDto
            {
                BorrowerId = x.BorrowerId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                SocialSecurityNumber = x.SocialSecurityNumber
            });
        }
        

        [HttpPost("Add a new borrower")]
        public async Task <IActionResult> Post(CreateBorrowerDto createBorrowerDto)
        {
            using var db = new ApplicationDbContext();

            if (await db.Borrowers.AnyAsync(borrower => borrower.SocialSecurityNumber == createBorrowerDto.SocialSecurityNumber))
            {
                return BadRequest("Social security number must be unique.");
            }
            if (createBorrowerDto.SocialSecurityNumber.ToString().Length != 5)
            {
                return BadRequest("SocialSecurityNumber must be 5 digits.");
            }
            if (createBorrowerDto.FirstName.Trim().Length == 0 || createBorrowerDto.LastName.Trim().Length == 0)
            {
                return BadRequest("First name can't be empty");
            }
            

            var entity = new Borrower()
            {
                FirstName = createBorrowerDto.FirstName,
                LastName = createBorrowerDto.LastName,
                SocialSecurityNumber = createBorrowerDto.SocialSecurityNumber
            };

            await db.AddAsync(entity);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("Delete a borrower")]
        public async Task<IActionResult> Delete(int id)
        {
            using var db = new ApplicationDbContext();

            var borrower = await db.Borrowers.FindAsync(id);

            if (borrower == null)
            {
                return NotFound(); 
            }

            db.Borrowers.Remove(borrower);
            await db.SaveChangesAsync();

            return NoContent(); 
        }
    }

}
