using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Repositories.Interfaces;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SoCot_HC_BE.Services
{
    public class NonCommunicableDiseaseService : Repository<NonCommunicableDisease, Guid>, INonCommunicableDiseaseService
    {
        public NonCommunicableDiseaseService(AppDbContext context) : base(context)
        {
        }


        public async Task<NonCommunicableDisease> getNCDAsync(Guid NCDId, CancellationToken cancellationToken) {

            var NCD = await _dbSet
                .Include(n => n.FamilyHistory)
                .Include(n => n.FirstVitalSign)
                .Include(n => n.SecondVitalSign)
                .FirstOrDefaultAsync(n => n.Id == NCDId, cancellationToken);

            if (NCD == null)
            {
                throw new Exception("Non Communicable Disease not found.");
            }

            return NCD;
        }


        public async Task<NonCommunicableDiseaseDto> getNCDDtoAsync(Guid NCDId, CancellationToken cancellationToken)
        {

        NonCommunicableDisease ncd = await getNCDAsync(NCDId, cancellationToken);
        FamilyHistory familyHistory = ncd.FamilyHistory;
        VitalSign firstVitalSign = ncd.FirstVitalSign;
        VitalSign secondVitalSign = ncd.SecondVitalSign;

        FamilyHistoryDto familyHistoryDto = new FamilyHistoryDto();
        familyHistoryDto.Id = familyHistory != null ? familyHistory.Id : Guid.Empty;
        familyHistoryDto.Stroke = familyHistory != null && familyHistory.Stroke;
        familyHistoryDto.HeartAttack = familyHistory != null && familyHistory.HeartAttack;
        familyHistoryDto.Diabetes = familyHistory != null && familyHistory.Diabetes;
        familyHistoryDto.Asthma = familyHistory != null && familyHistory.Asthma;
        familyHistoryDto.Cancer = familyHistory != null && familyHistory.Cancer;
        familyHistoryDto.KidneyDisease = familyHistory != null && familyHistory.KidneyDisease;

        VitalSignDto firstVitalSignDto = new VitalSignDto();
        firstVitalSignDto.VitalSignId = firstVitalSign == null ? Guid.Empty : firstVitalSign.VitalSignId;
        //firstVitalSignDto.PatientRegistryId = firstVitalSign == null ? Guid.Empty : firstVitalSign.PatientRegistryId;
        firstVitalSignDto.Temperature = firstVitalSign == null ? null : firstVitalSign.Temperature;
        firstVitalSignDto.Height = firstVitalSign == null ? decimal.Zero : firstVitalSign.Height;
        firstVitalSignDto.Weight = firstVitalSign == null ? decimal.Zero : firstVitalSign.Weight;
        firstVitalSignDto.RespiratoryRate = firstVitalSign == null ? null : firstVitalSign.RespiratoryRate;
        firstVitalSignDto.CardiacRate = firstVitalSign == null ? null : firstVitalSign.CardiacRate;
        firstVitalSignDto.Systolic = firstVitalSign == null ? 0 : firstVitalSign.Systolic;
        firstVitalSignDto.Diastolic = firstVitalSign == null ? 0 : firstVitalSign.Diastolic;
        firstVitalSignDto.BloodPressure = firstVitalSign == null ? null : firstVitalSign.BloodPressure;

        VitalSignDto secondVitalSignDto = new VitalSignDto();
        secondVitalSignDto.VitalSignId = secondVitalSign == null ? Guid.Empty : secondVitalSign.VitalSignId;
        //secondVitalSignDto.PatientRegistryId = secondVitalSign == null ? Guid.Empty : secondVitalSign.PatientRegistryId;
        secondVitalSignDto.Temperature = secondVitalSign == null ? null : secondVitalSign.Temperature;
        secondVitalSignDto.Height = secondVitalSign == null ? decimal.Zero : secondVitalSign.Height;
        secondVitalSignDto.Weight = secondVitalSign == null ? decimal.Zero : secondVitalSign.Weight;
        secondVitalSignDto.RespiratoryRate = secondVitalSign == null ? null : secondVitalSign.RespiratoryRate;
        secondVitalSignDto.CardiacRate = secondVitalSign == null ? null : secondVitalSign.CardiacRate;
        secondVitalSignDto.Systolic = secondVitalSign == null ? 0 : secondVitalSign.Systolic;
        secondVitalSignDto.Diastolic = secondVitalSign == null ? 0 : secondVitalSign.Diastolic;
        secondVitalSignDto.BloodPressure = secondVitalSign == null ? null : secondVitalSign.BloodPressure;

        NonCommunicableDiseaseDto ndcDto = new NonCommunicableDiseaseDto();
        ndcDto.Id = ncd.Id;
        ndcDto.PatientId = ncd.PatientId;
        ndcDto.FamilyHistoryId = familyHistoryDto.Id;
        ndcDto.FamilyHistory = familyHistoryDto;
        ndcDto.Waist = ncd.Waist;
        ndcDto.FirstVitalSignId = firstVitalSignDto.VitalSignId;
        ndcDto.FirstVitalSign = firstVitalSignDto;
        ndcDto.SecondVitalSignId = secondVitalSignDto.VitalSignId;
        ndcDto.SecondVitalSign = secondVitalSignDto;
        ndcDto.AverageBP = ncd.AverageBP;
        ndcDto.Smoking = ncd.Smoking;
        ndcDto.AlcoholIntake = ncd.AlcoholIntake;
        ndcDto.ExcessiveAlcoholIntake = ncd.ExcessiveAlcoholIntake;
        ndcDto.HighFatSalt = ncd.HighFatSalt;
        ndcDto.Vegetable = ncd.Vegetable;
        ndcDto.Fruits = ncd.Fruits;
        ndcDto.PhysicalActivity = ncd.PhysicalActivity;
        ndcDto.withDiabetes = ncd.withDiabetes;
        ndcDto.Polyphagia = ncd.Polyphagia;
        ndcDto.Polydipsia = ncd.Polydipsia;
        ndcDto.Polyuria = ncd.Polyuria;
        ndcDto.withKetones = ncd.withKetones;
        ndcDto.Ketones = ncd.Ketones;
        ndcDto.KetonesDateTaken = ncd.KetonesDateTaken;
        ndcDto.withProtein = ncd.withProtein;
        ndcDto.UrineProtein = ncd.UrineProtein;
        ndcDto.UrineProteinDateTaken = ncd.UrineProteinDateTaken;
        ndcDto.Glucose = ncd.Glucose;
        ndcDto.FBS_RBS = ncd.FBS_RBS;
        ndcDto.GlucoseDateTaken = ncd.GlucoseDateTaken;
        ndcDto.Lipids = ncd.Lipids;
        ndcDto.TotalCholesterol = ncd.TotalCholesterol;
        ndcDto.LipidsDateTaken = ncd.LipidsDateTaken;
        ndcDto.AnginaHeart = ncd.AnginaHeart;
        ndcDto.hasStrokeTIA = ncd.hasStrokeTIA;
        ndcDto.riskLevel = ncd.riskLevel;
        ndcDto.NCDQ1 = ncd.NCDQ1;
        ndcDto.NCDQ2 = ncd.NCDQ2;
        ndcDto.NCDQ3 = ncd.NCDQ3;
        ndcDto.NCDQ4 = ncd.NCDQ4;
        ndcDto.NCDQ5 = ncd.NCDQ5;
        ndcDto.NCDQ6 = ncd.NCDQ6;
        ndcDto.NCDQ7 = ncd.NCDQ7;
        ndcDto.NCDQ8 = ncd.NCDQ8;
        ndcDto.DateAssed = ncd.DateAssed;

        return ndcDto;
        }



        public async Task<PaginationHandler<NonCommunicableDisease>> GetAllWithPagingAsync(int pageNo, int limit, string keyword = "", CancellationToken cancellationToken = default)
        {
            var NonCommunicableDiseases = await _dbSet
                .Include(p => p.Patient)
                .Include(p => p.FamilyHistory)
                .Include(p => p.FirstVitalSign)
                .Include(p => p.SecondVitalSign)
                .Where(
                dt =>
                dt.Patient.Firstname.Contains(keyword) ||
                dt.Patient.Lastname.Contains(keyword) ||
                (dt.Patient.Middlename != null && dt.Patient.Middlename.Contains(keyword))
                )
               .AsNoTracking()
               .ToListAsync();

            int totalRecords = NonCommunicableDiseases.Count;


            var paginatedResult = new PaginationHandler<NonCommunicableDisease>(NonCommunicableDiseases, totalRecords, pageNo, limit);
            return paginatedResult;
        }

        public async Task SaveOrUpdateDentalRecordAsync(NonCommunicableDiseaseDto NDCDto, CancellationToken cancellationToken = default)
        {
            // check if model has id conclusion update else save
            bool isNew = NDCDto.Id == Guid.Empty;

            //ValidateFields(NDCDto); // validate NCD data
            var NCD = manageDtoToModel(NDCDto); // manage of data from dto to model
            FamilyHistory familyHistory = NCD.FamilyHistory;
            VitalSign firstVitalSign = NCD.FirstVitalSign;
            VitalSign secondVitalSign = NCD.SecondVitalSign;


            if (familyHistory != null) {
                if (familyHistory.Id != Guid.Empty)
                {
                    _context.Add(familyHistory);
                }
            }

            if (firstVitalSign != null) {
                if (firstVitalSign.VitalSignId != Guid.Empty)
                {
                    _context.Add(firstVitalSign);
                }
            }

            if (secondVitalSign != null)
            {
                if (secondVitalSign.VitalSignId != Guid.Empty)
                {
                    _context.Add(secondVitalSign);
                }
            }


            if (isNew)
            {
                await AddAsync(NCD, cancellationToken);
            }
            else
            {
                var existingModel = await _dbSet
                    .Include(d => d.Patient)
                    .Include(d => d.FamilyHistory)
                    .Include(d => d.FirstVitalSign)
                    .Include(d => d.SecondVitalSign)
                    .FirstOrDefaultAsync(d => d.Id == NCD.Id, cancellationToken);

                if (existingModel == null)
                {
                    throw new Exception("Non Communicable Disease not found.");
                }

                _context.Entry(existingModel).CurrentValues.SetValues(NCD);
                await UpdateAsync(existingModel, cancellationToken);

                throw new NotImplementedException();
            }
        }

        public NonCommunicableDisease manageDtoToModel(NonCommunicableDiseaseDto  NCDDto)
        {

            FamilyHistory familyHistory = manageFamilyHistoryDtoToModel(NCDDto.FamilyHistory);
            VitalSign firstVitalSign = vitalSignDtoToModel(NCDDto.FirstVitalSign);
            VitalSign secondVitalSign = vitalSignDtoToModel(NCDDto.SecondVitalSign);

        NonCommunicableDisease NCD = new NonCommunicableDisease();
            NCD.Id = NCDDto.Id; 
            NCD.PatientId = NCDDto.PatientId; 
            NCD.FamilyHistoryId = familyHistory.Id;
            NCD.FamilyHistory = familyHistory;
            NCD.Waist = NCDDto.Waist; 
            NCD.FirstVitalSignId = firstVitalSign.VitalSignId;
            NCD.FirstVitalSign = firstVitalSign;
            NCD.SecondVitalSignId = secondVitalSign.VitalSignId;
            NCD.SecondVitalSign = secondVitalSign;
            NCD.AverageBP = NCDDto.AverageBP; 
            NCD.Smoking = NCDDto.Smoking; 
            NCD.AlcoholIntake = NCDDto.AlcoholIntake; 
            NCD.ExcessiveAlcoholIntake = NCDDto.ExcessiveAlcoholIntake; 
            NCD.HighFatSalt = NCDDto.HighFatSalt; 
            NCD.Vegetable = NCDDto.Vegetable; 
            NCD.Fruits = NCDDto.Fruits; 
            NCD.PhysicalActivity = NCDDto.PhysicalActivity; 
            NCD.withDiabetes = NCDDto.withDiabetes; 
            NCD.Polyphagia = NCDDto.Polyphagia; 
            NCD.Polydipsia = NCDDto.Polydipsia; 
            NCD.Polyuria = NCDDto.Polyuria; 
            NCD.withKetones = NCDDto.withKetones; 
            NCD.Ketones = NCDDto.Ketones; 
            NCD.KetonesDateTaken = NCDDto.KetonesDateTaken; 
            NCD.withProtein = NCDDto.withProtein; 
            NCD.UrineProtein = NCDDto.UrineProtein; 
            NCD.UrineProteinDateTaken = NCDDto.UrineProteinDateTaken; 
            NCD.Glucose = NCDDto.Glucose; 
            NCD.FBS_RBS = NCDDto.FBS_RBS; 
            NCD.GlucoseDateTaken = NCDDto.GlucoseDateTaken; 
            NCD.Lipids = NCDDto.Lipids; 
            NCD.TotalCholesterol = NCDDto.TotalCholesterol; 
            NCD.LipidsDateTaken = NCDDto.LipidsDateTaken; 
            NCD.AnginaHeart = NCDDto.AnginaHeart; 
            NCD.hasStrokeTIA = NCDDto.hasStrokeTIA; 
            NCD.riskLevel = NCDDto.riskLevel; 
            NCD.NCDQ1 = NCDDto.NCDQ1; 
            NCD.NCDQ2 = NCDDto.NCDQ2; 
            NCD.NCDQ3 = NCDDto.NCDQ3; 
            NCD.NCDQ4 = NCDDto.NCDQ4; 
            NCD.NCDQ5 = NCDDto.NCDQ5; 
            NCD.NCDQ6 = NCDDto.NCDQ6; 
            NCD.NCDQ7 = NCDDto.NCDQ7; 
            NCD.NCDQ8 = NCDDto.NCDQ8; 
            NCD.DateAssed = NCDDto.DateAssed; 


            return NCD;
        }

        //private void ValidateFields(NonCommunicableDiseaseDto NCDDto)
        //{
        //    var errors = new Dictionary<string, List<string>>();

        //    ValidationHelper.IsRequired(errors, nameof(department.DepartmentName), department.DepartmentName, "Department Name");

        //    if (errors.Any())
        //        throw new ModelValidationException("Validation failed", errors);
        //}

        public FamilyHistory manageFamilyHistoryDtoToModel(FamilyHistoryDto? familyHistoryDto) {

            FamilyHistory familyHistory = new FamilyHistory();
            familyHistory.Id = familyHistoryDto != null ? familyHistoryDto.Id : Guid.Empty;
            familyHistory.Stroke = familyHistoryDto != null && familyHistoryDto.Stroke;
            familyHistory.HeartAttack = familyHistoryDto != null && familyHistoryDto.HeartAttack;
            familyHistory.Diabetes = familyHistoryDto != null && familyHistoryDto.Diabetes;
            familyHistory.Asthma = familyHistoryDto != null && familyHistoryDto.Asthma;
            familyHistory.Cancer = familyHistoryDto != null && familyHistoryDto.Cancer;
            familyHistory.KidneyDisease = familyHistoryDto != null && familyHistoryDto.KidneyDisease;

            return familyHistory;
        }

        public VitalSign vitalSignDtoToModel(VitalSignDto? vitalSignDto) { 
            VitalSign vitalSign = new VitalSign();

            vitalSign.VitalSignId = vitalSignDto == null ? Guid.Empty : vitalSignDto.VitalSignId;
            //vitalSign.PatientRegistryId = vitalSignDto == null ? Guid.Empty : vitalSignDto.PatientRegistryId;
            vitalSign.Temperature = vitalSignDto == null ? null : vitalSignDto.Temperature;
            vitalSign.Height = vitalSignDto == null ? decimal.Zero : vitalSignDto.Height;
            vitalSign.Weight = vitalSignDto == null ? decimal.Zero : vitalSignDto.Weight;
            vitalSign.RespiratoryRate = vitalSignDto == null ? null : vitalSignDto.RespiratoryRate;
            vitalSign.CardiacRate = vitalSignDto == null ? null : vitalSignDto.CardiacRate;
            vitalSign.Systolic = vitalSignDto == null ? 0 : vitalSignDto.Systolic;
            vitalSign.Diastolic = vitalSignDto == null ? 0 : vitalSignDto.Diastolic;
            vitalSign.BloodPressure = vitalSignDto == null ? null : vitalSignDto.BloodPressure;

            return vitalSign;

        }
    }



}
