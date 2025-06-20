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
        public IQueryable<Book> GetBooksWith() {
            return db.Books.Include(b => b.Author);
        }
        public List<Book> Pagination(int page = 1, int recordNum = 10, string search = "")
        {
            return db.Books.Include(b => b.Author).Include(b => b.Copies).Where(b => b.Title.ToLower().Contains(search.ToLower()) || b.Author.Name.ToLower().Contains(search.ToLower())).Skip(page).Take(recordNum).ToList();
        }
        public int RecordCount(string search = "")
        {
            return db.Books.Where(b => b.Title.ToLower().Contains(search.ToLower())).Count();
        }


        public List<Book> GetBooksWithIncludes(string? search = null)
        {
            var query = db.Books
                .Include(b => b.Author)
                .Include(b => b.Copies)
                .Include(b => b.Categories)
                    .ThenInclude(c => c.Category)
                .Where(b => !b.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Author!.Name.Contains(search));
            }

            return query.ToList();
        }


    }
}
