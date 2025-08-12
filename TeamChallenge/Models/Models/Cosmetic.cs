namespace TeamChallenge.Models.Models
{
    public class Cosmetic
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }
        public required decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public required List<Category> Categories { get; set; }
    }
}
