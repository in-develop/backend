using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.OrderResponses
{
    public class GetOrderItemResponseModel
    {
        public GetOrderItemResponseModel(OrderItemEntity orderItem)
        {
            Id = orderItem.Id;
            ProductId = orderItem.ProductId;
            ProductName = orderItem.Product.Name;
            Quantity = orderItem.Quantity;
            TotalPrice = orderItem.Product.Price * orderItem.Quantity;
            TotalPriceWithDiscount = (orderItem.Product.DiscountPrice ?? orderItem.Product.Price) * orderItem.Quantity;

        }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? TotalPriceWithDiscount { get; set; }
    }
}
