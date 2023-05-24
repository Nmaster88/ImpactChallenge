using ImpactChallenge.WebApi.Dtos;
using ImpactChallenge.WebApi.Utils;
using Polly;
using System.Text;
using System.Text.Json;

namespace ImpactChallenge.WebApi.ApiClients
{
    public interface IBasketApiClient
    {
        Task<string> GetTokenAsync(string email);
        Task<List<Product>> GetAllProductsAsync(string token);
    }

    public class BasketApiClient : IBasketApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfigurationHelper _configurationHelper;
        private readonly ILogger<BasketApiClient> _logger;
        private readonly HttpClient _httpClient;

        public BasketApiClient(
            IHttpClientFactory httpClientFactory,
            IConfigurationHelper configurationHelper,
            ILogger<BasketApiClient> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configurationHelper = configurationHelper ?? throw new ArgumentNullException(nameof(configurationHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _httpClient = _httpClientFactory.CreateClient(); //TODO: create an httpClient each time the GetAsync method is called?
        }

        /// <summary>
        /// Method to get token from the BasketApi
        /// </summary>
        /// <param name="email">A string parameter that represents an email</param>
        public async Task<string> GetTokenAsync(string email)
        {
            var errorMessage = "";
            var url = _configurationHelper.BasketApiUrl + "/api/login";
            var requestBody = "{\"email\": \"{" + email + "\"}";
            #region argument validations
            //TODO: add validations for email

            Uri uriResult;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uriResult))
            {
                errorMessage = $"The provided url ({url}) is not a valid Uri";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            {
                errorMessage = $"The provided uri scheme is not http or https";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            #endregion

            var circuitBreakerPolicy = Policy
                .Handle<HttpRequestException>()
                .CircuitBreaker(3, TimeSpan.FromSeconds(30));

            HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            //TODO: test if the Execute works
            var response = await circuitBreakerPolicy.Execute(async () =>
            {

                return await _httpClient.PostAsync(url, content);
            });

            if (response.IsSuccessStatusCode)
            {
                var responseBodyDeserialized = await JsonSerializer.DeserializeAsync<Login>(response.Content.ReadAsStream());

                _logger.LogInformation($"Request success with status code {response.StatusCode}");
                return responseBodyDeserialized.Token;
            }
            else
            {
                //TODO: throw error? exception?
                _logger.LogWarning($"Request failed with status code {response.StatusCode}");
                return default;
            }
        }

        public async Task<List<Product>> GetAllProductsAsync(string token)
        {
            var errorMessage = "";
            var url = _configurationHelper.BasketApiUrl + "/api/GetAllProducts";

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            #region argument validations
            //TODO: add validations for email

            Uri uriResult;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uriResult))
            {
                errorMessage = $"The provided url ({url}) is not a valid Uri";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            {
                errorMessage = $"The provided uri scheme is not http or https";
                _logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            #endregion

            var circuitBreakerPolicy = Policy
                .Handle<HttpRequestException>()
                .CircuitBreaker(3, TimeSpan.FromSeconds(30));

            //TODO: test if the Execute works
            var response = await circuitBreakerPolicy.Execute(
                async () =>
                {
                    return await _httpClient.GetAsync(url);
                }
            );

            if (response.IsSuccessStatusCode)
            {
                var responseBodyDeserialized = await JsonSerializer.DeserializeAsync<List<Product>>(response.Content.ReadAsStream());

                _logger.LogInformation($"Request success with status code {response.StatusCode}");
                return responseBodyDeserialized;
            }
            else
            {
                //TODO: throw error? exception?
                _logger.LogWarning($"Request failed with status code {response.StatusCode}");
                return default;
            }
        }

        private void CheckIfUrlIsValid()
        {

        }

    }
}
