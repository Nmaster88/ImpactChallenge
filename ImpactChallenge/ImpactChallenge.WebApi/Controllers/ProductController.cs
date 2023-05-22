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
        private readonly IProductService _productServices;

        public ProductController(
            ILogger<ProductController> logger,
             IProductService productServices
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

        [HttpGet(Name = "TenCheapestProducts")]
        public async Task<IActionResult> TenCheapestProducts([FromQuery] string token)
        {
            List<Product> topRankedProducts = await _productServices.GetTenCheapestProducts(token);
            return Ok(topRankedProducts);
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<IActionResult> GetBasket([FromQuery] string token, [FromQuery] string basketId)
        {
            List<Product> topRankedProducts = await _productServices.GetTenCheapestProducts(token);
            return Ok(topRankedProducts);
        }

        //TODO: create another controller for authentication purposes
        [HttpGet(Name = "Login")]
        public async Task<IActionResult> Authentication([FromQuery] string email)
        {
            string token = await _productServices.Login(email);
            return Ok(token);
        }

        //TODO: endpoint to create order
        //TODO: endpoint to get order
    }
}