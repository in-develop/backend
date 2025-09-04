using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class ProductEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Required]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<ProductSubCategoryEntity> ProductSubCategories { get; set; } = new List<ProductSubCategoryEntity>();
        public ICollection<ReviewEntity> Reviews { get; set; }
    }
}