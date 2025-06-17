namespace Bookify.Web.Repository
{
    public class RentalRepository : GenericRepository<Rental>
    {
        public RentalRepository(ApplicationDbContext _db) : base(_db)
        {
        }
    }
}
