using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }


        [HttpGet("GetPagedPersons")]
        public async Task<IActionResult> GetPagedPersons(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            try
            {
                if (pageNo <= 0 || limit <= 0)
                {
                    return BadRequest(new { message = "Page number and limit must be greater than zero." });
                }

                var dtoList = await _personService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
                var totalRecords = await _personService.CountAsync(keyword, cancellationToken);

                var paginatedResult = new PaginationHandler<PersonDto>(dtoList, totalRecords, pageNo, limit);
                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal error", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }

}
