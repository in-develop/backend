using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.DTOs.Cosmetic
{
    public class CosmeticAddDto
    {
        public required string Name { get; set; }

        public string Description { get; set; }

        public required decimal Price { get; set; }

        public required List<Entities.Category> Category { get; set; }
    }
}
