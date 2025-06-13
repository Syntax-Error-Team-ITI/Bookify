using Microsoft.AspNetCore.Mvc.Rendering;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Core.ViewModels
{
    public class SubscriberFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100), Display(Name = "First Name"),
            RegularExpression("^[^<>!#%$]*$", ErrorMessage = Errors.DenySpecialCharacters)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100), Display(Name = "Last Name"),
            RegularExpression("^[^<>!#%$]*$", ErrorMessage = Errors.DenySpecialCharacters)]
        public string LastName { get; set; } = null!;

        [Display(Name = "Date Of Birth")]
        [AssertThat("DateOfBirth <= Today()", ErrorMessage = Errors.NotAllowedFutureDate)]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        [MaxLength(14), Display(Name = "National ID"),
            RegularExpression("^[2,3]{1}[0-9]{13}$", ErrorMessage = Errors.InvalidNationalId)]
        [Remote("AllowNationalId", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        public string NationalId { get; set; } = null!;

        [MaxLength(11), Display(Name = "Mobile Number"),
            RegularExpression("^01[0,1,2,5]{1}[0-9]{8}$", ErrorMessage = Errors.InvalidMobileNumber)]
        [Remote("AllowMobileNumber", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        public string MobileNumber { get; set; } = null!;

        public bool HasWhatsApp { get; set; }

        [MaxLength(150), EmailAddress]
        [Remote("AllowEmail", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        public string Email { get; set; } = null!;

        [RequiredIf("Id == 0", ErrorMessage = Errors.EmptyImage)]
        public IFormFile? Image { get; set; }

        [Display(Name = "Area")]
        public int AreaId { get; set; }

        public IEnumerable<SelectListItem>? Areas { get; set; } = new List<SelectListItem>();

        [Display(Name = "Governorate")]
        public int GovernorateId { get; set; }

        public IEnumerable<SelectListItem>? Governorates { get; set; }

        [MaxLength(500)]
        public string Address { get; set; } = null!;

        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
    }
}