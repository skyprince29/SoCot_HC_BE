using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Repositories.Interfaces;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class SchoolAgeProfileService : Repository<SchoolAgeProfile, Guid>, ISchoolAgeProfileService
    {
        public SchoolAgeProfileService(AppDbContext context) : base(context)
        {
        }

        public override async Task<SchoolAgeProfile?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // You can include related data here if needed, like navigation properties
            var schoolageprofile = await _dbSet
                .Include(i => i.Person)
                .FirstOrDefaultAsync(f => f.SchoolAgeProfileId == id, cancellationToken);

            return schoolageprofile;
        }

        public async Task<List<SchoolAgeProfile>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(s => s.Person)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s =>
                    s.Person != null &&
                    (s.Person.Firstname + " " + s.Person.Middlename + " " + s.Person.Lastname).Contains(keyword));
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
                query = query.Where(s =>
                    s.Person != null &&
                    (s.Person.Firstname + " " + s.Person.Middlename + " " + s.Person.Lastname).Contains(keyword));
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task SaveSchoolAgeProfileAsync(SchoolAgeProfileDto schoolageprofiledto, CancellationToken cancellationToken = default)
        {
            // Determine if new or existing
            bool isNew = schoolageprofiledto.SchoolAgeProfileId == Guid.Empty;
            ValidateFields(schoolageprofiledto);

            if (isNew)
            {
                string timestamp = DateTime.Now.ToString("yyMMdd-HHmmss");
                var schoolageprofile = new SchoolAgeProfile()
                {
                    SchoolAgeProfileId = schoolageprofiledto.SchoolAgeProfileId,
                    PersonId = schoolageprofiledto.PersonId,
                    IsInSchool = schoolageprofiledto.IsInSchool,
                    EducationalLevel = schoolageprofiledto.EducationalLevel,
                    Grade = schoolageprofiledto.Grade,
                    SchoolYear = schoolageprofiledto.SchoolYear
                };

                await AddAsync(schoolageprofile, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { schoolageprofiledto.SchoolAgeProfileId }, cancellationToken);
                if (existing == null)
                    throw new Exception("School Age Profile not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(schoolageprofiledto);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(SchoolAgeProfileDto schoolageprofile)
        {
            var errors = new Dictionary<string, List<string>>();

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}
