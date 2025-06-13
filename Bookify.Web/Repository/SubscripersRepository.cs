namespace Bookify.Web.Repository
{
    public class SubscripersRepository : GenericRepository<Subscriber>
    {
        public SubscripersRepository(ApplicationDbContext _db) : base(_db)
        {
        }
    }
}
