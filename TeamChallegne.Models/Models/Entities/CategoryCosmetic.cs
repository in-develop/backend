using System.ComponentModel.DataAnnotations.Schema;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Models.Entities
{
    public class CategoryCosmetic
    {
        public int Id { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("Cosmetic")]
        public int CosmeticId { get; set; }

        public Category Category { get; set; }

        public Cosmetic Cosmetic { get; set; }
    }
}
