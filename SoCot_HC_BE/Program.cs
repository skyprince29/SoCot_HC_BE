using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Designations.Interfaces;
using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Personnels;
using SoCot_HC_BE.Personnels.Interfaces;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Repositories.Interfaces;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(x =>
     {
         x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
         x.JsonSerializerOptions.WriteIndented = true; // Optional: for prettier output
         x.JsonSerializerOptions.PropertyNamingPolicy = null;
     });

// Swagger Setup (API documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Configuration (consider tightening this in production)
builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add Database Context with SQL Server connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Generic Repository
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

builder.Services.Configure<ExternalApiSettings>(
    builder.Configuration.GetSection("ExternalApiSettings"));
// Register Specific Service
builder.Services.AddScoped<IVitalSignService, VitalSignService>();
builder.Services.AddScoped<IPatientRegistryService, PatientRegistryService>();
builder.Services.AddScoped<IServiceClassificationService, ServiceClassificationService>();
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IHouseholdService, HouseholdService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonnelService, PersonnelService>();
builder.Services.AddScoped<IProvinceService, ProvinceService>();
builder.Services.AddScoped<ICityMunicipalService, CityMunicipalService>();
builder.Services.AddScoped<IBarangayService, BarangayService>();
builder.Services.AddScoped<IDepartmentTypeService, DepartmentTypeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
builder.Services.AddScoped<IWoundTypeService, WoundTypeService>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<ISupplyStorageService, SupplyStorageService>();

// HTTP CLinet Injection
builder.Services.AddHttpClient<IReferralService, ReferralService>();

builder.Services.AddScoped<IDentalTreatmentService, DentalTreatmentService>();
builder.Services.AddScoped<IDentalRecordService, DentalRecordService>();
// Register HttpContextAccessor for cancellation token usage (optional, but useful)
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient<IReferralService, ReferralService>();
builder.Services.AddScoped<IUserGroupService, UserGroupService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();

var app = builder.Build();

// Enable for Development Only
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

// Enable for Publishing
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors();

// Apply Authorization middleware (only needed if you have authorization in place)
app.UseAuthorization();

// Map controllers to endpoints
app.MapControllers();

// Run the application
app.Run();
