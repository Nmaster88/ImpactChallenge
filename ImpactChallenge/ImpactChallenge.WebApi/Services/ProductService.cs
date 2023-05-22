using ImpactChallenge.WebApi.ApiClients;
using ImpactChallenge.WebApi.Dtos;

namespace ImpactChallenge.WebApi.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetTopRankedProducts(string token);
        Task<List<Product>> GetTenCheapestProducts(string token);
        Task<List<Product>> GetPaginatedProducts(string token, int limit);
        //TODO: login should go into its own controller
        Task<string> Login(string email);
    }

    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IBasketApiClient _apiClient;

        public ProductService(
            IBasketApiClient apiClient,
            ILogger<ProductService> logger
            )
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _logger = logger ?? throw new ArgumentNullException();
        }

        public async Task<List<Product>> GetTopRankedProducts(string token)
        {
            //TODO: top ranked limit/take can be a configurable variable
            List<Product> topRankedProducts = new List<Product>();
            List<Product> productsList = await _apiClient.GetAllProductsAsync(token);

            if (productsList != null)
            {
                topRankedProducts = productsList.OrderByDescending(p => p.Stars).Skip(0).Take(100).ToList();
            }

            return topRankedProducts;
        }

        public async Task<List<Product>> GetTenCheapestProducts(string token)
        {
            //TODO: top ranked limit/take can be a configurable variable
            List<Product> tenCheapestProducts = new List<Product>();
            List<Product> productsList = await _apiClient.GetAllProductsAsync(token);

            if (productsList != null)
            {
                tenCheapestProducts = productsList.OrderBy(p => p.Price).Skip(0).Take(10).ToList();
            }

            return tenCheapestProducts;
        }

        public async Task<List<Product>> GetPaginatedProducts(string token, int limit)
        {
            string errorMessage = "";
            if (limit < 0)
            {
                errorMessage = $"The limit ({limit}) should not be negative.";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            if (limit > 1000) //TODO: have the limit configurable
            {
                errorMessage = $"The limit ({limit}) cannot be over {1000}";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            //TODO: top ranked limit/take can be a configurable variable
            List<Product> topRankedProducts = new List<Product>();
            List<Product> productsList = await _apiClient.GetAllProductsAsync(token);

            if (productsList != null)
            {
                topRankedProducts = productsList.OrderBy(p => p.Price).Skip(0).Take(limit).ToList();
            }

            return topRankedProducts;
        }

        //public async Task<List<Product>> GetBasket(string token, Guid basketId)
        //{

        //    List<Product> productsList = await _apiClient.GetAllProductsAsync(token);

        //    if (productsList != null)
        //    {
        //        topRankedProducts = productsList.OrderBy(p => p.Price).Skip(0).Take(limit).ToList();
        //    }

        //    return topRankedProducts;
        //}

        public async Task<string> Login(string email)
        {
            //TODO: add email regex and validations, should go into another service/controller

            return await _apiClient.GetTokenAsync(email);
        }
    }
}
