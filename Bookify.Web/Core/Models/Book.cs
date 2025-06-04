namespace Bookify.Web.Core.Models
{
    [Index(nameof(Title), nameof(Author), IsUnique = true)]
    public class Book : BaseModel
    {
        public int Id { get; set; }
        [MaxLength(500)]
        public string Title { get; set; } = null!;
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public string Publisher { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; } = null!;
        public string? ImageThumbailUrl { get; set; } = null!;
        [MaxLength(50)]
        public string Hall { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        public ICollection<BookCategory> Categories { get; set; } = new List<BookCategory>();
        public ICollection<BookCopy> Copies { get; set; } = new List<BookCopy>();
    }
}
