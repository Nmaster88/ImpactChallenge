using ImpactChallenge.WebApi.Dtos;

namespace ImpactChallenge.WebApi.repository
{
    /// <summary>
    /// A temporary class just for testing purposes
    /// </summary>
    public class BasketRepo
    {
        private List<Basket> BasketList { get; set; } = new List<Basket>();

        public BasketRepo()
        {
        }

        public void CreateBasket(Basket basket)
        {
            BasketList.Add(basket);
        }

        public Basket GetBasket(Guid basketId)
        {
            return BasketList.FirstOrDefault(o => o.BasketId == basketId);
        }

        public void AddProductToBasket(Product product, int quantity, Guid basketId)
        {
            Basket basket = GetBasket(basketId);
            OrderLine line = new OrderLine();
            line.ProductId = product.Id;
            line.ProductName = product.Name;
            line.Quantity = quantity;
            line.ProductUnitPrice = product.Price;
            line.ProductSize = product.Size.ToString();
            line.TotalPrice = product.Price * quantity;
            basket.OrderLines.Add(line);
        }

        public void RemoveProductFromBasket(int productId, Guid basketId)
        {
            Basket basket = GetBasket(basketId);
            basket.OrderLines.RemoveAll(p => p.ProductId == productId);
        }

        public void DecreaseProductInBasket(int productId, int quantity, Guid basketId)
        {
            Basket basket = GetBasket(basketId);
            OrderLine orderLine = basket.OrderLines.FirstOrDefault(p => p.ProductId == productId);
            if (orderLine != null)
            {
                if (orderLine.Quantity < quantity)
                {
                    return; //TODO: exception?
                }
                orderLine.Quantity -= quantity;
            }

            return; //TODO: doesnt exist
        }

        public void IncreaseProductInBasket(int productId, int quantity, Guid basketId)
        {
            Basket basket = GetBasket(basketId);
            OrderLine orderLine = basket.OrderLines.FirstOrDefault(p => p.ProductId == productId);
            if (orderLine != null)
            {
                orderLine.Quantity += quantity;
            }

            return; //TODO: doesnt exist
        }
    }
}
