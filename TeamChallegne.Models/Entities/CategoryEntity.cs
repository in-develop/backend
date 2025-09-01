namespace TeamChallenge.Models.Entities
{
    public class CategoryEntity : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<SubCategoryEntity> SubCategories { get; set; } = new List<SubCategoryEntity>();
    }
}