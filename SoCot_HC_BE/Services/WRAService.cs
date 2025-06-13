using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Repositories.Interfaces;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using static SoCot_HC_BE.DTO.DentalDTO;

namespace SoCot_HC_BE.Services
{
    public class WRAService : Repository<WRA, Guid>, IWRAService
    {

        public WRAService(AppDbContext context) : base(context)
        {
        }

        public override async Task<WRA?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // You can include related data here if needed, like navigation properties
            var wra = await _dbSet
                .Include(i => i.Person)
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            return wra;
        }

        // Get a list of WRA with paging and cancellation support.
        public async Task<List<WRA>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                 .Include(s => s.Person)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s =>
                    s.Person != null &&
                    (s.Person.Firstname + " " + s.Person.Middlename + " " + s.Person.Lastname).Contains(keyword)
                );
            }
            return await query
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken); // Pass the CancellationToken here
        }

        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s =>
                    s.Person != null &&
                    (s.Person.Firstname + " " + s.Person.Middlename + " " + s.Person.Lastname).Contains(keyword)
                );
            }
            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        }

        public async Task SaveWRAAsync(WRADto wRADto, CancellationToken cancellationToken = default)
        {
            // Determine if new or existing
            bool isNew = wRADto.Id == Guid.Empty;
            ValidateFields(wRADto);


            if (isNew)
            {
                string timestamp = DateTime.Now.ToString("yyMMdd-HHmmss");
                var wra = new WRA()
                {
                    
                    Id = wRADto.Id,
                    ForCounseling = true,
                    PersonId = wRADto.PersonId,
                    Fecundity = wRADto.Fecundity,
                    HavePartner = wRADto.HavePartner,
                    FPMethod = wRADto.FPMethod,
                    FPType = wRADto.FPType,
                    ShiftToModernMethod = wRADto.ShiftToModernMethod,
                    UsingAnyFPMethod = wRADto.UsingAnyFPMethod,
                    WraPlanToHveMoreChildrenDecision = wRADto.WraPlanToHveMoreChildrenDecision,
                    WraPlanToHaveMoreChildren = wRADto.WraPlanToHaveMoreChildren,
                    WraDateOfAssessment = wRADto.WraDateOfAssessment,
                    WraDateRecorded = wRADto.WraDateRecorded,
                    CreatedDate = DateTime.UtcNow
                };
                //TODO: Update code
                await AddAsync(wra, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { wRADto.Id }, cancellationToken);
                if (existing == null)
                    throw new Exception("WRA not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(wRADto);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(WRADto wra)
        {
            var errors = new Dictionary<string, List<string>>();

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}