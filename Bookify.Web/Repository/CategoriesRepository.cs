namespace Bookify.Web.Repository
{
    public class CategoriesRepository : GenericRepository<Category>
    {
        public CategoriesRepository(ApplicationDbContext _db) : base(_db)
        {
        }
        public IEnumerable<Category> GetNotDeleted()
        {
            return db.Categories.Where(e => !e.IsDeleted).ToList();
        }
    }
}
