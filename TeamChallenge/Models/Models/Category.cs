namespace TeamChallenge.Models.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Cosmetic> Cosmetic { get; set; }
    }
}