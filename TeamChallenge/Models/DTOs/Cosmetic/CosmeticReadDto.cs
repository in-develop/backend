using TeamChallenge.Models.Models;

namespace TeamChallenge.Models.DTOs.Cosmetic
{
    public class CosmeticReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price  { get; set; }
        public List<Models.Category> Category { get; set; }
    }
}
