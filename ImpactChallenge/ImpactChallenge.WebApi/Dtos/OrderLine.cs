namespace ImpactChallenge.WebApi.Dtos
{
    public class OrderLine
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductUnitPrice { get; set; }
        public string ProductSize { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
