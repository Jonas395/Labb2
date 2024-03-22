using System.Reflection.Metadata;

namespace Labb2.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int ReleaseYear { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<Loan> Loans { get; set; }
    }
}
