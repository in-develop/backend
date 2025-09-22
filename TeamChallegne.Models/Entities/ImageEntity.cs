using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class ImageEntity : BaseEntity
    {
        [Required]
        public string? ImageUrl { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
