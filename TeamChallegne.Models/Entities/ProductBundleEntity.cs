using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class ProductBundleEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10, 2)"), Required]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<ProductEntity> Products { get; set; }

    }
}