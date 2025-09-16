using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetProductBundleResponseModel
    {
        public GetProductBundleResponseModel(ProductBundleEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            Price = entity.Price;
            DiscountPrice = entity.DiscountPrice ?? 0;
            StockQuantity = entity.StockQuantity;
            Products = entity.Products?.Select(p => new GetProductResponseModel(p)).ToList() ?? 
                new List<GetProductResponseModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public List<GetProductResponseModel> Products { get; set; }
    }
}