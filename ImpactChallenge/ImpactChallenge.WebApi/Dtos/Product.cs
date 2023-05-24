using System.Text.Json.Serialization;

namespace ImpactChallenge.WebApi.Dtos
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public int Price { get; set; }
        [JsonPropertyName("size")]
        public int Size { get; set; }
        [JsonPropertyName("stars")]
        public int Stars { get; set; }
    }
}
