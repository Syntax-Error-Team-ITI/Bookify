namespace Bookify.Web.Core.Models
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public virtual ApplicationUser? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string? LastUpdatedById { get; set; }
        public virtual ApplicationUser? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}