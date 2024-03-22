namespace Labb2.DTOs;

public class LoanDto
{ 
    public int LoanId { get; set; }
    public int BookId { get; set; }
    public int BorrowerId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set;}
}
