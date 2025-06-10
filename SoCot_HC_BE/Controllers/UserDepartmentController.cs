using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace SoCot_HC_BE.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserDepartmentController : Controller
    {

        private readonly IUserDepartmentService _userDepartmentService;
        private readonly IPersonService _personService;
        private readonly IDepartmentService _departmentService;

        public UserDepartmentController(IUserDepartmentService userDepartmentService, IPersonService personService, IDepartmentService departmentService)
        {
            _userDepartmentService = userDepartmentService;
            _personService = personService;
            _departmentService = departmentService;
        }


        [HttpGet("GetAllPersonWithUserAccount")]
        public async Task<IActionResult> GetAllPersonWithUserAccount(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var paginatedResult = await _personService.GetAllPersonWithUserAccount(pageNo, limit, keyword, cancellationToken);

            return Ok(paginatedResult);
        }

        [HttpGet("GetAllWithPagingAsync")]
        public async Task<IActionResult> GetAllWithPagingAsync(Guid personId, int pageNo, int limit, string? keyword, Boolean? isActive = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (pageNo <= 0 || limit <= 0)
                {
                    return BadRequest(new { message = "Page number and limit must be greater than zero." });
                }

                var paginatedResult = await _userDepartmentService.GetAllWithPagingAsync(personId, pageNo, limit, keyword, isActive, cancellationToken);
                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal error", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("GetDepartmentsExcludedAsync")]
        public async Task<IActionResult> GetDepartmentsExcludedAsync(Guid personId, int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var paginatedResult = await _departmentService.GetDepartmentsExcludedAsync(personId, pageNo, limit, keyword, cancellationToken);
            return Ok(paginatedResult);
        }

        [HttpPost("SaveUserDepartmentAsync")]
        public async Task<IActionResult> SaveUserDepartmentAsync(UserDeptModelDto userDeptModelDto, CancellationToken cancellationToken)
        {
            try
            {
                await _userDepartmentService.SaveUserDepartmentAsync(userDeptModelDto, cancellationToken);
                return Ok(new
                {
                    success = true,
                    message =  "User Department added successfully."
                });
            }
            catch (Exception ex) {
                return BadRequest(new { success = false, errors = ex.Message });
            }
        }
        [HttpPost("DeactivateOrActivateUserDepartmentAsync")]
        public async Task<IActionResult> DeactivateOrActivateUserDepartmentAsync(UserDeptModelDto userDeptModelDto, CancellationToken cancellationToken)
        {
            try
            {
                await _userDepartmentService.DeactivateOrActivateUserDepartmentAsync(userDeptModelDto, cancellationToken);
                return Ok(new
                {
                    success = true,
                    message = "User Department activated/deactivated successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, errors = ex.Message });
            }
        }

        [HttpGet("GetDepartmentsByUser")]
        public async Task<IActionResult> GetDepartmentsByUser(Guid personId, CancellationToken cancellationToken)
        {
            try
            {
                var departmentList = await _userDepartmentService.GetDepartmentsByUser(personId, cancellationToken);
                return Ok(departmentList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal error", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("GetAllWithPagingUserOnUserDepartmentAsync")]
        public async Task<IActionResult> GetAllWithPagingUserOnUserDepartmentAsync(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var paginatedResult = await _userDepartmentService.GetAllWithPagingUserOnUserDepartmentAsync(pageNo, limit, keyword, cancellationToken);
            return Ok(paginatedResult);
        }
    }
}
