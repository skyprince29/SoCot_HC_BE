using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoCot_HC_BE.Models.Enums
{
    public enum Religion
    {
        [Display(Name = "Roman Catholic")] Catholic,
        [Display(Name = "Islam")] Islam,
        [Display(Name = "Iglesia ni Cristo")] IglesiaNiCristo,
        [Display(Name = "Evangelical")] Evangelical,
        [Display(Name = "Protestant")] Protestant,
        [Display(Name = "Seventh-Day Adventist")] SeventhDayAdventist,
        [Display(Name = "Jehovah's Witnesses")] JehovahsWitnesses,
        [Display(Name = "Baptist")] Baptist,
        [Display(Name = "Pentecostal")] Pentecostal,
        [Display(Name = "Born Again Christian")] BornAgainChristian,
        [Display(Name = "Orthodox")] Orthodox,
        [Display(Name = "Buddhist")] Buddhist,
        [Display(Name = "Hindu")] Hindu,
        [Display(Name = "Aglipayan (Philippine Independent Church)")] Aglipayan,
        [Display(Name = "Methodist")] Methodist,
        [Display(Name = "Lutheran")] Lutheran,
        [Display(Name = "Church of Jesus Christ of Latter-Day Saints (Mormon)")] Mormon,
        [Display(Name = "Judaism")] Judaism,
        [Display(Name = "Indigenous/Tribal")] Tribal,
        [Display(Name = "None")] None,
        [Display(Name = "Others")] Others
    }
}