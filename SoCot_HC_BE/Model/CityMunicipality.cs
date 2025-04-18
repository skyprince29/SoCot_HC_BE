﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Municipality
    {
        [Key]
        public int MunicipalityId {  get; set; }
        public int ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual required Province Province { get; set; }
        [MaxLength(100)]
        public required string MunicipalityName { get; set; }

        [InverseProperty("Municipality")]
        public virtual ICollection<Address> Address { get; set; } = new List<Address>();
    }
}
