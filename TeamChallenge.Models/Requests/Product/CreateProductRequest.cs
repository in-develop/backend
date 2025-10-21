using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.Product
{
    public class CreateProductRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        [Required, Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }
        
        [Required, Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? Volume { get; set; }
        [Required, MinLength(1, ErrorMessage = "Product must be assigned to at least one subcategory.")]
        public List<int> SubCategoryIds { get; set; } = new List<int>();
    }
}