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
using SoCot_HC_BE.Hub; // Corrected Hub namespace: Assuming this is where PatientDepartmentTransactionHub is
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Personnels;
using SoCot_HC_BE.Personnels.Interfaces;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Repositories.Interfaces;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;
using System.Text;
using ReferralService = SoCot_HC_BE.Services.ReferralService;

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
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
         policy.WithOrigins()
            .AllowAnyMethod()    // Crucial for OPTIONS preflight, POST, GET, etc.
            .AllowAnyHeader()    // Allows any headers (e.g., Content-Type, Authorization)
            .AllowCredentials(); // REQUIRED for SignalR (and if you send cookies/auth tokens)
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
}
else
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
builder.Services.AddScoped<IPatientDepartmentTransactionService, PatientDepartmentTransactionService>();

builder.Services.AddScoped<ModuleServiceMapper>();

// HTTP CLinet Injection
builder.Services.AddHttpClient<IReferralService, ReferralService>();

// Register HttpContextAccessor for cancellation token usage (optional, but useful)
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemCategoryService, ItemCategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IStrengthService, StrengthService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IUoMService, UoMService>();
builder.Services.AddScoped<IUserDepartmentService, UserDepartmentService>();

// This Adds AuthorizationFilter to ALL controllers. Keep this in mind if some should be anonymous.
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
            Type = ReferenceType.SecurityScheme // FIX: Changed from SecuritySchemeType.SecurityScheme to ReferenceType.SecurityScheme
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

builder.Services.AddSignalR(); // Make sure this is called once.

var app = builder.Build();

// Enable for Development Only (It's fine to have both if you want Swagger in production, just be aware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable for Publishing (Duplicate of above, can be removed if not needed in prod)
// It's generally recommended to only enable Swagger UI in development environments
// for security reasons, unless you have a specific need and secure it properly.
// app.UseSwagger();
// app.UseSwaggerUI();

app.UseHttpsRedirection();

// **CRITICAL FIXES START HERE**

// 1. APPLY CORS POLICY (MUST be before UseRouting, UseAuthentication, UseAuthorization, MapHub, MapControllers)
app.UseCors("AllowSpecificOrigins"); // <-- Changed from app.UseCors() to apply the named policy!

// 2. Routing middleware (MUST come before Authentication and Authorization)
app.UseRouting();

// 3. Authentication and Authorization middleware
app.UseAuthentication(); // Must come before Authorization
app.UseAuthorization();

// 4. Map Endpoints (Hubs and Controllers)
app.MapHub<AppHub>("/appHub"); // <-- Hub mapping
app.MapControllers(); // <-- Controller mapping

// Run the application
app.Run();
