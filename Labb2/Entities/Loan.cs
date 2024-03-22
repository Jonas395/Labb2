namespace Labb2.Entities
{
    public class Loan
    {
        public int LoanId { get; set; }

        public int BorrowerId { get; set; }

        public int BookId { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public Borrower Borrower { get; set; }
        public Book Book { get; set; }

    }
}
