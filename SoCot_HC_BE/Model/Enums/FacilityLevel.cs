using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model.Enums
{
    public enum FacilityLevel : byte
    {
        [Display(Name = "Level 0")]
        Level0 = 0,

        [Display(Name = "Level 1")]
        Level1 = 1,

        [Display(Name = "Level 2")]
        Level2 = 2,

        [Display(Name = "Level 3")]
        Level3 = 3,

        [Display(Name = "APEX")]
        APEX = 4
    }
}
