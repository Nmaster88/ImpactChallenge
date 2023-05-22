namespace ImpactChallenge.WebApi.Services
{
    public interface IBasketService
    {

    }

    public class BasketService : IBasketService
    {
        private readonly ILogger<BasketService> _logger;

        public BasketService(
            ILogger<BasketService> logger
            )
        {
            _logger = logger ?? throw new ArgumentNullException();
        }


    }
}
