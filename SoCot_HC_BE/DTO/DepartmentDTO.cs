using SoCot_HC_BE.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SoCot_HC_BE.Model.BaseModels;

namespace SoCot_HC_BE.DTO
{
    public class DepartmentDTO : AuditInfo
    {
        public Guid DepartmentId { get; set; }
        public string? DepartmentCode { get; set; }
        public int FacilityId { get; set; }
        public string? DepartmentName { get; set; }
        public string? Description { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public bool IsReferable { get; set; }
        public bool IsActive { get; set; }
        public List<Guid> DepartmentTypeIds { get; set; }
    }
}
