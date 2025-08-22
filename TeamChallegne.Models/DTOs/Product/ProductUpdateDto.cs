using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.DTOs.Product
{
    public class ProductUpdateDto : BaseDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
