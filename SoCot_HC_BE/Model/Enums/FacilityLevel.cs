using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model.Enums
{
    public enum FacilityLevel : byte
    {
        [Display(Name = "Level 0")]
        Level0 = 1,

        [Display(Name = "Level 1")]
        Level1 = 2,

        [Display(Name = "Level 2")]
        Level2 = 3,

        [Display(Name = "Level 3")]
        Level3 = 4,

        [Display(Name = "APEX")]
        APEX = 5
    }
}
