using System;

namespace BookManagement.Core.Application.DTOs.BookDTOs
{
    public class BookDetailsDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int PublicationYear { get; set; }

        public string Author { get; set; }

        public int Views { get; set; }

        public double PopularityScore { get; set; }
    }
}
