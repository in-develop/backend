using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Entities
{
    public class CategoryEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public ICollection<SubCategoryEntity> SubCategories { get; set; } = new List<SubCategoryEntity>();
    }
}