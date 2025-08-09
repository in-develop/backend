namespace TeamChallenge.Models.Models
{
    public class Сosmetiс
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<Category> Category { get; set; }
    }
}
