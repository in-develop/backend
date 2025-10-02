
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses.ProductResponses
{
    public class ProductSummaryResponseModel
    {
        public ProductSummaryResponseModel(ProductEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Volume = entity.Volume;
            Price = entity.Price;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Volume { get; set; }
        public decimal Price { get; set; }

        // public string MainImageUrl { get; set; } 
    }
}
