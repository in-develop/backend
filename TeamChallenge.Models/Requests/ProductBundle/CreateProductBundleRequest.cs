using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class CreateProductBundleRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public string? Description { get; set; }
        public int StockQuantity { get; set; }

        [Required]
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        [Required, Length(2, 10)]
        public List<int> ProductIds { get; set; }
    }
}