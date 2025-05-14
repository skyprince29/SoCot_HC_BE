using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class DentalRecordDetailsSocialHistory
    {
        public Guid DentalRecordDetailsSocialHistoryId { get; set; }
        public Boolean HasSweetenedSugarBeverageOrFood { get; set; } = false;
        [MaxLength(500)]
        public String SweetenedSugarBeverageOrFood { get; set; } = string.Empty;
        public Boolean HasUseOfAlcohol { get; set; } = false;
        [MaxLength(500)]
        public String UseOfAlcohol { get; set; } = string.Empty;
        public Boolean HasUseOfTobacco { get; set; } = false;
        [MaxLength(500)]
        public String UseOfTobacco { get; set; } = string.Empty;
        public Boolean HasBetelNutChewing { get; set; } = false;
        [MaxLength(500)]
        public String BetelNutChewing { get; set; } = string.Empty;
    }
}
