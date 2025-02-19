using BookManagement.Core.Domain.Models;
using System;
using System.Collections.Generic;

namespace BookManagement.Infrastructure.Persistence.Seeds
{
    public static class DemoBooks
    {
        public static List<Book> DemoBooksList { get; set; }

        static DemoBooks()
        {
            DemoBooksList = new List<Book>
            {
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Housemaid",
                    PublicationYear = 2022,
                    Author = "Freida McFadden",
                    Views = 1000
                },

                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Sunrise on the Reaping",
                    PublicationYear = 2020,
                    Author = "Suzanne Collins",
                    Views = 700
                },

                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Lights Out",
                    PublicationYear = 2024,
                    Author = "Navessa Allen",
                    Views = 1200
                },

                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Me Talk Pretty One Day",
                    PublicationYear = 2000,
                    Author = "David Sedaris",
                    Views = 3000
                },

                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "A Visit from the Goon Squad",
                    PublicationYear = 2010,
                    Author = "Jennifer Egan",
                    Views = 2100
                },
            };
        }
    }
}
