using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class ReferralService
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReferralId { get; set; }
        public Referral? Referral { get; set; }
        public Guid ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
