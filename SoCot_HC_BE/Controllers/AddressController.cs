using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IProvinceService _iproviceService;
        private readonly ICityMunicipalService _cityMunicipalService;
        private readonly IBarangayService _barangayService;
      
        public AddressController(IProvinceService proviceService, ICityMunicipalService cityMunicipalService, IBarangayService barangayService)
        {
            _iproviceService = proviceService;
            _cityMunicipalService = cityMunicipalService;
            _barangayService = barangayService;
        }
        [HttpGet("GetProvinces")]
        public async Task<IActionResult> GetProvinces()
        {
            try
            {
                return Ok(await _iproviceService.GetProvinces());
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while retrieving municipalities.");
            }
        }

        [HttpGet("GetMunicipalities")]
        public async Task<IActionResult> GetMunicipalities(int? ProvinceId)
        {
            try
            {
                return Ok(await _cityMunicipalService.GetMunicipalities(ProvinceId));
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while retrieving municipalities.");
            }
        } 

        [HttpGet("GetBarangays")]
        public async Task<IActionResult> GetBarangays(int? CityMunicipalId)
        {
            try
            {
                return Ok(await _barangayService.GetBarangays(CityMunicipalId));
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while retrieving barangay.");
            }
        }
    }
}
