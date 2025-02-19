using BookManagement.Core.Domain.Common;

namespace BookManagement.Core.Domain.Models
{
    public class Book : BaseDataModel, ISoftDeletable
    {
        public string Title { get; set; }

        public int PublicationYear { get; set; }

        public string Author { get; set; }

        public int Views { get; set; }

        public bool IsDeleted { get; set; }
    }
}
