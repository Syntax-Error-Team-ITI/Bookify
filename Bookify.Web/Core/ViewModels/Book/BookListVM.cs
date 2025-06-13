namespace Bookify.Web.Core.ViewModels.Book
{
    public class BookListVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public string Hall { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
