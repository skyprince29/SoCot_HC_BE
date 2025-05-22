using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _productService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Product not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<Product> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _productService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _productService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _productService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
}
