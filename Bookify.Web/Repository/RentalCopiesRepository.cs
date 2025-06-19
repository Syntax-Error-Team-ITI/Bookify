namespace Bookify.Web.Repository
{
    public class RentalCopiesRepository : GenericRepository<RentalCopy>
    {
        public RentalCopiesRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public bool IsCopyInRental(int copyId, int? excludeRentalId = null)
        {
            return db.RentalCopies.Any(c =>
                c.BookCopyId == copyId &&
                !c.ReturnDate.HasValue &&
                (excludeRentalId == null || c.RentalId != excludeRentalId));
        }

        public List<RentalCopy> GetCopiesForRental(int rentalId)
        {
            return db.RentalCopies
                .Include(c => c.BookCopy)
                    .ThenInclude(b => b.Book)
                .Where(c => c.RentalId == rentalId)
                .ToList();
        }

        public List<int> GetSubscriberBookIdsInRental(int subscriberId, int? excludeRentalId = null)
        {
            return db.Rentals
                .Where(r => r.SubscriberId == subscriberId && (!excludeRentalId.HasValue || r.Id != excludeRentalId))
                .SelectMany(r => r.RentalCopies)
                .Where(rc => !rc.ReturnDate.HasValue)
                .Select(rc => rc.BookCopy!.BookId)
                .Distinct()
                .ToList();
        }

        public int Count(int id)
        {
            return db.RentalCopies.Count(r => r.RentalId == id);
        }
        public int CountByRentalId(int rentalId)
        {
            return db.RentalCopies.Count(rc => rc.RentalId == rentalId);
        }

    }
}
