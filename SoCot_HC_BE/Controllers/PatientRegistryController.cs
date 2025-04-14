using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
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
        [HttpPost(Name = "SavePatientRegistry")]
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
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(new { success = false, errors = modelErrors });
            }
        }

        // Get a specific Patient Registry by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientRegistry(Guid id, CancellationToken cancellationToken)
        {
            var patientRegistry = await _patientRegistryService.GetAsync(id, cancellationToken);
            if (patientRegistry == null)
            {
                return NotFound(new { success = false, message = "Patient Registry not found." });
            }

            return Ok(patientRegistry);
        }
    }
}
