using ImpactChallenge.WebApi.Filters;
using ImpactChallenge.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImpactChallenge.WebApi.Controllers
{
    //[ExceptionFilter]
    //[ExecutionTimeFilter]
    //[ServiceFilter(typeof(ExecutionTimeFilter))]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        OrderController(ILogger<OrderController> logger, IOrderService orderService, IProductService productService)
        {
            _logger = logger;
            _orderService = orderService;
            _productService = productService;
        }

        //        Use our Code Challenge API to build your own Basket API.Your Basket API should support the regular operations of a Web Cart, e.g., add product to
        //        basket, remove product from basket, decrease and increase product quantity.Note that the mentioned operations do not correspond directly to endpoints.
        //It is up to you to create the Basket API to support the operations according to your own design decisions.



        [HttpPost("create-basket")]
        public IActionResult CreateBasket([FromQuery] string email)
        {
            var basketId = _orderService.CreateBasket(email);
            return Ok(basketId);
        }

        [HttpPost("{basketId}/product/{productId}")]
        public IActionResult AddProductToBasket(Guid basketId, int productId)
        {
            var basket = _orderService.AddProductToBasket(basketId, productId);
            return Ok(basket);
        }


        [HttpDelete("{basketId}/product/{productId}")]
        public IActionResult RemoveProductFromBasket(Guid basketId, int productId)
        {
            var basket = _orderService.RemoveProductFromBasket(basketId, productId);
            return Ok(basket);
        }

        [HttpPut("{basketId}/product/{productId}/decrease")]
        public IActionResult DecreaseProductQuantityInBasket(Guid basketId, int productId)
        {
            var basket = _orderService.DecreaseProductQuantityInBasket(basketId, productId);
            return Ok(basket);
        }

        [HttpPut("{basketId}/product/{productId}/increase")]
        public IActionResult IncreaseProductQuantityInBasket(Guid basketId, int productId)
        {
            var basket = _orderService.IncreaseProductQuantityInBasket(basketId, productId);
            return Ok(basket);
        }
    }
}