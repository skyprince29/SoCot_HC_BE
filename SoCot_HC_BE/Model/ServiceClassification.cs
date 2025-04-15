using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    /// <summary>
    /// Represents a service classification in the system.
    /// Note: This is a system-managed entity and should only be modified by developers.
    /// </summary>
    public class ServiceClassification
    {
        [Key]
        public int ServiceClassificationId { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
