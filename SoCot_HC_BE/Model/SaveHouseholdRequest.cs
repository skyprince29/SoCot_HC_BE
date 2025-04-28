using System;
using System.Collections.Generic;

namespace SoCot_HC_BE.Model.Requests
{
    public class SaveHouseholdRequest
    {
        public Guid HouseholdId { get; set; }
        public required Address Address { get; set; }
        public required List<FamilyRequest> Families { get; set; }
    }

    public class FamilyRequest
    {
        public Guid FamilyId { get; set; }
        public required List<PersonRequest> Persons { get; set; }
    }

    public class PersonRequest
    {
        public Guid PersonId { get; set; }
        public required string Firstname { get; set; }
        public required string Middlename { get; set; }
        public required string Lastname { get; set; }
        public required string Birthdate { get; set; }
    }
}
