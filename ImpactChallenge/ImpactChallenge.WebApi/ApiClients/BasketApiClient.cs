using ImpactChallenge.WebApi.Dtos;
using ImpactChallenge.WebApi.Utils;
using Polly;
using System;
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

            _httpClient = _httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Method to get token from the BasketApi
        /// </summary>
        /// <param name="email">A string parameter that represents an email</param>
        public async Task<string> GetTokenAsync(string email)
        {
            var url = _configurationHelper.BasketApiUrl + "/api/login";
            var requestBody = "{\"email\": \"{" + email + "\"}";

            CheckIfUrlIsValid(url);

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
            CheckIfTokenIsValid(token);

            var url = _configurationHelper.BasketApiUrl + "/api/GetAllProducts";

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            CheckIfUrlIsValid(url);

            var circuitBreakerPolicy = Policy
                .Handle<HttpRequestException>()
                .CircuitBreaker(3, TimeSpan.FromSeconds(30));

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
                _logger.LogWarning($"Request failed with status code {response.StatusCode}");
                return default;
            }
        }

        private void CheckIfTokenIsValid(string token)
        {             
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }
        }

        private void CheckIfUrlIsValid(string url)
        {
            var errorMessage = "";
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

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
        }

    }
}
