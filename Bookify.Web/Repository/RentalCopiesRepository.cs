namespace Bookify.Web.Repository
{
    public class RentalCopiesRepository : GenericRepository<RentalCopy>
    {
        public RentalCopiesRepository(ApplicationDbContext _db) : base(_db)
        {
        }
        public int Count(int id)
        {
            return db.RentalCopies.Count(r => r.RentalId == id);
        }
    }
}
