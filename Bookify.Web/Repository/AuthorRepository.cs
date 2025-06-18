
namespace Bookify.Web.Repository
{
    public class AuthorRepository : GenericRepository<Author>
    {
        public AuthorRepository(ApplicationDbContext _db) : base(_db)
        {
        }
        public IEnumerable<Author> GetNotDeleted()
        {
            return db.Authors.Where(e => !e.IsDeleted).ToList();
        }
    }
}
