using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Repository
{
    public class BooksRepository : GenericRepository<Book>
    {
        public BooksRepository(ApplicationDbContext _db) :base(_db)
        {
        }
        public Book? GetByIdWithCategories(int id)
        {
            return db.Books.Include(b => b.Categories).ThenInclude(c => c.Category).SingleOrDefault(b => b.Id == id);
        }
        public bool CheckDuplicatedTitleAuthor(BookFormVM book)
        {
            return db.Books.Any(b =>
            b.Title.Equals(book.Title) &&
            b.AuthorId == book.AuthorId &&
            b.Id != book.Id);
        }
    }
}
