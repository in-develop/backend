using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetProductResponseModel
    {
        public GetProductResponseModel(ProductEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            Price = entity.Price;
            DiscountPrice = entity.DiscountPrice ?? 0;
            StockQuantity = entity.StockQuantity;
            CategoryId = entity.CategoryId;
            ProductBundleId = entity.ProductBundleId ?? 0;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public int ProductBundleId { get; set; }
    }
}