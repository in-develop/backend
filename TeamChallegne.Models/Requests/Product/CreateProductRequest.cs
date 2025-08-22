using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class CreateProductRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        [Required]
        public required int CategoryId { get; set; }
    }
}