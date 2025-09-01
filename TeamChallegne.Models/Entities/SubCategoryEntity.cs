using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class SubCategoryEntity : BaseEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        [ForeignKey("Category")]
        public required int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
        
    }
}
