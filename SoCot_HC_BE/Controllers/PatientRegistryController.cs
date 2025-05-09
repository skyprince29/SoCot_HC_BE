using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientRegistryController : Controller
    {
        private readonly IPatientRegistryService _patientRegistryService;

        public PatientRegistryController(IPatientRegistryService patientRegistryService)
        {
            _patientRegistryService = patientRegistryService;
        }

        // Save or update a Patient Registry
        [HttpPost("SavePatientRegistry")]
        public async Task<IActionResult> SavePatientRegistry(PatientRegistry patientRegistry, CancellationToken cancellationToken)
        {
            try
            {
                await _patientRegistryService.SavePatientRegistryAsync(patientRegistry, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = patientRegistry.PatientRegistryId == Guid.Empty
                        ? "Patient Registry created successfully."
                        : "Patient Registry updated successfully."
                });
            }
            catch (ModelValidationException ex)
            {
                foreach (var kvp in ex.Errors)
                {
                    foreach (var error in kvp.Value)
                    {
                        ModelState.AddModelError(kvp.Key, error);
                    }
                }

                var modelErrors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(new { success = false, errors = modelErrors });
            }
        }

        // Create a new Patient Registry
        [HttpPost("CreatePatientRegistry")]
        public async Task<IActionResult> CreatePatientRegistry(
            string? referralNo, 
            Guid patientId, 
            PatientRegistryType patientRegistryType, 
            int facilityId, 
            bool isUrgent = false, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Call the service method to create a new PatientRegistry
                var newPatientRegistry = await _patientRegistryService.CreatePatientRegistryAsync(
                    referralNo, 
                    patientId, 
                    patientRegistryType, 
                    facilityId, 
                    isUrgent, 
                    cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = "Patient Registry created successfully.",
                    data = newPatientRegistry
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a bad request response
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // Get a specific Patient Registry by ID
        [HttpGet("GetPatientRegistry/{id}")]
        public async Task<IActionResult> GetPatientRegistry(Guid id, CancellationToken cancellationToken)
        {
            var patientRegistry = await _patientRegistryService.GetAsync(id, cancellationToken);
            if (patientRegistry == null)
            {
                return NotFound(new { success = false, message = "Patient Registry not found." });
            }

            return Ok(patientRegistry);
        }

        // Get all Patient Registry with paging
        [HttpGet("GetPagedPatientRegistries")]
        public async Task<IActionResult> GetPagedPatientRegistries(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var patientRegistries = await _patientRegistryService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _patientRegistryService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<PatientRegistry>(patientRegistries, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }
    }
}
