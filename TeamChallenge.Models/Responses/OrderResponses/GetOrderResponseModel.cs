using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.OrderResponses
{
    public class GetOrderResponseModel
    {
        public GetOrderResponseModel(OrderEntity order)
        {
            Id = order.Id;
            OrderItems = order.OrderItems.Select(oi => new GetOrderItemResponseModel(oi)).ToList();
        }
        public int Id { get; set; }
        public List<GetOrderItemResponseModel> OrderItems { get; set; }
    }
}
