using Microsoft.Extensions.Hosting;

namespace Labb2.Entities
{
    public class Borrower
    {
        public int BorrowerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SocialSecurityNumber { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
