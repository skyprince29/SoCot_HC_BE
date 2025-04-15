using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model.Enums
{
    public enum FacilityLevel
    {
        [Display(Name = "Level 0")]
        Level0,

        [Display(Name = "Level 1")]
        Level1,

        [Display(Name = "Level 2")]
        Level2,

        [Display(Name = "Level 3")]
        Level3,

        [Display(Name = "APEX")]
        APEX
    }
}
