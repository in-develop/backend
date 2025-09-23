using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class CreateProductRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}