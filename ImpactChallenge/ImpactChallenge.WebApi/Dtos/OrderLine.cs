using System.Text.Json.Serialization;

namespace ImpactChallenge.WebApi.Dtos
{
    public class OrderLine
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }
        [JsonPropertyName("productUnitPrice")]
        public int ProductUnitPrice { get; set; }
        [JsonPropertyName("productSize")]
        public string ProductSize { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("totalPrice")]
        public int TotalPrice { get; set; }
    }
}
