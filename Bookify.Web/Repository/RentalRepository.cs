namespace Bookify.Web.Repository
{
    public class RentalRepository : GenericRepository<Rental>
    {
        public RentalRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public Rental? GetWithCopiesAndBook(int id)
        {
            return db.Rentals
                .Include(r => r.RentalCopies)
                    .ThenInclude(c => c.BookCopy)
                        .ThenInclude(b => b.Book)
                .FirstOrDefault(r => r.Id == id);
        }
        public Rental? GetByIdWithCopies(int? id)
        {
            return db.Rentals
                .Include(r => r.RentalCopies)
                .FirstOrDefault(r => r.Id == id);
        }


        public Rental? GetWithSubscriberAndCopies(int id)
        {
            return db.Rentals
                .Include(r => r.RentalCopies)
                .Include(r => r.Subscriber)
                    .ThenInclude(s => s.Subscriptions)
                .FirstOrDefault(r => r.Id == id);
        }

        public Rental? GetByIdWithFullDetails(int id)
        {
            return db.Rentals
                .Include(r => r.RentalCopies)
                    .ThenInclude(rc => rc.BookCopy)
                        .ThenInclude(bc => bc.Book)
                .Include(r => r.Subscriber)
                    .ThenInclude(s => s.Subscriptions)
                .Include(r => r.Subscriber)
                    .ThenInclude(s => s.Rentals)
                        .ThenInclude(r => r.RentalCopies)
                .FirstOrDefault(r => r.Id == id);
        }

        public void MarkAsDeleted(int id, string userId)
        {
            var rental = db.Rentals.Find(id);
            if (rental is null || rental.CreatedOn.Date != DateTime.Today)
                return;

            rental.IsDeleted = true;
            rental.LastUpdatedOn = DateTime.Now;
            rental.LastUpdatedById = userId;
            db.SaveChanges();
        }

        public List<Rental> GetSubscriberRentalsWithCopies(int subscriberId, int? excludeRentalId = null)
        {
            return db.Rentals
                .Where(r => r.SubscriberId == subscriberId && (!excludeRentalId.HasValue || r.Id != excludeRentalId))
                .Include(r => r.RentalCopies)
                    .ThenInclude(rc => rc.BookCopy)
                .ToList();
        }

        public Rental? GetForEdit(int id)
        {
            return db.Rentals
                .Include(r => r.RentalCopies)
                .FirstOrDefault(r => r.Id == id);
        }

        public Rental? GetForReturn(int id)
        {
            return db.Rentals
               .Include(r => r.RentalCopies)
                   .ThenInclude(c => c.BookCopy)
                       .ThenInclude(b => b.Book)
               .Include(r => r.Subscriber)
                   .ThenInclude(s => s.Subscriptions)
               .FirstOrDefault(r => r.Id == id);
        }

        public Subscriber? GetSubscriberWithRentalsAndSubscriptions(int subscriberId)
        {
            return db.Subscribers
                .Include(s => s.Subscriptions)
                .Include(s => s.Rentals)
                    .ThenInclude(r => r.RentalCopies)
                .FirstOrDefault(s => s.Id == subscriberId);
        }

        public void AddRental(Rental rental)
        {
            db.Rentals.Add(rental);
            db.SaveChanges();
        }

        public void UpdateRental(Rental rental)
        {
            db.Rentals.Update(rental);
            db.SaveChanges();
        }

        public Subscriber? GetSubscriberWithSubscriptionsOnly(int subscriberId)
        {
            return db.Subscribers
                .Include(s => s.Subscriptions)
                .FirstOrDefault(s => s.Id == subscriberId);
        }

        public BookCopy? GetBookCopyWithBookBySerial(string serial)
        {
            return db.BookCopies
                .Include(c => c.Book)
                .SingleOrDefault(c => c.SerialNumber.ToString() == serial && !c.IsDeleted && !c.Book!.IsDeleted);
        }

        public bool IsCopyInRental(int copyId)
        {
            return db.RentalCopies.Any(c => c.BookCopyId == copyId && !c.ReturnDate.HasValue);
        }

        public List<RentalCopy> GetRentalCopiesWithoutReturn(int rentalId)
        {
            return db.RentalCopies
                .Where(rc => rc.RentalId == rentalId && !rc.ReturnDate.HasValue)
                .ToList();
        }
        public List<int> GetSubscriberCurrentBookIds(int subscriberId, int? excludeRentalId = null)
        {
            return db.Rentals
                .Where(r => r.SubscriberId == subscriberId && (!excludeRentalId.HasValue || r.Id != excludeRentalId))
                .SelectMany(r => r.RentalCopies)
                .Where(rc => !rc.ReturnDate.HasValue)
                .Select(rc => rc.BookCopy.BookId)
                .Distinct()
                .ToList();
        }


    }
}
