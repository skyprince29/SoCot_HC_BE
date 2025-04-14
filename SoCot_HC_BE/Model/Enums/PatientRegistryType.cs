using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model.Enums
{
    public enum PatientRegistryType
    {
        [Display(Name = "WALK IN W/O REFERRAL")]
        WALK_IN_WO_R = 1,

        [Display(Name = "WALK IN W/ REFERRAL")]
        WALK_IN_W_R = 2,

        [Display(Name = "COORDINATED REFERRAL")]
        COORDINATED_R = 3,

        [Display(Name = "EMERGENCY CASES")]
        EMERGENCY = 4,
    }
}
