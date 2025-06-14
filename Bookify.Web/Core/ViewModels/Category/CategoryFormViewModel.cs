namespace Bookify.Web.Core.ViewModels.Category
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "Category name is required.")]
        [Remote("IsAllowed", "Categories", AdditionalFields = "Id", ErrorMessage = "Category with the same name already exists.")]
        public string Name { get; set; } = null!;
    }
}
