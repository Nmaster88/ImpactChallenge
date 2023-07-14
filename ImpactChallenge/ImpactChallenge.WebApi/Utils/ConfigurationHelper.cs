namespace ImpactChallenge.WebApi.Utils
{
    public interface IConfigurationHelper
    {
        string BasketApiUrl { get; }
        int CircuitBreakerExceptionsLimit { get; }
        int CircuitBreakerDurationOfBreak { get; }
    }

    public class ConfigurationHelper : IConfigurationHelper
    {
        public ConfigurationHelper(IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            BasketApiUrl = configuration.GetSection("AppSettings").GetValue<string>("BasketApiUrl");
            CircuitBreakerExceptionsLimit = configuration.GetSection("AppSettings").GetValue<int>("CircuitBreaker.ExceptionsLimit");
            CircuitBreakerDurationOfBreak = configuration.GetSection("AppSettings").GetValue<int>("CircuitBreaker.DurationOfBreak");
        }

        public string BasketApiUrl { get; }
        public int CircuitBreakerExceptionsLimit { get; }
        public int CircuitBreakerDurationOfBreak { get; }
    }
}
