using SoCot_HC_BE.Model;

namespace SoCot_HC_BE.DTO
{
    public class PatientDeptTransVitalSignsDto
    {
        public PatientDepartmentTransaction? PatientDepartmentTransaction { get; set; }
        public ICollection<VitalSign> VitalSigns { get; set; } = new List<VitalSign>();
    }
}
