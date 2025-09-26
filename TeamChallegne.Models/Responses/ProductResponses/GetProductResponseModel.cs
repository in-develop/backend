using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses.SubCategoryResponses;

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
            ProductBundleId = entity.ProductBundleId ?? 0;

            if (entity.ProductSubCategories != null)
            {
                SubCategories = entity.ProductSubCategories
                    .Select(psc => new SubCategoryResponseModel
                    {
                        Id = psc.SubCategory.Id,
                        Name = psc.SubCategory.Name
                    }).ToList();
            }

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public int ProductBundleId { get; set; }
        public List<SubCategoryResponseModel> SubCategories { get; set; } = new List<SubCategoryResponseModel>();
    }
}