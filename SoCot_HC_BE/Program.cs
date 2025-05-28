using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Designations.Interfaces;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Personnels;
using SoCot_HC_BE.Personnels.Interfaces;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Repositories.Interfaces;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;
using System.Text;

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

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<JwtSettings>>().Value);

// Load the settings directly (optional, for building key)
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

if (jwtSettings == null)
{
    throw new InvalidOperationException("JWT Key is missing or too short (must be at least 16 characters).");
} else
{
    if (string.IsNullOrWhiteSpace(jwtSettings.Key) || jwtSettings.Key.Length < 16)
    {
        throw new InvalidOperationException("JWT Key is missing or too short (must be at least 16 characters).");
    }
}

var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

// Register authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // for development only
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // optional, remove time drift
    };
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IJwtService, JwtService>();
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
builder.Services.AddScoped<IModuleStatusFlowService, ModuleStatusFlowService>();
builder.Services.AddScoped<ITransactionFlowHistoryService, TransactionFlowHistoryService>();
builder.Services.AddScoped<IDentalTreatmentService, DentalTreatmentService>();
builder.Services.AddScoped<IDentalRecordService, DentalRecordService>();
builder.Services.AddScoped<IUserGroupService, UserGroupService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<INonCommunicableDiseaseService, NonCommunicableDiseaseService>();

builder.Services.AddScoped<ModuleServiceMapper>();

// HTTP CLinet Injection
builder.Services.AddHttpClient<IReferralService, ReferralService>();

// Register HttpContextAccessor for cancellation token usage (optional, but useful)
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IItemCategoryService, ItemCategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IStrengthService, StrengthService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IUoMService, UoMService>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter()); // makes all routes require authorization by default
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Add JWT Bearer definition
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: Bearer eyJhbGciOiJIUzI1NiIs...",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme,
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Enable for Development Only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable for Publishing
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors();

// Apply Authorization middleware (only needed if you have authorization in place)
app.UseAuthentication(); // must come before Authorization
app.UseAuthorization();


// Map controllers to endpoints
app.MapControllers();

// Run the application
app.Run();
