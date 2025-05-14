using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class SupplyStorageService : Repository<SupplyStorage, Guid>, ISupplyStorageService
    {
        public SupplyStorageService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<SupplyStorage>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(s => s.Facility)
                .Include(s => s.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => s.SupplyStorageName.Contains(keyword));
            }

            return await query
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => s.SupplyStorageName.Contains(keyword));
            }

            return await query.CountAsync(cancellationToken);
        }

        private SupplyStorage DTOToModel(SupplyStorageDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new SupplyStorage
            {
                SupplyStorageId = dto.SupplyStorageId == Guid.Empty ? Guid.Empty : dto.SupplyStorageId,
                SupplyStorageName = dto.SupplyStorageName?? "",
                Description = dto.Description,
                FacilityId = dto.FacilityId,
                DepartmentId = dto.DepartmentId,
                IsActive = dto.IsActive,
                // Copying base audit properties
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate,
                UpdatedBy = dto.UpdatedBy,
                UpdatedDate = dto.UpdatedDate
            };
        }

        public async Task SaveSupplyStorageAsync(SupplyStorageDto supplyStorageDto, CancellationToken cancellationToken = default)
        {
            ValidateFields(supplyStorageDto);
            SupplyStorage supplyStorage = DTOToModel(supplyStorageDto);
            // Determine if new or existing
            bool isNew = supplyStorage.SupplyStorageId == Guid.Empty;
            if (isNew)
            {
                supplyStorage.SupplyStorageId = Guid.NewGuid();
                supplyStorage.IsActive = true;
                await AddAsync(supplyStorage, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { supplyStorage.SupplyStorageId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Supply Storage not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(supplyStorage);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(SupplyStorageDto supplyStorage)
        {
            var errors = new Dictionary<string, List<string>>();


            int facilityId = supplyStorage.FacilityId;
            string facilityClassName = nameof(supplyStorage.FacilityId);
            ValidationHelper.IsRequired(errors, facilityClassName, facilityId, "Facility");
            // Verify that the FacilityId exists
            var facilityExists = _context.Facility.Any(f => f.FacilityId == facilityId);
            if (!facilityExists && facilityId > 0)
            {
                ValidationHelper.AddError(errors, facilityClassName, "Facility is invalid.");
            }

            Guid departmentId = supplyStorage.DepartmentId;
            string departmentClassName = nameof(supplyStorage.DepartmentId);
            ValidationHelper.IsRequired(errors, departmentClassName, departmentId, "Department");
            // Verify that the FacilityId exists
            var departmentExists = _context.Department.Any(f => f.DepartmentId == departmentId);
            if (!departmentExists && departmentId != Guid.Empty)
            {
                ValidationHelper.AddError(errors, departmentClassName, "Department is invalid.");
            }

            string supplyStorageNameCN = nameof(supplyStorage.SupplyStorageName);
            string supplyStorageName = supplyStorage.SupplyStorageName?? "";
            ValidationHelper.IsRequired(errors, supplyStorageNameCN, supplyStorageName, "Supply Storage Name");
            bool duplicate = _dbSet.Any(s =>
                s.SupplyStorageName == supplyStorageName &&
                s.FacilityId == facilityId &&
                s.DepartmentId == departmentId &&
                s.SupplyStorageId != supplyStorage.SupplyStorageId);

            if (duplicate)
                ValidationHelper.AddError(errors, supplyStorageNameCN, "Supply storage is already exists in this facility.");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}
