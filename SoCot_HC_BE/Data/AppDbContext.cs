using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.BaseModels;

namespace SoCot_HC_BE.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<VitalSign> VitalSigns { get; set; }
        public DbSet<Province> Provice {  get; set; }
        public DbSet<CityMunicipality> CityMunicipality { get; set; }
        public DbSet<PatientRegistry> PatientRegistry { get; set; }

        // Add more DbSets here...

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var user = GetCurrentUser();
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditInfo &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                Guid userId = user.UserId;
                var entity = (AuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = userId;
                    entity.CreatedDate = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedBy = userId;
                    entity.UpdatedDate = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        // Retrieve user data (id, name, designation, etc.) from claims
        private UserData GetCurrentUser()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
            var fullName = _httpContextAccessor.HttpContext?.User?.FindFirst("FullName")?.Value;
            var designation = _httpContextAccessor.HttpContext?.User?.FindFirst("Designation")?.Value; // Using Designation

            return new UserData
            {
                UserId = Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty,
                FullName = fullName,
                Designation = designation  // Now using Designation
            };
        }
    }
}
