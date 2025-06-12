using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        // Get a specific service by ID
        [HttpGet("GetService/{id}")]
        public async Task<IActionResult> GetService(Guid id, CancellationToken cancellationToken)
        {
            var service = await _serviceService.GetAsync(id, cancellationToken);
            if (service == null)
            {
                return NotFound(new { success = false, message = "Service not found." });
            }

            return Ok(service);
        }

        // Get all services with paging
        [HttpGet("GetPagedServices")]
        public async Task<IActionResult> GetPagedServices(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var services = await _serviceService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _serviceService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<Service>(services, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        // Save or update a Service
        [HttpPost("SaveService")]
        public async Task<IActionResult> SaveService(ServiceDto service, CancellationToken cancellationToken)
        {
            try
            {
                await _serviceService.SaveServiceAsync(service, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = service.ServiceId == Guid.Empty
                        ? "Service created successfully."
                        : "Service updated successfully."
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

        [HttpGet("GetDepartmentByServiceId/{serviceId}")]
        public async Task<IActionResult> GetDepartmentByServiceId(Guid serviceId, CancellationToken cancellationToken)
        {
            var department = await _serviceService.GetDepartmentByServiceIdAsync(serviceId, cancellationToken);

            if (department == null)
                return NotFound(new { success = false, message = "Department not found." });

            return Ok(department);
        }

        [HttpGet("GetServicesByDepartment/{departmentId}")]
        public async Task<IActionResult> GetServicesByDepartment(Guid departmentId, CancellationToken cancellationToken)
        {
            var services = await _serviceService.GetServicesByDepartment(departmentId, cancellationToken);

            if (services == null)
                return NotFound(new { success = false, message = "Department not found." });

            return Ok(services);
        }

        [HttpGet("GetServicesByFacility/{facilityId}")]
        public async Task<IActionResult> GetServicesByFacility(int facilityId, CancellationToken cancellationToken)
        {
            var services = await _serviceService.GetServicesByFacility(facilityId, cancellationToken);

            if (services == null)
                return NotFound(new { success = false, message = "Services not found." });

            return Ok(services);
        }

        [HttpGet("GetDepartmentFlowsByServiceId/{serviceId}")]
        public async Task<IActionResult> GetDepartmentFlowsByServiceIdAsync(
            Guid serviceId,
            Guid? excludeDepartmentId,
            CancellationToken cancellationToken = default)
        {
            var services = await _serviceService.GetDepartmentFlowsByServiceIdAsync(serviceId, excludeDepartmentId, cancellationToken);

            if (services == null)
                return NotFound(new { success = false, message = "Services not found." });

            return Ok(services);
        }
    }
} 