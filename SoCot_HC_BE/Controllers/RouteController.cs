using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;
using Route = SoCot_HC_BE.Model.Route;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RouteController : Controller
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetRoute/{id}")]
        public async Task<IActionResult> GetRoute(Guid id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _routeService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Route not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetRoutes")]
        public async Task<IActionResult> GetRoutes(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<Route> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _routeService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _routeService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _routeService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
}
