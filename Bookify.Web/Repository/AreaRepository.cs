
namespace Bookify.Web.Repository
{
    public class AreaRepository : GenericRepository<Area>
    {
        public AreaRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public List<Area> GetAreasByGovernorateId(int governorateId)
        {
            return db.Areas
         .Where(a => a.GovernorateId == governorateId)
         .OrderBy(a => a.Name)
         .ToList();

        }

    }
}
