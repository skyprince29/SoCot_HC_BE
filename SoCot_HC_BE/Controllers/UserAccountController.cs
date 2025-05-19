using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;

        public UserAccountController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [HttpPost("SaveUserAccount")]
        public async Task<IActionResult> SaveUserAccount(UserAccountDTO userAccountDTO, CancellationToken cancellationToken)
        {
            try
            {
                await _userAccountService.SaveUserAcccountAsync(userAccountDTO, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = userAccountDTO.UserAccountId == Guid.Empty
                        ? "User account created successfully."
                        : "User account updated successfully."
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

                return BadRequest(new
                {
                    success = false,
                    messge = "The request could not be processed due to invalid input. Please verify the submitted data and try again.",
                    errors = modelErrors
                }
                );
            }
        }

        [HttpGet("GetPagedUserAccount")]
        public async Task<IActionResult> GetPagedUserAccount(int pageNo, int statusId, int facility, int userGroupId, int limit, CancellationToken cancellationToken, string keyword = "")
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var paginatedResult = await _userAccountService.GetAllWithPagingAsync(pageNo, statusId, facility, userGroupId, limit, keyword, cancellationToken);
            return Ok(paginatedResult);
        }

        [HttpGet("GetUserAccountById/{id}")]
        public async Task<IActionResult> GetUserAccountById(Guid id, CancellationToken cancellationToken)
        {
            var userAccount = await _userAccountService.GetAsync(id, cancellationToken);
            if (userAccount == null)
            {
                return NotFound(new { success = false, message = "User account not found." });
            }
            return Ok(userAccount);
        }
    }
}
