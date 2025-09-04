using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChallenge.Models.Entities
{
    public class SubCategoryEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [ForeignKey("Category")]
        [Required]
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
        public ICollection<ProductSubCategoryEntity> ProductSubCategories { get; set; } = new List<ProductSubCategoryEntity>();
    }
}
