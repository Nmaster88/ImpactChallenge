using ImpactChallenge.WebApi.Dtos;
using ImpactChallenge.WebApi.repository;

namespace ImpactChallenge.WebApi.Services
{
    public interface IOrderService
    {
        Guid CreateBasket(string email);
        Task<Basket> AddProductToBasket(Guid basketId, int productId);
        Basket DecreaseProductQuantityInBasket(Guid basketId, int productId);
        Basket IncreaseProductQuantityInBasket(Guid basketId, int productId);
        Basket RemoveProductFromBasket(Guid basketId, int productId);
    }

    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IBasketRepo _basketRepo;
        private readonly IProductService _productService;

        public OrderService(
            ILogger<OrderService> logger,
            IBasketRepo basketRepo,
            IProductService productService
            )
        {
            _logger = logger ?? throw new ArgumentNullException();
            _basketRepo = basketRepo ?? throw new ArgumentNullException();
            _productService = productService ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Creates a basket for the user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Guid CreateBasket(string email)
        {
            Basket basket = new Basket();
            basket.BasketId = Guid.NewGuid();
            basket.UserEmail = email;
            basket.TotalAmount = 0;
            basket.OrderLines = new List<OrderLine>();

            _basketRepo.CreateBasket(basket);

            return basket.BasketId;
        }

        public async Task<Basket> AddProductToBasket(Guid basketId, int productId)
        {
            var basket = _basketRepo.GetBasket(basketId);
            var token = await _productService.Login(basket.UserEmail);
            var products = await _productService.GetTopRankedProducts(token);
            var product = GetProductFromList(products, productId);

            basket.OrderLines.Add(new OrderLine
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductUnitPrice = product.Price,
                ProductSize = product.Size.ToString(),
                Quantity = 1 //TODO: implement quantity
            });

            return basket;
        }

        private Product GetProductFromList(List<Product> products, int productId)
        {
            if (products == null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            var product = products.SingleOrDefault(p => p.Id == productId);

            if (product == null)
            {
                throw new Exception($"Product with id {productId} not found");
            }

            return product;
        }

        private OrderLine GetOrderLineByProductId(Basket basket, int productId)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }

            var orderLine = basket.OrderLines.SingleOrDefault(ol => ol.ProductId == productId);

            if (orderLine == null)
            {
                throw new Exception($"Product with id {productId} not found in basket");
            }

            return orderLine;
        }

        public Basket DecreaseProductQuantityInBasket(Guid basketId, int productId)
        {
            var basket = _basketRepo.GetBasket(basketId);
            var orderLine = GetOrderLineByProductId(basket, productId);

            if (orderLine == null)
            {
                throw new Exception($"Product with id {productId} not found in basket");
            }

            if (orderLine.Quantity == 1)
            {
                basket.OrderLines.Remove(orderLine);
            }
            else
            {
                orderLine.Quantity--;
            }

            return basket;
        }

        public Basket IncreaseProductQuantityInBasket(Guid basketId, int productId)
        {
            var basket = _basketRepo.GetBasket(basketId);
            var orderLine = GetOrderLineByProductId(basket, productId);

            if (orderLine == null)
            {
                throw new Exception($"Product with id {productId} not found in basket");
            }

            orderLine.Quantity++;

            return basket;
        }

        public Basket RemoveProductFromBasket(Guid basketId, int productId)
        {
            var basket = _basketRepo.GetBasket(basketId);
            var orderLine = GetOrderLineByProductId(basket, productId);

            if (orderLine == null)
            {
                throw new Exception($"Product with id {productId} not found in basket");
            }

            basket.OrderLines.Remove(orderLine);
            return basket;
        }
    }
}
