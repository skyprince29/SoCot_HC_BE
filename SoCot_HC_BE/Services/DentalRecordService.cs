using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using static SoCot_HC_BE.DTO.DentalDTO;

namespace SoCot_HC_BE.Services
{
    public class DentalRecordService : Repository<DentalRecord, Guid>, IDentalRecordService
    {
        public DentalRecordService(AppDbContext context) : base(context)
        {

        }

        public async Task<PaginationHandler<DentalRecord>> GetAllWithPagingAsync(int pageNo, int limit, string keyword = "", CancellationToken cancellationToken = default)
        {
            var dentalRecords = await _dbSet
                .Include(p => p.Patient)
                .Include(f => f.Facility)
                .Include(pr => pr.Physician)
                .Include(pr => pr.DentalRecordDetailsMedicalHistory)
                .Include(pr => pr.DentalRecordDetailsSocialHistory)
                .Include(pr => pr.DentalRecordDetailsOralHealthCondition)
                .Include(pr => pr.DentalRecordDetailsPresence)
                .Include(pr => pr.DentalRecordDetailsToothCount)
                .Include(pr => pr.DentalRecordDetailsServices)
                .Include(pr => pr.DentalRecordDetailsFindings)
                .Where(
                dt =>
                dt.Patient.Firstname.Contains(keyword) ||
                dt.Patient.Lastname.Contains(keyword) ||
                (dt.Patient.Middlename != null && dt.Patient.Middlename.Contains(keyword))
                )
               .AsNoTracking()
               .ToListAsync();

            int totalRecords = dentalRecords.Count;


            var paginatedResult = new PaginationHandler<DentalRecord>(dentalRecords, totalRecords, pageNo, limit);
            return paginatedResult;
        }

        public async Task<DentalRecord> UpdateDentalRecord(Guid DentalRecordId, CancellationToken cancellationToken = default)
        {

            var dentalRecords = await _dbSet
               .Include(p => p.Patient)
               .Include(f => f.Facility)
               .Include(pr => pr.Physician)
               .Include(pr => pr.DentalRecordDetailsMedicalHistory)
               .Include(pr => pr.DentalRecordDetailsSocialHistory)
               .Include(pr => pr.DentalRecordDetailsOralHealthCondition)
               .Include(pr => pr.DentalRecordDetailsPresence)
               .Include(pr => pr.DentalRecordDetailsToothCount)
               .Include(pr => pr.DentalRecordDetailsServices)
               .Include(pr => pr.DentalRecordDetailsFindings)
               .Where(
               dt =>
               dt.DentalRecordId == DentalRecordId
               )
              .FirstOrDefaultAsync(cancellationToken);

            if (dentalRecords == null) { 
            throw new Exception("Dental Record not found.");
            }

            return dentalRecords;   
        }

        public async Task<DentalRecord> CreateDentalRecord(String ReferralNo, Guid userAccountId, CancellationToken cancellationToken = default) {

            PatientRegistry? patientRegistry = await _context.Set<PatientRegistry>().Where(i => i.ReferralNo == ReferralNo).FirstOrDefaultAsync(cancellationToken);

            if (patientRegistry == null)
            {
                throw new Exception("Patient Registry not found.");
            }

            Person? patient = await _context.Set<Person>().Where(p => p.PersonId == patientRegistry.PatientId).FirstOrDefaultAsync(cancellationToken);


            if (patient == null)
            {
                throw new Exception("Patient not found.");
            }

            UserAccount? userAccount = await _context.Set<UserAccount>()
                .Include(u => u.FacilityAsUserAccount)
                .Where(u => u.UserAccountId == userAccountId).FirstOrDefaultAsync(cancellationToken);

           
                if (userAccount == null)
                {
                    throw new Exception("user not found.");
                }

                DentalRecord dentalRecord = new DentalRecord();
            dentalRecord.PatientId = patient.PersonId;
            dentalRecord.Patient = patient;
            dentalRecord.FacilityId = userAccount.FacilityId;
            dentalRecord.Facility = userAccount.FacilityAsUserAccount;
            dentalRecord.PatientRegistryId = patientRegistry.PatientRegistryId;
            dentalRecord.ReferralNo = ReferralNo;
            dentalRecord.CreatedBy = userAccount.PersonId;
            dentalRecord.CreatedDate = DateTime.Now;
            dentalRecord.DateRecord = DateTime.Now;

            dentalRecord.DentalRecordDetailsMedicalHistory = new DentalRecordDetailsMedicalHistory();
            dentalRecord.DentalRecordDetailsSocialHistory = new DentalRecordDetailsSocialHistory();
            dentalRecord.DentalRecordDetailsOralHealthCondition = new DentalRecordDetailsOralHealthCondition();
            dentalRecord.DentalRecordDetailsPresence = new DentalRecordDetailsPresence();
            dentalRecord.DentalRecordDetailsToothCount = new DentalRecordDetailsToothCount();

            return dentalRecord;
        }

        public DentalRecord manageDentalRecordModel(DentalDTO.DentalRecordDTO dentalRecordDTO) { 

            DentalRecord dentalRecord = new DentalRecord();
            dentalRecord.DentalRecordId = dentalRecordDTO.DentalRecordId;
            dentalRecord.PatientId = dentalRecordDTO.PatientId;
            dentalRecord.FacilityId = dentalRecordDTO.FacilityId;
            dentalRecord.ConsentedByName = dentalRecordDTO.ConsentedByName;
            dentalRecord.PhysicianId = dentalRecordDTO.PhysicianId;
            dentalRecord.PatientRegistryId = dentalRecordDTO.PatientRegistryId;
            dentalRecord.ReferralNo = dentalRecordDTO.ReferralNo;
            dentalRecord.DateRecord = dentalRecordDTO.DateRecord;
            dentalRecord.CreatedBy = dentalRecordDTO.CreatedBy;
            dentalRecord.CreatedDate = dentalRecordDTO.CreatedDate;

            return dentalRecord;    
        }

        public List<DentalRecordDetailsFindings> manageDentalFindings(DentalDTO.DentalRecordDTO dentalRecordDTO)
        {

            List<DentalRecordDetailsFindings> items = new List<DentalRecordDetailsFindings>();

            if (dentalRecordDTO.Findings != null)
            {
                foreach (FindingsDTO item in dentalRecordDTO.Findings)
                {
                    DentalRecordDetailsFindings findings = new DentalRecordDetailsFindings();

                    findings.DentalRecordDetailsFindingsId = item.DentalRecordDetailsFindingsId;
                    findings.DentalRecordId = item.DentalRecordId;
                    findings.DateDiagnose = item.DateDiagnose;
                    findings.ToothNo = item.ToothNo;
                    findings.Condition = item.Condition;
                    findings.Diagnosis = item.Diagnosis;
                    findings.Remarks = item.Remarks;

                    items.Add(findings);
                }
            }


            return items;
        }


        public List<DentalRecordDetailsServices> manageDentalService(DentalDTO.DentalRecordDTO dentalRecordDTO)
        {

            List<DentalRecordDetailsServices> items = new List<DentalRecordDetailsServices>();

            if (dentalRecordDTO.Services != null)
            {
                foreach (ServicesDTO item in dentalRecordDTO.Services)
                {
                    DentalRecordDetailsServices services = new DentalRecordDetailsServices();
                    services.DentalRecordDetailsServicesId = item.DentalRecordDetailsServicesId;
                    services.DateDiagnose = item.DateDiagnose;
                    services.Diagnosis = item.Diagnosis;
                    services.ToothNo = item.ToothNo;
                    services.ServiceRendered = item.ServiceRendered;
                    services.Remarks = item.Remarks;

                    items.Add(services);
                }
            }


            return items;
        }


        public DentalRecordDetailsMedicalHistory manageMedicalHistory(DentalDTO.DentalRecordDTO dentalRecordDTO)
        {

            DentalDTO.MedicalHistoryDTO medicalHistoryDTO = dentalRecordDTO.MedicalHistory;
            DentalRecordDetailsMedicalHistory medicalHistory = new DentalRecordDetailsMedicalHistory();
            medicalHistory.DentalRecordDetailsMedicalHistoryId = medicalHistoryDTO.DentalRecordDetailsMedicalHistoryId;
            medicalHistory.HasAlergies = medicalHistoryDTO.HasAlergies;
            medicalHistory.Alergies = medicalHistoryDTO.Alergies;
            medicalHistory.HasHypertentionOrCVA = medicalHistoryDTO.HasHypertentionOrCVA;
            medicalHistory.HasDiabetesMelitus = medicalHistoryDTO.HasDiabetesMelitus;
            medicalHistory.HasBloodDisorders = medicalHistoryDTO.HasBloodDisorders;
            medicalHistory.HasCardiovascularOrHeartDiseases = medicalHistoryDTO.HasCardiovascularOrHeartDiseases;
            medicalHistory.HasThyroidDisorders = medicalHistoryDTO.HasThyroidDisorders;
            medicalHistory.HasHepatitis = medicalHistoryDTO.HasHepatitis;
            medicalHistory.HepatitisType = medicalHistoryDTO.HepatitisType;
            medicalHistory.HasMalignancy = medicalHistoryDTO.HasMalignancy;
            medicalHistory.MalignancyType = medicalHistoryDTO.MalignancyType;
            medicalHistory.HasHistoryOfPrevHospitalization = medicalHistoryDTO.HasHistoryOfPrevHospitalization;
            medicalHistory.Medical = medicalHistoryDTO.Medical;
            medicalHistory.Surgical = medicalHistoryDTO.Surgical;
            medicalHistory.HasBloodTransfusion = medicalHistoryDTO.HasBloodTransfusion;
            medicalHistory.BloodTransfusionMonth = medicalHistoryDTO.BloodTransfusionMonth;
            medicalHistory.BloodTransfusionYear = medicalHistoryDTO.BloodTransfusionYear;
            medicalHistory.HasTattoo = medicalHistoryDTO.HasTattoo;
            medicalHistory.HasOthers = medicalHistoryDTO.HasOthers;
            medicalHistory.Others = medicalHistoryDTO.Others;

            return medicalHistory;
        }


        public DentalRecordDetailsSocialHistory managesocialHistory(DentalDTO.DentalRecordDTO dentalRecordDTO)
        {

            DentalDTO.SocialHistoryDTO socialHistoryDTO = dentalRecordDTO.SocialHistory;
            DentalRecordDetailsSocialHistory socialHistory = new DentalRecordDetailsSocialHistory();

            socialHistory.DentalRecordDetailsSocialHistoryId = socialHistoryDTO.DentalRecordDetailsSocialHistoryId;
            socialHistory.HasSweetenedSugarBeverageOrFood = socialHistoryDTO.HasSweetenedSugarBeverageOrFood;
            socialHistory.SweetenedSugarBeverageOrFood = socialHistoryDTO.SweetenedSugarBeverageOrFood;
            socialHistory.HasUseOfAlcohol = socialHistoryDTO.HasUseOfAlcohol;
            socialHistory.UseOfAlcohol = socialHistoryDTO.UseOfAlcohol;
            socialHistory.HasUseOfTobacco = socialHistoryDTO.HasUseOfTobacco;
            socialHistory.UseOfTobacco = socialHistoryDTO.UseOfTobacco;
            socialHistory.HasBetelNutChewing = socialHistoryDTO.HasBetelNutChewing;
            socialHistory.BetelNutChewing = socialHistoryDTO.BetelNutChewing;

            return socialHistory;
        }

        public DentalRecordDetailsOralHealthCondition manageOralHealthCondition(DentalDTO.DentalRecordDTO dentalRecordDTO)
        {

            DentalDTO.OralHealthConditionDTO oralHealthConditionDTO = dentalRecordDTO.OralHealthCondition;
            DentalRecordDetailsOralHealthCondition oralHealthCondition = new DentalRecordDetailsOralHealthCondition();
            oralHealthCondition.DentalRecordDetailsOralHealthConditionId = oralHealthConditionDTO.DentalRecordDetailsOralHealthConditionId;
            oralHealthCondition.DateOfOralExamination = oralHealthConditionDTO.DateOfOralExamination;
            oralHealthCondition.OrallyFitChild = oralHealthConditionDTO.OrallyFitChild;
            oralHealthCondition.DentalCarries = oralHealthConditionDTO.DentalCarries;
            oralHealthCondition.Gingivitis = oralHealthConditionDTO.Gingivitis;
            oralHealthCondition.PeriodontalDisease = oralHealthConditionDTO.PeriodontalDisease;
            oralHealthCondition.Debris = oralHealthConditionDTO.Debris;
            oralHealthCondition.Calculus = oralHealthConditionDTO.Calculus;
            oralHealthCondition.AbnormalGrowth = oralHealthConditionDTO.AbnormalGrowth;
            oralHealthCondition.CleftLipOrPalate = oralHealthConditionDTO.CleftLipOrPalate;
            oralHealthCondition.Others = oralHealthConditionDTO.Others;
            oralHealthCondition.NoPermTeethPresent = oralHealthConditionDTO.NoPermTeethPresent;
            oralHealthCondition.NoPermSoundTeeth = oralHealthConditionDTO.NoPermSoundTeeth;
            oralHealthCondition.NoOfDecayedTeethBigD = oralHealthConditionDTO.NoOfDecayedTeethBigD;
            oralHealthCondition.NoOfMissingTeethM = oralHealthConditionDTO.NoOfMissingTeethM;
            oralHealthCondition.NoOfFilledTeethBigF = oralHealthConditionDTO.NoOfFilledTeethBigF;
            oralHealthCondition.TotalDMFTeeth = oralHealthConditionDTO.TotalDMFTeeth;
            oralHealthCondition.NoTempTeethPresent = oralHealthConditionDTO.NoTempTeethPresent;
            oralHealthCondition.NoTempSoundTeeth = oralHealthConditionDTO.NoTempSoundTeeth;
            oralHealthCondition.NoOfDecayedTeethSmallD = oralHealthConditionDTO.NoOfDecayedTeethSmallD;
            oralHealthCondition.NoOfFilledTeethSmallF = oralHealthConditionDTO.NoOfFilledTeethSmallF;
            oralHealthCondition.TotalDFTeeth = oralHealthConditionDTO.TotalDFTeeth;

            return oralHealthCondition;
        }

        public DentalRecordDetailsPresence managePresence(DentalDTO.DentalRecordDTO dentalRecordDTO)
        {

            DentalDTO.PresenceDTO presenceDTO = dentalRecordDTO.Presence;
            DentalRecordDetailsPresence presence = new DentalRecordDetailsPresence();
            presence.DentalRecordDetailsPresenceId = presenceDTO.DentalRecordDetailsPresenceId;
            presence.DateOfExamination = presenceDTO.DateOfExamination;
            presence.AgeLastBirthday = presenceDTO.AgeLastBirthday;
            presence.PresenceOfDentalCarries = presenceDTO.PresenceOfDentalCarries;
            presence.PresenceOfGingivitis = presenceDTO.PresenceOfGingivitis;
            presence.PresenceOfPeriodicPocket = presenceDTO.PresenceOfPeriodicPocket;
            presence.PresenceOfOralDebris = presenceDTO.PresenceOfOralDebris;
            presence.PresenceOfCalculus = presenceDTO.PresenceOfCalculus;
            presence.PresenceOfNeoplasm = presenceDTO.PresenceOfNeoplasm;
            presence.PresenceOfDentoFacialAnomaly = presenceDTO.PresenceOfDentoFacialAnomaly;


            return presence;
        }

        public DentalRecordDetailsToothCount manageToothCount(DentalDTO.DentalRecordDTO dentalRecordDTO)
        {
            DentalDTO.ToothCountDTO toothCountDTO = dentalRecordDTO.ToothCount;
            DentalRecordDetailsToothCount toothCount = new DentalRecordDetailsToothCount();
            toothCount.DentalRecordDetailsToothCountId = toothCountDTO.DentalRecordDetailsToothCountId;
            toothCount.NoOfTeethPresentTemp = toothCountDTO.NoOfTeethPresentTemp;
            toothCount.NoOfTeethPresentPerm = toothCountDTO.NoOfTeethPresentPerm;
            toothCount.CarriesIndicatedForFillingTemp = toothCountDTO.CarriesIndicatedForFillingTemp;
            toothCount.CarriesIndicatedForFillingPerm = toothCountDTO.CarriesIndicatedForFillingPerm;
            toothCount.CarriesIndicatedForExtractionTemp = toothCountDTO.CarriesIndicatedForExtractionTemp;
            toothCount.CarriesIndicatedForExtractionPerm = toothCountDTO.CarriesIndicatedForExtractionPerm;
            toothCount.RootFragmentTemp = toothCountDTO.RootFragmentTemp;
            toothCount.RootFragmentPerm = toothCountDTO.RootFragmentPerm;
            toothCount.MissingDueToCarries = toothCountDTO.MissingDueToCarries;
            toothCount.FilledOrRestoredTemp = toothCountDTO.FilledOrRestoredTemp;
            toothCount.FilledOrRestoredPerm = toothCountDTO.FilledOrRestoredPerm;
            toothCount.TotalDfAndDmfTeeth = toothCountDTO.TotalDfAndDmfTeeth;
            toothCount.FluorideApplication = toothCountDTO.FluorideApplication;
            toothCount.Examiner = toothCountDTO.Examiner;

            return toothCount;
        }

        public async Task SaveOrUpdateDentalRecordAsync(DentalDTO.DentalRecordDTO dentalRecord, CancellationToken cancellationToken = default)
        {

            DentalRecord record = manageDentalRecordModel(dentalRecord);
            DentalRecordDetailsMedicalHistory medicalHistory = manageMedicalHistory(dentalRecord);
            DentalRecordDetailsSocialHistory socialHistory = managesocialHistory(dentalRecord);
            DentalRecordDetailsOralHealthCondition oralHealthCondition = manageOralHealthCondition(dentalRecord);
            DentalRecordDetailsPresence presence = managePresence(dentalRecord);
            DentalRecordDetailsToothCount toothCount = manageToothCount(dentalRecord);

            if (record.DentalRecordId != Guid.Empty)
            {
                //UpdateAsync(record);
                _context.Update(medicalHistory);
                _context.Update(socialHistory);
                _context.Update(oralHealthCondition);
                _context.Update(presence);
                _context.Update(toothCount);
            }
            else
            {
                _context.Add(medicalHistory);
                _context.Add(socialHistory);
                _context.Add(oralHealthCondition);
                _context.Add(presence);
                _context.Add(toothCount);
            }

            await _context.SaveChangesAsync(cancellationToken);

            record.DentalRecordDetailsMedicalHistoryId = medicalHistory.DentalRecordDetailsMedicalHistoryId;
            record.DentalRecordDetailsSocialHistoryId = socialHistory.DentalRecordDetailsSocialHistoryId;
            record.DentalRecordDetailsOralHealthConditionId = oralHealthCondition.DentalRecordDetailsOralHealthConditionId;
            record.DentalRecordDetailsPresenceId = presence.DentalRecordDetailsPresenceId;
            record.DentalRecordDetailsToothCountId = toothCount.DentalRecordDetailsToothCountId;

            if (record.DentalRecordId != Guid.Empty)
            {
                await UpdateAsync(record, cancellationToken);
            }
            else
            {
                await AddAsync(record, cancellationToken);
            }

            List<DentalRecordDetailsFindings> dentalFindings = manageDentalFindings(dentalRecord);
            List<Guid> tempFindingsIds = new List<Guid>();

            foreach (DentalRecordDetailsFindings findings in dentalFindings)
            {

                findings.DentalRecordId = record.DentalRecordId;

                if (findings.DentalRecordDetailsFindingsId == Guid.Empty)
                {
                    _context.Add(findings);
                }
                else
                {
                    _context.Update(findings);
                }

                tempFindingsIds.Add(findings.DentalRecordDetailsFindingsId);
            }

            List<DentalRecordDetailsServices> dentalServices = manageDentalService(dentalRecord);
            List<Guid> tempServicesIds = new List<Guid>();

            foreach (DentalRecordDetailsServices services in dentalServices)
            {

                services.DentalRecordId = record.DentalRecordId;

                if (services.DentalRecordDetailsServicesId == Guid.Empty)
                {
                    _context.Add(services);
                }
                else
                {
                    _context.Update(services);
                }

                tempServicesIds.Add(services.DentalRecordDetailsServicesId);
            }

            // remove not existing findings on list
            List<DentalRecordDetailsFindings> existingFindings = await _context.DentalRecordDetailsFindings.Where(i => i.DentalRecordId == record.DentalRecordId).ToListAsync();

            if (existingFindings.Count > 0) 
            {
                foreach (DentalRecordDetailsFindings findings in existingFindings) 
                {
                    if (!tempFindingsIds.Contains(findings.DentalRecordDetailsFindingsId)) 
                    {
                        _context.Remove(findings);
                    }
                }
            }

            // remove not existing services on list
            List<DentalRecordDetailsServices> existingServices = await _context.DentalRecordDetailsServices.Where(i => i.DentalRecordId == record.DentalRecordId).ToListAsync();

            if (existingServices.Count > 0)
            {
                foreach (DentalRecordDetailsServices services in existingServices)
                {
                    if (!tempServicesIds.Contains(services.DentalRecordDetailsServicesId))
                    {
                        _context.Remove(services);
                    }
                }
            }

            await _context.SaveChangesAsync();
            await manageDentalTreatmentCompletion(record.PatientRegistryId);
        }


        public async Task manageDentalTreatmentCompletion(Guid? PatientRegistryId)
        {

            if (PatientRegistryId != Guid.Empty)
            {
                DentalTreatment dentalTreatment = await _context.DentalTreatment
               .Where(dt => dt.PatientRegistry.PatientRegistryId == PatientRegistryId
               && dt.Status != Model.Enums.DentalTreatmentStatus.Completed)
               .FirstOrDefaultAsync();


                if (dentalTreatment != null)
                {
                    dentalTreatment.Status = Model.Enums.DentalTreatmentStatus.Completed;
                    _context.Update(dentalTreatment);
                    await _context.SaveChangesAsync();
                }
            }




        }


        public async Task<DentalRecord> GetDentalRecord(Guid dentalRecordId, CancellationToken cancellationToken = default)
        {

            DentalRecord dentalRecords = await _dbSet
               .Include(p => p.Patient)
               .Include(f => f.Facility)
               .Include(pr => pr.Physician)
               .Include(pr => pr.DentalRecordDetailsPresence)
               .Include(pr => pr.DentalRecordDetailsMedicalHistory)
               .Include(pr => pr.DentalRecordDetailsSocialHistory)
               .Include(pr => pr.DentalRecordDetailsOralHealthCondition)
               .Include(pr => pr.DentalRecordDetailsToothCount)
               .Include(pr => pr.DentalRecordDetailsServices)
               .Include(pr => pr.DentalRecordDetailsFindings)
               .Where(dt =>
               dt.DentalRecordId == dentalRecordId)
              .FirstOrDefaultAsync(cancellationToken);

            if (dentalRecords == null)
            {
                throw new Exception("Dental Record not found.");
            }

            return dentalRecords;
        }

      
    }

}
