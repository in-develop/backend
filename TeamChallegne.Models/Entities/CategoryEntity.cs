namespace TeamChallenge.Models.Entities
{
    public class CategoryEntity : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
    }
}