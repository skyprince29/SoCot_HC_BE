using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceCategoryController : Controller
    {
        private readonly IServiceCategoryService _serviceCategoryService;
        public ServiceCategoryController(IServiceCategoryService serviceCategoryService)
        {
            _serviceCategoryService = serviceCategoryService;
        }

        [HttpGet("GetServiceCategory/{id}")]
        public async Task<IActionResult> GetServiceCategory(Guid id, CancellationToken cancellationToken)
        {
            var serviceCategory = await _serviceCategoryService.GetAsync(id, cancellationToken);
            if (serviceCategory == null)
            {
                return NotFound(new { success = false, message = "Service Catetgory not found." });
            }

            return Ok(serviceCategory);
        }

        [HttpGet("GetServiceCategories")]
        public async Task<IActionResult> GetServiceCategories(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<ServiceCategory> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _serviceCategoryService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _serviceCategoryService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _serviceCategoryService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }

        [HttpGet("GetPagedServiceCategories")]
        public async Task<IActionResult> GetPagedServiceCategories(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var serviceCategories = await _serviceCategoryService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _serviceCategoryService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<ServiceCategory>(serviceCategories, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        [HttpPost("SaveServiceCategory")]
        public async Task<IActionResult> SaveServiceCategory(ServiceCategory serviceCategory, CancellationToken cancellationToken)
        {
            try
            {
                await _serviceCategoryService.SaveServiceCategoryAsync(serviceCategory, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = serviceCategory.ServiceCategoryId == Guid.Empty
                        ? "Service Category created successfully."
                        : "Service Category updated successfully."
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
    }
}
