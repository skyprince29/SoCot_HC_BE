using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class DentalRecordDetailsToothCount
    {
        public Guid DentalRecordDetailsToothCountId { get; set; }
        public int NoOfTeethPresentTemp { get; set; }
        public int NoOfTeethPresentPerm { get; set; }
        public int CarriesIndicatedForFillingTemp { get; set; }
        public int CarriesIndicatedForFillingPerm { get; set; }
        public int CarriesIndicatedForExtractionTemp { get; set; }
        public int CarriesIndicatedForExtractionPerm { get; set; }
        public int RootFragmentTemp { get; set; }
        public int RootFragmentPerm { get; set; }
        public int MissingDueToCarries { get; set; }
        public int FilledOrRestoredTemp { get; set; }
        public int FilledOrRestoredPerm { get; set; }
        public int TotalDfAndDmfTeeth { get; set; }
        [MaxLength(500)]
        public string FluorideApplication { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Examiner { get; set; } = string.Empty;
    }
}
