using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.DTOs.Product
{
    public class ProductCreateDto : BaseDTO
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        
        public string? Description { get; set; }
        
        [Required, Column(TypeName = "decimal(10, 2)")]
        public required decimal Price { get; set; }
        public required int CategoryId { get; set; }
    }
}