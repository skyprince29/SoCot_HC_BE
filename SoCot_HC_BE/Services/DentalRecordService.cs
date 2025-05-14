using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

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

        public async Task<DentalRecord> CreateDentalRecord(String ReferralNo, CancellationToken cancellationToken = default) {
        
        DentalTreatment dentalTreatment = await _context.DentalTreatment
                .Include(p => p.Patient)
                .Include(pr => pr.Facility)
                .Include(pr => pr.PatientRegistry)
                .FirstOrDefaultAsync(i => i.PatientRegistry.ReferralNo == ReferralNo);

            if (dentalTreatment == null) {
                throw new ArgumentNullException("Dental Treatment not found.");
            }

            DentalRecord dentalRecord = new DentalRecord();
            dentalRecord.PatientId = dentalTreatment.PatientId;
            dentalRecord.Patient = dentalTreatment.Patient;
            dentalRecord.FacilityId = dentalTreatment.FacilityId;
            dentalRecord.Facility = dentalTreatment.Facility;
            dentalRecord.ReferralNo = dentalTreatment.PatientRegistry.ReferralNo;
            dentalRecord.CreatedBy = dentalTreatment.CreatedBy;
            dentalRecord.CreatedDate = DateTime.Now;
            dentalRecord.DateRecord = DateTime.Now;

            dentalRecord.DentalRecordDetailsMedicalHistory = new DentalRecordDetailsMedicalHistory();   
            dentalRecord.DentalRecordDetailsSocialHistory = new DentalRecordDetailsSocialHistory();   
            dentalRecord.DentalRecordDetailsOralHealthCondition = new DentalRecordDetailsOralHealthCondition();   
            dentalRecord.DentalRecordDetailsPresence = new DentalRecordDetailsPresence();   
            dentalRecord.DentalRecordDetailsToothCount = new DentalRecordDetailsToothCount();   

            return dentalRecord;
        }

        public async Task SaveOrUpdateDentalRecord(DentalRecord dentalRecord, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        //    public async Task SaveOrUpdateDentalRecord(
        //        DentalRecord dentalRecord
        //        , CancellationToken cancellationToken)
        //    {

        //        bool isNew = dentalRecord.DentalRecordId == Guid.Empty;

        //        if (isNew) {

        //        }

        //        await AddAsync(dentalRecord, cancellationToken);

        //    }
    }

}
