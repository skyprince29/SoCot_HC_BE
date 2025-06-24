using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.OldReferralDto; // Assuming your DbContext is here
using SoCot_HC_BE.Helpers;
using SoCot_HC_BE.Model;

namespace SoCot_HC_BE.Services
{
    public class OldReferralService
    {   
        private readonly AppDbContext _context;

        public OldReferralService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveReferralAsync(ReferralDto referralDto)
        {
            if (referralDto != null)
            {
                var referral = new Referral()
                {
                    TempRefId = referralDto.Id,
                    ReferralNo = referralDto.ReferralNo,
                    ReferralDateTime = referralDto.DateGenerated.HasValue 
                        ? referralDto.DateGenerated.Value 
                        : DateTime.Now, // Fallback to the current date and time if DateGenerated is null
                    Complains = referralDto.Complains,
                    Reason = referralDto.Reason,
                    Remarks = referralDto.Remarks,
                    Diagnosis = referralDto.Diagnosis,
                    //Status = referralDto.Status,
                    ReferredFrom = referralDto.Referredfrom.HasValue
                            ? referralDto.Referredfrom.Value
                            : 0,
                    // PatientId = referralDto.PatientId,
                    ReferredTo = referralDto.FacilityId.HasValue
                     ? referralDto.FacilityId.Value : 0,
                   // PersonnelId = referralDto.PersonnelId,
                    // UserId = referralDto.UserId,
                    //IsAccepted = referralDto.IsAccepted,
                    IsUrgent = referralDto.isUrgent == null? false : referralDto.isUrgent.Value,
                };
                _context.Referral.Add(referral);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveVitalSignAsync(ReferralDto referralDto)
        {
            if (referralDto != null)
            {
                var vitalSign = new VitalSign()
                {
                    BloodPressure = referralDto.BloodPressure,
                    Systolic = !string.IsNullOrEmpty(referralDto.BloodPressure) && referralDto.BloodPressure.Contains("/")
                       ? int.Parse(referralDto.BloodPressure.Split('/')[0].Trim())
                       : 0, // Set to 0 if BloodPressure is null or invalid
                    Diastolic = !string.IsNullOrEmpty(referralDto.BloodPressure) && referralDto.BloodPressure.Contains("/")
                       ? int.Parse(referralDto.BloodPressure.Split('/')[1].Trim())
                       : 0, // Set to 0 if BloodPressure is null or invalid
                    CardiacRate = ObjectConverterHelper.ConvertToNumericValue<int>(referralDto.HeartRate),
                    Weight = referralDto.Weight.HasValue
                           ? referralDto.Weight.Value
                           : 0,
                    Height = referralDto.Height.HasValue
                           ? referralDto.Height.Value
                           : 0,
                    Temperature = ObjectConverterHelper.ConvertToNumericValue<decimal>(referralDto.Temperature),
                    RespiratoryRate = ObjectConverterHelper.ConvertToNumericValue<int>(referralDto.RespirationRate),
                };

                _context.VitalSigns.Add(vitalSign);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task SavePatientAsync(SoCot_HC_BE.DTO.OldReferralDto.Patient patientDto)
        //{
        //    var patient = new PatientModel
        //    {
        //        Id = patientDto.Id,
        //        PHouseholdNo = patientDto.PHouseholdNo,
        //        FamilySerialNo = patientDto.FamilySerialNo,
        //        PhilHealthNo = patientDto.PhilHealthNo,
        //        Lastname = patientDto.Lastname,
        //        Firstname = patientDto.Firstname,
        //        Middlename = patientDto.Middlename,
        //        Sex = patientDto.Sex,
        //        CivilStatus = patientDto.CivilStatus,
        //        Birthdate = patientDto.Birthdate,
        //        Age = patientDto.Age,
        //        BloodType = patientDto.BloodType,
        //        BirthPlace = patientDto.BirthPlace,
        //        EmploymentStatus = patientDto.EmploymentStatus,
        //        EducationalAttainment = patientDto.EducationalAttainment,
        //        Occupation = patientDto.Occupation,
        //        IsIndigent = patientDto.IsIndigent,
        //        ContactNumber = patientDto.ContactNumber,
        //        Citizenship = patientDto.Citizenship,
        //        SpouseLname = patientDto.SpouseLname,
        //        SpouseFname = patientDto.SpouseFname,
        //        SpouseMname = patientDto.SpouseMname,
        //        FatherLname = patientDto.FatherLname,
        //        FatherFname = patientDto.FatherFname,
        //        MotherFname = patientDto.MotherFname,
        //        IsActive = patientDto.IsActive
        //    };

        //    _context.Patients.Add(patient);
        //    await _context.SaveChangesAsync();
        //}

        public async Task SaveHouseHoldAsync(HouseholdDTO houseHoldDto)
        {
            var houseHold = new Household()
            {
                //TempHouseholdId = houseHoldDto.Id,
                HouseholdNo = houseHoldDto.HouseholdNo,
                ResidenceName = houseHoldDto.ResidenceName,
                //HeadofFamily = houseHoldDto.HeadofFamily,
                //Address = houseHoldDto.Address,
                IsActive = houseHoldDto.IsActive
            };
            _context.Households.Add(houseHold);
            await _context.SaveChangesAsync();
        }

        public async Task SaveFamilyAsync(FamilyDto familyDto)
        {
            var family = new Family()
            {
                //TempFamilyId = familyDto.Id,
                FamilyNo = familyDto.FamilySerialNo,
                //TempHouseholdId = familyDto.HouseHoldId,
                IsActive = familyDto.IsActive
            };
            _context.Families.Add(family);
            await _context.SaveChangesAsync();
        }

        //public async Task SaveFamilyMemberAsync(SoCot_HC_BE.DTO.OldReferralDto.FamilyMember familyMemberDto)
        //{
        //    var familyMember = new FamilyMemberModel
        //    {
        //        Id = familyMemberDto.Id,
        //        FamilyId = familyMemberDto.FamilyId,
        //        PatientId = familyMemberDto.PatientId,
        //        IsActive = familyMemberDto.IsActive
        //    };

        //    _context.FamilyMembers.Add(familyMember);
        //    await _context.SaveChangesAsync();
        //}
    }
}