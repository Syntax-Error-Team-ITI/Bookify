
namespace Bookify.Web.Repository
{
    public class GovernorateRepository : GenericRepository<Governorate>
    {
        public GovernorateRepository(ApplicationDbContext _db) : base(_db)
        {
        }
    }
}
