using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.Product
{
    public class UpdateProductRequest
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? DiscountPrice { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        public string? Volume { get; set; }

        [Required, MinLength(1, ErrorMessage = "Product must be assigned to at least one subcategory.")]
        public List<int> SubCategoryIds { get; set; } = new List<int>();
    }
}
