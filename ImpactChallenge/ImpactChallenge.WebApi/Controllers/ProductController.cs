using ImpactChallenge.WebApi.Dtos;
using ImpactChallenge.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImpactChallenge.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productServices;

        public ProductController(
            ILogger<ProductController> logger,
             IProductService productServices
            )
        {
            _logger = logger;
            _productServices = productServices;
        }

        [HttpGet("top-ranked-products", Name = "top-ranked-products")]
        public async Task<IActionResult> TopRankedProducts([FromQuery] string token)
        {
            List<Product> topRankedProducts = await _productServices.GetTopRankedProducts(token);
            return Ok(topRankedProducts);
        }

        [HttpGet("ten-cheapest-products", Name = "ten-cheapest-products")]
        public async Task<IActionResult> TenCheapestProducts([FromQuery] string token)
        {
            List<Product> topRankedProducts = await _productServices.GetTenCheapestProducts(token);
            return Ok(topRankedProducts);
        }

        [HttpGet("get-basket", Name = "get-basket")]
        public async Task<IActionResult> GetBasket([FromQuery] string token, [FromQuery] string basketId)
        {
            List<Product> topRankedProducts = await _productServices.GetTenCheapestProducts(token);
            return Ok(topRankedProducts);
        }

        //TODO: create another controller for authentication purposes
        [HttpGet("login", Name = "login")]
        public async Task<IActionResult> Authentication([FromQuery] string email)
        {
            string token = await _productServices.Login(email);
            return Ok(token);
        }
    }
}