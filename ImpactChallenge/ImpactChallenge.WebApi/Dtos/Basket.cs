namespace ImpactChallenge.WebApi.Dtos
{
    /// <summary>
    /// Basket is a temporary dto used when the user is building it's basket
    /// </summary>
    public class Basket
    {
        public Guid BasketId { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
}
