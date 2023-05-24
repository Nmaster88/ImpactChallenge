using System.Text.Json.Serialization;

namespace ImpactChallenge.WebApi.Dtos
{
    /// <summary>
    /// Order of the user that contains it's orderlines
    /// </summary>
    public class Order
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }
        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }
        [JsonPropertyName("totalAmount")]
        public string TotalAmount { get; set; }
        [JsonPropertyName("orderLines")]
        public List<OrderLine> OrderLines { get; set; }
    }
}
