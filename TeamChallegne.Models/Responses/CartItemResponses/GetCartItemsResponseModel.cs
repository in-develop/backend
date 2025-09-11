namespace TeamChallenge.Models.Responses
{
    public class GetCartItemsResponseModel
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
