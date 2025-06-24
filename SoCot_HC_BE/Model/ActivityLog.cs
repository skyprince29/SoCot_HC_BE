using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class ActivityLog
    {
        [Key]
        public Guid ActivityLogId { get; set; }
        [Required]
        public required Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public UserAccount? UserAccount { get; set; }
        [Required ,MaxLength(30)]
        public required string Action { get; set; } // Action performed (e.g., "Created", "Updated", "Deleted", "Statuses")
        [Required, MaxLength(50)]
        public required string EntityType { get; set; }
        [Required]
        public required Guid EntityId { get; set; } // You can use string or Guid based on your needs
        public int ModuleId { get; set; }
        [Required, MaxLength(300)]
        public string? Message { get; set; }
        [Required, MaxLength(150)]
        public string? Link { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
