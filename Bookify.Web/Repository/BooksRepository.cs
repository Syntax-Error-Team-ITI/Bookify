using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Repository
{
    public class BooksRepository : GenericRepository<Book>
    {
        public BooksRepository(ApplicationDbContext _db) :base(_db)
        {
        }
        public Book? GetByIdWithAllRelations(int id)
        {
            return db.Books.Include(b => b.Categories).ThenInclude(c => c.Category).Include(b => b.Author).Include(b =>b.Copies).SingleOrDefault(b => b.Id == id);
        }
        public bool CheckDuplicatedTitleAuthor(BookFormVM book)
        {
            return db.Books.Any(b =>
            b.Title.Equals(book.Title) &&
            b.AuthorId == book.AuthorId &&
            b.Id != book.Id);
        }
        public List<Book> GetBooksWith() {
            return db.Books.Include(b => b.Author).ToList();
        }
    }
}
