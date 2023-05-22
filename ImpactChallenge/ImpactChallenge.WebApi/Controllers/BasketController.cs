using ImpactChallenge.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImpactChallenge.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IProductService _productServices;

        //TODO: tried to apply the additional endpoints first, left no time for the own Basket API.

        //TODO: implement endpoint for add product to basket
        //TODO: implement endpoint remove product to basket
        //TODO: implement endpoint(s) for increase decrease product in basket

    }
}