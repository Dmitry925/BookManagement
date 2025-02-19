namespace BookManagement.Core.Application.DTOs.BookDTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; }

        public int PublicationYear { get; set; }

        public string Author { get; set; }
    }
}
