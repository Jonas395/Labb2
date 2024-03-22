namespace Labb2.DTOs;

public class BookDto
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsAvailable { get; set; }
}
