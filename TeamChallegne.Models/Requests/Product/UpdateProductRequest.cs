using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class UpdateProductRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
