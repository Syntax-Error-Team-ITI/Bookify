namespace Bookify.Web.Core.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Models.Book> AllBooks { get; set; } = new List<Models.Book>();

        public IEnumerable<Models.Author> AllAuthors { get; set; } = new List<Models.Author>();

        public int AllBooksCount { get; set; } = 0;

        public IEnumerable<BookCopy> AllCopies { get; set; } = new List<BookCopy>();
        public int AllAvailableBooksCount { get; set; } = 0;
        public int AllRentalsCount { get; set; } = 0;

        public int NumOfBooksLowStock
        {
            get
            {
                return AllCopies
                    .GroupBy(copy => copy.BookId)
                    .Where(g => g.Count() == 1)
                    .Count();
            }
        }
        public IEnumerable<Subscriber> AllSubscribers { get; set; } = new List<Subscriber>();

        public IEnumerable<Models.Book> FeaturedBooks
        {
            get
            {
                return AllCopies
                    .GroupBy(b => b.BookId)        // 1. Group copies by their BookId
                    .Where(g => g.Count() > 1)     // 2. Filter groups where a book has more than one copy
                    .Select(g => g.First().Book)   // 3. For each group, get the Book object from the first copy
                                                   //    (assuming BookCopy has a 'Book' navigation property)
                    .Distinct();                   // 4. Ensure only unique Book objects are returned
            }
        }


        public IEnumerable<Models.Category> AllCategories = new List<Models.Category>();
    }
}