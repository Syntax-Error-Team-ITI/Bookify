
namespace Bookify.Web.Repository
{
    public class BookCopyRepository : GenericRepository<BookCopy>
    {
        public BookCopyRepository(ApplicationDbContext _db) : base(_db)
        { 
        }
        public BookCopy? GetByIdWithBook(int id)
        {
            return db.BookCopies.Include(c => c.Book).SingleOrDefault(b => b.Id == id);
        }
    
    }
}
