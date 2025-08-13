using TeamChallenge.Models.Models;

namespace TeamChallenge.Models.DTOs
{
    public class CosmeticAddDto
    {
        public required string Name { get; set; }

        public string Description { get; set; }

        public required decimal Price { get; set; }

        public required List<Category> Category { get; set; }
    }
}
