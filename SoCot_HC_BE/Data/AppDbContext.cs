﻿using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.BaseModels;
using System.Security.Claims;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Declare the trigger for Person table to avoid OUTPUT without INTO
            modelBuilder.Entity<Person>().ToTable(tb =>
            {
                tb.HasTrigger("trg_UpdateFullNames");
            });

            // Declare other triggers here
        }


        public DbSet<VitalSign> VitalSigns { get; set; }
        public DbSet<Province> Province {  get; set; }
        public DbSet<Municipality> Municipality { get; set; }
        public DbSet<Barangay> Barangay { get; set; }
        public DbSet<PatientRegistry> PatientRegistry { get; set; }
        public DbSet<ServiceClassification> ServiceClassification { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Facility> Facility { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Personnel> Personnel { get; set; }
        public DbSet<Referral> Referral { get; set; }
        public DbSet<DepartmentType> DepartmentType { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<DepartmentDepartmentType> DepartmentDepartmentType { get; set; }
        public DbSet<ServiceCategory> ServiceCategory { get; set; }
        public DbSet<WoundType> WoundType { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<WRA> WRA { get; set; }
        public DbSet<SchoolAgeProfile> SchoolAgeProfile { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<PatientDepartmentTransaction> PatientDepartmentTransaction { get; set; }
        public DbSet<ItemCategory> ItemCategory { get; set; }
        public DbSet<Form> Form { get; set; }
        public DbSet<Model.Route> Route { get; set; } // special case because of ambiguity  due to naming collisions with .NET/ASP.NET classes.
        public DbSet<Strength> Strength { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<UoM> UoM { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<DentalTreatment> DentalTreatment { get; set; }
        public DbSet<DentalRecord> DentalRecord { get; set; }
        public DbSet<DentalRecordDetailsFindings> DentalRecordDetailsFindings { get; set; }
        public DbSet<DentalRecordDetailsMedicalHistory> DentalRecordDetailsMedicalHistory { get; set; }
        public DbSet<DentalRecordDetailsOralHealthCondition> DentalRecordDetailsOralHealthCondition { get; set; }
        public DbSet<DentalRecordDetailsPresence> DentalRecordDetailsPresence { get; set; }
        public DbSet<DentalRecordDetailsServices> DentalRecordDetailsServices { get; set; }
        public DbSet<DentalRecordDetailsSocialHistory> DentalRecordDetailsSocialHistory { get; set; }
        public DbSet<DentalRecordDetailsToothCount> DentalRecordDetailsToothCount { get; set; }
        public DbSet<SupplyStorage> SupplyStorage { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<TransactionFlowHistory> TransactionFlowHistory { get; set; }
        public DbSet<ModuleStatusFlow> ModuleStatusFlow { get; set; }
        public DbSet<NonCommunicableDisease> NonCommunicableDisease { get; set; }
        public DbSet<UserDepartment> UserDepartment { get; set; }
        public DbSet<ServiceDepartment> ServiceDepartment { get; set; }
        public DbSet<VitalSignReference> VitalSignReference { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<ActivityLog> ActivityLog { get; set; }

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
        public UserData GetCurrentUser()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var fullName = _httpContextAccessor.HttpContext?.User?.FindFirst("FullName")?.Value;
            var designation = _httpContextAccessor.HttpContext?.User?.FindFirst("Designation")?.Value; // Using Designation

            // Define a default GUID to use when the actual userIdClaim is missing or invalid
            var defaultUserId = Guid.Parse("00000001-0001-0001-0001-000000000001"); // Your default GUID here

            return new UserData
            {
                UserId = Guid.TryParse(userIdClaim, out var userId) ? userId : defaultUserId, // Assign the default GUID if invalid
                FullName = fullName ?? "Default User",  // Default value for FullName
                Designation = designation ?? "No Designation" // Default value for Designation
            };
        }

    }
}
