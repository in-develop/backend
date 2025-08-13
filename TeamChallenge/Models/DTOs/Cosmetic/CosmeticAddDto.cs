using TeamChallenge.Models.Models;

namespace TeamChallenge.Models.DTOs.Cosmetic
{
    public class CosmeticAddDto
    {
        public required string Name { get; set; }

        public string Description { get; set; }

        public required decimal Price { get; set; }

        public required List<Models.Category> Category { get; set; }
    }
}
