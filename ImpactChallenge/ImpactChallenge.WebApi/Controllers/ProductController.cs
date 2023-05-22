using ImpactChallenge.WebApi.Dtos;
using ImpactChallenge.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImpactChallenge.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductServices _productServices;

        public ProductController(
            ILogger<ProductController> logger,
             IProductServices productServices
            )
        {
            _logger = logger;
            _productServices = productServices;
        }

        [HttpGet(Name = "TopRankedProducts")]
        public async Task<IActionResult> TopRankedProducts([FromQuery] string token)
        {
            List<Product> topRankedProducts = await _productServices.GetTopRankedProducts(token);
            return Ok(topRankedProducts);
        }

        //TODO: create another controller for authentication purposes
        [HttpGet(Name = "Login")]
        public async Task<IActionResult> Authentication([FromQuery] string email)
        {
            string token = await _productServices.Login(email);
            return Ok(token);
        }
    }
}