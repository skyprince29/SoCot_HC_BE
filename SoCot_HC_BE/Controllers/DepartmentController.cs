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
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetDepartment/{id}")]
        public async Task<IActionResult> GetDepartment(Guid id, CancellationToken cancellationToken)
        {
            var department = await _departmentService.GetAsync(id, cancellationToken);
            if (department == null)
            {
                return NotFound(new { success = false, message = "Department not found." });
            }

            return Ok(department);
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetFacilities(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<Department> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _departmentService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _departmentService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _departmentService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }

        [HttpGet("GetPagedDepartments")]
        public async Task<IActionResult> GetPagedDepartments(int pageNo, int statusId, [FromQuery] List<Guid>? departmentTypes,  int limit, CancellationToken cancellationToken, string keyword = "")
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var paginatedResult = await _departmentService.GetAllWithPagingAsync(pageNo, statusId, departmentTypes, limit, keyword, cancellationToken);
            return Ok(paginatedResult);
        }

        [HttpPost("SaveDepartment")]
        public async Task<IActionResult> SaveDepartment(DepartmentDTO department, CancellationToken cancellationToken)
        {
            try
            {
                await _departmentService.SaveDepartmentAsync(department, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = department.DepartmentId == Guid.Empty
                        ? "Department created successfully."
                        : "Department updated successfully."
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

                return BadRequest(new { 
                    success = false,
                    messge = "The request could not be processed due to invalid input. Please verify the submitted data and try again.",
                    errors = modelErrors }
                );
            }
        }
    }
}
