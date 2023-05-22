//using Polly;
//using System.Text.Json;

//namespace ImpactChallenge.WebApi.ApiClients
//{
//    //TODO: put this on a common project?
//    public interface IApiClient<TResponse>
//    {
//        /// <summary>
//        /// Generic method to make a get http request to the specific url
//        /// </summary>
//        /// <param name="url">A string parameter that represents an url</param>
//        /// <returns>An instance of <see cref="TResponse"/> generic object</returns>
//        Task<TResponse> GetAsync(string url);
//    }

//    public class ApiClient<TResponse> : IApiClient<TResponse>, IDisposable
//    {
//        private readonly IHttpClientFactory _httpClientFactory;

//        private readonly ILogger<ApiClient<TResponse>> _logger;
//        private readonly HttpClient _httpClient;

//        public ApiClient(IHttpClientFactory httpClientFactory, ILogger<ApiClient<TResponse>> logger)
//        {
//            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

//            _httpClient = _httpClientFactory.CreateClient(); //TODO: create an httpClient each time the GetAsync method is called?
//        }

//        /// <summary>
//        /// Generic method to make a get http request to the specific url
//        /// </summary>
//        /// <param name="url">A string parameter that represents an url</param>
//        /// <returns>An instance of <see cref="TResponse"/> generic object</returns>
//        public async Task<TResponse> GetAsync(string url)
//        {
//            var errorMessage = "";
//            #region argument validations
//            Uri uriResult;
//            if (!Uri.TryCreate(url, UriKind.Absolute, out uriResult))
//            {
//                errorMessage = $"The provided url ({url}) is not a valid Uri";
//                _logger.LogError(errorMessage);
//                throw new ArgumentException(errorMessage);
//            }

//            if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
//            {
//                errorMessage = $"The provided uri scheme is not http or https";
//                _logger.LogError(errorMessage);
//                throw new ArgumentException(errorMessage);
//            }
//            #endregion

//            var circuitBreakerPolicy = Policy
//                .Handle<HttpRequestException>()
//                .CircuitBreaker(3, TimeSpan.FromSeconds(30));

//            //TODO: test if the Execute works
//            var response = await circuitBreakerPolicy.Execute(async () =>
//            {
//                return await _httpClient.GetAsync(url);
//            });
//            //var response = await _httpClient.GetAsync(url);

//            if (response.IsSuccessStatusCode)
//            {
//                var responseBodyDeserialized = await JsonSerializer.DeserializeAsync<TResponse>(response.Content.ReadAsStream());

//                _logger.LogInformation($"Request success with status code {response.StatusCode}");
//                return responseBodyDeserialized;
//            }
//            else
//            {
//                //TODO: throw error? exception?
//                _logger.LogWarning($"Request failed with status code {response.StatusCode}");
//                return default;
//            }
//        }

//        //TODO: check is this required?
//        public void Dispose()
//        {
//            _httpClient.Dispose();
//        }
//    }
//}
