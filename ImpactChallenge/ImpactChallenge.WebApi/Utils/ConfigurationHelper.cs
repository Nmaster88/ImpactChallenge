namespace ImpactChallenge.WebApi.Utils
{
    public interface IConfigurationHelper
    {
        string BasketApiUrl { get; }
    }

    public class ConfigurationHelper : IConfigurationHelper
    {
        public ConfigurationHelper(IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            BasketApiUrl = configuration.GetSection("AppSettings").GetValue<string>("BasketApiUrl");
        }

        public string BasketApiUrl { get; }
    }
}
