using TeamChallenge.Models.Models.Entities;

namespace TeamChallenge.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<CategoryCosmetic> Cosmetic { get; set; }
    }
}