namespace ImpactChallenge.WebApi.Dtos
{
    /// <summary>
    /// Order of the user that contains it's orderlines
    /// </summary>
    public class Order
    {
        public string OrderId { get; set; }
        public string UserEmail { get; set; }
        public string TotalAmount { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
}
