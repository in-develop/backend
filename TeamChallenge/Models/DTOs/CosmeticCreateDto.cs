using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.DTOs
{
    public class CosmeticCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0.01, 100)]
        public decimal Price { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
