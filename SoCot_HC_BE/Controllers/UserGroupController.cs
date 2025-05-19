using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserGroupController : Controller
    {
        private readonly IUserGroupService _userGroupService;

        public UserGroupController(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetActiveUserGroups")]
        public async Task<IActionResult> GetDepartmentType(CancellationToken cancellationToken)
        {
            var serviceClassification = await _userGroupService.GetAllActiveOnlyAsync(cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Department Type not found." });
            }

            return Ok(serviceClassification);
        }


    }
}
