using TeamChallenge.Models.Models;

namespace TeamChallenge.Models
{
    public class AddCosmeticDto
    {
        public required string Name { get; set; }
        public string Description { get; set; }
        public required decimal Price { get; set; }
        public required List<Category> Category { get; set; }
    }
}
