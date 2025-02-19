namespace BookManagement.Core.Domain.Common
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
