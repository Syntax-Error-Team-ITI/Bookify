using Bookify.Web.Core.ViewModels.BookCopies;

namespace Bookify.Web.Core.ViewModels.Book
{
    public class BookWithAvailabilityVM
    {

        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; }
        public string Hall { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IEnumerable<string> Categories { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public IEnumerable<BookCopyViewModel> Copies { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsAvailable => AvailableCopies > 0;
    }
}
