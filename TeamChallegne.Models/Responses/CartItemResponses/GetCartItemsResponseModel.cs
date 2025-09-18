using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetCartItemsResponseModel
    {
        public GetCartItemsResponseModel(CartItemEntity entity)
        {
            Id = entity.Id;
            CartId = entity.CartId;
            ProductId = entity.ProductId;
            ProductName = entity.Product?.Name;
            Price = entity.Product?.Price ?? 0;
            Quantity = entity.Quantity;
        }

        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
