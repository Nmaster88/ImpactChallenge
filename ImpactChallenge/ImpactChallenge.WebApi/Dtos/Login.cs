using System.Text.Json.Serialization;

namespace ImpactChallenge.WebApi.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class Login
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
