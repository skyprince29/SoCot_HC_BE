using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
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

// Register Specific Service
builder.Services.AddScoped<IVitalSignService, VitalSignService>();
builder.Services.AddScoped<IPatientRegistryService, PatientRegistryService>();
builder.Services.AddScoped<IServiceClassificationService, ServiceClassificationService>();
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProvinceService, ProvinceService>();
builder.Services.AddScoped<ICityMunicipalService, CityMunicipalService>();
builder.Services.AddScoped<IBarangayService, BarangayService>();
builder.Services.AddScoped<IDepartmentTypeService, DepartmentTypeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
// Register HttpContextAccessor for cancellation token usage (optional, but useful)
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors();

// Apply Authorization middleware (only needed if you have authorization in place)
app.UseAuthorization();

// Map controllers to endpoints
app.MapControllers();

// Run the application
app.Run();
