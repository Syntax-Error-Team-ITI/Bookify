namespace Bookify.Web.Core.Models
{
    [PrimaryKey(nameof(BookId), nameof(CategoryId))]
    public class BookCategory
    {
        public int BookId { get; set; }
        public virtual Book? Book { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
