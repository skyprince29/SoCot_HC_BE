using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("GetPerson/{id}")]
        public async Task<IActionResult> GetPerson(Guid id, CancellationToken cancellationToken)
        {
            var person = await _personService.GetByIdAsync(id, cancellationToken);
            if (person == null)
            {
                return NotFound(new { success = false, message = "Person not found." });
            }

            return Ok(person);
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


        [HttpPost("SavePerson")]
        public async Task<IActionResult> SavePerson(PersonDto person, CancellationToken cancellationToken)
        {
            try
            {
                await _personService.SavePersonAsync(person, cancellationToken);
                return Ok(new
                {
                    success = true,
                    message = person.PersonId == Guid.Empty
                       ? "Person created successfully."
                       : "Person updated successfully."
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

        [HttpPost("CheckDuplicate")]
        public async Task<IActionResult> CheckDuplicate([FromBody] Person person, CancellationToken cancellationToken)
        {
            bool exists = await _personService.CheckIfPersonExistsAsync(person.Firstname, person.Lastname, person.BirthDate, cancellationToken);
            return Ok(new { exists });
        }

    }

}
