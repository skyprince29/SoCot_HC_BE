using SoCot_HC_BE.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.DTO
{
    public class HouseholdDTO
    {
        public Guid HouseholdId { get; set; }
        public string? HouseholdNo { get; set; }
        public string? ResidenceName { get; set; }
        public Guid? PersonIdHeadOfHousehold { get; set; }
        public Guid AddressId { get; set; }
        public bool IsActive { get; set; }
        public AddressDto? Address { get; set; }
    }
}
