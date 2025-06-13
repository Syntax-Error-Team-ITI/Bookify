namespace Bookify.Web.Core.ViewModels.BookCopies
{
    public class BookCopyFormViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        [Display(Name = "Is available for rental?")]
        public bool IsAvailableForRental { get; set; }
        [Range(1, 1000 , ErrorMessage = Errors.InvalidRange)]
        public int EditionNumber { get; set; }
        public bool ShowRentalInput { get; set; }
    }
}
