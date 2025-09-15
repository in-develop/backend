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
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
<<<<<<< HEAD

=======
        public decimal? DiscountPrice { get; set; }
>>>>>>> master
        [Required]
        public List<int> SubCategories { get; set; } = new List<int>();
    }
}