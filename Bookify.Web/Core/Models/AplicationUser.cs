namespace Bookify.Web.Core.Models
{
    public class AplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreateOn { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedOn { get; set; }
        public string? LastUpdatedById { get; set; }
    }
}
