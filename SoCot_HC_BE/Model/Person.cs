﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Models.Enums;
using SoCot_HC_BE.Model.BaseModels;

namespace SoCot_HC_BE.Model
{
    public class Person : AuditInfo
    {
        [Key]
        public Guid PersonId { get; set; }

        [MaxLength(50)]
        public required string Firstname { get; set; }

        [MaxLength(50)]
        public string? Middlename { get; set; }

        [MaxLength(50)]
        public required string Lastname { get; set; }

        [MaxLength(5)]
        public string? Suffix { get; set; }

        public DateTime BirthDate { get; set; }

        [MaxLength(100)]
        public string? BirthPlace { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        [MaxLength(20)]
        public string? CivilStatus { get; set; }

        [MaxLength(50)]
        public string? Religion { get; set; }

        [MaxLength(15)]
        public string? ContactNo { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        public Guid? AddressIdResidential { get; set; }
        public Guid? AddressIdPermanent { get; set; }

        public bool IsDeceased { get; set; }

        [MaxLength(100)]
        public string? Citizenship { get; set; }

        [MaxLength(5)]
        public string? BloodType { get; set; }

        public int PatientIdTemp { get; set; }

        public string? Fullname { get; set; } = "";
        public string? Completename { get; set; } = "";

        // --- Navigation Properties ---
        [ForeignKey(nameof(AddressIdResidential))]
        [InverseProperty(nameof(Address.PersonsWithResidentialAddress))]
        public virtual Address? AddressAsResidential { get; set; }

        [ForeignKey(nameof(AddressIdPermanent))]
        [InverseProperty(nameof(Address.PersonsWithPermanentAddress))]
        public virtual Address? AddressAsPermanent { get; set; }


        [InverseProperty(nameof(Household.PersonAsHeadOfHousehold))]
        public ICollection<Household> Households { get; set; } = new List<Household>();

        [InverseProperty(nameof(Family.Person))]
        public ICollection<Family> Families { get; set; } = new List<Family>();
        [InverseProperty(nameof(FamilyMember.Person))]
        public ICollection<FamilyMember> FamilyMemberships { get; set; } = new List<FamilyMember>();


        [InverseProperty(nameof(PersonRelation.PersonAsSelf))]
        public ICollection<PersonRelation> PersonRelationsAsSelf { get; set; } = new List<PersonRelation>();

        [InverseProperty(nameof(PersonRelation.PersonAsRelated))]
        public ICollection<PersonRelation> PersonRelationsAsRelated { get; set; } = new List<PersonRelation>();

        [InverseProperty(nameof(UserAccount.PersonAsUserAccount))]
        public ICollection<UserAccount> UserAccountsAsPerson { get; set; } = new List<UserAccount>();


    }
}
