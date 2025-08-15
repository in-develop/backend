using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class ProductEntity : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public required decimal Price { get; set; }
        public int StockQuantity { get; set; }
        [ForeignKey("Category")]
        public required int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
    }
}