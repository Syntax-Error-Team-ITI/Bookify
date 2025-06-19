using Bookify.Web.Core.ViewModels.BookCopies;

namespace Bookify.Web.Core.ViewModels
{
    public class RentalFormViewModel
    {
        public int? Id { get; set; }

        public int SubscriberId { get; set; }

        public IList<int> SelectedCopies { get; set; } = new List<int>();

        public IEnumerable<BookCopyViewModel> CurrentCopies { get; set; } = new List<BookCopyViewModel>();

        public int? MaxAllowedCopies { get; set; }
    }
}