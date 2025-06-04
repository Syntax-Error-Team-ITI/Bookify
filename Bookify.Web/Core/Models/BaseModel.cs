namespace Bookify.Web.Core.Models
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public AplicationUser? CreatedBy { get; set; }
        public DateTime CreateOn { get; set; } = DateTime.Now;
        public string? LastUpdatedById { get; set; }
        public AplicationUser? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}