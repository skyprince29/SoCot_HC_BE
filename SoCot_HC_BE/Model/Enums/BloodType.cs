using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoCot_HC_BE.Models.Enums
{
    public enum BloodType
    {
        [Display(Name = "A+")]
        A_Positive,

        [Display(Name = "A-")]
        A_Negative,

        [Display(Name = "B+")]
        B_Positive,

        [Display(Name = "B-")]
        B_Negative,

        [Display(Name = "AB+")]
        AB_Positive,

        [Display(Name = "AB-")]
        AB_Negative,

        [Display(Name = "O+")]
        O_Positive,

        [Display(Name = "O-")]
        O_Negative
    }
}