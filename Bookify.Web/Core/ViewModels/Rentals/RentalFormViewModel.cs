namespace Bookify.Web.Core.ViewModels.Rentals
{
    public class RentalFormViewModel
    {
        public string subscriberKey { get; set; } = null!;
        public IList<int> SelectedCopies { get; set; }=new List<int>();
    }
}
