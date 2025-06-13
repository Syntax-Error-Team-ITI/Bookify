using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Bookify.Web.Repository
{
    public class SubscribersRepository : GenericRepository<Subscriber>
    {
        public SubscribersRepository(ApplicationDbContext _db) : base(_db)
        {
        }
        public List<Subscriber> Pagination(int page = 1, int recordNum = 10, string search = "")
        {
            return db.Subscripers
                .Include(s => s.Area)
                .ThenInclude(a => a.Governorate)
                .Where(s => (s.FirstName + " " + s.LastName).ToLower().Contains(search.ToLower()))
                .Skip((page - 1) * recordNum)
                .Take(recordNum)
                .ToList();
        }
        public int RecordCount(string search = "")
        {
            return db.Subscripers.Where(s => (s.FirstName + " " + s.LastName).ToLower().Contains(search.ToLower())).Count();
        }
        public bool IsNationalIdExists(string nationalId, int? excludeId = null)
        {
            return db.Subscripers.Any(s => s.NationalId == nationalId && (excludeId == null || s.Id != excludeId));
        }
        public bool IsEmailExists(string email, int? excludeId = null)
        {
            return db.Subscripers.Any(s => s.Email == email && (excludeId == null || s.Id != excludeId));
        }
        public bool IsMobileNumberExists(string mobile, int? excludeId = null)
        {
            return db.Subscripers.Any(s => s.MobileNumber == mobile && (excludeId == null || s.Id != excludeId));
        }
        public bool IsEmailExists(string email, int excludeId)
        {
            return db.Subscripers.Any(s => s.Email == email && s.Id != excludeId);
        }

        public bool IsNationalIdExists(string nationalId, int excludeId)
        {
            return db.Subscripers.Any(s => s.NationalId == nationalId && s.Id != excludeId);
        }

        public bool IsMobileNumberExists(string mobileNumber, int excludeId)
        {
            return db.Subscripers.Any(s => s.MobileNumber == mobileNumber && s.Id != excludeId);
        }
        public Subscriber? GetByIdWithAreaAndGovernorate(int id)
        {
            return db.Subscripers
                .Include(s => s.Area)
                .ThenInclude(a => a.Governorate)
                .FirstOrDefault(s => s.Id == id);
        }

    }
}
