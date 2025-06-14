namespace Bookify.Web.Core.ViewModels.Author
{
    public class AuthorFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "Author name is required.")]
        [Remote("IsAllowed", "Authors", AdditionalFields = "Id", ErrorMessage = "An Author with the same name already exists.")]
        public string Name { get; set; } = null!;
    }
}
