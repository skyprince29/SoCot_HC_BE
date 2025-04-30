using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Personnels.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Personnels
{
    public class PersonnelService : Repository<Personnel, Guid>, IPersonnelService
    {
        public PersonnelService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Personnel>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(s => s.Facility)
                .Include(s => s.Person)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s =>
                    s.Person != null &&
                    (s.Person.Firstname + " " + s.Person.Middlename + " " + s.Person.Lastname).Contains(keyword)
                );
            }

            return await query
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    query = query.Where(s => s.ServiceName.Contains(keyword));
            //}

            return await query.CountAsync(cancellationToken);
        }

        public async Task SavePersonnelAsync(Personnel personnel, CancellationToken cancellationToken = default)
        {
            // Determine if new or existing
            bool isNew = personnel.PersonnelId == Guid.Empty;
            ValidateFields(personnel);

            if (isNew)
            {
                personnel.PersonnelId = Guid.NewGuid();
                personnel.IsActive = true;
                await AddAsync(personnel, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { personnel.PersonnelId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Personnel not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(personnel);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(Personnel personnel)
        {
            var errors = new Dictionary<string, List<string>>();


            int facilityId = personnel.FacilityId;
            ValidationHelper.IsRequired(errors, nameof(personnel.FacilityId), facilityId, "Facility");
            // Verify that the FacilityId exists
            var facilityExists = _context.Facility.Any(f => f.FacilityId == facilityId);
            if (!facilityExists && facilityId > 0)
            {
                ValidationHelper.AddError(errors, nameof(personnel.FacilityId), "Facility is invalid.");
            }

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}